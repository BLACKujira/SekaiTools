using SekaiTools.Spine;
using SekaiTools.UI.BackGround;
using System.Collections;
using UnityEngine;

namespace SekaiTools.UI.SpineAniShowPlayer
{
    //public class SpineAniShowPlayer : MonoBehaviour
    //{
    //    public TextAsset json;
    //    [Header("Components")]
    //    public SpineControllerTypeA spineControllerPrefab;
    //    public Transition01 transition01;
    //    public Transition01_CustomColor transition01_CustomColor;
    //    [Header("Settings")]
    //    public InbuiltSpineModelSet spineModelSet;
    //    public BackGroundPart bgpSpinePrefab;
    //    public float whiteFadeTime = 2f;
    //    public float holdTime = 15f;
    //    public HDRColorSet transitionColor;
    //    [Header("Message")]
    //    public MessageLayer.MessageLayerBase messageLayer;

    //    [System.NonSerialized] public SpineControllerTypeA spineController;
    //    [System.NonSerialized] public SpineAniShowData spineAniShowData;

    //    public BackGroundController backGroundController => BackGroundController.backGroundController;

    //    int currentID = 0;

    //    private void Awake()
    //    {
    //        spineController = Instantiate(spineControllerPrefab);

    //        SpineAniShowData spineAniShowData = JsonUtility.FromJson<SpineAniShowData>(File.ReadAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\255\save.sas"));
    //        spineAniShowData.savePath = @"C:\Users\KUROKAWA_KUJIRA\Desktop\255\save.sas";
    //        Initialize(spineAniShowData);

    //        Play();
    //    }

    //    private void Initialize(SpineAniShowData spineAniShowData)
    //    {
    //        this.spineAniShowData = spineAniShowData;
    //    }

    //    public void ShowScene(int index)
    //    {
    //        spineController.ClearModel();
    //        foreach (var spineObject in spineAniShowData.spineScenes[index].spineObjects)
    //        {
    //            SpineControllerTypeA.ModelPair modelPair = spineController.AddModel(spineModelSet.GetValue(spineObject.atlasAssetName));
    //            SpineControllerTypeA.SetModel(spineObject, modelPair);
    //        }
    //        backGroundController.ClearAndReset();
    //        backGroundController.Load(spineAniShowData.spineScenes[index].backGroundData);
    //        backGroundController.AddDecoration(bgpSpinePrefab, 1);
    //    }

    //    private void OnDestroy()
    //    {
    //        Destroy(spineController);
    //    }

    //    public void Play()
    //    {
    //        StartCoroutine(IPlay());
    //    }

    //    IEnumerator IPlay()
    //    {
    //        yield return new WaitForSeconds(10);

    //        int count = spineAniShowData.spineScenes.Count;
    //        ShowScene(0);
    //        for (int i = 0; i < count; i++)
    //        {
    //            yield return new WaitForSeconds(holdTime - whiteFadeTime / 2);
    //            if(i!=count-1)
    //            {
    //                if(i%2==0)
    //                {
    //                    transition01_CustomColor.hdrColor = transitionColor.colors[i / 2 + 2];
    //                    transition01_CustomColor.StartTransition(() => { ShowScene(i + 1); });
    //                }
    //                else
    //                {
    //                    transition01.StartTransition(() => { ShowScene(i + 1); });
    //                }
    //                yield return new WaitForSeconds(whiteFadeTime);
    //            }
    //        }
    //    }
    //}

    public class SpineAniShowPlayer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public SpineAniShowPlayer_Player player;

        BackGroundController.BackGroundSaveData backGroundSaveData = null;

        public void Initialize(SpineAniShowData spineAniShowData)
        {
            backGroundSaveData = new BackGroundController.BackGroundSaveData(BackGroundController.backGroundController);
            window.OnClose.AddListener(
                () =>
                {
                    BackGroundController.backGroundController.Load(backGroundSaveData);
                });
            player.Initialize(spineAniShowData);
            StartCoroutine(WaitCoroutine());
        }

        IEnumerator WaitCoroutine()
        {
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return 1;
            }
            player.Play();
        }
    }
}