using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
/********************************************************************************
 ****创建目的：  地图文件方法帮助类
 ****创 建 人：  李洋
 ****创建时间：  2020-12-03
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcengineHelper.MapHelper
{
    public class MapDocumenHelper
    {
        //设置文档对象的成员变量
        protected static IMapDocument mapDocument = new MapDocumentClass();

        //新建地图文档
        public static void NewMapDoc(AxMapControl axMapControl1)
        {
            DialogResult dialogResult = MessageBox.Show("保存文件？", "关闭文件", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                SaveDocument(axMapControl1);
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "新建地图文档";
                saveFileDialog1.Filter = "地图文档(*.mxd)|*.mxd";
                saveFileDialog1.ShowDialog();
                string filePath = saveFileDialog1.FileName;//地图文档路径
                if (string.IsNullOrEmpty(filePath)) return;
                mapDocument.New(filePath);//新建
                mapDocument.Open(filePath, "");//打开
                axMapControl1.Map = mapDocument.get_Map(0);
            }
            if (dialogResult == DialogResult.No)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "新建地图文档";
                saveFileDialog1.Filter = "地图文档(*.mxd)|*.mxd";
                saveFileDialog1.ShowDialog();
                string filePath = saveFileDialog1.FileName;//地图文档路径
                if (string.IsNullOrEmpty(filePath)) return;
                mapDocument.New(filePath);//新建
                mapDocument.Open(filePath, "");//打开
                axMapControl1.Map = mapDocument.get_Map(0);
            }
            if (dialogResult == DialogResult.Cancel)
                return;

        }
        //打开地图文档
        public static void OpenMapDocument(AxMapControl axMapControl1)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "打开地图文档";
            openFileDialog.Filter = "地图文档(*.mxd)|*.mxd";
            openFileDialog.ShowDialog();
            string sFilePath = openFileDialog.FileName;
            if (axMapControl1.CheckMxFile(sFilePath))
            {
                axMapControl1.LoadMxFile(sFilePath, 0, Type.Missing);
            }
            else
            {
                MessageBox.Show(sFilePath + "不是有效的地图文档路径");
                return;
            }
            axMapControl1.Refresh();

        }
        //保存地图文档
        public static void SaveDocument(AxMapControl axMapControl1)
        {
            mapDocument.Open(axMapControl1.DocumentFilename, "");
            //确保地图文档不是只读的
            if (mapDocument.get_IsReadOnly(mapDocument.DocumentFilename) == true)
            {
                MessageBox.Show("该地图只读！"); return;
            }
            //以相对路径保存
            mapDocument.Save(mapDocument.UsesRelativePaths, true);
            MessageBox.Show("成功保存地图！");
        }
        //另存地图文档
        public static void SaveAsDocument(AxMapControl axMapControl1)
        {
            mapDocument.Open(axMapControl1.DocumentFilename, "");
            SaveFileDialog SaveFileDialog2 = new SaveFileDialog();
            SaveFileDialog2.Title = "另存为地图文档";
            SaveFileDialog2.Filter = "地图文档(*.mxd)|*.mxd";//设置要保存的地图文档的类型
            SaveFileDialog2.ShowDialog();
            String sFilePath = SaveFileDialog2.FileName;//获取文件路径
            if (sFilePath == "") return;      //路径为空，则返回 
            //判断文件路径是否改变，如果没有改变保存当前修改，改变则另存
            if (sFilePath == mapDocument.DocumentFilename)
            {
                SaveDocument(axMapControl1);
            }
            else
            {
                mapDocument.SaveAs(sFilePath, true, true);
            }
        }
        //导出地图
        //public void ExportMapToImage(AxPageLayoutControl axPageLayoutControl1)
        //{
        //    try
        //    {
        //        SaveFileDialog saveFileDialog = new SaveFileDialog();
        //        saveFileDialog.FileName = "";
        //        saveFileDialog.Filter = "JPG图片(*.JPG)|*.jpg|tif图片(*.tif)|*.tif|PDF文档(*.PDF)|*.pdf";
        //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            double iScreenDispalyResolution = axPageLayoutControl1.ActiveView.ScreenDisplay.
        //            DisplayTransformation.Resolution;// 获取屏幕分辨率的值
        //            IExporter exporter = new JpegExporterClass();
        //            exporter.ExportFileName = saveFileDialog.FileName;
        //            exporter.Resolution = (short)iScreenDispalyResolution; //分辨率
        //            tagRECT deviceRect = axPageLayoutControl1.ActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
        //            IEnvelope deviceEnvelope = new EnvelopeClass();
        //            deviceEnvelope.PutCoords(deviceRect.left, deviceRect.bottom, deviceRect.right, deviceRect.top);
        //            exporter.PixelBounds = deviceEnvelope; // 输出图片的范围
        //            ITrackCancel pCancle = new CancelTrackerClass();//可用ESC键取消操作
        //            axPageLayoutControl1.ActiveView.Output(exporter.StartExporting(), exporter.Resolution, ref deviceRect,
        //            axPageLayoutControl1.ActiveView.Extent, pCancle);
        //            Application.DoEvents();
        //            exporter.FinishExporting();
        //        }

        //    }
        //    catch (Exception Err)
        //    {
        //        MessageBox.Show(Err.Message, "输出图片", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
        
        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="axMapControl1"></param>
        /// <param name="axTOCControl1"></param>
        public static void ExitProgram(AxMapControl axMapControl1, AxTOCControl axTOCControl1)
        {
            DialogResult dialogResult = MessageBox.Show("保存文件？", "关闭文件", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                SaveDocument(axMapControl1);
                mapDocument.Close();
                axMapControl1.ClearLayers();
                axMapControl1.Refresh();
                axTOCControl1.Update();
                Application.Exit();
            }
            if (dialogResult == DialogResult.No)
            {
                //mapDocument.Close();
                //axMapControl1.ClearLayers();
                //axMapControl1.Refresh();
                //axTOCControl1.Update();
                Application.Exit();
            }
            if (dialogResult == DialogResult.Cancel)
                return;
        }
    }
}
