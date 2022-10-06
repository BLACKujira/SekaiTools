using SekaiTools.DecompiledClass;
using SekaiTools.StringConverter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SekaiTools.UI.Radio
{
    public class Radio_MusicLayer : MonoBehaviour
    {
        public Radio radio;
        [Header("Components")]
        public AudioSource audioSource;
        public Text textMusicTitle;
        public Text textQueueLength;
        public Text textNextMusic;
        public Text textVocalSize;
        public Text textVocalSinger;
        public PerecntBar progressBar;
        [Header("Settings")]
        public int maxPlaylistLength = 10;

        public event Action<MusicInQueue> onMusicChange;
        public event Action<MusicInQueue> onAddMusic;

        [System.NonSerialized] public StringConverter_MusicName musicNames;
        [System.NonSerialized] public StringConverter_SingerName singerNames;
        [System.NonSerialized] public StringConverter_StringAlias cpNames;
        [System.NonSerialized] public StringConverter_StringAlias vocalTypeNames;
        [System.NonSerialized] public StringConverter_StringAlias sizeTypeNames;
        [System.NonSerialized] public StringConverter_UnitName unitNames;

        MasterMusic[] masterMusics;
        MasterMusicVocal[] masterMusicVocals;
        MasterOutsideCharacter[] masterOutsideCharacters;
        MasterMusicTag[] masterMusicTags;

        MusicData[] musicDatas;
        public MusicData[] MusicDatas => musicDatas;

        Queue<MusicInQueue> musicQueue = new Queue<MusicInQueue>();
        MusicInQueue randomMusic = null;
        List<int> unusedMusicSet = new List<int>();
        HashSet<int> bannedMusics;

        MusicInQueue nowPlaying = null;

        public MusicInQueue NowPlaying => nowPlaying;
        public WaitForMusicChanged waitForMusicChanged => new WaitForMusicChanged(this);
        public MusicInQueue[] PlayList => musicQueue.ToArray();

        /// <summary>
        /// 添加到事件onMusicChange，触发时停止等待
        /// </summary>
        public class WaitForMusicChanged : CustomYieldInstruction
        {
            Radio_MusicLayer musicLayer;
            bool _keepWaiting = true;

            public WaitForMusicChanged(Radio_MusicLayer musicLayer)
            {
                this.musicLayer = musicLayer;
                musicLayer.onMusicChange += OnMusicChange;
            }

            public override bool keepWaiting => _keepWaiting;

            void OnMusicChange(MusicInQueue musicInQueue)
            {
                _keepWaiting = false;
                musicLayer.onMusicChange -= OnMusicChange;
            }
        }

        #region 初始化

        public void Initialize(Settings settings)
        {
            musicNames = settings.musicNames;
            singerNames = settings.singerNames;
            cpNames = settings.cpNames;
            vocalTypeNames = settings.vocalTypeNames;
            sizeTypeNames = settings.sizeTypeNames;
            unitNames = settings.unitNames;

            masterMusics = settings.masterMusics;
            masterMusicVocals = settings.masterMusicVocals;
            masterOutsideCharacters = settings.masterOutsideCharacters;
            masterMusicTags = settings.masterMusicTags;

            bannedMusics = settings.bannedMusics;

            InitializeMusicData();

            if (!string.IsNullOrEmpty(settings.musicFolderSV))
                LoadMusicInSV(settings.musicFolderSV, settings.musicExtensions);
            if (!string.IsNullOrEmpty(settings.musicFolderOutside))
                LoadMusicInOutside(settings.musicFolderOutside, settings.musicExtensions);

            for (int i = 0; i < musicDatas.Length; i++)
            {
                MusicData musicData = musicDatas[i];
                if (musicData != null)
                    if (musicData.vocalDatas.Count == 0)
                        musicDatas[i] = null;
            }

            this.maxPlaylistLength = settings.maxPlaylistLength;
        }

        private void InitializeMusicData()
        {
            int maxMusicId = 0;
            foreach (var masterMusic in masterMusics)
            {
                if (masterMusic.id > maxMusicId)
                    maxMusicId = masterMusic.id;
            }
            musicDatas = new MusicData[maxMusicId + 1];

            List<MusicTag>[] musicTags = new List<MusicTag>[maxMusicId + 1];
            for (int i = 0; i < maxMusicId + 1; i++)
            {
                musicTags[i] = new List<MusicTag>();
            }

            foreach (var masterMusicTag in masterMusicTags)
            {
                if(masterMusicTag.musicId<maxMusicId)
                    musicTags[masterMusicTag.musicId].Add(masterMusicTag.MusicTag);
            }

            for (int i = 0; i < masterMusics.Length; i++)
            {
                musicDatas[masterMusics[i].id] = new MusicData(masterMusics[i], musicTags[masterMusics[i].id]);
            }
        }

        public void LoadMusicInSV(string folder,params string[] extensions)
        {
            foreach (var masterMusicVocal in masterMusicVocals)
            {
                foreach (var extension in extensions)
                {
                    string path = Path.Combine(folder, $"{masterMusicVocal.assetbundleName}_rip",masterMusicVocal.assetbundleName + extension);
                    if (File.Exists(path))
                    {
                        List<string> singers = new List<string>();
                        foreach (var character in masterMusicVocal.characters)
                        {
                            switch (character.CharacterType)
                            {
                                case CharacterType.game_character:
                                    singers.Add(ConstData.characters[character.characterId].Name);
                                    break;
                                case CharacterType.outside_character:
                                    singers.Add(masterOutsideCharacters[character.characterId-1].name);
                                    break;
                                case CharacterType.mob:
                                    singers.Add($"mob{character.characterId}");
                                    break;
                                default:
                                    break;
                            }
                        }
                        MusicVocalData vocalData = new MusicVocalData(path,7,
                            masterMusicVocal.MusicVocalType == MusicVocalType.original_song?
                            MusicVocalType.virtual_singer:masterMusicVocal.MusicVocalType,
                            "game",singers.ToArray());
                        musicDatas[masterMusicVocal.musicId].vocalDatas.Add(vocalData);
                        break;
                    }
                }
            }
        }

        public void LoadMusicInOutside(string folder, params string[] extensions)
        {
            string[] files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                foreach (var extension in extensions)
                {
                    if(Path.GetExtension(file).Equals(extension))
                    {
                        string metaFile = Path.ChangeExtension(file, ".musicmeta");
                        if(File.Exists(metaFile))
                        {
                            RadioMusicMeta radioMusicMeta = JsonUtility.FromJson<RadioMusicMeta>(File.ReadAllText(metaFile));
                            MusicVocalType vocalType = (MusicVocalType)Enum.Parse(typeof(MusicVocalType), radioMusicMeta.vocalType);
                            MusicVocalData musicVocalData = new MusicVocalData(file,
                                radioMusicMeta.offset,
                                vocalType==MusicVocalType.original_song?MusicVocalType.virtual_singer:vocalType,
                                radioMusicMeta.vocalSize,
                                radioMusicMeta.singers);
                            
                            musicDatas[radioMusicMeta.id].vocalDatas.Add(musicVocalData);
                        }
                        break;
                    }
                }
            }

            string[] subFolders = Directory.GetDirectories(folder);
            foreach (var subFolder in subFolders)
            {
                LoadMusicInOutside(subFolder, extensions);
            }
        }

        public class Settings
        {
            public StringConverter_MusicName musicNames;
            public StringConverter_SingerName singerNames;
            public StringConverter_StringAlias cpNames;
            public StringConverter_StringAlias vocalTypeNames;
            public StringConverter_StringAlias sizeTypeNames;
            public StringConverter_UnitName unitNames;

            public MasterMusic[] masterMusics;
            public MasterMusicVocal[] masterMusicVocals;
            public MasterOutsideCharacter[] masterOutsideCharacters;
            public MasterMusicTag[] masterMusicTags;

            /// <summary>
            /// SekaiViewer歌曲文件夹，留空以跳过加载
            /// </summary>
            public string musicFolderSV;
            /// <summary>
            /// 外部歌曲文件夹，留空以跳过加载
            /// </summary>
            public string musicFolderOutside;

            public string[] musicExtensions;
            public int maxPlaylistLength = 10;
            public HashSet<int> bannedMusics;
        }

        #endregion

        public MusicInQueue GetVocal(MusicOrderInfo musicOrderInfo)
        {
            int musicId = musicNames.GetValue(musicOrderInfo.musicName.Replace('_',' '));
            if (musicId == -1) return null;
            if (musicDatas[musicId] == null) return null;
            MusicVocalData vocalData = musicDatas[musicId].GetVocal(musicOrderInfo, this);
            if (vocalData != null)
                return new MusicInQueue(musicDatas[musicId], vocalData);
            else
                return null;
        }

        public void Play()
        {
            StartCoroutine(IPlay());
        }

        public IEnumerator IPlay()
        {
            SelectNextMusic();
            while (true)
            {
                MusicInQueue playMusic = SelectNextMusic();
                yield return playMusic;
                audioSource.clip = playMusic.audioClip;
                audioSource.Play();
                audioSource.time = playMusic.vocalData.startAt;
                if(NowPlaying!=null) Destroy(NowPlaying.audioClip);
                nowPlaying = playMusic;
                ShowMusicInfo(playMusic);
                onMusicChange(playMusic);
                RefreshPlaylist();
                yield return new WaitForSeconds(playMusic.audioClip.length + 1 - playMusic.vocalData.startAt);
            }
        }

        private void ShowMusicInfo(MusicInQueue playMusic)
        {
            textMusicTitle.text = $"正在播放 - {playMusic.musicData.masterMusic.composer}\n{playMusic.musicData.masterMusic.title}";

            if (playMusic.vocalData.singers.Length != 0)
            {
                List<string> singerStrs = new List<string>();
                foreach (var singer in playMusic.vocalData.singers)
                {
                    singerStrs.Add(singer.Replace(" ", string.Empty));
                }
                textVocalSinger.text = $"Vo. {string.Join("、", singerStrs)}";
            }
            else
            {
                textVocalSinger.text = "Instrument";
            }

            string sizeStr = playMusic.vocalData.musicSize;
            switch (sizeStr)
            {
                case "full":
                    sizeStr = "Full Version";
                    break;
                case "game":
                    sizeStr = "Game Size";
                    break;
                default:
                    break;
            }
            textVocalSize.text = sizeStr;
        }

        public MusicInQueue SelectNextMusic()
        {
            MusicInQueue selectedMusic = null;
            //如果播放列表为空
            if(musicQueue.Count==0)
            {
                if (unusedMusicSet.Count == 0)
                {
                    unusedMusicSet = new List<int>(from MusicData musicdata in musicDatas
                                                      where musicdata != null && musicdata.vocalDatas.Count > 0
                                                      select musicdata.id);
                    foreach (var musicId in bannedMusics)
                    {
                        unusedMusicSet.Remove(musicId);
                    }
                }

                selectedMusic = randomMusic;

                int rdmId = Random.Range(0, unusedMusicSet.Count);
                randomMusic = new MusicInQueue(musicDatas[unusedMusicSet[rdmId]]);
                unusedMusicSet.RemoveAt(rdmId);
                StartCoroutine(randomMusic.LoadMusic());
            }
            //如果播放列表不为空
            else
            {
                selectedMusic = musicQueue.Dequeue();
                if (unusedMusicSet.Contains(selectedMusic.musicData.id))
                    unusedMusicSet.Remove(selectedMusic.musicData.id);
                if(musicQueue.Count!=0)
                    StartCoroutine(musicQueue.Peek().LoadMusic());
            }

            return selectedMusic;
        }

        public void RefreshPlaylist()
        {
            string strQueueLength;
            string strNextMusic;

            if (musicQueue.Count == 0)
            {
                strQueueLength = "当前播放列表为空";
                strNextMusic = "下一首 - 随机歌曲";
            }
            else if (musicQueue.Count == maxPlaylistLength)
            {
                strQueueLength = $"当前播放列表长度为 {musicQueue.Count}，播放列表已满";
                strNextMusic = $"下一首 - {musicQueue.Peek().musicData.masterMusic.title}";
            }
            else
            {
                strQueueLength = $"当前播放列表长度为 {musicQueue.Count}";
                strNextMusic = $"下一首 - {musicQueue.Peek().musicData.masterMusic.title}";
            }

            textQueueLength.text = strQueueLength;
            textNextMusic.text = strNextMusic;
        }

        public class MusicInQueue : CustomYieldInstruction
        {
            public AudioClip audioClip;
            public MusicData musicData;
            public MusicVocalData vocalData;

            public bool _keepWaiting = true;

            public MusicInQueue(MusicData musicData)
            {
                this.musicData = musicData;
                this.vocalData = musicData.GetTopPriorityVocal();
            }

            public MusicInQueue(MusicData musicData, MusicVocalData vocalData) : this(musicData)
            {
                this.vocalData = vocalData;
            }

            public override bool keepWaiting => _keepWaiting;
            public IEnumerator LoadMusic()
            {
                AudioData audioData = new AudioData();
                yield return audioData.LoadFile(vocalData.filePath);
                audioClip = audioData.valueArray[0];
                
                _keepWaiting = false;
            }
        }

        public RadioMessage OrderMusic(MusicOrderInfo musicOrderInfo,string userName)
        {
            MusicInQueue musicInQueue = GetVocal(musicOrderInfo);
            if (musicInQueue == null||musicInQueue.vocalData==null)
                return new RadioMessage(userName, MessageType.error, "未找到符合条件的歌曲");
            if (bannedMusics.Contains(musicInQueue.musicData.id))
                return new RadioMessage(userName, MessageType.error, $"此电台不可播放 {musicInQueue.musicData.masterMusic.title}");
            MusicInQueue[] musicInArray = musicQueue.ToArray();
            foreach (var music in musicInArray)
            {
                if(music.musicData == musicInQueue.musicData)
                    return new RadioMessage(userName, MessageType.error, $"播放列表中已有 {musicInQueue.musicData.masterMusic.title}");
            }
            musicQueue.Enqueue(musicInQueue);
            if (musicQueue.Count == 1)
                StartCoroutine(musicQueue.Peek().LoadMusic());
            onAddMusic(musicInQueue);
            RefreshPlaylist();
            Debug.Log($"@{userName} 点歌成功 {musicInQueue.musicData.masterMusic.title}，Vo. {string.Join("、", musicInQueue.vocalData.singers)}");
            return new RadioMessage(userName, MessageType.success, $"点歌成功 {musicInQueue.musicData.masterMusic.title}");
        }

        private void Update()
        {
            if(audioSource&&audioSource.clip&&NowPlaying!=null)
            {
                progressBar.priority =
                    (audioSource.time - NowPlaying.vocalData.startAt)
                    / (audioSource.clip.length - NowPlaying.vocalData.startAt);
            }
            else
            {
                progressBar.priority = 0;
            }
        }
    }
}