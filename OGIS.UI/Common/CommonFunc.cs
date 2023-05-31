using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OGIS.UI
{
    public static class CommonFunc
    {
        /// <summary>
        /// 检查是否有 “坐标操作页签”  暂未使用
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="tabName">tagpage名称</param>
        /// <param name="index">坐标操作 签所在index</param>
        /// <returns></returns>
        public static bool tabControlCheckHave(System.Windows.Forms.TabControl tab, String tabName, out int index)
        {
            for (int i = 0; i < tab.TabCount; i++)
            {
                if (tab.TabPages[i].Text == tabName)
                {
                    var a = tab.TabPages[i].Controls;
                    tab.SelectedIndex = i;
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        /// <summary>
        /// 向TabControl中添加Control Form;当TabControl有Form时，不需要添加
        /// 必要条件：TabControl.TabPage.Name必须等于TabControl.TabPage.Text,因为TabPages 及Control的key为Name,非Text
        /// </summary>
        /// <param name="form">要添加的Control组件，类型为Form</param>
        /// <param name="tabControl">容器</param>
        /// <param name="tabPageName">页签名称</param>
        /// <param name="index">页签的指针</param>
        /// <returns>是否添加成功</returns>
        public static bool LoadFormToTabControl(Form form, TabControl tabControl, string tabPageName, ref int index)
        {
            try
            {
                if (tabControl.TabPages.ContainsKey(tabPageName))
                {
                    var page = tabControl.TabPages[tabPageName];
                    index = tabControl.TabPages.IndexOfKey(tabPageName);

                    if (page.Controls.ContainsKey(form.Name))
                        return true;
                }
                else
                {
                    if (index == -1) index = 0;
                    tabControl.TabPages.Insert(index, tabPageName, tabPageName);
                }


                //tabControl.TabPages[tabPageName].Name = tabPageName;
                tabControl.SelectTab(index);

                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.TopLevel = false;
                form.Show();
                form.Parent = tabControl.SelectedTab;
                return true;
            }
            catch (Exception ex)
            {
                index = -1;
                //WriteLogs.Instance.WriteErrorLogs(string.Format("向TabControl中添加Control，方法：{0}", "LoadFormToTabControl"), ex);
                return false;
            }

        }
        /// <summary>
        /// 向TabControl中添加Control Form;当TabControl有Form时，不需要添加
        /// 必要条件：TabControl.TabPage.Name必须等于TabControl.TabPage.Text,因为TabPages 及Control的key为Name,非Text
        /// </summary>
        /// <param name="form">要添加的Control组件，类型为Form</param>
        /// <param name="tabControl">容器</param>
        /// <param name="tabPageName">页签名称</param>
        /// <param name="index">页签的指针</param>
        /// <returns>是否添加成功</returns>
        public static bool LoadFormToTabControl(Form form, TabControl tabControl, string tabPageName, int index)
        {
            try
            {
                if (tabControl.TabPages.ContainsKey(tabPageName))
                {
                    var page = tabControl.TabPages[tabPageName];
                    index = tabControl.TabPages.IndexOfKey(tabPageName);

                    if (page.Controls.ContainsKey(form.Name))
                        return true;
                }
                else
                {
                    index = 0;
                    tabControl.TabPages.Clear();
                    tabControl.TabPages.Insert(0, tabPageName, tabPageName);
                }


                //tabControl.TabPages[tabPageName].Name = tabPageName;
                tabControl.SelectTab(index);

                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.TopLevel = false;
                form.Show();
                form.Parent = tabControl.SelectedTab;
                tabControl.Update();
                return true;
            }
            catch (Exception ex)
            {
                index = -1;
                //Richway.OceanGis.Utils.Logs.Instance.WriteErrorLogs(string.Format("向TabControl中添加Control，方法：{0}", "LoadFormToTabControl"), ex);
                return false;
            }

        }
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ImportFilePath"路径></param>
        /// <param name="ImportFileShortName">名称</param>
        /// <returns></returns>
        public static bool GetFile(out string ImportFilePath, out string ImportFileShortName)
        {
            ImportFilePath = "";
            ImportFileShortName = "";

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "导入坐标";
            openDialog.Filter = "MapDocument(*.shp)|*.shp";
            openDialog.ShowDialog();
            string path = openDialog.FileName;
            if (string.IsNullOrEmpty(path))
            {
                //WriteLogs.Instance.WriteMessage(LogType.Info, path + "不是有效的文档！");
                return false;
            }

            ImportFileShortName = System.IO.Path.GetFileNameWithoutExtension(path);
            ImportFilePath = System.IO.Path.GetDirectoryName(path);
            openDialog.Dispose();
            openDialog = null;
            path = string.Empty;
            return true;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="ImportFilePath"></param>
        /// <param name="ImportFileShortName"></param>
        /// <returns></returns>
        public static bool GetSaveFile(out string ImportFilePath, out string ImportFileShortName)
        {
            ImportFilePath = "";
            ImportFileShortName = "";

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "导入坐标";
            saveDialog.Filter = "MapDocument(*.shp)|*.shp";
            saveDialog.ShowDialog();
            string path = saveDialog.FileName;
            if (string.IsNullOrEmpty(path))
            {
                //WriteLogs.Instance.WriteMessage(LogType.Info, path + "不是有效的文档！");
                return false;
            }

            ImportFileShortName = System.IO.Path.GetFileNameWithoutExtension(path);
            ImportFilePath = System.IO.Path.GetDirectoryName(path);
            saveDialog.Dispose();
            saveDialog = null;
            path = string.Empty;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="strPath"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static bool GetLayerPath(ref string fileType, out string strPath, out string strName)
        {
            strPath = "";
            strName = "";
            if (fileType == null || fileType.Split('|') == null)
                return false;
            string path = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = string.Format("打开*{0}文件", fileType);
            var fileTypes = "";
            foreach (var item in fileType.Split('|'))
            {
                fileTypes += string.Format("|（*{0}）|*{0}", item);
            }
            openFileDialog.Filter = fileTypes.Substring(1);
            openFileDialog.ShowDialog();
            path = openFileDialog.FileName;
            if (string.IsNullOrWhiteSpace(path))
                return false;
            int pIndex = path.LastIndexOf("\\");
            strPath = path.Substring(0, pIndex); //文件路径
            strName = path.Substring(pIndex + 1); //文件名
            fileType = strName.Substring(strName.LastIndexOf(".") + 1);
            openFileDialog.Dispose();
            return true;
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        /// <returns></returns>
        public static bool CopyFile(string sourceFile, string destFile)
        {
            if (!System.IO.File.Exists(sourceFile)) return false;
            string path = System.IO.Path.GetDirectoryName(destFile);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            try
            {
                System.IO.File.Copy(sourceFile, destFile, true);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 拷贝另存文件
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="destPath">目标路径</param>
        public static bool CopyEntireDir(string sourcePath, string destPath)
        {
            try
            {
                if (!System.IO.Directory.Exists(sourcePath)) return false;
                if (!System.IO.Directory.Exists(destPath))
                {
                    System.IO.Directory.CreateDirectory(destPath);
                }
                //Now Create all of the directories
                foreach (string dirPath in System.IO.Directory.GetDirectories(sourcePath, "*",
                   System.IO.SearchOption.AllDirectories))
                    System.IO.Directory.CreateDirectory(dirPath.Replace(sourcePath, destPath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in System.IO.Directory.GetFiles(sourcePath, "*.*",
                   System.IO.SearchOption.AllDirectories))
                    System.IO.File.Copy(newPath, newPath.Replace(sourcePath, destPath), true);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 连接路径
        /// </summary>
        /// <param name="firstStr"></param>
        /// <param name="secondStr"></param>
        /// <returns></returns>
        public static string StringCombine(string firstStr, string secondStr)
        {
            var combineStr = System.IO.Path.Combine(firstStr, secondStr);
            if (!combineStr.Equals(secondStr))
                return combineStr;
            return firstStr + secondStr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ctrlChilden"></param>
        public static void Bingding(System.Windows.Forms.Control ctrl, System.Windows.Forms.Control ctrlChilden)
        {
            ctrlChilden.Parent = ctrl;
            ctrlChilden.Dock = System.Windows.Forms.DockStyle.Fill;
        }
    }
}
