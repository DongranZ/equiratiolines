using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using System.Runtime.InteropServices;

namespace OGIS.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkSpaceAndFeatureHelper
    {
        #region 获取工作空间
        /// <summary>
        /// 打开工作空间
        /// </summary>
        /// <param name="strPath">文件路径</param>
        /// <param name="featureWorkspace">工作空间</param>
        /// <param name="type">工作空间类型FileGDB/Access/Shapefile</param>
        /// <returns></returns>
        public static bool OpenWorkspace(string strPath, out IFeatureWorkspace featureWorkspace, string type = "FileGDB")
        {
            IWorkspaceFactory2 pWF = null;
            try
            {
                if (!Directory.Exists(strPath))
                {
                    throw new Exception(strPath + "目录不存在");
                }

                if (type == "FileGDB")
                {
                    pWF = new FileGDBWorkspaceFactoryClass();
                }
                else if (type == "Access")
                {
                    pWF = new AccessWorkspaceFactoryClass();
                }
                else if (type == "Shapefile")
                {
                    pWF = new ShapefileWorkspaceFactoryClass();
                }


                //关闭资源锁定
                IWorkspaceFactoryLockControl ipWsFactoryLock;
                ipWsFactoryLock = (IWorkspaceFactoryLockControl)pWF;
                if (ipWsFactoryLock.SchemaLockingEnabled)
                {
                    ipWsFactoryLock.DisableSchemaLocking();
                }
                featureWorkspace = pWF.OpenFromFile(strPath, 0) as IFeatureWorkspace;
                if (featureWorkspace == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                featureWorkspace = null;
                e.ToString();
                return false;
            }
            finally
            {
                pWF = null;
            }
        }
        #endregion

        #region 构造筛选条件
        /// <summary>
        /// 构造查询过滤器，包括空间和属性查询,flag=0为选点，1为选线面
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="pGeo"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static IQueryFilter GetQueryFilterByWhereAndGeo(string whereStr, IGeometry pGeo, int flag)
        {
            IQueryFilter pQueryFilter;
            if (pGeo != null)
            {
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = pGeo;
                if (whereStr.Trim() != "")
                    spatialFilter.WhereClause = whereStr;
                if (flag == 0)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                }
                else if (flag == 1)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;//|;
                }
                else if (flag == 2)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelUndefined;//|;
                }
                else if (flag == 3)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelOverlaps;//|;
                }
                else
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelOverlaps;//| esriSpatialRelEnum.esriSpatialRelIntersects;
                }
                // spatialFilter.SubFields = "*";
                spatialFilter.GeometryField = "SHAPE";
                pQueryFilter = spatialFilter as IQueryFilter;
            }
            else if (!string.IsNullOrEmpty(whereStr))
            {
                pQueryFilter = new QueryFilter();
                string[] filter = whereStr.Split(';');
                pQueryFilter.WhereClause = filter[0];
                if (filter.Length > 1)
                {
                    IQueryFilterDefinition qfd = pQueryFilter as IQueryFilterDefinition;
                    qfd.PostfixClause = filter[1];
                }

            }
            else
            {

                pQueryFilter = null;
            }
            return pQueryFilter;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="pGeo"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static IQueryFilter GetQueryFilterByGeo(IGeometry pGeo, int flag)
        {
            IQueryFilter pQueryFilter;
            if (pGeo != null)
            {
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = pGeo;

                if (flag == 0)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                }
                else if (flag == 1)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;//|;
                }
                else
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;//| esriSpatialRelEnum.esriSpatialRelIntersects;
                }
                // spatialFilter.SubFields = "*";
                spatialFilter.GeometryField = "Shape";
                pQueryFilter = spatialFilter as IQueryFilter;
            }
            else
            {

                pQueryFilter = null;
            }
            return pQueryFilter;
        }
        #endregion


        /// <summary>
        /// 校验要素空间是否存 图层strLyrName
        /// </summary>
        /// <param name="featureWorkspace"></param>
        /// <param name="strLyrName"></param>
        /// <returns></returns>
        public static bool IsExistFeat(IFeatureWorkspace featureWorkspace, string strLyrName)
        {
            bool flag = false;
            IWorkspace pWK = featureWorkspace as IWorkspace;
            IEnumDataset pEnumDataset = pWK.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();

            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {

                flag = IsExistFeatInWF(pDataset, strLyrName);
                if (flag)
                    break;
                pDataset = pEnumDataset.Next();
            }
            return flag;
        }
        /// <summary>
        /// 校验要素空间是否存 图层strLyrName
        /// </summary>
        /// <param name="featureWorkspace"></param>
        /// <param name="strLyrName"></param>
        /// <returns></returns>
        public static bool IsExistDataSet(IFeatureWorkspace featureWorkspace, string strDataSetName)
        {
            bool flag = false;
            IWorkspace pWK = featureWorkspace as IWorkspace;
            IEnumDataset pEnumDataset = pWK.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();

            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {

                if (pDataset.Name == strDataSetName)
                {
                    flag = true;
                }
                if (flag)
                    break;
                pDataset = pEnumDataset.Next();
            }
            return flag;
        }
        /// <summary>
        /// 遍历校验
        /// </summary>
        /// <param name="pDataset"></param>
        /// <param name="strLyrName"></param>
        /// <returns></returns>
        private static bool IsExistFeatInWF(IDataset pDataset, string strLyrName)
        {
            bool flag = false;

            if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
            {
                string strTemp = pDataset.Name;
                if (strTemp == strLyrName)
                {
                    flag = true;

                }
            }
            else if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
            {
                IEnumDataset pEnumDataset = pDataset.Subsets;
                pEnumDataset.Reset();
                IDataset pTempDataset = pEnumDataset.Next();

                while (pTempDataset != null)
                {
                    flag = IsExistFeatInWF(pTempDataset, strLyrName);
                    if (flag == true)
                        break;
                    pTempDataset = pEnumDataset.Next();
                }
            }
            return flag;
        }
        public static List<string> GetFeatureClassNameInWorkspace(IFeatureWorkspace featureWorkspace)
        {
            var inList = new List<string>();
            IWorkspace pWK = featureWorkspace as IWorkspace;
            IEnumDataset pEnumDataset = pWK.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();

            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {

                IsExistFeatInWF(pDataset, inList);
                pDataset = pEnumDataset.Next();
            }
            return inList;
        }
        private static void IsExistFeatInWF(IDataset pDataset, List<string> inList)
        {
            if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
            {
                string strTemp = pDataset.Name;
                if (!inList.Contains(strTemp))
                    inList.Remove(strTemp);
                return;
            }
            else if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
            {
                IEnumDataset pEnumDataset = pDataset.Subsets;
                pEnumDataset.Reset();
                IDataset pTempDataset = pEnumDataset.Next();

                while (pTempDataset != null)
                {
                    IsExistFeatInWF(pTempDataset, inList);
                    pTempDataset = pEnumDataset.Next();
                }
            }
        }
        //创建gdb图层
        public static IFeatureClass CreateFeatureClassOnWorkspace(IFeatureWorkspace featureWorkspace, string layerName, esriGeometryType type, ISpatialReference spatialReference)
        {
            IFields pFields;
            IFeatureClass pFeatCls;
            try
            {
                if (IsExistFeat(featureWorkspace, layerName))
                {
                    pFeatCls = featureWorkspace.OpenFeatureClass(layerName);
                    return pFeatCls;
                }


                pFields = new FieldsClass();

                IField field = new FieldClass();
                IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                IFieldEdit fieldEdit = field as IFieldEdit;


                fieldEdit.Name_2 = "OBJECTID";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit.IsNullable_2 = false;
                fieldEdit.Required_2 = false;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                IGeometryDef geoDef = new GeometryDefClass();
                IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                geoDefEdit.AvgNumPoints_2 = 5;
                geoDefEdit.GeometryType_2 = type;
                geoDefEdit.GridCount_2 = 1;
                geoDefEdit.HasM_2 = false;
                geoDefEdit.HasZ_2 = false;
                geoDefEdit.SpatialReference_2 = spatialReference;

                fieldEdit.Name_2 = "SHAPE";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEdit.GeometryDef_2 = geoDef;
                fieldEdit.IsNullable_2 = true;
                fieldEdit.Required_2 = true;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "NAME";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "TYPE";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "ID";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                //创建要素类                
                pFeatCls = featureWorkspace.CreateFeatureClass(
                    layerName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                //featureWorkspace.CreateFeatureDataset(layerName, spatialReference);
                return pFeatCls;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public static IFeatureClass GreateFeatureClassWithGroupOnWorkspace(IFeatureWorkspace featureWorkspace, string groupName, string layerName, esriGeometryType type, ISpatialReference spatialReference, List<string> fieds = null)
        {
            IFeatureDataset pFeatDst;
            IFields pFields;
            IFeatureClass pFeatCls;
            try
            {
                if (IsExistDataSet(featureWorkspace, groupName))
                {
                    pFeatDst = featureWorkspace.OpenFeatureDataset(groupName);
                }
                else
                {
                    pFeatDst = featureWorkspace.CreateFeatureDataset(groupName, spatialReference);
                }
                if (IsExistFeat(featureWorkspace, layerName))
                {
                    pFeatCls = featureWorkspace.OpenFeatureClass(layerName);
                    return pFeatCls;
                }

                pFields = new FieldsClass();

                IField field = new FieldClass();
                IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                IFieldEdit fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "OBJECTID";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit.IsNullable_2 = false;
                fieldEdit.Required_2 = false;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                IGeometryDef geoDef = new GeometryDefClass();
                IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                geoDefEdit.AvgNumPoints_2 = 5;
                geoDefEdit.GeometryType_2 = type;
                geoDefEdit.GridCount_2 = 1;
                geoDefEdit.HasM_2 = false;
                geoDefEdit.HasZ_2 = false;
                geoDefEdit.SpatialReference_2 = spatialReference;

                fieldEdit.Name_2 = "SHAPE";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEdit.GeometryDef_2 = geoDef;
                fieldEdit.IsNullable_2 = true;
                fieldEdit.Required_2 = true;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "NAME";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "INDEX";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                field = new FieldClass();
                fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "TYPE";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                if (fieds != null && fieds.Count > 0)
                {
                    for (var index = 0; index < fieds.Count; index++)
                    {
                        if (fieds[index].Trim() == "OBJECTID" || fieds[index].Trim() == "SHAPE" || fieds[index].Trim() == "NAME" || fieds[index].Trim() == "INDEX" || fieds[index].Trim() == "TYPE")
                            continue;
                        field = new FieldClass();
                        fieldEdit = field as IFieldEdit;
                        fieldEdit.Name_2 = fieds[index].Trim();
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        fieldEdit.IsNullable_2 = true;
                        fieldsEdit.AddField(field);
                    }
                }

                //创建要素类   
                IFeatureClassDescription fcDesc = new FeatureClassDescriptionClass();
                IObjectClassDescription ocDesc = (IObjectClassDescription)fcDesc;
                pFeatCls = pFeatDst.CreateFeatureClass(layerName, pFields, ocDesc.InstanceCLSID, ocDesc.ClassExtensionCLSID, esriFeatureType.esriFTSimple, "SHAPE", "");
                //pFeatCls = pFeatDst.CreateFeatureClass(layerName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                return pFeatCls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 增
        public static bool InsertFeatureOnWorkspace(IFeatureWorkspace featureWorkspace, IGeometry pGeo, string strLyrName)
        {
            bool success = false;
            try
            {
                IFeatureClass pFeatCls = featureWorkspace.OpenFeatureClass(strLyrName);

                if (pFeatCls != null)
                {

                    IFeature pFeature = pFeatCls.CreateFeature();
                    pFeature.Shape = pGeo;
                    pFeature.Store();

                    success = true;
                    return success;
                }
                success = false;
            }
            catch (Exception ex)
            {
                success = false;
                throw ex;
            }
            return success;

        }
        /// <summary>
        /// 工作空间生成要素    方法当前未被使用
        /// </summary>
        /// <param name="strPath">划界工程路径</param>
        /// <param name="pGeo"></param>
        /// <param name="strLyrName">图层名称</param>
        public static bool CreateFeatureOnWorkspace(string strPath, IGeometry pGeo, string strLyrName)
        {
            IWorkspaceFactory2 pWF;
            pWF = new FileGDBWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace = pWF.OpenFromFile(strPath, 0) as IFeatureWorkspace;

            bool success = false;
            try
            {
                IFeatureClass pFeatCls = pFeatureWorkspace.OpenFeatureClass(strLyrName);

                if (pFeatCls != null)
                {

                    IFeature pFeature = pFeatCls.CreateFeature();
                    pFeature.Shape = pGeo;
                    pFeature.Store();

                    success = true;
                    return success;
                }
                success = false;
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception(ex.ToString());
            }
            finally
            {
                pWF = null;
                pFeatureWorkspace = null;
            }
            return success;

        }

        /// <summary>
        /// 添加单个要素至指定图层
        /// </summary>
        /// <param name="strLayer"></param>
        /// <param name="pGeo"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static bool AddSingleGeomFeatureToLayar(IFeatureWorkspace featureWorkspace, string layerName, IGeometry pGeo, string strName, int nType)
        {
            bool success = false;
            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur;
            IFeatureBuffer pFeatBuf;
            IFields pFields;

            try
            {
                pFeatCls = featureWorkspace.OpenFeatureClass(layerName);
                ITopologicalOperator pTP = pGeo as ITopologicalOperator;
                pTP.Simplify();
                if (pFeatCls != null)
                {
                    pFeatCur = pFeatCls.Insert(true);
                    pFeatBuf = pFeatCls.CreateFeatureBuffer();

                    int index;
                    pFields = pFeatCls.Fields;
                    index = pFields.FindField("SHAPE");
                    pFeatBuf.set_Value(index, pGeo);

                    index = pFields.FindField("NAME");
                    if (index >= 0)
                        pFeatBuf.set_Value(index, strName);

                    index = pFields.FindField("TYPE");
                    if (index >= 0)
                        pFeatBuf.set_Value(index, nType);

                    pFeatCur.InsertFeature(pFeatBuf);

                    pFeatCur.Flush();

                    Marshal.ReleaseComObject(pFeatCur);
                    GC.Collect();
                }
                success = true;
                return success;
            }

            catch (Exception ex)
            {
                success = false;
                return success;
                //throw new Exception(ex.ToString());
            }
            finally
            {
                pFeatCls = null;
                pFeatCur = null;
                pFeatBuf = null;
                pFields = null;
            }

        }
        ///// <summary>
        ///// 添加多要素至指定图层
        ///// </summary>
        ///// <param name="strLayer"></param>
        ///// <param name="pGeo"></param>
        ///// <param name="strCode"></param>
        ///// <returns></returns>
        //public static bool AddMultipleGeomFeatureToLayar(IFeatureWorkspace featureWorkspace, string layerName, IGeometry pGeo, string strName, int nType)
        //{
        //    bool success = false;
        //    IFeatureClass pFeatCls;

        //    IFeatureCursor pFeatCur;
        //    IFeatureBuffer pFeatBuf;

        //    IFields pFields;

        //    try
        //    {
        //        pFeatCls = featureWorkspace.OpenFeatureClass(layerName);
        //        ITopologicalOperator pTP = pGeo as ITopologicalOperator;
        //        pTP.Simplify();
        //        if (pFeatCls != null)
        //        {
        //            pFeatCur = pFeatCls.Insert(true);
        //            pFeatBuf = pFeatCls.CreateFeatureBuffer();

        //            int index;
        //            pFields = pFeatCls.Fields;
        //            index = pFields.FindField("SHAPE");
        //            pFeatBuf.set_Value(index, pGeo);

        //            index = pFields.FindField("NAME");
        //            if (index >= 0)
        //                pFeatBuf.set_Value(index, strName);

        //            index = pFields.FindField("TYPE");
        //            if (index >= 0)
        //                pFeatBuf.set_Value(index, nType);

        //            pFeatCur.InsertFeature(pFeatBuf);

        //            pFeatCur.Flush();

        //            Marshal.ReleaseComObject(pFeatCur);
        //            GC.Collect();
        //        }

        //        success = true;
        //        return success;
        //    }

        //    catch (Exception ex)
        //    {
        //        success = false;
        //        throw new Exception(ex.ToString());

        //    }
        //    finally
        //    {
        //        pFeatCls = null;

        //        pFeatCur = null;
        //        pFeatBuf = null;

        //        pFields = null;

        //    }

        //}
        /// <summary>
        /// 添加多要素至指定图层
        /// </summary>
        /// <param name="strLayer"></param>
        /// <param name="pGeo"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static bool AddMultipleGeomFeatureToLayar(IFeatureWorkspace featureWorkspace, string layerName, List<IGeometry> pGeoList, int nType)
        {
            bool success = false;
            IFeatureClass pFeatCls;

            IFeatureCursor pFeatCur;
            IFeatureBuffer pFeatBuf;

            IFields pFields;

            try
            {
                pFeatCls = featureWorkspace.OpenFeatureClass(layerName);
                if (pFeatCls != null)
                {
                    pFeatCur = pFeatCls.Insert(true);
                    for (int indexList = 0; indexList < pGeoList.Count; indexList++)
                    {
                        pFeatBuf = pFeatCls.CreateFeatureBuffer();
                        IGeometry pGeo = pGeoList[indexList];
                        ITopologicalOperator pTP = pGeo as ITopologicalOperator;
                        pTP.Simplify();
                        int index;
                        pFields = pFeatCls.Fields;
                        index = pFields.FindField("SHAPE");
                        pFeatBuf.set_Value(index, pGeo);

                        index = pFields.FindField("NAME");
                        if (index >= 0)
                            pFeatBuf.set_Value(index, indexList);

                        index = pFields.FindField("TYPE");
                        if (index >= 0)
                            pFeatBuf.set_Value(index, nType);

                        pFeatCur.InsertFeature(pFeatBuf);
                        pFeatCur.Flush();
                    }

                    Marshal.ReleaseComObject(pFeatCur);
                    GC.Collect();
                }

                success = true;
                return success;
            }

            catch (Exception ex)
            {
                success = false;
                throw new Exception(ex.ToString());

            }
            finally
            {
                pFeatCls = null;

                pFeatCur = null;
                pFeatBuf = null;

                pFields = null;

            }

        }


        /// <summary>
        /// 用于数据存入shp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="oceanEntity"></param>
        /// <param name="spatialReference"></param>
        /// <returns></returns>
        public static IFeatureClass CreateFeatureOnWorkspace(string filePath, string fileName, AOceanEntity oceanEntity, ISpatialReference spatialReference, bool isGroup = true)
        {
            var fildEntity = oceanEntity as FeatureFieldEntity;
            if (fildEntity == null)
                return null;

            IWorkspaceFactory2 pWF = new ShapefileWorkspaceFactoryClass();
            IWorkspace workSpace;
            IFeatureWorkspace pFeatureWorkspace;
            //关闭资源锁定
            IWorkspaceFactoryLockControl ipWsFactoryLock;
            ipWsFactoryLock = (IWorkspaceFactoryLockControl)pWF;
            if (ipWsFactoryLock.SchemaLockingEnabled)
            {
                ipWsFactoryLock.DisableSchemaLocking();
            }


            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur;
            IFeatureBuffer pFeatBuf;
            IFields pFields;
            try
            {

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var filePthFull = filePath;
                if (isGroup)
                    filePthFull = filePath + "\\" + fileName;

                if (Directory.Exists(filePath + "\\" + fileName))
                    filePthFull = filePath + "\\" + fileName;
                if (!File.Exists(filePthFull + "\\" + fileName + ".shp"))
                {
                    #region 第一次时 新增
                    IWorkspaceName pWSName = pWF.Create(filePthFull, fileName, null, 0);
                    IName pName = (IName)pWSName;
                    workSpace = (IWorkspace)pName.Open();
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFields = new FieldsClass();

                    IField field = new FieldClass();
                    IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                    IFieldEdit fieldEdit = field as IFieldEdit;


                    fieldEdit.Name_2 = "OBJECTID";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                    fieldEdit.IsNullable_2 = false;
                    fieldEdit.Required_2 = false;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    IGeometryDef geoDef = new GeometryDefClass();
                    IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                    geoDefEdit.AvgNumPoints_2 = 5;
                    geoDefEdit.GeometryType_2 = fildEntity.Geometry.GeometryType;
                    geoDefEdit.GridCount_2 = 1;
                    geoDefEdit.HasM_2 = false;
                    geoDefEdit.HasZ_2 = false;
                    geoDefEdit.SpatialReference_2 = spatialReference;

                    fieldEdit.Name_2 = "SHAPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                    fieldEdit.GeometryDef_2 = geoDef;
                    fieldEdit.IsNullable_2 = true;
                    fieldEdit.Required_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "Name";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "Type";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "ID";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    //创建要素类                
                    pFeatCls = pFeatureWorkspace.CreateFeatureClass(
                        fileName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                    //IFeature pFeature = pFeatCls.CreateFeature();                    
                    //pFeature.Shape = fildEntity.Geometry;
                    //pFeature.Store();
                    IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo("", null, 1);
                    var t = pFeatCls.FeatureCount(pQueryFilter);
                    ITopologicalOperator pTP = fildEntity.Geometry as ITopologicalOperator;
                    pTP.Simplify();
                    if (pFeatCls != null)
                    {

                        pFeatCur = pFeatCls.Insert(true);
                        pFeatBuf = pFeatCls.CreateFeatureBuffer();

                        int index;
                        pFields = pFeatCls.Fields;
                        index = pFields.FindField("SHAPE");
                        pFeatBuf.set_Value(index, fildEntity.Geometry);

                        index = pFields.FindField("Name");
                        pFeatBuf.set_Value(index, fileName);

                        index = pFields.FindField("Type");
                        pFeatBuf.set_Value(index, fildEntity.Type);

                        index = pFields.FindField("ID");
                        pFeatBuf.set_Value(index, fildEntity.Id);

                        pFeatCur.InsertFeature(pFeatBuf);

                        pFeatCur.Flush();

                        Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                        t = pFeatCls.FeatureCount(pQueryFilter);
                    }

                    #endregion

                }
                else
                {
                    #region 修改
                    workSpace = pWF.OpenFromFile(filePthFull, 0);
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFeatCls = pFeatureWorkspace.OpenFeatureClass(fileName);
                    if (pFeatCls != null)
                    {
                        string whereStr = "Type=" + "'" + fildEntity.Type + "'";
                        IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo(whereStr, null, 1);
                        pFeatCur = pFeatCls.Update(pQueryFilter, false);
                        IFeature pFeature = pFeatCur.NextFeature();
                        int index;
                        if (pFeature != null && fildEntity.IsInsert == false)//如果存在，则替换
                        {
                            pFields = pFeatCls.Fields;
                            index = pFields.FindField("SHAPE");
                            pFeature.set_Value(index, fildEntity.Geometry);
                            pFeatCur.UpdateFeature(pFeature);
                            pFeatCur.Flush();
                            Marshal.ReleaseComObject(pFeatCur);
                            return pFeatCls;
                        }
                        //else//否则新增
                        //{
                        ITopologicalOperator pTP = fildEntity.Geometry as ITopologicalOperator;
                        //if (pTP!=null)
                        //pTP.Simplify();

                        pFeatCur = pFeatCls.Insert(true);
                        pFeatBuf = pFeatCls.CreateFeatureBuffer();


                        pFields = pFeatCls.Fields;
                        index = pFields.FindField("SHAPE");
                        pFeatBuf.set_Value(index, fildEntity.Geometry);

                        index = pFields.FindField("Name");
                        pFeatBuf.set_Value(index, fileName);

                        index = pFields.FindField("Type");
                        pFeatBuf.set_Value(index, fildEntity.Type);

                        index = pFields.FindField("ID");
                        pFeatBuf.set_Value(index, fildEntity.Id);

                        pFeatCur.InsertFeature(pFeatBuf);//提交
                        pFeatCur.Flush();
                        Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                        //}
                    }
                    #endregion
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSpace);
                return pFeatCls;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

                pFields = null;
                pFeatureWorkspace = null;
                workSpace = null;
                pWF = null;

                pFeatCur = null;
                pFeatBuf = null;
                pFields = null;

            }

        }
        /// <summary>
        /// 用于数据存入shp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="oceanEntity"></param>
        /// <param name="spatialReference"></param>
        /// <returns></returns>
        public static IFeatureClass CreateFeaturesOnWorkspace(string filePath, string fileName, AOceanEntity oceanEntity, ISpatialReference spatialReference, bool isGroup = true)
        {
            var fildEntity = oceanEntity as FeaturesFieldEntity;
            if (fildEntity == null || fildEntity.GeometryList == null || fildEntity.GeometryList.Count <= 0)
                return null;

            IWorkspaceFactory2 pWF = new ShapefileWorkspaceFactoryClass();
            IWorkspace workSpace;
            IFeatureWorkspace pFeatureWorkspace;
            //关闭资源锁定
            IWorkspaceFactoryLockControl ipWsFactoryLock;
            ipWsFactoryLock = (IWorkspaceFactoryLockControl)pWF;
            if (ipWsFactoryLock.SchemaLockingEnabled)
            {
                ipWsFactoryLock.DisableSchemaLocking();
            }


            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur = null;
            IFeatureBuffer pFeatBuf;
            IFields pFields;
            try
            {

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var filePthFull = filePath;
                if (isGroup)
                    filePthFull = filePath + "\\" + fileName;

                if (Directory.Exists(filePath + "\\" + fileName))
                    filePthFull = filePath + "\\" + fileName;
                if (!File.Exists(filePthFull + "\\" + fileName + ".shp"))
                {
                    #region 第一次时 新增
                    IWorkspaceName pWSName = pWF.Create(filePthFull, fileName, null, 0);
                    IName pName = (IName)pWSName;
                    workSpace = (IWorkspace)pName.Open();
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFields = new FieldsClass();

                    IField field = new FieldClass();
                    IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                    IFieldEdit fieldEdit = field as IFieldEdit;


                    fieldEdit.Name_2 = "OBJECTID";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                    fieldEdit.IsNullable_2 = false;
                    fieldEdit.Required_2 = false;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    IGeometryDef geoDef = new GeometryDefClass();
                    IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                    geoDefEdit.AvgNumPoints_2 = 5;
                    geoDefEdit.GeometryType_2 = fildEntity.GeometryType;
                    geoDefEdit.GridCount_2 = 1;
                    geoDefEdit.HasM_2 = false;
                    geoDefEdit.HasZ_2 = false;
                    geoDefEdit.SpatialReference_2 = spatialReference;

                    fieldEdit.Name_2 = "SHAPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                    fieldEdit.GeometryDef_2 = geoDef;
                    fieldEdit.IsNullable_2 = true;
                    fieldEdit.Required_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "Name";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "Type";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "ID";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    //创建要素类                
                    pFeatCls = pFeatureWorkspace.CreateFeatureClass(
                        fileName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                    //IFeature pFeature = pFeatCls.CreateFeature();                    
                    //pFeature.Shape = fildEntity.Geometry;
                    //pFeature.Store();
                    //IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo("", null, 1);
                    //var t = pFeatCls.FeatureCount(pQueryFilter);

                    if (pFeatCls != null)
                    {
                        for (var j = 0; j < fildEntity.GeometryList.Count; j++)
                        {
                            ITopologicalOperator pTP = fildEntity.GeometryList[j] as ITopologicalOperator;
                            pTP.Simplify();

                            pFeatCur = pFeatCls.Insert(true);
                            pFeatBuf = pFeatCls.CreateFeatureBuffer();

                            int index;
                            pFields = pFeatCls.Fields;
                            index = pFields.FindField("SHAPE");
                            pFeatBuf.set_Value(index, fildEntity.GeometryList[j]);

                            //index = pFields.FindField("Name");
                            //pFeatBuf.set_Value(index, fileName);

                            //index = pFields.FindField("Type");
                            //pFeatBuf.set_Value(index, fildEntity.Type);

                            //index = pFields.FindField("ID");
                            //pFeatBuf.set_Value(index, fildEntity.Id);

                            pFeatCur.InsertFeature(pFeatBuf);
                            pFeatCur.Flush();
                        }
                        if (pFeatCur != null)
                            Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                        //t = pFeatCls.FeatureCount(pQueryFilter);
                    }

                    #endregion

                }
                else
                {
                    #region 修改
                    workSpace = pWF.OpenFromFile(filePthFull, 0);
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFeatCls = pFeatureWorkspace.OpenFeatureClass(fileName);
                    if (pFeatCls != null)
                    {
                        if (fildEntity.IsInsert == false)//如果存在，则替换
                        {
                            IDataset pds = pFeatCls as IDataset;
                            pds.Delete();
                        }
                        for (var j = 0; j < fildEntity.GeometryList.Count; j++)
                        {
                            ITopologicalOperator pTP = fildEntity.GeometryList[j] as ITopologicalOperator;

                            pFeatCur = pFeatCls.Insert(true);
                            pFeatBuf = pFeatCls.CreateFeatureBuffer();


                            pFields = pFeatCls.Fields;
                            int index = pFields.FindField("SHAPE");
                            pFeatBuf.set_Value(index, fildEntity.GeometryList[j]);

                            //index = pFields.FindField("Name");
                            //pFeatBuf.set_Value(index, fileName);

                            //index = pFields.FindField("Type");
                            //pFeatBuf.set_Value(index, fildEntity.Type);

                            //index = pFields.FindField("ID");
                            //pFeatBuf.set_Value(index, fildEntity.Id);

                            pFeatCur.InsertFeature(pFeatBuf);//提交
                            pFeatCur.Flush();
                        }
                        if (pFeatCur != null)
                            Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                    }
                    #endregion
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSpace);
                return pFeatCls;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

                pFields = null;
                pFeatureWorkspace = null;
                workSpace = null;
                pWF = null;

                pFeatCur = null;
                pFeatBuf = null;
                pFields = null;

            }

        }

        /// <summary>
        /// 用于数据存入shp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="oceanEntity"></param>
        /// <param name="spatialReference"></param>
        /// <returns></returns>
        public static IFeatureClass CreateFeatureOnShpWorkspace(string filePath, string fileName, AOceanEntity oceanEntity, string groupName)
        {
            var fildEntity = oceanEntity as FeatureFieldExtendEntity;
            if (fildEntity == null)
                return null;

            IWorkspaceFactory2 pWF = new ShapefileWorkspaceFactoryClass();
            IWorkspace workSpace;
            IFeatureWorkspace pFeatureWorkspace;
            //关闭资源锁定
            IWorkspaceFactoryLockControl ipWsFactoryLock;
            ipWsFactoryLock = (IWorkspaceFactoryLockControl)pWF;
            if (ipWsFactoryLock.SchemaLockingEnabled)
            {
                ipWsFactoryLock.DisableSchemaLocking();
            }


            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur;
            IFeatureBuffer pFeatBuf;
            IFields pFields;
            try
            {

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var filePthFull = filePath;
                if (!string.IsNullOrEmpty(groupName))
                    filePthFull = filePath + "\\" + groupName;
                if (!Directory.Exists(filePthFull))
                    Directory.CreateDirectory(filePthFull);

                //if (Directory.Exists(filePthFull + "\\" + fileName))
                //    filePthFull = filePthFull + "\\" + fileName;
                if (!File.Exists(filePthFull + "\\" + fileName + ".shp"))
                {
                    #region 第一次时 新增
                    IWorkspaceName pWSName = pWF.Create(filePthFull, fileName, null, 0);
                    IName pName = (IName)pWSName;
                    workSpace = (IWorkspace)pName.Open();
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFields = new FieldsClass();

                    IField field = new FieldClass();
                    IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                    IFieldEdit fieldEdit = field as IFieldEdit;


                    fieldEdit.Name_2 = "OBJECTID";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                    fieldEdit.IsNullable_2 = false;
                    fieldEdit.Required_2 = false;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    IGeometryDef geoDef = new GeometryDefClass();
                    IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                    geoDefEdit.AvgNumPoints_2 = 5;
                    geoDefEdit.GeometryType_2 = fildEntity.Geometry.GeometryType;
                    geoDefEdit.GridCount_2 = 1;
                    geoDefEdit.HasM_2 = false;
                    geoDefEdit.HasZ_2 = false;
                    geoDefEdit.SpatialReference_2 = fildEntity.SpatialReference;

                    fieldEdit.Name_2 = "SHAPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                    fieldEdit.GeometryDef_2 = geoDef;
                    fieldEdit.IsNullable_2 = true;
                    fieldEdit.Required_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "NAME";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "COUNTRY";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "TYPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    if (fildEntity.OtherFields != null)
                    {
                        foreach (var item in fildEntity.OtherFields)
                        {
                            if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE" || item.Key.ToUpper() == "COUNTRY")
                                continue;
                            field = new FieldClass();
                            fieldEdit = field as IFieldEdit;
                            fieldEdit.Name_2 = item.Key.ToUpper();
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit.IsNullable_2 = true;
                            fieldsEdit.AddField(field);
                        }
                    }

                    //创建要素类                
                    pFeatCls = pFeatureWorkspace.CreateFeatureClass(
                        fileName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                    //IFeature pFeature = pFeatCls.CreateFeature();                    
                    //pFeature.Shape = fildEntity.Geometry;
                    //pFeature.Store();
                    IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo("", null, 1);
                    var t = pFeatCls.FeatureCount(pQueryFilter);
                    ITopologicalOperator pTP = fildEntity.Geometry as ITopologicalOperator;
                    pTP.Simplify();
                    if (pFeatCls != null)
                    {

                        pFeatCur = pFeatCls.Insert(true);
                        pFeatBuf = pFeatCls.CreateFeatureBuffer();

                        int index;
                        pFields = pFeatCls.Fields;
                        index = pFields.FindField("SHAPE");
                        pFeatBuf.set_Value(index, fildEntity.Geometry);

                        index = pFields.FindField("NAME");
                        pFeatBuf.set_Value(index, fileName);

                        index = pFields.FindField("COUNTRY");
                        pFeatBuf.set_Value(index, fildEntity.Country);

                        index = pFields.FindField("TYPE");
                        pFeatBuf.set_Value(index, fildEntity.Type);

                        if (fildEntity.OtherFields != null)
                        {
                            foreach (var item in fildEntity.OtherFields)
                            {
                                if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE")
                                    continue;
                                index = pFields.FindField(item.Key.ToUpper());
                                if (index < 0) continue;
                                pFeatBuf.set_Value(index, item.Value);
                            }
                        }

                        pFeatCur.InsertFeature(pFeatBuf);

                        pFeatCur.Flush();

                        Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                        t = pFeatCls.FeatureCount(pQueryFilter);
                    }

                    #endregion

                }
                else
                {
                    #region 修改
                    workSpace = pWF.OpenFromFile(filePthFull, 0);
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFeatCls = pFeatureWorkspace.OpenFeatureClass(fileName);
                    if (pFeatCls != null)
                    {
                        string whereStr = "TYPE=" + "'" + fildEntity.Type + "'";
                        IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo(whereStr, null, 1);
                        pFeatCur = pFeatCls.Update(pQueryFilter, false);
                        IFeature pFeature = pFeatCur.NextFeature();
                        int index;
                        if (pFeature != null && fildEntity.IsInsert == false)//如果存在，则替换
                        {
                            pFields = pFeatCls.Fields;
                            index = pFields.FindField("SHAPE");
                            pFeature.set_Value(index, fildEntity.Geometry);
                            pFeatCur.UpdateFeature(pFeature);
                            pFeatCur.Flush();
                            Marshal.ReleaseComObject(pFeatCur);
                            return pFeatCls;
                        }
                        //else//否则新增
                        //{
                        ITopologicalOperator pTP = fildEntity.Geometry as ITopologicalOperator;
                        //if (pTP!=null)
                        //pTP.Simplify();

                        pFeatCur = pFeatCls.Insert(true);
                        pFeatBuf = pFeatCls.CreateFeatureBuffer();


                        pFields = pFeatCls.Fields;
                        index = pFields.FindField("SHAPE");
                        pFeatBuf.set_Value(index, fildEntity.Geometry);

                        index = pFields.FindField("NAME");
                        pFeatBuf.set_Value(index, fileName);

                        index = pFields.FindField("COUNTRY");
                        pFeatBuf.set_Value(index, fildEntity.Country);

                        index = pFields.FindField("TYPE");
                        pFeatBuf.set_Value(index, fildEntity.Type);

                        if (fildEntity.OtherFields != null)
                        {
                            foreach (var item in fildEntity.OtherFields)
                            {
                                if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE" || item.Key.ToUpper() == "COUNTRY")
                                    continue;
                                index = pFields.FindField(item.Key.ToUpper());
                                pFeatBuf.set_Value(index, item.Value);
                            }
                        }

                        pFeatCur.InsertFeature(pFeatBuf);//提交
                        pFeatCur.Flush();
                        Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                        //}
                    }
                    #endregion
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSpace);
                return pFeatCls;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

                pFields = null;
                pFeatureWorkspace = null;
                workSpace = null;
                pWF = null;

                pFeatCur = null;
                pFeatBuf = null;
                pFields = null;

            }

        }

        /// <summary>
        /// 用于数据存入shp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="oceanEntity"></param>
        /// <param name="spatialReference"></param>
        /// <returns></returns>
        public static IFeatureClass CreateMultFeatureOnShpWorkspace(string filePath, string fileName, AOceanEntity oceanEntity, string groupName)
        {
            var fildEntity = oceanEntity as FeatureFieldExtendEntity;
            if (fildEntity == null)
                return null;

            IWorkspaceFactory2 pWF = new ShapefileWorkspaceFactoryClass();
            IWorkspace workSpace;
            IFeatureWorkspace pFeatureWorkspace;
            //关闭资源锁定
            IWorkspaceFactoryLockControl ipWsFactoryLock;
            ipWsFactoryLock = (IWorkspaceFactoryLockControl)pWF;
            if (ipWsFactoryLock.SchemaLockingEnabled)
            {
                ipWsFactoryLock.DisableSchemaLocking();
            }


            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur;
            IFeatureBuffer pFeatBuf;
            IFields pFields;
            try
            {

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var filePthFull = filePath;
                if (!string.IsNullOrEmpty(groupName))
                    filePthFull = filePath + "\\" + groupName;
                if (!Directory.Exists(filePthFull))
                    Directory.CreateDirectory(filePthFull);

                //if (Directory.Exists(filePthFull + "\\" + fileName))
                //    filePthFull = filePthFull + "\\" + fileName;
                if (!File.Exists(filePthFull + "\\" + fileName + ".shp"))
                {
                    #region 第一次时 新增
                    IWorkspaceName pWSName = pWF.Create(filePthFull, fileName, null, 0);
                    IName pName = (IName)pWSName;
                    workSpace = (IWorkspace)pName.Open();
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFields = new FieldsClass();

                    IField field = new FieldClass();
                    IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                    IFieldEdit fieldEdit = field as IFieldEdit;


                    fieldEdit.Name_2 = "OBJECTID";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                    fieldEdit.IsNullable_2 = false;
                    fieldEdit.Required_2 = false;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    IGeometryDef geoDef = new GeometryDefClass();
                    IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                    geoDefEdit.AvgNumPoints_2 = 5;
                    geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline
;
                    geoDefEdit.GridCount_2 = 1;
                    geoDefEdit.HasM_2 = false;
                    geoDefEdit.HasZ_2 = false;
                    geoDefEdit.SpatialReference_2 = fildEntity.SpatialReference;

                    fieldEdit.Name_2 = "SHAPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                    fieldEdit.GeometryDef_2 = geoDef;
                    fieldEdit.IsNullable_2 = true;
                    fieldEdit.Required_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "NAME";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "COUNTRY";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "TYPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    if (fildEntity.OtherFields != null)
                    {
                        foreach (var item in fildEntity.OtherFields)
                        {
                            if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE" || item.Key.ToUpper() == "COUNTRY")
                                continue;
                            field = new FieldClass();
                            fieldEdit = field as IFieldEdit;
                            fieldEdit.Name_2 = item.Key.ToUpper();
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit.IsNullable_2 = true;
                            fieldsEdit.AddField(field);
                        }
                    }

                    //创建要素类                
                    pFeatCls = pFeatureWorkspace.CreateFeatureClass(
                        fileName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                    //IFeature pFeature = pFeatCls.CreateFeature();                    
                    //pFeature.Shape = fildEntity.Geometry;
                    //pFeature.Store();
                    IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo("", null, 1);
                    var t = pFeatCls.FeatureCount(pQueryFilter);

                    if (pFeatCls != null)
                    {
                        pFeatCur = pFeatCls.Insert(true);
                        foreach (var line in fildEntity.ListLine)
                        {
                            if (line == null || line.IsEmpty)
                                continue;
                            pFeatBuf = pFeatCls.CreateFeatureBuffer();
                            ITopologicalOperator pTP = line as ITopologicalOperator;
                            pTP.Simplify();
                            int index;
                            pFields = pFeatCls.Fields;
                            index = pFields.FindField("SHAPE");
                            pFeatBuf.set_Value(index, line);

                            index = pFields.FindField("NAME");
                            pFeatBuf.set_Value(index, fileName);

                            index = pFields.FindField("COUNTRY");
                            pFeatBuf.set_Value(index, fildEntity.Country);

                            index = pFields.FindField("TYPE");
                            pFeatBuf.set_Value(index, fildEntity.Type);

                            if (fildEntity.OtherFields != null)
                            {
                                foreach (var item in fildEntity.OtherFields)
                                {
                                    if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE")
                                        continue;
                                    index = pFields.FindField(item.Key.ToUpper());
                                    pFeatBuf.set_Value(index, item.Value);
                                }
                            }

                            pFeatCur.InsertFeature(pFeatBuf);
                            pFeatCur.Flush();
                        }



                        Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                        t = pFeatCls.FeatureCount(pQueryFilter);
                    }

                    #endregion

                }
                else
                {
                    #region 修改
                    workSpace = pWF.OpenFromFile(filePthFull, 0);
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFeatCls = pFeatureWorkspace.OpenFeatureClass(fileName);
                    if (pFeatCls != null)
                    {
                        string whereStr = "TYPE=" + "'" + fildEntity.Type + "'";
                        IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo(whereStr, null, 1);
                        pFeatCur = pFeatCls.Update(pQueryFilter, false);
                        IFeature pFeature = pFeatCur.NextFeature();
                        int index;
                        if (pFeature != null && fildEntity.IsInsert == false)//如果存在，则替换
                        {
                            pFields = pFeatCls.Fields;
                            index = pFields.FindField("SHAPE");
                            pFeature.set_Value(index, fildEntity.Geometry);
                            pFeatCur.UpdateFeature(pFeature);
                            pFeatCur.Flush();
                            Marshal.ReleaseComObject(pFeatCur);
                            return pFeatCls;
                        }
                        //else//否则新增
                        //{

                        //if (pTP!=null)
                        //pTP.Simplify();

                        pFeatCur = pFeatCls.Insert(true);
                        foreach (var line in fildEntity.ListLine)
                        {
                            ITopologicalOperator pTP = line as ITopologicalOperator;
                            pTP.Simplify();
                            if (line == null || line.IsEmpty)
                                continue;
                            pFeatBuf = pFeatCls.CreateFeatureBuffer();
                            pFields = pFeatCls.Fields;
                            index = pFields.FindField("SHAPE");
                            pFeatBuf.set_Value(index, fildEntity.Geometry);

                            index = pFields.FindField("NAME");
                            pFeatBuf.set_Value(index, fileName);

                            index = pFields.FindField("COUNTRY");
                            pFeatBuf.set_Value(index, fildEntity.Country);

                            index = pFields.FindField("TYPE");
                            pFeatBuf.set_Value(index, fildEntity.Type);

                            if (fildEntity.OtherFields != null)
                            {
                                foreach (var item in fildEntity.OtherFields)
                                {
                                    if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE" || item.Key.ToUpper() == "COUNTRY")
                                        continue;
                                    index = pFields.FindField(item.Key.ToUpper());
                                    pFeatBuf.set_Value(index, item.Value);
                                }
                            }

                            pFeatCur.InsertFeature(pFeatBuf);//提交
                            pFeatCur.Flush();
                        }

                        Marshal.ReleaseComObject(pFeatCur);
                        GC.Collect();
                        //}
                    }
                    #endregion
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSpace);
                return pFeatCls;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

                pFields = null;
                pFeatureWorkspace = null;
                workSpace = null;
                pWF = null;

                pFeatCur = null;
                pFeatBuf = null;
                pFields = null;

            }

        }
        /// <summary>
        /// 用于数据存入shp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="oceanEntity"></param>
        /// <param name="spatialReference"></param>
        /// <returns></returns>
        public static IFeatureClass CreateFeaturePointsOnShpWorkspace(string filePath, string fileName, AOceanEntity oceanEntity, string groupName)
        {
            var fildEntity = oceanEntity as FeatureFieldExtendEntity;
            if (fildEntity == null)
                return null;

            IWorkspaceFactory2 pWF = new ShapefileWorkspaceFactoryClass();
            IWorkspace workSpace;
            IFeatureWorkspace pFeatureWorkspace;
            //关闭资源锁定
            IWorkspaceFactoryLockControl ipWsFactoryLock;
            ipWsFactoryLock = (IWorkspaceFactoryLockControl)pWF;
            if (ipWsFactoryLock.SchemaLockingEnabled)
            {
                ipWsFactoryLock.DisableSchemaLocking();
            }


            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur = null;
            IFeatureBuffer pFeatBuf = null;
            IFields pFields;
            try
            {

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var filePthFull = filePath;
                if (!string.IsNullOrEmpty(groupName))
                    filePthFull = filePath + "\\" + groupName;
                if (!Directory.Exists(filePthFull))
                    Directory.CreateDirectory(filePthFull);

                if (Directory.Exists(filePthFull + "\\" + fileName))
                    filePthFull = filePthFull + "\\" + fileName;
                if (!File.Exists(filePthFull + "\\" + fileName + ".shp"))
                {
                    #region 第一次时 新增
                    IWorkspaceName pWSName = pWF.Create(filePthFull, fileName, null, 0);
                    IName pName = (IName)pWSName;
                    workSpace = (IWorkspace)pName.Open();
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFields = new FieldsClass();

                    IField field = new FieldClass();
                    IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                    IFieldEdit fieldEdit = field as IFieldEdit;


                    fieldEdit.Name_2 = "OBJECTID";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                    fieldEdit.IsNullable_2 = false;
                    fieldEdit.Required_2 = false;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    IGeometryDef geoDef = new GeometryDefClass();
                    IGeometryDefEdit geoDefEdit = (IGeometryDefEdit)geoDef;
                    geoDefEdit.AvgNumPoints_2 = 5;
                    geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
                    geoDefEdit.GridCount_2 = 1;
                    geoDefEdit.HasM_2 = false;
                    geoDefEdit.HasZ_2 = false;
                    geoDefEdit.SpatialReference_2 = fildEntity.SpatialReference;

                    fieldEdit.Name_2 = "SHAPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                    fieldEdit.GeometryDef_2 = geoDef;
                    fieldEdit.IsNullable_2 = true;
                    fieldEdit.Required_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "NAME";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "COUNTRY";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "TYPE";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    field = new FieldClass();
                    fieldEdit = field as IFieldEdit;
                    fieldEdit.Name_2 = "INDEX";
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                    fieldEdit.IsNullable_2 = true;
                    fieldsEdit.AddField(field);

                    if (fildEntity.OtherFields != null)
                    {
                        foreach (var item in fildEntity.OtherFields)
                        {
                            if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE" || item.Key.ToUpper() == "COUNTRY")
                                continue;
                            field = new FieldClass();
                            fieldEdit = field as IFieldEdit;
                            fieldEdit.Name_2 = item.Key.ToUpper();
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit.IsNullable_2 = true;
                            fieldsEdit.AddField(field);
                        }
                    }

                    //创建要素类                
                    pFeatCls = pFeatureWorkspace.CreateFeatureClass(
                        fileName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                    //IFeature pFeature = pFeatCls.CreateFeature();                    
                    //pFeature.Shape = fildEntity.Geometry;
                    //pFeature.Store();
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    var t = pFeatCls.FeatureCount(pQueryFilter);
                    if (pFeatCls != null)
                    {
                        var col = fildEntity.Geometry as IPointCollection;
                        if (col != null)
                        {
                            for (int i = 0; i < col.PointCount; i++)
                            {
                                pFeatCur = pFeatCls.Insert(true);
                                pFeatBuf = pFeatCls.CreateFeatureBuffer();

                                int index;
                                pFields = pFeatCls.Fields;
                                index = pFields.FindField("SHAPE");
                                pFeatBuf.set_Value(index, col.get_Point(i));

                                index = pFields.FindField("NAME");
                                pFeatBuf.set_Value(index, fileName);

                                index = pFields.FindField("COUNTRY");
                                pFeatBuf.set_Value(index, fildEntity.Country);

                                index = pFields.FindField("TYPE");
                                pFeatBuf.set_Value(index, fildEntity.Type);

                                index = pFields.FindField("INDEX");
                                pFeatBuf.set_Value(index, i);
                                if (fildEntity.OtherFields != null)
                                {
                                    foreach (var item in fildEntity.OtherFields)
                                    {
                                        if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE" || item.Key.ToUpper() == "INDEX")
                                            continue;
                                        index = pFields.FindField(item.Key.ToUpper());
                                        pFeatBuf.set_Value(index, item.Value);
                                    }
                                }

                                pFeatCur.InsertFeature(pFeatBuf);
                            }

                        }
                        if (pFeatCur != null)
                        {
                            pFeatCur.Flush();
                            Marshal.ReleaseComObject(pFeatCur);
                        }
                        GC.Collect();
                        t = pFeatCls.FeatureCount(pQueryFilter);
                    }

                    #endregion

                }
                else
                {
                    #region 修改
                    workSpace = pWF.OpenFromFile(filePthFull, 0);
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFeatCls = pFeatureWorkspace.OpenFeatureClass(fileName);
                    if (pFeatCls != null)
                    {
                        var pQueryFilter = new QueryFilterClass();
                        var pFC = pFeatCls.Search(pQueryFilter, false);
                        var pFeat = pFC.NextFeature();

                        if (pFeat != null)
                        {

                            while (pFeat != null)
                            {
                                pFeat.Delete();
                                pFeat = pFC.NextFeature();
                            }


                            if (pFC != null)
                                Marshal.ReleaseComObject(pFC);
                            if (pFeat != null)
                                Marshal.ReleaseComObject(pFeat);
                        }
                        Marshal.ReleaseComObject(pQueryFilter);

                        var col = fildEntity.Geometry as IPointCollection;
                        if (col != null)
                        {
                            for (int i = 0; i < col.PointCount; i++)
                            {
                                pFeatCur = pFeatCls.Insert(true);
                                pFeatBuf = pFeatCls.CreateFeatureBuffer();

                                int index;


                                pFields = pFeatCls.Fields;
                                index = pFields.FindField("SHAPE");
                                pFeatBuf.set_Value(index, col.get_Point(i));

                                index = pFields.FindField("NAME");
                                pFeatBuf.set_Value(index, fileName);

                                index = pFields.FindField("COUNTRY");
                                pFeatBuf.set_Value(index, fildEntity.Country);

                                index = pFields.FindField("TYPE");
                                pFeatBuf.set_Value(index, fildEntity.Type);

                                index = pFields.FindField("INDEX");
                                pFeatBuf.set_Value(index, i);

                                if (fildEntity.OtherFields != null)
                                {
                                    foreach (var item in fildEntity.OtherFields)
                                    {
                                        if (item.Key.ToUpper() == "OBJECTID" || item.Key.ToUpper() == "SHAPE" || item.Key.ToUpper() == "NAME" || item.Key.ToUpper() == "TYPE" || item.Key.ToUpper() == "COUNTRY" || item.Key.ToUpper() == "INDEX")
                                            continue;
                                        index = pFields.FindField(item.Key.ToUpper());
                                        pFeatBuf.set_Value(index, item.Value);
                                    }
                                }
                                pFeatCur.InsertFeature(pFeatBuf);
                            }
                            if (pFeatCur != null)
                            {
                                pFeatCur.Flush();
                                Marshal.ReleaseComObject(pFeatCur);
                                Marshal.ReleaseComObject(pFeatCur);
                            }
                        }
                        GC.Collect();
                        //}
                    }
                    #endregion
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSpace);
                return pFeatCls;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

                pFields = null;
                pFeatureWorkspace = null;
                workSpace = null;
                pWF = null;

                pFeatCur = null;
                pFeatBuf = null;
                pFields = null;

            }

        }

        public static IFeatureClass InsertFeatureOnShpWorkspace(List<IFeature> list, string filePath, string fileName)
        {
            if (list == null || list.Count == 0 || string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileName))
                return null;
            IWorkspaceFactory2 pWF = new ShapefileWorkspaceFactoryClass();
            IWorkspace workSpace;
            IFeatureWorkspace pFeatureWorkspace;
            //关闭资源锁定
            IWorkspaceFactoryLockControl ipWsFactoryLock;
            ipWsFactoryLock = (IWorkspaceFactoryLockControl)pWF;
            if (ipWsFactoryLock.SchemaLockingEnabled)
            {
                ipWsFactoryLock.DisableSchemaLocking();
            }


            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur;
            IFeatureBuffer pFeatBuf;
            IFields pFields;
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var filePthFull = filePath;
                if (Directory.Exists(filePath + "\\" + fileName))
                    filePthFull = filePath + "\\" + fileName;
                if (!File.Exists(filePthFull + "\\" + fileName + ".shp"))
                {
                    #region 第一次时 新增
                    IWorkspaceName pWSName = pWF.Create(filePthFull, fileName, null, 0);
                    IName pName = (IName)pWSName;
                    workSpace = (IWorkspace)pName.Open();
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    //获取属性
                    pFields = new FieldsClass();
                    IField field = new FieldClass();
                    IFieldsEdit fieldsEdit = pFields as IFieldsEdit;
                    var fds = list.FirstOrDefault().Fields;
                    pFields = fds;
                    //for (int index = 0; index < fds.FieldCount; index++)
                    //{
                    //    fieldsEdit.set_Field(index, fds.get_Field(index));
                    //}
                    //创建要素类                
                    pFeatCls = pFeatureWorkspace.CreateFeatureClass(
                        fileName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                    foreach (var feat in list)
                    {
                        //IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo("", null, 1);
                        //var t = pFeatCls.FeatureCount(pQueryFilter);
                        ITopologicalOperator pTP = feat.Shape as ITopologicalOperator;
                        pTP.Simplify();
                        if (pFeatCls != null)
                        {

                            pFeatCur = pFeatCls.Insert(true);
                            pFeatBuf = pFeatCls.CreateFeatureBuffer();
                            for (int j = 0; j < feat.Fields.FieldCount; j++)
                            {
                                if (feat.Fields.get_Field(j).Name.ToUpper() == "ID" || feat.Fields.get_Field(j).Name.ToUpper() == "FID" || feat.Fields.get_Field(j).Name.ToUpper() == "OBJECTID" || feat.Fields.get_Field(j).Name.ToUpper() == "SHAPE")
                                    continue;
                                var obj = feat.get_Value(j).ToString();
                                if (!string.IsNullOrEmpty(obj))
                                    pFeatBuf.set_Value(j, obj);
                            }
                            pFeatBuf.Shape = feat.Shape;
                            pFeatCur.InsertFeature(pFeatBuf);
                            pFeatCur.Flush();
                            Marshal.ReleaseComObject(pFeatCur);
                            //t = pFeatCls.FeatureCount(pQueryFilter);
                        }
                    }
                    GC.Collect();
                    #endregion

                }
                else
                {
                    #region 第二次
                    workSpace = pWF.OpenFromFile(filePthFull, 0);
                    pFeatureWorkspace = workSpace as IFeatureWorkspace;
                    pFeatCls = pFeatureWorkspace.OpenFeatureClass(fileName);
                    if (pFeatCls != null)
                    {
                        var pQueryFilter = new QueryFilterClass();
                        var pFC = pFeatCls.Search(pQueryFilter, false);
                        var pFeat = pFC.NextFeature();

                        if (pFeat != null)
                        {

                            while (pFeat != null)
                            {
                                pFeat.Delete();
                                pFeat = pFC.NextFeature();
                            }

                            if (pFC != null)
                                Marshal.ReleaseComObject(pFC);
                            if (pFeat != null)
                                Marshal.ReleaseComObject(pFeat);
                        }
                        Marshal.ReleaseComObject(pQueryFilter);

                        foreach (var feat in list)
                        {
                            //IQueryFilter pQueryFilter = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo("", null, 1);
                            //var t = pFeatCls.FeatureCount(pQueryFilter);
                            ITopologicalOperator pTP = feat.Shape as ITopologicalOperator;
                            pTP.Simplify();
                            if (pFeatCls != null)
                            {

                                pFeatCur = pFeatCls.Insert(true);
                                pFeatBuf = pFeatCls.CreateFeatureBuffer();
                                for (int j = 0; j < feat.Fields.FieldCount; j++)
                                {
                                    if (feat.Fields.get_Field(j).Name.ToUpper() == "ID" || feat.Fields.get_Field(j).Name.ToUpper() == "FID" || feat.Fields.get_Field(j).Name.ToUpper() == "OBJECTID" || feat.Fields.get_Field(j).Name.ToUpper() == "SHAPE")
                                        continue;
                                    var obj = feat.get_Value(j).ToString();
                                    if (!string.IsNullOrEmpty(obj))
                                        pFeatBuf.set_Value(j, obj);
                                }
                                pFeatBuf.Shape = feat.Shape;
                                pFeatCur.InsertFeature(pFeatBuf);
                                pFeatCur.Flush();
                                Marshal.ReleaseComObject(pFeatCur);
                                //t = pFeatCls.FeatureCount(pQueryFilter);
                            }
                        }
                        GC.Collect();
                    }
                    #endregion
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSpace);
                return pFeatCls;
            }
            catch (Exception)
            {

                throw;
            }


            return null;
        }
        #endregion

        #region 删   当前未启用
        /// <summary>
        /// 删除要素
        /// </summary>
        /// <param name="pFeatureWorkspace">工作空间</param>
        /// <param name="layerName">图层名</param>
        /// <param name="pGeo">条件图元</param>
        /// <param name="whereStr">筛选条件</param>
        public static void DeleteFeatureFromWorkspace(IFeatureWorkspace pFeatureWorkspace, string layerName, IGeometry pGeo, string whereStr, int flag)
        {
            IFeatureCursor pFC;
            IFeature pFeat;
            IFeatureClass pFeatCls;
            IQueryFilter pQF;

            try
            {
                pFeatCls = pFeatureWorkspace.OpenFeatureClass(layerName);

                if (pFeatCls != null)
                {
                    if (pFeatCls.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        pQF = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo(whereStr, pGeo, 0);
                    }
                    else if (pFeatCls.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        pQF = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo(whereStr, pGeo, flag);
                    }
                    else if (pFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        pQF = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo(whereStr, pGeo, 2);
                    }
                    else
                    {
                        pQF = new QueryFilterClass();
                        pQF.WhereClause = "";
                    }

                    pFC = pFeatCls.Search(pQF, false);
                    pFeat = pFC.NextFeature();

                    if (pFeat != null)
                    {

                        while (pFeat != null)
                        {
                            pFeat.Delete();
                            pFeat = pFC.NextFeature();
                        }
                    }
                    else
                    {
                        return;
                    }

                    pFC.Flush();
                    Marshal.ReleaseComObject(pFC);
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                pFC = null;
                pFeat = null;
                pFeatureWorkspace = null;
                pFeatCls = null;
                pQF = null;
            }
        }
        /// <summary>
        /// 删除图元
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="whereStr"></param>
        /// <param name="pFeatureWorkspace"></param>
        /// <returns></returns>
        public static bool DeleteFeature(IFeatureWorkspace featureWorkspace, String layerName, String whereStr)
        {
            bool success = false;
            try
            {
                ITable pTable = featureWorkspace.OpenTable(layerName);
                IQueryFilter findQueryFilter = new QueryFilterClass();
                findQueryFilter.WhereClause = whereStr;
                pTable.DeleteSearchedRows(findQueryFilter);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception("删除地图图元出现错误！:" + ex.Message);
            }
            return success;
        }
        #endregion

        #region 查  已使用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureWorkspace"></param>
        /// <param name="strLyrName"></param>
        /// <returns></returns>
        public static IFeatureClass GetFeatuerClassFromWorkspace(IFeatureWorkspace featureWorkspace, string strLyrName)
        {
            try
            {
                IFeatureClass pFtCls;
                if (strLyrName != null && strLyrName.Trim() != "")
                {
                    pFtCls = featureWorkspace.OpenFeatureClass(strLyrName);
                    return pFtCls;
                }
                else
                {
                    pFtCls = null;
                    return pFtCls;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 获取IFeature
        /// </summary>
        /// <param name="featureWorkspace"></param>
        /// <param name="strLyrName"></param>
        /// <param name="strWhere"></param>
        /// <param name="geo"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<IFeature> GetFeatsFromFeatWorkspace(IFeatureWorkspace featureWorkspace, string strLyrName, string strWhere, IGeometry geo, int flag)
        {
            List<IFeature> listFeat = new List<IFeature>();

            IFeatureClass pFtCls;
            IQueryFilter pQF;
            IFeatureCursor pFQ;
            IFeature pFeat;
            try
            {

                //if (WorkSpaceAndFeatureHelper.IsExistFeat(featureWorkspace, strLyrName))
                //{
                pFtCls = featureWorkspace.OpenFeatureClass(strLyrName);
                pQF = GetQueryFilterByWhereAndGeo(strWhere, geo, flag);
                pFQ = pFtCls.Search(pQF, false);

                pFeat = pFQ.NextFeature();
                while (pFeat != null)
                {
                    listFeat.Add(pFeat);
                    pFeat = pFQ.NextFeature();
                }
                pFQ.Flush();

                Marshal.ReleaseComObject(pFQ);
                GC.Collect();
                //}
                return listFeat;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            finally
            {
                pFtCls = null;
                pQF = null;
                pFQ = null;
                pFeat = null;
            }
        }
        public static List<IFeature> GetFeatsFromFeatWorkspaceWithoutCheck(IFeatureWorkspace featureWorkspace, string strLyrName, string strWhere)
        {
            List<IFeature> listFeat = new List<IFeature>();

            IFeatureClass pFtCls;
            IQueryFilter pQF;
            IFeatureCursor pFQ;
            IFeature pFeat;
            try
            {
                pFtCls = featureWorkspace.OpenFeatureClass(strLyrName);
                pQF = new QueryFilterClass();
                pQF.WhereClause = strWhere;
                pFQ = pFtCls.Search(pQF, false);

                pFeat = pFQ.NextFeature();
                while (pFeat != null)
                {
                    listFeat.Add(pFeat);
                    pFeat = pFQ.NextFeature();
                }
                pFQ.Flush();

                Marshal.ReleaseComObject(pFQ);
                GC.Collect();
                return listFeat;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new Exception(strLyrName+ex.Message,ex);
            }
            finally
            {
                pFtCls = null;
                pQF = null;
                pFQ = null;
                pFeat = null;
            }
        }
        public static List<IFeature> GetFeatsFromFeatWorkspaceByEnvelopeWithoutCheck(IFeatureWorkspace featureWorkspace, string strLyrName, IGeometry geo)
        {
            List<IFeature> listFeat = new List<IFeature>();

            IFeatureClass pFtCls;
            IQueryFilter pQF;
            IFeatureCursor pFQ;
            IFeature pFeat;
            try
            {
                pFtCls = featureWorkspace.OpenFeatureClass(strLyrName);
                pQF = GetQueryFilterByWhereAndGeo("", geo, 1);
                pFQ = pFtCls.Search(pQF, false);

                pFeat = pFQ.NextFeature();
                while (pFeat != null)
                {
                    listFeat.Add(pFeat);
                    pFeat = pFQ.NextFeature();
                }
                pFQ.Flush();

                Marshal.ReleaseComObject(pFQ);
                GC.Collect();
                return listFeat;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                pFtCls = null;
                pQF = null;
                pFQ = null;
                pFeat = null;
            }
        }


        /// <summary>
        /// 获得要素类中符合条件的要素
        /// </summary>
        /// <param name="strLyrName"></param>
        public static void GetFeatsFromFeatWorkspace(IFeatureWorkspace featureWorkspace, string strLyrName, string strWhere, List<IFeature> listFeat)
        {
            IFeatureClass pFtCls;
            IQueryFilter pQF;
            IFeatureCursor pFQ;
            IFeature pFeat;
            try
            {

                if (WorkSpaceAndFeatureHelper.IsExistFeat(featureWorkspace, strLyrName))
                {
                    pFtCls = featureWorkspace.OpenFeatureClass(strLyrName);
                    pQF = new QueryFilterClass();
                    pQF.WhereClause = strWhere;
                    pFQ = pFtCls.Search(pQF, false);

                    pFeat = pFQ.NextFeature();
                    while (pFeat != null)
                    {
                        listFeat.Add(pFeat);
                        pFeat = pFQ.NextFeature();
                    }
                    pFQ.Flush();

                    Marshal.ReleaseComObject(pFQ);
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            finally
            {
                pFtCls = null;
                pQF = null;
                pFQ = null;
                pFeat = null;
            }
        }

        /// <summary>
        /// 从表中查询符合条件的记录
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="whereStr"></param>
        /// <param name="listFT"></param>
        public static void QueryObjectByWhere(IFeatureWorkspace featureWorkspace, string tableName, string whereStr, out List<IRow> listRow)
        {
            ITable pTable;
            ICursor pCursor;
            IQueryFilter pQF;
            IRow pRow;
            try
            {

                pTable = featureWorkspace.OpenTable(tableName);
                listRow = new List<IRow>();
                pQF = WorkSpaceAndFeatureHelper.GetQueryFilterByWhereAndGeo(whereStr, null, 0);
                pCursor = pTable.Search(pQF, false);
                pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    listRow.Add(pRow);
                    pRow = pCursor.NextRow();
                }
                pCursor.Flush();
                Marshal.ReleaseComObject(pCursor);
                GC.Collect();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

                pTable = null;
                pCursor = null;
                pQF = null;
                pRow = null;

            }
        }


        public static IPointCollection GetPointFeatureToLine(IFeatureWorkspace featureWorkspace, string strLyrName)
        {
            IFeatureClass pFtCls;
            IQueryFilter pQF;
            IFeatureCursor pFQ;
            IFeature pFeat;

            IPointCollection col = null;
            try
            {

                if (WorkSpaceAndFeatureHelper.IsExistFeat(featureWorkspace, strLyrName))
                {
                    col = new PolylineClass();
                    pFtCls = featureWorkspace.OpenFeatureClass(strLyrName);
                    pQF = new QueryFilterClass();
                    pFQ = pFtCls.Search(pQF, false);

                    pFeat = pFQ.NextFeature();
                    object mis = Type.Missing;
                    while (pFeat != null)
                    {
                        col.AddPoint(pFeat.ShapeCopy as IPoint, mis, mis);
                        pFeat = pFQ.NextFeature();
                    }
                    pFQ.Flush();
                    if (col.PointCount < 2)
                        col = null;
                    Marshal.ReleaseComObject(pFQ);

                    GC.Collect();
                }
                return col;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            finally
            {
                pFtCls = null;
                pQF = null;
                pFQ = null;
                pFeat = null;
            }
        }

        public static List<string> GetLyrNames(IWorkspace pWorkspace)
        {
            List<string> list = new List<string>();
            IEnumDatasetName FeatureEnumDatasetName;
            IDatasetName pDatasetName;
            try
            {
                IFeatureWorkspace m_pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                FeatureEnumDatasetName = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
                if (FeatureEnumDatasetName == null) return null;
                FeatureEnumDatasetName.Reset();
                pDatasetName = FeatureEnumDatasetName.Next();
                while (pDatasetName != null)
                {
                    string sss = pDatasetName.Name;
                    list.Add(sss);
                    pDatasetName = FeatureEnumDatasetName.Next();
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                FeatureEnumDatasetName = null;
                pDatasetName = null;
            }
        }

        #endregion

        #region 改
        #endregion

        public static void ConvertFeatureClass(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, string nameOfSourceFeatureClass, string nameOfTargetFeatureClass, IQueryFilter pQueryFilter)
        {
            //create source workspace name
            IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
            IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;


            //create source dataset name
            IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
            IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
            sourceDatasetName.WorkspaceName = sourceWorkspaceName;
            sourceDatasetName.Name = nameOfSourceFeatureClass;


            //create target workspace name
            IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
            IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;


            //create target dataset name
            IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
            IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
            targetDatasetName.WorkspaceName = targetWorkspaceName;
            targetDatasetName.Name = nameOfTargetFeatureClass;


            //Open input Featureclass to get field definitions.
            ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
            IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();


            //Validate the field names because you are converting between different workspace types.
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IFields targetFeatureClassFields;
            IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
            IEnumFieldError enumFieldError;


            // Most importantly set the input and validate workspaces!
            fieldChecker.InputWorkspace = sourceWorkspace;
            fieldChecker.ValidateWorkspace = targetWorkspace;
            fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError, out targetFeatureClassFields);


            // Loop through the output fields to find the geomerty field
            IField geometryField;
            for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
            {
                if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    geometryField = targetFeatureClassFields.get_Field(i);
                    // Get the geometry field's geometry defenition
                    IGeometryDef geometryDef = geometryField.GeometryDef;


                    //Give the geometry definition a spatial index grid count and grid size
                    IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;


                    targetFCGeoDefEdit.GridCount_2 = 1;
                    targetFCGeoDefEdit.set_GridSize(0, 0); //Allow ArcGIS to determine a valid grid size for the data loaded
                    targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;


                    // we want to convert all of the features
                    //IQueryFilter queryFilter = new QueryFilterClass();
                    //queryFilter.WhereClause = "";


                    // Load the feature class
                    IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                    //IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, queryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);
                    IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, pQueryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);

                    break;
                }
            }
        }

        public static void ConvertFeatureClass(string ImportFilePath, string ImportFileShortName, IFeatureDataset apFD, IWorkspace pWorkspace, string gdbLyrName)
        {
            try
            {


                IWorkspaceName pInWorkspaceName;
                IFeatureClassName pInFeatureClassName;
                IDatasetName pInDatasetName;
                IFeatureClass pInFeatureClass;
                IFields pInFields;

                IFeatureDatasetName pOutFeatureDSName;
                IFeatureClassName pOutFeatureClassName;
                IDatasetName pOutDatasetName;
                long iCounter;
                IFields pOutFields;
                IFieldChecker pFieldChecker;
                IField pGeoField;
                IGeometryDef pOutGeometryDef;
                IGeometryDefEdit pOutGeometryDefEdit;

                IName pName;
                IFeatureDataConverter pShpToClsConverter;
                IEnumFieldError pEnumFieldError = null;

                //得到一个输入SHP文件的工作空间，
                pInWorkspaceName = new WorkspaceNameClass();
                pInWorkspaceName.PathName = ImportFilePath;
                pInWorkspaceName.WorkspaceFactoryProgID = "esriCore.ShapefileWorkspaceFactory.1";
                //创建一个新的要素类名称，目的是为了以来PNAME接口的OPEN方法打开SHP文件
                pInFeatureClassName = new FeatureClassNameClass();
                pInDatasetName = (IDatasetName)pInFeatureClassName;
                pInDatasetName.Name = ImportFileShortName;
                pInDatasetName.WorkspaceName = pInWorkspaceName;
                //打开一个SHP文件，将要读取它的字段集合
                pName = (IName)pInFeatureClassName;
                pInFeatureClass = (IFeatureClass)pName.Open();
                //通过FIELDCHECKER检查字段的合法性，为输入要素类获得字段集合
                pInFields = pInFeatureClass.Fields;
                pFieldChecker = new FieldChecker();
                pFieldChecker.Validate(pInFields, out pEnumFieldError, out pOutFields);
                //通过循环查找几何字段
                pGeoField = null;
                for (iCounter = 0; iCounter < pOutFields.FieldCount; iCounter++)
                {
                    if (pOutFields.get_Field((int)iCounter).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pGeoField = pOutFields.get_Field((int)iCounter);
                        break;
                    }
                }
                //得到几何字段的几何定义
                pOutGeometryDef = pGeoField.GeometryDef;
                //设置几何字段的空间参考和网格
                pOutGeometryDefEdit = (IGeometryDefEdit)pOutGeometryDef;
                pOutGeometryDefEdit.GridCount_2 = 1;
                pOutGeometryDefEdit.set_GridSize(0, 1500000);

                //创建一个新的要素类名称作为可用的参数
                pOutFeatureClassName = new FeatureClassNameClass();
                pOutDatasetName = (IDatasetName)pOutFeatureClassName;
                pOutDatasetName.Name = gdbLyrName;

                //创建一个新的数据集名称作为可用的参数
                pOutFeatureDSName = (IFeatureDatasetName)new FeatureDatasetName();
                //因为ConvertFeatureClass需要传入一个IFeatureDatasetName的参数，通过它确定导入生成的要素类的工作空间和要素集合
                //情况一
                //如果本函数的参数（IFeatureDataset）是一个确切的值，那么将它转换成IFeatureDatasetName接口就可以了。因为ConvertFeatureClass根据该接口就
                //可以确定工作空间和要素集合，IFeatureClassName就可以不考虑上述问题
                //情况二
                //如果本函数的参数（IFeatureDataset）是一个NULL值，表示要创建独立要素类，
                //那么ConvertFeatureClass函数无法根据IFeatureDatasetName参数确定工作空间和要素集合
                //这个时候需要IFeatureClassName参数确定工作空间和要素集合

                //如果参数的值是NULL，说明要创建独立要素类
                if (apFD == null)
                {
                    //创建一个不存在的要素集合pFDN，通过它将IFeatureClassName和工作空间连接起来，而ConvertFeatureClass函数并不使用该变量作为参数，
                    IFeatureDatasetName pFDN = new FeatureDatasetNameClass();
                    IDatasetName pDN = (IDatasetName)pFDN;
                    IDataset pDS = (IDataset)pWorkspace;
                    pDN.WorkspaceName = (IWorkspaceName)pDS.FullName;
                    pDN.Name = pDS.Name;
                    pOutFeatureClassName.FeatureDatasetName = (IDatasetName)pFDN;
                    //将pOutFeatureDSName设置为Null，将它做为参数给ConvertFeatureClass函数，因为IFeatureClassName本身已经和工作空间关联了，生成的
                    //要素类在工作空间的根目录下，即独立要素类
                    pOutFeatureDSName = null;

                }
                else//创建的要素类是在给定的参数（要素集合）下
                {
                    pOutFeatureDSName = (IFeatureDatasetName)apFD.FullName;
                }

                //开始导入
                pShpToClsConverter = new FeatureDataConverterClass();
                //pShpToClsConverter.ConvertFeatureClass(pInFeatureClassName, null, pOutFeatureDSName, pOutFeatureClassName, pOutGeometryDef, pOutFields, "", 1000, 0);
                pShpToClsConverter.ConvertFeatureClass(pInFeatureClassName, null, pOutFeatureDSName, pOutFeatureClassName, pOutGeometryDef, pOutFields, "", 1000, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool ConvertFeatureDataset(IWorkspace sourceWorkspace, IWorkspace targetWorkspace,
     string nameOfSourceFeatureDataset, string nameOfTargetFeatureDataset)
        {
            try
            {
                //create source workspace name  
                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
                //create source dataset name   
                IFeatureDatasetName sourceFeatureDatasetName = new FeatureDatasetNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureDatasetName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = nameOfSourceFeatureDataset;
                //create target workspace name   
                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
                //create target dataset name  
                IFeatureDatasetName targetFeatureDatasetName = new FeatureDatasetNameClass();
                IDatasetName targetDatasetName = (IDatasetName)targetFeatureDatasetName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = nameOfTargetFeatureDataset;
                //Convert feature dataset     
                IFeatureDataConverter featureDataConverter = new FeatureDataConverterClass();
                featureDataConverter.ConvertFeatureDataset(sourceFeatureDatasetName, targetFeatureDatasetName, null, "", 1000, 0);
                return true;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public static bool ConvertFeatureClass2FeatureDataset(IWorkspace sourceWorkspace,
      IWorkspace targetWorkspace, string nameOfSourceFeatureClass,
      string nameOfTargetFeatureClass, IFeatureDatasetName pName)
        {
            try
            {
                //create source workspace name 
                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
                //create source dataset name   
                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = nameOfSourceFeatureClass;

                //create target workspace name   
                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
                //create target dataset name    
                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = nameOfTargetFeatureClass;
                //Open input Featureclass to get field definitions.  
                ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
                IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();
                //Validate the field names because you are converting between different workspace types.   
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IFields targetFeatureClassFields;
                IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
                IEnumFieldError enumFieldError;
                // Most importantly set the input and validate workspaces! 
                fieldChecker.InputWorkspace = sourceWorkspace;
                fieldChecker.ValidateWorkspace = targetWorkspace;
                fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError,
                    out targetFeatureClassFields);
                // Loop through the output fields to find the geomerty field   
                IField geometryField;
                for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
                {
                    if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        geometryField = targetFeatureClassFields.get_Field(i);
                        // Get the geometry field's geometry defenition          
                        IGeometryDef geometryDef = geometryField.GeometryDef;
                        //Give the geometry definition a spatial index grid count and grid size     
                        IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                        targetFCGeoDefEdit.GridCount_2 = 1;
                        targetFCGeoDefEdit.set_GridSize(0, 0);
                        //Allow ArcGIS to determine a valid grid size for the data loaded     
                        targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
                        // we want to convert all of the features    
                        IQueryFilter queryFilter = new QueryFilterClass();
                        queryFilter.WhereClause = "";
                        // Load the feature class            
                        IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                        IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName,
                            queryFilter, pName, targetFeatureClassName,
                            geometryDef, targetFeatureClassFields, "", 1000, 0);
                        break;
                    }
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }


    }
}
