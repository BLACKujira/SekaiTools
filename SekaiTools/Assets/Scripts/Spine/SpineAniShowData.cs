using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.Spine
{
    public class SpineAniShowData:ISaveData
    {
        public List<SpineScene> spineScenes = new List<SpineScene>();

        public static string bgs_lnSekai = "{\"spritePath\":\"Built-in\",\"backGround\":{\"name\":\"area5\",\"serializedModifiers\":[]},\"parts\":[{\"name\":\"area5\",\"serializedModifiers\":[]},{\"name\":\"area5\",\"serializedModifiers\":[]}]}";
        public static string bgs_mmjSekai = "{\"spritePath\":\"Built-in\",\"backGround\":{\"name\":\"area7\",\"serializedModifiers\":[]},\"parts\":[{\"name\":\"area7\",\"serializedModifiers\":[]},{\"name\":\"area7\",\"serializedModifiers\":[]}]}";
        public static string bgs_vbsSekai = "{\"spritePath\":\"Built-in\",\"backGround\":{\"name\":\"area8\",\"serializedModifiers\":[]},\"parts\":[{\"name\":\"area8\",\"serializedModifiers\":[]},{\"name\":\"area8\",\"serializedModifiers\":[]}]}";
        public static string bgs_wsSekai = "{\"spritePath\":\"Built-in\",\"backGround\":{\"name\":\"area9\",\"serializedModifiers\":[]},\"parts\":[{\"name\":\"area9\",\"serializedModifiers\":[]},{\"name\":\"area9\",\"serializedModifiers\":[]}]}";
        public static string bgs_25Sekai = "{\"spritePath\":\"Built-in\",\"backGround\":{\"name\":\"area10\",\"serializedModifiers\":[]},\"parts\":[{\"name\":\"area10\",\"serializedModifiers\":[]},{\"name\":\"area10\",\"serializedModifiers\":[]}]}";

        public string savePath { get; set; }

        public void SaveData()
        {
            string json = JsonUtility.ToJson(this,true);
            File.WriteAllText(savePath,json);
        }
    }
}