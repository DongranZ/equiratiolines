using Microsoft.VisualStudio.TestTools.UnitTesting;
using OGIS.Algorithm;
using System;

namespace OGIS.UnitTest
{
    [TestClass]
    public class GeodeticSolution_KarneyGeodesicUnitTest
    {
        GeodeticSolution_KarneyGeodesic geodeticSolution;

        [TestMethod]
        public void SetParameterUnitTest()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesic();
                geodeticSolution.SetParameter(6378137.0, 0, 298.257223563);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [TestMethod]
        public void SetParameterTypeUnitTest()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesic();
                geodeticSolution.SetParameterType(0);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [TestMethod]
        public void OnFirstSubjectUnitest()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesic();
                geodeticSolution.SetParameterType(0);
                double B1, L1, B2, length12, L2, angle12, angle21;
                B1 = 30;
                L1 = 120;
                length12 = 10000;
                angle12 = 30;
                geodeticSolution.FirstSubject(L1, B1, angle12, length12, out L2, out B2, out angle21);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [TestMethod]
        public void OnSecondSubject()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesic();
                geodeticSolution.SetParameterType(0);
                double B1, L1, B2, length12, L2, angle12, angle21;
                B1 = 30;
                L1 = 10;
                B2 = 31;
                L2 = 10;
                geodeticSolution.SecondSubject(B1, L1, B2, L2, out length12, out angle12, out angle21);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }

    [TestClass]
    public class GeodeticSolution_KarneyGeodesicCppUnitTest
    {
        GeodeticSolution_KarneyGeodesicCpp geodeticSolution;

        [TestMethod]
        public void SetParameterUnitTest()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesicCpp();
                geodeticSolution.SetParameter(6378137.0, 0, 298.257223563);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [TestMethod]
        public void SetParameterTypeUnitTest()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesicCpp();
                geodeticSolution.SetParameterType(0);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [TestMethod]
        public void OnFirstSubjectUnitest()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesicCpp();
                geodeticSolution.SetParameterType(0);
                double B1, L1, B2, length12, L2, angle12, angle21;
                B1 = 30;
                L1 = 120;
                length12 = 10000;
                angle12 = 30;
                geodeticSolution.FirstSubject(L1, B1, angle12, length12, out L2, out B2, out angle21);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [TestMethod]
        public void OnSecondSubject()
        {
            try
            {
                geodeticSolution = new GeodeticSolution_KarneyGeodesicCpp();
                geodeticSolution.SetParameterType(0);
                double B1, L1, B2, length12, L2, angle12, angle21;
                B1 = 30;
                L1 = 120;
                B2 = 31;
                L2 = 121;
                geodeticSolution.SecondSubject(B1, L1, B2, L2, out length12, out angle12, out angle21);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
