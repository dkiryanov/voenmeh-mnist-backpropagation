using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Controls;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Statistics.Visualizations;
using Backpropagation.IoC;
using BLL.Models;
using BLL.Services.Interfaces;
using Deedle;

namespace Backpropagation
{
    class Program
    {
        private const int EpochCount = 500;

        private static readonly string[] FeatureColumns = Enumerable.Range(2, 784).Select(v => $"Column{v}").ToArray();
        private static readonly string[] LabelColumns = Enumerable.Range(0, 10).Select(v => $"Label{v}").ToArray();

        private static readonly IDataService DataService = DependencyResolver.Resolve<IDataService>();
        private static readonly INetworkValidationService NetworkValidationService = DependencyResolver.Resolve<INetworkValidationService>();
        private static readonly IPlotService PlotService = DependencyResolver.Resolve<IPlotService>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine(@"Загрузка данных для обучения и проверки...");
            Frame<int, string> training = DataService.GetTrainingData();
            Frame<int, string> validation = DataService.GetValidationData(500);
            
            Console.WriteLine(@"Проводится нормализация пикселей...");
            NormalizePixels(training);
            NormalizePixels(validation);

            Console.WriteLine(@"Вывод случайного тренировочного образца...");
            Random rnd = new Random();
            int row = rnd.Next(1, training.RowCount);
            string randomDigit = training.Rows[row]["Column1"].ToString();
            
            double[] x = Enumerable.Range(0, 784).Select(v => (double) (v % 28)).ToArray();
            double[] y = Enumerable.Range(0, 784).Select(v => (double)(-v / 28)).ToArray();
            int[] z = Enumerable.Range(2, 784)
                .Select(i => new { i, v = training.Rows[row][$"Column{i}"] as double? })
                .Select(t => t.v > 0.5 ? 1 : 0).ToArray();

            Scatterplot plot = new Scatterplot($"Цифра {randomDigit}", "x", "y");
            plot.Compute(x, y, z);
            ScatterplotBox.Show(plot);

            Console.WriteLine(@"Кодирование цифр в обучающей и проверочной выборках...");
            EncodeDigit(training);
            EncodeDigit(validation);
           
            // build a neural network
            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), 784, 100, 100, 10);

            BackPropagationLearning learner = new BackPropagationLearning(network)
            {
                LearningRate = 0.05
            };
            new GaussianWeights(network, 0.1).Randomize();

            // train the neural network
            List<double> errors = new List<double>();
            List<double> validationErrors = new List<double>();

            double[][] features = training.Columns[FeatureColumns].ToArray2D<double>().ToJagged();
            double[][] labels = training.Columns[LabelColumns].ToArray2D<double>().ToJagged();

            Console.WriteLine($@"Запущен процесс обучения нейронной сети, количество эпох: {EpochCount}...");

            for (int i = 0; i < EpochCount; i++)
            {
                double error = learner.RunEpoch(features, labels) / labels.GetLength(0);
                errors.Add(error);

                // validate network while we are training
                //double validationError = NetworkValidationService
                //    .Validate(network, validation, labels);
                //validationErrors.Add(validationError);

                //                Console.WriteLine($"{DateTime.Now} Эпоха: {i}, Ошибка обучения: {error}, Ошибка тестирования: {validationError}");

                Console.WriteLine($"{DateTime.Now} Эпоха: {i}, Ошибка обучения: {error}");
            }

            //PlotService.PlotValidationCurve(validationErrors, EpochCount);
            PlotService.PlotTrainingCurve(errors, EpochCount);

            PrintValidationReport(network, NetworkValidationService, training);

            Console.Write("Сохраняем состояние обученной сети на диск... ");
            DataService.SaveNetworkState(network);

            Console.ReadKey();
        }

        private static void EncodeDigit(Frame<int, string> data)
        {
            for (int i = 0; i < 10; i++)
            {
                data.AddColumn($"Label{i}", 
                    data["Column1"]
                    .Values
                    .Select(v => (int)v == i ? 1.0 : 0.0));
            }
        }

        private static void NormalizePixels(Frame<int, string> data)
        {
            for (int i = 2; i < 785; i++)
            {
                data[$"Column{i}"] /= 255.0;
            }
        }

        private static void PrintValidationReport(ActivationNetwork network, 
            INetworkValidationService networkValidationService,
            Frame<int, string> validation)
        {
            Console.WriteLine($"Тестирование нейронной сети на {validation.Rows.KeyCount} записях...");

            IEnumerable<PredictionInfoModel> validationResult = networkValidationService
                .BulkValidation(network, validation);

            int numMistakes = 0;

            foreach (PredictionInfoModel prediction in validationResult)
            {
                if (!prediction.IsCorrect.Value)
                {
                    Console.WriteLine($@"ОШИБКА! {prediction.ExpectedDigit}: -> {prediction.ExpectedDigit} = {prediction.Symbol} ({100 * prediction.Probability:0.00}%)");
                    numMistakes++;
                }
                else
                {
                    Console.WriteLine($@"ВЕРНО. {prediction.ExpectedDigit}: -> {prediction.ExpectedDigit} = {prediction.Symbol} ({100 * prediction.Probability:0.00}%)");
                }
            }

            // the accuracy
            double accuracy = 100.0 * (validation.Rows.KeyCount - numMistakes) / validation.Rows.KeyCount;
            Console.WriteLine($@"Количество ошибок: {numMistakes}, Точность распознавания: {accuracy:0.00}%");
        }
    }
}