using ArcengineHelper.DisplayHelper;
using ArcengineHelper.Entity;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
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
    public class MapDisplay
    {
        #region 私有成员
        private AxMapControl _axMapControl;
        private IElement _selectedElement;
        private IGeometry _selectedGeometry;
        #endregion

        public MapDisplay(AxMapControl axMapControl)
        {
            Initialize(axMapControl);
        }
        private void Initialize(AxMapControl axMapControl)
        {
            _axMapControl=axMapControl;
        }

        public void DispalyGeometries(string subLayerName,params IGeometry[] geometris)
        {
            IGraphicsLayer sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer(subLayerName, _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            var col = new ElementCollectionClass();
            geometris.ToList().ForEach(
                item => { 
                    var element=IElementHelper.CreateElement(item);
                    if(element!=null)
                        col.Add(element,-1);
                }
                );
            gc.AddElements(col, 0);
            _axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        /// <summary>
        /// 向地图上绘制要素
        /// </summary>
        /// <param name="element"></param>
        public void Display(string subLayerName, IElement element)
        {
            IGraphicsLayer sublayer;
            sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer("选择", _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            gc.AddElement(element, 0);
            _axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        /// <summary>
        /// 向地图上绘制要素
        /// </summary>
        /// <param name="element"></param>
        public void Display(string subLayerName,IElementCollection elements)
        {
            IGraphicsLayer sublayer;
            sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer("选择", _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            gc.AddElements(elements, 0);
            _axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        /// <summary>
        /// 向地图绘制展示要素
        /// </summary>
        /// <param name="displayEntity"></param>
        public void Display(IDisplayEntity displayEntity)
        {
            IGraphicsLayer sublayer ;
            if (displayEntity is SelectDisplayEntity)
                sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer("选择", _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            else if(displayEntity is ElementDisplayEntity)
                sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer("绘图", _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            else if (displayEntity is MarkDisplayEntity)
                sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer("标注", _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            else
                sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer("绘图", _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            if (displayEntity.IsClearCache)
                gc.DeleteAllElements();
            gc.AddElements(displayEntity.GetDisplayElements, 0);
            if (displayEntity.IsClearSelect)
            {
                var select = MapLayerHelper.FindOrCreateGraphicsSubLayer("选择", _axMapControl.Map);//返回的实际上是一个GraphicsSubLayer的实例对象
                (select as IGraphicsContainer).DeleteAllElements();
            }
            _axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);  
        }

        /// <summary>
        /// 清除绘图
        /// </summary>
        public void ClearDisplay()
        {
            _selectedElement = null;
            _selectedGeometry = null;

        }
        /// <summary>
        /// 清除选择
        /// </summary>
        public void ClearSelectDisplay()
        {
            IGraphicsLayer sublayer = MapLayerHelper.FindOrCreateGraphicsSubLayer("选择", _axMapControl.Map);
            IGraphicsContainer gc = sublayer as IGraphicsContainer;//这里之所以可以QI，是因为GraphicsSubLayer同时实现了IGraphicsLayer和IGraphicsContainer              
            gc.DeleteAllElements();
            _axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, _axMapControl.ActiveView.Extent);
            _axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, _axMapControl.ActiveView.FullExtent);
        }

        public void ClearMap()
        {
            MapHelper.ClearMapElement(_axMapControl);
        }
        /// <summary>
        /// 获取选择的绘图元素
        /// </summary>
        /// <returns></returns>
        public IElement GetSelectedElement()
        {
            return _selectedElement;
        }
        /// <summary>
        /// 获取选择的几何
        /// </summary>
        /// <returns></returns>
        public IGeometry GetSelectedIGeometry()
        {
            return _selectedGeometry;
        }

        /// <summary>
        /// 创建展示类
        /// </summary>
        /// <returns></returns>
        public IDisplayEntity CreateDisplay()
        {
            return new SelectDisplayEntity();
        }


    }
}
