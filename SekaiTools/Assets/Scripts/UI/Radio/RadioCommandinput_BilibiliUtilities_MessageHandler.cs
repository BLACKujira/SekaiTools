using BilibiliUtilities.Live.Lib;
using BilibiliUtilities.Live.Message;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class RadioCommandinput_BilibiliUtilities_MessageHandler : IMessageHandler
    {
        public Radio radio;

        public RadioCommandinput_BilibiliUtilities_MessageHandler(Radio radio)
        {
            this.radio = radio;
        }

        public Task AudiencesHandlerAsync(int audiences)
        {
            return null;
        }

        public Task ComboEndMessageHandlerAsync(ComboEndMessage comboEndMessage)
        {
            return null;
        }

        public Task DanmuMessageHandlerAsync(DanmuMessage danmuMessage)
        {
            if (danmuMessage.Content.StartsWith("/"))
            {
                radio.ProcessRequest(danmuMessage.Content, danmuMessage.Username);
            }

            return null;
        }

        public Task EntryEffectMessageHandlerAsync(EntryEffectMessage entryEffectMessage)
        {
            return null;
        }

        public Task GiftMessageHandlerAsync(GiftMessage giftMessage)
        {
            return null;
        }

        public Task GuardBuyMessageHandlerAsync(GuardBuyMessage guardBuyMessage)
        {
            return null;
        }

        public Task InteractWordMessageHandlerAsync(InteractWordMessage message)
        {
            return null;
        }

        public Task LiveStartMessageHandlerAsync(int roomId)
        {
            return null;
        }

        public Task LiveStopMessageHandlerAsync(int roomId)
        {
            return null;
        }

        public Task NoticeMessageHandlerAsync(NoticeMessage noticeMessage)
        {
            return null;
        }

        public Task RoomUpdateMessageHandlerAsync(RoomUpdateMessage roomUpdateMessage)
        {
            return null;
        }

        public Task UserToastMessageHandlerAsync(UserToastMessage userToastMessage)
        {
            return null;
        }

        public Task WelcomeGuardMessageHandlerAsync(WelcomeGuardMessage welcomeGuardMessage)
        {
            return null;
        }

        public Task WelcomeMessageHandlerAsync(WelcomeMessage welcomeMessage)
        {
            return null;
        }
    }
}