using ServerAllInOne.Configs;

namespace ServerAllInOne.Forms
{
    public partial class EditServerForm : Form
    {
        public Server Server { get; private set; }

        public EditServerForm()
        {
            InitializeComponent();
        }
        public void Edit(Server server)
        {
            Server = server;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var openFileDlg = new OpenFileDialog();
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                txtExePath.Text = openFileDlg.FileName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("服务名称未指定");
            }
            if (txtExePath.Text.Length == 0 || !File.Exists(txtExePath.Text))
            {
                MessageBox.Show("执行程序路径未指定或文件不存在");
            }

            if (Server == null)
            {
                Server = new Server
                {
                    UUID = Guid.NewGuid().ToString()
                };
            }

            Server.Name = txtName.Text;
            Server.ExePath = (txtExePath.Text ?? "").Replace('\\', '/');
            Server.Arguments = txtArguments.Text;
            Server.WorkingDirectory = (txtWorkingDirectory.Text ?? "").Replace('\\', '/');
            Server.Sort = (int)nudSort.Value;
            Server.CanInput = chkCanInput.Checked;
        }
    }
}
