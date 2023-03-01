using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Cutin;
using System.IO;

namespace SekaiTools.Kizuna
{
    public abstract class KizunaSceneDataBase :ISaveData
    {
        public string cutinPlayerType = "default";
        public abstract KizunaSceneBase[] kizunaSceneBaseArray { get; }

        public KizunaSceneBase this[Vector2Int vector2Int]
        {
            get
            {
                foreach (var kizunaScene in kizunaSceneBaseArray)
                {
                    if ((vector2Int.x == kizunaScene.charAID && vector2Int.y == kizunaScene.charBID)
                        || ((vector2Int.x == kizunaScene.charBID && vector2Int.y == kizunaScene.charAID)))
                        return kizunaScene;
                }
                return null;
            }
        }

        public string SavePath { get; set; }

        public CutinSceneData CutinSceneData
        {
            get
            {
                List<CutinScene> cutinScenes = new List<CutinScene>();
                foreach (var kizunaSceneBase in kizunaSceneBaseArray)
                {
                    cutinScenes.AddRange(kizunaSceneBase.cutinScenes);
                }

                CutinSceneData cutinSceneData = new CutinSceneData(cutinScenes);
                cutinSceneData.SavePath = Path.ChangeExtension(SavePath, ".csd");
                return cutinSceneData;
            }
        }

        public SerializedAudioData StandardizeAudioData(SerializedAudioData serializedAudioData)
        {
            SerializedAudioData newSerializedAudioData = new SerializedAudioData();
            foreach (var item in serializedAudioData.items)
            {
                CutinSceneData.CutinSceneInfo cutinSceneInfo = CutinSceneData.IsCutinVoice(item.name);
                if (cutinSceneInfo!=null)
                    foreach (var kizunaScene in kizunaSceneBaseArray)
                    {
                        if(kizunaScene.IsKizunaOf(cutinSceneInfo.charFirstID,cutinSceneInfo.charSecondID))
                        {
                            newSerializedAudioData.items.Add(new SerializedAudioData.DataItem(CutinSceneData.StandardizeName(cutinSceneInfo), item.path));
                            break;
                        }
                    }
            }
            return newSerializedAudioData;
        }

        public void StandardizeAudioData(AudioData audioData)
        {
            AudioClip[] valueArray = audioData.ValueArray;
            foreach (var value in valueArray)
            {
                CutinSceneData.CutinSceneInfo cutinSceneInfo = CutinSceneData.IsCutinVoice(value.name);
                if (cutinSceneInfo != null)
                {
                    string savePath = audioData.GetSavePath(value);
                    audioData.RemoveValue(value);
                    foreach (var kizunaScene in kizunaSceneBaseArray)
                    {
                        if (kizunaScene.IsKizunaOf(cutinSceneInfo.charFirstID, cutinSceneInfo.charSecondID))
                        {
                            value.name = CutinSceneData.StandardizeName(cutinSceneInfo);
                            audioData.AppendValue(value, savePath);
                            break;
                        }
                    }
                }
                else
                {
                    audioData.RemoveValue(value);
                }
            }
        }

        public void SaveData()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(SavePath, json);
        }

        /// <summary>
        /// 统计语音匹配与缺失
        /// </summary>
        /// <param name="audioData"></param>
        /// <returns></returns>
        public CutinSceneData.AudioMatchingCount CountMatching(AudioData audioData)
        {
            CutinSceneData.AudioMatchingCount audioMatchingCount = new CutinSceneData.AudioMatchingCount();
            foreach (var kizunaScene in kizunaSceneBaseArray)
            {
                audioMatchingCount += new CutinSceneData(kizunaScene.cutinScenes).CountMatching(audioData);
            }
            return audioMatchingCount;
        }

