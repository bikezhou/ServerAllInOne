namespace ServerAllInOne.Controls
{
    partial class ServerConsole
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox = new RichTextBox();
            txtInput = new TextBox();
            SuspendLayout();
            // 
            // richTextBox
            // 
            richTextBox.BackColor = Color.Black;
            richTextBox.BorderStyle = BorderStyle.None;
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Font = new Font("Consolas", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox.ForeColor = Color.Gainsboro;
            richTextBox.Location = new Point(0, 0);
            richTextBox.Name = "richTextBox";
            richTextBox.ReadOnly = true;
            richTextBox.Size = new Size(642, 333);
            richTextBox.TabIndex = 1;
            richTextBox.Text = "";
            // 
            // txtInput
            // 
            txtInput.BorderStyle = BorderStyle.None;
            txtInput.Dock = DockStyle.Bottom;
            txtInput.Font = new Font("Consolas", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtInput.Location = new Point(0, 333);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(642, 17);
            txtInput.TabIndex = 0;
            txtInput.KeyDown += txtInput_KeyDown;
            // 
            // ServerConsole
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(richTextBox);
            Controls.Add(txtInput);
            Name = "ServerConsole";
            Size = new Size(642, 350);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox;
        private TextBox txtInput;
    }
}
