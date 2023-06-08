using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.Algorithm
{
    public class GeometrySeach : IGeometrySearch
    {
        public double _pai = ConstantValue.Pai;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinateArray"></param>
        /// <param name="radius"></param>
        /// <param name="ratio"></param>
        /// <param name="helperDirection"></param>
        /// <param name="errorRange"></param>
        /// <param name="geodetic"></param>
        /// <param name="resultCoord"></param>
        /// <param name="distanceTA"></param>
        /// <param name="distanceTB"></param>
        /// <returns></returns>
        public bool SearchPointRatio2Point(double[][] coordinateArray, double radius, double ratio, double helperDirection, double errorRange
             , IGeodeticSolution geodetic, out double[,] resultCoord, out double distanceTA, out double distanceTB)
        {
            resultCoord = null;
            distanceTA = 0;
            distanceTB = 0;
            int isAddB = 1;
            var coordArr = coordinateArray;
            if (coordinateArray.GetLength(0) != 3)
                return false;
            double lengthAB, lengthAC, lengthBC, angleAB, angleBA, angleAC, angleCA, angleBC, angleCB;
            geodetic.SecondSubject(coordArr[2][0], coordArr[2][1], coordArr[0][0], coordArr[0][1], out lengthAC, out angleCA, out angleAC);
            geodetic.SecondSubject(coordArr[2][0], coordArr[2][1], coordArr[1][0], coordArr[1][1], out lengthBC, out angleCB, out angleBC);
            geodetic.SecondSubject(coordArr[1][0], coordArr[1][1], coordArr[0][0], coordArr[0][1], out lengthAB, out angleBA, out angleAB);
            if (helperDirection < 0)
                angleBC = (angleCA + angleCB) / 2.0;
            if (angleBC - angleBA < 0)
                isAddB = -1 * isAddB;
            if (Math.Abs(angleBC - angleBA) > 180)
                isAddB = -1 * isAddB;
            if (lengthAB == 0)
                isAddB = 1;

            double lengthBt, longitudeT, latitudeT, angleTB, angleBT;
            double lengthAt, angleTA, angleAT;
            if (Math.Abs(radius - (ratio * lengthAB / (ratio + 1))) < errorRange)
            {
                geodetic.SecondSubject(coordArr[0][0], coordArr[0][1], coordArr[1][0], coordArr[1][1], out lengthAB, out angleAB, out angleBA);
                geodetic.FirstSubject(coordArr[0][0], coordArr[0][1], angleAB, radius, out longitudeT, out latitudeT, out angleTB);
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[0][0], coordArr[0][1], out lengthAt, out angleTA, out angleAT);//查询该点到A点的距离
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[1][0], coordArr[1][1], out lengthBt, out angleTB, out angleBT);//查询该点到B点的距离
                resultCoord = new double[,] { { longitudeT, latitudeT } };
                distanceTA = lengthAt;
                distanceTB = lengthBt;
                return true;
            }
            //如果是非1：1，比例点在延长线上
            if (ratio > 1 && Math.Abs(radius - (ratio * lengthAB / (ratio - 1))) < errorRange)
            {
                geodetic.SecondSubject(coordArr[0][0], coordArr[0][1], coordArr[1][0], coordArr[1][1], out lengthAB, out angleAB, out angleBA);
                geodetic.FirstSubject(coordArr[0][0], coordArr[0][1], angleAB, radius, out longitudeT, out latitudeT, out angleTB);
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[0][0], coordArr[0][1], out lengthAt, out angleTA, out angleAT);//查询该点到A点的距离
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[1][0], coordArr[1][1], out lengthBt, out angleTB, out angleBT);//查询该点到B点的距离
                resultCoord = new double[,] { { longitudeT, latitudeT } };
                distanceTA = lengthAt;
                distanceTB = lengthBt;
                return true;
            }
            if (ratio < 1 && Math.Abs(radius - (ratio * lengthAB / (-ratio + 1))) < errorRange)
            {
                geodetic.FirstSubject(coordArr[1][0], coordArr[1][1], angleBA, lengthAB / (1 - ratio), out longitudeT, out latitudeT, out angleTB);
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[0][0], coordArr[0][1], out lengthAt, out angleTA, out angleAT);//查询该点到A点的距离
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[1][0], coordArr[1][1], out lengthBt, out angleTB, out angleBT);//查询该点到B点的距离
                distanceTA = lengthAt;
                distanceTB = lengthBt;
                return true;
            }
            if (lengthAB * ratio / (1 + ratio) > radius)
                return false;
           // double cosB = lengthAB * ratio / 2.0 / radius + (1 / ratio - ratio) * radius / 2.0 / lengthAB;
          //  if (cosB > 1 || cosB < 0)
          //      cosB = 0.5;
            double cosB = (Math.Cos(radius * ratio) - Math.Cos(radius) * Math.Cos(lengthAB)) / Math.Sin(lengthAB) * Math.Sin(radius); 
           

            double angle = lengthAB == 0 ? angleBC : Math.Acos(cosB) * 180 / _pai;
            try
            {

                geodetic.FirstSubject(coordArr[1][0], coordArr[1][1], angleBA + isAddB * angle, radius / ratio, out longitudeT, out latitudeT, out angleTB);//从B处查询一个距离点              
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[0][0], coordArr[0][1], out lengthAt, out angleTA, out angleAT);//查询该点到A点的距离
                geodetic.SecondSubject(longitudeT, latitudeT, coordArr[1][0], coordArr[1][1], out lengthBt, out angleTB, out angleBT);//查询该点到B点的距离

                double derta = radius - lengthAt;
                int index = 0;

                angle = lengthAB == 0 ? angleBC : angle + (derta / lengthAB) * 180 / _pai;
                while (Math.Abs(derta) > errorRange)
                {
                    index++;
                    geodetic.FirstSubject(coordArr[1][0], coordArr[1][1], angleBA + isAddB * angle, radius / ratio, out longitudeT, out latitudeT, out angleTB);//从B处查询一个距离点              
                    geodetic.SecondSubject(longitudeT, latitudeT, coordArr[0][0], coordArr[0][1], out lengthAt, out angleTA, out angleAT);//查询该点到A点的距离
                    geodetic.SecondSubject(longitudeT, latitudeT, coordArr[1][0], coordArr[1][1], out lengthBt, out angleTB, out angleBT);//查询该点到B点的距离
                    derta = radius - lengthAt;
                    angle = angle + (derta / lengthAB) * 180 / _pai;
                    if (index > 10000)
                        throw new Exception("The error is occured in  search the equiratio point!");
                }
                resultCoord = new double[,] { { longitudeT, latitudeT } };
                distanceTA = lengthAt;
                distanceTB = lengthBt;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
