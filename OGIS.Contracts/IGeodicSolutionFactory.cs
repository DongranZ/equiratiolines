using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.Contracts
{
    public interface IGeodicSolutionFactory
    {
        IGeodeticSolution Create(string funcName);
    }
}
