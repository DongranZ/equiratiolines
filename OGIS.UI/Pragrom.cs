using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OGIS.UI
{
    static class Pragrom
    {
        [STAThread]
        static void Main(string[] aArgs)
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Initialize();
            Application.Run(new MainForm());
        }
        public static void Initialize()
        {
            Gis.App_Start.UI_Init.Inits();
        }
    }
}
