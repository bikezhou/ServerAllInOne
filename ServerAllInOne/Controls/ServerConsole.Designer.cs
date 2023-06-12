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
            components = new System.ComponentModel.Container();
            richTextBox = new RichTextBox();
            cmsRichText = new ContextMenuStrip(components);
            tsmiCopy = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            tsmiClear = new ToolStripMenuItem();
            txtInput = new TextBox();
            cmsRichText.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox
            // 
            richTextBox.BackColor = Color.Black;
            richTextBox.BorderStyle = BorderStyle.None;
            richTextBox.ContextMenuStrip = cmsRichText;
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
            // cmsRichText
            // 
            cmsRichText.Items.AddRange(new ToolStripItem[] { tsmiCopy, toolStripMenuItem1, tsmiClear });
            cmsRichText.Name = "cmsMain";
            cmsRichText.Size = new Size(181, 76);
            cmsRichText.Opening += cmsRichText_Opening;
            // 
            // tsmiCopy
            // 
            tsmiCopy.Name = "tsmiCopy";
            tsmiCopy.ShortcutKeys = Keys.Control | Keys.C;
            tsmiCopy.Size = new Size(180, 22);
            tsmiCopy.Text = "复制";
            tsmiCopy.Click += tsmiCopy_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(177, 6);
            // 
            // tsmiClear
            // 
            tsmiClear.Name = "tsmiClear";
            tsmiClear.Size = new Size(180, 22);
            tsmiClear.Text = "清空(:clear)";
            tsmiClear.Click += tsmiClear_Click;
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
            cmsRichText.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox;
        private TextBox txtInput;
        private ContextMenuStrip cmsRichText;
        private ToolStripMenuItem tsmiCopy;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem tsmiClear;
    }
}
