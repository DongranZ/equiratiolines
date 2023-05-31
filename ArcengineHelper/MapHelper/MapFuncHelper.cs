using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2020-12-03
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcengineHelper.MapHelper
{
    public static class MapFuncHelper
    {
        #region 地图放大缩小
        /// <summary>
        /// 地图全图
        /// </summary>
        public static void MapFull(AxMapControl axMapControl)
        {
            axMapControl.Extent = axMapControl.FullExtent;
        }
        /// <summary>
        /// 地图放大
        /// </summary>
        public static void MapZoomIn(AxMapControl axMapControl)
        { 
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomInToolClass();
            pCommand.OnCreate(axMapControl.Object);
            axMapControl.CurrentTool = pCommand as ITool;
        }
        /// <summary>
        /// 地图缩小
        /// </summary>
        public static void MapZoomOut(AxMapControl axMapControl)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapZoomOutToolClass();
            pCommand.OnCreate(axMapControl.Object);
            axMapControl.CurrentTool = pCommand as ITool;
        }
        /// <summary>
        /// 地图平移
        /// </summary>
        public static void MapPan(AxMapControl axMapControl)
        {
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapPanToolClass();
            pCommand.OnCreate(axMapControl.Object);
            axMapControl.CurrentTool = pCommand as ITool;

        }
        #endregion


    }
}
