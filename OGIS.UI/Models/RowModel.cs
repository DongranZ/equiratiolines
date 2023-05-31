using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class RowModel
    {
        public int index { get; set; }
        public string CalculateType { get; set; }
        public double PLon { get; set; }
        public double PLat { get; set; }
        public double QLon { get; set; }
        public double QLat { get; set; }
        public double Length { get; set; }
        public double OLon { get; set; }
        public double OLat { get; set; }
        public double OPLength { get; set; }
        public double OPError { get; set; }
        public double OQLength { get; set; }
        public double OQError { get; set; }
        public double OPQError { get; set; }
    }
}
