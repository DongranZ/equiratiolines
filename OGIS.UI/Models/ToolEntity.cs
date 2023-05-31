using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public class ToolEntity : AOceanEntity
    {
        //private int _optionType = 3;
        public ToolEntity()
        {
            EntityType = EntityType.Tool;
        }
        /// <summary>
        /// 是否清空地图
        /// </summary>
        private bool _isClearMap = false;

        public CurrentOperate ToolOption { get; set; }
        public string OptionExplain { get; set; }
        public string FuncIntroduce { get; set; }
        public int OptionType { get; set; }
        public bool IsClearMap { get { return _isClearMap; } set { _isClearMap = value; } }

    }
}
