using ESRI.ArcGIS.Geometry;
using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public interface IRatioPoint
    {
        void SetGeodeticSolutionType(IGeodeticSolution iGeodeticSolution);
        IPoint FindPointBy2Point(IPoint fromPoint, IPoint toPoint, bool isOnline, double ratio, out double ratiolength);
    }
}
