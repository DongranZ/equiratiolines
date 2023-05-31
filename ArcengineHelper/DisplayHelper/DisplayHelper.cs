using ArcengineHelper.MapHelper;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2021-05-26
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcengineHelper.DisplayHelper
{
    public static class DisplayHelper
    {
        /// <summary>
        /// 在地图上绘制几何；并刷新几何
        /// </summary>
        /// <param name="axMapControl"></param>
        /// <param name="subLayerName"></param>
        /// <param name="geometris"></param>
        public static void DispalyGeometries(AxMapControl axMapControl, string subLayerName, params IGeometry[] geometris)
        {
            IGraphicsLayer sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer(subLayerName, axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            var col = new ElementCollectionClass();
            geometris.ToList().ForEach(
                item =>
                {
                    var element = IElementHelper.CreateElement(item);
                    if (element != null)
                        col.Add(element, -1);
                }
                );
            gc.AddElements(col, 0);
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        /// <summary>
        /// 向地图上绘制要素
        /// </summary>
        /// <param name="element"></param>
        public static void Display(AxMapControl axMapControl, string subLayerName, IElement element)
        {
            IGraphicsLayer sublayer;
            sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer(subLayerName, axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            gc.AddElement(element, 0);
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        /// <summary>
        /// 向地图上绘制要素
        /// </summary>
        /// <param name="element"></param>
        public static void Display(AxMapControl axMapControl, string subLayerName, IElementCollection elements)
        {
            IGraphicsLayer sublayer;
            sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer(subLayerName, axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            gc.AddElements(elements, 0);
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 查询GraphicsLayer中的要素
        /// </summary>
        /// <param name="axMapControl"></param>
        /// <param name="subLayerName"></param>
        /// <returns></returns>
        public static IGeometry[] GetGeometries(AxMapControl axMapControl, string subLayerName)
        {
            IGraphicsLayer sublayer;
            sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer(subLayerName, axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            var list = new List<IGeometry>();
            var geo=gc.Next();
            while (geo != null)
            {
                list.Add(geo.Geometry);
                geo = gc.Next();
            }
            return list.ToArray();
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="axMapControl"></param>
        public static void Clear(AxMapControl axMapControl)
        {
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)axMapControl.ActiveView;
            pGraphicsContainer.DeleteAllElements();
            axMapControl.Map.ClearSelection();
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, axMapControl.ActiveView.Extent);
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, axMapControl.ActiveView.Extent);
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl.ActiveView.Extent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axMapControl"></param>
        /// <param name="subLayerName"></param>
        public static void ClearDisplay(AxMapControl axMapControl,string subLayerName )
        {
            IGraphicsLayer sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer(subLayerName, axMapControl.Map);
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//          
            gc.DeleteAllElements();
            axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, axMapControl.ActiveView.Extent);
        }
    }
}
