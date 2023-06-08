using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using OGIS.Contracts;
using OGIS.Core;
using ESRI.ArcGIS.Carto;
using ArcengineHelper.MapHelper;
using ArcengineHelper.Entity;
using ESRI.ArcGIS.SystemUI;
namespace OGIS.UI
{
    public partial class RatioPointControl : UserControl
    {
        private AxMapControl _AxMapControl;
        private MapDisplay _mapDisPaly;
        private BindingList<RowModel> _ds = new BindingList<RowModel>();


        private IPoint _p, _q, _o;
        private IPolyline _pq, _op, _oq;
        private bool mapClickP = false;
        private bool mapClickQ = false;
        private int btnIdx = 0;

        private IGeodeticSolution _GeodeticSolution;
        private IGeometrySearch _GeometrySearch;
        private IGeodicSolutionFactory _GeodicSolutionFactory;
        private IMeasure _Measure;
        private IRatioPoint _RatioPoint;


        public RatioPointControl()
        {
            InitializeComponent();
            txtRatio.RatioChanged += txtRatio_TextChanged;
        }

        public void LoadMap(AxMapControl axMapControl)
        {
            _AxMapControl = axMapControl;
            _AxMapControl.OnMouseDown += axMapCtrl_OnMouseDown;
            btnIdx = 0;

            dgvDetails.DataSource = _ds;
            Reset();
            _GeodicSolutionFactory = new GeodicSolutionFactory();
            _GeodeticSolution = _GeodicSolutionFactory.Create("vincenty");
            _Measure = new Measure();
            _Measure.SetGeodeticSolutionType(_GeodeticSolution);
            _GeometrySearch = IGeometrySeachProvider.Instance;
            _RatioPoint = new RatioPoint();
            _RatioPoint.SetGeodeticSolutionType(_GeodeticSolution);
        }

        public void Reset()
        {
            //Reset page
            lblDistance.Text = "Distance:";
           
            _p = null; _q = null; _o = null;
            _pq = null; _op = null; _oq = null;
            mapClickP = false;
            mapClickQ = false;
            btnIdx = 0;
            txt_start_lng.Text = "";
            txt_start_lat.Text = "";
            txt_end_lng.Text = "";
            txt_end_lat.Text = "";
            txtDPointX.Text = "";
            txtDPointY.Text = "";
            txt_pq_length.Text = "";
            txtOmLon.Text = "";
            txtOmLat.Text = "";
            txtOnlon.Text = "";
            txtOnlat.Text = "";
        }

        private void initialize()
        {
            if (txtRatio.Ratio == 1)
            {
                if (cmbAlgorithm.Items.Count > 2)
                    cmbAlgorithm.Items.RemoveAt(2);
            }
            else
            {
                if (cmbAlgorithm.Items.Count == 2)
                    cmbAlgorithm.Items.Add("Circle method");
            }
        }

        private void CalcultePQ()
        {
            IPoint pMarker = new PointClass();
            pMarker.X = double.Parse(txt_start_lng.Text);
            pMarker.Y = double.Parse(txt_start_lat.Text);

            IPoint qMarker = new PointClass();
            qMarker.X = double.Parse(txt_end_lng.Text);
            qMarker.Y = double.Parse(txt_end_lat.Text);

            _GeodeticSolution = _GeodicSolutionFactory.Create(cmbSelectType.Text);
            _Measure.SetGeodeticSolutionType(_GeodeticSolution);
            var length = _Measure.CalculateLength(pMarker, qMarker);
            var line = GeometryHelper.ConstructGeodeticLine(pMarker, qMarker, null, _GeodeticSolution);
            _pq = line;
            //_gdbManager.AddSingleGeomFeatureToLayar("绘图线", line, "", 1);
            //_AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            SucceedOption(line, false, "绘图");

            double dbRatio = 1;
            if (!txtRatio.Validable)
            {
                MessageBox.Show("The ratio is error!");
                return;
            }
            dbRatio = txtRatio.Ratio;

            double ratioLength;
           
            _RatioPoint.SetGeodeticSolutionType(_GeodeticSolution);
            var omPoint = _RatioPoint.FindPointBy2Point(pMarker, qMarker, true, dbRatio, out ratioLength);
            var omLength = _Measure.CalculateLength(omPoint, pMarker);
             
            txt_pq_length.Text = (length / ConstantValue.Nm).ToString();

            txtOmLon.Text = omPoint.X.ToString();
            txtOmLat.Text = omPoint.Y.ToString();
            txtOpMin.Text = (length / ConstantValue.Nm * dbRatio / (1 + dbRatio)).ToString();
        }

