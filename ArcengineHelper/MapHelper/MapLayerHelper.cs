using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
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
    public static class MapLayerHelper
    {
        #region 查询图层
        /// <summary>
        /// 通过图层名称查询图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public static ILayer GetLyrByName(string layerName, IMap pMap)
        {
            try
            {
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    ESRI.ArcGIS.Carto.ILayer pLayer = pMap.get_Layer(i);
                    if (pLayer.Name == layerName)
                        return pLayer;
                    if (pLayer is IGroupLayer)
                    {
                        ICompositeLayer pCompLayer = pLayer as ICompositeLayer;
                        int n = pCompLayer.Count;
                        for (int j = 0; j < n; j++)
                        {
                            string strName = pCompLayer.get_Layer(j).Name;
                            if (strName == layerName)
                                return pCompLayer.get_Layer(j);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// 查询图层组
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public static IGroupLayer GetGrpByName(string layerName, IMap pMap)
        {
            try
            {
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    ESRI.ArcGIS.Carto.ILayer pLayer = pMap.get_Layer(i);

                    if (!(pLayer is IGroupLayer))
                        continue;

                    if (pLayer.Name == layerName)
                        return pLayer as IGroupLayer;

                    continue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        #endregion

        #region 查询要素图层
        /// <summary>
        /// 从map中获取要素图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public static IFeatureLayer GetFeatureLyrByName(string layerName, IMap pMap)
        {
            try
            {
                UID pUID = new UID();
                pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
                if (pMap.LayerCount <= 0) return null;

                IEnumLayer pEnumLayer = pMap.get_Layers(pUID, true);
                IFeatureLayer pFeatureLayer;
                pEnumLayer.Reset();

                //选择集是从多个图层中获取出来的值
                pFeatureLayer = (IFeatureLayer)pEnumLayer.Next();
                //对每一个图层进行循环，获取选择目标
                while (pFeatureLayer != null)
                {
                    if (layerName == pFeatureLayer.Name)
                        return pFeatureLayer;

                    pFeatureLayer = (IFeatureLayer)pEnumLayer.Next();

                }//循环对每一个FeatureLayer进行选择目标获取
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        /// <summary>
        /// 从map中获取要素图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public static List<IFeatureLayer> GetFeatureLyrByName(List<string> namelist, IMap pMap)
        {
            try
            {
                UID pUID = new UID();
                pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
                if (pMap.LayerCount <= 0) return null;

                IEnumLayer pEnumLayer = pMap.get_Layers(pUID, true);
                IFeatureLayer pFeatureLayer;
                pEnumLayer.Reset();

                //选择集是从多个图层中获取出来的值
                pFeatureLayer = (IFeatureLayer)pEnumLayer.Next();
                var featureLayerList = new List<IFeatureLayer>();
                //对每一个图层进行循环，获取选择目标
                while (pFeatureLayer != null)
                {
                    if (namelist.Contains(pFeatureLayer.Name))
                        featureLayerList.Add(pFeatureLayer);
                    if (namelist.Count == featureLayerList.Count)
                        return featureLayerList;
                    pFeatureLayer = (IFeatureLayer)pEnumLayer.Next();

                }//循环对每一个FeatureLayer进行选择目标获取
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        /// <summary>
        /// 从map中获取要素图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="layerName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static IFeatureLayer GetFeatureLyrByName(string layerName, string groupName, IMap pMap)
        {
            try
            {
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    ESRI.ArcGIS.Carto.ILayer pLayer = pMap.get_Layer(i);
                    if (pLayer is IGroupLayer)
                    {
                        if (pLayer.Name != groupName) continue;
                        ICompositeLayer pCompLayer = pLayer as ICompositeLayer;
                        int n = pCompLayer.Count;
                        for (int j = 0; j < n; j++)
                        {
                            string strName = pCompLayer.get_Layer(j).Name;
                            if (strName == layerName)
                                return pCompLayer.get_Layer(j) as ESRI.ArcGIS.Carto.IFeatureLayer;
                        }
                    }
                    else if (pLayer is IFeatureLayer)
                    {
                        ESRI.ArcGIS.Carto.IFeatureLayer pFeatureLayer = pLayer as ESRI.ArcGIS.Carto.IFeatureLayer;
                        if (pFeatureLayer.Name == layerName)
                            return pFeatureLayer;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        #endregion


        public static IGraphicsLayer FindOrCreateGraphicsSubLayer(string subLayerName, IMap pMap)
        {
            IGraphicsLayer gl = pMap.BasicGraphicsLayer;
            ICompositeGraphicsLayer cgl = gl as ICompositeGraphicsLayer;
            IGraphicsLayer sublayer;
            try
            {
                sublayer = cgl.FindLayer(subLayerName);//查找CompositeGraphicsLayer中有没有名为SubLayerName的GraphicsSubLayer  
                if (sublayer == null)
                    sublayer = cgl.AddLayer(subLayerName, null);//ICompositeGraphicsLayer.AddLayer方法其实返回的是一个GraphicsSubLayer的实例对象  
            }
            catch (Exception ex)//当图层中没有当前查询的图层时，com会报错
            {
                sublayer = cgl.AddLayer(subLayerName, null);//ICompositeGraphicsLayer.AddLayer方法其实返回的是一个GraphicsSubLayer的实例对象
            }
            return sublayer;
        }  

    }
}
