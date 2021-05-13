using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Accord.Imaging.Filters;
using Accord.IO;
using Accord.Neuro;

namespace ImageRecognition
{
    public partial class MainForm : Form
    {
        private const int ImageWidth = 28;
        private const int ImageHeight = 28;
        private const int BlackColourCode = 255;
        private const int SymbolsCount = 10;

        private ActivationNetwork _network;

        /// <summary>
        /// A list of the progress bars
        /// </summary>
        private List<ProgressBar> _bars;

        public MainForm()
        {
            InitializeComponent();
        }

        private void handWrittenPictureBox_Click(object sender, EventArgs e)
        {

        }

        private Point? _previous;

        private void handWrittenPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _previous = null;
        }

        private void handWrittenPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _previous = e.Location;
            handWrittenPictureBox_MouseMove(sender, e);
        }

        private void handWrittenPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_previous == null) return;

            if (handWrittenPictureBox.Image == null)
            {
                Bitmap bmp = new Bitmap(handWrittenPictureBox.Width, handWrittenPictureBox.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                }
                handWrittenPictureBox.Image = bmp;
            }

            using (Graphics g = Graphics.FromImage(handWrittenPictureBox.Image))
            {
                g.DrawLine(new Pen(Color.Black, 5),  _previous.Value, e.Location);
                g.SmoothingMode = SmoothingMode.HighQuality;
            }

            handWrittenPictureBox.Invalidate();
            _previous = e.Location;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // build the progress bars
            _bars = new List<ProgressBar>(SymbolsCount);

            for (int i = 0; i < SymbolsCount; i++)
            {
                // add label
                Label label = new Label
                {
                    Text = i.ToString(),
                    Top = 20 + i * 25,
                    Left = 300,
                    Width = 25
                };
                Controls.Add(label);

                // add progress bar
                ProgressBar progressBar = new ProgressBar()
                {
                    Top = 20 + i * 25,
                    Left = 350,
                    Width = 170
                };
                Controls.Add(progressBar);
                _bars.Add(progressBar);

                // load the neural network
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
                path = Path.Combine(path, "..");
                string fileName = Path.Combine(path, "trained_network.state");

                Serializer.Load(fileName, out _network);
            }
        }

        /// <summary>
        /// Predict a hand written digit from the pictureBox
        /// </summary>
        private void PredictDigit()
        {
            // получаем нарисованную мышью цифру
            Bitmap bitmap = new Bitmap(handWrittenPictureBox.Image);

            // приводим изображение к размеру 28x28
            ResizeBilinear resizer = new ResizeBilinear(ImageWidth, ImageHeight);
            Bitmap img = resizer.Apply(bitmap);

            // Раскладываем изображение в пиксели
            IEnumerable<byte> pixels = Enumerable.Range(0, ImageWidth)
                .SelectMany(y => Enumerable.Range(0, ImageHeight), (y, x) => img.GetPixel(x, y).B);

            // нормализуем пиксели
            double[] input = pixels
                .Select(p => new { p, v = 1.0 * (BlackColourCode - p) / BlackColourCode })
                .Select(t => t.v > 0.1 ? t.v : 0)
                .ToArray();

            // запускаем вычисление нейросетью вектора с вероятностями
            double[] predictions = _network.Compute(input.ToArray());
            
            // показываем результат в элементах ProgressBar
            for (int i = 0; i < SymbolsCount; i++)
            {
                _bars[i].Value = (int) (100 * predictions[i]);
            }

            Refresh();
        }

        private void recognizeDigitButton_Click(object sender, EventArgs e)
        {
            PredictDigit();
        }

        private void ClearPictureBoxButton_Click(object sender, EventArgs e)
        {
            handWrittenPictureBox.Image = null;
            _previous = null;

            for (int i = 0; i < SymbolsCount; i++)
            {
                _bars[i].Value = 0;
            }

            Refresh();
        }
    }
}