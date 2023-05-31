using ESRI.ArcGIS.Geometry;
/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2011-02-01
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcengineHelper.GeometryHelper
{
    public static class GeometryHelper
    {
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
    }
}
