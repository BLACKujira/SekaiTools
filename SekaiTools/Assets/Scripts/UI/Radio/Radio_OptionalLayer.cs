using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public abstract class Radio_OptionalLayer : MonoBehaviour
    {
        public Radio radio;
        protected bool enableLayer = false;
        public bool EnableLayer => enableLayer;

        protected void Initialize(Settings settings)
        {
            enableLayer = settings.enable;
        }

        public abstract class Settings
        {
            public bool enable;
        }
    }
}