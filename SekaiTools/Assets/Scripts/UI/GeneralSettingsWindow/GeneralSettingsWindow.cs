using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public abstract class GeneralSettingsWindow : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text titleText;
        public ToggleGenerator toggleGenerator;
        public RectTransform targetRectTransform;
        [Header("Settings")]
        public float itemDistance = 15f;
        [Header("Prefab")]
        public GSW_Label labelPrefab;

        public abstract (Type type, GSW_Item item)[] typeDictionary { get; }
        Dictionary<Type, GSW_Item> runtimeTypeDictionary;
        GroupedItems groupedItems;
        int currentTabId = 0;
        List<GameObject> items = new List<GameObject>();

        public void Initialize(ConfigUIItem[] configUIItems,Action onExit = null,string title = "设置")
        {
            titleText.text = title;
            if (onExit != null) window.OnClose.AddListener(() => { if (onExit != null) onExit(); });
            groupedItems = new GroupedItems();
            
            foreach (var configUIItem in configUIItems)
            {
                groupedItems.AddItem(configUIItem);
            }

            runtimeTypeDictionary = new Dictionary<Type, GSW_Item>();
            foreach (var keyValuePair in typeDictionary)
            {
                runtimeTypeDictionary[keyValuePair.type] = keyValuePair.item;
            }

            toggleGenerator.Generate(groupedItems.itemGroups.Count,
                (toggle, id) =>
                {
                    toggle.GetComponent<GSW_Tab>().text = groupedItems.itemGroups[id].groupName;
                },
                (value, id) =>
                {
                    if (value)
                    {
                        currentTabId = id;
                        Refresh();
                    }
                });

            if(toggleGenerator.toggles.Count>0)
            {
                toggleGenerator.toggles[0].isOn = true;
                Refresh();
            }
        }

        public void Refresh()
        {
            foreach (var item in items)
            {
                Destroy(item);
            }
            items = new List<GameObject>();

            float height = 5;
            foreach (var item in groupedItems.itemGroups[currentTabId].items)
            {
                GSW_Label gSWLabel = Instantiate(labelPrefab, targetRectTransform);
                gSWLabel.rectTransform.anchoredPosition = new Vector2(gSWLabel.rectTransform.anchoredPosition.x, -height);
                gSWLabel.labelText.text = item.itemName;
                height += gSWLabel.rectTransform.sizeDelta.y;
                height += itemDistance;

                GSW_Item gSWItem = Instantiate(runtimeTypeDictionary[item.GetType()], targetRectTransform);
                gSWItem.rectTransform.anchoredPosition = new Vector2(gSWItem.rectTransform.anchoredPosition.x,-height);
                gSWItem.Initialize(item,this);
                height += gSWItem.rectTransform.sizeDelta.y;
                height += itemDistance;

                items.Add(gSWLabel.gameObject);
                items.Add(gSWItem.gameObject);
            }
            targetRectTransform.sizeDelta = new Vector2(targetRectTransform.sizeDelta.x, height);
        }
        public class GroupedItems
        {
            public List<ItemGroup> itemGroups = new List<ItemGroup>();   
            
            public void AddItem(ConfigUIItem configUIItem)
            {
                foreach (var itemGroup in itemGroups)
                {
                    if(itemGroup.groupName.Equals(configUIItem.itemGroup))
                    {
                        itemGroup.items.Add(configUIItem);
                        return;
                    }
                }
                ItemGroup newGroup = new ItemGroup(configUIItem.itemGroup);
                newGroup.items.Add(configUIItem);
                itemGroups.Add(newGroup);
            }
        }
        public class ItemGroup
        {
            public string groupName;

            public ItemGroup(string groupName)
            {
                this.groupName = groupName;
            }

            public List<ConfigUIItem> items = new List<ConfigUIItem>();
        }

    }
}