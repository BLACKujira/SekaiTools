using SekaiTools.DecompiledClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Radio
{

    public class Radio_MusicListLayer : MonoBehaviour, IReturnableWindow
    {
        public Radio radio;
        [Header("Components")]
        public RadioReturnableWindowController returnableWindowController;
        public Text text_Title;
        public UniversalGeneratorV2 universalGenerator;
        public AutoScroll autoScroll;
        [Header("Prefab")]
        public Radio_MusicListLayer_Item itemPrefab;

        public ReturnPermission ReturnPermission => returnableWindowController.ReturnPermission;

        public string SenderUserName { get; set; }

        public delegate bool ApplyFilter(List<MusicListItem> musicListItemsIn, string filter, out List<MusicListItem> musicListItemsOut);

        public MusicListQueryResult Query(MusicListQueryInfo musicListQueryInfo)
        {
            List<MusicListItem> musicListItems = new List<MusicListItem>();

            foreach (var musicData in radio.musicLayer.MusicDatas)
            {
                if(musicData!=null)
                    musicListItems.Add(new MusicListItem(musicData, new List<MusicVocalData>(musicData.vocalDatas)));
            }

            ApplyFilter[] applyFilters =
            {
                ApplyFilter_Unit,
                ApplyFilter_MusicName
            };
            foreach (var filter in musicListQueryInfo.filters)
            {
                bool flag = false;
                foreach (var applyFilter in applyFilters)
                {
                    if (applyFilter(musicListItems, filter, out List<MusicListItem> musicListItemsNext))
                    {
                        musicListItems = musicListItemsNext;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    musicListItems = ApplyFilter_SelectVocal(musicListItems,filter);
                    break;
                }
                if (musicListItems.Count == 0)
                    break;
            }
            return new MusicListQueryResult(musicListQueryInfo,musicListItems); 
        }

        bool ApplyFilter_Unit(List<MusicListItem> musicListItemsIn,string filter,out List<MusicListItem> musicListItemsOut)
        {
            Unit? unit = radio.musicLayer.unitNames.GetValueNullable(filter);
            if(unit==null)
            {
                musicListItemsOut = null;
                return false;
            }
            MusicTag musicTag;
            switch (unit)
            {
                case Unit.none:
                    musicTag = MusicTag.other;
                    break;
                case Unit.VirtualSinger:
                    musicTag = MusicTag.vocaloid;
                    break;
                case Unit.Leoneed:
                    musicTag = MusicTag.light_music_club;
                    break;
                case Unit.MOREMOREJUMP:
                    musicTag = MusicTag.idol;
                    break;
                case Unit.VividBADSQUAD:
                    musicTag = MusicTag.street;
                    break;
                case Unit.WonderlandsXShowtime:
                    musicTag = MusicTag.theme_park;
                    break;
                case Unit.NightCord:
                    musicTag = MusicTag.school_refusal;
                    break;
                default:
                    musicTag = MusicTag.all;
                    break;
            }
            musicListItemsOut = new List<MusicListItem>(
                from MusicListItem item in musicListItemsIn
                where item.musicData.musicTag.MusicTags.Contains(musicTag) &&
                item.musicVocalDatas.Count > 0
                select item);
            return true;
        }

        bool ApplyFilter_MusicName(List<MusicListItem> musicListItemsIn, string filter, out List<MusicListItem> musicListItemsOut)
        {
            int musicId = radio.musicLayer.musicNames.GetValue(filter);
            if (musicId == -1)
            {
                musicListItemsOut = null;
                return false;
            }
            foreach (var musicListItem in musicListItemsIn)
            {
                if(musicListItem.musicData.id == musicId)
                {
                    musicListItemsOut = new List<MusicListItem>() { musicListItem };
                    return true;
                }
            }
            musicListItemsOut = null;
            return false;
        }

        List<MusicListItem> ApplyFilter_SelectVocal(List<MusicListItem> musicListItems, string filter)
        {
            List<MusicListItem> musicListItemsOut = new List<MusicListItem>();
            foreach (var musicListItem in musicListItems)
            {
                List<MusicVocalData> musicVocalDatas = musicListItem.musicData.FitterVocal(radio.musicLayer, filter);
                if (musicVocalDatas.Count > 0)
                    musicListItemsOut.Add(new MusicListItem(musicListItem.musicData, musicVocalDatas));
            }
            return musicListItemsOut;
        }

        public void SetDisplayQuery(MusicListQueryResult musicListQueryResult)
        {
            universalGenerator.ClearItems();

            foreach (var musicListItem in musicListQueryResult.resultItems)
            {
                universalGenerator.AddItem(itemPrefab.gameObject, (gobj) =>
                {
                    Radio_MusicListLayer_Item radio_MusicListLayer_Item = gobj.GetComponent<Radio_MusicListLayer_Item>();
                    radio_MusicListLayer_Item.Initialize(musicListItem.musicData, 
                        musicListItem.musicVocalDatas.ToArray(),
                        radio.musicLayer.musicNames.GetAliases(musicListItem.musicData.id));
             });
            }
        }
    
        public void Play(string userName,Action onComplete)
        {
            SenderUserName = userName;
            StartCoroutine(IPlay(onComplete));
            returnableWindowController.ResetReturnPermission();
        }

        IEnumerator IPlay(Action onComplete)
        {
            yield return 1;
            yield return autoScroll.IPlay(onComplete);
        }
    }
}