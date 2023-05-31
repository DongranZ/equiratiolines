using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using OGIS.Contracts;
using OGIS.UI.ArcgisMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    /// <summary>
    /// 地图逻辑类：方法
    /// 公用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class MapBll<T> : IBaseMapView<T> where T : System.Windows.Forms.Form
    {
        protected CurrentOperate _CurrentOperate = CurrentOperate.OptionNull;

        #region control成员变量
        protected internal ESRI.ArcGIS.Controls.AxMapControl axMapControl;
        protected internal ESRI.ArcGIS.Controls.AxTOCControl axTOCControl;
        protected internal T _parentForm;
        #endregion
        #region 逻辑组件
        protected ArcengineHelper.MapHelper.MapDisplay _MapDisplay;
        protected IOMapDocument _MapDocument;
        #endregion

        #region 事件
        public event Action<string, int, CurrentOperate> SetCurrentOpterate;
        #endregion

        #region 属性
        public IOMapDocument MapDocumenHelper { get { return _MapDocument; } }
        #endregion


        /// <summary>
        /// 实例化地图控件；加载地图控件到主窗体界面；
        /// </summary>
        /// <param name="father"></param>
        public void NewMap(T father)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OGIS.UI.MainForm));
            axMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            axTOCControl = new ESRI.ArcGIS.Controls.AxTOCControl();

            _parentForm = father;

            //// 
            //// axMapControl1
            //// 
            //this.axMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.axMapControl.Location = new System.Drawing.Point(0, 0);
            this.axMapControl.Name = "axMapControl";
            this.axMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl.OcxState")));
            this.axMapControl.Size = new System.Drawing.Size(1461, 779);
            this.axMapControl.OnMouseDown += this.axMapCtrl_OnMouseDown;
            this.axMapControl.OnMouseMove += this.axMapCtrl_OnMouseMove;
            this.axMapControl.OnMouseUp += this.axMapCtrl_OnMouseUp;
            this.axMapControl.OnDoubleClick += this.axMapCtrl_OnDoubleClick;
            this.axMapControl.OnExtentUpdated += this.axMapCtrl_OnExtentUpdated;
            //this.axMapControl.TabIndex = 0;

            axTOCControl.Enabled = true;
            axTOCControl.Name = "axTOCControl";
            this.axTOCControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl.OcxState")));
            this.axTOCControl.OnMouseDown += this.axTocCtrl_OnMouseDown;
            this.axTOCControl.OnMouseUp += this.axTocCtrl_OnMouseUp;

            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).BeginInit();
            father.Controls.Add(axMapControl);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).BeginInit();
            father.Controls.Add(axTOCControl);

            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).EndInit();

            InitializeBll();
        }
        /// <summary>
        /// 初始化BLL逻辑类
        /// </summary>
        public virtual void InitializeBll()
        {
            _MapDisplay = new ArcengineHelper.MapHelper.MapDisplay(axMapControl); //_MapDisplay.Initialize(axMapControl);

            _MapDocument = new OMapDocument();
            _MapDocument.MapCtrl = axMapControl;
        }
        /// <summary>
        /// 图层操作控件与地图控件绑定
        /// </summary>
        /// <param name="ctrlMap"></param>
        /// <param name="ctrlleft"></param>
        public void Banding(System.Windows.Forms.Control ctrlMap, System.Windows.Forms.Control ctrlleft)
        {
            CommonFunc.Bingding(ctrlMap, axMapControl);
            CommonFunc.Bingding(ctrlleft, axTOCControl);
            axTOCControl.SetBuddyControl(axMapControl);
        }

        public virtual void LoadMap()
        {
        }

        public virtual void SetMapDoc(IOMapDocument mapDocument)
        {
            _MapDocument = mapDocument;
        }

        public abstract void SetCurrentOperate(CurrentOperate operate);

        #region 地图文档操作
        /// <summary>
        /// 新建地图
        /// </summary>
        public void NewMapDoc()
        {
            _MapDocument.NewMapDoc();
        }
        /// <summary>
        /// 打开地图
        /// </summary>
        public void OpenMapDocument()
        {
            _MapDocument.OpenMapDocument();
        }
        /// <summary>
        /// 保存地图
        /// </summary>
        public void SaveDocument()
        {
            _MapDocument.SaveDocument();
        }
        /// <summary>
        /// 另存地图
        /// </summary>
        public void SaveAsDocument()
        {
            _MapDocument.SaveAsDocument();
        }
        #endregion

        #region 地图放大缩小操作
        public void MapFull()
        {
            ArcengineHelper.MapHelper.MapFuncHelper.MapFull(axMapControl);
        }
        public void MapZoomIn()
        {
            ArcengineHelper.MapHelper.MapFuncHelper.MapZoomIn(axMapControl);
        }
        public void MapZoomOut()
        {
            ArcengineHelper.MapHelper.MapFuncHelper.MapZoomOut(axMapControl);
        }
        public void MapPan()
        {
            ArcengineHelper.MapHelper.MapFuncHelper.MapPan(axMapControl);
        }
        #endregion

        #region 地图数据操作
        public void ClearMap()
        {
            _MapDisplay.ClearDisplay();
            ArcengineHelper.MapHelper.MapHelper.ClearMapElement(axMapControl);
        }
        public void ClearData()
        {
            //清理临时图层
            _MapDisplay.ClearDisplay();
            ArcengineHelper.MapHelper.MapHelper.ClearMapElement(axMapControl);
        }
        public void DelelteElement()
        {
            axMapControl.Map.FeatureSelection.Clear();
            //删除对应图层
        }
        public void SelectElement()
        {
            var pCommand = new ESRI.ArcGIS.Controls.ControlsSelectToolClass();
            pCommand.OnCreate(axMapControl.Object);
            axMapControl.CurrentTool = pCommand as ESRI.ArcGIS.SystemUI.ITool;
        }
        public void NoFunction()
        {
            axMapControl.CurrentTool = null;
            axMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            _MapDisplay.ClearSelectDisplay();
        }
        #endregion
    }

    /// <summary>
    /// 地图逻辑类：地图事件处理
    /// 公用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class MapBll<T> : IBaseMapView<T> where T : System.Windows.Forms.Form
    {
        protected void axMapCtrl_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {

        }
        protected void axMapCtrl_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (SetCurrentOpterate != null)
            {
                IPoint pinPT = new PointClass();
                pinPT.PutCoords(e.mapX, e.mapY);
                pinPT.SpatialReference = axMapControl.SpatialReference;
                pinPT = ArcengineHelper.MapHelper.SpatialReferenceHelper.ConvertToIGeographicCoordinateSystem(pinPT) as IPoint;
                string x = ConvertHelper.ConvertDoubleToString(pinPT.X);
                string y = ConvertHelper.ConvertDoubleToString(pinPT.Y);
                string strCordinate = string.Format("鼠标位置：x={0}；y={1}", x, y);
                SetCurrentOpterate(strCordinate, 2, CurrentOperate.OptionNull);//显示当前坐标
            }
        }
        protected void axMapCtrl_OnMouseUp(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseUpEvent e)
        {
        }
        protected void axMapCtrl_OnDoubleClick(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnDoubleClickEvent e)
        {
        }
        protected void axMapCtrl_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            if (SetCurrentOpterate != null)
            {
                var nScale = ArcengineHelper.MapHelper.MapHelper.GetMapScaleRuler(axMapControl.ActiveView);
                var strScale = string.Format("当前比例尺: 1:{0}", nScale);
                SetCurrentOpterate(strScale, 1, CurrentOperate.OptionNull);//显示比例尺
            }
        }
    }
    /// <summary>
    /// axTocCtrl 相关逻辑
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class MapBll<T> : IBaseMapView<T> where T : System.Windows.Forms.Form
    {
        protected virtual void axTocCtrl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        { }

        protected virtual void axTocCtrl_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        { }
    }


    public abstract partial class MapBll<T> : IBaseMapView<T> where T : System.Windows.Forms.Form
    {
        public void DisplayGementry(string layer, IGeometry geometry)
        {
            this._MapDisplay.DispalyGeometries(layer, geometry);
        }

        public void DisplayGeometries(string layer, params IGeometry[] geometris)
        {
            this._MapDisplay.DispalyGeometries(layer, geometris);
        }

        public void DisplayElement(string layer, IElement element)
        {
            this._MapDisplay.Display(layer, element);
        }

        public void DisplayElements(string layer, IElementCollection elements)
        {
            this._MapDisplay.Display(layer, elements);
        }



        public void SaveLayerAndDisplay(string layer, params IGeometry[] geometris)
        {
            try
            {
                this._MapDisplay.DispalyGeometries(layer, geometris);
                //this._GdbManager.InsertMultGeometry(layer, geometris);
                this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }
}
