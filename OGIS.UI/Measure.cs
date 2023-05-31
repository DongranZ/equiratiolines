using ESRI.ArcGIS.Geometry;
using OGIS.Contracts;
using OGIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class Measure : IMeasure
    {
        /// <summary>
        /// 大地主题解算方法类
        /// </summary>
        private IGeodeticSolution _geodeticSolution = null;
        /// <summary>
        /// 设置椭球参数
        /// </summary>
        /// <param name="Type"></param>
        public void SetParameterType(int Type)
        {
            if (_geodeticSolution == null)
                _geodeticSolution = new GeodicSolutionFactory().Create("vincenty");
        }
        /// <summary>
        /// 设置 大地主题解算方法类
        /// </summary>
        /// <param name="iGeodeticSolution"></param>
        public void SetGeodeticSolutionType(IGeodeticSolution iGeodeticSolution)
        {
            _geodeticSolution = iGeodeticSolution;
        }

        public void FirstSubject(IPoint pointA, double angleAB, double length, out IPoint pointB, out double angleBA)
        {
            pointB = null;
            angleBA = 0;
            if (pointA == null || pointA.IsEmpty) return;
            double x, y;
            _geodeticSolution.FirstSubject(pointA.X, pointA.Y, angleAB, length, out x, out y, out angleBA);
            pointB = new PointClass();
            pointB.PutCoords(x, y);

        }

        /// <summary>
        /// 已知两点，求两点距离，方位角
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="Length"></param>
        /// <param name="angle12"></param>
        /// <param name="angle21"></param>
        public void CalculateLength(ESRI.ArcGIS.Geometry.IPoint point1, ESRI.ArcGIS.Geometry.IPoint point2, out double Length, out double angle12, out double angle21)
        {
            if (point1 == null || point2 == null)
            {
                Length = 0;
                angle12 = 0;
                angle21 = 0;
                return;
            }
            _geodeticSolution.SecondSubject(point1.X, point1.Y, point2.X, point2.Y, out Length, out angle12, out angle21);
        }
        /// <summary>
        /// 已知两点，求两点距离
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public double CalculateLength(ESRI.ArcGIS.Geometry.IPoint point1, ESRI.ArcGIS.Geometry.IPoint point2)
        {
            if (point1 == null || point2 == null) return 0;
            double length, angle12, angle21;
            _geodeticSolution.SecondSubject(point1.X, point1.Y, point2.X, point2.Y, out length, out angle12, out angle21);
            return length;
        }
        /// <summary>
        /// 已知两点经纬度坐标，求两点距离、方位角
        /// </summary>
        /// <param name="longitude1"></param>
        /// <param name="latitude1"></param>
        /// <param name="longitude2"></param>
        /// <param name="latitude2"></param>
        /// <param name="length"></param>
        /// <param name="angle12"></param>
        /// <param name="angle21"></param>
        public void CalculateLength(double longitude1, double latitude1, double longitude2, double latitude2, out double length, out double angle12, out double angle21)
        {
            _geodeticSolution.SecondSubject(longitude1, latitude1, longitude2, latitude2, out length, out angle12, out angle21);
        }
        /// <summary>
        /// 已知两点经纬度坐标，求两点距离
        /// </summary>
        /// <param name="longitude1"></param>
        /// <param name="latitude1"></param>
        /// <param name="longitude2"></param>
        /// <param name="latitude2"></param>
        /// <returns></returns>
        public double CalculateLength(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            double length, angle12, angle21;
            _geodeticSolution.SecondSubject(longitude1, latitude1, longitude2, latitude2, out length, out angle12, out angle21);
            return length;
        }


        /// <summary>
        /// 已知线，求线段长度
        /// </summary>
        /// <param name="line"></param>
        /// <param name="Length"></param>
        public void CalculateLength(ESRI.ArcGIS.Geometry.IPolyline line, out double Length)
        {
            Length = -1;
            try
            {
                IPoint pPT1;
                IPoint pPT2;
                if (line != null)
                {
                    IPointCollection pPtCol = new PolylineClass();
                    pPtCol = line as IPointCollection;
                    int nCnt = pPtCol.PointCount;
                    if (nCnt <= 1)
                    {
                        return;
                    }
                    else
                    {
                        for (int index = 0; index <= nCnt - 2; index++)
                        {
                            pPT1 = pPtCol.get_Point(index);
                            pPT2 = pPtCol.get_Point(index + 1);
                            Length = Length + CalculateLength(pPT1.X, pPT1.Y, pPT2.X, pPT2.Y);
                        }
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Instance.WriteMsg(LogType.Error, string.Format("方法{0}发生错误，错误：{1}", "MeasureDistance", ex));
            }
        }

        public double CalculateLength(IGeometry line)
        {
            double Length = 0;
            try
            {
                if (line.GeometryType == esriGeometryType.esriGeometryPoint)
                    return 0;
                IPoint pPT1;
                IPoint pPT2;
                if (line != null)
                {
                    IPointCollection pPtCol = new PolylineClass();
                    pPtCol = line as IPointCollection;
                    int nCnt = pPtCol.PointCount;
                    if (nCnt <= 1)
                    {
                        return Length;
                    }
                    else
                    {
                        for (int index = 0; index <= nCnt - 2; index++)
                        {
                            pPT1 = pPtCol.get_Point(index);
                            pPT2 = pPtCol.get_Point(index + 1);
                            Length = Length + CalculateLength(pPT1.X, pPT1.Y, pPT2.X, pPT2.Y);
                        }
                        return Length;
                    }
                }
                else
                {
                    return Length;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Instance.WriteMsg(LogType.Error, string.Format("方法{0}发生错误，错误：{1}", "MeasureDistance", ex));
                return 0;
            }
        }

        /// <summary>
        /// 量算距离
        /// </summary>
        /// <param name="line"></param>
        /// <param name="Length"></param>
        /// <param name="angle12"></param>
        /// <param name="angle21"></param>
        public void CalculateLength(ESRI.ArcGIS.Geometry.IPolyline line, out double Length, out double angle12, out double angle21)
        {
            Length = angle12 = angle21 = double.NaN;

            try
            {
                IPoint pPT1;
                IPoint pPT2;
                if (line != null)
                {
                    IPointCollection pPtCol = line as IPointCollection;
                    int nCnt = pPtCol.PointCount;
                    if (nCnt <= 1)
                    {
                        return;
                    }
                    else
                    {
                        Length = 0;
                        for (int index = 0; index <= nCnt - 2; index++)
                        {
                            pPT1 = pPtCol.get_Point(index);
                            pPT2 = pPtCol.get_Point(index + 1);
                            double lengthTemp;
                            CalculateLength(pPT1.X, pPT1.Y, pPT2.X, pPT2.Y, out lengthTemp, out angle12, out angle21);
                            Length = Length + lengthTemp;
                        }
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Instance.WriteMsg(LogType.Error, string.Format("方法{0}发生错误，错误：{1}", "MeasureDistance", ex));
            }
        }
        /// <summary>
        /// 已知线段坐标，求线段长度
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public double CalculateLength(double[,] coordinates)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 已知线段经度坐标、纬度坐标，求线段长度
        /// </summary>
        /// <param name="longitudes"></param>
        /// <param name="latitudes"></param>
        /// <returns></returns>
        public double CalculateLength(double[] longitudes, double[] latitudes)
        {
            throw new NotImplementedException();
        }
        public double IPolylineLength(IPolyline line, ESRI.ArcGIS.esriSystem.esriUnits unit)
        {
            try
            {
                if (line == null || line.IsEmpty) return 0;
                //return line.Length * unit;


                IPolycurveGeodetic polycurve = line as IPolycurveGeodetic;
                ILinearUnitEdit linearUnitEdit = new LinearUnitClass();

                //Define the properties for the linear unit 
                object name = "Meter";
                object alias = "Meter";
                object abbreviation = "M";
                object remarks = "Meter is the linear unit";
                object metersPerUnit = 1;
                linearUnitEdit.Define(ref name, ref alias, ref abbreviation, ref remarks, ref metersPerUnit);
                ILinearUnit pLU = linearUnitEdit as ILinearUnit;

                double distance = polycurve.get_LengthGeodetic(esriGeodeticType.esriGeodeticTypeGeodesic, pLU);


                ESRI.ArcGIS.esriSystem.IUnitConverter unitConverte = new ESRI.ArcGIS.esriSystem.UnitConverterClass();
                if (unit != ESRI.ArcGIS.esriSystem.esriUnits.esriMeters)
                    distance = unitConverte.ConvertUnits(distance, unit, ESRI.ArcGIS.esriSystem.esriUnits.esriMeters);
                return distance;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Calculate_GreatCircleLine(IPolyline line, out double Length)
        {
            throw new NotImplementedException();
        }
    }
}
