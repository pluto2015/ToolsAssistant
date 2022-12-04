using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsAssistant
{
    public class EventHandlers
    {
        /// <summary>
        /// 收到数据
        /// </summary>
        /// <param name="ipPort"></param>
        /// <param name="data"></param>
        public delegate void DataRecievedEventHandler(string ipPort,string data);
        /// <summary>
        /// 连接及断开事件
        /// </summary>
        /// <param name="ipPort"></param>
        /// <param name="isConnect"></param>
        public delegate void ConnectEventHandler(string ipPort, bool isConnect);
    }
}
