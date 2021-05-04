namespace ImageRecognition
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.handWrittenPictureBox = new System.Windows.Forms.PictureBox();
            this.recognizeDigitButton = new System.Windows.Forms.Button();
            this.ClearPictureBoxButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.handWrittenPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // handWrittenPictureBox
            // 
            this.handWrittenPictureBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.handWrittenPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.handWrittenPictureBox.Location = new System.Drawing.Point(12, 28);
            this.handWrittenPictureBox.Name = "handWrittenPictureBox";
            this.handWrittenPictureBox.Size = new System.Drawing.Size(280, 280);
            this.handWrittenPictureBox.TabIndex = 0;
            this.handWrittenPictureBox.TabStop = false;
            this.handWrittenPictureBox.Click += new System.EventHandler(this.handWrittenPictureBox_Click);
            this.handWrittenPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handWrittenPictureBox_MouseDown);
            this.handWrittenPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.handWrittenPictureBox_MouseMove);
            this.handWrittenPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.handWrittenPictureBox_MouseUp);
            // 
            // recognizeDigitButton
            // 
            this.recognizeDigitButton.Location = new System.Drawing.Point(12, 333);
            this.recognizeDigitButton.Name = "recognizeDigitButton";
            this.recognizeDigitButton.Size = new System.Drawing.Size(132, 44);
            this.recognizeDigitButton.TabIndex = 1;
            this.recognizeDigitButton.Text = "Распознать";
            this.recognizeDigitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.recognizeDigitButton.UseVisualStyleBackColor = true;
            this.recognizeDigitButton.Click += new System.EventHandler(this.recognizeDigitButton_Click);
            // 
            // ClearPictureBoxButton
            // 
            this.ClearPictureBoxButton.Location = new System.Drawing.Point(162, 333);
            this.ClearPictureBoxButton.Name = "ClearPictureBoxButton";
            this.ClearPictureBoxButton.Size = new System.Drawing.Size(130, 44);
            this.ClearPictureBoxButton.TabIndex = 2;
            this.ClearPictureBoxButton.Text = "Очистить";
            this.ClearPictureBoxButton.UseVisualStyleBackColor = true;
            this.ClearPictureBoxButton.Click += new System.EventHandler(this.ClearPictureBoxButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 389);
            this.Controls.Add(this.ClearPictureBoxButton);
            this.Controls.Add(this.recognizeDigitButton);
            this.Controls.Add(this.handWrittenPictureBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.handWrittenPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox handWrittenPictureBox;
        private System.Windows.Forms.Button recognizeDigitButton;
        private System.Windows.Forms.Button ClearPictureBoxButton;
    }
}

