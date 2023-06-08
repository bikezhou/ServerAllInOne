using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerAllInOne.Controls
{
    public class ServerTabControl : TabControl
    {
        public ServerTabControl()
        {
            DrawMode = TabDrawMode.OwnerDrawFixed;
            DrawItem += ServerTabControl_DrawItem;
        }

        private void ServerTabControl_DrawItem(object? sender, DrawItemEventArgs e)
        {
            TabPage changedpage = this.TabPages[e.Index];//当前处理标签
            Rectangle backrect = this.GetTabRect(e.Index);//标签背景区域
            Brush backbrush;//标签背景色
            Brush fontbrush;//标签字体颜色
            Font tabFont;//标签字体
                         //Pen borderpen;//边框颜色

            Brush backtabcontrol = new SolidBrush(Color.FromArgb(255, 43, 87, 154));
            e.Graphics.FillRectangle(backtabcontrol, this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Size.Width, this.ClientRectangle.Height);
            backtabcontrol.Dispose();

            if (e.State == DrawItemState.Selected)
            {
                backbrush = new SolidBrush(Color.FromArgb(255, 51, 102, 255));
                fontbrush = new SolidBrush(Color.White);
                tabFont = new Font("微软雅黑", 13, FontStyle.Bold, GraphicsUnit.Pixel);
                //borderpen = new Pen(Color.FromArgb(255, 51, 102, 255));
            }
            else
            {
                backbrush = new SolidBrush(Color.FromArgb(255, 43, 87, 154));
                fontbrush = new SolidBrush(Color.White);
                tabFont = new Font("微软雅黑", 13, FontStyle.Bold, GraphicsUnit.Pixel);
                //borderpen = new Pen(Color.FromArgb(255, 43, 87, 154));
            }
            //绘制标签背景
            e.Graphics.FillRectangle(backbrush, backrect);

            //绘制标签字体
            StringFormat _StringFlags = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            _StringFlags.Alignment = StringAlignment.Center;
            _StringFlags.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(this.Controls[e.Index].Text, tabFont, fontbrush, backrect, _StringFlags);
            //绘制非标签原始名称【可依据e.State修改】 g.DrawString("呵呵", tabFont, fontbrush, backrect, new StringFormat(_StringFlags));

            //绘制标签边框
            //backrect.Offset(1, 1);
            //backrect.Inflate(2, 2);
            //e.Graphics.DrawRectangle(borderpen, backrect);

            backbrush.Dispose();
            tabFont.Dispose();
            fontbrush.Dispose();
            //borderpen.Dispose();

            // 背景添加图片
            //Bitmap b0 = new Bitmap(@"haha.png");
            //e.Graphics.DrawImage(b0, e.Bounds);
        }
    }
}
