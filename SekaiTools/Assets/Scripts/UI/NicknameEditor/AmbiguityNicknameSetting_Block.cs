using SekaiTools.Count;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameSetting
{
    public class AmbiguityNicknameSetting_Block : MonoBehaviour
    {
        public NicknameSetting nicknameSetting;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        [Header("Prefab")]
        public Button addItemButtonPrefab;

        AmbiguityNicknameSet ambiguityNicknameSet;

        public void Initialize(AmbiguityNicknameSet ambiguityNicknameSet)
        {
            this.ambiguityNicknameSet = ambiguityNicknameSet;
            Refresh();
        }

        public void Refresh()
        {
            universalGenerator.ClearItems();

            universalGenerator.Generate(ambiguityNicknameSet.ambiguityRegices.Count,
                (GameObject gameObject, int id) =>
                {
                    NicknameSetting_Block_Item nicknameSetting_Block_Item = gameObject.GetComponent<NicknameSetting_Block_Item>();
                    nicknameSetting_Block_Item.Initialize(ambiguityNicknameSet.ambiguityRegices[id], (string str) => { ambiguityNicknameSet.ambiguityRegices[id] = str; });
                    nicknameSetting_Block_Item.removeButton.onClick.AddListener(() =>
                    {
                        ambiguityNicknameSet.ambiguityRegices.RemoveAt(id);
                        Refresh();
                    });
                });
            universalGenerator.AddItem(addItemButtonPrefab.gameObject, (GameObject gameObject) =>
            {
                Button button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    ambiguityNicknameSet.ambiguityRegices.Add("");
                    Refresh();
                });
            });
        }
    }
}