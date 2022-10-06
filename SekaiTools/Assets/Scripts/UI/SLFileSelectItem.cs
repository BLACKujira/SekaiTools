using System.Windows.Forms;

namespace SekaiTools.UI
{
    public class SLFileSelectItem : LoadFileSelectItem
    {
        public bool ifNewFile = false;

        public override void SelectPath()
        {
            base.SelectPath();
            ifNewFile = false;
        }

        public void NewFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "新建文件";
            saveFileDialog.Filter = fileFilter;
            saveFileDialog.RestoreDirectory = true;
            if (!string.IsNullOrEmpty(SelectedPath)) saveFileDialog.FileName = SelectedPath;

            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            pathInputField.text = saveFileDialog.FileName;
            ifNewFile = true;
        }
    }
}