namespace ServerAllInOne.Forms
{
    partial class EditServerForm
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
            txtName = new TextBox();
            label1 = new Label();
            label2 = new Label();
            txtExePath = new TextBox();
            label3 = new Label();
            txtArguments = new TextBox();
            btnOk = new Button();
            btnSelect = new Button();
            label4 = new Label();
            nudSort = new NumericUpDown();
            chkCanInput = new CheckBox();
            txtWorkingDirectory = new TextBox();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)nudSort).BeginInit();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(86, 20);
            txtName.Name = "txtName";
            txtName.Size = new Size(278, 23);
            txtName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 23);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 1;
            label1.Text = "名      称：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 53);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 3;
            label2.Text = "程序路径：";
            // 
            // txtExePath
            // 
            txtExePath.Location = new Point(86, 50);
            txtExePath.Name = "txtExePath";
            txtExePath.Size = new Size(245, 23);
            txtExePath.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 83);
            label3.Name = "label3";
            label3.Size = new Size(68, 17);
            label3.TabIndex = 5;
            label3.Text = "执行参数：";
            // 
            // txtArguments
            // 
            txtArguments.Location = new Point(86, 80);
            txtArguments.Name = "txtArguments";
            txtArguments.Size = new Size(278, 23);
            txtArguments.TabIndex = 4;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(289, 139);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 29);
            btnOk.TabIndex = 6;
            btnOk.Text = "添加";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnSelect
            // 
            btnSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelect.Location = new Point(337, 50);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(30, 23);
            btnSelect.TabIndex = 7;
            btnSelect.Text = "...";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(18, 145);
            label4.Name = "label4";
            label4.Size = new Size(68, 17);
            label4.TabIndex = 9;
            label4.Text = "排      序：";
            // 
            // nudSort
            // 
            nudSort.BorderStyle = BorderStyle.FixedSingle;
            nudSort.Location = new Point(86, 140);
            nudSort.Name = "nudSort";
            nudSort.Size = new Size(69, 23);
            nudSort.TabIndex = 10;
            // 
            // chkCanInput
            // 
            chkCanInput.AutoSize = true;
            chkCanInput.Location = new Point(177, 143);
            chkCanInput.Name = "chkCanInput";
            chkCanInput.Size = new Size(75, 21);
            chkCanInput.TabIndex = 11;
            chkCanInput.Text = "允许输入";
            chkCanInput.UseVisualStyleBackColor = true;
            // 
            // txtWorkingDirectory
            // 
            txtWorkingDirectory.Location = new Point(86, 110);
            txtWorkingDirectory.Name = "txtWorkingDirectory";
            txtWorkingDirectory.Size = new Size(278, 23);
            txtWorkingDirectory.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 113);
            label5.Name = "label5";
            label5.Size = new Size(68, 17);
            label5.TabIndex = 5;
            label5.Text = "工作目录：";
            // 
            // EditServerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(376, 175);
            Controls.Add(chkCanInput);
            Controls.Add(nudSort);
            Controls.Add(label4);
            Controls.Add(btnSelect);
            Controls.Add(btnOk);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(txtWorkingDirectory);
            Controls.Add(txtArguments);
            Controls.Add(label2);
            Controls.Add(txtExePath);
            Controls.Add(label1);
            Controls.Add(txtName);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "EditServerForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "添加服务";
            ((System.ComponentModel.ISupportInitialize)nudSort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private Label label1;
        private Label label2;
        private TextBox txtExePath;
        private Label label3;
        private TextBox txtArguments;
        private Button btnOk;
        private Button btnSelect;
        private Label label4;
        private NumericUpDown nudSort;
        private CheckBox chkCanInput;
        private TextBox txtWorkingDirectory;
        private Label label5;
    }
}