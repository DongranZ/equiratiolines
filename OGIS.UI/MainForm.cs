using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OGIS.UI
{
    public partial class MainForm : Form
    {
        protected internal RatioPointControl ratioPointControl;
        protected internal MapBll<MainForm> mapBll;
        public MainForm()
        {
            InitializeComponent();
            Initialize();

            ratioPointControl=new RatioPointControl();
            ratioPointControl.Parent = tpgSet;
            ratioPointControl.Dock = DockStyle.Fill;
            ratioPointControl.LoadMap(mapBll.axMapControl);
        }
        /// <summary>
        /// 初始化逻辑组件
        /// </summary>
        public void Initialize()
        {
            mapBll = new MainForm_BLL();
            mapBll.NewMap(this);
            mapBll.Banding(splitContainer1.Panel2, tpgToc);
            //mapBll.SetCurrentOpterate += DoSetCurrentOpterate;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            mapBll.NewMapDoc();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            mapBll.OpenMapDocument();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            mapBll.SaveAsDocument();
        }

        private void tsbSaveAs_Click(object sender, EventArgs e)
        {
            mapBll.SaveAsDocument();
        }

        private void tsbZoomIn_Click(object sender, EventArgs e)
        {
            mapBll.MapZoomIn();
        }

        private void tsbRoomOut_Click(object sender, EventArgs e)
        {
            mapBll.MapZoomOut();
        }

        private void tsbMapFull_Click(object sender, EventArgs e)
        {
            mapBll.MapFull();
        }

        private void tsbMapPan_Click(object sender, EventArgs e)
        {
            mapBll.MapPan();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void tsbImportShp_Click(object sender, EventArgs e)
        {

        }

        private void tsbNull_Click(object sender, EventArgs e)
        {
            mapBll.NoFunction();
        }
    }
}
