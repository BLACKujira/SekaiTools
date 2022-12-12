using SekaiTools.DecompiledClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools
{
    public enum Unit
    {
        none,
        VirtualSinger,
        Leoneed,
        MOREMOREJUMP,
        VividBADSQUAD,
        WonderlandsXShowtime,
        NightCord
    }

    public enum Character
    {
        none,
        ichika, saki, honami, shiho,
        minori, haruka, airi, shizuku,
        kohane, an, akito, touya,
        tsukasa, emu, nene, rui,
        kanade, mafuyu, ena, mizuki,
        miku, rin, len, luka, meiko, kaito
    }

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
        public static string saveDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games","SekaiTools");

        public class Characters
        {
            CharacterInfo[] characterInfos =
            {
                null,
                new CharacterInfo(1,"星乃","一歌",new Color32(0x33,0xAA,0xEE,255),(8,11),Unit.Leoneed),
                new CharacterInfo(2,"天馬","咲希",new Color32(0xFF,0xDD,0x44,255),(5,9),Unit.Leoneed),
                new CharacterInfo(3,"望月","穂波",new Color32(0xEE,0x66,0x66,255),(10,27),Unit.Leoneed),
                new CharacterInfo(4,"日野森","志歩",new Color32(0xBB,0xDD,0x22,255),(1,8),Unit.Leoneed),

                new CharacterInfo(5,"花里","みのり",new Color32(0xFF,0xCC,0xAA,255),(4,14),Unit.MOREMOREJUMP),
                new CharacterInfo(6,"桐谷","遥",new Color32(0x99,0xCC,0xFF,255),(10,5),Unit.MOREMOREJUMP),
                new CharacterInfo(7,"桃井","愛莉",new Color32(0xFF,0xAA,0xCC,255),(3,19),Unit.MOREMOREJUMP),
                new CharacterInfo(8,"日野森","雫",new Color32(0x99,0xEE,0xDD,255),(12,6),Unit.MOREMOREJUMP),

                new CharacterInfo(9,"小豆沢","こはね",new Color32(0xFF,0x66,0x99,255),(3,2),Unit.VividBADSQUAD),
                new CharacterInfo(10,"白石","杏",new Color32(0x00,0xBB,0xDD,255),(7,26),Unit.VividBADSQUAD),
                new CharacterInfo(11,"東雲","彰人",new Color32(0xFF,0x77,0x22,255),(11,12),Unit.VividBADSQUAD),
                new CharacterInfo(12,"青柳","冬弥",new Color32(0x00,0x77,0xDD,255),(5,25),Unit.VividBADSQUAD),

                new CharacterInfo(13,"天馬","司",new Color32(0xFF,0xBB,0x00,255),(5,17),Unit.WonderlandsXShowtime),
                new CharacterInfo(14,"鳳","えむ",new Color32(0xFF,0x66,0xBB,255),(9,9),Unit.WonderlandsXShowtime),
                new CharacterInfo(15,"草薙","寧々",new Color32(0x33,0xDD,0x99,255),(7,20),Unit.WonderlandsXShowtime),
                new CharacterInfo(16,"神代","類",new Color32(0xBB,0x88,0xEE,255),(6,24),Unit.WonderlandsXShowtime),

                new CharacterInfo(17,"宵崎","奏",new Color32(0xBB,0x66,0x88,255),(2,10),Unit.NightCord),
                new CharacterInfo(18,"朝比奈","まふゆ",new Color32(0x88,0x88,0xCC,255),(1,27),Unit.NightCord),
                new CharacterInfo(19,"東雲","絵名",new Color32(0xCC,0xAA,0x88,255),(4,30),Unit.NightCord),
                new CharacterInfo(20,"暁山","瑞希",new Color32(0xDD,0xAA,0xCC,255),(8,27),Unit.NightCord),

                new CharacterInfo(21,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.VirtualSinger),
                new CharacterInfo(22,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.VirtualSinger),
                new CharacterInfo(23,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.VirtualSinger),
                new CharacterInfo(24,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.VirtualSinger),
                new CharacterInfo(25,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.VirtualSinger),
                new CharacterInfo(26,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.VirtualSinger),

                new CharacterInfo(27,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.Leoneed),
                new CharacterInfo(28,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.MOREMOREJUMP),
                new CharacterInfo(29,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.VividBADSQUAD),
                new CharacterInfo(30,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.WonderlandsXShowtime),
                new CharacterInfo(31,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.NightCord),

                new CharacterInfo(32,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.Leoneed),
                new CharacterInfo(33,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.MOREMOREJUMP),
                new CharacterInfo(34,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.VividBADSQUAD),
                new CharacterInfo(35,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.WonderlandsXShowtime),
                new CharacterInfo(36,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.NightCord),

                new CharacterInfo(37,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.Leoneed),
                new CharacterInfo(38,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.MOREMOREJUMP),
                new CharacterInfo(39,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.VividBADSQUAD),
                new CharacterInfo(40,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.WonderlandsXShowtime),
                new CharacterInfo(41,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.NightCord),

                new CharacterInfo(42,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.Leoneed),
                new CharacterInfo(43,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.MOREMOREJUMP),
                new CharacterInfo(44,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.VividBADSQUAD),
                new CharacterInfo(45,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.WonderlandsXShowtime),
                new CharacterInfo(46,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.NightCord),

                new CharacterInfo(47,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.Leoneed),
                new CharacterInfo(48,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.MOREMOREJUMP),
                new CharacterInfo(49,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.VividBADSQUAD),
                new CharacterInfo(50,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.WonderlandsXShowtime),
                new CharacterInfo(51,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.NightCord),

                new CharacterInfo(52,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.Leoneed),
                new CharacterInfo(53,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.MOREMOREJUMP),
                new CharacterInfo(54,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.VividBADSQUAD),
                new CharacterInfo(55,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.WonderlandsXShowtime),
                new CharacterInfo(56,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.NightCord),

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
            public readonly Unit unit;

            public string Name { get => string.IsNullOrEmpty(myouji) ? namae : myouji + ' ' + namae; }

            public CharacterInfo(int id, string myouji, string namae, Color imageColor, (int month, int day) birthday,Unit unit)
            {
                this.id = id;
                this.myouji = myouji;
                this.namae = namae;
                this.imageColor = imageColor;
                this.birthday = birthday;
                this.unit = unit;
            }
        }

        public class Units
        {
            UnitInfo[] unitInfos =
            {
                null,
                new UnitInfo(1,@"バーチャル・シンガー",new Color32(0xFF,0xFF,0xFF,255),"vs"),
                new UnitInfo(2,@"Leo/need",new Color32(0x44,0x55,0xDD,255),"l/n"),
                new UnitInfo(3,@"MORE MORE JUMP！",new Color32(0x88,0xDD,0x44,255),"mmj"),
                new UnitInfo(4,@"Vivid BAD SQUAD",new Color32(0xEE,0x11,0x66,255),"vbs"),
                new UnitInfo(5,@"ワンダーランズ×ショウタイム",new Color32(0xFF,0x99,0x00,255),"wxs"),
                new UnitInfo(6,@"25時、ナイトコードで。",new Color32(0x88,0x44,0x99,255),"25时"),
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
            public readonly string abbr;

            public UnitInfo(int id, string name, Color32 color,string abbr)
            {
                this.id = id;
                this.name = name;
                this.color = color;
                this.abbr = abbr;
            }
        }

        public static int IsLive2DModelOfCharacter(string name, bool mergeVirtualSinger = true)
        {
            for (int i = 1; i < 27; i++)
            {
                string value = i.ToString("00")+((Character)i).ToString();
                if (name.StartsWith(value))
                {
                    if (mergeVirtualSinger)
                        return i;
                    else
                    {
                        if (i >= 21 && i <= 26)
                        {
                            string[] array = name.Split('_');
                            if (array.Length < 2) return i;
                            else
                            {
                                int[] ids = SeparateVirtualSinger(i);
                                if (array[1].StartsWith("band")) return ids[1];
                                else if (array[1].StartsWith("idol")) return ids[2];
                                else if (array[1].StartsWith("street")) return ids[3];
                                else if (array[1].StartsWith("wonder")) return ids[4];
                                else if (array[1].StartsWith("night")) return ids[5];
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
        public static EventStoryInfo IsEventStory(string name)
        {
            string[] nameArray = name.Split('_');
            if (nameArray.Length < 3) return null;
            if (!nameArray[0].Equals("event")) return null;
            if (nameArray[1].Length != 2) return null;
            if (nameArray[2].Length != 2) return null;
            int eventId, chapter;
            if (!int.TryParse(nameArray[1], out eventId)) return null;
            if (!int.TryParse(nameArray[2], out chapter)) return null;

            return new EventStoryInfo(eventId, chapter);
        }
        public static CardStoryInfo IsCardStory(string name)
        {
            string[] nameArray = name.Split('_');
            if (nameArray.Length < 2) return null;
            if (nameArray[0].Length != 6) return null;
            if (!int.TryParse(nameArray[0].Substring(0, 3), out int charId)) return null;
            if (!int.TryParse(nameArray[0].Substring(3, 3), out int cardId)) return null;
            if (nameArray[1].Length != characterNames_Roman[charId].Length + 2) return null;
            if (!nameArray[1].StartsWith(characterNames_Roman[charId])) return null;
            if (!int.TryParse(nameArray[1].Substring(characterNames_Roman[charId].Length), out int chapter)) return null;
            return new CardStoryInfo(charId, cardId, chapter);
        }
        public static UnitStoryInfo IsUnitStory(string name)
        {
            string[] nameArray = name.Split('_');
            if (nameArray.Length != 3) return null;
            HashSet<string> unitSet = new HashSet<string>(unitStoryTypes);
            if (!unitSet.Contains(nameArray[0])) return null;
            if (!int.TryParse(nameArray[1], out int season)) return null;
            if (!int.TryParse(nameArray[2], out int chapter)) return null;
            return new UnitStoryInfo(nameArray[0], season, chapter);
        }

        public static readonly string[] characterNames_Roman =
        {
            null,
            "ichika","saki","honami","shiho",
            "minori","haruka","airi","shizuku",
            "kohane","an","akito","touya",
            "tsukasa","emu","nene","rui",
            "kanade","mafuyu","ena","muzuki",
            "miku","rin","len","luka","meiko","kaito"
        };
        public static readonly string[] unitStoryTypes = 
        { 
            "leo","mmj","street","wonder","nightcode",
            "vs","vsleo","vsmmj","vsstreet","vswonder","vsnightcode"
        };

        public static int GetUnitVirtualSinger(int virtualSingerID,Unit unit)
        {
            if (virtualSingerID >= 21 && virtualSingerID <= 26)
            {
                int offset = 0;
                switch (unit)
                {
                    case Unit.none:
                        return virtualSingerID;
                    case Unit.VirtualSinger:
                        return virtualSingerID;
                    case Unit.Leoneed:
                        offset = 0;
                        break;
                    case Unit.MOREMOREJUMP:
                        offset = 1;
                        break;
                    case Unit.VividBADSQUAD:
                        offset = 2;
                        break;
                    case Unit.WonderlandsXShowtime:
                        offset = 3;
                        break;
                    case Unit.NightCord:
                        offset = 4;
                        break;
                    default:
                        break;
                }

                int startID = (virtualSingerID - 21) * 5 + 27;

                int vsID = startID + offset;

                return vsID;
            }
            else
                return virtualSingerID;
        }
        public static int GetUnitVirtualSinger(int virtualSingerID, UnitType unitType) => GetUnitVirtualSinger(virtualSingerID, unitType.ToUnit());

        /// <summary>
        /// 将5个SEKAI的VS的ID合并到原始VS上,不适用于Scenario
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

        /// <summary>
        /// 判断ScenarioSnippetTalk的说话角色
        /// </summary>
        /// <param name="scenarioSnippetTalk"></param>
        /// <returns></returns>
        public static int GetCharacterId_Scenario(ScenarioSnippetTalk scenarioSnippetTalk)
        {
            int characterId = 0;
            if (scenarioSnippetTalk.TalkCharacters.Length <= 0
                || scenarioSnippetTalk.TalkCharacters[0].Character2dId == 0
                || scenarioSnippetTalk.TalkCharacters[0].Character2dId > 31)
                characterId = NamaeToId(scenarioSnippetTalk.WindowDisplayName);
            else if (scenarioSnippetTalk.TalkCharacters[0].Character2dId <= 20)
                characterId = scenarioSnippetTalk.TalkCharacters[0].Character2dId;
            else if (scenarioSnippetTalk.TalkCharacters[0].Character2dId <= 26)
                characterId = 21;
            else
                characterId = scenarioSnippetTalk.TalkCharacters[0].Character2dId - 5;
            return characterId;
        }

        public static Unit InUnit(int id)
        {
            if (id <= 0) return Unit.none;
            else if (id <= 4) return Unit.Leoneed;
            else if (id <= 8) return Unit.MOREMOREJUMP;
            else if (id <= 12) return Unit.VividBADSQUAD;
            else if (id <= 16) return Unit.WonderlandsXShowtime;
            else if (id <= 20) return Unit.NightCord;
            else if (id <= 26) return Unit.VirtualSinger;
            else if(id<=56)
            {
                switch ((id-27)%5)
                {
                    case 0:return Unit.Leoneed;
                    case 1:return Unit.MOREMOREJUMP;
                    case 2:return Unit.VividBADSQUAD;
                    case 3:return Unit.WonderlandsXShowtime;
                    case 4:return Unit.NightCord;
                    default:return Unit.none;
                }
            }
            return Unit.none;
        }

        public static int NamaeToId(string namae)
        {
            for (int i = 1; i < 27; i++)
            {
                if (namae.Equals(characters[i].namae))
                    return i;
            }
            return -1;
        }

        public static int NameToId(string name)
        {
            for (int i = 1; i < 27; i++)
            {
                if (name.Equals(characters[i].Name))
                    return i;
            }
            return -1;
        }

        public static bool MusicTagEqualsUnitType(MusicTag musicTag,UnitType unitType)
        {
            if (
                (musicTag == MusicTag.light_music_club && unitType == UnitType.light_sound) ||
                (musicTag == MusicTag.idol && unitType == UnitType.idol) ||
                (musicTag == MusicTag.street && unitType == UnitType.street) ||
                (musicTag == MusicTag.theme_park && unitType == UnitType.theme_park) ||
                (musicTag == MusicTag.school_refusal && unitType == UnitType.school_refusal) ||
                (musicTag == MusicTag.vocaloid && unitType == UnitType.piapro)
                )
                return true;
            return false;
        }

        public static Unit ToUnit(this UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.none:
                    return Unit.none;
                case UnitType.piapro:
                    return Unit.VirtualSinger;
                case UnitType.theme_park:
                    return Unit.WonderlandsXShowtime;
                case UnitType.idol:
                    return Unit.MOREMOREJUMP;
                case UnitType.street:
                    return Unit.VividBADSQUAD;
                case UnitType.light_sound:
                    return Unit.Leoneed;
                case UnitType.school_refusal:
                    return Unit.NightCord;
                case UnitType.any:
                    return Unit.none;
                default:
                    return Unit.none;
            }
        }

        public static UnitType ToUnitType(int unitId)
        {
            switch (unitId)
            {
                case 1:return UnitType.piapro;
                case 2:return UnitType.light_sound;
                case 3:return UnitType.idol;
                case 4:return UnitType.street;
                case 5:return UnitType.theme_park;
                case 6:return UnitType.school_refusal;
                default: return UnitType.none;
            }
        }

    }


    [Serializable]
    public class EventStoryInfo
    {
        public int eventId;
        public int chapter;

        public EventStoryInfo(int eventId, int chapter)
        {
            this.eventId = eventId;
            this.chapter = chapter;
        }
    }

    [Serializable]
    public class CardStoryInfo
    {
        public int charId;
        public int cardId;
        public int chapter;

        public CardStoryInfo(int charId, int cardId, int chapter)
        {
            this.charId = charId;
            this.cardId = cardId;
            this.chapter = chapter;
        }
    }

    [Serializable]
    public class UnitStoryInfo
    {
        public string unit;
        public int season;
        public int chapter;

        public UnitStoryInfo(string unit, int season, int chapter)
        {
            this.unit = unit;
            this.season = season;
            this.chapter = chapter;
        }
    }

    public enum StoryType
    { UnitStory, EventStory, CardStory, MapTalk, LiveTalk, OtherStory }

}
