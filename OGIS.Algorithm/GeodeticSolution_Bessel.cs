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
    public class GeodeticSolution_Bessel : AGeodeticSolution
    {
        protected override bool OnFirstSubject(double L1, double B1, double dbAlp12, double dbLength, out double L2, out double B2, out double dbAlp21)
        {
            L2 = B2 = dbAlp21 = double.NaN;
            double PO = base.exchangeArcUnitSec;//206265;
            double A1 = dbAlp12 * _dbPAI / 180;
            double dbL1 = L1 * _dbPAI / 180;
            double dbB1 = B1 * _dbPAI / 180;
            try
            {

                double U1 = Math.Atan(Math.Sqrt(1 - _earthE12) * Math.Tan(dbB1));
                double sinA0 = Math.Cos(U1) * Math.Sin(A1);
                double cosA0 = Math.Sqrt(1 - sinA0 * sinA0);
                double sigma1 = Math.Atan(Math.Tan(U1) / Math.Cos(A1));
                double zk2 = _earthE22 * cosA0 * cosA0;
                double zk4 = zk2 * zk2;
                double zk6 = zk4 * zk2;
                double alpha = (1 - zk2 / 4 + 7 * zk4 / 64 - 15 * zk6 / 256) / _earthB;
                double beta = zk2 / 4 - zk4 / 8 + 37 * zk6 / 512;
                double gamma = zk4 / 128 - zk6 / 128;
                double sigma = alpha * dbLength;
                double Dsigma;
                do
                {
                    double sigma0 = sigma;
                    sigma = alpha * dbLength + beta * Math.Sin(sigma0) * Math.Cos(2 * sigma1 + sigma0);
                    sigma = sigma + gamma * Math.Sin(2 * sigma0) * Math.Cos(4 * sigma1 + 2 * sigma0);
                    Dsigma = Math.Abs(sigma - sigma0) * PO;//常熟PO=206265
                }
                while (Dsigma > 1e-6);

                //计算方位角A2
                double sinA2 = Math.Cos(U1) * Math.Sin(A1);
                double cosA2 = Math.Cos(U1) * Math.Cos(sigma) * Math.Cos(A1) - Math.Sin(U1) * Math.Sin(sigma);
                double tanA2 = sinA2 / cosA2;
                double A2 = Math.Abs(Math.Atan(sinA2 / cosA2));
                double sinA1 = Math.Sin(A1);
                if (sinA1 < 0 && tanA2 > 0)
                    A2 = A2 + 0;
                else if (sinA1 < 0 && tanA2 < 0)
                    A2 = _dbPAI - A2;
                else if (sinA1 > 0 && tanA2 > 0)
                    A2 = _dbPAI + A2;
                else if (sinA1 > 0 && tanA2 < 0)
                    A2 = 2 * _dbPAI - A2;
                //计算大地纬度 B2
                double sinU2 = Math.Sin(U1) * Math.Cos(sigma) + Math.Cos(U1) * Math.Cos(A1) * Math.Sin(sigma);
                double dbB2 = Math.Atan(sinU2 / Math.Sqrt(1 - _earthE12) / Math.Sqrt(1 - sinU2 * sinU2));
                //计算大地经度L2
                double sin1 = Math.Sin(A1) * Math.Sin(sigma);
                double cos1 = Math.Cos(U1) * Math.Cos(sigma) - Math.Sin(U1) * Math.Sin(sigma) * Math.Cos(A1);
                double tanLambda = sin1 / cos1;
                double lambda = Math.Abs(Math.Atan(sin1 / cos1));
                if (tanLambda > 0 && sinA1 > 0)
                    lambda = lambda + 0;
                else if (tanLambda < 0 && sinA1 > 0)
                    lambda = _dbPAI - lambda;
                else if (tanLambda < 0 && sinA1 < 0)
                    lambda = -lambda;
                else if (tanLambda > 0 && sinA1 < 0)
                    lambda = lambda - _dbPAI;
                double e4 = _earthE12 * _earthE12;
                double e6 = e4 * _earthE12;
                zk2 = _earthE12 * cosA0 * cosA0;
                zk4 = zk2 * zk2;
                zk6 = zk4 * zk2;
                double alpha1 = (_earthE12 / 2 + e4 / 8 + e6 / 16) - _earthE22 * zk2 / 16 + 3 * zk4 * _earthE12 / 128;
                double beta1 = _earthE22 * zk2 / 16 - _earthE12 * zk4 / 32;
                double gamma1 = _earthE12 * zk4 / 256;
                double xx = alpha1 * sigma + beta1 * Math.Sin(sigma) * Math.Cos(2 * sigma1 + sigma);
                xx = xx + gamma1 * Math.Sin(2 * sigma) * Math.Cos(4 * sigma + 2 * sigma);
                double dbl = lambda - sinA0 * xx;
                double dbL2 = dbL1 + dbl;

                if (dbL2 > _dbPAI)//东西经转化
                    dbL2 = dbL2 - 2 * _dbPAI;
                else if (dbL2 < -_dbPAI)
                    dbL2 = dbL2 + 2 * _dbPAI;


                L2 = dbL2 * 180.0 / _dbPAI;
                B2 = dbB2 * 180.0 / _dbPAI;
                dbAlp21 = A2 * 180.0 / _dbPAI;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override bool OnSecondSubject(double L1, double B1, double L2, double B2, out double dbLength, out double dbAlp12, out double dbAlp21)
        {
            dbLength = dbAlp12 = dbAlp21 = double.NaN;

            double dbL1 = L1 * _dbPAI / 180;
            double dbB1 = B1 * _dbPAI / 180;
            double dbL2 = L2 * _dbPAI / 180;
            double dbB2 = B2 * _dbPAI / 180;

            double p0 = 206265;
            double U1 = Math.Atan(Math.Sqrt(1 - _earthE12) * Math.Tan(dbB1));
            double U2 = Math.Atan(Math.Sqrt(1 - _earthE12) * Math.Tan(dbB2));
            double DL = dbL2 - dbL1;
            double sa1 = Math.Sin(U1) * Math.Sin(U2);
            double sa2 = Math.Cos(U1) * Math.Cos(U2);
            double eb1 = Math.Cos(U1) * Math.Sin(U2);
            double eb2 = Math.Sin(U1) * Math.Cos(U2);
            double lambda = DL;
            double Dlambda;
            double sinA0, cosA0;//=Math.Sqrt(1-sinA0*sinA0);
            double zk2, zk4, zk6;
            double sigma;
            double A1, A2;
            double sigma1;
            //if (B1 == B2)
            //{
            //    dbLength = _earthA * Math.Cos(dbB1) / Math.Sqrt(1 - _earthE12 * Math.Sin(dbB1) * Math.Sin(dbB1)) * DL;
            //    dbAlp12 = DL >= 0 ? 90 : 270;
            //    dbAlp21 = DL >= 0 ? 270 : 90;
            //    return true;
            //}
            //当处于赤道时
            if (B1 == 0 && B2 == 0 && DL >= 0)
            {
                dbLength = _earthA * DL;
                dbAlp12 = 90;
                dbAlp21 = 270;
                return true;
            }
            else if ((B1 == 0 && B2 == 0 && DL < 0))
            {
                dbLength = _earthA * Math.Abs(DL);
                dbAlp12 = 270;
                dbAlp21 = 90;
                return true;
            }


            do
            {
                double lambda0 = lambda;
                double p = Math.Cos(U2) * Math.Sin(lambda0);
                double q = eb1 - eb2 * Math.Cos(lambda0);
                if (q == 0)
                    q = 1e-16;
                //计算方位角A1
                A1 = Math.Abs(Math.Atan(p / q));
                if (p > 0 && q > 0)
                    A1 = A1 + 0;
                else if (p > 0 && q < 0)
                    A1 = _dbPAI - A1;
                else if (p < 0 && q < 0)
                    A1 = _dbPAI + A1;
                else if (p < 0 && q > 0)
                    A1 = 2 * _dbPAI - A1;

                double Ssigma = p * Math.Sin(A1) + q * Math.Cos(A1);
                double Csigma = sa1 + sa2 * Math.Cos(lambda0);
                //sigma = Math.Abs(Math.Atan2(Ssigma, Csigma));
                sigma = Math.Abs(Math.Atan(Ssigma / Csigma));
                if (Csigma > 0)
                    sigma = sigma + 0;
                else if (Csigma < 0)
                    sigma = _dbPAI - sigma;
                //计算A0与sigma1
                sinA0 = Math.Cos(U1) * Math.Sin(A1);
                sigma1 = Math.Atan(Math.Tan(U1) / Math.Cos(A1));
                //计算椭球面经度差lambda
                cosA0 = Math.Sqrt(1 - sinA0 * sinA0);
                double e4 = _earthE12 * _earthE12;
                double e6 = e4 * _earthE12;
                zk2 = _earthE12 * cosA0 * cosA0;
                zk4 = zk2 * zk2;
                zk6 = zk4 * zk2;
                double alpha1 = (_earthE12 / 2 + e4 / 8 + e6 / 16) - _earthE22 * zk2 / 16 + 3 * zk4 * _earthE12 / 32;
                double beta1 = _earthE22 * zk2 / 16 - _earthE12 * zk4 / 32;
                double gamma1 = _earthE12 * zk4 / 256;
                double xx = alpha1 * sigma + beta1 * Math.Sin(sigma) * Math.Cos(2 * sigma1 + sigma);
                xx = xx + gamma1 * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma);
                lambda = DL + sinA0 * xx;
                Dlambda = Math.Abs(lambda - lambda0) * p0;//p0=206265

            } while (Dlambda > 1e-6);
            //计算椭球面距离S12
            cosA0 = Math.Sqrt(1 - sinA0 * sinA0);
            zk2 = _earthE22 * cosA0 * cosA0;
            zk4 = zk2 * zk2;
            zk6 = zk4 * zk2;
            double alpha = (1 - zk2 / 4 + 7 * zk4 / 64 - 15 * zk6 / 256) / _earthB;
            double beta = zk2 / 4 - zk4 / 8 + 37 * zk6 / 512;
            double gamma = zk4 / 128 - zk6 / 128;
            double ss12 = gamma * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma);
            double S12 = (sigma - beta * Math.Sin(sigma) * Math.Cos(2 * sigma1 + sigma) - ss12) / alpha;
            //计算椭球面方方位角A21
            double sinA2 = Math.Cos(U1) * Math.Sin(A1);
            double cosA2 = Math.Cos(U1) * Math.Cos(sigma) * Math.Cos(A1) - sigma * Math.Sin(sigma);
            double tanA2 = sinA2 / cosA2;
            A2 = Math.Abs(Math.Atan(sinA2 / cosA2));
            double sinA1 = Math.Sin(A1);
            if (sinA1 < 0 && tanA2 > 0)
                A2 = A2 + 0;
            else if (sinA1 < 0 && tanA2 < 0)
                A2 = _dbPAI - A2;
            else if (sinA1 > 0 && tanA2 > 0)
                A2 = _dbPAI + A2;
            else if (sinA1 > 0 && tanA2 < 0)
                A2 = 2 * _dbPAI - A2;
            //反向方位角=结束方位角+180
            //A2 = A2 + _dbPAI;
            if (A2 < 0)
            { A2 += 2 * _dbPAI; }
            if (A2 > 2 * _dbPAI)
            { A2 -= 2 * _dbPAI; }
            dbLength = S12;
            dbAlp12 = A1 * 180.0 / _dbPAI;
            dbAlp21 = A2 * 180.0 / _dbPAI;
            return true;
        }

    }
}
