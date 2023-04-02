using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsAssistant.Models;
using static ToolsAssistant.EventHandlers;

namespace ToolsAssistant.Services
{
    public interface IServerService
    {
        /// <summary>
        /// 类型
        /// </summary>
        ClientServerType ServerType { get; }
        #region 事件
        /// <summary>
        /// 收到数据
        /// </summary>
        event DataRecievedEventHandler DataRecieved;
        /// <summary>
        /// 连接及断开事件
        /// </summary>
        event ConnectEventHandler ClientConnected;
        /// <summary>
        /// 服务停止
        /// </summary>
        event EventHandler ServerStoped;
        #endregion
        /// <summary>
        /// 开启server
        /// </summary>
        /// <param name="url">ip:port</param>
        void Start(string url);
        /// <summary>
        /// 停止server
        /// </summary>
        void Stop();
        /// <summary>
        /// 设置编码格式
        /// </summary>
        /// <param name="encordingType">编码类型</param>
        void SetEncording(EncordingType encordingType);
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ipPort">ip:port</param>
        /// <param name="data">字符串数据</param>
        void SendData(string ipPort, string data);
    }
}
