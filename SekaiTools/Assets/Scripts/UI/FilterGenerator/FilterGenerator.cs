using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.FilterGenerator
{
    public class FilterGenerator : MonoBehaviour
    {
        public Window window;
        public HashSet<Vector2Int> bonds = new HashSet<Vector2Int>();
        public ButtonGenerator buttonGenerator;
        public List<Toggle> toggles;
        public int[] selectedIDs 
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
        public Vector2Int[] selectedBonds
        {
            get
            {
                List<Vector2Int> vector2Ints = new List<Vector2Int>(bonds);
                return vector2Ints.ToArray();
            }
        }

        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;

        private void Awake()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择组合分类存档";
            openFileDialog.Filter = "组合分类存档 (*.cpf)|*.cpf|Others (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存组合分类存档";
            saveFileDialog.Filter = "组合分类存档 (*.cpf)|*.cpf|Others (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;
        }

        public void CombineAndAddToList()
        {
            List<Vector2Int> bonds = new List<Vector2Int>();
            int[] selectedIDs = this.selectedIDs;
            for (int i = 0; i < selectedIDs.Length; i++)
            {
                for (int j = i+1; j < selectedIDs.Length; j++)
                {
                    bonds.Add(new Vector2Int(selectedIDs[i], selectedIDs[j]));
                }
            }
            foreach (var bond in bonds)
            {
                this.bonds.Add(bond);
            }
            InitializeButtons();
            foreach (var toggle in toggles)
            {
                if (toggle && toggle.isOn) toggle.isOn = false;
            }
        }

        public void Initialize(Vector2Int[] vector2Ints = null, Action<Vector2Int[]> onClose = null)
        {
            if(vector2Ints != null) bonds = new HashSet<Vector2Int>(vector2Ints);
            if(onClose != null) window.OnClose.AddListener(() => { onClose(selectedBonds); });

            InitializeButtons();
        }

        public void InitializeButtons()
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
                    InitializeButtons();
                });
        }

        public void Save()
        {
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = saveFileDialog.FileName;

            SaveData saveData = new SaveData(bonds);
            string json = JsonUtility.ToJson(saveData,true);

            File.WriteAllText(fileName, json);
        }

        public void Load()
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;

            string json = File.ReadAllText(fileName);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            bonds = new HashSet<Vector2Int>(saveData.bonds);
            InitializeButtons();
        }

        [System.Serializable]
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