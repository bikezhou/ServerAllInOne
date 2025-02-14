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
    public partial class ConfigEditorForm : Form
    {
        private string _originConfig = string.Empty;
        private string _openConfigPath = string.Empty;

        private int _selectedIndex = -1;

        public string WorkingDirectory { get; set; }

        public KeyValuePair<string, string>[] Configs { get; set; }

        public bool ConfigModified
        {
            get => !string.IsNullOrEmpty(_openConfigPath) && File.Exists(_openConfigPath) && _originConfig != richEditor.Text;
        }

        public ConfigEditorForm()
        {
            InitializeComponent();

            Load += ConfigEditorForm_Load;
            FormClosing += ConfigEditorForm_FormClosing;
        }

        private void ConfigEditorForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            CheckConfigSave();
        }

        private void ConfigEditorForm_Load(object? sender, EventArgs e)
        {
            listConfig.DisplayMember = "Key";
            listConfig.ValueMember = "Value";
            listConfig.DataSource = Configs;
        }

        private void listConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedIndex == listConfig.SelectedIndex)
            {
                return;
            }

            _selectedIndex = listConfig.SelectedIndex;
            CheckConfigSave();

            var item = listConfig.SelectedItem;
            _openConfigPath = Path.Combine(WorkingDirectory, ((KeyValuePair<string, string>)item).Value);
            richEditor.Clear();
            richEditor.ClearUndo();
            if (File.Exists(_openConfigPath))
            {
                _originConfig = File.ReadAllText(_openConfigPath);
                richEditor.Text = _originConfig;
            }
            else
            {
                richEditor.Text = "-- empty --";
            }
        }

        private void CheckConfigSave()
        {
            if (ConfigModified)
            {
                if (MessageBox.Show("配置修改未保存，是否保存修改？", "保存配置", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveConfig();
                }
            }
        }

        private void SaveConfig()
        {
            File.WriteAllText(_openConfigPath, richEditor.Text, new UTF8Encoding(false)/* 不带BOM的UTF8编码 */);
            _originConfig = richEditor.Text;
        }

        private void ConfigEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveConfig();
            }
        }

        private void richEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                e.Handled = true;
                richEditor.SelectedText = new string(' ', 4);
            }
        }
    }
}
