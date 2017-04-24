using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWSServer
{
    public class JsonData
    {
        private string name = string.Empty;

        /// <summary>
        /// Json的Name字段
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private object _value = null;

        /// <summary>
        /// JSON的数据字段
        /// </summary>
        public object Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
    }
}
