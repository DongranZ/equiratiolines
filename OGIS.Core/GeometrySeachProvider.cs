using OGIS.Algorithm;
using OGIS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.Core
{
    public class IGeometrySeachProvider
    {
        private static IGeometrySearch _instance;
        public static IGeometrySearch Instance { get { return _instance ?? new GeometrySeach(); } }
    }
}