        public int[] AppearCharacters
        {
            get
            {
                HashSet<int> appearCharacters = new HashSet<int>();
                foreach (var kizunaScene in kizunaSceneBaseArray)
                {
                    appearCharacters.Add(kizunaScene.charAID);
                    appearCharacters.Add(kizunaScene.charBID);
                }

                List<int> list = new List<int>(appearCharacters);
                list.Sort();
                return list.ToArray();
            }
        }

        public void AddCutinScenes(CutinSceneData cutinSceneData)
        {
            foreach (var kizunaSceneBase in kizunaSceneBaseArray)
            {
                kizunaSceneBase.AddCutinScenes(cutinSceneData);
            }
        }

        public KizunaSceneDataBase LoadData(string savePath)
        {
            throw new System.NotImplementedException();
        }
    }

    public class KizunaSceneData : KizunaSceneDataBase
    {
        public List<KizunaScene> kizunaScenes = new List<KizunaScene>();

        public override KizunaSceneBase[] kizunaSceneBaseArray => kizunaScenes.ToArray();

        public KizunaScene this[Vector2Int vector2Int]
        {
            get
            {
                foreach (var kizunaScene in kizunaScenes)
                {
                    if ((vector2Int.x == kizunaScene.charAID && vector2Int.y == kizunaScene.charBID)
                        || ((vector2Int.x == kizunaScene.charBID && vector2Int.y == kizunaScene.charAID)))
                        return (KizunaScene)kizunaScene;
                }
                return null;
            }
        }

        public KizunaSceneData(Vector2Int[] bonds)
        {
            foreach (var bond in bonds)
            {
                KizunaScene kizunaScene = new KizunaScene(bond.x, bond.y);
                int x = ConstData.MergeVirtualSinger(bond.x);
                int y = ConstData.MergeVirtualSinger(bond.y);
                kizunaScene.textSpriteLv1 = new BondsHonorWordInfo(x, y, 1, 1).StandardizedName;
                kizunaScene.textSpriteLv2 = new BondsHonorWordInfo(x, y, 2, 1).StandardizedName;
                kizunaScene.textSpriteLv3 = new BondsHonorWordInfo(x, y, 3, 1).StandardizedName;
                kizunaScenes.Add(kizunaScene);
            }
        }

        public class ImageMatchingCount
        {
            public int matching = 0;
            public int missing = 0;

            public int CountAll { get => matching + missing; }
            public string Log
            {
                get
                {
                    List<string> logs = new List<string>();
                    foreach (var countCell in countCells)
                    {
                        if (!countCell.ifComplete)
                        {
                            List<string> missingFile = new List<string>();
                            if (!countCell.ifHasLv1) missingFile.Add("Level1");
                            if (!countCell.ifHasLv2) missingFile.Add("Level2");
                            if (!countCell.ifHasLv3) missingFile.Add("Level3");
                            string log = $"{countCell.charAID.ToString("00")}{countCell.charBID.ToString("00")} {ConstData.characters[countCell.charAID].namae} ♥ {ConstData.characters[countCell.charBID].namae} 缺少 {string.Join(" ", missingFile)}";
                            logs.Add(log);
                        }
                    }
                    return logs.Count == 0 ? "无缺失" : string.Join("\n", logs);
                }
            }
            public readonly CountCell[] countCells;

            public class CountCell
            {
                public readonly int charAID;
                public readonly int charBID;

                public bool ifHasLv1 = false;
                public bool ifHasLv2 = false;
                public bool ifHasLv3 = false;

                public bool ifComplete { get => ifHasLv1 && ifHasLv2 && ifHasLv3; }

                public CountCell(int charAID, int charBID)
                {
                    this.charAID = charAID;
                    this.charBID = charBID;
                }
            }
            public void UpdateInfo(BondsHonorWordInfo bondsHonorWordInfo)
            {
                foreach (var countCell in countCells)
                {
                    if (bondsHonorWordInfo.IsKizunaOf(countCell.charAID, countCell.charBID) && bondsHonorWordInfo.style == 1)
                    {
                        switch (bondsHonorWordInfo.level)
                        {
                            case 1: countCell.ifHasLv1 = true; break;
                            case 2: countCell.ifHasLv2 = true; break;
                            case 3: countCell.ifHasLv3 = true; break;
                            default:
                                break;
                        }
                    }
                }

                matching = 0; missing = 0;
                foreach (var countCell in countCells)
                {
                    if (countCell.ifComplete) matching++;
                    else missing++;
                }
            }

            public ImageMatchingCount(KizunaScene[] kizunaScenes)
            {
                List<CountCell> countCells = new List<CountCell>();
                foreach (var kizunaScene in kizunaScenes)
                {
                    countCells.Add(new CountCell(kizunaScene.charAID, kizunaScene.charBID));
                }
                this.countCells = countCells.ToArray();
            }
        }

        public ImageMatchingCount CountImageMatching(ImageData imageData)
        {
            ImageMatchingCount imageMatchingCount = new ImageMatchingCount(kizunaScenes.ToArray());
            Sprite[] valueArray = imageData.ValueArray;
            foreach (var value in valueArray)
            {
                BondsHonorWordInfo bondsHonorWordInfo = BondsHonorWordInfo.IsBondsHonorWord(value.name);
                if (bondsHonorWordInfo != null) imageMatchingCount.UpdateInfo(bondsHonorWordInfo);
            }
            return imageMatchingCount;
        }

        public static KizunaSceneData LoadData(string serializedData)
        {
            return JsonUtility.FromJson<KizunaSceneData>(serializedData);
        }
    }

    public class CustomKizunaData : KizunaSceneDataBase
    {
        public List<KizunaSceneCustom> kizunaScenes = new List<KizunaSceneCustom>();
        public override KizunaSceneBase[] kizunaSceneBaseArray => kizunaScenes.ToArray();

        public CustomKizunaData(Vector2Int[] bonds)
        {
            foreach (var bond in bonds)
            {
                KizunaSceneCustom kizunaScene = new KizunaSceneCustom(bond.x, bond.y);
                kizunaScenes.Add(kizunaScene);
            }
        }

        public static CustomKizunaData LoadData(string serializedData)
        {
            return JsonUtility.FromJson<CustomKizunaData>(serializedData);
        }
    }


    public class BondsHonorWordInfo
    {
        static readonly string honorname = "honorname";

        public int charAID;
        public int charBID;
        public int level;
        public int style;

        public string StandardizedName
        {
            get
            {
                return $"{honorname}_{charAID.ToString("00")}{charBID.ToString("00")}_{level.ToString("00")}_{style.ToString("00")}";
            }
        }

        public BondsHonorWordInfo(int charAID, int charBID, int level, int style)
        {
            this.charAID = charAID;
            this.charBID = charBID;
            this.level = level;
            this.style = style;
        }

        public static BondsHonorWordInfo IsBondsHonorWord(string fileName)
        {
            if (fileName.Length < 20) return null;
            string[] nameArray = fileName.Substring(0, 20).Split('_');
            if (nameArray.Length != 4) return null;
            if (!nameArray[0].Equals(honorname)) return null;
            if (nameArray[1].Length != 4) return null;
            int charAID, charBID,level,style;
            if (!int.TryParse(nameArray[1].Substring(0, 2), out charAID)) return null;
            if (!int.TryParse(nameArray[1].Substring(2, 2), out charBID)) return null;
            if (!int.TryParse(nameArray[2], out level)) return null;
            if (!int.TryParse(nameArray[3], out style)) return null;

            return new BondsHonorWordInfo(charAID, charBID, level, style);            
        }

        public bool IsKizunaOf(int charAID,int charBID)
        {
            if (this.charAID == charAID && this.charBID == charBID) return true;
            if (this.charBID == charAID && this.charAID == charBID) return true;
            return false;
        }
    }
}