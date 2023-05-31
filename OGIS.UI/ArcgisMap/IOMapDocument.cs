using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public interface IOMapDocument
    {
        AxMapControl MapCtrl { set; }
        void Init(AxMapControl axMapControl);
        void NewMapDoc();
        void OpenMapDocument();
        void SaveDocument();
        void SaveAsDocument();
    }
}
