using OGIS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OGIS.UI
{
    public class ExcelService
    {
        public bool ExportToExcelNPOI(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return false;
            string pathFull, path, name;
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "请选择保存坐标点的位置:";
            saveFile.Filter = "Excel文件 (*.xls)|*.xls";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                pathFull = saveFile.FileName;
                path = System.IO.Path.GetDirectoryName(pathFull);
                name = System.IO.Path.GetFileNameWithoutExtension(path);
            }
            else
                return false;

            try
            {
                NPOIHelper.NPOIToExcel(dt, name, pathFull);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;

        }
    }
}
