using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2018-**-**
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
    public static class MapHelper
    {
        public static double GetMapScaleRuler(IActiveView activeView)
        {
            double lnScaleRuler = -1;
            if (activeView == null || activeView.FocusMap == null) return lnScaleRuler;
            try
            {
                lnScaleRuler = Math.Round(activeView.FocusMap.MapScale, 0);
                if (60000000 > lnScaleRuler && lnScaleRuler >= 10000)
                {
                    lnScaleRuler = Math.Round(lnScaleRuler / 10000) * 10000;
                }
                else if (10000 > lnScaleRuler && lnScaleRuler >= 1000)
                {
                    lnScaleRuler = Math.Round(lnScaleRuler / 1000) * 1000;
                }

                else if (1000 > lnScaleRuler && lnScaleRuler >= 100)
                {
                    lnScaleRuler = Math.Round(lnScaleRuler / 100) * 100;
                }
                return lnScaleRuler;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static void FocusMapLayer(AxTOCControl axTOCControl, AxMapControl axMapControl)
        {
            if (axTOCControl == null || axMapControl == null) return;
            esriTOCControlItem itemSel = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap mapSel = null;
            ILayer layerSel = null;
            object otherSel = null;
            object indexSel = null;
            axTOCControl.GetSelectedItem(ref itemSel, ref mapSel, ref layerSel, ref otherSel, ref indexSel);
            if (layerSel == null) return;
            (axMapControl.Map as IActiveView).Extent = layerSel.AreaOfInterest;
            (axMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        public static void ClearMapElement(AxMapControl axMapControl)
        {
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)axMapControl.ActiveView;
            pGraphicsContainer.DeleteAllElements();
            axMapControl.Map.ClearSelection();
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, axMapControl.ActiveView.Extent);
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, axMapControl.ActiveView.Extent);
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl.ActiveView.Extent);
        }
    }
}
