using SekaiTools.DecompiledClass;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class MusicData
    {
        public int id => masterMusic.id;
        public readonly MasterMusic masterMusic;
        public readonly List<MusicVocalData> vocalDatas = new List<MusicVocalData>();
        public readonly MusicTagData musicTag; 

        public MusicData(MasterMusic masterMusic, IEnumerable<MusicTag> musicTags)
        {
            this.masterMusic = masterMusic;
            musicTag = new MusicTagData(musicTags);
        }

        delegate bool ApplyFilter(List<MusicVocalData> vocalListIn, string fitter, Radio_MusicLayer player, out List<MusicVocalData> vocalListOut);
        public MusicVocalData GetVocal(MusicOrderInfo musicOrderInfo, Radio_MusicLayer player)
        {
            List<MusicVocalData> filteredVocal = FitterVocal(player,musicOrderInfo.filters);
            return GetTopPriorityVocal(filteredVocal);
        }

        public List<MusicVocalData> FitterVocal(Radio_MusicLayer player,params string[] filters)
        {
            List<MusicVocalData> filteredVocal = new List<MusicVocalData>(vocalDatas);

            ApplyFilter[] applyFilters =
            {
                ApplyFilter_VocalSize,
                ApplyFilter_VocalType,
                ApplyFilter_VocalSinger
            };
            foreach (var filter in filters)
            {
                bool flag = false;
                foreach (var applyFilter in applyFilters)
                {
                    if (applyFilter(filteredVocal, filter, player, out List<MusicVocalData> filteredVocalNext))
                    {
                        filteredVocal = filteredVocalNext;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    filteredVocal = new List<MusicVocalData>();
                    break;
                }
            }

            return filteredVocal;
        }

        public MusicVocalData GetTopPriorityVocal(List<MusicVocalData> filteredVocal)
        {
            List<MusicVocalData> lastFilteredVocal = new List<MusicVocalData>();
            MusicVocalType[] musicVocalTypePriority = new MusicVocalType[]
                {
                    MusicVocalType.sekai,
                    MusicVocalType.april_fool_2022,
                    MusicVocalType.virtual_singer,
                    MusicVocalType.another_vocal,
                    MusicVocalType.instrumental
                };
            string[] musicSizeTypePriority = { "full", "game" };
            for (int i = 0; i < musicVocalTypePriority.Length; i++)
            {
                MusicVocalType musicVocal = musicVocalTypePriority[i];
                lastFilteredVocal = new List<MusicVocalData>(from MusicVocalData musicVocalData in filteredVocal
                                                             where musicVocalData.vocalType == musicVocal
                                                             select musicVocalData);
                if (lastFilteredVocal.Count > 0)
                    break;
            }

            for (int i = 0; i < musicSizeTypePriority.Length; i++)
            {
                string musicSize = musicSizeTypePriority[i];
                foreach (var vocalData in lastFilteredVocal)
                {
                    if (vocalData.musicSize.Equals(musicSize))
                        return vocalData;
                }
            }
            if (lastFilteredVocal.Count > 0) return lastFilteredVocal[0];
            else return null;
        }

        public MusicVocalData GetTopPriorityVocal()
        {
            return GetTopPriorityVocal(vocalDatas);
        }

        bool ApplyFilter_VocalSize(List<MusicVocalData> vocalListIn, string fitter, Radio_MusicLayer player , out List<MusicVocalData> vocalListOut)
        {
            var filteredVocal = new List<MusicVocalData>();
            string size = player.sizeTypeNames.GetValue(fitter);
            if (!string.IsNullOrEmpty(size))
            {
                string sizeType = player.sizeTypeNames.GetValue(size);
                if (string.IsNullOrEmpty(sizeType))
                    sizeType = size;
                filteredVocal = new List<MusicVocalData>(from MusicVocalData vocal in vocalListIn
                                                         where vocal.musicSize.Equals(sizeType)
                                                         select vocal);
                vocalListOut = filteredVocal;
                return true;
            }
            vocalListOut = null;
            return false;
        }

        bool ApplyFilter_VocalType(List<MusicVocalData> vocalListIn, string fitter, Radio_MusicLayer player, out List<MusicVocalData> vocalListOut)
        {
            var filteredVocal = new List<MusicVocalData>();
            //首先判断是否为类型名称
            string vocalType = player.vocalTypeNames.GetValue(fitter);
            if (!string.IsNullOrEmpty(vocalType))
            {
                MusicVocalType musicVocalType;
                if (!Enum.TryParse(vocalType, out musicVocalType))
                {
                    vocalListOut = null;
                    return false;
                }
                else
                {
                    filteredVocal = new List<MusicVocalData>(from MusicVocalData vocal in vocalListIn
                                                             where vocal.vocalType == musicVocalType
                                                             select vocal);
                    vocalListOut = filteredVocal;
                    return true;
                }
            }
            vocalListOut = null;
            return false;
        }

        bool ApplyFilter_VocalSinger(List<MusicVocalData> vocalListIn, string fitter, Radio_MusicLayer player, out List<MusicVocalData> vocalListOut)
        {
            vocalListOut = null;
            var filteredVocal = new List<MusicVocalData>();
            //之后判断是否为cp名称
            string singerStr = fitter;
            string cpName = player.cpNames.GetValue(fitter.ToLower());
            if (!string.IsNullOrEmpty(cpName))
            {
                singerStr = cpName;
            }
            //最后分割并分别判断是否为角色名称
            string[] singerArray = singerStr.Split('_');
            List<string> singerList = new List<string>();
            foreach (var singer in singerArray)
            {
                string sname = player.singerNames.GetValue(singer);
                if (string.IsNullOrEmpty(sname))
                {
                    vocalListOut = null;
                    return false;
                }

                singerList.Add(sname);
            }
            filteredVocal = new List<MusicVocalData>(from MusicVocalData vocal in vocalListIn
                                                     where new HashSet<string>(vocal.singers).SetEquals(singerList)
                                                     select vocal);
            vocalListOut = filteredVocal;
            return true;
        }
    }
}