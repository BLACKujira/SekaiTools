using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.BackGround
{
    /// <summary>
    /// 背景控制器
    /// </summary>
    public class BackGroundController : MonoBehaviour
    {
        public const string strBuiltIn = "Built-in";

        public BackGroundRoot defaultBackGroundPrefab;

        private string spritePath = strBuiltIn;
        [SerializeField] private BackGroundRoot backGround;
        [SerializeField] private List<BackGroundPart> decorations = new List<BackGroundPart>();
        public BackGroundPartSet decorationSet;
        public BackGroundPrefabSet bGPrefabSet;

        public static BackGroundController backGroundController;

        public BackGroundRoot BackGround { get => backGround; }
        public List<BackGroundPart> Decorations
        {
            get
            {
                return decorations;
            }
        }
        public string SpritePath { get => spritePath;}

        private void Awake()
        {
            backGroundController = this;
        }

        public BackGroundPart AddDecoration(BackGroundPart prefab,int index = -1)
        {
            BackGroundPart backGroundDecoration = Instantiate(prefab, transform);
            backGroundDecoration.name = prefab.name;
            if (index == -1) decorations.Add(backGroundDecoration);
            else decorations.Insert(Mathf.Min(index,decorations.Count), backGroundDecoration);
            SetSortingLayers();
            return backGroundDecoration;
        }
        public BackGroundPart AddDecorationBehindForeground(BackGroundPart prefab)
        {
            if (!decorations[decorations.Count - 1].ifForeground) return AddDecoration(prefab); 
            else
            {
                int index = decorations.Count;
                while (index>=0)
                {
                    if(!decorations[index].ifForeground)
                    {
                        return AddDecoration(prefab, index);
                    }
                    index--;
                }
            }
            return AddDecoration(prefab, 0);
        }

        public void RemoveDecoration(int id)
        {
            Destroy(decorations[id].gameObject);
            decorations.RemoveAt(id);
            SetSortingLayers();
        }
        public void RemoveDecoration(BackGroundPart decoration)
        {
            int id = decorations.IndexOf(decoration);
            if (id == -1) return;
            RemoveDecoration(id);
        }

        public void ChangeBackGround(Sprite newSprite, string spritePath = strBuiltIn)
        {
            ChangeBackGround(defaultBackGroundPrefab,spritePath);
            backGround.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        public void ChangeBackGround(BackGroundRoot prefab, string spritePath = strBuiltIn ,bool addPrefabParts = true)
        {
            BackGroundRoot backGround = Instantiate(prefab, transform);
            backGround.name = prefab.name;
            if (this.backGround.gameObject) Destroy(this.backGround.gameObject);
            this.backGround = backGround;
            this.spritePath = spritePath;

            BackGroundPart[] oldDecorations = decorations.ToArray();
            foreach (var item in oldDecorations)
            {
                if (item.isPartOfBGPrefab)
                    decorations.Remove(item);
            }

            if (addPrefabParts)
            {
                //将预制件所带的部分添加到背景中
                foreach (var backGroundPart in BackGround.decorations)
                {
                    backGroundPart.isPartOfBGPrefab = true;
                    backGroundPart.disableRemove = true;
                    if(backGroundPart.ifForeground)
                        decorations.Add(backGroundPart);
                    else
                        decorations.Insert(0,backGroundPart);
                }
                SetSortingLayers();
            }
        }

        public void ClearAndReset()
        {
            List<BackGroundPart> backGroundParts = new List<BackGroundPart>(Decorations);
            foreach (var backGroundPart in backGroundParts)
            {
                if(!backGroundPart.disableRemove)
                    RemoveDecoration(backGroundPart);
            }
        }

        public void SetSortingLayers()
        {
            int layerID = 1;
            layerID = backGround.mainPart.SetSortingLayer(layerID);
            foreach (var backGroundPart in Decorations)
            {
                layerID = backGroundPart.SetSortingLayer(layerID);
            }
        }

        //用于序列化的类，生成时读取当前背景配置
        [System.Serializable]
        public class BackGroundSaveData
        {
            public string spritePath;
            public BackGroundPart.BackGroundPartSaveData backGround;
            public List<BackGroundPart.BackGroundPartSaveData> parts;

            public BackGroundSaveData(BackGroundController backGroundController)
            {
                spritePath = backGroundController.SpritePath;
                backGround = backGroundController.BackGround.mainPart.GetSaveData();
                parts = new List<BackGroundPart.BackGroundPartSaveData>();

                foreach (var backGroundPart in backGroundController.Decorations)
                {
                    if(!backGroundPart.nonSerialize)
                        parts.Add(backGroundPart.GetSaveData());
                }
            }
        }

        public string[] Load(BackGroundSaveData saveData,List<BackGroundPart> backGroundParts = null)
        {
            ClearAndReset();
            List<string> log = new List<string>();

            //读取背景
            if (saveData.spritePath.Equals(strBuiltIn))
            {
                BackGroundRoot backGroundParefab = bGPrefabSet.GetPrefab(saveData.backGround.name);
                if (backGroundParefab == null)
                {
                    ChangeBackGround(defaultBackGroundPrefab,strBuiltIn,false);
                    log.Add("未找到背景预制件 " + saveData.backGround.name);
                }
                else
                {
                    ChangeBackGround(backGroundParefab);
                    try
                    {
                        backGroundController.BackGround.mainPart.Load(saveData.backGround);
                    }
                    catch
                    {
                        log.Add("读取装饰失败 " + saveData.backGround.name);
                    }

                }
            }
            else
            {
                if (File.Exists(saveData.spritePath))
                {
                    ImageData imageData = new ImageData(null);
                    NowLoadingTypeA nowLoadingTypeA = WindowController.windowController.currentWindow.OpenWindow<NowLoadingTypeA>(WindowController.windowController.nowLoadingTypeAWindow);
                    nowLoadingTypeA.StartProcess(imageData.LoadFile());
                }
                else
                {
                    ChangeBackGround(defaultBackGroundPrefab);
                    log.Add("未找到背景图像 " + saveData.spritePath);
                }
            }

            List<BackGroundPart> parts = backGroundParts ?? decorationSet.backGroundParts;
            //设置装饰
            foreach (var backGroundPart in saveData.parts)
            {
                BackGroundPart bGPart = null;
                foreach (var part in parts)
                {
                    if(part.name.Equals(backGroundPart.name))
                    {
                        bGPart = part;
                        break;
                    }
                }

                if (bGPart == null)
                {
                    log.Add("未找到装饰 " + backGroundPart.name);
                }
                else
                {
                    try
                    {
                        AddDecoration(bGPart).Load(backGroundPart);
                    }
                    catch
                    {
                        log.Add("读取装饰失败 " + backGroundPart.name);
                    }
                }
            }

            return log.ToArray();
        }

        public class BackGroundPartSaveDataWithOrder : BackGroundPart.BackGroundPartSaveData
        {
            public int order;
            public BackGroundPartSaveDataWithOrder(BackGroundPart backGroundPart,int order) : base(backGroundPart)
            {
                this.order = order;
            }
        }
    }
}