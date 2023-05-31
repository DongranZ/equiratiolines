using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class FeatureFieldExtendEntity : AOceanEntity
    {
        private bool _isInsert = false;

        /// <summary>
        /// 坐标系
        /// </summary>
        public ESRI.ArcGIS.Geometry.ISpatialReference SpatialReference { get; set; }

        /// <summary>
        /// shp
        /// </summary>
        public ESRI.ArcGIS.Geometry.IGeometry Geometry { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 划界线类型
        /// </summary>
        public string Type { get; set; }

        public string Country { get; set; }

        /// <summary>
        /// 是否新增图元，否则替换当前ID的图元
        /// </summary>
        public bool IsInsert
        {
            get { return _isInsert; }
            set { _isInsert = value; }
        }

        /// <summary>
        /// 其他属性
        /// </summary>
        public Dictionary<string, string> OtherFields { get; set; }

        public List<ESRI.ArcGIS.Geometry.IGeometry> ListLine { get; set; }


    }
}