        private bool DoCircle(IPoint pPoint, IPoint qPoint, IPoint sPoint, double ratio, double ds, double dbStep, double dbOpmin, double dbOpMax, double ratioA, double ratioB)
        {
            //Circle centry
            double dbLengthPQ, angle12, angle21;
            double centerX, centerY, centerLength, angleC;
            double r;
            if (ratio > 1)
            {
                _GeodeticSolution.SecondSubject(pPoint.X, pPoint.Y, qPoint.X, qPoint.Y, out dbLengthPQ, out angle12, out angle21);
                centerLength = dbLengthPQ * ratio * ratio / (ratio * ratio - 1);
                r = dbLengthPQ * ratio / (ratio * ratio - 1);
                _GeodeticSolution.FirstSubject(pPoint.X, pPoint.Y, angle12, centerLength, out centerX, out centerY, out angleC);
            }
            else
            {
                _GeodeticSolution.SecondSubject(qPoint.X, qPoint.Y, pPoint.X, pPoint.Y, out dbLengthPQ, out angle21, out angle12);
                centerLength = dbLengthPQ / (1 - ratio * ratio);
                r = dbLengthPQ * ratio / (1.0 - ratio * ratio);
                _GeodeticSolution.FirstSubject(qPoint.X, qPoint.Y, angle21, centerLength, out centerX, out centerY, out angleC); ;
            }
            double cpLength, cpAngle12, cpAngle21;
            double cqLength, cqAngle12, cqAngle21;
            _GeodeticSolution.SecondSubject(centerX, centerY, pPoint.X, pPoint.Y, out cpLength, out cpAngle12, out cpAngle21);
            _GeodeticSolution.SecondSubject(centerX, centerY, qPoint.X, qPoint.Y, out cqLength, out cqAngle12, out cqAngle21);

            int count = 0;
            double stepAngle = 180 * ConstantValue.Nm * dbStep / ConstantValue.Pai / r;
            double angle = cpAngle12;
            double oLon, oLat, angleOC;
            double angle180 = GeometryHelper.GetDirectionAngle(cqAngle12, 180);
            do
            {
                _GeodeticSolution.FirstSubject(centerX, centerY, angle, r, out oLon, out oLat, out angleOC);
                PointClass oPoint = new PointClass();
                oPoint.PutCoords(oLon, oLat);
                if (DoOkCircleLine(pPoint, qPoint, oPoint, ratio, ratioA, ratioB))
                {
                    drawGraphic(_p, _q, oPoint, false);
                }
                if (count > 1 && Math.Abs(cpAngle12 - angle) < stepAngle)
                {
                    angle = cqAngle12;
                }
                else if (count * stepAngle < 180 && Math.Abs(count * stepAngle - 180) < stepAngle && angle != angle180)
                {
                    angle = angle180;
                }
                else
                    angle = GeometryHelper.GetDirectionAngle(angle, stepAngle);
                count++;
            }
            while (Math.Abs(cqAngle12 - angle) > 0.0000001);

            _GeodeticSolution.FirstSubject(centerX, centerY, cqAngle12, r, out oLon, out oLat, out angleOC);
            PointClass o1Point = new PointClass();
            o1Point.PutCoords(oLon, oLat);
            if (DoOkCircleLine(pPoint, qPoint, o1Point, ratio, ratioA, ratioB))
            {
                drawGraphic(_p, _q, o1Point, false);
            }
            return true;
        }
        /// <summary>
        /// Pendulum method
        /// </summary>
        /// <param name="pPoint"></param>
        /// <param name="qPoint"></param>
        /// <param name="sPoint"></param>
        /// <param name="ratio"></param>
        /// <param name="ds"></param>
        /// <param name="ratioA"></param>
        /// <param name="ratioB"></param>
        /// <returns></returns>
        bool DoOkLine(IPoint pPoint, IPoint qPoint, IPoint sPoint, double ratio, double ds, double ratioA, double ratioB)
        {
            var lnglats = new double[3][];
            lnglats[0] = new double[2] { pPoint.X, pPoint.Y };
            lnglats[1] = new double[2] { qPoint.X, qPoint.Y };
            lnglats[2] = new double[2] { sPoint.X, sPoint.Y };

            double[,] resultCoord;
            double distanceTA, distanceTB;
            //0.00001,
            _GeometrySearch.SearchPointRatio2Point(lnglats, ds, ratio, 1, 0.00001, _GeodeticSolution, out resultCoord, out distanceTA, out distanceTB);
            if (resultCoord != null && !double.IsNaN(resultCoord[0, 1]) && !double.IsNaN(resultCoord[0, 1]))
            {
                IPoint pPT = new PointClass();
                pPT.X = resultCoord[0, 0];
                pPT.Y = resultCoord[0, 1];

                double dbOp, angleOp, anglePo;
                double dbOq, angleOq, angleQo;
                _GeodeticSolution.SecondSubject(pPoint.X, pPoint.Y, resultCoord[0, 0], pPT.Y = resultCoord[0, 1], out dbOp, out angleOp, out anglePo);
                _GeodeticSolution.SecondSubject(qPoint.X, qPoint.Y, resultCoord[0, 0], pPT.Y = resultCoord[0, 1], out dbOq, out angleOq, out angleQo);
                double op = dbOp / ConstantValue.Nm;
                double oq = dbOq / ConstantValue.Nm;

                double op1 = distanceTA / ConstantValue.Nm;
                double oq1 = distanceTB / ConstantValue.Nm;

                string[] tableList = new string[10];
                var rowM = new RowModel()
                {
                    index = _ds.Count + 1,
                    CalculateType = "Pendulum method",
                    PLon = pPoint.X,
                    PLat = pPoint.Y,
                    QLon = qPoint.X,
                    QLat = qPoint.Y,
                    Length = ds / ConstantValue.Nm,
                    OLon = pPT.X,
                    OLat = pPT.Y,
                    OPLength = op,
                    OPError = op * ConstantValue.Nm - ds,
                    OQLength = oq,
                    OQError = oq * ConstantValue.Nm * ratio - ds,
                    //OPQError = (op - oq * ratio) * ConstantValue.Nm,
                    OPQError = (op * ratioB - oq * ratioA) * ConstantValue.Nm,
                };
                _ds.Add(rowM);
                _p = pPoint;
                _q = qPoint;
                _o = pPT;
                return true;
            }
            else
            {
               // MessageBox.Show("The equiratio point isn't found!", "Tip");
               return false;
              //  return true;
            }
        }
        //Circle method
        bool DoOkCircleLine(IPoint pPoint, IPoint qPoint, IPoint circlePoint, double ratio, double ratioA, double ratioB)
        {
            if (!circlePoint.IsEmpty)
            {
                double dbOp, angleOp, anglePo;
                double dbOq, angleOq, angleQo;
                _GeodeticSolution.SecondSubject(pPoint.X, pPoint.Y, circlePoint.X, circlePoint.Y, out dbOp, out angleOp, out anglePo);
                _GeodeticSolution.SecondSubject(qPoint.X, qPoint.Y, circlePoint.X, circlePoint.Y, out dbOq, out angleOq, out angleQo);
                double op = dbOp / ConstantValue.Nm;
                double oq = dbOq / ConstantValue.Nm;

                string[] tableList = new string[10];
                var rowM = new RowModel()
                {
                    index = _ds.Count + 1,
                    CalculateType = "Circle method",
                    PLon = pPoint.X,
                    PLat = pPoint.Y,
                    QLon = qPoint.X,
                    QLat = qPoint.Y,
                    Length = 0,
                    OLon = circlePoint.X,
                    OLat = circlePoint.Y,
                    OPLength = op,
                    OPError = 0,
                    OQLength = oq,
                    OQError = 0,
                    //OPQError = (op - oq * ratio) * ConstantValue.Nm,
                    OPQError = (op * ratioB - oq * ratioA) * ConstantValue.Nm,
                };
                _ds.Add(rowM);
                _p = pPoint;
                _q = qPoint;
                _o = circlePoint;
                return true;
            }
            else
            {
                MessageBox.Show("Can't find the equiratio point!", "Tip");
                return false;
            }
        }

