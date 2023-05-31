using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI.ArcgisMap
{
    public interface IBaseMapView<T> where T : System.Windows.Forms.Form
    {
        void NewMap(T father);

        void InitializeBll();

        void Banding(System.Windows.Forms.Control ctrlMap, System.Windows.Forms.Control ctrlleft);

        void LoadMap();

        event Action<string, int, CurrentOperate> SetCurrentOpterate;

        void SetCurrentOperate(CurrentOperate operate);


        #region 文档操作
        void NewMapDoc();

        void OpenMapDocument();

        void SaveDocument();

        void SaveAsDocument();
        #endregion

        #region 地图放大缩小操作
        void MapFull();
        void MapZoomIn();
        void MapZoomOut();
        void MapPan();
        #endregion

        #region 地图数据操作
        void ClearMap();
        void ClearData();
        void DelelteElement();
        void SelectElement();
        void NoFunction();
        #endregion

        #region 显示操作
        void DisplayGementry(string layer, IGeometry geometry);

        void DisplayGeometries(string layer, params IGeometry[] geometris);

        void DisplayElement(string layer, IElement element);

        void DisplayElements(string layer, IElementCollection elements);
        #endregion
    }
}
