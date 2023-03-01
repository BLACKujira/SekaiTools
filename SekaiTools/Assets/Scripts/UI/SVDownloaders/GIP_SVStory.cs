using SekaiTools.UI.GenericInitializationParts;
using SekaiTools.UI.NicknameCounterInitialize;
using System.Collections;
using System.Collections.Generic;

namespace SekaiTools.UI.SVDownloaders
{
    public class GIP_SVStory : GIP_PathSelect
    {
        FileStruct fileStruct = FileStruct.Server;
        public FileStruct FileStruct => fileStruct;

        public void SwitchFileStruct_Server() => fileStruct = FileStruct.Server;
        public void SwitchFileStruct_Classic() => fileStruct = FileStruct.Classic;
    }
}