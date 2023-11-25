using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.Contracts
{
    public interface IGeodeticSolution
    {
        double A { get; }
        void SetParameterType(int type);
        /// <summary>
        /// 设置椭球参数
        /// </summary>
        /// <param name="a">地球椭球长轴</param>
        /// <param name="b">地球椭球短轴</param>
        /// <param name="alpha_inverse">扁率倒数</param>
        void SetParameter(double a, double b, double alpha_inverse);
        /// <summary>
        /// 大地第一主题解算
        /// </summary>
        /// <param name="L1">起始点经度</param>
        /// <param name="B1">起始点纬度</param>
        /// <param name="dbAlp12">正方位角</param>
        /// <param name="dbLength">距离</param>
        /// <param name="L2">结果点经度</param>
        /// <param name="B2">结果点纬度</param>
        /// <param name="dbAlp21">反向方位角</param>
        /// <returns>是否计算成功</returns>
        bool FirstSubject(double L1, double B1, double dbAlp12, double dbLength, out double L2, out double B2, out double dbAlp21);
        /// <summary>
        /// 大地第二主题解算
        /// </summary>
        /// <param name="L1">起始点经度</param>
        /// <param name="B1">起始点纬度</param>
        /// <param name="L2">终止点经度</param>
        /// <param name="B2">终止点纬度</param>
        /// <param name="dbLength">距离</param>
        /// <param name="dbAlp12">正向方位角</param>
        /// <param name="dbAlp21">反向方位角</param>
        /// <returns>是否计算成功</returns>
        bool SecondSubject(double L1, double B1, double L2, double B2, out double dbLength, out double dbAlp12, out double dbAlp21);


    }
}
