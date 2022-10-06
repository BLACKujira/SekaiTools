using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class Radio_PlayListLayer : MonoBehaviour, IReturnableWindow
    {
        public Radio radio;
        [Header("Components")]
        public RadioReturnableWindowController returnableWindowController;
        public UniversalGeneratorV2 universalGenerator;
        public AutoScroll autoScroll;
        [Header("Settings")]
        public int voFontSize = 23;
        [Header("Prefab")]
        public ItemWithTitleAndContent itemPrefab;

        public ReturnPermission ReturnPermission => returnableWindowController.ReturnPermission;
        public string SenderUserName { get; set; }

        public void Initialize()
        {
            radio.musicLayer.onAddMusic += OnAddMusic;
        }

        public void Play(string userName, Action onComplete)
        {
            SenderUserName = userName;
            RefreshDisplay();
            StartCoroutine(IPlay(onComplete));
            returnableWindowController.ResetReturnPermission();
        }

        IEnumerator IPlay(Action onComplete)
        {
            yield return 1;
            yield return autoScroll.IPlay(onComplete);
        }

        public void RefreshDisplay()
        {
            universalGenerator.ClearItems();
            Radio_MusicLayer.MusicInQueue[] playList = radio.musicLayer.PlayList;
            for (int i = 0; i < playList.Length; i++)
            {
                Radio_MusicLayer.MusicInQueue musicInQueue = playList[i];
                AddItem(i, musicInQueue);
            }
        }

        private void AddItem(int i, Radio_MusicLayer.MusicInQueue musicInQueue)
        {
            universalGenerator.AddItem(itemPrefab.gameObject, (gobj) =>
            {
                ItemWithTitleAndContent itemWithTitleAndContent = gobj.GetComponent<ItemWithTitleAndContent>();
                DecompiledClass.MasterMusic masterMusic = musicInQueue.musicData.masterMusic;
                string titleStr = $"{(i + 1).ToString("00")} {masterMusic.title}";
                if (musicInQueue.vocalData.singers.Length != 0)
                {
                    List<string> singerStrs = new List<string>();
                    foreach (var singer in musicInQueue.vocalData.singers)
                    {
                        singerStrs.Add(singer.Replace(" ", string.Empty));
                    }
                    titleStr += $" Vo. {string.Join("、", singerStrs)}";
                }
                itemWithTitleAndContent.text_Title.text = titleStr;
                itemWithTitleAndContent.text_Content.text = $"编曲 {masterMusic.arranger}  作曲 {masterMusic.composer}  作词 {masterMusic.lyricist}";
            });
        }

        public void OnAddMusic(Radio_MusicLayer.MusicInQueue musicInQueue)
        {
            if (!gameObject.activeSelf)
                return;
            AddItem(universalGenerator.Items.Count + 1, musicInQueue);
        }
    }
}