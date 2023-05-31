using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.Contracts
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 度分秒转化为度
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static double ConvertCoordinate(double degrees, double minutes = 0, double seconds = 0)
        {
            double fuhao = 1;//为负值的时候需要变号
            if (degrees < 0)
                fuhao = -1;
            if (degrees > 360 || degrees < -360)
                return double.NaN;
            if (minutes >= 60 || minutes <= -60)
                return double.NaN;
            if (seconds >= 60 || seconds <= -60)
                return double.NaN;
            return fuhao * Math.Round(Math.Abs(degrees) + Math.Abs(minutes) / 60.0 + Math.Abs(seconds) / 3600.0, 11);
        }
        /// <summary>
        /// 度分秒转化为度
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static double ConvertCoordinateValue(double degrees, double minutes = 0, double seconds = 0, CoordTypeEnum type = CoordTypeEnum.longitude)
        {
            double fuhao = 1;//为负值的时候需要变号
            if (degrees < 0)
                fuhao = -1;
            if (type == CoordTypeEnum.longitude && degrees >= 180 || degrees <= -180)
                return double.NaN;
            if (type == CoordTypeEnum.latitude && degrees >= 90 || degrees <= -90)
                return double.NaN;
            if (minutes >= 60 || minutes <= -60)
                return double.NaN;
            if (seconds >= 60 || seconds <= -60)
                return double.NaN;
            return fuhao * Math.Round(Math.Abs(degrees) + Math.Abs(minutes) / 60.0 + Math.Abs(seconds) / 3600.0, 11);
        }

        /// <summary>
        /// 转化string为经纬度；
        /// 
        /// </summary>
        /// <param name="coordinateStr"></param>
        /// <param name="unitType">度分秒分割符号类别：空格 分割；破折号-分割；竖线|分割</param>
        /// <param name="value">输出数值</param>
        /// <param name="tipArr">错误提示用数组；length=3;格式正确则为null，否则string提示</param>
        /// <param name="type"></param>
        /// <returns>是否成功</returns>
        public static bool TryConvertCoordinateStr(string coordinateStr, string unitType
            , out double value, ref string[] tipArr, CoordTypeEnum type = CoordTypeEnum.longitude)
        {
            string degreeStr, minuteStr, secondStr;
            value = double.NaN;
            //分解参数
            if (!CoordinateValid.SplitCoordinateStr(coordinateStr, unitType, out degreeStr, out minuteStr, out secondStr, ref tipArr))
                return false;
            //转化值
            return ValidCoordinateStrWithTip(degreeStr, minuteStr, secondStr, out value, out tipArr, type);
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
            , out double value, out string[] tipArr
            , CoordTypeEnum coordType = CoordTypeEnum.longitude)
        {
            value = double.NaN;
            tipArr = new string[] { null, null, null };

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
            if (!CoordinateValid.ValidCoordinate(degree, CoordTypeEnum.longitude))
            {
                double min, max;
                CoordinateValid.GetValidInterval(coordType, out min, out max);
                tipArr[0] = string.Format("坐标：{0}超出了范围（{1}，{2}）", degreeStr, min, max);
                return false;
            }

            //分
            if ((!string.IsNullOrEmpty(minuteStr)) && (!double.TryParse(minuteStr, out minute)))
            {
                tipArr[1] = string.Format("坐标：{0}中分值{1}无法转化为数值", degreeStr, minuteStr);
                return false;
            }
            if (!CoordinateValid.ValidCoordinate(minute, CoordTypeEnum.minute))
            {
                double min, max;
                CoordinateValid.GetValidInterval(CoordTypeEnum.minute, out min, out max);
                tipArr[1] = string.Format("坐标：{0}中分值{1}部分超出了范围（{2}，{3}）", degreeStr, minuteStr, min, max);
                return false;
            }

            //秒
            if (!string.IsNullOrEmpty(secondStr) && !double.TryParse(secondStr, out second))
            {
                tipArr[2] = string.Format("坐标：{0}中秒值{1}无法转化为数值", degreeStr, secondStr);
                return false;
            }
            if (!CoordinateValid.ValidCoordinate(second, CoordTypeEnum.second))
            {
                double min, max;
                CoordinateValid.GetValidInterval(CoordTypeEnum.second, out min, out max);
                tipArr[1] = string.Format("坐标：{0}中秒值{1}部分超出了范围（{2}，{3}）", degreeStr, secondStr, min, max);
                return false;
            }
            value = degree + minute / 60.0 + second / 3600.0;
            return true;
        }

        public static bool ValidCoordinateStrWithTip(string degreeStr
            , out double degree, out string[] tipArr
            , CoordTypeEnum coordType = CoordTypeEnum.longitude)
        {
            degree = double.NaN;
            tipArr = new string[] { null, null, null };
            string str = degreeStr;
            if (degreeStr.Contains("°"))
                str = degreeStr.Replace('°', ' ').Trim();
            if (string.IsNullOrEmpty(str))
            {
                tipArr[0] = "坐标字符串为空";
                return false;
            }

            if (!double.TryParse(str, out degree))
            {
                tipArr[0] = string.Format("坐标：{0}无法转化为数值", degreeStr);
                return false;
            }
            return true;

        }
        /// <summary>
        /// 度转化为度分秒
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public static void ConvertCoordinate(double dbIn, out double degrees, out double minutes, out double seconds)
        {
            double dataIn = Math.Abs(dbIn);
            degrees = Math.Floor(dataIn);
            double dbTempFen;
            dbTempFen = (dataIn - degrees) * 60;
            minutes = Math.Floor(dbTempFen);
            seconds = (dbTempFen - minutes) * 60;

            if (seconds == 60)
            {
                seconds = 0;
                minutes = minutes + 1;
            }
            if (minutes == 60)
            {
                minutes = 0;
                degrees = degrees + 1;
            }
            if (dbIn < 0)
                degrees = -degrees;
        }

        public static bool TryConvertStringToDouble(string temp, out double result)
        {
            result = 0;
            double fuhao = 1;//为负值的时候需要变号
            if (!string.IsNullOrEmpty(temp) && temp.Contains("°"))
            {
                double degrees, minutes = 0, seconds = 0;
                if (!double.TryParse(temp.Substring(0, temp.IndexOf("°")), out degrees))
                    return false;
                if (temp.Substring(0, temp.IndexOf("°")).Contains("-")) fuhao = -1;
                if (temp.Substring(temp.Length - 1, 1).Contains("S") || temp.Substring(temp.Length - 1, 1).Contains("W")) fuhao = -1;
                if (degrees < 0)
                {
                    fuhao = -1;
                    degrees = Math.Abs(degrees);
                }
                if (temp.IndexOf("′") > 0)
                    double.TryParse(temp.Substring(temp.IndexOf("°") + 1, temp.IndexOf("′") - temp.IndexOf("°") - 1), out minutes);
                if (temp.IndexOf("″") > 0)
                    double.TryParse(temp.Substring(temp.IndexOf("′") + 1, temp.IndexOf("″") - temp.IndexOf("′") - 1), out seconds);
                result = (degrees + minutes / 60 + seconds / 3600) * fuhao;
                return true;
            }
            return false;
        }

        public static bool TryConvertStringToDMS(string temp, out double degrees, out double minutes, out double seconds)
        {
            double result = 0;
            degrees = 0; minutes = 0; seconds = 0;
            double fuhao = 1;//为负值的时候需要变号
            if (!string.IsNullOrEmpty(temp) && temp.Contains("°"))
            {

                if (!double.TryParse(temp.Substring(0, temp.IndexOf("°")), out degrees))
                    return false;
                if (temp.Substring(0, temp.IndexOf("°")).Contains("-"))
                    fuhao = -1;
                if (degrees < 0)
                {
                    fuhao = -1;
                    //degrees = Math.Abs(degrees);
                }
                if (temp.IndexOf("′") > 0)
                    double.TryParse(temp.Substring(temp.IndexOf("°") + 1, temp.IndexOf("′") - temp.IndexOf("°") - 1), out minutes);
                if (temp.IndexOf("″") > 0)
                    double.TryParse(temp.Substring(temp.IndexOf("′") + 1, temp.IndexOf("″") - temp.IndexOf("′") - 1), out seconds);
                result = (Math.Abs(degrees) + minutes / 60 + seconds / 3600) * fuhao;
                return true;
            }
            return false;
        }

        public static string ConvertDoubleToString(double dbIn, int figures = 8)
        {
            double degrees, dbTempFen, minutes, seconds;
            double dbvalue = Math.Abs(dbIn);
            degrees = Math.Floor(dbvalue);
            dbTempFen = (dbvalue - degrees) * 60;
            minutes = Math.Floor((dbvalue - degrees) * 60);
            seconds = (dbTempFen - minutes) * 60;
            if (seconds == 60 || Convert.ToDouble(seconds.ToString("f" + figures)) == 60)
            {
                seconds = 0;
                minutes = minutes + 1;
            }
            if (minutes == 60 || Convert.ToDouble(minutes.ToString("f" + figures)) == 60)
            {
                minutes = 0;
                degrees = degrees + 1;
            }
            return string.Format("{3}{0}°{1}′{2}″", degrees, minutes, seconds.ToString("f" + figures), dbIn >= 0 ? "" : "-");
        }


        public static void ConvertDistance(double distIn, out double hl)
        {
            hl = distIn / 10;
        }

        public static string CovertDistance(double distIn, int digits = 0)
        {

            double dbDistNM = Math.Round(distIn / 1852.0, digits);
            double dbDistKM = Math.Round(distIn / 1000.0, digits);
            var result = string.Format("{0}海里({1}公里)", dbDistNM, dbDistKM);
            return result;
        }



    }
}
