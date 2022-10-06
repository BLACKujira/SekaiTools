using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class Radio_CMDListLayer : MonoBehaviour
    {
        public Radio radio;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        public AutoScroll autoScroll;

        class CMDInfo
        {
            public string cmd;
            public string description;

            public CMDInfo(string cmd, string description)
            {
                this.cmd = cmd;
                this.description = description;
            }
        }

        /// <summary>
        /// 在电台初始化函数的最后调用，根据组件开关情况生成指令列表
        /// </summary>
        public void Initialize()
        {
            List<CMDInfo> cMDInfos = new List<CMDInfo>();
            cMDInfos.Add(new CMDInfo(
                "/返回", "从当前页面返回主页"));
            cMDInfos.Add(new CMDInfo(
                "/点歌 歌曲名称","向播放列表中添加指定的歌曲，仅支持游戏Project Sekai中出现的歌曲"));
            cMDInfos.Add(new CMDInfo(
                "/歌曲列表", "查看此电台的歌曲列表"));
            cMDInfos.Add(new CMDInfo(
                "/播放列表", "查看当前播放列表"));

            if (radio.serifQueryLayer.enabled)
                cMDInfos.Add(new CMDInfo(
                "/对话查询 角色1 角色2", "显示 角色1 提到 角色2 的对话"));

            universalGenerator.Generate(cMDInfos.Count,
                (gobj, id) =>
                {
                    ItemWithTitleAndContent itemWithTitleAndContent = gobj.GetComponent<ItemWithTitleAndContent>();
                    itemWithTitleAndContent.text_Title.text = cMDInfos[id].cmd;
                    itemWithTitleAndContent.text_Content.text = cMDInfos[id].description;
                });
        }

        public void Play(Action onComplete)
        {
            StartCoroutine(IPlay(onComplete));
        }

        IEnumerator IPlay(Action onComplete)
        {
            yield return 1;
            yield return autoScroll.IPlay(onComplete);
        }
    }
}