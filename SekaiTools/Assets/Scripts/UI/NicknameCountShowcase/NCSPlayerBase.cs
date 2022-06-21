using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using System;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSPlayerBase : MonoBehaviour
    {
        [NonSerialized] public NicknameCountData countData;
        [NonSerialized] public Count.Showcase.NicknameCountShowcase showcase;
        [NonSerialized] public SekaiLive2DModel[] live2DModels = new SekaiLive2DModel[57];
        [NonSerialized] public AudioData audioData;
    }
}