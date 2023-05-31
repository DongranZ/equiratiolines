using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Richway.Ocean.Common
{
    public partial class NumTextBox : System.Windows.Forms.TextBox
    {
        public NumTextBox()
        {
            InitializeComponent();
        }
        //ascII
        //回车键:VK_RETURN (13)
        //Shift键:VK_SHIFT (16)
        //Ctrl键:VK_CONTROL (17)
        //Alt键:VK_MENU (18)
        //退格键:VK_BACK (8)
        //End键:VK_END (35)
        //Delete键:VK_DELETE (46)
        //方向键(←):VK_LEFT (37)
        //方向键(↑):VK_UP (38)
        //方向键(→):VK_RIGHT (39)
        //方向键(↓):VK_DOWN (40)
        //小键盘0:VK_NUMPAD0 (96)
        //小键盘1:VK_NUMPAD1 (97)
        //小键盘2:VK_NUMPAD2 (98)
        //小键盘3:VK_NUMPAD3 (99)
        //小键盘4:VK_NUMPAD4 (100)
        //小键盘5:VK_NUMPAD5 (101)
        //小键盘6:VK_NUMPAD6 (102)
        //小键盘7:VK_NUMPAD7 (103)
        //小键盘8:VK_NUMPAD8 (104)
        //小键盘9:VK_NUMPAD9 (105)
        //小键盘。:VK_DECIMAL (110)
        //小键盘*:VK_MULTIPLY (106)
        //小键盘+:VK_ADD (107)
        //小键盘-:VK_SUBTRACT (109)
        //小键盘/:VK_DIVIDE (111)
        private int[] arr = new int[] { 8, 13, 16, 17, 18, 35, 36, 37, 38, 39, 40,45, 46, 109, 189 };
        protected override bool ProcessKeyEventArgs(ref Message m)
        {
            int keyValue = m.WParam.ToInt32();
            if ((keyValue > 47 && keyValue < 58) || arr.Contains(keyValue)
                )
            {
                return base.ProcessKeyEventArgs(ref m);
            }
            else if (m.Msg == 256 && keyValue == 46)//Delete Key
            {
                return base.ProcessKeyEventArgs(ref m);
            }
            return true;

        }

        double _numText = 0;
        /// <summary>
        /// 属性NumText 获取Text值，并转化为数字
        /// </summary>
        public double NumText
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    return 0;
                }
                if (double.TryParse(this.Text, out _numText))
                {
                    return _numText;
                }
                return 0;
            }
        }
    }
}
