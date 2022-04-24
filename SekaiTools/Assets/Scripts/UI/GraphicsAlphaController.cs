using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class GraphicsAlphaController : MonoBehaviour
    {
        public Transform targetObject;

        public float alpha
        {
            get => _alpha;
            set
            {
                if(!Initialized)
                {
                    Initialize();
                    Initialized = true;
                }

                for (int i = 0; i < graphics.Length; i++)
                {
                    Color color = graphics[i].color;
                    color.a = Mathf.Lerp(0, alphas[i], value);
                    graphics[i].color = color;
                }
                _alpha = value;
            }
        }

        Graphic[] graphics;
        float[] alphas;
        float _alpha;
        bool Initialized = false;

        private void Initialize()
        {
            graphics = targetObject.GetComponentsInChildren<Graphic>();
            alphas = new float[graphics.Length];
            for (int i = 0; i < graphics.Length; i++)
            {
                alphas[i] = graphics[i].color.a;
            }
        }
    }
}