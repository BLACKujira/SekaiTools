using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BilibiliUtilities.Live.Message;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Lib
{
    public class MessageDispatcher : IMessageDispatcher
    {
        public async Task DispatchAsync(JObject message, IMessageHandler messageHandler)
        {
            try
            {
                switch (message["cmd"].ToString())
                {
                    case "DANMU_MSG":
                        await messageHandler.DanmuMessageHandlerAsync(DanmuMessage.JsonToDanmuMessage(message));
                        break;
                    case "SEND_GIFT":
                        await messageHandler.GiftMessageHandlerAsync(GiftMessage.JsonToGiftMessage(message));
                        break;
                    case "GUARD_MSG": // TODO 上舰信息未处理(第一次购买)
                        Debug.WriteLine("上舰信息");
                        Debug.WriteLine(message);
                        break;
                    case "GUARD_BUY":
                        await messageHandler.GuardBuyMessageHandlerAsync(GuardBuyMessage.JsonToGuardBuyMessage(message));
                        break;
                    case "USER_TOAST_MSG":
                        await messageHandler.UserToastMessageHandlerAsync(UserToastMessage.JsonToUserToastMessage(message));
                        break;
                    case "GUARD_LOTTERY_START": // TODO 上舰抽奖通知
                        Debug.WriteLine("购买舰长后出现的抽奖");
                        Debug.WriteLine(message);
                        break;
                    case "NOTICE_MSG": // TODO 通知信息未处理
                        Debug.WriteLine(message);
                        Debug.WriteLine(message);
                        break;
                    case "WELCOME":
                        await messageHandler.WelcomeMessageHandlerAsync(WelcomeMessage.JsonToWelcomeMessage(message));
                        break;
                    case "SYS_MSG": // TODO 系统消息未处理
                        Debug.WriteLine("SYS_MSG");
                        Debug.WriteLine(message);                        
                        break;
                    case "COMBO_END":
                        await messageHandler.ComboEndMessageHandlerAsync(ComboEndMessage.JsonToComboEndMessage(message));
                        break;
                    case "SUPER_CHAT_MESSAGE": // TODO 醒目留言信息未处理
                        break;
                    case "ROOM_REAL_TIME_MESSAGE_UPDATE":
                        await messageHandler.RoomUpdateMessageHandlerAsync(RoomUpdateMessage.JsonToRoomUpdateMessage(message));
                        break;
                    case "SUPER_CHAT_MESSAGE_JPN": // TODO 另一种醒目留言信息未处理
                        break;
                    case "WELCOME_GUARD":
                        await messageHandler.WelcomeGuardMessageHandlerAsync(WelcomeGuardMessage.JsonToWelcomeGuardMessage(message));
                        break;
                    case "ROOM_RANK": // TODO 房间排行信息未处理
                        break;
                    case "ENTRY_EFFECT": // TODO 貌似是舰长的进入信息
                        await messageHandler.EntryEffectMessageHandlerAsync(EntryEffectMessage.JsonToEntryEffectMessage(message));
                        break;
                    case "COMBO_SEND": // TODO COMBO_SEND
                        await messageHandler.ComboEndMessageHandlerAsync(ComboEndMessage.JsonToComboEndMessage(message));
                        break;
                    case "ANCHOR_LOT_START": // TODO 天选时刻
                        Debug.WriteLine("ANCHOR_LOT_START");
                        Debug.WriteLine(message);                        
                        break;
                    case "ACTIVITY_BANNER_UPDATE_V2":
                        Debug.WriteLine("ACTIVITY_BANNER_UPDATE_V2");
                        Debug.WriteLine(message);                        
                        break;
                    case "ROOM_CHANGE": //
                        Debug.WriteLine("ROOM_CHANGE");
                        Debug.WriteLine(message);                        
                        break;
                    case "WEEK_STAR_CLOCK":
                        Debug.WriteLine("WEEK_STAR_CLOCK");
                        Debug.WriteLine(message);                        
                        break;
                    case "LIVE":
                        await messageHandler.LiveStartMessageHandlerAsync(int.Parse(message["roomid"].ToString()));
                        break;
                    case "PREPARING":
                        await messageHandler.LiveStopMessageHandlerAsync(int.Parse(message["roomid"].ToString()));
                        break;
                    case "INTERACT_WORD":
                        await messageHandler.InteractWordMessageHandlerAsync(InteractWordMessage.JsonToInteractWordMessage(message));
                        break;
                    case "ONLINERANK": //更新排行榜信息
                        Debug.WriteLine("ONLINERANK");
                        Debug.WriteLine(message);                        
                        break;
                    case "PANEL": //更新分区排行等信息
                        Debug.WriteLine("PANEL");
                        Debug.WriteLine(message);                        
                        break;
                    case "ROOM_BANNER":
                        Debug.WriteLine("ROOM_BANNER");
                        Debug.WriteLine(message);
                        break;
                    default:
                        Debug.WriteLine("未记录的信息");
                        Debug.WriteLine(message);
                        break;
                }
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}