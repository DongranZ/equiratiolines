using ESRI.ArcGIS.Geometry;
using OGIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public static class SpatialReferenceHelper
    {
        /// <summary>
        /// 创建地理坐标系
        /// </summary>
        /// <param name="geoSystem"></param>
        /// <returns></returns>
        public static ISpatialReference CreateISpatialReference(esriSRGeoCSType geoSystem = esriSRGeoCSType.esriSRGeoCS_WGS1984)
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            //geoSystem = esriSRGeoCSType.esriSRGeoCS_WGS1984;
            ISpatialReferenceResolution spatialReferenceResolution = spatialReferenceFactory.CreateGeographicCoordinateSystem(Convert.ToInt32(geoSystem)) as ISpatialReferenceResolution;
            spatialReferenceResolution.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance = spatialReferenceResolution as ISpatialReferenceTolerance;
            spatialReferenceTolerance.SetDefaultXYTolerance();
            ISpatialReference spatialReference = spatialReferenceResolution as ISpatialReference;
            return spatialReference;
        }
        /// <summary>
        /// 创建投影坐标系
        /// </summary>
        /// <param name="geoSystem"></param>
        /// <returns></returns>
        public static ISpatialReference CreateProjectedISpatialReference(esriSRProjCS2Type geoSystem = esriSRProjCS2Type.esriSRProjCS_WGS1984WorldMercator)
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            esriSRProjCS2Type proSystem = esriSRProjCS2Type.esriSRProjCS_WGS1984WorldMercator;
            ISpatialReferenceResolution spatialReferenceResolution = spatialReferenceFactory.CreateProjectedCoordinateSystem(Convert.ToInt32(proSystem)) as ISpatialReferenceResolution;
            spatialReferenceResolution.ConstructFromHorizon();
            ISpatialReferenceTolerance spatialReferenceTolerance = spatialReferenceResolution as ISpatialReferenceTolerance;
            spatialReferenceTolerance.SetDefaultXYTolerance();
            ISpatialReference spatialReference = spatialReferenceResolution as ISpatialReference;

            return spatialReference;
        }

        public static ISpatialReference CreateESRISpatialReferenceFromPRJFile(string prjPath)
        {
            ISpatialReferenceFactory spatialReferenceFactory;
            try
            {
                spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                if (!System.IO.File.Exists(prjPath)) return null;
                var spatialReference = spatialReferenceFactory.CreateESRISpatialReferenceFromPRJFile(prjPath);
                return spatialReference;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                spatialReferenceFactory = null;
            }

        }

        public static IGeometry ConvertToIGeographicCoordinateSystem(IGeometry geometry)
        {
            if (geometry.SpatialReference == null)
            {
                geometry.SpatialReference = CreateISpatialReference();
                return geometry;
            }

            if (geometry.SpatialReference is IGeographicCoordinateSystem)
                return geometry;
            if (geometry.SpatialReference is IProjectedCoordinateSystem)
                geometry.Project(CreateISpatialReference());
            return geometry;
        }


        /// <summary>
        ///  投影坐标-》地理坐标
        /// </summary>
        /// <param name="pGeom"></param>
        /// <param name="sp">当前投影坐标系</param>
        /// <returns></returns>
        public static IGeometry PRJ2GCS(IGeometry pGeom, ISpatialReference sp = null)
        {
            //如果地理图形 无坐标或者坐标为地理坐标
            if (pGeom.SpatialReference == null || pGeom.SpatialReference as IGeographicCoordinateSystem != null)
                return pGeom;
            //如果目标坐标为投影坐标 返回
            if (sp != null && sp as IProjectedCoordinateSystem != null)
                return pGeom;
            try
            {
                if (sp == null)
                {
                    ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
                    int GCSType = (int)esriSRGeoCSType.esriSRGeoCS_WGS1984;
                    sp = pSRF.CreateGeographicCoordinateSystem(GCSType);
                }
                pGeom.Project(sp);
                return pGeom;
            }
            catch (Exception ex)
            {
                WriteLog.Instance.WriteErrMsg("投影坐标转换为地理坐标失败！", ex);
                return pGeom;
            }


        }

        /// <summary>
        /// 地理坐标-》投影坐标
        /// </summary>        
        /// <param name="pGeom">地理图形</param>
        /// <param name="sp">当前地图的投影，要投影的坐标</param>
        /// <returns></returns>
        public static IGeometry GCS2PRJ(IGeometry pGeom, ISpatialReference sp)
        {
            if (pGeom == null) return pGeom;
            if (sp == null || sp as IProjectedCoordinateSystem == null)//sp
                return pGeom;

            if (pGeom.SpatialReference != null && pGeom.SpatialReference.Name.Equals(sp.Name))
                return pGeom;
            try
            {
                if (pGeom.SpatialReference == null)
                {
                    ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
                    int GCSType = (int)esriSRGeoCSType.esriSRGeoCS_WGS1984;
                    pGeom.SpatialReference = pSRF.CreateGeographicCoordinateSystem(GCSType);//地理坐标
                }
                pGeom.Project(sp);
                return pGeom;
            }
            catch (Exception ex)
            {
                WriteLog.Instance.WriteErrMsg("投影坐标转换为地理坐标失败！", ex);
                return pGeom;
            }


        }
        /// <summary>
        /// 地理坐标转投影坐标
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="GCSType"></param>
        /// <param name="PRJType"></param>
        /// <returns></returns>
        private static IGeometry GCStoPRJ(IGeometry geo, int GCSType, int PRJType)
        {
            ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
            geo.SpatialReference = pSRF.CreateGeographicCoordinateSystem(GCSType);
            geo.Project(pSRF.CreateProjectedCoordinateSystem(PRJType));
            return geo;
        }
    }
}
