﻿namespace ServerAllInOne
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabHome = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnStopServer = new Button();
            btnAddServer = new Button();
            btnStartServer = new Button();
            tabControl = new TabControl();
            cmsTabMenu = new ContextMenuStrip(components);
            tsmiStartServer = new ToolStripMenuItem();
            tsmiStopServer = new ToolStripMenuItem();
            notifyIcon = new NotifyIcon(components);
            cmsMain = new ContextMenuStrip(components);
            tsmiOpenMainForm = new ToolStripMenuItem();
            tsmiExit = new ToolStripMenuItem();
            tabHome.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tabControl.SuspendLayout();
            cmsTabMenu.SuspendLayout();
            cmsMain.SuspendLayout();
            SuspendLayout();
            // 
            // tabHome
            // 
            tabHome.Controls.Add(tableLayoutPanel1);
            tabHome.Location = new Point(4, 26);
            tabHome.Name = "tabHome";
            tabHome.Padding = new Padding(3);
            tabHome.Size = new Size(966, 501);
            tabHome.TabIndex = 0;
            tabHome.Text = "主页";
            tabHome.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(btnStopServer, 3, 1);
            tableLayoutPanel1.Controls.Add(btnAddServer, 1, 1);
            tableLayoutPanel1.Controls.Add(btnStartServer, 2, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(960, 495);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // btnStopServer
            // 
            btnStopServer.Dock = DockStyle.Fill;
            btnStopServer.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnStopServer.ForeColor = Color.Red;
            btnStopServer.Location = new Point(543, 220);
            btnStopServer.Name = "btnStopServer";
            btnStopServer.Size = new Size(114, 54);
            btnStopServer.TabIndex = 2;
            btnStopServer.Text = "停止服务";
            btnStopServer.UseVisualStyleBackColor = true;
            btnStopServer.Click += btnStopServer_Click;
            // 
            // btnAddServer
            // 
            btnAddServer.Dock = DockStyle.Fill;
            btnAddServer.Location = new Point(303, 220);
            btnAddServer.Name = "btnAddServer";
            btnAddServer.Size = new Size(114, 54);
            btnAddServer.TabIndex = 1;
            btnAddServer.Text = "添加服务";
            btnAddServer.UseVisualStyleBackColor = true;
            btnAddServer.Click += btnAddServer_Click;
            // 
            // btnStartServer
            // 
            btnStartServer.Dock = DockStyle.Fill;
            btnStartServer.Location = new Point(423, 220);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(114, 54);
            btnStartServer.TabIndex = 0;
            btnStartServer.Text = "启动服务";
            btnStartServer.UseVisualStyleBackColor = true;
            btnStartServer.Click += btnStartServer_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabHome);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(5, 5);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(974, 531);
            tabControl.TabIndex = 0;
            tabControl.MouseDown += tabControl_MouseDown;
            tabControl.MouseLeave += tabControl_MouseLeave;
            // 
            // cmsTabMenu
            // 
            cmsTabMenu.Items.AddRange(new ToolStripItem[] { tsmiStartServer, tsmiStopServer });
            cmsTabMenu.Name = "contextMenuStrip1";
            cmsTabMenu.Size = new Size(125, 48);
            cmsTabMenu.Opening += cmsTabMenu_Opening;
            // 
            // tsmiStartServer
            // 
            tsmiStartServer.Name = "tsmiStartServer";
            tsmiStartServer.Size = new Size(124, 22);
            tsmiStartServer.Text = "启动服务";
            tsmiStartServer.Click += tsmiStartServer_Click;
            // 
            // tsmiStopServer
            // 
            tsmiStopServer.Name = "tsmiStopServer";
            tsmiStopServer.Size = new Size(124, 22);
            tsmiStopServer.Text = "停止服务";
            tsmiStopServer.Click += tsmiStopServer_Click;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = cmsMain;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "4543634646";
            notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            // 
            // cmsMain
            // 
            cmsMain.Items.AddRange(new ToolStripItem[] { tsmiOpenMainForm, tsmiExit });
            cmsMain.Name = "cmsMain";
            cmsMain.Size = new Size(137, 48);
            // 
            // tsmiOpenMainForm
            // 
            tsmiOpenMainForm.Name = "tsmiOpenMainForm";
            tsmiOpenMainForm.Size = new Size(136, 22);
            tsmiOpenMainForm.Text = "打开主界面";
            tsmiOpenMainForm.Click += tsmiOpenMainForm_Click;
            // 
            // tsmiExit
            // 
            tsmiExit.Name = "tsmiExit";
            tsmiExit.Size = new Size(136, 22);
            tsmiExit.Text = "退出";
            tsmiExit.Click += tsmiExit_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 541);
            Controls.Add(tabControl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Padding = new Padding(5);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "服务一键启动";
            FormClosing += MainForm_FormClosing;
            tabHome.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            cmsTabMenu.ResumeLayout(false);
            cmsMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabPage tabHome;
        private TabControl tabControl;
        private Button btnStartServer;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnAddServer;
        private Button btnStopServer;
        private ContextMenuStrip cmsTabMenu;
        private ToolStripMenuItem tsmiStartServer;
        private ToolStripMenuItem tsmiStopServer;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip cmsMain;
        private ToolStripMenuItem tsmiOpenMainForm;
        private ToolStripMenuItem tsmiExit;
    }
}