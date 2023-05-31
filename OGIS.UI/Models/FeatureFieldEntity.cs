using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    /// <summary>
    /// 要素属性实体
    /// </summary>
    public class FeatureFieldEntity : AOceanEntity
    {
        private bool _isInsert = false;
        private string _name = string.Empty;
        private int _id = 0;
        public FeatureFieldEntity()
        {
            EntityType = EntityType.FeatureFiled;

        }
        /// <summary>
        /// 根据划界线 类型 构造对应实体
        /// </summary>
        /// <param name="id"></param>
        public FeatureFieldEntity(OcsLineEnum id)
        {
            EntityType = EntityType.FeatureFiled;
            _id = (int)id;
            switch (id)
            {
                default:
                    break;
                case OcsLineEnum.BaseLine:
                    _name = "基线";
                    break;
                case OcsLineEnum.FootLine:
                    _name = "坡脚线";
                    break;
                case OcsLineEnum.Foot60NmBufferLine:
                    _name = "60海里缓冲线";
                    break;
                case OcsLineEnum.SedimentLine:
                    _name = "1%沉积物线";
                    break;
                case OcsLineEnum.FormularLine:
                    _name = "公式融合线";
                    break;
                case OcsLineEnum.Contour2500Line:
                    _name = "2500米等深线";
                    break;
                case OcsLineEnum.Contour100NmBufferline:
                    _name = "2500米等深线100海里缓冲";
                    break;
                case OcsLineEnum.Coast350Nmline:
                    _name = "350海里海岸线";
                    break;
                case OcsLineEnum.LimitLine:
                    _name = "限制线";
                    break;
                case OcsLineEnum.FusionLine:
                    _name = "融合线";
                    break;
            }

        }
        /// <summary>
        /// 图元
        /// </summary>
        public IGeometry Geometry { get; set; }
        /// <summary>
        /// 是否新增图元，否则替换当前ID的图元
        /// </summary>
        public bool IsInsert
        {
            get { return _isInsert; }
            set { _isInsert = value; }
        }
        public int ObjectId { get; set; }
        /// <summary>
        /// IGeometry形状类型
        /// </summary>
        public esriFieldType Shape { get; set; }
        /// <summary>
        /// ID属性
        /// </summary>
        public int Id { get; set; }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 划界线类型
        /// </summary>
        public string Type { get; set; }
        public string Code { get; set; }
    }
}
