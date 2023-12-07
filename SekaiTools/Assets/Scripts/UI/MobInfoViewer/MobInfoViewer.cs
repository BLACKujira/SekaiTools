using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI
{
    /// <summary>
    /// 用于显示配角信息的UI组件
    /// </summary>
    public class MobInfoViewer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator universalGenerator;

        MobInfoCounter mobInfoCounter;
        public MobInfoCounter MobInfoCounter => mobInfoCounter;

        public void Initialize(MobInfoCounter mobInfoCounter)
        {
            this.mobInfoCounter = mobInfoCounter;
            Refresh();
        }

        void Refresh()
        {
            MobInfo[] mobInfos = mobInfoCounter.MobInfos.Values.ToArray();
            universalGenerator.Generate(mobInfoCounter.MobInfos.Count, (gobj, id) =>
            {
                gobj.GetComponent<MobInfoViewer_Item>().SetData(this, mobInfos[id]);
            });
        }
    }
}
