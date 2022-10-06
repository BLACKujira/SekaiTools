using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Lib
{
    /// <summary>
    /// 消息分发器接口
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// 不需要手动调用.只需要实现方法
        ///
        /// 
        ///{
        ///case "DANMU_MSG":
        ///await messageHandler.DanmuMessageHandlerAsync(DanmuMessage.JsonToDanmuMessage(message));
        ///}
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageHandler"></param>
        /// <returns></returns>
        Task DispatchAsync(JObject message,IMessageHandler messageHandler);
    }
}