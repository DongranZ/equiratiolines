using Microsoft.VisualStudio.TestTools.UnitTesting;
using OGIS.Algorithm;
using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.UnitTest
{
    [TestClass]
    public class IGeodeticSolutionUnitTest
    {
        List<IGeodeticSolution> _list;

        public IGeodeticSolutionUnitTest()
        {
            _list = new List<IGeodeticSolution>();
            _list.Add(new GeodeticSolution_Vincenty());
            _list.Add(new GeodeticSolution_KarneyGeodesic());
        }

        [TestMethod]
        public void DirectUnitTest()
        {
            //算例一
            var dataStrList = new List<string[]>() {
                new string[]{ "30°29′58.2043″","120°05′40.2184″","30°24′05.8354″","119°49′23.3854″","28230.935","247°27′50.428″","67°19′35.373″" }
                ,new string[]{ "35°00′00.22″", "90°00′00.11″", "-30°29′20.96″", "215°59′04.34″", "15000000.2", "100°00′00.33″", "290°32′53.39″" }
            };
            //算例2
            //,new string[] { "", "", "", "", "", "", "" }
            dataStrList = new List<string[]>() {
                //new string[]{ "50°00′00″", "10°00′00″", "-62°57′03.203867″", "105°05′38.299663″", "15000000", "140°00′00″", "294°46′41.483903" }
                new string[]{ "15°00′00″", "15°00′00″", "00°00′0.00019107651426″", "155°00′00.00034613638″", "15330446.775919888", "72°39′45.8268325704159″", "292°44′34.9921639751847″" }
                ,new string[]{ "-15°00′00″", "70°00′00″", "90°00′00″", "70°00′00″", "11661156.725250330", "00°00′00″", "180°00′00″" }
            ,new string[] { "00°00′00.0000″", "00°00′00.0000″", "23°39′23.744650″", "139°04′35.781112″", "95000000", "55°00′00″", "296°38′33.318321″" }
            };
            var datasList = new List<double[]>();
            dataStrList.ForEach(item =>
            {
                var dataValue = new double[7];
                for (int i = 0; i < 7; i++)
                {
                    double theValue;
                    if (i == 4)
                    {
                        theValue=double.Parse(item[i]);
                    }
                    else
                    {
                        ConvertHelper.TryConvertStringToDouble(item[i], out theValue);
                    }
                    dataValue[i]= theValue;
                }
                datasList.Add(dataValue);
            });
            foreach (var item in _list)
            {
                item.SetParameterType(1);
                foreach (var dataItem in datasList)
                {
                    double l2, b2, angle21;
                    item.FirstSubject(dataItem[1], dataItem[0], dataItem[5], dataItem[4],out l2,out b2,out angle21);
                    if (angle21 < 0)
                        angle21 += 180;
                  //  Debug.WriteLine($"{item.GetType().Name.PadRight(40,' ')}:经度{ConvertHelper.ConvertDoubleToString(l2)} 经度误差{l2- dataItem[3]}；纬度{ConvertHelper.ConvertDoubleToString(b2)} 纬度误差{b2 - dataItem[2]}； 反向方向角{ConvertHelper.ConvertDoubleToString(angle21)} 方向角误差{angle21 - dataItem[6]}");
                }
            }

        }

        [TestMethod]
        public void InverseUnitTest()
        {
        }
    }
}
