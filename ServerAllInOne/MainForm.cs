using ServerAllInOne.Configs;
using ServerAllInOne.Controls;
using ServerAllInOne.Forms;
using System.Reflection;
using System.Windows.Forms;

namespace ServerAllInOne
{
    public partial class MainForm : Form
    {
        ServerConfigs configs;

        private int runningServerCount = 0;

        public bool ConfirmExit { get; set; }
        public MainForm()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            configs = ServerConfigs.Default;
            configs.Servers.Sort((a, b) => a.Sort - b.Sort);
            foreach (var server in configs.Servers)
            {
                AddServerTab(server);
            }

            UpdateNotifyIcon();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ConfirmExit)
            {
                StopAllServer();
            }
            else
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void AddServerTab(Server server)
        {
            var tabPage = new TabPage(server.Name);
            var serverConsole = new ServerConsole()
            {
                Dock = DockStyle.Fill,
                ServerConfig = server
            };
            serverConsole.ServerStateChanged += ServerConsole_ServerStateChanged;
            tabPage.Controls.Add(serverConsole);
            tabPage.Tag = server;
            tabControl.TabPages.Add(tabPage);
        }

        private void ServerConsole_ServerStateChanged(object? sender, EventArgs e)
        {
            // 服务状态改变

            if (sender is ServerConsole server)
            {
                if (server.Running)
                {
                    runningServerCount++;
                }
                else
                {
                    runningServerCount--;
                }
            }

            UpdateNotifyIcon();
        }

        private void SortServerTab()
        {
            tabControl.SuspendLayout();
            var tabPages = new List<TabPage>();

            ServerTabForeach(tabPage =>
            {
                tabPages.Add(tabPage);
            });
            foreach (var tabPage in tabPages)
            {
                tabControl.TabPages.Remove(tabPage);
            }

            foreach (var server in configs.Servers)
            {
                var tabPage = tabPages.Find(t => t.Tag == server);
                tabControl.TabPages.Add(tabPage);
            }

            tabControl.ResumeLayout(true);
        }

        private void btnAddServer_Click(object sender, EventArgs e)
        {
            var addForm = new AddServerForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                if (addForm.Server != null)
                {
                    configs.Servers.Add(addForm.Server);
                    configs.Servers.Sort((a, b) => a.Sort - b.Sort);
                    configs.Save();

                    AddServerTab(addForm.Server);

                    SortServerTab();
                }
            }
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否一键启动所有服务？", "启动服务", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            StartAllServer();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (runningServerCount == 0)
                return;

            if (MessageBox.Show("是否一键停止已启动服务？", "停止服务", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            StopAllServer();
        }

        private void StartAllServer()
        {
            ServerTabForeach(tabPage =>
            {
                if (tabPage.Controls[0] is ServerConsole server)
                {
                    server.Start();

                    if (tabPage.Tag is Server c && server.ProcessId.HasValue)
                    {
                        tabPage.Text = $"{c.Name}[{server.ProcessId.Value}]";
                    }
                }
            });
        }

        private void StopAllServer()
        {
            ServerTabForeach(tabPage =>
            {
                if (tabPage.Controls[0] is ServerConsole server)
                {
                    server.Stop();
                }

                if (tabPage.Tag is Server c)
                {
                    tabPage.Text = c.Name;
                }
            });
        }

        private void UpdateNotifyIcon()
        {
            var running = 0;
            var total = 0;
            ServerTabForeach(tabPage =>
            {
                total++;
                if (tabPage.Controls[0] is ServerConsole sc && sc.Running)
                {
                    running++;
                }
            });

            var iconPath = running == 0 ? @"ServerAllInOne.Assets.Icons.server_windows_stop.ico"
                                        : @"ServerAllInOne.Assets.Icons.server_windows.ico";

            notifyIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(iconPath));

            notifyIcon.Text = $"正在运行：{running}/{total}";
        }

        private void ServerTabForeach(Action<TabPage> action)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage == tabHome)
                    continue;

                action?.Invoke(tabPage);
            }
        }

        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 1; i < tabControl.TabPages.Count; i++)
                {
                    var tabPage = tabControl.TabPages[i];
                    if (tabControl.GetTabRect(i).Contains(new Point(e.X, e.Y)))
                    {
                        tabControl.SelectedTab = tabPage;
                        break;
                    }
                }

                tabControl.ContextMenuStrip = cmsTabMenu;
            }
        }

        private void tabControl_MouseLeave(object sender, EventArgs e)
        {
            tabControl.ContextMenuStrip = null;
        }

        private void cmsTabMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var console = tabControl.SelectedTab.Controls[0] as ServerConsole;
            if (console != null)
            {
                tsmiStartServer.Enabled = !console.Running;
                tsmiStopServer.Enabled = console.Running;
            }
            else
            {
                tsmiStopServer.Enabled = tsmiStartServer.Enabled = false;
            }
        }

        private void tsmiStartServer_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                server.Start();

                if (tabPage.Tag is Server c && server.ProcessId.HasValue)
                {
                    tabPage.Text = $"{c.Name}[{server.ProcessId.Value}]";
                }
            }
        }

        private void tsmiStopServer_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                server.Stop();
            }
            if (tabPage.Tag is Server c)
            {
                tabPage.Text = c.Name;
            }
        }

        private void tsmiOpenMainForm_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            ConfirmExit = true;
            Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsmiOpenMainForm_Click(tsmiOpenMainForm, EventArgs.Empty);
        }
    }
}