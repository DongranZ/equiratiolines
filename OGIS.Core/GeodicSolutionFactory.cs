using OGIS.Algorithm;
using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.Core
{
    public class GeodicSolutionFactory : IGeodicSolutionFactory
    {
        public IGeodeticSolution Create(string funcName)
        {
            IGeodeticSolution geodeticSolution;
           if (funcName == "bessel")/* */
            {
                geodeticSolution = new GeodeticSolution_Bessel();
                geodeticSolution.SetParameterType(0);
                //geodeticSolution.SetParameter(6378135.0,6356750.52,-1);
            }
           else if (funcName == "vincenty")
           {
               geodeticSolution = new GeodeticSolution_Vincenty();
               geodeticSolution.SetParameterType(0);
           }
           else
           {
               geodeticSolution = null;
           }
            return geodeticSolution;
        }
    }
}
