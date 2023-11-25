using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OGIS.Algorithm
{
    [StructLayout(LayoutKind.Sequential)]
    public struct geod_geodesic
    {
        double a;                   /**< the equatorial radius */
        double f;                   /**< the flattening */
        /**< @cond SKIP */
        double f1, e2, ep2, n, b, c2, etol2;
        //double A3x[6], C3x[15], C4x[21];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.R8)]
        double[] A3x;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.R8)]
        double[] C3x;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.R8)]
        double[] C4x;
        /**< @endcond */
    };
    public class KarneyGeodesicInvoke
    {

        private const string _dllPath = @"\lib\geodc_d.dll";

        [DllImport(_dllPath, EntryPoint = "geod_init", CallingConvention = CallingConvention.Cdecl,CharSet =CharSet.Ansi)]
        public static extern void geod_init(ref geod_geodesic g, double a, double f);

        [DllImport(_dllPath, EntryPoint = "geod_direct", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void geod_direct(ref geod_geodesic g,double lat1, double lon1, double azi1, double s12
            ,ref double plat2, ref double plon2, ref double pazi2);

        [DllImport(_dllPath, EntryPoint = "geod_inverse", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void geod_inverse(ref geod_geodesic g, double lat1, double lon1,double lat2, double lon2
            , ref double ps12, ref double pazi1, ref double pazi2);
    }

    public class KarneyGeodesicInvokeCpp
    {
        private const string _dllPath = @"C:\E\richway\OGIS.Algorithm\OGIS.UnitTest\bin\Debug\lib\GeographicLib_d.dll";

        [DllImport(_dllPath, EntryPoint = @"?Init@GeodesicSingleton@GeographicLib@@QAEXNN_N@Z", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void Init(double a, double f,bool exact=false);

        [DllImport(_dllPath, EntryPoint = @"?Direct@GeodesicSingleton@GeographicLib@@QAENNNNNAAN00@Z", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        
        //[DllImport(_dllPath, EntryPoint = "Direct", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern float Direct(double lat1, double lon1, double azi1, double s12
            , ref double plat2, ref double plon2, ref double pazi2);

        [DllImport(_dllPath, EntryPoint = @"?Inverse@GeodesicSingleton@GeographicLib@@QAENNNNNAAN00@Z", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern float Inverse(double lat1, double lon1, double lat2, double lon2
            , ref double ps12, ref double pazi1, ref double pazi2);
    }
}
