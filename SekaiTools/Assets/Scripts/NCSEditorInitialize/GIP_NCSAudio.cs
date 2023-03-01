using SekaiTools.Message;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.NicknameCountShowcase;
using System.Collections.Generic;
using System.IO;

namespace SekaiTools.UI.NCSEditorInitialize
{
    public class GIP_NCSAudio : GIP_AudioData
    {
        protected override IEnumerable<string> RequireAudioKeys
        {
            get
            {
                HashSet<string> keys = new HashSet<string>();
                foreach (var scene in nicknameCountShowcase.scenes)
                {
                    if(scene.nCSScene is IAudioFileReference audioFileReference) 
                    {
                        keys.UnionWith(audioFileReference.RequireAudioKeys);
                    }
                }
                return keys;
            }
        }

        protected override string DefaultFileName => Path.GetFileNameWithoutExtension(nicknameCountShowcase.SavePath) + ".aud";

        Count.Showcase.NicknameCountShowcase nicknameCountShowcase;
        public void Initialize(Count.Showcase.NicknameCountShowcase nicknameCountShowcase)
        {
            base.Initialize();
            this.nicknameCountShowcase = nicknameCountShowcase;
        }
    }
}