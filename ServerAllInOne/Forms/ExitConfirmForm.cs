using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerAllInOne.Forms
{
    public partial class ExitConfirmForm : Form
    {
        /// <summary>
        /// 是否直接退出
        /// </summary>
        public bool SimplyExit { get; private set; }

        public ExitConfirmForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SimplyExit = rbExit.Checked;
        }
    }
}