        public void drawGraphic(IPoint p, IPoint q, IPoint o, bool isRefresh)
        {
            try
            {
                
                SucceedOption(o);
             
                ISpatialReferenceFactory3 spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                ISpatialReference spatialReference = spatialReferenceFactory.CreateSpatialReference((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                p.SpatialReference = spatialReference;
                q.SpatialReference = spatialReference;
                o.SpatialReference = spatialReference;

                var line1 = GeometryHelper.ConstructGeodeticLine(p, q, spatialReference, _GeodeticSolution);
                _pq = line1;
                
                SucceedOption(line1);

                var line2 = GeometryHelper.ConstructGeodeticLine(o, p, spatialReference, _GeodeticSolution);
                _op = line2;
               

                var line3 = GeometryHelper.ConstructGeodeticLine(o, q, spatialReference, _GeodeticSolution);
                _oq = line3;
                

                if (isRefresh)
                    _AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SucceedOption(IGeometry geo, bool isClearCache = false, string lyrName = "绘图")
        {
            try
            {
                if (_mapDisPaly == null)
                    _mapDisPaly = new MapDisplay(_AxMapControl);
                //_mapDisPaly.SucceedOption(geo);
                var disp = new ElementDisplayEntity();
                disp.AddGeometry(geo);
                disp.DisplayType = 0;
                disp.IsClearCache = isClearCache;
                _mapDisPaly.Display(disp);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool GetHelpPoint(IPoint p, IPoint q, out IPoint helpPoint, out IPoint helpPoint2)
        {
            if (_GeodeticSolution == null)
                _GeodeticSolution = _GeodicSolutionFactory.Create(cmbSelectType.Text);
            double length, anglePQ, angleQP, L1, B1, L2, B2, anale1AB, angleBA, anale1AB2, angleBA2;
            _GeodeticSolution.SecondSubject(p.X, p.Y, q.X, q.Y, out length, out anglePQ, out angleQP);
            _GeodeticSolution.FirstSubject(p.X, p.Y, anglePQ - 60, length, out L1, out B1, out anale1AB);
            _GeodeticSolution.FirstSubject(p.X, p.Y, anglePQ + 60, length, out L2, out B2, out anale1AB2);
            helpPoint = new PointClass();
            helpPoint.PutCoords(L1, B1);
            helpPoint2 = new PointClass();
            helpPoint2.PutCoords(L2, B2);
            return true;
        }

        public void selectClear()
        {
            //Clear map element
            //GlobelCache.Instance.CurrentElement = null;
            //GlobelCache.Instance.DrawGeometryEntiry = null;
            //ElementCache.Instance.Clear();
            //Clear map selection
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)_AxMapControl.ActiveView;
            pGraphicsContainer.DeleteAllElements();
            _AxMapControl.Map.ClearSelection();
            _AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, _AxMapControl.ActiveView.Extent);
            _AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, _AxMapControl.ActiveView.Extent);
            _AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, _AxMapControl.ActiveView.Extent);
        }

        private bool ExportShps(ISpatialReference sp)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Title = "Save the Drawing Files";
            string strPath = "";
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                string strDestPath = saveFileDlg.FileName;
                strPath = System.IO.Path.GetDirectoryName(strDestPath);
                strPath = strPath + @"\" + System.IO.Path.GetFileName(saveFileDlg.FileName);
            }
            else
                return false;
            if (string.IsNullOrEmpty(strPath))
                return false;
            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);
            try
            {
                //Point P and Q
                var logic = ShpServiceProvider.Instance;
                var list = new List<IGeometry>();
                list.Add(_p);
                list.Add(_q);
                logic.ExportMultToShp(list, strPath, "pqPoint", sp);
                //Line PQ
                if (_pq != null)
                {
                    logic.ExportToShp(_pq, strPath, "pqLine", sp);
                }
                //Point O 
                var oList = new List<IGeometry>();
                foreach (var item in _ds)
                {
                    var opint = new PointClass();
                    opint.PutCoords(item.OLon, item.OLat);
                    oList.Add(opint);
                }
                logic.ExportMultToShp(oList, strPath, "oPoints", sp);
               
                //Eqiratio
                if (_ds.Count > 1)
                {
                    List<IPoint> oPoints = new List<IPoint>();
                    foreach (var item in _ds)
                    {
                        var opint = new PointClass();
                        opint.PutCoords(item.OLon, item.OLat);
                        oPoints.Add(opint);
                    }
                    var oLine = GeometryHelper.ConstructGeodeticLine(oPoints, null, _GeodeticSolution);
                    logic.ExportToShp(oLine, strPath, "oLine", sp);
                }

                //Line OP and OQ
                if (_ds.Count > 1)
                {
                    var opqList = new List<IGeometry>();
                    foreach (var item in _ds)
                    {
                        var opint = new PointClass();
                        opint.PutCoords(item.OLon, item.OLat);

                        var poqList = new List<IPoint>();
                        poqList.Add(_p);
                        poqList.Add(opint);
                        poqList.Add(_q);
                        var poqLine = GeometryHelper.ConstructGeodeticLine(poqList, null, _GeodeticSolution);
                        opqList.Add(poqLine);
                    }
                    logic.ExportMultToShp(opqList, strPath, "opqLine", sp);
                }

                WriteLog.Instance.WriteMsg(LogType.Info, "Export successed！");
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Instance.WriteMsg(LogType.Error, string.Format("error，method：{0}，error：{1}", "btnShpAll_Click", ex));
                return false;
            }
        }

