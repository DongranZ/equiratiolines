using ESRI.ArcGIS.Geometry;
/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2021-01-13
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcengineHelper.MapHelper
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

        public static IGeometry PRJ2GCS(IGeometry pGeom, ISpatialReference sp)
        {
            if (sp == null || sp as IProjectedCoordinateSystem == null)
                return pGeom;
            try
            {
                ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
                pGeom.SpatialReference = sp;
                int GCSType = (int)esriSRGeoCSType.esriSRGeoCS_WGS1984;
                pGeom.Project(pSRF.CreateGeographicCoordinateSystem(GCSType));
                return pGeom;
            }
            catch (Exception ex)
            {
                throw ex;
                //Logs.Instance.WriteErrorLogs("投影坐标转换为地理坐标失败！", ex);
                //return pGeom;
            }


        }

        public static IGeometry GCSToPRJ(IGeometry pGeom, ISpatialReference spPRJ)
        {
            if (pGeom == null || spPRJ == null) return pGeom;
            if (spPRJ is IGeographicCoordinateSystem)
                return pGeom;
            if (pGeom.SpatialReference == null)
                pGeom.SpatialReference = CreateISpatialReference();
            pGeom.Project(spPRJ);
            return pGeom;
        }
    }
}
