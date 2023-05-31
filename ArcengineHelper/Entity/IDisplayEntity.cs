using ArcengineHelper.DisplayHelper;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcengineHelper.Entity
{
    public interface IDisplayEntity
    {
        string GroupName { get; set; }
        List<ESRI.ArcGIS.Geometry.IGeometry> ListGeo { get; }
        List<IElement> ListElement { get; }

        void AddIElement(ESRI.ArcGIS.Carto.IElement element);

        void AddGeometry(ESRI.ArcGIS.Geometry.IGeometry geo);

        void AddRangeGeometry(IEnumerable<ESRI.ArcGIS.Geometry.IGeometry> list);

        void Clear();

        /// <summary>
        /// 0 只绘图；1即绘图又存到dgb
        /// </summary>
        int DisplayType { get; set; }

        /// <summary>
        /// 如果图元是线，是否同时描绘线节点；默认不描绘
        /// </summary>
        bool IsShowLinePoint { get; set; }
        /// <summary>
        /// 是否清除上次绘图
        /// </summary>
        bool IsClearCache { get; set; }
        /// <summary>
        /// 是否清除选择高亮
        /// </summary>
        bool IsClearSelect { get; set; }

        IElementCollection GetDisplayElements { get; }
    }



    public class ElementDisplayEntity : IDisplayEntity
    {
        private List<ESRI.ArcGIS.Geometry.IGeometry> _list;
        private List<ESRI.ArcGIS.Carto.IElement> _listElement;
        private bool _IsShowLinePoint = false;
        private bool _IsClearCache = true;
        private bool _IsClearSelect = true;
        private bool _IsShowLineDirection = false;

        public ElementDisplayEntity()
        {
            _list = new List<ESRI.ArcGIS.Geometry.IGeometry>();
            _listElement = new List<ESRI.ArcGIS.Carto.IElement>();
        }

        public string GroupName { get; set; }
        public List<ESRI.ArcGIS.Geometry.IGeometry> ListGeo { get { return _list; } }

        public void AddGeometry(ESRI.ArcGIS.Geometry.IGeometry geo)
        {
            _list.Add(geo);
        }

        public void AddRangeGeometry(IEnumerable<ESRI.ArcGIS.Geometry.IGeometry> list)
        {
            _list.AddRange(list);
        }
        /// <summary>
        /// 0 只绘图；1即绘图又存到dgb
        /// </summary>
        public int DisplayType { get; set; }

        /// <summary>
        /// 如果图元是线，是否同时描绘线节点；默认不描绘
        /// </summary>
        public bool IsShowLinePoint { get { return _IsShowLinePoint; } set { _IsShowLinePoint = value; } }
        /// <summary>
        /// 是否清除上次绘图
        /// </summary>
        public bool IsClearCache { get { return _IsClearCache; } set { _IsClearCache = value; } }
        /// <summary>
        /// 是否清除选择高亮
        /// </summary>
        public bool IsClearSelect { get { return _IsClearSelect; } set { _IsClearSelect = value; } }

        public bool IsShowLineDirection { get { return _IsShowLineDirection; } set { _IsShowLineDirection = value; } }


        public void Clear()
        {
            _list.Clear();
        }


        public void AddIElement(ESRI.ArcGIS.Carto.IElement element)
        {
            _listElement.Add(element);
        }


        public List<IElement> ListElement
        {
            get { return _listElement; }
        }

        public IElementCollection GetDisplayElements
        {
            get 
            {
                var col = new ElementCollectionClass();
                _listElement.ForEach(item=>col.Add(item,-1));
                _list.ForEach(item => col.Add(IElementHelper.CreateElement(item), -1));
                return col;
            }
        }
    }

    /// <summary>
    /// 用于记录图层
    /// </summary>
    public class SelectDisplayEntity : IDisplayEntity
    {
        private Dictionary<ESRI.ArcGIS.Geometry.IGeometry, string> _dic;
        private bool _IsShowLinePoint = false;
        private bool _IsClearCache = true;
        private bool _IsClearSelect = true;
        private bool _IsShowLineDirection = false;

        public SelectDisplayEntity()
        {
            _dic = new Dictionary<ESRI.ArcGIS.Geometry.IGeometry, string>();
        }

        public string GroupName { get; set; }

        public List<ESRI.ArcGIS.Geometry.IGeometry> ListGeo { get { return _dic.Keys.ToList(); } }

        public Dictionary<ESRI.ArcGIS.Geometry.IGeometry, string> DicGeo { get { return _dic; } }

        public void AddGeometry(ESRI.ArcGIS.Geometry.IGeometry geo)
        {
            _dic.Add(geo, "");
        }


        public void AddGeometry(ESRI.ArcGIS.Geometry.IGeometry geo, string layerName)
        {
            _dic.Add(geo, layerName);
        }

        public void AddRangeGeometry(IEnumerable<ESRI.ArcGIS.Geometry.IGeometry> list)
        {
            if (list == null || list.Count() == 0) return;
            list.ToList().ForEach(item => _dic.Add(item, ""));
        }

        public void AddRangeGeometry(IEnumerable<ESRI.ArcGIS.Geometry.IGeometry> list, string layerName)
        {
            if (list == null || list.Count() == 0) return;
            list.ToList().ForEach(
                item =>
                {
                    if (!_dic.ContainsKey(item))
                        _dic.Add(item, layerName);
                }
                );
        }
        /// <summary>
        /// 0 只绘图；1即绘图又存到dgb
        /// </summary>
        public int DisplayType { get; set; }

        /// <summary>
        /// 如果图元是线，是否同时描绘线节点；默认不描绘
        /// </summary>
        public bool IsShowLinePoint { get { return _IsShowLinePoint; } set { _IsShowLinePoint = value; } }
        /// <summary>
        /// 是否清除上次绘图
        /// </summary>
        public bool IsClearCache { get { return _IsClearCache; } set { _IsClearCache = value; } }
        /// <summary>
        /// 是否清除选择高亮
        /// </summary>
        public bool IsClearSelect { get { return _IsClearSelect; } set { _IsClearSelect = value; } }

        public bool IsShowLineDirection { get { return _IsShowLineDirection; } set { _IsShowLineDirection = value; } }


        public void Clear()
        {
            if (_dic == null) _dic = new Dictionary<ESRI.ArcGIS.Geometry.IGeometry, string>();
            _dic.Clear();
        }

        public void Add(SelectDisplayEntity entity)
        {
            try
            {
                if (_dic == null) _dic = new Dictionary<ESRI.ArcGIS.Geometry.IGeometry, string>();
                if (entity.DicGeo == null) return;
                foreach (var key in entity.DicGeo.Keys)
                {
                    if (_dic.ContainsKey(key)) continue;
                    _dic[key] = entity.DicGeo[key];
                }

            }
            catch (Exception)
            {

                throw;
            }

        }


        public void AddIElement(ESRI.ArcGIS.Carto.IElement element)
        {

        }


        public List<IElement> ListElement
        {
            get { return null; }
        }


        public IElementCollection GetDisplayElements
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class MarkDisplayEntity : IDisplayEntity
    {
        private List<ESRI.ArcGIS.Geometry.IGeometry> _list;
        private List<ESRI.ArcGIS.Carto.IElement> _listElement;
        private bool _IsShowLinePoint = false;
        private bool _IsClearCache = true;
        private bool _IsClearSelect = true;
        private bool _IsShowLineDirection = false;

        public MarkDisplayEntity()
        {
            _list = new List<ESRI.ArcGIS.Geometry.IGeometry>();
            _listElement = new List<ESRI.ArcGIS.Carto.IElement>();
        }

        public string GroupName { get; set; }
        public List<ESRI.ArcGIS.Geometry.IGeometry> ListGeo { get { return _list; } }

        public void AddGeometry(ESRI.ArcGIS.Geometry.IGeometry geo)
        {
            _list.Add(geo);
        }

        public void AddRangeGeometry(IEnumerable<ESRI.ArcGIS.Geometry.IGeometry> list)
        {
            _list.AddRange(list);
        }
        /// <summary>
        /// 0 只绘图；1即绘图又存到dgb
        /// </summary>
        public int DisplayType { get; set; }

        /// <summary>
        /// 如果图元是线，是否同时描绘线节点；默认不描绘
        /// </summary>
        public bool IsShowLinePoint { get { return _IsShowLinePoint; } set { _IsShowLinePoint = value; } }
        /// <summary>
        /// 是否清除上次绘图
        /// </summary>
        public bool IsClearCache { get { return _IsClearCache; } set { _IsClearCache = value; } }
        /// <summary>
        /// 是否清除选择高亮
        /// </summary>
        public bool IsClearSelect { get { return _IsClearSelect; } set { _IsClearSelect = value; } }

        public bool IsShowLineDirection { get { return _IsShowLineDirection; } set { _IsShowLineDirection = value; } }


        public void Clear()
        {
            _list.Clear();
        }


        public void AddIElement(ESRI.ArcGIS.Carto.IElement element)
        {
            _listElement.Add(element);
        }


        public List<IElement> ListElement
        {
            get { return _listElement; }
        }


        public IElementCollection GetDisplayElements
        {
            get { throw new NotImplementedException(); }
        }
    }
}
