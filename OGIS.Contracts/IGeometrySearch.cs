using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.Contracts
{
    public interface IGeometrySearch
    {
        bool SearchPointRatio2Point(double[][] coordinateArray, double radius, double ratio, double helperDirection, double errorRange
          , IGeodeticSolution geodetic, out double[,] resultCoord, out double distanceTA, out double distanceTB);
    }
}
