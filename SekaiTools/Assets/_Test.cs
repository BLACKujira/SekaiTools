using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools;
using SekaiTools.Live2D;
using System.IO;
using SekaiTools.Cutin;
using SekaiTools.UI.CutinSceneEditor;
using Spine.Unity;
using SekaiTools.UI.SpineLayer;
using SekaiTools.UI.BackGround;
using SekaiTools.Spine;
using static SekaiTools.UI.BackGround.BackGroundController;
using SekaiTools.Count;
using SekaiTools.UI.Radio;
using SekaiTools.UI;
using SekaiTools.StringConverter;
using System;
using SekaiTools.DecompiledClass;

public class _Test : MonoBehaviour
{
    //private void Awake1()
    //{
    //    Radio_MusicLayer player = new Radio_MusicLayer();
    //    player.musicNames = new StringConverter_MusicName(
    //        CSVTools.LoadCSV(
    //            File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\CSV\MusicNameCSV.txt"),
    //            ",",Environment.NewLine));
    //    player.singerNames = new StringConverter_SingerName(
    //        CSVTools.LoadCSV(
    //            File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\CSV\CharNameCSV.txt"),
    //            ",", Environment.NewLine),
    //        CSVTools.LoadCSV(
    //            File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\CSV\OutsideCharVocalCSV.txt"),
    //            ",", Environment.NewLine));
    //    player.cpNames = new StringConverter_StringAlias(
    //        CSVTools.LoadCSV(
    //            File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\CSV\CPnameCSV.txt"),
    //            ",", Environment.NewLine));
    //    player.vocalTypeNames = new StringConverter_StringAlias(
    //        CSVTools.LoadCSV(
    //            File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\CSV\VocalTypeCSV.txt"),
    //            ",", Environment.NewLine));
    //    player.sizeTypeNames = new StringConverter_StringAlias(
    //        CSVTools.LoadCSV(
    //            File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\CSV\SizeTypeCSV.txt"),
    //            ",", Environment.NewLine));

    //    player.masterMusics = JsonHelper.getJsonArray<MasterMusic>(
    //        File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Documents\SekaiTools\sekai_master_db_diff\musics.json"));
    //    player.masterMusicVocals = JsonHelper.getJsonArray<MasterMusicVocal>(
    //        File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Documents\SekaiTools\sekai_master_db_diff\musicVocals.json"));

    //    player.Initialize();
    //    player.LoadMusicInSV(@"C:\Users\KUROKAWA_KUJIRA\Documents\SekaiTools\assets\music\long",
    //        JsonHelper.getJsonArray<MasterOutsideCharacter>(
    //            File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Documents\SekaiTools\sekai_master_db_diff\outsideCharacters.json")),
    //        ".mp3");

    //    MusicVocalData music1 = player.GetVocal(new MusicOrderInfo("raddogs", "ankh", null));
    //    MusicVocalData music2 = player.GetVocal(new MusicOrderInfo("スイートマジック",null,null));
    //}

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
