using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Spine
{
    [CreateAssetMenu(menuName = "SekaiTools/Spine/InbuiltModelSet")]
    public class InbuiltSpineModelSet : ScriptableObject
    {
        public Character[] characters = new Character[58];

        Dictionary<string, AtlasAssetPair> keyValuePairs;

        private void OnEnable()
        {
            keyValuePairs = new Dictionary<string, AtlasAssetPair>();
            foreach (var character in characters)
            {
                foreach (var atlasAssetPair in character.atlasAssets)
                {
                    keyValuePairs[atlasAssetPair.name] = atlasAssetPair;
                }
            }
        }

        public AtlasAssetPair GetValue(string name)
        {
            if (!keyValuePairs.ContainsKey(name)) return null;
            return keyValuePairs[name];
        }

        [System.Serializable]
        public class Character
        {
            public List<AtlasAssetPair> atlasAssets = new List<AtlasAssetPair>();
        }

        public AtlasAssetPair[] GetAtlasAssets(int characterID,bool mergeVirtualSinger = false)
        {
            if(!mergeVirtualSinger)
            {
                return characters[characterID].atlasAssets.ToArray();
            }
            else
            {
                List<AtlasAssetPair> atlasAssetPairs = new List<AtlasAssetPair>();
                int[] ids = ConstData.SeparateVirtualSinger(characterID);
                foreach (var id in ids)
                {
                    atlasAssetPairs.AddRange(characters[id].atlasAssets);
                }
                return atlasAssetPairs.ToArray();
            }
        }

        public int BelongsTo(AtlasAssetPair atlasAssetPair)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Character character = characters[i];
                foreach (var atlasAsset in character.atlasAssets)
                {
                    if (atlasAsset == atlasAssetPair)
                        return i;
                }
            }
            return 0;
        }
    }
    [System.Serializable]
    public class AtlasAssetPair
    {
        public string name;
        public AtlasAsset atlasAsset;
        public AtlasAsset atlasAsset_r;

        public AtlasAssetPair(string name, AtlasAsset atlasAsset, AtlasAsset atlasAsset_r)
        {
            this.name = name;
            this.atlasAsset = atlasAsset;
            this.atlasAsset_r = atlasAsset_r;
        }
    }
}