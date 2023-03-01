using SekaiTools.Cutin;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaSceneCreate
{
    public class CreatedKizunaSceneInfo
    {
        public Vector2Int[] bonds;
        public CutinSceneData cutinSceneData;

        public int[] AppearCharacters
        {
            get
            {
                HashSet<int> charIds = new HashSet<int>();
                foreach (var vector2Int in bonds)
                {
                    charIds.Add(vector2Int.x);
                    charIds.Add(vector2Int.y);
                }
                return new List<int>(charIds).ToArray();
            }
        }

        public CreatedKizunaSceneInfo(Vector2Int[] bonds, CutinSceneData cutinSceneData)
        {
            this.bonds = bonds;
            this.cutinSceneData = cutinSceneData;
        }
    }
}