using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.UI.Radio;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioWelcome : MonoBehaviour
    {
        public InputField inputField_TipStayTime;

        public Radio_WelcomeLayer.Settings Settings
        {
            get
            {
                Radio_WelcomeLayer.Settings settings = new Radio_WelcomeLayer.Settings();
                settings.tipStayTime = float.Parse(inputField_TipStayTime.text);
                settings.tips = new List<string>()
                {
                    "发送弹幕 \"/点歌 歌曲名\" 点歌",
                    "如果参数中有空格，请删去空格或替换为下划线",
                    "点歌范围仅限游戏Project Sekai中的歌曲",
                    "发送弹幕 \"/歌曲列表\" 查看电台所有歌曲",
                    "也可以使用歌曲的译名、别名或ID点歌",
                    "在点歌指令之后输入角色名称，可以选择歌曲版本",
                    "示例 \"点歌 Forward an_khn\" 可以选择杏、こはね演唱的Forward"
                };
                return settings;
            }
        }
    }
}