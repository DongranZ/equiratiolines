using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public partial class MainForm_BLL : MapBll<MainForm>
    {
        public override void InitializeBll()
        {
            _MapDocument = new OMapDocument();
            _MapDocument.Init(base.axMapControl);

            _MapDisplay = new ArcengineHelper.MapHelper.MapDisplay(base.axMapControl);
        }

        public AxMapControl Map { get { return this.axMapControl; } }
    }
    public partial class MainForm_BLL : MapBll<MainForm>
    {
        public void SetCurrentOperate(AOceanEntity aEntity)
        {
            var toolEntity = aEntity as ToolEntity;
            if (toolEntity == null) return;
            SetCurrentOperate(toolEntity.ToolOption);
        }
        public override void SetCurrentOperate(CurrentOperate operate)
        {
            base._CurrentOperate = operate;
        }
    }
}