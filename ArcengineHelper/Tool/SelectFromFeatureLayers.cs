using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2020-03-10
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcengineHelper.Tool
{
    public class SelectFromFeatureLayers : ESRI.ArcGIS.ADF.BaseClasses.BaseTool
    {
        private IHookHelper m_hookHelper;
        private List<IFeatureLayer> _layerList;
        private string _selectType = "rect";
        private IPoint _lastPoint;
        private double m_distance = 10;

        private INewRectangleFeedback _newRectFeedBack;
        private INewLineFeedback _newLineFeedBack;
        private INewPolygonFeedback _newPolygonFeedback;

        private IGeometry _drawGeometry;
        private List<IGeometry> _selectedGeometry;


        public event Action<IGeometry[]> GetSelectedGeometry;

        public void SetLayers(List<IFeatureLayer> list)
        {
            _layerList = list;
        }

        #region 重载基类方法

        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)m_hookHelper.ActiveView;
            m_hookHelper.FocusMap.ClearSelection();
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, m_hookHelper.ActiveView.Extent);
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_hookHelper.ActiveView.Extent);
        }

        public override void OnClick()
        {
            // TODO: Add StationTool.OnClick implementation

        }

        public override void OnDblClick()
        {           
            if (_selectType == "polyline")
            {
                if (_newLineFeedBack != null)
                {
                    _drawGeometry=_newLineFeedBack.Stop();
                    ClearDraw();
                    GetFeature(_drawGeometry,-1);
                }
                return;
            }

            if (_selectType == "polygon")
            {
                if (_newPolygonFeedback != null)
                {
                    _drawGeometry=_newPolygonFeedback.Stop();
                    ClearDraw();
                    GetFeature(_drawGeometry,-1);
                }
                return;
            }
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            base.OnMouseDown(Button, Shift, X, Y);
            // TODO:  Add StationTool.OnMouseDown implementation
            if (Button == 1)
            {
                var point = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);


                if (_selectType == "rect")//矩形
                {
                    if (_newRectFeedBack == null)
                    {
                        _newRectFeedBack = new NewRectangleFeedbackClass();
                        _newRectFeedBack.Display = m_hookHelper.ActiveView.ScreenDisplay;
                        _newRectFeedBack.Start(point);
                        _newRectFeedBack.IsEnvelope = true;
                    }
                    else
                    {
                        //if (_lastPoint != null && _lastPoint.X == point.X && _lastPoint.Y == point.Y)//双击点的同一个点
                        _drawGeometry=_newRectFeedBack.Stop(point);
                        ClearDraw();
                        GetFeature(_drawGeometry, Shift);
                    }
                    return;
                }

                if (_selectType == "polyline")
                {
                    if (_newLineFeedBack == null)
                    {
                        _newLineFeedBack = new NewLineFeedbackClass();
                        _newLineFeedBack.Display = m_hookHelper.ActiveView.ScreenDisplay;
                        _newLineFeedBack.Start(point);
                    }
                    else
                    {
                        _newLineFeedBack.AddPoint(point);
                    }
                    return;
                }

                if (_selectType == "polygon")
                {
                    if (_newPolygonFeedback == null)
                    {
                        _newPolygonFeedback = new NewPolygonFeedbackClass();
                        _newPolygonFeedback.Display = m_hookHelper.ActiveView.ScreenDisplay;
                        _newPolygonFeedback.Start(point);
                    }
                    else
                    {
                        _newPolygonFeedback.AddPoint(point);
                    }
                    return;
                }
            }

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            var point = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (_selectType == "rect")//矩形
            {
                if (_newRectFeedBack != null)
                {
                    _newRectFeedBack.MoveTo(point);
                }
                return;
            }

            if (_selectType == "polyline")
            {
                if (_newLineFeedBack != null)
                {
                    _newLineFeedBack.MoveTo(point);
                }
                return;
            }

            if (_selectType == "polygon")
            {
                if (_newPolygonFeedback != null)
                {
                    _newPolygonFeedback.MoveTo(point);
                }
                return;
            }

        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add StationTool.OnMouseUp implementation
            _lastPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (_selectType == "point")
            {
                var pTopo = _lastPoint as ITopologicalOperator;
                m_distance = m_hookHelper.ActiveView.Extent.Width / 100.0;
                var pBuffer = pTopo.Buffer(m_distance);
                GetFeature(pBuffer, Shift);
            }
        }

        #endregion
        #region 查询相关方法
        public void Display(params IGeometry[] geos)
        {
            if (geos == null) return;
        }

        public IGeometry[] GetFeature(IGeometry sourceGeo, int Shift)
        {
            if (sourceGeo == null) return null;
            _selectedGeometry = null;
            _layerList.ForEach(layer =>
            {
                SelectFeaturesByLayer(sourceGeo, layer, Shift);
            });
            sourceGeo = null;
            if (GetSelectedGeometry != null)
                GetSelectedGeometry(SelectedFeature);
            return null;
        }

        public IGeometry[] SelectedFeature
        {
            get
            {
                return _selectedGeometry == null?null:_selectedGeometry.ToArray();
            }
        }

        ///<summary>
        /// 按图层选择要素
        /// </summary>
        /// <param name="pGeometry">筛选几何图形</param>
        /// <param name="pFeatureLayer">筛选图层</param>
        /// <param name="shift">Shift键</param>
        private void SelectFeaturesByLayer(IGeometry pGeometry, IFeatureLayer pFeatureLayer, int shift)
        {
            if (pFeatureLayer == null) return;
            // 空间分析
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //var type = pFeatureLayer.FeatureClass.ShapeType;
            //switch (type)
            //{
            //    case esriGeometryType.esriGeometryPoint:
            //        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            //        break;
            //    case esriGeometryType.esriGeometryPolyline:
            //        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
            //        break;
            //    case esriGeometryType.esriGeometryPolygon:
            //        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //        break;
            //    default:
            //         pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
            //        break;
            //}  
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.GeometryField = pFeatureLayer.FeatureClass.ShapeFieldName;

            // 添加选择要素
            esriSelectionResultEnum pResultEnum;
            if (shift == 0)
            {
                pResultEnum = esriSelectionResultEnum.esriSelectionResultNew;
                //m_hookHelper.FocusMap.ClearSelection();
            }
            else
                pResultEnum = esriSelectionResultEnum.esriSelectionResultXOR;
            IFeatureSelection pFSelection = pFeatureLayer as IFeatureSelection;
            pFSelection.SelectionColor = ArcengineHelper.DisplayHelper.IColorHelper.GetRgbColor(255,0,0);
            pFSelection.SelectFeatures(pSpatialFilter, pResultEnum, false);

            Dispaly(pFeatureLayer);
            
            //IQueryFilter pFilter = pSpatialFilter;
            //IFeatureCursor pFeatCursor = pFeatureLayer.Search(pFilter, false);
            //IFeature pFeature = pFeatCursor.NextFeature();
            //while (pFeature != null)
            //{
            //    m_hookHelper.FocusMap.SelectFeature(pFeatureLayer, pFeature);
            //    pFeature = pFeatCursor.NextFeature();
            //}
            //m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, null);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pSpatialFilter);
        }

        private void Dispaly(IFeatureLayer pFeatureLayer)
        {
            ISelectionSet selectSet = (pFeatureLayer as IFeatureSelection).SelectionSet;
            if (selectSet == null) return;
            ICursor pCursor;
            selectSet.Search(null, true, out pCursor);
            IFeatureCursor pFeatureCursor = pCursor as IFeatureCursor;
            IFeature pFeature = pFeatureCursor.NextFeature();
            if (_selectedGeometry == null)
                _selectedGeometry = new List<IGeometry>();
            while (pFeature != null)
            {
                m_hookHelper.FocusMap.SelectFeature(pFeatureLayer, pFeature);
                _selectedGeometry.Add(pFeature.ShapeCopy);
                pFeature=pFeatureCursor.NextFeature();
            }
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_hookHelper.ActiveView.FullExtent);
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, m_hookHelper.ActiveView.FullExtent);
        }

        private void ClearDraw()
        {
            _newRectFeedBack = null;
            _newLineFeedBack = null;
            _newPolygonFeedback = null;
        }

        #endregion
    }
}
