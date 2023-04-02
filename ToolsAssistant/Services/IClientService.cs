using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsAssistant.Models;
using static ToolsAssistant.EventHandlers;

namespace ToolsAssistant.Services
{
    public interface IClientService
    {
        /// <summary>
        /// 类型
        /// </summary>
        ClientServerType ClientType { get; }
        /// <summary>
        /// 连接状态
        /// </summary>
        bool IsConnected { get; }
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
        /// <param name="timeOut">超时时间(s)</param>
        Task<bool> ConnectWithTimeoutAsync(string ipPort, int timeOut = 5);
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
