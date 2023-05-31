using ESRI.ArcGIS.esriSystem;
/********************************************************************************
 ****创建目的：  
 ****创 建 人：  李洋
 ****创建时间：  2018-**-**
 ****修 改 人：
 ****修改时间：
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gis.App_Start
{
    public static class UI_Init
    {
        private static LicenseInitializer m_AOLicenseInitializer = new LicenseInitializer();

        public static void Inits()
        {
            Init_RuntimeManager();

            Init_License();
        }
        public static void Init_RuntimeManager()
        {
            if(ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop)
                || ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine))
            {
                System.Console.WriteLine("应用程序未能加载正确的ArcGIS版本.");
                return;
            }
            //if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine))
            //{
            //    if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop))
            //    {
            //        System.Console.WriteLine("This application could not load the correct version of ArcGIS.");
            //        return;
            //    }
            //}
        }
        public static void Init_License()
        {
            m_AOLicenseInitializer = new LicenseInitializer();
            //ESRI License Initializer generated code.
            if (!m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeAdvanced, esriLicenseProductCode.esriLicenseProductCodeStandard, esriLicenseProductCode.esriLicenseProductCodeBasic },
            new esriLicenseExtensionCode[] { esriLicenseExtensionCode.esriLicenseExtensionCode3DAnalyst, esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork, esriLicenseExtensionCode.esriLicenseExtensionCodeSchematics, esriLicenseExtensionCode.esriLicenseExtensionCodeArcScan, esriLicenseExtensionCode.esriLicenseExtensionCodeBusiness, esriLicenseExtensionCode.esriLicenseExtensionCodeMLE, esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst, esriLicenseExtensionCode.esriLicenseExtensionCodeCOGO, esriLicenseExtensionCode.esriLicenseExtensionCodeGeoStats, esriLicenseExtensionCode.esriLicenseExtensionCodePublisher, esriLicenseExtensionCode.esriLicenseExtensionCodeDataInteroperability, esriLicenseExtensionCode.esriLicenseExtensionCodeTracking }))
            {
                System.Console.WriteLine(m_AOLicenseInitializer.LicenseMessage());
                System.Console.WriteLine("This application could not initialize with the correct ArcGIS license and will shutdown.");
                m_AOLicenseInitializer.ShutdownApplication();
                return;
            }
        }
        public static void Stop()
        {
            m_AOLicenseInitializer.ShutdownApplication();
        }
    }
}
