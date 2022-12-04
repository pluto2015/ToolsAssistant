using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ToolsAssistant.EventHandlers;

namespace ToolsAssistant.Services
{
    public interface IWebSocketClient
    {
        /// <summary>
        /// 收到数据
        /// </summary>
        event DataRecievedEventHandler DataRecievedEvent;
        /// <summary>
        /// 连接及断开事件
        /// </summary>
        event ConnectEventHandler ConnectEvent;
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ipPort">ip：port</param>
        void Connect(string ipPort);
        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();
        /// <summary>
        /// 设置编码格式
        /// </summary>
        /// <param name="encordingType">编码类型</param>
        void SetEncording(EncordingType encordingType);
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        void SendData(string data);
    }
}
