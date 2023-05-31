using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class FeaturesFieldEntity : AOceanEntity
    {
        private bool _isInsert = false;
        private List<IGeometry> _list;
        public FeaturesFieldEntity()
        {
            EntityType = EntityType.FeatureFiled;
            _list = new List<IGeometry>();
        }

        public esriGeometryType GeometryType { get; set; }
        /// <summary>
        /// 图元
        /// </summary>
        public List<IGeometry> GeometryList { get { return _list; } }

        public void AddGoemetry(IGeometry geometry)
        {
            _list.Add(geometry);
        }

        public void AddRangleGoemetry(IEnumerable<IGeometry> geometries)
        {
            _list.AddRange(geometries);
        }
        public void ClearList()
        {
            _list.Clear();
        }
        /// <summary>
        /// 是否新增图元，否则替换当前ID的图元
        /// </summary>
        public bool IsInsert
        {
            get { return _isInsert; }
            set { _isInsert = value; }
        }

    }
}
