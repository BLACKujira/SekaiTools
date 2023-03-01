using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.CoupleCombiner
{
    public class CoupleCombiner : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public List<Toggle> toggles;
        public ButtonGenerator buttonGenerator;

        int[] SelectedIDs
        {
            get
            {
                List<int> ids = new List<int>();
                for (int i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i] && toggles[i].isOn)
                        ids.Add(i);
                }
                return ids.ToArray();
            }
        }
        Vector2Int[] SelectedBonds
        {
            get
            {
                List<Vector2Int> vector2Ints = new List<Vector2Int>(bonds);
                return vector2Ints.ToArray();
            }
        }

        HashSet<Vector2Int> bonds = new HashSet<Vector2Int>();
        Action<Vector2Int[]> onApply;

        public void Initialize(Vector2Int[] vector2Ints = null, Action<Vector2Int[]> onApply = null)
        {
            this.onApply = onApply;
            if (vector2Ints != null) bonds = new HashSet<Vector2Int>(vector2Ints);
            if (onApply != null) window.OnClose.AddListener(() => { onApply(SelectedBonds); });

            RefreshButtons();
        }

        public void CombineAndAddToList()
        {
            List<Vector2Int> bonds = new List<Vector2Int>();
            int[] selectedIDs = this.SelectedIDs;
            for (int i = 0; i < selectedIDs.Length; i++)
            {
                for (int j = i + 1; j < selectedIDs.Length; j++)
                {
                    bonds.Add(new Vector2Int(selectedIDs[i], selectedIDs[j]));
                }
            }
            foreach (var bond in bonds)
            {
                this.bonds.Add(bond);
            }
            RefreshButtons();
            foreach (var toggle in toggles)
            {
                if (toggle && toggle.isOn) toggle.isOn = false;
            }
        }

        public void RefreshButtons()
        {
            buttonGenerator.ClearButtons();
            List<Vector2Int> bondsList = new List<Vector2Int>(bonds);
            buttonGenerator.Generate(bondsList.Count,
                (Button button, int id) =>
                {
                    BondsHonorSub bondsHonorSub = button.GetComponent<BondsHonorSub>();
                    bondsHonorSub.SetCharacter(bondsList[id].x, bondsList[id].y);
                },
                (int id) =>
                {
                    bonds.Remove(bondsList[id]);
                    RefreshButtons();
                });
        }

        public void Save()
        {
            SaveFileDialog saveFileDialog = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_CPF);
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = saveFileDialog.FileName;

            SaveData saveData = new SaveData(bonds);
            string json = JsonUtility.ToJson(saveData, true);

            File.WriteAllText(fileName, json);
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_CPF);
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;

            string json = File.ReadAllText(fileName);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            bonds = new HashSet<Vector2Int>(saveData.bonds);
            RefreshButtons();
        }

        public void Apply()
        {
            onApply?.Invoke(SelectedBonds);
            window.Close();
        }

        public class SaveData
        {
            public List<Vector2Int> bonds;

            public SaveData(HashSet<Vector2Int> bonds)
            {
                this.bonds = new List<Vector2Int>(bonds);
            }
        }
    }
}