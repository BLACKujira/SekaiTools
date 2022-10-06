namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_StringList : GSW_Item
    {
        public StringListEditItem stringListEditItem;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_StringList configUIItem_StringList = configUIItem as ConfigUIItem_StringList;
            if (configUIItem_StringList == null) throw new ItemTypeMismatchException();

            stringListEditItem.Initialize(configUIItem_StringList.getValue());
        }
    }
}