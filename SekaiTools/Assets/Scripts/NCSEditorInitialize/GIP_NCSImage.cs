using SekaiTools.UI.Downloader;
using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.NicknameCountShowcase;
using System.Collections.Generic;
using System.IO;

namespace SekaiTools.UI.NCSEditorInitialize
{
    public class GIP_NCSImage : GIP_ImageData
    {
        protected override IEnumerable<string> RequireImageKeys
        {
            get
            {
                HashSet<string> keys = new HashSet<string>();
                foreach (var scene in nicknameCountShowcase.scenes)
                {
                    if (scene.nCSScene is IImageFileReference imageFileReference)
                    {
                        keys.UnionWith(imageFileReference.RequireImageKeys);
                    }
                }
                return keys;
            }
        }
        protected override DownloadFileInfo[] RequireFileList => base.RequireFileList;
        protected override string DefaultFileName => Path.GetFileNameWithoutExtension(nicknameCountShowcase.SavePath) + ".imd";

        Count.Showcase.NicknameCountShowcase nicknameCountShowcase;
        public void Initialize(Count.Showcase.NicknameCountShowcase nicknameCountShowcase)
        {
            base.Initialize();
            this.nicknameCountShowcase = nicknameCountShowcase;
        }
    }
}