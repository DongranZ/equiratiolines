/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2018-**-**
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Display;

namespace ArcengineHelper.DisplayHelper
{
    public static class IColorHelper
    {
        /// <summary>
        /// 创建rgb颜色
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static IRgbColor CreateRGBColor(int red, int green, int blue)
        {
            //Create rgb color and grab hold of the IRGBColor interface
            IRgbColor rGB = new RgbColorClass();
            //Set rgb color properties
            rGB.Red = red;
            rGB.Green = green;
            rGB.Blue = blue;
            rGB.UseWindowsDithering = true;
            return rGB;
        }

        /// <summary>
        /// 创建IColor
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static IColor GetRgbColor(int R, int G, int B)
        {
            try
            {
                IRgbColor pRgbColor = new RgbColorClass();
                if (R < 0 || B < 0 || G < 0 || R > 255 || B > 255 || G > 255)
                {
                    pRgbColor.NullColor = true;
                }
                else
                {
                    pRgbColor.Red = R;
                    pRgbColor.Green = G;

                    pRgbColor.Blue = B;
                }
                IColor pColor = pRgbColor as IColor;
                return pColor;
            }
            catch (Exception ex)
            {
                throw ex;
                //DevComponents.DotNetBar.MessageBoxEx.Show(Err.Message, "获得RgbColor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
