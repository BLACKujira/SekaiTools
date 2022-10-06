using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class StringListEditItem : MonoBehaviour
    {
        public UniversalGenerator universalGenerator;
        [Header("Prefab")]
        public GameObject addItemButtonPrefab;

        List<string> targetList;

        public void Initialize(List<string> targetList)
        {
            this.targetList = targetList;
            Refresh();
        }

        public void Refresh()
        {
            universalGenerator.ClearItems();

            universalGenerator.Generate(targetList.Count, (gobj, id) =>
             {
                 StringListEditItem_Item stringListEditItem_Item = gobj.GetComponent<StringListEditItem_Item>();

                 System.Action moveUp = null; 
                 if(id > 0) moveUp = () =>
                                         {
                                             string temp = targetList[id];
                                             targetList[id] = targetList[id - 1];
                                             targetList[id - 1] = temp;
                                             Refresh();
                                         };

                 System.Action moveDown = null;
                 if(id<targetList.Count-1) moveDown = () =>
                                         {
                                             string temp = targetList[id];
                                             targetList[id] = targetList[id + 1];
                                             targetList[id + 1] = temp;
                                             Refresh();
                                         };

                 stringListEditItem_Item.Initialize(
                     () => targetList[id],
                     (str)=> targetList[id] = str,
                     moveUp,
                     moveDown,
                     ()=>
                     {
                         targetList.RemoveAt(id);
                         Refresh();
                     }) ;
             });

            universalGenerator.AddItem(addItemButtonPrefab, (gobj) =>
             {
                 gobj.GetComponent<Button>().onClick.AddListener(() =>
                 {
                     targetList.Add(string.Empty);
                     Refresh();
                 });
             });
        }

    }
}