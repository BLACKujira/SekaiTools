using SekaiTools.Count.Showcase;
using SekaiTools.UI.GenericInitializationParts;

namespace SekaiTools.UI.NCSEditorInitialize
{
    public class GIP_NCSSaveData : GIP_CreateOrSaveData
    {
        public Count.Showcase.NicknameCountShowcase NicknameCountShowcase
        {
            get
            {
                if(IfNewFile) 
                {
                    Count.Showcase.NicknameCountShowcase nicknameCountShowcase = new Count.Showcase.NicknameCountShowcase();
                    nicknameCountShowcase.SavePath = file_SaveData.SelectedPath;
                    return nicknameCountShowcase;
                }
                else
                {
                    return Count.Showcase.NicknameCountShowcase.LoadData(file_LoadData.SelectedPath,false);
                }
            }
        }
    }
}