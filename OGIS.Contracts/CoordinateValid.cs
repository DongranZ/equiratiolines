using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.Contracts
{
    public static class CoordinateValid
    {

        /// <summary>
        /// 获取坐标范围
        /// </summary>
        /// <param name="coordType"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void GetValidInterval(CoordTypeEnum coordType, out double min, out double max)
        {
            min = 0; max = 0;
            switch (coordType)
            {
                case CoordTypeEnum.latitude:
                    min = -180;
                    max = 180;
                    break;
                case CoordTypeEnum.longitude:
                    min = -180;
                    max = 180;
                    break;
                case CoordTypeEnum.minute:
                    min = 0;
                    max = 60;
                    break;
                case CoordTypeEnum.second:
                    min = 0;
                    max = 60;
                    break;
            }
        }

        /// <summary>
        /// 验证坐标值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="coordType"></param>
        /// <returns></returns>
        public static bool ValidCoordinate(double values, CoordTypeEnum coordType)
        {
            double min, max;
            GetValidInterval(coordType, out min, out max);
            if (values < min || values >= max) return false;
            return true;
        }

        /*
        public static bool ValidCoordinateStrWithTip(string degreeStr, out double value, out string[] tipArr, CoordTypeEnum coordType = CoordTypeEnum.longitude)
        {
            value = double.NaN;
            tipArr=new string[]{null,null,null};

            double degree = 0;

            //度
            if (string.IsNullOrEmpty(degreeStr))
            {
                tipArr[0] = string.Format("坐标：{0}内容为空", degreeStr);
                return false;
            }
            if (!double.TryParse(degreeStr, out degree))
            {
                tipArr[0] = string.Format("坐标：{0}无法转化为数值",degreeStr);
                return false;
            }
            if (!ValidCoordinate(degree, CoordTypeEnum.longitude))
            {
                double min, max;
                GetValidInterval(coordType, out min, out max);
                tipArr[0] = string.Format("坐标：{0}超出了范围（{1}，{2}）", degreeStr, min, max);
                return false;
            }
            ValidCoordinateStr(degreeStr, "", "", out value, coordType);
            return true;
        }
        /// <summary>
        /// 验证string是否有效的坐标
        /// </summary>
        /// <param name="degreeStr"></param>
        /// <param name="minuteStr"></param>
        /// <param name="secondStr"></param>
        /// <param name="value"></param>
        /// <param name="coordType"></param>
        /// <returns></returns>
        public static bool ValidCoordinateStr(string degreeStr,string minuteStr,string secondStr
            ,out double value
            ,CoordTypeEnum coordType=CoordTypeEnum.longitude)
        {
            value = double.NaN;
            double degree=0, minute=0, second=0;
            //度
            if (!double.TryParse(degreeStr, out degree)||!ValidCoordinate(degree, CoordTypeEnum.longitude))
                return false;

            //分
            if ((!string.IsNullOrEmpty(minuteStr)) && (!double.TryParse(minuteStr, out minute)))
                return false;
            if(!ValidCoordinate(minute, CoordTypeEnum.minute))
                return false;

            //秒
            if ((!string.IsNullOrEmpty(secondStr) && !double.TryParse(secondStr, out second))
                || !ValidCoordinate(second, CoordTypeEnum.second))
                return false;
            value = degree + minute / 60.0 + second / 3600.0;
            return true;
        }
        /// <summary>
        /// 验证string是否有效的坐标
        /// </summary>
        /// <param name="degreeStr"></param>
        /// <param name="minuteStr"></param>
        /// <param name="secondStr"></param>
        /// <param name="value"></param>
        /// <param name="coordType"></param>
        /// <returns></returns>
        public static bool ValidCoordinateStrWithTip(string degreeStr, string minuteStr, string secondStr
            , out double value,out string[] tipArr
            , CoordTypeEnum coordType = CoordTypeEnum.longitude)
        {
            value = double.NaN;
            tipArr = new string[] { null,null,null};

            double degree = 0, minute = 0, second = 0;
            //度
            if (string.IsNullOrEmpty(degreeStr))
            {
                tipArr[0] = string.Format("坐标：{0}内容为空", degreeStr);
                return false;
            }
            if (!double.TryParse(degreeStr, out degree))
            {
                tipArr[0] = string.Format("坐标：{0}无法转化为数值", degreeStr);
                return false;
            }
            if (!ValidCoordinate(degree, CoordTypeEnum.longitude))
            {
                double min, max;
                GetValidInterval(coordType, out min, out max);
                tipArr[0] = string.Format("坐标：{0}超出了范围（{1}，{2}）", degreeStr, min, max);
                return false;
            }

            //分
            if ((!string.IsNullOrEmpty(minuteStr)) && (!double.TryParse(minuteStr, out minute)))
            {
                tipArr[1] = string.Format("坐标：{0}中分值{1}无法转化为数值", degreeStr, minuteStr);
                return false;
            }
            if (!ValidCoordinate(minute, CoordTypeEnum.minute))
            {
                double min, max;
                GetValidInterval(CoordTypeEnum.minute, out min, out max);
                tipArr[1] = string.Format("坐标：{0}中分值{1}部分超出了范围（{2}，{3}）", degreeStr, minuteStr, min, max);
                return false;
            }

            //秒
            if (!string.IsNullOrEmpty(secondStr) && !double.TryParse(secondStr, out second))
            {
                tipArr[2] = string.Format("坐标：{0}中秒值{1}无法转化为数值", degreeStr,secondStr);
                return false;
            }
            if(!ValidCoordinate(second, CoordTypeEnum.second))
            {
                double min, max;
                GetValidInterval(CoordTypeEnum.second, out min, out max);
                tipArr[1] = string.Format("坐标：{0}中秒值{1}部分超出了范围（{2}，{3}）", degreeStr,secondStr, min, max);
                return false;
            }
            value = degree + minute / 60.0 + second / 3600.0;
            return true;
        }


        /// <summary>
        /// 验证度分秒是否有效坐标
        /// </summary>
        /// <param name="coordinateStr"></param>
        /// <param name="unitType"></param>
        /// <param name="value"></param>
        /// <param name="coordType"></param>
        /// <returns></returns>
        public static bool ValidCoordinateStr(string coordinateStr,string unitType
            ,out double value
            ,CoordTypeEnum coordType=CoordTypeEnum.longitude)
        {
            value = double.NaN;
            string degreeStr, minuteStr="", secondStr="";
            if (unitType == "°")
            {
                if(coordinateStr.Contains("°"))
                    degreeStr = coordinateStr.Replace('°','|').Trim();
                else
                    degreeStr=coordinateStr;
                return ValidCoordinateStr(degreeStr.Split('|')[0], minuteStr, secondStr, out value, coordType);
            }
           
            //度分秒格式
            int indexDeg=-1, indexMin=-1, indexSec=-1;
            indexDeg=coordinateStr.IndexOf('°');
            indexMin = coordinateStr.IndexOf('′');
            indexSec = coordinateStr.LastIndexOf('″');
            if (indexDeg < 0)
                return false;
            else
                degreeStr = coordinateStr.Substring(0,indexDeg);

            if (indexMin < 0)
                return false;
            else
                minuteStr = coordinateStr.Substring(indexDeg+1,indexMin-indexDeg-1);
            
            if(indexSec>=0)
                secondStr=coordinateStr.Substring(indexMin+1,indexSec-indexMin-1);
            else
                secondStr = coordinateStr.Substring(indexMin + 1, coordinateStr.Length - indexMin-1);

            return ValidCoordinateStr(degreeStr, minuteStr, secondStr, out value, coordType);           
        }

        public static bool ValidCoordinateStr(string coordinateStr, string unitType
            , out double value
            , CoordTypeEnum coordType = CoordTypeEnum.longitude)
        {
            value = double.NaN;
            switch (unitType)
            {
                case " ":
                    break;
                case "|":
                    break;
                case "-":
                    break;
                case "°":
                    break;
                case "°′″":
                    break;
            }

            string degreeStr, minuteStr = "", secondStr = "";
            if (unitType == "°")
            {
                if (coordinateStr.Contains("°"))
                    degreeStr = coordinateStr.Replace('°', '|').Trim();
                else
                    degreeStr = coordinateStr;
                return ValidCoordinateStr(degreeStr.Split('|')[0], minuteStr, secondStr, out value, coordType);
            }

            //度分秒格式
            int indexDeg = -1, indexMin = -1, indexSec = -1;
            indexDeg = coordinateStr.IndexOf('°');
            indexMin = coordinateStr.IndexOf('′');
            indexSec = coordinateStr.LastIndexOf('″');
            if (indexDeg < 0)
                return false;
            else
                degreeStr = coordinateStr.Substring(0, indexDeg);

            if (indexMin < 0)
                return false;
            else
                minuteStr = coordinateStr.Substring(indexDeg + 1, indexMin - indexDeg - 1);

            if (indexSec >= 0)
                secondStr = coordinateStr.Substring(indexMin + 1, indexSec - indexMin - 1);
            else
                secondStr = coordinateStr.Substring(indexMin + 1, coordinateStr.Length - indexMin - 1);

            return ValidCoordinateStr(degreeStr, minuteStr, secondStr, out value, coordType);
        }

        public static bool ValidCoordinateStrWithTip(string coordinateStr, string unitType
            , out double value,out string[] tipArr
            , CoordTypeEnum coordType = CoordTypeEnum.longitude)
        {
            value = double.NaN;
            tipArr = new string[] { null, null, null };

            string degreeStr, minuteStr = "", secondStr = "";
            if (unitType == "°")
            {
                if (coordinateStr.Contains("°"))
                    degreeStr = coordinateStr.Replace('°', '|').Trim();
                else
                    degreeStr = coordinateStr;
                return ValidCoordinateStrWithTip(degreeStr.Split('|')[0], minuteStr, secondStr, out value, out tipArr, coordType);
            }
            

            //度分秒格式
            int indexDeg = -1, indexMin = -1, indexSec = -1;
            indexDeg = coordinateStr.IndexOf('°');
            indexMin = coordinateStr.IndexOf('′');
            indexSec = coordinateStr.LastIndexOf('″');
            if (indexDeg < 0)
                return false;
            else
                degreeStr = coordinateStr.Substring(0, indexDeg);

            if (indexMin < 0)
                return false;
            else
                minuteStr = coordinateStr.Substring(indexDeg + 1, indexMin - indexDeg - 1);

            if (indexSec >= 0)
                secondStr = coordinateStr.Substring(indexMin + 1, indexSec - indexMin - 1);
            else
                secondStr = coordinateStr.Substring(indexMin + 1, coordinateStr.Length - indexMin - 1);

            return ValidCoordinateStr(degreeStr, minuteStr, secondStr, out value, coordType);
        }


        */


        public static bool SplitCoordinateStr(string coordinateStr, string unitType
            , out string degreeStr, out string minuteStr, out string secondStr, ref string[] tipArr)
        {
            degreeStr = null;
            minuteStr = null;
            secondStr = null;

            switch (unitType)
            {
                case " ":
                    return OnSplitCoordinateStr_Space(coordinateStr, out degreeStr, out minuteStr, out secondStr, ref tipArr);
                //break;
                case "-":
                    return OnSplitCoordinateStr_Dash(coordinateStr, out degreeStr, out minuteStr, out secondStr, ref tipArr);
                //break;
                case "°":
                    return OnSplitCoordinateStr_Degree(coordinateStr, out degreeStr, out minuteStr, out secondStr, ref tipArr);
                //break;
                case "°′″":
                    return OnSplitCoordinateStr_DegreeMinuteSecond(coordinateStr, out degreeStr, out minuteStr, out secondStr, ref tipArr);
                //break;
                default:
                    degreeStr = coordinateStr.Trim();
                    return true;
            }
        }

        public static bool OnSplitCoordinateStr_Space(string coordinateStr
            , out string degreeStr, out string minuteStr, out string secondStr, ref string[] tipArr)
        {
            degreeStr = null;
            minuteStr = null;
            secondStr = null;

            if (string.IsNullOrEmpty(coordinateStr))
                tipArr[0] = "坐标字符串为空";
            var coordArr = coordinateStr.Split(' ');
            if (coordArr.Length == 3)
            {
                degreeStr = coordArr[0];
                minuteStr = coordArr[1];
                secondStr = coordArr[2];
            }
            else
            {
                tipArr[0] = string.Format("坐标{0}格式不正确，无法使用符号“-”获取度分秒", coordinateStr);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 以破折号分割 120-20-2
        /// </summary>
        /// <param name="coordinateStr"></param>
        /// <param name="degreeStr"></param>
        /// <param name="minuteStr"></param>
        /// <param name="secondStr"></param>
        /// <param name="tipArr"></param>
        /// <returns></returns>
        public static bool OnSplitCoordinateStr_Dash(string coordinateStr
            , out string degreeStr, out string minuteStr, out string secondStr, ref string[] tipArr)
        {
            degreeStr = null;
            minuteStr = null;
            secondStr = null;

            if (string.IsNullOrEmpty(coordinateStr))
                tipArr[0] = "坐标字符串为空";
            var coordArr = coordinateStr.Trim().Split('-');
            if (coordArr.Length == 3)
            {
                degreeStr = coordArr[0];
                minuteStr = coordArr[1];
                secondStr = coordArr[2];
            }
            else
            {
                tipArr[0] = string.Format("坐标{0}格式不正确，无法使用符号“-”获取度分秒", coordinateStr);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 以°分割，无分秒
        /// </summary>
        /// <param name="coordinateStr"></param>
        /// <param name="degreeStr"></param>
        /// <param name="minuteStr"></param>
        /// <param name="secondStr"></param>
        /// <param name="tipArr"></param>
        /// <returns></returns>
        public static bool OnSplitCoordinateStr_Degree(string coordinateStr
            , out string degreeStr, out string minuteStr, out string secondStr, ref string[] tipArr)
        {
            degreeStr = null;
            minuteStr = null;
            secondStr = null;

            if (string.IsNullOrEmpty(coordinateStr))
                tipArr[0] = "坐标字符串为空";

            if (coordinateStr.Contains("°"))
                degreeStr = coordinateStr.Replace("°", " ").Trim();
            degreeStr = coordinateStr;
            return true;
        }

        /// <summary>
        /// 以°分割，无分秒
        /// </summary>
        /// <param name="coordinateStr"></param>
        /// <param name="degreeStr"></param>
        /// <param name="minuteStr"></param>
        /// <param name="secondStr"></param>
        /// <param name="tipArr"></param>
        /// <returns></returns>
        public static bool OnSplitCoordinateStr_DegreeMinuteSecond(string coordinateStr
            , out string degreeStr, out string minuteStr, out string secondStr, ref string[] tipArr)
        {
            degreeStr = null;
            minuteStr = null;
            secondStr = null;

            //度分秒格式
            int indexDeg = -1, indexMin = -1, indexSec = -1;
            indexDeg = coordinateStr.IndexOf('°');
            indexMin = coordinateStr.IndexOf('′');
            indexSec = coordinateStr.LastIndexOf('″');
            if (indexDeg < 0)
            {
                return false;
                tipArr[0] = string.Format("坐标{0}字符串中度{1}格式不正确", coordinateStr);
            }
            else
                degreeStr = coordinateStr.Substring(0, indexDeg);

            if (indexMin < 0)
                return false;
            else
                minuteStr = coordinateStr.Substring(indexDeg + 1, indexMin - indexDeg - 1);

            if (indexSec >= 0)
                secondStr = coordinateStr.Substring(indexMin + 1, indexSec - indexMin - 1);
            else
                secondStr = coordinateStr.Substring(indexMin + 1, coordinateStr.Length - indexMin - 1);

            return true;
        }
    }
}
