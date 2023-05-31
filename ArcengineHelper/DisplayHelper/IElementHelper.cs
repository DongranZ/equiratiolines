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
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace ArcengineHelper.DisplayHelper
{
    public static class IElementHelper
    {
        //创建符号要素
        /// <summary>
        /// 创建点图元
        /// </summary>
        /// <param name="pGeo"></param>
        /// <returns></returns>
        public static IElement CreatePointElement(IPoint pGeo, int r, int g, int b, int width)
        {
            IMarkerElement pPointElement = new MarkerElementClass();

            ISymbol pSymbol = ISymbolHelper.CreatePointSymbol(r, g, b, width);
            pPointElement.Symbol = pSymbol as IMarkerSymbol;
            IElement pElement = (IElement)pPointElement;
            pElement.Geometry = pGeo;
            return pElement;
        }
        /// <summary>
        /// 创建点图元
        /// </summary>
        /// <param name="pGeo"></param>
        /// <returns></returns>
        public static IElement CreatePointElement(IPoint pGeo, int r, int g, int b, int width, esriSimpleMarkerStyle style)
        {
            IMarkerElement pPointElement = new MarkerElementClass();

            ISymbol pSymbol = ISymbolHelper.CreatePointSymbol(r, g, b, width, style);
            pPointElement.Symbol = pSymbol as IMarkerSymbol;
            IElement pElement = (IElement)pPointElement;
            pElement.Geometry = pGeo;
            return pElement;
        }

        /// <summary>
        /// 创建线图元
        /// </summary>
        /// <param name="pLine"></param>
        /// <returns></returns>
        public static IElement CreateLineElement(IPolyline pLine, int width, int r, int g, int b)
        {
            ILineElement pLineElement = new LineElementClass();
            ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
            IRgbColor pRgb = new RgbColorClass();
            pRgb.Red = r;
            pRgb.Green = g;
            pRgb.Blue = b;
            pLineSymbol.Color = pRgb;
            pLineSymbol.Width = width;
            pLineElement.Symbol = pLineSymbol;
            IElement pElement;
            pElement = (IElement)pLineElement;
            pElement.Geometry = pLine;
            return pElement;
        }
        public static IElement CreateHashLineEement(IPolyline pLine, int width, int r, int g, int b)
        {
            ILineElement pLineElement = new LineElementClass();
            ILineSymbol pLineSymbol = new HashLineSymbolClass();
            IRgbColor pRgb = new RgbColorClass();
            pRgb.Red = r;
            pRgb.Green = g;
            pRgb.Blue = b;
            pLineSymbol.Color = pRgb;
            pLineSymbol.Width = width;
            pLineElement.Symbol = pLineSymbol;
            IElement pElement;
            pElement = (IElement)pLineElement;
            pElement.Geometry = pLine;
            return pElement;
        }

        /// <summary>
        /// 创建面图元
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <param name="pCurrenStyle"></param>
        /// <returns></returns>
        public static IElement CreateRectElement(IGeometry pGeometry, int r, int g, int b, esriSimpleFillStyle pCurrenStyle)
        {

            IElement pElement;
            IFillShapeElement pFillShapeElement = new RectangleElementClass();
            ISimpleFillSymbol pFillSymbol = ISymbolHelper.CreateFillSymbol(r, g, b, pCurrenStyle);
            ISymbol symbol = pFillSymbol as ISymbol;
            symbol.ROP2 = esriRasterOpCode.esriROPMaskPen;
            pFillShapeElement.Symbol = pFillSymbol;
            pElement = (IElement)pFillShapeElement;
            pElement.Geometry = pGeometry;
            return pElement;


        }
        /// <summary>
        /// 创建环图元
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <param name="pCurrenStyle"></param>
        /// <returns></returns>
        public static IElement CreateRingElement(IPolygon ipPolygon, int width, int r, int g, int b)
        {
            ILineElement ipLineEle = new LineElementClass();
            IElement ipRingElement = ipLineEle as IElement;

            ITopologicalOperator ipTopo = ipPolygon as ITopologicalOperator;
            IGeometry ipGeoOut;
            ipGeoOut = ipTopo.Boundary;
            esriGeometryType type = ipGeoOut.GeometryType;
            ipRingElement.Geometry = ipGeoOut;


            ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
            IRgbColor pRgb = new RgbColorClass();
            pRgb.Red = r;
            pRgb.Blue = b;
            pRgb.Green = g;
            pLineSymbol.Color = pRgb;
            pLineSymbol.Width = width;

            ipLineEle.Symbol = pLineSymbol;

            return ipRingElement;

        }
        /// <summary>
        /// 创建矩形环图元
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <param name="pCurrenStyle"></param>
        /// <returns></returns>
        public static IElement CreateRectElement(IEnvelope ipRect, int width, int r, int g, int b)
        {
            ILineElement ipLineEle = new LineElementClass();
            IElement ipRingElement = ipLineEle as IElement;

            ITopologicalOperator ipTopo = ipRect as ITopologicalOperator;
            IGeometry ipGeoOut;
            ipGeoOut = ipTopo.Boundary;
            esriGeometryType type = ipGeoOut.GeometryType;
            ipRingElement.Geometry = ipGeoOut;


            ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
            IRgbColor pRgb = new RgbColorClass();
            pRgb.Red = r;
            pRgb.Green = g;
            pRgb.Blue = b;
            pLineSymbol.Color = pRgb;
            pLineSymbol.Width = width;

            ipLineEle.Symbol = pLineSymbol;

            return ipRingElement;
        }
        /// <summary>
        /// 创建点图片图元
        /// </summary>
        /// <param name="pGeo"></param>
        /// <returns></returns>
        public static IElement CreateMarkPictElem(IGeometry pGeo, string strPath)
        {
            try
            {
                ISymbol pSymbol;
                IElement pElement;
                IMarkerElement pPointElement = new MarkerElementClass();

                IRgbColor pColor = new RgbColorClass();
                pColor.Red = 255;
                pColor.Green = 255;
                pColor.Blue = 255;

                if (strPath.Trim() != "")
                {

                    IPictureMarkerSymbol pPicMarkerSymb = new PictureMarkerSymbolClass();
                    pPicMarkerSymb.CreateMarkerSymbolFromFile(esriIPictureType.esriIPictureBitmap, strPath);
                    pPicMarkerSymb.Size = 8;
                    pPicMarkerSymb.Angle = 0;
                    pPicMarkerSymb.XOffset = 0;
                    pPicMarkerSymb.YOffset = 0;
                    pPicMarkerSymb.Color = pColor;


                    pSymbol = pPicMarkerSymb as ISymbol;

                    pPointElement.Symbol = pSymbol as IMarkerSymbol;
                    pElement = (IElement)pPointElement;
                    pElement.Geometry = pGeo;
                }
                else
                {
                    ISimpleMarkerSymbol pSimpMarkerSymbol;
                    pSimpMarkerSymbol = new SimpleMarkerSymbol();
                    pSimpMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSimpMarkerSymbol.Size = 24;
                    pSimpMarkerSymbol.Color = pColor;
                    pSymbol = (ISymbol)pSimpMarkerSymbol;
                    pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                    pPointElement.Symbol = pSymbol as IMarkerSymbol;
                    pElement = (IElement)pPointElement;
                    pElement.Geometry = pGeo;

                }
                return pElement;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        /// <summary>
        /// 创建点标注
        /// </summary>
        /// <param name="pPoint"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static IElement CreateMarkElement(IPoint pPoint, string strName)
        {
            stdole.IFontDisp pFont;
            pFont = new stdole.StdFontClass() as stdole.IFontDisp;
            pFont.Name = "微软雅黑";

            IRgbColor pRGB;
            pRGB = new RgbColorClass();
            pRGB.Red = 255;
            pRGB.Blue = 0;
            pRGB.Green = 0;

            ITextSymbol pTextSymbol;
            pTextSymbol = new TextSymbolClass();
            pTextSymbol.Size = 3;
            pTextSymbol.Font = pFont;
            pTextSymbol.Color = pRGB;

            ITextElement pTextEle;
            IElement pEle;
            pTextEle = new TextElementClass();
            pTextEle.Text = strName;
            pTextEle.ScaleText = true;
            pTextEle.Symbol = pTextSymbol;

            pEle = pTextEle as IElement;
            pEle.Geometry = pPoint;
            pEle.Deactivate();
            return pEle;

        }
        /// <summary>
        /// 注释元素
        /// </summary>
        /// <param name="pElement"></param>
        /// <param name="pAnchorPt"></param>
        /// <param name="textMarker"></param>
        /// <param name="textSymbol"></param>
        /// <returns></returns>
        public static IElement CreateTextElement(IPoint pAnchorPt, string textMarker, ITextSymbol textSymbol)
        {
            IElement element = null;
            ITextElement textElem;
            IPoint pTempPt;
            ISimpleTextSymbol simpleTextSymbol;
            try
            {
                //填充符号、样式
                textElem = new TextElementClass();
                textElem.Symbol = textSymbol;
                textElem.Text = textMarker;
                //填充图元
                element = textElem as IElement;
                pTempPt = new PointClass();
                pTempPt = pAnchorPt;
                element.Geometry = pTempPt;
                simpleTextSymbol = (ISimpleTextSymbol)textSymbol;//不知道做什么用的 
                return element;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public static IElement CreateTextElement(IPoint point, string textMarker, int red, int green, int blue, double dbAngle, double dbFntSize, string FntType)
        {
            IElement markerElement;
            var markerSymbol = ISymbolHelper.CreateTextSymbol(red, green, blue, dbAngle, dbFntSize, FntType);
            markerElement = IElementHelper.CreateTextElement(point, textMarker, markerSymbol);//获取IElement  
            return markerElement;
        }
        /// <summary>
        /// 注释元素-axPageLayoutControl
        /// </summary>
        /// <param name="pElement"></param>
        /// <param name="pAnchorPt"></param>
        /// <param name="textMarker"></param>
        /// <param name="textSymbol"></param>
        /// <returns></returns>
        public static IElement CreateTextElement(IGeometry geo, string textMarker, string elementPropsName, ITextSymbol textSymbol)
        {
            IElement pElement = null;
            ITextElement pTxtEle = null;
            try
            {
                pTxtEle = new TextElementClass();
                IElement pEle = (IElement)pTxtEle;

                pEle.Geometry = geo;

                pTxtEle.Symbol = textSymbol;
                pTxtEle.Text = textMarker;
                pTxtEle.ScaleText = false;

                ESRI.ArcGIS.Carto.IElementProperties pElementProps;
                pElementProps = pEle as IElementProperties;
                pElementProps.Name = elementPropsName;
                pElement = pTxtEle as IElement;
                return pElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static IElement CreateElement(IGeometry geo)
        {
            IElement element=null;
            switch (geo.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    element = CreatePointElement(geo as IPoint,255,0,0,2); 
                    break;
                case esriGeometryType.esriGeometryMultipoint:
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    element = CreateLineElement(geo as IPolyline,2,255,0,0);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    element = CreateRingElement(geo as IPolygon, 2, 255, 0, 0);
                    break;
                case esriGeometryType.esriGeometryCircularArc:
                    break;

            }
            return element;
                
        }
        //更新要素
        public static IElement UpdatePointElement(IElement element, ISymbol symbol, IGeometry geo = null)
        {
            if (geo != null)
                element.Geometry = geo;     //更新图元       
            var pointElement = element as IMarkerElement;
            pointElement.Symbol = symbol as IMarkerSymbol;//更新样式
            return pointElement as IElement;
        }
        public static IElement UpdatePointElement(IElement element, IColor pColor, IGeometry geo = null)
        {
            if (geo != null)
                element.Geometry = geo;     //更新图元       
            var pointElement = element as IMarkerElement;
            pointElement.Symbol.Color = pColor;
            return pointElement as IElement;
        }
        public static IElement UpdateLineElement(IElement element, ILineSymbol symbol, IGeometry geo = null)
        {
            if (geo != null)
                element.Geometry = geo;     //更新图元       
            var lineElement = element as ILineElement;

            lineElement.Symbol = symbol;//更新样式
            return lineElement as IElement;
        }
        public static IElement UpdateLineElement(IElement element, IColor pColor, IGeometry geo = null)
        {
            if (geo != null)
                element.Geometry = geo;     //更新图元       
            var lineElement = element as ILineElement;

            lineElement.Symbol.Color = pColor;//更新样式
            return lineElement as IElement;
        }
    }
}
