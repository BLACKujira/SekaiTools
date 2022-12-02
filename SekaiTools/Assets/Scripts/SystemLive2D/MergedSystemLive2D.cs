using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.SystemLive2D
{
    [System.Serializable]
    public class MergedSystemLive2D
    {
        public int CharacterId => masterSystemLive2Ds[0].characterId;
        public string Unit => masterSystemLive2Ds[0].unit;
        public string Serif => masterSystemLive2Ds[0].serif;
        public string AssetbundleName => masterSystemLive2Ds[0].assetbundleName;
        public string Voice => masterSystemLive2Ds[0].voice;
        public string Motion => masterSystemLive2Ds[0].motion;
        public string Expression => masterSystemLive2Ds[0].expression;
        public int Weight => masterSystemLive2Ds[0].weight;
        public UnitType UnitType => masterSystemLive2Ds[0].UnitType;

        public bool IsSingle => masterSystemLive2Ds.Count == 1;
        public int FirstId => masterSystemLive2Ds[0].id;

        public List<MasterSystemLive2D> masterSystemLive2Ds;

        public static bool CanMerge(MasterSystemLive2D a, MasterSystemLive2D b)
        {
            return a.assetbundleName.Equals(b.assetbundleName) && a.voice.Equals(b.voice);
        }

        public bool CanMerge(MasterSystemLive2D masterSystemLive2D)
        {
            return CanMerge(masterSystemLive2Ds[0], masterSystemLive2D);
        }

        public MergedSystemLive2D(MasterSystemLive2D masterSystemLive2D)
        {
            masterSystemLive2Ds = new List<MasterSystemLive2D>();
            masterSystemLive2Ds.Add(masterSystemLive2D);
        }

        public static List<MergedSystemLive2D> GetMergedSystemLive2Ds(MasterSystemLive2D[] originTable)
        {
            List<MergedSystemLive2D> mergedSystemLive2Ds = new List<MergedSystemLive2D>();
            foreach (var raw in originTable)
            {
                bool flagAdd = false;
                foreach (var merged in mergedSystemLive2Ds)
                {
                    if(merged.CanMerge(raw))
                    {
                        flagAdd = true;
                        merged.masterSystemLive2Ds.Add(raw);
                        break;
                    }
                }
                if(!flagAdd)
                {
                    MergedSystemLive2D mergedSystemLive2D = new MergedSystemLive2D(raw);
                    mergedSystemLive2Ds.Add(mergedSystemLive2D);
                }
            }
            return mergedSystemLive2Ds;
        }
    }
}