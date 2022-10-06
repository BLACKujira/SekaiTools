using BilibiliUtilities.Live.Lib;
using BilibiliUtilities.Live.Message;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    internal class RadioCommandinput_BilibiliUtilitiesMuti_MessageHandler : IMessageHandler
    {
        public RadioCommandinput_BilibiliUtilitiesMutiBase radioCommandinput_BilibiliUtilitiesMuti;
        int id;

        public RadioCommandinput_BilibiliUtilitiesMuti_MessageHandler(RadioCommandinput_BilibiliUtilitiesMutiBase radioCommandinput_BilibiliUtilitiesMuti, int id)
        {
            this.radioCommandinput_BilibiliUtilitiesMuti = radioCommandinput_BilibiliUtilitiesMuti;
            this.id = id;
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
            radioCommandinput_BilibiliUtilitiesMuti.ReceiveDanmaku(
                new RadioCommandinput_BilibiliUtilitiesMuti.Danmaku(danmuMessage.Username, danmuMessage.Content), id);

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
