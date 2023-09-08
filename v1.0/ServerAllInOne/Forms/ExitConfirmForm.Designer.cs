namespace ServerAllInOne.Forms
{
    partial class ExitConfirmForm
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
            rbMinimum = new RadioButton();
            rbExit = new RadioButton();
            btnOk = new Button();
            SuspendLayout();
            // 
            // rbMinimum
            // 
            rbMinimum.AutoSize = true;
            rbMinimum.Checked = true;
            rbMinimum.Location = new Point(12, 12);
            rbMinimum.Name = "rbMinimum";
            rbMinimum.Size = new Size(98, 21);
            rbMinimum.TabIndex = 0;
            rbMinimum.TabStop = true;
            rbMinimum.Text = "最小化到托盘";
            rbMinimum.UseVisualStyleBackColor = true;
            // 
            // rbExit
            // 
            rbExit.AutoSize = true;
            rbExit.Location = new Point(12, 39);
            rbExit.Name = "rbExit";
            rbExit.Size = new Size(74, 21);
            rbExit.TabIndex = 1;
            rbExit.Text = "直接退出";
            rbExit.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(86, 67);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 29);
            btnOk.TabIndex = 7;
            btnOk.Text = "确定";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // ExitConfirmForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(257, 108);
            Controls.Add(btnOk);
            Controls.Add(rbExit);
            Controls.Add(rbMinimum);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExitConfirmForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "退出程序？";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton rbMinimum;
        private RadioButton rbExit;
        private Button btnOk;
    }
}