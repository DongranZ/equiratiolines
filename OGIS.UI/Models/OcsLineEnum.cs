using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    /// <summary>
    /// 划界线类型 枚举类
    /// </summary>
    public enum OcsLineEnum : int
    {
        OcsNull = -1,
        BaseLine = 1,//基线
        FootLine = 2,//坡脚线
        Foot60NmBufferLine = 3,//60海里缓冲线
        SedimentLine = 4,//1%沉积物线
        FormularLine = 5,//公式融合线
        Contour2500Line = 6,//2500米等深线
        Contour100NmBufferline = 7,//2500米等深线100海里缓冲
        Coast350Nmline = 8,//350海里海岸线
        LimitLine = 9,//限制线
        FusionLine = 10,//融合线
        FootPoints = 11,//坡脚点
        SedimentPoints = 12,//坡脚点
    }
    /// <summary>
    /// 划界线类型对应 划界线名称
    /// </summary>
    public static class OcsLineEnumText
    {
        //private const string _BaseLine = "基线";
        //private const string _FootLine = "坡脚线";
        //private const string _Foot60NmBufferLine = "60海里缓冲线";
        //private const string _SedimentLine = "1%沉积物线";
        //private const string _FormularLine = "公式线";

        //private const string _Contour2500Line = "2500米等深线";
        //private const string _Contour100NmBufferline = "2500米等深线100海里缓冲";
        //private const string _Coast350Nmline = "领海基线350海里缓冲线";
        //private const string _LimitLine = "限制线";
        //private const string _FusionLine = "融合线";
        private const string _BaseLine = "领海基线";
        private const string _FootLine = "大陆架坡脚线";
        private const string _Foot60NmBufferLine = "FOS六十海里缓冲线";
        private const string _SedimentLine = "百分之一沉积物厚度线";
        private const string _FormularLine = "公式线融合线";

        private const string _Contour2500Line = "两千五百米等深线";
        private const string _Contour100NmBufferline = "等深线一百海里缓冲线";
        private const string _Coast350Nmline = "领海基线三百五十海里缓冲线";
        private const string _LimitLine = "限制线融合线";
        private const string _FusionLine = "大陆架外部界线";
        private const string _FootPoints = "大陆架坡脚点";
        private const string _SedimentPoints = "百分之一沉积物厚度点";
        /// <summary>
        /// 已知 划界线类型，获取其名称。用于名称统一
        /// </summary>
        /// <param name="OcsLineEnum"></param>
        /// <returns></returns>
        public static string GetText(OcsLineEnum OcsLineEnum, string type = "")
        {
            string strText = string.Empty;
            switch (OcsLineEnum)
            {
                case OcsLineEnum.BaseLine:
                    strText = _BaseLine;
                    break;
                case OcsLineEnum.FootLine:
                    strText = _FootLine;
                    break;
                case OcsLineEnum.Foot60NmBufferLine:
                    strText = _Foot60NmBufferLine;
                    break;
                case OcsLineEnum.SedimentLine:
                    strText = _SedimentLine;
                    break;
                case OcsLineEnum.FormularLine:
                    strText = _FormularLine;
                    break;
                case OcsLineEnum.Contour2500Line:
                    strText = _Contour2500Line;
                    break;
                case OcsLineEnum.Contour100NmBufferline:
                    strText = _Contour100NmBufferline;
                    break;
                case OcsLineEnum.Coast350Nmline:
                    strText = _Coast350Nmline;
                    break;
                case OcsLineEnum.LimitLine:
                    strText = _LimitLine;
                    break;
                case OcsLineEnum.FusionLine:
                    strText = _FusionLine;
                    break;
                case OcsLineEnum.FootPoints:
                    strText = _FootPoints;
                    break;
                case OcsLineEnum.SedimentPoints:
                    strText = _SedimentPoints;
                    break;
            }
            return type + strText;
        }

        public static OcsLineEnum GetOCSType(string name)
        {
            if (string.IsNullOrEmpty(name))
                return OcsLineEnum.OcsNull;

            if (name == _BaseLine)
                return OcsLineEnum.BaseLine;
            else if (name == _FootLine)
                return OcsLineEnum.FootLine;
            else if (name == _Foot60NmBufferLine)
                return OcsLineEnum.Foot60NmBufferLine;
            else if (name == _SedimentLine)
                return OcsLineEnum.SedimentLine;
            else if (name == _FormularLine)
                return OcsLineEnum.FormularLine;
            else if (name == _Contour2500Line)
                return OcsLineEnum.Contour2500Line;
            else if (name == _Contour100NmBufferline)
                return OcsLineEnum.Contour100NmBufferline;
            else if (name == _Coast350Nmline)
                return OcsLineEnum.Coast350Nmline;
            else if (name == _LimitLine)
                return OcsLineEnum.LimitLine;
            else if (name == _FusionLine)
                return OcsLineEnum.FusionLine;
            else if (name == _FootPoints)
                return OcsLineEnum.FootPoints;
            else if (name == _SedimentPoints)
                return OcsLineEnum.SedimentPoints;
            else
                return OcsLineEnum.OcsNull;
        }
    }
}
