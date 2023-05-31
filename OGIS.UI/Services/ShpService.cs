using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class ShpService
    {
        public bool ExportMultToShp(List<IGeometry> goeList, string strPath, string fileName, ISpatialReference spatialReference = null)
        {
            if (goeList == null || goeList.Count <= 0) return false;
            try
            {
                var entity = new FeaturesFieldEntity();
                entity.GeometryType = goeList[0].GeometryType;
                entity.AddRangleGoemetry(goeList);
                entity.IsInsert = false;
                WorkSpaceAndFeatureHelper.CreateFeaturesOnWorkspace(strPath, fileName, entity, spatialReference, false);
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 有问题
        /// </summary>
        /// <param name="col"></param>
        /// <param name="spatialReference"></param>
        /// <returns></returns>
        public bool ExportToShp(IGeometry geo, string strPath, string fileName, ISpatialReference spatialReference = null)
        {
            if (geo == null) return false;
            try
            {
                var entity = new FeatureFieldEntity();
                entity.Geometry = geo;
                entity.Type = "其他";
                entity.Id = 0;
                entity.IsInsert = false;
                WorkSpaceAndFeatureHelper.CreateFeatureOnWorkspace(strPath, fileName, entity, spatialReference, false);
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ShpServiceProvider
    {
        static ShpService _shpService;
        public static ShpService Instance { get { return _shpService ?? new ShpService(); } }
    }

}
