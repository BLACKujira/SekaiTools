using SekaiTools.DecompiledClass;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.Radio;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.OutsideMusicMetadataGeneratorInitialize
{
    public class OutsideMusicMetadataGeneratorInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_OMMGFileInput gIP_OMMGFile;
        public GIP_OutsideMusicMetadata gIP_OutsideMusicMetadata;

        public void Apply()
        {
            List<string> log = new List<string>();

            MasterMusic[] masterMusics = EnvPath.GetTable<MasterMusic>("musics");
            MasterMusicVocal[] masterMusicVocals = EnvPath.GetTable<MasterMusicVocal>("musicVocals");
            MasterOutsideCharacter[] masterOutsideCharacters = EnvPath.GetTable<MasterOutsideCharacter>("outsideCharacters");

            string folder = gIP_OMMGFile.selectedFolder;
            List<string> extensionList = gIP_OMMGFile.extensionList;
            string vocalSize = gIP_OutsideMusicMetadata.vocalSize;
            MusicVocalType vocalType = gIP_OutsideMusicMetadata.musicVocalType;

            string[] files = Directory.GetFiles(folder);
            HashSet<string> fileHashSet = new HashSet<string>();
            HashSet<string> extensionHashSet = new HashSet<string>(extensionList);
            foreach (var file in files)
            {
                if (extensionHashSet.Contains(Path.GetExtension(file).ToLower()))
                    fileHashSet.Add(file);
            }

            foreach (var masterMusic in masterMusics)
            {
                foreach (var extension in extensionList)
                {
                    string filePath = Path.Combine(folder,masterMusic.title+extension);
                    if(File.Exists(filePath))
                    {
                        fileHashSet.Remove(filePath);

                        string metaFilePath = Path.ChangeExtension(filePath,".musicmeta");
                        RadioMusicMeta radioMusicMeta = new RadioMusicMeta();
                        radioMusicMeta.id = masterMusic.id;
                        radioMusicMeta.vocalType = vocalType.ToString();
                        radioMusicMeta.vocalSize = vocalSize;
                        radioMusicMeta.offset = 0;

                        MasterMusicVocal selectedVocal = null;
                        foreach (var masterMusicVocal in masterMusicVocals)
                        {
                            if (masterMusicVocal.musicId == masterMusic.id && masterMusicVocal.MusicVocalType == vocalType)
                            {
                                selectedVocal = masterMusicVocal;
                                break;
                            }
                        }

                        if (selectedVocal != null)
                        {
                            List<string> singers = new List<string>();
                            foreach (var character in selectedVocal.characters)
                            {
                                switch (character.CharacterType)
                                {
                                    case CharacterType.game_character:
                                        singers.Add(ConstData.characters[character.characterId].Name);
                                        break;
                                    case CharacterType.outside_character:
                                        singers.Add(masterOutsideCharacters[character.characterId - 1].name);
                                        break;
                                    case CharacterType.mob:
                                        singers.Add($"mob{character.characterId}");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            radioMusicMeta.singers = singers.ToArray();
                        }
                        else
                        {
                            log.Add($"{masterMusic.title} 未找到演唱者信息");
                            break;
                        }
                        File.WriteAllText(metaFilePath, JsonUtility.ToJson(radioMusicMeta,true));
                    }
                }
            }

            if(fileHashSet.Count>0)
            {
                foreach (var item in fileHashSet)
                {
                    log.Add($"{item} 未找到歌曲信息");
                }
            }

            if (log.Count > 0)
                WindowController.ShowLog("生成错误消息", string.Join("\n", log));
            else
                WindowController.ShowMessage("消息", "生成成功");
        }
    }
}