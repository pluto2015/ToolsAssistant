using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ToolsAssistant.EventHandlers;

namespace ToolsAssistant.Services
{
    public interface IWebSocketServer
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
        /// 开启server
        /// </summary>
        /// <param name="port">端口号</param>
        void Start(int port);
        /// <summary>
        /// 停止server
        /// </summary>
        void Stop();
        /// <summary>
        /// 设置编码格式
        /// </summary>
        /// <param name="encordingType">编码类型</param>
        void SetEncording(EncordingType encordingType);
    }
}
