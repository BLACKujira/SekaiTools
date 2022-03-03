using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    /// <summary>
    /// 背景控制器
    /// </summary>
    public class BackGroundController : MonoBehaviour
    {
        public BackGroundPart defaultBackGroundPrefab;

        private string spritePath = "Built-in";
        [SerializeField] private BackGroundPart backGround;
        [SerializeField] private List<BackGroundPart> decorations = new List<BackGroundPart>();

        public static BackGroundController backGroundController;

        public BackGroundPart BackGround { get => backGround; }
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

        public void AddDecoration(BackGroundPart prefab)
        {
            BackGroundPart backGroundDecoration = Instantiate(prefab, transform);
            backGroundDecoration.name = prefab.name;
            decorations.Add(backGroundDecoration);
        }
        public void RemoveDecoration(int id)
        {
            Destroy(decorations[id].gameObject);
            decorations.RemoveAt(id);
        }
        
        public void ChangeBackGround(Sprite newSprite, string spritePath = "Built-in")
        {
            ChangeBackGround(defaultBackGroundPrefab,spritePath);
            backGround.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        public void ChangeBackGround(BackGroundPart prefab, string spritePath = "Built-in")
        {
            BackGroundPart backGround = Instantiate(prefab, transform);
            backGround.name = prefab.name;
            if (this.backGround.gameObject) Destroy(this.backGround.gameObject);
            this.backGround = backGround;
            this.spritePath = spritePath;
        }
    }
}