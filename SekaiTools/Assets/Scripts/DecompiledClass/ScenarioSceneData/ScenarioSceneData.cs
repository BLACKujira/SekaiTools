// Sekai.ScenarioSceneData
using UnityEngine;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class ScenarioSceneData
    {
        public string ScenarioId;
        public ScenarioCharacterResourceSet[] AppearCharacters;
        public ScenarioCharacterLayout[] FirstLayout;
        public string FirstBgm;
        public string FirstBackground;
        public ScenarioSnippet[] Snippets;
        public ScenarioSnippetTalk[] TalkData;
        public ScenarioSnippetCharacterLayout[] LayoutData;
        public ScenarioSnippetSpecialEffect[] SpecialEffectData;
        public ScenarioSnippetSound[] SoundData;
        public string[] NeedBundleNames;
        public string[] IncludeSoundDataBundleNames;

        //构造函数，感觉是默认生成的，暂且先留着吧
        public ScenarioSceneData()
        {

        }
    }
}