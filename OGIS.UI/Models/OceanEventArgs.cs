using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class OceanEventArgs : EventArgs
    {
        public object obj { get; set; }

        public AOceanEntity aOceanEntity { get; set; }
    }
}
