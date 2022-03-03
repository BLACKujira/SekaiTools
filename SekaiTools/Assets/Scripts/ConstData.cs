using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    /// <summary>
    /// 用于保存固定的资料
    /// </summary>
    public static class ConstData
    {
        public static readonly string WebRequestLocalFileHead = @"file:///";
        public static Characters characters = new Characters();
        public static readonly int layerLive2D = 8;
        public static readonly int layerBackGround = 9;

        public class Characters
        {
            CharacterInfo[] characterInfos =
            {
                null,
                new CharacterInfo(1,"星乃","一歌",new Color32(0x33,0xAA,0xEE,255),(8,11)),
                new CharacterInfo(2,"天馬","咲希",new Color32(0xFF,0xDD,0x44,255),(5,9)),
                new CharacterInfo(3,"望月","穂波",new Color32(0xEE,0x66,0x66,255),(10,27)),
                new CharacterInfo(4,"日野森","志歩",new Color32(0xBB,0xDD,0x22,255),(1,8)),

                new CharacterInfo(5,"花里","みのり",new Color32(0xFF,0xCC,0xAA,255),(4,14)),
                new CharacterInfo(6,"桐谷","遥",new Color32(0x99,0xCC,0xFF,255),(10,5)),
                new CharacterInfo(7,"桃井","愛莉",new Color32(0xFF,0xAA,0xCC,255),(3,19)),
                new CharacterInfo(8,"日野森","雫",new Color32(0x99,0xEE,0xDD,255),(12,6)),

                new CharacterInfo(9,"小豆沢","こはね",new Color32(0xFF,0x66,0x99,255),(3,2)),
                new CharacterInfo(10,"白石","杏",new Color32(0x00,0xBB,0xDD,255),(7,26)),
                new CharacterInfo(11,"東雲","彰人",new Color32(0xFF,0x77,0x22,255),(11,12)),
                new CharacterInfo(12,"青柳","冬弥",new Color32(0x00,0x77,0xDD,255),(5,25)),

                new CharacterInfo(13,"天馬","司",new Color32(0xFF,0xBB,0x00,255),(5,17)),
                new CharacterInfo(14,"鳳","えむ",new Color32(0xFF,0x66,0xBB,255),(9,9)),
                new CharacterInfo(15,"草薙","寧々",new Color32(0x33,0xDD,0x99,255),(7,20)),
                new CharacterInfo(16,"神代","類",new Color32(0xBB,0x88,0xEE,255),(6,24)),

                new CharacterInfo(17,"宵崎","奏",new Color32(0xBB,0x66,0x88,255),(2,10)),
                new CharacterInfo(18,"朝比奈","まふゆ",new Color32(0x88,0x88,0xCC,255),(1,27)),
                new CharacterInfo(19,"東雲","絵名",new Color32(0xCC,0xAA,0x88,255),(4,30)),
                new CharacterInfo(20,"暁山","瑞希",new Color32(0xDD,0xAA,0xCC,255),(8,27)),

                new CharacterInfo(21,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27)),
                new CharacterInfo(22,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27)),
                new CharacterInfo(23,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27)),
                new CharacterInfo(24,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30)),
                new CharacterInfo(25,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5)),
                new CharacterInfo(26,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17)),

                new CharacterInfo(27,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27)),
                new CharacterInfo(28,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27)),
                new CharacterInfo(29,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27)),
                new CharacterInfo(30,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27)),
                new CharacterInfo(31,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27)),
            };

            public CharacterInfo this[Character character]
            {
                get => characterInfos[(int)character];
            }
            public CharacterInfo this[int characterID]
            {
                get => characterInfos[characterID];
            }
        }
        public class CharacterInfo
        {
            public readonly int id;
            public readonly string myouji;
            public readonly string namae;
            public readonly Color32 imageColor;
            public readonly (int month, int day) birthday;

            public string Name { get => myouji + ' ' + namae; }

            public CharacterInfo(int id, string myouji, string namae, Color imageColor, (int month, int day) birthday)
            {
                this.id = id;
                this.myouji = myouji;
                this.namae = namae;
                this.imageColor = imageColor;
                this.birthday = birthday;
            }
        }
        public enum Character
        {
            none,
            ichika,saki,honami,shiho,
            minori,haruka,airi,shizuku,
            kohane,an,akito,touya,
            tsukasa,emu,nene,rui,
            kanade,mafuyu,ena,mizuki,
            miku,rin,len,luka,meiko,kaito
        }
        public enum Unit
        {
            none,
            VirtualSinger,
            Leoneed,
            MOREMOREJUMP,
            VividBADSQUAD,
            WonderlandsShowtime,
            _25jiNightCordde
        }

        public static int IsModelOfCharacter(string name)
        {
            for (int i = 1; i < 27; i++)
            {
                string value = i.ToString("00")+((Character)i).ToString();
                if (name.StartsWith(value))
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
