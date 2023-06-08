using ServerAllInOne.Configs;
using ServerAllInOne.Controls;
using ServerAllInOne.Forms;

namespace ServerAllInOne
{
    public partial class MainForm : Form
    {
        ServerConfigs configs;
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
        }

        private void AddServerTab(Server server)
        {
            var tabPage = new TabPage(server.Name);
            tabPage.Controls.Add(new ServerConsole()
            {
                Dock = DockStyle.Fill,
                ServerConfig = server
            });
            tabPage.Tag = server;
            tabControl.TabPages.Add(tabPage);
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

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            foreach (TabPage tabPage in this.tabControl.TabPages)
            {
                if (tabPage == tabHome)
                    continue;

                var console = tabPage.Controls[0] as ServerConsole;
                if (console != null)
                {
                    if (!console.Running)
                    {
                        console.Start();
                    }
                }
            }
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnStopServer_Click(btnStopServer, EventArgs.Empty);
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            ServerTabForeach(tabPage =>
            {
                (tabPage.Controls[0] as ServerConsole)?.Stop();
            });
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
            var console = tabControl.SelectedTab.Controls[0] as ServerConsole;
            console?.Start();
        }

        private void tsmiStopServer_Click(object sender, EventArgs e)
        {
            var console = tabControl.SelectedTab.Controls[0] as ServerConsole;
            console?.Stop();
        }
    }
}