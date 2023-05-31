using ESRI.ArcGIS.Geometry;
using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public interface IMeasure
    {
        /// <summary>
        /// 设置椭球参数
        /// </summary>
        /// <param name="Type"></param>
        void SetParameterType(int Type);

        /// <summary>
        /// 设置大地解算方法
        /// </summary>
        /// <param name="iGeodeticSolution">大地解算方法接口</param>
        void SetGeodeticSolutionType(IGeodeticSolution iGeodeticSolution);

        /// <summary>
        /// 第一主题解
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="angleAB"></param>
        /// <param name="length"></param>
        /// <param name="pointB"></param>
        /// <param name="angleBA"></param>
        void FirstSubject(IPoint pointA, double angleAB, double length, out IPoint pointB, out double angleBA);

        //第二主题解变化形式
        /// <summary>
        /// 获取两点之间的大地线长度
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        void CalculateLength(ESRI.ArcGIS.Geometry.IPoint point1, ESRI.ArcGIS.Geometry.IPoint point2, out double Length, out double angle12, out double angle21);
        /// <summary>
        /// 获取两点之间的大地线长度
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        double CalculateLength(IPoint point1, IPoint point2);
        /// <summary>
        /// 获取两点之间的距离
        /// </summary>
        /// <param name="longitudeA"></param>
        /// <param name="latitudeA"></param>
        /// <param name="longitude2"></param>
        /// <param name="latitude2"></param>
        /// <returns></returns>
        void CalculateLength(double longitude1, double latitude1, double longitude2, double latitude2, out double length, out double angle12, out double angle21);
        /// <summary>
        /// 获取两点之间的距离
        /// </summary>
        /// <param name="longitudeA"></param>
        /// <param name="latitudeA"></param>
        /// <param name="longitude2"></param>
        /// <param name="latitude2"></param>
        /// <returns></returns>
        double CalculateLength(double longitude1, double latitude1, double longitude2, double latitude2);


        /// <summary>
        /// 获取线段的长度
        /// </summary>
        /// <param name="line"></param>
        /// <param name="Length"></param>
        void CalculateLength(IPolyline line, out double Length);

        ///// <summary>
        ///// 岸线抽稀
        ///// </summary>
        ///// <param name="line"></param>
        ///// <param name="extractRadius"></param>
        ///// <param name="Length"></param>
        //void CalculateLength(IPolyline line, double extractRadius, out double Length);

        /// <summary>
        /// 获取线段的长度
        /// </summary>
        /// <param name="line"></param>
        /// <param name="Length"></param>
        double CalculateLength(IGeometry line);
        /// <summary>
        /// 获取线段的长度
        /// </summary>
        /// <param name="line"></param>
        /// <param name="Length"></param>
        void CalculateLength(IPolyline line, out double Length, out double angle12, out double angle21);
        /// <summary>
        /// 获取线段的长度
        /// </summary>
        /// <param name="coordinate">经纬度坐标</param>
        /// <returns></returns>
        double CalculateLength(double[,] coordinates);
        /// <summary>
        /// 获取线段的长度
        /// </summary>
        /// <param name="coordinate">经纬度坐标</param>
        /// <returns></returns>
        double CalculateLength(double[] longitudes, double[] latitudes);

        void Calculate_GreatCircleLine(IPolyline line, out double Length);


        double IPolylineLength(IPolyline line, ESRI.ArcGIS.esriSystem.esriUnits unit);
    }
}
