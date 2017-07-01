using System;
using System.Collections.Generic;
using System.Text;

namespace 标签打印_测试检测.Domain
{
    public class SamProject
    {
        /// <summary>
        /// 分组编号
        /// </summary>
        public string SequenceNumber { get; set; }
        /// <summary>
        /// 监测项目名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 添加的试剂
        /// </summary>
        public string Reagent { get; set; }
        /// <summary>
        /// 容器
        /// </summary>
        public string Container { get; set; }

        private int count = 1;
        /// <summary>
        /// 采样数量，默认为1
        /// </summary>
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        /// <summary>
        /// 保存方式
        /// </summary>
        public string SaveMethod { get; set; }
    }
}
