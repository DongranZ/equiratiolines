using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace OGIS.Algorithm
{
    [Export(typeof(IGeodeticSolution))]
    public class GeodeticSolution_KarneyGeodesic : AGeodeticSolution
    {
        private geod_geodesic g=new geod_geodesic();
        protected override bool OnFirstSubject(double L1, double B1, double dbAlp12, double dbLength, out double L2, out double B2, out double dbAlp21)
        {
            try
            {
                L2 = 0;
                B2 = 0;
                dbAlp21 = 0;
                KarneyGeodesicInvoke.geod_init(ref g,base.A,1.0/base._earthAlpha);
                KarneyGeodesicInvoke.geod_direct(ref g,B1,L1,dbAlp12,dbLength,ref B2,ref L2,ref dbAlp21);
                if (dbAlp21 > 360)
                {
                    dbAlp21 -= 360;
                }

                if (dbAlp21 < 0)
                {
                    dbAlp21 += 360;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        protected override bool OnSecondSubject(double L1, double B1, double L2, double B2, out double dbLength, out double dbAlp12, out double dbAlp21)
        {
            dbLength = 0;
            dbAlp12 = 0;
            dbAlp21 = 0;
            try
            {
                KarneyGeodesicInvoke.geod_init(ref g, base.A, 1.0 / base._earthAlpha);
                KarneyGeodesicInvoke.geod_inverse(ref g, B1, L1, B2, L2, ref dbLength, ref dbAlp12, ref dbAlp21);
                if (dbAlp21 > 360)
                {
                    dbAlp21 -= 360;
                }

                if (dbAlp21 < 0)
                {
                    dbAlp21 += 360;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

    }

    [Export(typeof(IGeodeticSolution))]
    public class GeodeticSolution_KarneyGeodesicCpp : AGeodeticSolution
    {
        protected override bool OnFirstSubject(double L1, double B1, double dbAlp12, double dbLength, out double L2, out double B2, out double dbAlp21)
        {
            try
            {
                L2 = 0;
                B2 = 0;
                dbAlp21 = 0;
                KarneyGeodesicInvokeCpp.Init(base.A, base._earthAlpha,false);
                KarneyGeodesicInvokeCpp.Direct( B1, L1, dbAlp12, dbLength, ref B2, ref L2, ref dbAlp21);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        protected override bool OnSecondSubject(double L1, double B1, double L2, double B2, out double dbLength, out double dbAlp12, out double dbAlp21)
        {
            dbLength = 0;
            dbAlp12 = 0;
            dbAlp21 = 0;
            try
            {
                KarneyGeodesicInvokeCpp.Init(base.A, base._earthAlpha);
                KarneyGeodesicInvokeCpp.Inverse(B1, L1, B2, L2, ref dbLength, ref dbAlp12, ref dbAlp21);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }
}

