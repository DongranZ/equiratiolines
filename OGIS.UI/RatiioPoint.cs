using ESRI.ArcGIS.Geometry;
using OGIS.Contracts;
using OGIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class RatioPoint : IRatioPoint
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
        public IPoint FindPointBy2Point(IPoint fromPoint, IPoint toPoint, bool isOnline, double ratio, out double ratiolength)
        {
            if (fromPoint == null || fromPoint.IsEmpty || toPoint == null || toPoint.IsEmpty || ratio <= 0)
                throw new ArgumentException();

            IPoint resultPoint = null;
            double length, angle12, angle21;
            _geodeticSolution.SecondSubject(fromPoint.X, fromPoint.Y, toPoint.X, toPoint.Y, out length, out angle12, out angle21);

            double longitude, latitude;
            if (isOnline)
            {
                ratiolength = ratio / (1 + ratio) * length;
                _geodeticSolution.FirstSubject(fromPoint.X, fromPoint.Y, angle12, ratiolength, out longitude, out latitude, out angle21);
                resultPoint = new PointClass();
                resultPoint.PutCoords(longitude, latitude);
                return resultPoint;
            }

            //正向取点
            if (ratio > 1)
            {
                ratiolength = length / Math.Abs(1 - ratio);
                _geodeticSolution.FirstSubject(fromPoint.X, fromPoint.Y, angle12, ratiolength, out longitude, out latitude, out angle21);
                resultPoint = new PointClass();
                resultPoint.PutCoords(longitude, latitude);
                return resultPoint;
            }
            //反向取点
            if (ratio < 1)
            {
                ratiolength = ratio * length / (1 - ratio);
                angle12 = angle12 + 180 > 360 ? angle12 - 180 : angle12 + 180;
                _geodeticSolution.FirstSubject(fromPoint.X, fromPoint.Y, angle12, ratiolength, out longitude, out latitude, out angle21);
                resultPoint = new PointClass();
                resultPoint.PutCoords(longitude, latitude);
                return resultPoint;
            }
            //等于1时需要迭代获取
            if (ratio == 1)
            {
                ratiolength = 0;
                return resultPoint;
            }

            ratiolength = 0;
            return resultPoint;
        }
    }
}
