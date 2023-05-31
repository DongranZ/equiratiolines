using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class OMapDocument : IOMapDocument
    {
        private AxMapControl axMapControl1;
        public ESRI.ArcGIS.Controls.AxMapControl MapCtrl
        {
            set { axMapControl1 = value; }
        }
        public void Init(AxMapControl axMapControl)
        {
            axMapControl1 = axMapControl;
        }

        public void NewMapDoc()
        {
            if (axMapControl1 == null) throw new ArgumentNullException("地图控件为空！");

            MapDocumenHelper.NewMapDoc(axMapControl1);
        }

        public void OpenMapDocument()
        {
            if (axMapControl1 == null) throw new ArgumentNullException("地图控件为空！");

            MapDocumenHelper.OpenMapDocument(axMapControl1);
        }

        public void SaveDocument()
        {
            if (axMapControl1 == null) throw new ArgumentNullException("地图控件为空！");

            MapDocumenHelper.SaveAsDocument(axMapControl1);
        }

        public void SaveAsDocument()
        {
            if (axMapControl1 == null) throw new ArgumentNullException("地图控件为空！");

            MapDocumenHelper.SaveAsDocument(axMapControl1);
        }
    }
}