        /// <summary>
        /// Map mousedown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapCtrl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            try
            {

                IPoint pPT = new PointClass();
                pPT.X = e.mapX;
                pPT.Y = e.mapY;
                if (btnIdx == 1)
                {
                    mapClickP = true;
                    txt_start_lng.Text = e.mapX.ToString();
                    txt_start_lat.Text = e.mapY.ToString();
                }
                else if (btnIdx == 2)
                {
                    mapClickQ = true;
                    txt_end_lng.Text = e.mapX.ToString();
                    txt_end_lat.Text = e.mapY.ToString();

                }
                else if (btnIdx == 3)
                {
                    txtDPointX.Text = e.mapX.ToString();
                    txtDPointY.Text = e.mapY.ToString();
                }
                if (btnIdx == 1 || btnIdx == 2)
                {
                    //_gdbManager.AddSingleGeomFeatureToLayar("绘图点", pPT, "", 0);
                    //_AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, pPT, null);
                    SucceedOption(pPT);
                }
                else if (btnIdx == 3)
                {
                    //_gdbManager.AddSingleGeomFeatureToLayar("绘图点", pPT, "", 0);
                    SucceedOption(pPT);
                }
                if (btnIdx == 0)
                    return;
                btnIdx = 0;
                //_AxMapControl.OnMouseDown -= axMapCtrl_OnMouseDown;
                if (!string.IsNullOrEmpty(txt_start_lng.Text) && !string.IsNullOrEmpty(txt_start_lat.Text) && !string.IsNullOrEmpty(txt_end_lng.Text) && !string.IsNullOrEmpty(txt_end_lat.Text))
                {
                    CalcultePQ();
                }
                //this.ShowDialog();
            }
            catch (Exception ex)
            {
                WriteLog.Instance.WriteMsg(LogType.Error, string.Format("error，method：{0}，error：{1}", "axMapCtrl_OnMouseDown", ex));
            }
        }





        private void btnClickP_Click(object sender, EventArgs e)
        {
            btnIdx = 1;
            
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapUpCommand();
            pCommand.OnCreate(_AxMapControl.Object);
            _AxMapControl.CurrentTool = pCommand as ITool;
        }

        private void btnClickQ_Click(object sender, EventArgs e)
        {
            btnIdx = 2;
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapUpCommand();
            pCommand.OnCreate(_AxMapControl.Object);
            _AxMapControl.CurrentTool = pCommand as ITool;
        }

        private void btnClickMap_Click(object sender, EventArgs e)
        {
            btnIdx = 3;
            ESRI.ArcGIS.SystemUI.ICommand pCommand;
            pCommand = new ESRI.ArcGIS.Controls.ControlsMapUpCommand();
            pCommand.OnCreate(_AxMapControl.Object);
            _AxMapControl.CurrentTool = pCommand as ITool;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            selectClear();
            IPoint pPoint, qPoint, sPoint;
            if (string.IsNullOrEmpty(txtDPointX.Text) || string.IsNullOrEmpty(txtDPointY.Text)
                || string.IsNullOrEmpty(txt_start_lng.Text) || string.IsNullOrEmpty(txt_start_lat.Text)
                || string.IsNullOrEmpty(txt_end_lng.Text) || string.IsNullOrEmpty(txt_end_lat.Text)
                || string.IsNullOrEmpty(tbxDistance.Text) || string.IsNullOrEmpty(cmbSelectType.Text))
            {
                MessageBox.Show("请确认是否有未填项", "Tip");
                return;
            }

            // 如果是手动输入而非地图点选时，点击确认按钮新增PQ两点
            IPoint pMarker = new PointClass();
            pMarker.X = double.Parse(txt_start_lng.Text);
            pMarker.Y = double.Parse(txt_start_lat.Text);

            IPoint qMarker = new PointClass();
            qMarker.X = double.Parse(txt_end_lng.Text);
            qMarker.Y = double.Parse(txt_end_lat.Text);

            if (!mapClickP)
            {
                //_gdbManager.AddSingleGeomFeatureToLayar("绘图点", pMarker, "", 0);
                //_AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, pMarker, null);
                SucceedOption(pMarker);
            }

            if (!mapClickQ)
            {
                //_gdbManager.AddSingleGeomFeatureToLayar("绘图点", qMarker, "", 0);
                //_AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, qMarker, null);
                SucceedOption(qMarker);
            }


            _GeodeticSolution = _GeodicSolutionFactory.Create(cmbSelectType.Text);
            //_geometryToloty = new Richway.Ocean.BL.Implement.GeometryTopology();
            //_geometryToloty.SetGeodeticSolutionType(_GeodeticSolution);

            pPoint = new PointClass();
            pPoint.PutCoords(double.Parse(txt_start_lng.Text), double.Parse(txt_start_lat.Text));
            qPoint = new PointClass();
            qPoint.PutCoords(double.Parse(txt_end_lng.Text), double.Parse(txt_end_lat.Text));

            double ds = double.Parse(tbxDistance.Text);//n mile
            double dbOpmin, dbOpMax;
            dbOpmin = double.Parse(txtOpMin.Text);
            dbOpMax = double.Parse(txtOpMax.Text);
            double ratio = txtRatio.Ratio;
            double ratioA = txtRatio.RatioNumerator;
            double ratioB = txtRatio.RatioDenominator;
            if (dbOpmin > dbOpMax )
            {
                MessageBox.Show(" The minimun distance of OP should be less than the maximum distance.");
                return;
            }

            //Generated the auxiliary points
           
            IPoint hPoint1, hPoint2;
            GetHelpPoint(pMarker, qMarker, out hPoint1, out hPoint2);
            //_gdbManager.AddSingleGeomFeatureToLayar("绘图点", hPoint1, "", 0);
            //_gdbManager.AddSingleGeomFeatureToLayar("绘图点", hPoint2, "", 0);
            try
            {
               // if (chkAutoOKAll.Checked)
                //{
                    _ds.Clear();
                    double dbStep = double.Parse(tbxDistance.Text);
                    if (cmbAlgorithm.Text == "Circle Method")
                    {
                        sPoint = hPoint1;
                        DoCircle(pPoint, qPoint, sPoint, ratio, ds * ConstantValue.Nm, dbStep, dbOpmin, dbOpMax, ratioA, ratioB);
                    }
                    else
                    {
                        #region "while left"
                        ds = dbOpmin;
                        int count = 0;
                        while (ds >= dbOpmin || ds <= dbOpMax)
                        {
                            switch (cmbAlgorithm.Text)
                            {
                                case "Pendulum method":
                                    // S点(Auxiliary point)
                                    sPoint = hPoint1;
                                    //sPoint = new PointClass();
                                    //sPoint.PutCoords(double.Parse(txtDPointX.Text), double.Parse(txtDPointY.Text));
                                    if (DoOkLine(pPoint, qPoint, sPoint, ratio, ds * ConstantValue.Nm, ratioA, ratioB))
                                    {
                                        drawGraphic(_p, _q, _o, false);
                                    }
                                    break;
                                    //case "Circle intersection":
                                    //    if (DoOkArc(pPoint, qPoint, null, ratio, ds, ratioA, ratioB))
                                    //    {
                                    //        drawGraphic(_p, _q, _o, false);
                                    //    }
                                    //    break;
                            }
                            count++;
                            if (ds == dbOpMax)
                            {
                                break;
                            }
                            ds = Math.Sqrt(dbOpmin * dbOpmin + count * count * dbStep * dbStep);
                            if (ds >= dbOpMax)
                            {
                                ds = dbOpMax;
                            }

                        }
                        #endregion
                        var hDs = _ds.Reverse().ToList();
                        hDs.RemoveAt(hDs.Count - 1);
                        _ds.Clear();
                        hDs.ForEach(item => _ds.Add(item));
                        #region "while right"
                        ds = dbOpmin;
                        count = 0;
                        while (ds >= dbOpmin || ds <= dbOpMax)
                        {
                            switch (cmbAlgorithm.Text)
                            {
                                case "Pendulum method":
                                    // S点(Auxiliary point)
                                    sPoint = hPoint2;
                                    //sPoint = new PointClass();
                                    //sPoint.PutCoords(double.Parse(txtDPointX.Text), double.Parse(txtDPointY.Text));
                                    if (DoOkLine(pPoint, qPoint, sPoint, ratio, ds * ConstantValue.Nm, ratioA, ratioB))
                                    {
                                        drawGraphic(_p, _q, _o, false);
                                    }
                                    break;
                                    //case "Circle intersection":
                                    //    if (DoOkArc(pPoint, qPoint, null, ratio, ds, ratioA, ratioB))
                                    //    {
                                    //        drawGraphic(_p, _q, _o, false);
                                    //    }
                                    //    break;
                            }
                            count++;
                            if (ds == dbOpMax)
                            {
                                break;
                            }
                            ds = Math.Sqrt(dbOpmin * dbOpmin + count * count * dbStep * dbStep);
                            if (ds >= dbOpMax)
                            {
                                ds = dbOpMax;
                            }

                        }
                        #endregion
                    }
                    //绘制 equiratio Line
                    if (_ds.Count >= 2)
                    {
                        List<IPoint> oPoints = new List<IPoint>();
                        foreach (var item in _ds)
                        {
                            var opint = new PointClass();
                            opint.PutCoords(item.OLon, item.OLat);
                            oPoints.Add(opint);
                        }
                        var oLine = GeometryHelper.ConstructGeodeticLine(oPoints, null, _GeodeticSolution);
                        //_gdbManager.AddSingleGeomFeatureToLayar("绘图线", oLine, "", 1);
                        SucceedOption(oLine);
                    }
                    _AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
              /*  else
                {
                    switch (cmbAlgorithm.Text)
                    {
                        case "Pendulum method":
                            // S点(辅助点)
                            sPoint = new PointClass();
                            sPoint.PutCoords(double.Parse(txtDPointX.Text), double.Parse(txtDPointY.Text));
                            ds = double.Parse(tbxDistance.Text) * ConstantValue.Nm;
                            if (DoOkLine(pPoint, qPoint, sPoint, 1, ds, ratioA, ratioB))
                            {
                                drawGraphic(_p, _q, _o, true);
                            }
                            break;
                            //case "圆弧求交法":
                            //    ds = tbxDistance.Text != "" ? double.Parse(tbxDistance.Text) : 60;
                            //    if (DoOkArc(pPoint, qPoint, null, ratio, ds, ratioA, ratioB))
                            //    {
                            //        drawGraphic(_p, _q, _o, true);
                            //    }
                            //    break;
                    }
                }
            } */
            catch (Exception ex)
            {
                //WriteLog.Instance.WriteErrMsg("计算出错", ex);
            }

             
            mapClickP = false;
            mapClickQ = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            selectClear();
            Reset();
            //this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (_ds != null)
                _ds.Clear();
        }

        private void btnExportShp_Click(object sender, EventArgs e)
        {
            var sp = _AxMapControl.Map.SpatialReference != null ? _AxMapControl.Map.SpatialReference : SpatialReferenceHelper.CreateISpatialReference(esriSRGeoCSType.esriSRGeoCS_WGS1984);
            ExportShps(sp);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ds.Count() == 0)
                    return;
                var logic = new ExcelService();
                var dt = new DataTable();
                dt.Columns.Add("Num");
                dt.Columns.Add("Method");
                dt.Columns.Add("P Long");
                dt.Columns.Add("P Lat");
                dt.Columns.Add("Q Long");
                dt.Columns.Add("Q Lat");
                dt.Columns.Add("Distance (n mile)");
                dt.Columns.Add("O Long");
                dt.Columns.Add("O Lat");
                dt.Columns.Add("Distance of OP (n mile)");
                dt.Columns.Add("Distance of OQ (n mile)");
                dt.Columns.Add("Error of OP (m)");
                dt.Columns.Add("Error of OQ (m)");
                dt.Columns.Add("OP-OQ(m)");
                foreach (var item in _ds)
                {
                    var dr = dt.NewRow();
                    dr["Method"] = item.CalculateType;
                    dr["P Long"] = item.PLon;
                    dr["P Lat"] = item.PLat;
                    dr["Q Long"] = item.QLon;
                    dr["Q Lat"] = item.QLat;
                    dr["Distance (n mile)"] = item.Length;
                    dr["O Long"] = item.OLon;
                    dr["O Lat"] = item.OLat;
                    dr["Distance of OP (n mile)"] = item.OPLength;
                    dr["Distance of OQ (n mile)"] = item.OQLength;
                    dr["Error of OP (m)"] = item.OPError.ToString("f6");
                    dr["Error of OQ (m)"] = item.OQError.ToString("f6");
                    dr["OP-OQ(m)"] = item.OPQError.ToString("f6");
                    dt.Rows.Add(dr);
                }
                logic.ExportToExcelNPOI(dt);
                MessageBox.Show("Export successed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed!");
            }
        }

        private void cmbSelectType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtRatio_TextChanged(object sender, EventArgs e)
        {
            initialize();

            if (!txtRatio.Validable)
            {
                MessageBox.Show("ratio error!");
                return;
            }
            double dbRatio = txtRatio.Ratio;
            double length;
            if (!double.TryParse(txt_pq_length.Text, out length))
            {
                return;
            }
            txtOpMin.Text = (length * dbRatio / (1 + dbRatio)).ToString();
            if (dbRatio == 1)
            {
                txtOpMax.Text = "200";
            }
            else if (dbRatio > 1)
            {
                txtOpMax.Text = (dbRatio * length / (dbRatio - 1)).ToString();
            }
            else if (dbRatio < 1)
            {
                txtOpMax.Text = (dbRatio * length / (1 - dbRatio)).ToString();
            }
        }
      
        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAlgorithm.Text != "Pendulum method")
            {
                label12.Visible = true;
                cmbSelectType.Visible = true;
            }
            else
            {
                label12.Visible = true;
                cmbSelectType.Visible = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
