using System;
using System.Collections.Generic;
using System.Text;

namespace 标签打印_测试检测.Domain
{
    class PrintObject
    {
        private string _reagent;
        private string _container;
        private string _name;

        /// <summary>
        /// 采样编号
        /// </summary>
        public string SamCode { get; set; }

        /// <summary>
        /// 检测项目
        /// </summary>
        public string Name
        {
            get { return _name == null ? "" : _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 添加的试剂
        /// </summary>
        public string Reagent
        {
            get { return _reagent == null ? "" : _reagent; }
            set { _reagent = value; }
        }

        /// <summary>
        /// 容器
        /// </summary>
        public string Container
        {
            get { return _container == null ? "" : _container; }
            set { _container = value; }
        }

        /// <summary>
        /// 采样时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 采样公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 二维码内容
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 二维码内容
        /// </summary>
        public string SaveMethod { get; set; }
    }
}
