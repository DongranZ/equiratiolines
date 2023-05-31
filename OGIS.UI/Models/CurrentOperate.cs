using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public enum CurrentOperate
    {
        OptionMeaningless = -1,
        //工具栏按钮
        OptionNull = 0,     //没有使用工具
        MapPan = 1,            //漫游
        MapFull = 2,           //全图
        MapZoomIn = 3,         //放大
        MapZoomOut = 4,        //缩小

        ElementSelect = 5,      //选择图元
        ElementDelet = 6,      //删除图元
        MapClear = 7,          //清图

        MapLengthMessure = 8,  //长度量算
        MapAreaMessure = 9,    //面积量算
        MapAngleMessure = 10,  //方位量算

        MapVector = 11,        //打开矢量图
        MapImage = 12,         //打开影像图 
        MapDEM = 13,           //打开地形图 
        DataInput = 14,        //数据录入
        DBBake = 15,           //数据备份 
        DBRecover = 16,        //数据还原     

        SpatialQuery = 17,     //空间查询            

        MXDOpen = 18,          //打开地图文档      
        MXDSaveAs = 19,        //另存地图文档
        MapAddLayer = 20,         //添加图层

        MapLabelSelect = 1021,//选择标注
        MapLabelAdd = 21,      //添加标注
        MapLabelMove = 22,     //移动标注
        MapLabelEdit = 1023,
        MapLabelRotate = 23,   //旋转标注
        MapProjectSet = 24,    //坐标系设置
        MapReCoordSys = 25,    //恢复坐标系
        MapPageLayout = 26,    //版面设计
        MapPrintRect = 27,       //矩形打印
        MapPrintCircle = 28,     //圆形打印
        MapPrintPolygon = 29,    //多边形打印

        CaseLocal = 30,        //事件坐标定位
        CaseMapLocal = 31,     //事件地图定位             

        HJLoadData = 32,         //加载数据
        HJDrawLine = 34,       //划线
        HJDrawPolygon = 35,    //划面
        HJSelectDrawLine = 36, //选择线
        HJSelectALine = 236,      //选择A方划界线
        HJSelectBLine = 237,      //选择B方划界线
        HJMidLine = 37,        //中间线 
        HJSenior = 38,         //高级划界       

        QuanwenMapInfor = 39,   //地图信息
        QuanwenHJXX = 40,       //划界信息
        QuanwenWenxian = 41,    //文献信息      
        QuanwenNHDJ = 42,       //南海岛礁
        QuanwenLaw = 43,        //法规       

        HelpAboutSystem = 44,   //关于系统
        HelpOnline = 45,        //在线帮助    
        HelpSysExit = 46,       //退出系统

        PageZoomIn = 51,        //布局放大
        PageZoomOut = 52,       //布局缩小
        PagePan = 53,           //布局漫游
        MapOutlineSet = 54,     //图框设置
        MapSelect = 50,         //要素选择
        MapTxtSet = 55,         //文本设置
        MapRuleSet = 56,        //比例尺设置
        MapNorthArrowSet = 57,  //指南针设置
        MapSampleSet = 58,      //图例设置
        MapPrintSet = 59,       //打印设置
        MapExport = 60,         //地图导出
        MapPrint = 61,           //地图打印
        MapAmendText = 62,      //修改文本
        MapTLXG = 63,             //图例修改

        SelectFromWorkSpace = 70, //从工作空间选择要素
        SelectFromLayer = 71, //从图层选择要素

        SectionFootPoint = 80,  //获取剖面，确认坡脚点
        DCSElementSelect = 81, //坡脚线选择
        DCSSediment = 82, //1%沉积物
        ContourLine = 83,//等深线
        Contour100NmBufferLine = 84,//等深线
        QueryBaseLine = 85,//从图层查询领海基线
        FormularLineSelect = 86, //公式线选择
        LimitLineSelect = 87, //限制线选择
        FusionLineSelect = 88, //融合线选择

        DrawDirectionLineOnPoint = 89,
        DrawDirectionLineOnLine = 300,
        DrawCoordLineOnPoint = 90,
        DrawCircleOnPoint = 91,

        ConnectPoint = 92,
        LineParallel = 93,
        LineDirection = 94,

        LineTransfer = 95,
        LineConnect = 96,
        PloygonConnect = 97,
        PointToLine = 98, //点转线
        LineToPolygon = 99,//
        PloygonToLine = 100,
        LineToPoint = 101,

        DrawPoint = 102,
        DrawLine = 103,
        DrawCircle = 104,
        DrawPolygon = 105,//

        GeometryEdit = 106,//折点编辑
        GetElevationTerrain = 107,//获取高程地形

        Distinguish = 108,//识别要素

        LineExtend = 109,//线段延长线
        GeometryBuffer = 110,//图形缓冲分析
        GeometryCrossPoint = 111,//交点分析
        InsertPointsOnline = 112,//线内插点
        GetCenterBy3Point = 113,//三点共圆

        GetMidnormalLine = 114,//垂直平分线
        GetAngularBisectorLine = 115,//角平分线
        GetTwoLineAngularBisectorLine = 123,//两线角平分线
        DrawArcBy2Ptn = 116,//过两点画弧
        MultiDraw = 117,//右键连续画图
        DrawArcByCenter = 118,//过两点画弧
        DrawArcBy3Pt = 119,

        CalculateDisPtToLine = 120,//点到线距离

        PointList = 121,
        MapElementSelect = 122,      //选择图元

        ArcEnvole = 124,//弧形缓冲
        TwoPointRatioPoint = 125,//两点求比例点

        LineMeasureByDraw = 130,
        LineMeasureBySelect = 131,
        PickupPoint = 132,

        CoastLine = 133,
        SingleBuffer = 134,//单边划界
    }
}
