using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using OGIS.Contracts;
/********************************************************************************
****创建目的：  
****创 建 人：  李洋
****创建时间：  2019-04-10
****修 改 人：
****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.UI
{
    public static class GeometryHelper
    {
        #region
        public static bool IsNeedTurnDirection(IPolyline pLineA, ref IPolyline pLineB)
        {
            if (pLineA == null || pLineA == null || (pLineA as IPointCollection).PointCount < 2 || (pLineB as IPointCollection).PointCount < 2)
                throw new ArgumentException("线段格式不正确");
            try
            {
                bool isNeed = false;
                //判断是否同向，可判断 首首尾尾尾是否相交
                IPolyline tmpLine1 = new PolylineClass();
                tmpLine1.FromPoint = pLineA.FromPoint;
                tmpLine1.ToPoint = pLineB.FromPoint;

                IPolyline tmpLine2 = new PolylineClass();
                tmpLine2.FromPoint = pLineA.ToPoint;
                tmpLine2.ToPoint = pLineB.ToPoint;
                ITopologicalOperator topoOperator = tmpLine1 as ITopologicalOperator;
                IGeometry geo = topoOperator.Intersect(tmpLine2, esriGeometryDimension.esriGeometry0Dimension);
                if (!geo.IsEmpty)
                {
                    //IPointCollection Pc = geo as IPointCollection;
                    //IPoint Pt = Pc.get_Point(0);
                    isNeed = true;
                }
                if (!isNeed) return isNeed;
                IPointCollection line = new PolylineClass();
                object miss = Type.Missing;
                var col = pLineB as IPointCollection;
                for (int index = col.PointCount - 1; index >= 0; index--)
                {
                    line.AddPoint(col.get_Point(index), ref miss, ref miss);
                }
                pLineB = line as IPolyline;
                return isNeed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SameDirection(IPolyline pLineA, ref IPolyline pLineB)
        {
            if (pLineA == null || pLineA == null || (pLineA as IPointCollection).PointCount < 2 || (pLineB as IPointCollection).PointCount < 2)
                throw new ArgumentException("线段格式不正确");
            try
            {
                bool isNeed = false;
                //判断是否同向，可判断 首首尾尾尾是否相交
                IPolyline tmpLine1 = new PolylineClass();
                tmpLine1.FromPoint = pLineA.FromPoint;
                tmpLine1.ToPoint = pLineB.FromPoint;

                IPolyline tmpLine2 = new PolylineClass();
                tmpLine2.FromPoint = pLineA.ToPoint;
                tmpLine2.ToPoint = pLineB.ToPoint;
                ITopologicalOperator topoOperator = tmpLine1 as ITopologicalOperator;
                IGeometry geo = topoOperator.Intersect(tmpLine2, esriGeometryDimension.esriGeometry0Dimension);
                if (!(geo.IsEmpty || geo == null))
                {
                    isNeed = true;
                }
                if (!isNeed) return isNeed;
                IPointCollection line = new PolylineClass();
                object miss = Type.Missing;
                var col = pLineB as IPointCollection;
                for (int index = col.PointCount - 1; index >= 0; index--)
                {
                    line.AddPoint(col.get_Point(index), ref miss, ref miss);
                }
                pLineB = line as IPolyline;
                return isNeed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IRelationalOperator GetOpertorBy2Line(IPointCollection pLineA, IPointCollection pLineB)
        {
            if (pLineA == null || pLineA == null || (pLineA as IPointCollection).PointCount < 2 || (pLineB as IPointCollection).PointCount < 2)
                throw new ArgumentException("线段格式不正确");
            IPointCollection pPTCol = new RingClass();
            object miss = Type.Missing;
            try
            {
                for (int index = 0; index < pLineA.PointCount; index++)
                {
                    pPTCol.AddPoint(pLineA.get_Point(index), ref miss, ref miss);
                }
                for (int index = pLineB.PointCount - 1; index >= 0; index--)
                {
                    pPTCol.AddPoint(pLineB.get_Point(index), ref miss, ref miss);
                }
                IRing pRing;
                pRing = pPTCol as IRing; ;
                pRing.Close();

                IGeometryCollection pPolygon;
                pPolygon = new PolygonClass();
                pPolygon.AddGeometry(pRing, ref miss, ref miss);
                IPolygon pPG = pPolygon as IPolygon;
                IRelationalOperator pRelation = pPG as IRelationalOperator;
                return pRelation;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static ISegment CreateSegment(IPoint fromPoint, IPoint toPoint)
        {
            ISegment segment = new LineClass();
            segment.FromPoint = fromPoint;
            segment.ToPoint = toPoint;
            return segment;
        }
        public static IPath CreatePath(IPoint fromPoint, IPoint toPoint)
        {
            IPath path = new PathClass();
            try
            {
                path.FromPoint = fromPoint;
                path.ToPoint = toPoint;
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static void CreatePath(ref ISegmentCollection pathCol, ISegment segment)
        {
            if (pathCol == null)
                pathCol = new PathClass();
            pathCol.AddSegment(segment);
        }

        public static void CreatePolyline(ref ISegmentCollection polylineCol, ISegmentCollection segmentCol)
        {
            if (polylineCol == null)
                polylineCol = new PolylineClass();
            polylineCol.AddSegmentCollection(segmentCol);
        }

        public static void CreatePolyline(ref IGeometryCollection polylineCol, ISegmentCollection segmentCol)
        {
            if (polylineCol == null)
                polylineCol = new PolylineClass();
            polylineCol.AddGeometry((IGeometry)segmentCol);
        }

        public static void CreatePolyline(ref IGeometryCollection polylineCol, IGeometry segment)
        {
            if (polylineCol == null)
                polylineCol = new PolylineClass();
            polylineCol.AddGeometry(segment);
        }

        public static IPolygon ConstractPolygon(IPointCollection colA, IPointCollection colB)
        {
            if (colA == null || colB == null || colA.PointCount == 1 && colB.PointCount == 1)
                return null;
            var lineACol = (colA as IClone).Clone() as IPointCollection;
            var lineBCol = (colB as IClone).Clone() as IPointCollection;
            List<IPoint[]> list = new List<IPoint[]>();
            if (lineACol.PointCount <= lineBCol.PointCount)
            {
                var temp = lineACol;
                lineACol = lineBCol;
                lineBCol = temp;
            }
            IPoint[] arr;
            #region 组点
            for (int i = 0; i < lineACol.PointCount - 1; i++)
            {
                if (i >= lineBCol.PointCount - 1)
                {
                    arr = new IPoint[3];
                    arr[0] = lineACol.get_Point(i);
                    arr[1] = lineACol.get_Point(i + 1);
                    arr[2] = lineBCol.get_Point(lineBCol.PointCount - 1);
                    list.Add(arr);


                    continue; ;
                }
                //B的点数必须小于等于A
                arr = new IPoint[3];
                arr[0] = lineACol.get_Point(i);
                arr[1] = lineACol.get_Point(i + 1);
                arr[2] = lineBCol.get_Point(i);
                list.Add(arr);
                if (i + 1 <= lineBCol.PointCount - 1)
                {
                    arr = new IPoint[3];
                    arr[0] = lineACol.get_Point(i);
                    arr[1] = lineACol.get_Point(i + 1);
                    arr[2] = lineBCol.get_Point(i + 1);
                }
                list.Add(arr);
                continue;
            }
            for (int j = 0; j < lineBCol.PointCount - 1; j++)
            {
                arr = new IPoint[3];
                arr[0] = lineBCol.get_Point(j);
                arr[1] = lineBCol.get_Point(j + 1);
                arr[2] = lineACol.get_Point(j);
                list.Add(arr);

                arr = new IPoint[3];
                arr[0] = lineBCol.get_Point(j);
                arr[1] = lineBCol.get_Point(j + 1);
                arr[2] = lineACol.get_Point(j + 1);
                list.Add(arr);
            }
            #endregion

            #region 构造面
            IPolygon polygonUnion = new PolygonClass();
            ITopologicalOperator polygonTopo;//= polygonUnion as ITopologicalOperator;
            foreach (var item in list)
            {
                PolygonClass polygon = new PolygonClass();
                for (int i = 0; i < item.Length; i++)
                {
                    polygon.AddPoint(item[i]);
                }
                polygon.AddPoint(item[0]);
                polygon.SimplifyPreserveFromTo();
                polygonTopo = polygonUnion as ITopologicalOperator;
                var rels = polygonTopo.Union(polygon);
                polygonUnion = rels as IPolygon;
            }
            //polygonUnion.Simplify();

            //IGeometry geometryBag = new GeometryBagClass();
            //IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            //foreach (var item in list)
            //{
            //    PolygonClass polygon = new PolygonClass();
            //    for (int i = 0; i < item.Length; i++)
            //    {
            //        polygon.AddPoint(item[i]);
            //    }
            //    geometryCollection.AddGeometry(polygon);
            //}
            //PolygonClass unionedPolygon = new PolygonClass();
            //unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);
            #endregion
            return polygonUnion;

        }

        /// <summary>
        /// 该方法最好还是单独成类，因为该方法涉及IGeodeticSolution的应用
        /// </summary>
        /// <param name="points"></param>
        /// <param name="spatialReference"></param>
        /// <param name="geodetic"></param>
        /// <returns></returns>
        public static IPolyline ConstructGeodeticLine(IList<IPoint> points, ISpatialReference spatialReference, IGeodeticSolution geodetic)
        {
            if (points == null || points.Count() < 2)
                throw new ArgumentException();

            ILinearUnitEdit linearUnitEdit = new LinearUnitClass();
            object name = "Meter";
            object alias = "Meter";
            object abbreviation = "M";
            object remarks = "Meter is the linear unit";
            object metersPerUnit = 1;
            linearUnitEdit.Define(ref name, ref alias, ref abbreviation, ref remarks, ref metersPerUnit);
            ILinearUnit pLU = linearUnitEdit as ILinearUnit;

            if (spatialReference == null)
            {
                // 设置坐标系
                ISpatialReferenceFactory3 spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                spatialReference = spatialReferenceFactory.CreateSpatialReference((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

            }
            PolylineClass line = new PolylineClass();
            for (int i = 0; i < points.Count() - 1; i++)
            {
                var p = points[i];
                var q = points[i + 1];
                p.SpatialReference = spatialReference;
                q.SpatialReference = spatialReference;

                var construct1 = new PolylineClass();
                double lengthPQ, anglePQ, angleQP;
                geodetic.SecondSubject(p.X, p.Y, q.X, q.Y, out lengthPQ, out anglePQ, out angleQP);
                // ConstructGeodeticLineFromDistance 需设置点的空间参考，否则报错
                construct1.ConstructGeodeticLineFromDistance(0, p, pLU, lengthPQ, anglePQ, esriCurveDensifyMethod.esriCurveDensifyByDeviation, -1.0);
                construct1.SpatialReference = spatialReference;
                line.AddGeometryCollection(construct1);
            }
            line.SpatialReference=spatialReference;
            return line;
        }
        public static IPolyline ConstructGeodeticLine(IPoint p, IPoint q, ISpatialReference spatialReference, IGeodeticSolution geodetic)
        {
            if (p == null || p.IsEmpty || q == null || q.IsEmpty)
                throw new ArgumentException();

            ILinearUnitEdit linearUnitEdit = new LinearUnitClass();
            object name = "Meter";
            object alias = "Meter";
            object abbreviation = "M";
            object remarks = "Meter is the linear unit";
            object metersPerUnit = 1;
            linearUnitEdit.Define(ref name, ref alias, ref abbreviation, ref remarks, ref metersPerUnit);
            ILinearUnit pLU = linearUnitEdit as ILinearUnit;

            if (spatialReference == null)
            {
                // 设置坐标系
                ISpatialReferenceFactory3 spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                spatialReference = spatialReferenceFactory.CreateSpatialReference((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

            }

            p.SpatialReference = spatialReference;
            q.SpatialReference = spatialReference;

            var construct1 = new Polyline() as IConstructGeodetic;
            double lengthPQ, anglePQ, angleQP;
            geodetic.SecondSubject(p.X, p.Y, q.X, q.Y, out lengthPQ, out anglePQ, out angleQP);
            // ConstructGeodeticLineFromDistance 需设置点的空间参考，否则报错
            construct1.ConstructGeodeticLineFromDistance(0, p, pLU, lengthPQ, anglePQ, esriCurveDensifyMethod.esriCurveDensifyByDeviation, -1.0);
            IPolyline line1 = construct1 as IPolyline;
            line1.SpatialReference = spatialReference;
            return line1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="geometry"></param>
        /// <param name="pixelsUnit">Tolerance in pixels for line hits</param>
        /// <returns></returns>
        public static IPoint CatchIgeometry(double x, double y, IGeometry geometry, double pixelsUnit, out int index, out int segmentIndex, out bool isRight)
        {
            IPoint iHitPt = new PointClass();
            IHitTest iHitTest = geometry as IHitTest;
            IPoint point = new PointClass();
            point.PutCoords(x, y);
            double hitDist = 0;
            int partIndex = 0;
            int vertexIndex = 0;
            bool bVertexHit = false;
            if (iHitTest.HitTest(point, pixelsUnit, esriGeometryHitPartType.esriGeometryPartVertex,
                iHitPt, ref hitDist, ref partIndex, ref vertexIndex, ref bVertexHit))
            {
                index = vertexIndex;
                segmentIndex = vertexIndex - partIndex;
                isRight = bVertexHit;
                return iHitPt;
            }
            index = -1;
            segmentIndex = -1;
            isRight = false;
            return null;

        }


        public static int QueryIndexFromIGeometry(IPointCollection geo, IPoint point)
        {
            var col = geo as IPointCollection;
            if (col == null || col.PointCount == 0)
                return -1;
            for (int index = 0; index < col.PointCount; index++)
            {
                if (Math.Abs(col.get_Point(index).X - point.X) <= 0.000001 && Math.Abs(col.get_Point(index).Y - point.Y) <= 0.000001)
                    return index;
            }
            return -1;
        }

        public static IPolygon CreatePolygon(IPolyline pLineA, IPolyline pLineB)
        {
            if (pLineA == null || pLineA == null || (pLineA as IPointCollection).PointCount < 2 || (pLineB as IPointCollection).PointCount < 2)
                throw new ArgumentException("线段格式不正确");
            try
            {
                bool isSameDirection = false;
                //判断是否同向，可判断 首首尾尾尾是否相交
                IPolyline tmpLine1 = new PolylineClass();
                tmpLine1.FromPoint = pLineA.FromPoint;
                tmpLine1.ToPoint = pLineB.FromPoint;

                IPolyline tmpLine2 = new PolylineClass();
                tmpLine2.FromPoint = pLineA.ToPoint;
                tmpLine2.ToPoint = pLineB.ToPoint;
                ITopologicalOperator topoOperator = tmpLine1 as ITopologicalOperator;
                IGeometry geo = topoOperator.Intersect(tmpLine2, esriGeometryDimension.esriGeometry0Dimension);
                if (geo == null || geo.IsEmpty)
                {
                    isSameDirection = true;
                }

                IPointCollection polygonCol = new PolygonClass();
                polygonCol.AddPointCollection(pLineA as IPointCollection);
                var col2 = pLineB as IPointCollection;
                if (!isSameDirection)
                {
                    for (int i = 0; i < col2.PointCount; i++)
                    {
                        polygonCol.AddPoint(col2.get_Point(i));
                    }
                }
                else
                {
                    for (int j = col2.PointCount - 1; j >= 0; j--)
                    {
                        polygonCol.AddPoint(col2.get_Point(j));
                    }
                }
                return polygonCol as IPolygon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IPolygon CreatePolygon(IPointCollection pLineA, IPointCollection pLineB)
        {
            if (pLineA == null || pLineA == null || pLineA.PointCount < 2 || pLineB.PointCount < 2)
                throw new ArgumentException("线段格式不正确");
            try
            {
                bool isSameDirection = false;
                //判断是否同向，可判断 首首尾尾尾是否相交
                IPolyline tmpLine1 = new PolylineClass();
                tmpLine1.FromPoint = pLineA.get_Point(0);
                tmpLine1.ToPoint = pLineB.get_Point(0);

                IPolyline tmpLine2 = new PolylineClass();
                tmpLine2.FromPoint = pLineA.get_Point(pLineA.PointCount - 1);
                tmpLine2.ToPoint = pLineB.get_Point(pLineB.PointCount - 1);
                ITopologicalOperator topoOperator = tmpLine1 as ITopologicalOperator;
                IGeometry geo = topoOperator.Intersect(tmpLine2, esriGeometryDimension.esriGeometry0Dimension);
                if (geo == null || geo.IsEmpty)
                {
                    isSameDirection = true;
                }

                IPointCollection polygonCol = new PolygonClass();
                polygonCol.AddPointCollection(pLineA as IPointCollection);
                var col2 = pLineB as IPointCollection;
                if (!isSameDirection)
                {
                    for (int i = 0; i < col2.PointCount; i++)
                    {
                        polygonCol.AddPoint(col2.get_Point(i));
                    }
                }
                else
                {
                    for (int j = col2.PointCount - 1; j >= 0; j--)
                    {
                        polygonCol.AddPoint(col2.get_Point(j));
                    }
                }
                return polygonCol as IPolygon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IPointCollection PolylineConvertMultPoint(IPolyline line)
        {
            if (line == null) return null;
            MultipointClass col = new MultipointClass();
            col.AddPointCollection(line as IPointCollection);
            col.SpatialReference = line.SpatialReference;
            return col;
        }

        public static IPointCollection ConstractPointCollection(IPointCollection lineCollection, IPoint[] pointArr)
        {
            if (lineCollection == null) return lineCollection;
            if (pointArr == null || pointArr.Length == 0) return lineCollection;

            for (int i = 0; i < pointArr.Length; i++)
            {
                int index, segmentIndex;
                bool isRight;
                CatchIgeometry(pointArr[i].X, pointArr[i].Y, lineCollection as IGeometry, 12, out index, out segmentIndex, out isRight);
                if (lineCollection.PointCount == index + 1)
                    lineCollection.AddPoint(pointArr[i]);
                else
                    lineCollection.InsertPoints(index + 1, 1, pointArr[i]);
            }
            return lineCollection;
        }

        public static IPoint[] ConvertPointCollectionToArr(IPointCollection lineCollection)
        {
            if (lineCollection == null || lineCollection.PointCount == 0) return null;
            IPoint[] arr = new IPoint[lineCollection.PointCount];
            for (int i = 0; i < lineCollection.PointCount; i++)
            {
                arr[i] = lineCollection.get_Point(i);
            }
            return arr;
        }

        public static IGeometry[] ConvertGeometryCollectionToArr(IGeometryCollection collection)
        {
            if (collection == null || collection.GeometryCount == 0) return null;
            IGeometry[] arr = new IGeometry[collection.GeometryCount];
            for (int i = 0; i < collection.GeometryCount; i++)
            {
                var line = new PolylineClass();
                line.AddGeometry(collection.get_Geometry(i));
                arr[i] = line;
            }
            return arr;
        }
        public static IGeometry[] ConvertGeometryCollectionToArr(IGeometry geometry)
        {
            if (geometry == null) return null;
            if (geometry.GeometryType == esriGeometryType.esriGeometryPoint) return new IGeometry[] { geometry };

            if (geometry.GeometryType == esriGeometryType.esriGeometryMultipoint)
            {
                return ConvertPointCollectionToArr(geometry as IPointCollection);
            }
            if (geometry.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                return ConvertGeometryCollectionToArr(geometry as IGeometryCollection);
            }
            return new IGeometry[] { geometry };
        }

        #endregion


        /// <summary>
        /// 获取0到360度的角
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="delta"></param>
        public static double GetDirectionAngle(double angle, double AddAngle)
        {
            double angleResult = angle + AddAngle;
            while (angleResult > 360 || angleResult < 0)
            {
                if (angleResult >= 360)
                {
                    angleResult = angleResult - 360;
                    continue;
                }

                if (angleResult < 0)
                {
                    angleResult = angleResult + 360;
                    continue;
                }

            }

            return angleResult;

        }

        /// <summary>
        /// 判断线段的凹凸性
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double CheckLineConcavity(IPointCollection col)
        {
            if (col == null || col.PointCount < 2) throw new ArgumentNullException();

            double concavity = 0;
            for (int index = 0; index < col.PointCount - 2; index++)
            {
                concavity += (col.get_Point(index + 1).X - col.get_Point(index).X) * (col.get_Point(index + 1).Y - col.get_Point(index).Y) - (col.get_Point(index + 2).X - col.get_Point(index + 1).X) * (col.get_Point(index + 2).Y - col.get_Point(index + 1).Y);
            }
            return concavity;

        }

        /// <summary>
        /// 判断线段的凹凸性
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double CheckLineConcavity(IPoint pt1, IPoint pt2, IPoint pt3)
        {
            if (pt1 == null || pt1.IsEmpty || pt2 == null || pt2.IsEmpty || pt3 == null || pt3.IsEmpty)
                throw new ArgumentNullException();

            double concavity = (pt2.X - pt1.X) * (pt2.Y - pt1.Y) - (pt3.X - pt2.X) * (pt3.Y - pt2.Y);
            return concavity;

        }
    }
}
