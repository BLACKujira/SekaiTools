using SekaiTools.DecompiledClass;
using System.Collections.Generic;

namespace SekaiTools.UI.Radio
{
    public class MusicTagData
    {
        private HashSet<MusicTag> musicTags;

        static MusicTag[] unitTags =
        {
                MusicTag.light_music_club,
                MusicTag.idol,
                MusicTag.street,
                MusicTag.theme_park,
                MusicTag.school_refusal
            };

        public MusicTag TopPriorityTag
        {
            get
            {
                //其他类别优先级最高
                if (MusicTags.Contains(MusicTag.other))
                    return MusicTag.other;

                int unitTagCount = 0;
                MusicTag unitTag = MusicTag.all;
                foreach (var tag in unitTags)
                {
                    if (MusicTags.Contains(tag))
                    {
                        unitTag = tag;
                        unitTagCount++;
                    }
                }

                if (unitTagCount > 1)
                {
                    return MusicTag.all;
                }
                else if (unitTagCount == 1)
                {
                    return unitTag;
                }
                else
                {
                    if (MusicTags.Contains(MusicTag.vocaloid))
                        return MusicTag.vocaloid;
                }
                return MusicTag.all;
            }
        }

        public MusicTag[] SortedTags
        {
            get
            {
                List<MusicTag> musicTags = new List<MusicTag>(this.MusicTags);
                musicTags.Sort((x, y) => ((int)x).CompareTo((int)y));
                return musicTags.ToArray();
            }
        }

        public HashSet<MusicTag> MusicTags { get => musicTags;}

        public MusicTagData(IEnumerable<MusicTag> musicTags)
        {
            this.musicTags = new HashSet<MusicTag>(musicTags);
        }
    }
}