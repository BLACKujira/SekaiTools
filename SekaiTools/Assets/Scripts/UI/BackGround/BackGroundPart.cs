using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SekaiTools.UI.BackGround
{
    /// <summary>
    /// 标记一个游戏对象为背景的一部分，有一个预览
    /// </summary>
    public class BackGroundPart : MonoBehaviour
    {
        public string itemName;
        public Sprite preview;
        public List<BGModifierBase> bGModifiers;
        public List<Component> sortingLayerController;
        public bool ifForeground = false;
        public bool nonSerialize = false;

        public UnityEvent onDestroy;

        [System.NonSerialized] public bool disableRemove = false;
        [System.NonSerialized] public bool isPartOfBGPrefab = false;

        private void OnDestroy()
        {
            onDestroy.Invoke();
        }

        public T GetModifier<T>() where T : class
        {
            foreach (var bGModifier in bGModifiers)
            {
                if (bGModifier is T)
                    return bGModifier as T;
            }
            return null;
        }

        /// <summary>
        /// 依次为组件设置图层，返回下一个图层的ID
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public int SetSortingLayer(int layer)
        {
            foreach (var component in sortingLayerController)
            {
                if(component is SpriteRenderer)
                {
                    SpriteRenderer spriteRenderer = (SpriteRenderer)component;
                    spriteRenderer.sortingOrder = layer;
                }
                else if(component is ParticleSystem)
                {
                    ParticleSystemRenderer particleSystemRenderer = component.GetComponent<ParticleSystemRenderer>();
                    particleSystemRenderer.sortingOrder = layer;
                }
                else if (component is Canvas)
                {
                    Canvas canvas = (Canvas)component;
                    canvas.sortingOrder = layer;
                }
                else
                {
                    Debug.LogError("Unsupported component type");
                }
                layer++;
            }
            return layer;
        }

        public void Load(BackGroundPartSaveData backGroundPartSave)
        {
            for (int i = 0; i < bGModifiers.Count; i++)
            {
                bGModifiers[i].Deserialize(backGroundPartSave.serializedModifiers[i]);
            }
        }

        public BackGroundPartSaveData GetSaveData()
        {
            return new BackGroundPartSaveData(this);
        }

        [System.Serializable]
        public class BackGroundPartSaveData
        {
            public string name;
            public string[] serializedModifiers;

            public BackGroundPartSaveData(BackGroundPart backGroundPart)
            {
                name = backGroundPart.name;
                serializedModifiers = new string[backGroundPart.bGModifiers.Count];
                for (int i = 0; i < backGroundPart.bGModifiers.Count; i++)
                {
                    serializedModifiers[i] = backGroundPart.bGModifiers[i].Serialize();
                }
            }
        }

    }
}