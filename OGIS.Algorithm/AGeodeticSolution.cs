using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.Algorithm
{
    public class AGeodeticSolution : IGeodeticSolution
    {
        //常数值
        public const double _dbPAI = 3.14159265358979;
        protected double exchangeArcUnit = _dbPAI / 180;//度转弧度
        protected double exchangeArcUnitSec = 206265;


        //e12 第一偏心率， e22  第二偏心率， alpha 椭球体扁率
        protected double _earthA;     //地球椭球体长半轴
        protected double _earthB;      //地球椭球体短半轴
        protected double _earthC;      //地球椭球体焦半径
        protected double _earthAlpha;  //地球椭球体扁率倒数
        protected double _earthE12;    //第一偏心率平方
        protected double _earthE22;    //第二偏心率平方

        //dtbA0~dtbD0椭球面梯形面积计算公式系数
        protected double _dbA0;
        protected double _dbB0;
        protected double _dbC0;
        protected double _dbD0;

        //dtbA00~dtbDD0椭球面长度计算公式系数
        protected double _dbA00;
        protected double _dbB00;
        protected double _dbC00;
        protected double _dbD00;
        protected const double _dbRou0 = 180 / _dbPAI;
        protected const double _dbRou1 = _dbRou0 * 60;
        protected const double _dbRou2 = _dbRou0 * 3600;

        //图斑椭球梯形计算系数
        protected double _paramA;
        protected double _paramB;
        protected double _paramC;
        protected double _paramD;
        protected double _paramE;

        private static double f = 0;//测试用
        public bool FirstSubject(double L1, double B1, double dbAlp12, double dbLength, out double L2, out double B2, out double dbAlp21)
        {
            try
            {
                return this.OnFirstSubject(L1, B1, dbAlp12, dbLength, out L2, out B2, out dbAlp21);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SecondSubject(double L1, double B1, double L2, double B2, out double dbLength, out double dbAlp12, out double dbAlp21)
        {
            try
            {
                return OnSecondSubject(L1, B1, L2, B2, out dbLength, out dbAlp12, out dbAlp21);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected virtual bool OnFirstSubject(double L1, double B1, double dbAlp12, double dbLength, out double L2, out double B2, out double dbAlp21)
        {
            throw new NotImplementedException();
        }

        protected virtual bool OnSecondSubject(double L1, double B1, double L2, double B2, out double dbLength, out double dbAlp12, out double dbAlp21)
        {
            throw new NotImplementedException();
        }
        
        public void SetParameter(double a, double b, double alpha_inverse)
        {
            _earthA = a;
            if (b <= 0)
                _earthB = _earthA - _earthA / _earthAlpha;
            else
                _earthB = b;
            if (alpha_inverse > 0)
            {
                _earthAlpha = alpha_inverse;
                _earthB = _earthA - _earthA / _earthAlpha;
            }

            _earthE12 = (_earthA * _earthA - _earthB * _earthB) / (_earthA * _earthA);
            _earthE22 = (_earthA * _earthA - _earthB * _earthB) / (_earthB * _earthB);
        }

        public void SetParameterType(int type)
        {
            switch (type)
            {
                case 0://wgs-84
                    _earthA = 6378137.0;
                    _earthAlpha = 298.257223563;
                    break;
                case 1:           //克拉索伏斯基 椭球体
                    _earthA = 6378245.0;
                    _earthAlpha = 298.299999957;    // = 6356863.01877
                    break;
                case 2:             //中国西安80 坐标系 国际1975年推荐使用椭球体
                    _earthA = 6378140.0;
                    _earthAlpha = 298.257165169;   //b = 6356755.3
                    break;
                case 3:               //GRS80
                    _earthA = 6378137.0;
                    _earthAlpha = 298.257223563;     //b = 6356752.314245
                    break;
                case 4:               //1975年国际椭球体
                    _earthA = 6378140.0;
                    _earthAlpha = 298.257;
                    break;
                case 5:                  //2000中国大地坐标系
                    _earthA = 6378137.0;
                    _earthAlpha = 298.257222101;
                    break;
                default:
                    break;
            }
            _earthB = _earthA - _earthA / _earthAlpha;

            _earthE12 = (_earthA * _earthA - _earthB * _earthB) / (_earthA * _earthA);
            _earthE22 = (_earthA * _earthA - _earthB * _earthB) / (_earthB * _earthB);
        }
    }
}
