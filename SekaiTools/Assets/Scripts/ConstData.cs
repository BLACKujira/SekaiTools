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
        public static Units units = new Units();
        public static readonly int layerLive2D = 8;
        public static readonly int layerBackGround = 9;
        public static readonly string defaultSpineAnimation = "pose_default";

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

                new CharacterInfo(32,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27)),
                new CharacterInfo(33,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27)),
                new CharacterInfo(34,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27)),
                new CharacterInfo(35,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27)),
                new CharacterInfo(36,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27)),

                new CharacterInfo(37,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27)),
                new CharacterInfo(38,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27)),
                new CharacterInfo(39,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27)),
                new CharacterInfo(40,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27)),
                new CharacterInfo(41,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27)),

                new CharacterInfo(42,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30)),
                new CharacterInfo(43,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30)),
                new CharacterInfo(44,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30)),
                new CharacterInfo(45,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30)),
                new CharacterInfo(46,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30)),

                new CharacterInfo(47,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5)),
                new CharacterInfo(48,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5)),
                new CharacterInfo(49,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5)),
                new CharacterInfo(50,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5)),
                new CharacterInfo(51,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5)),

                new CharacterInfo(52,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17)),
                new CharacterInfo(53,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17)),
                new CharacterInfo(54,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17)),
                new CharacterInfo(55,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17)),
                new CharacterInfo(56,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17)),

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

        public class Units
        {
            UnitInfo[] unitInfos =
            {
                null,
                new UnitInfo(1,@"バーチャル・シンガー",new Color32(0xFF,0xFF,0xFF,255)),
                new UnitInfo(2,@"Leo/need",new Color32(0x44,0x55,0xDD,255)),
                new UnitInfo(3,@"MORE MORE JUMP！",new Color32(0x88,0xDD,0x44,255)),
                new UnitInfo(4,@"Vivid BAD SQUAD",new Color32(0xEE,0x11,0x66,255)),
                new UnitInfo(5,@"ワンダーランズ×ショウタイム",new Color32(0xFF,0x99,0x00,255)),
                new UnitInfo(6,@"25時、ナイトコードで。",new Color32(0x88,0x44,0x99,255)),
            };

            public UnitInfo this[Unit unit]
            {
                get => unitInfos[(int)unit];
            }
            public UnitInfo this[int unitID]
            {
                get => unitInfos[unitID];
            }

        }
        public class UnitInfo
        {
            public readonly int id;
            public readonly string name;

            public readonly Color32 color;

            public UnitInfo(int id, string name, Color32 color)
            {
                this.id = id;
                this.name = name;
                this.color = color;
            }
        }
        public enum Unit
        {
            none,
            VirtualSinger,
            Leoneed,
            MOREMOREJUMP,
            VividBADSQUAD,
            WonderlandsShowtime,
            NightCord
        }

        public static int IsLive2DModelOfCharacter(string name)
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

        public static int IsSpineModelOfCharacter(string name,bool mergeVirtualSinger = false)
        {
            for (int i = 1; i < 27; i++)
            {
                string value = "sd_" + i.ToString("00") + ((Character)i).ToString();
                if (name.StartsWith(value))
                {
                    if(mergeVirtualSinger)
                        return i;
                    else
                    {
                        if (i >= 21 && i <= 26)
                        {
                            string[] array = name.Split('_');
                            if (array.Length < 3) return i;
                            else
                            {
                                int[] ids = SeparateVirtualSinger(i);
                                if (array[2].StartsWith("band")) return ids[1];
                                else if (array[2].StartsWith("idol")) return ids[2];
                                else if (array[2].StartsWith("street")) return ids[3];
                                else if (array[2].StartsWith("wonder")) return ids[4];
                                else if (array[2].StartsWith("night")) return ids[5];
                                else return i;
                            }
                        }
                        else
                            return i;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 将5个SEKAI的VS的ID合并到原始VS上
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int MergeVirtualSinger(int id)
        {
            if (id >= 27 && id <= 56)
            {
                return (id - 27) / 5 + 21;
            }
            else
                return id;
        }

        public static int[] SeparateVirtualSinger(int id)
        {
            if (id >= 21 && id <= 56)
            {
                id = MergeVirtualSinger(id);
                int[] vsIDs = new int[6];
                vsIDs[0] = id;
                int startID = (id - 21) * 5 + 27;
                for (int i = 0; i < 5; i++)
                {
                    vsIDs[i+1] = startID + i;
                }
                return vsIDs;
            }
            else
                return new int[] { id };
        }

        public static Unit InUnit(int id)
        {
            if (id <= 0) return Unit.none;
            else if (id <= 4) return Unit.Leoneed;
            else if (id <= 8) return Unit.MOREMOREJUMP;
            else if (id <= 12) return Unit.VividBADSQUAD;
            else if (id <= 16) return Unit.WonderlandsShowtime;
            else if (id <= 20) return Unit.NightCord;
            else if (id <= 26) return Unit.VirtualSinger;
            else if(id<=56)
            {
                switch ((id-27)%5)
                {
                    case 0:return Unit.Leoneed;
                    case 1:return Unit.MOREMOREJUMP;
                    case 2:return Unit.VividBADSQUAD;
                    case 3:return Unit.WonderlandsShowtime;
                    case 4:return Unit.NightCord;
                    default:return Unit.none;
                }
            }
            return Unit.none;
        }
    }
}
