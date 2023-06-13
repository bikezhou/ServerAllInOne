using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAllInOne.Controls
{
    public partial class ServerList : ListBox
    {
        public ServerList()
        {
            InitializeComponent();

            ItemHeight = 24;
            DrawMode = DrawMode.OwnerDrawVariable;
            DrawItem += ServerList_DrawItem;
        }

        private void ServerList_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || Items.Count == 0)
                return;

            var rect = e.Bounds;

            if (Items[e.Index] is ServerListItem item)
            {
                var iconSize = 10;

                e.DrawBackground();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(item.Running ? Brushes.Green : Brushes.Red, new Rectangle(rect.X + (rect.Height - iconSize) / 2, rect.Y + (rect.Height - iconSize) / 2, iconSize, iconSize));

                using var brush = new SolidBrush(e.ForeColor);
                using var sf = new StringFormat()
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                var textArea = new Rectangle(rect.Height, rect.Y, rect.Width - rect.Height, rect.Height);
                e.Graphics.DrawString(item.Name, Font, brush, textArea, sf);

                if (item.Running && item.ProcessId > 0)
                {
                    sf.Alignment = StringAlignment.Far;
                    e.Graphics.DrawString(item.ProcessId.ToString(), Font, brush, textArea, sf);

                }
            }
        }
    }
}
