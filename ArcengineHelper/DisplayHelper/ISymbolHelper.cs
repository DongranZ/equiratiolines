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
    public static class ISymbolHelper
    {
        //创建样式        
        /// <summary>
        /// 创建点符号
        /// </summary>
        /// <returns></returns>
        public static ISymbol CreatePointSymbol(int r, int g, int b, int width)
        {
            ISimpleMarkerSymbol pSimpleSymbol = new SimpleMarkerSymbolClass();
            pSimpleSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            IRgbColor pColor = IColorHelper.CreateRGBColor(r, g, b);

            pSimpleSymbol.Color = pColor;
            pSimpleSymbol.Size = width;
            pSimpleSymbol.Outline = true;
            pSimpleSymbol.OutlineColor = pColor;
            //pSimpleSymbol.XOffset = 6;
            //pSimpleSymbol.YOffset = -6;
            return pSimpleSymbol as ISymbol;
        }
        /// <summary>
        /// 创建点符号
        /// </summary>
        /// <returns></returns>
        public static ISymbol CreatePointSymbol(int r, int g, int b, int width, esriSimpleMarkerStyle Style)
        {
            ISimpleMarkerSymbol pSimpleSymbol = new SimpleMarkerSymbolClass();
            pSimpleSymbol.Style = Style;
            IRgbColor pColor = IColorHelper.CreateRGBColor(r, g, b);

            pSimpleSymbol.Color = pColor;
            pSimpleSymbol.Size = width;
            pSimpleSymbol.Outline = true;
            pSimpleSymbol.OutlineColor = pColor;
            return pSimpleSymbol as ISymbol;
        }
        /// <summary>
        /// 创建字体符号
        /// </summary>
        /// <param name="textSize"></param>
        /// <param name="pColor"></param>
        /// <returns></returns>
        public static ISymbol CreateTextSymbol(int r, int g, int b, int size)
        {
            ESRI.ArcGIS.Display.ITextSymbol pSymbol;
            try
            {
                IRgbColor pColor = IColorHelper.CreateRGBColor(r, g, b);

                pSymbol = new ESRI.ArcGIS.Display.TextSymbol();
                pSymbol.Size = size;
                pSymbol.Color = pColor;
                return pSymbol as ESRI.ArcGIS.Display.ISymbol;
            }
            catch (Exception e)
            {
                throw new Exception("创建文本错误:" + e.Message);
            }
        }
        /// <summary>
        /// 创建字体符号
        /// </summary>
        /// <param name="textSize"></param>
        /// <param name="pColor"></param>
        /// <returns></returns>
        public static ISymbol CreateTextSymbol(int r, int g, int b, string fontName, int size, esriTextHorizontalAlignment alignment)
        {
            ESRI.ArcGIS.Display.ITextSymbol pSymbol;
            try
            {

                IRgbColor pColor = IColorHelper.CreateRGBColor(r, g, b);

                pSymbol = new ESRI.ArcGIS.Display.TextSymbol();
                pSymbol.Size = size;
                pSymbol.Color = pColor;
                return pSymbol as ESRI.ArcGIS.Display.ISymbol;
            }
            catch (Exception e)
            {
                throw new Exception("创建文本错误:" + e.Message);
            }
        }
        /// <summary>
        /// 创建线符号
        /// </summary>
        /// <returns></returns>
        public static ISymbol CreateLineSymbol(int r, int g, int b, int width)
        {
            ISimpleLineSymbol pSimpleSymbol = new SimpleLineSymbolClass();
            pSimpleSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            IRgbColor pColor = IColorHelper.CreateRGBColor(r, g, b);
            pSimpleSymbol.Color = pColor;
            pSimpleSymbol.Width = width;
            return pSimpleSymbol as ISymbol;
        }
        /// <summary>
        /// 创建线样式
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="width"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ILineSymbol CreateLineSymbol(int r, int g, int b, int width, object obj)
        {
            ICartographicLineSymbol pCartoLineSymbol = new CartographicLineSymbolClass();
            pCartoLineSymbol.Cap = esriLineCapStyle.esriLCSRound;

            ILineProperties pLineProp = pCartoLineSymbol as ILineProperties;
            pLineProp.DecorationOnTop = true;

            ILineDecoration pLineDecoration = new LineDecorationClass();
            ISimpleLineDecorationElement pSimpleLineDecoElem = new SimpleLineDecorationElementClass();
            pSimpleLineDecoElem.AddPosition(1);

            IRgbColor pColor = IColorHelper.CreateRGBColor(r, g, b);

            IArrowMarkerSymbol pArrowMarkerSym = new ArrowMarkerSymbolClass();
            pArrowMarkerSym.Size = 6;
            pArrowMarkerSym.Color = pColor;
            pSimpleLineDecoElem.MarkerSymbol = pArrowMarkerSym as IMarkerSymbol;
            pLineDecoration.AddElement(pSimpleLineDecoElem as ILineDecorationElement);
            pLineProp.LineDecoration = pLineDecoration;

            ILineSymbol pLineSymbol = pCartoLineSymbol as ILineSymbol;


            return pLineSymbol;
        }
        /// <summary>
        /// 创建面符号
        /// </summary>
        /// <param name="pCurrenStyle"></param>
        /// <returns></returns>
        public static ISimpleFillSymbol CreateFillSymbol(int r, int g, int b, esriSimpleFillStyle pCurrenStyle)
        {
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Style = pCurrenStyle;


            IRgbColor pColor = IColorHelper.CreateRGBColor(r, g, b);
            pColor.Transparency = 20;
            IRgbColor plColor = IColorHelper.CreateRGBColor(r, g, b);
            pColor.Transparency = 20;
            ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
            pLineSymbol.Width = 1;
            pLineSymbol.Color = plColor;

            pFillSymbol.Outline = pLineSymbol;
            pFillSymbol.Color = pColor;
            return pFillSymbol;
        }
        /// <summary>
        /// 注释样式
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="dbAngle"></param>
        /// <param name="dbFntSize"></param>
        /// <param name="FntType"></param>
        /// <returns></returns>
        public static ITextSymbol CreateTextSymbol(int red, int green, int blue, double dbAngle, double dbFntSize, string fntName)
        {
            ITextSymbol textSymbol;
            textSymbol = new TextSymbolClass();
            stdole.IFontDisp pFont;
            stdole.StdFontClass a = new stdole.StdFontClass();
            pFont = new stdole.StdFontClass() as stdole.IFontDisp;
            pFont.Name = fntName;//arial
            pFont.Bold = true;

            //Set the text symbol font by getting the IFontDisp interface
            textSymbol.Font = pFont;
            textSymbol.Color = IColorHelper.CreateRGBColor(red, green, blue);
            textSymbol.Size = dbFntSize;    //12
            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
            textSymbol.Angle = dbAngle;

            return textSymbol;
        }
        /// <summary>
        /// 注释样式-axPageLayoutControl
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="dbAngle"></param>
        /// <param name="dbFntSize"></param>
        /// <param name="fntName"></param>
        /// <param name="HAliag"></param>
        /// <param name="VAliag"></param>
        /// <returns></returns>
        public static ITextSymbol CreateTextSymbol(int red, int green, int blue, double dbAngle, double dbFntSize, string fntName, esriTextHorizontalAlignment HAliag, esriTextVerticalAlignment VAliag)
        {
            ITextSymbol textSymbol;
            textSymbol = new TextSymbolClass();
            stdole.IFontDisp pFont;
            stdole.StdFontClass a = new stdole.StdFontClass();
            pFont = new stdole.StdFontClass() as stdole.IFontDisp;
            pFont.Name = fntName;//arial
            pFont.Bold = true;

            //Set the text symbol font by getting the IFontDisp interface
            textSymbol.Font = pFont;
            textSymbol.Color = IColorHelper.CreateRGBColor(red, green, blue);
            textSymbol.Size = dbFntSize;    //12
            textSymbol.HorizontalAlignment = HAliag;
            textSymbol.VerticalAlignment = VAliag;
            textSymbol.Angle = dbAngle;

            return textSymbol;
        }
        /// <summary>
        /// 注释样式-axPageLayoutControl
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="dbAngle"></param>
        /// <param name="dbFntSize"></param>
        /// <param name="fntName"></param>
        /// <param name="HAliag"></param>
        /// <param name="VAliag"></param>
        /// <returns></returns>
        public static ITextSymbol CreateTextSymbol(int red, int green, int blue, double dbAngle, double dbFntSize, string fntName, esriTextHorizontalAlignment HAliag)
        {
            ITextSymbol textSymbol;
            textSymbol = new TextSymbolClass();
            stdole.IFontDisp pFont;
            stdole.StdFontClass a = new stdole.StdFontClass();
            pFont = new stdole.StdFontClass() as stdole.IFontDisp;
            pFont.Name = fntName;//arial
            pFont.Bold = true;

            //Set the text symbol font by getting the IFontDisp interface
            textSymbol.Font = pFont;
            textSymbol.Color = IColorHelper.CreateRGBColor(red, green, blue);
            textSymbol.Size = dbFntSize;    //12
            textSymbol.HorizontalAlignment = HAliag;

            return textSymbol;
        }



        public static ISymbol CreatePointSymbol(IColor pColor, int width)
        {
            ISimpleMarkerSymbol pSimpleSymbol = new SimpleMarkerSymbolClass();
            pSimpleSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            pSimpleSymbol.Color = pColor;
            pSimpleSymbol.Size = width;
            pSimpleSymbol.Outline = true;
            pSimpleSymbol.OutlineColor = pColor;
            //pSimpleSymbol.XOffset = 6;
            //pSimpleSymbol.YOffset = -6;
            return pSimpleSymbol as ISymbol;
        }
        public static ISymbol CreatePointSymbol(IColor pColor, int width, esriSimpleMarkerStyle Style)
        {
            ISimpleMarkerSymbol pSimpleSymbol = new SimpleMarkerSymbolClass();
            pSimpleSymbol.Style = Style;
            pSimpleSymbol.Color = pColor;
            pSimpleSymbol.Size = width;
            pSimpleSymbol.Outline = true;
            pSimpleSymbol.OutlineColor = pColor;
            //pSimpleSymbol.XOffset = 6;
            //pSimpleSymbol.YOffset = -6;
            return pSimpleSymbol as ISymbol;
        }
    }
}
