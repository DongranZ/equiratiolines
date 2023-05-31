using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.Algorithm
{
    [Export(typeof(IGeodeticSolution))]
    public class GeodeticSolution_Vincenty : AGeodeticSolution
    {
        protected override bool OnFirstSubject(double L1, double B1, double dbAlp12, double dbLength, out double L2, out double B2, out double dbAlp21)
        {
            B2 = L2 = dbAlp21 = double.NaN;
            try
            {

                double φ1 = B1 * _dbPAI / 180.0;
                double λ1 = L1 * _dbPAI / 180.0;
                double f = 1 / _earthAlpha;
                double α1 = dbAlp12 * _dbPAI / 180.0;
                double sinα1 = Math.Sin(α1);
                double cosα1 = Math.Cos(α1);

                double tanU1 = (1 - f) * Math.Tan(φ1)
                    , cosU1 = 1 / Math.Sqrt((1 + tanU1 * tanU1))
                    , sinU1 = tanU1 * cosU1;
                double σ1 = Math.Atan2(tanU1, cosα1);
                double sinα = cosU1 * sinα1;
                double cosSqα = 1 - sinα * sinα;
                double uSq = cosSqα * (_earthA * _earthA - _earthB * _earthB) / (_earthB * _earthB);
                double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
                double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

                double σ = dbLength / (_earthB * A);
                double σPie;
                double cos2σM, sinσ, cosσ, Δσ;
                do
                {
                    cos2σM = Math.Cos(2 * σ1 + σ);
                    sinσ = Math.Sin(σ);
                    cosσ = Math.Cos(σ);
                    Δσ = B * sinσ * (cos2σM + B / 4 * (cosσ * (-1 + 2 * cos2σM * cos2σM) -
                       B / 6 * cos2σM * (-3 + 4 * sinσ * sinσ) * (-3 + 4 * cos2σM * cos2σM)));
                    σPie = σ;
                    σ = dbLength / (_earthB * A) + Δσ;
                    //} while (Math.Abs(σ - σPie) > 1e-12);
                } while (Math.Abs(σ - σPie) * base.exchangeArcUnitSec > 1e-6);

                double tmp = sinU1 * sinσ - cosU1 * cosσ * cosα1;
                double φ2 = Math.Atan2(sinU1 * cosσ + cosU1 * sinσ * cosα1, (1 - f) * Math.Sqrt(sinα * sinα + tmp * tmp));
                double λ = Math.Atan2(sinσ * sinα1, cosU1 * cosσ - sinU1 * sinσ * cosα1);
                double C = f / 16 * cosSqα * (4 + f * (4 - 3 * cosSqα));
                double L = λ - (1 - C) * f * sinα *
                    (σ + C * sinσ * (cos2σM + C * cosσ * (-1 + 2 * cos2σM * cos2σM)));
                double λ2 = (λ1 + L + 3 * _dbPAI) % (2 * _dbPAI) - _dbPAI;  // normalise to -180...+180

                double revAz = Math.Atan2(sinα, -tmp);
                B2 = φ2 / _dbPAI * 180.0;
                L2 = λ2 / _dbPAI * 180.0;

                double angle21 = revAz + _dbPAI;
                if (angle21 > 2 * _dbPAI)
                {
                    angle21 -= 2 * _dbPAI;
                }
                else if (angle21 < 0)
                {
                    angle21 += 2 * _dbPAI;
                }
                //dbAlp21 = revAz / _dbPAI * 180.0;
                dbAlp21 = angle21 / _dbPAI * 180.0;

            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        protected override bool OnSecondSubject(double L1, double B1, double L2, double B2, out double dbLength, out double dbAlp12, out double dbAlp21)
        {
            dbLength = double.NaN;
            dbAlp12 = double.NaN;
            dbAlp21 = double.NaN;
            if (L1 == L2 && B1 == B2)
            {
                dbLength = 0;
                dbAlp12 = 0;
                dbAlp21 = 0;
                return true;
            }


            double φ1 = B1 * _dbPAI / 180.0;
            double φ2 = B2 * _dbPAI / 180.0;
            double λ1 = L1 * _dbPAI / 180.0;
            double λ2 = L2 * _dbPAI / 180.0;
            double f = 1 / _earthAlpha;
            double L = λ2 - λ1;
            double tanU1 = (1 - f) * Math.Tan(φ1)
                , cosU1 = 1 / Math.Sqrt((1 + tanU1 * tanU1))
                , sinU1 = tanU1 * cosU1;
            double tanU2 = (1 - f) * Math.Tan(φ2)
                , cosU2 = 1 / Math.Sqrt((1 + tanU2 * tanU2))
                , sinU2 = tanU2 * cosU2;

            double λ = L, λPie, iterationLimit = 1000;
            double sinλ, cosλ, sinSqσ, sinσ, cosσ;
            double σ, sinα, cosSqα, cos2σM, C;
            do
            {
                sinλ = Math.Sin(λ);
                cosλ = Math.Cos(λ);
                sinSqσ = (cosU2 * sinλ) * (cosU2 * sinλ) + (cosU1 * sinU2 - sinU1 * cosU2 * cosλ) * (cosU1 * sinU2 - sinU1 * cosU2 * cosλ);
                sinσ = Math.Sqrt(sinSqσ);
                if (sinσ == 0) return false;  // co-incident points
                cosσ = sinU1 * sinU2 + cosU1 * cosU2 * cosλ;
                σ = Math.Atan2(sinσ, cosσ);
                sinα = cosU1 * cosU2 * sinλ / sinσ;
                cosSqα = 1 - sinα * sinα;
                cos2σM = cosσ - 2 * sinU1 * sinU2 / cosSqα;
                if (double.IsNaN(cos2σM)) cos2σM = 0;  // equatorial line: cosSqα=0 (§6)
                C = f / 16 * cosSqα * (4 + f * (4 - 3 * cosSqα));
                λPie = λ;
                λ = L + (1 - C) * f * sinα * (σ + C * sinσ * (cos2σM + C * cosσ * (-1 + 2 * cos2σM * cos2σM)));
                //} while (Math.Abs(λ - λPie) > 1e-12 && --iterationLimit > 0);
            } while (Math.Abs(λ - λPie) * base.exchangeArcUnitSec > 1e-6 && --iterationLimit > 0);
            if (iterationLimit == 0)
                throw new Exception("Formula failed to converge");

            double uSq = cosSqα * (_earthA * _earthA - _earthB * _earthB) / (_earthB * _earthB);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            double Δσ = B * Math.Sin(σ) * (cos2σM + B / 4 * (Math.Cos(σ) * (-1 + 2 * cos2σM * cos2σM) -
                B / 6 * cos2σM * (-3 + 4 * Math.Sin(σ) * Math.Sin(σ)) * (-3 + 4 * cos2σM * cos2σM)));

            double s = _earthB * A * (σ - Δσ);

            double fwdAz = Math.Atan2(cosU2 * Math.Sin(λ), cosU1 * sinU2 - sinU1 * cosU2 * Math.Cos(λ));//开始方位角
            double revAz = Math.Atan2(cosU1 * Math.Sin(λ), -sinU1 * cosU2 + cosU1 * sinU2 * Math.Cos(λ));//结束方位角

            double angle12;//正向方位角
            double angle21;//反向方位角=结束方位角+180°
            angle12 = fwdAz;
            angle21 = revAz + _dbPAI;
            //if (B1 > B2)
            //{
            //    angle12 = fwdAz + _dbPAI;
            //    angle21 = revAz;
            //}
            //else
            //{
            //    angle12 = fwdAz;
            //    angle21 = revAz + _dbPAI;
            //}

            if (angle12 > 2 * _dbPAI)//经纬度回归到0~2π范围
            { angle12 -= 2 * _dbPAI; }
            if (angle12 < 0)
            { angle12 += 2 * _dbPAI; }

            if (angle21 < 0)
            { angle21 += 2 * _dbPAI; }
            if (angle21 > 2 * _dbPAI)
            { angle21 -= 2 * _dbPAI; }
            dbLength = s;
            dbAlp12 = angle12 / _dbPAI * 180;
            dbAlp21 = angle21 / _dbPAI * 180;
            return true;
        }

    }
}
