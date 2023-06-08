using ServerAllInOne.Configs;

namespace ServerAllInOne.Forms
{
    public partial class AddServerForm : Form
    {
        public Server Server { get; private set; }

        public AddServerForm()
        {
            InitializeComponent();
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

            Server = new Server
            {
                Name = txtName.Text,
                ExePath = txtExePath.Text,
                Arguments = txtArguments.Text,
                Sort = (int)nudSort.Value,
                CanInput = chkCanInput.Checked
            };
        }
    }
}
