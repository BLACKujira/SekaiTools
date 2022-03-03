using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools;
using SekaiTools.Live2D;
using System.IO;
using SekaiTools.Cutin;
using SekaiTools.UI.CutinSceneEditor;

public class _Test : MonoBehaviour
{
    public ModelLoader modelLoader;
    public CutinSceneEditor cutinSceneEditor;
    //public CutinScenePlayer cutinScenePlayer;
    //public TextAsset dataJson;

    private void Start()
    {
        StartCoroutine(MainProcess());
    }

    IEnumerator MainProcess()
    {
        AudioData audioData = new AudioData();
        yield return audioData.LoadDatas(Directory.GetFiles(@"D:\Project_Unity\SekaiTalkCounter\Assets\Cutin\wav"));

        string[] dir = Directory.GetDirectories(@"C:\Users\KUROKAWA_KUJIRA\Desktop\1");
        foreach (var d in dir)
        {
            string[] files = Directory.GetFiles(d);
            foreach (var file in files)
            {
                if(file.EndsWith("model3.json"))
                {
                    modelLoader.LoadModel(file);
                }
            }
        }

        //cutinScenePlayer.l2DController.SetModels(modelLoader);
        cutinSceneEditor.audioData = audioData;
        //cutinScenePlayer.cutinSceneData = JsonUtility.FromJson<CutinSceneData>(dataJson.text);
    }
}
