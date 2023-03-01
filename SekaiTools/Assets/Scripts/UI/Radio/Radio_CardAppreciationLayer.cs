using DG.Tweening;
using SekaiTools.DecompiledClass;
using SekaiTools.StringConverter;
using SekaiTools.UI.BackGround;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static SekaiTools.UI.Radio.Radio_MusicLayer;

namespace SekaiTools.UI.Radio
{

    public class Radio_CardAppreciationLayer : Radio_OptionalLayer
    {
        [Header("Components")]
        public GraphicColorMapping bgCardName;
        public Text textCardName;
        public ContentSizeFitter contentSizeFitterCardName;
        [Header("Prefab")]
        public BackGroundRoot switchableImagePrefab;

        BGModifier_SwitchableImage switchableImage;
        BackGroundController.BackGroundSaveData prevBGSaveData;

        float switchingTime;
        float minimumSwitchingTime;

        MasterCard[] masterCards;

        CardData[] cardDatas;
        CardData[] currentCardDataSet;
        List<CardData> unusedCardDataInSet;

        float fadeTime = 1;

        private void OnDestroy()
        {
            if(enableLayer) BackGroundController.backGroundController.Load(prevBGSaveData);
        }

        public void Initialize(Settings settings)
        {
            base.Initialize(settings);

            if (enableLayer)
            {
                prevBGSaveData = new BackGroundController.BackGroundSaveData(BackGroundController.backGroundController);
                BackGroundController.backGroundController.ChangeBackGround(switchableImagePrefab);
                switchableImage = BackGroundController.backGroundController.BackGround.mainPart.GetModifier<BGModifier_SwitchableImage>();

                masterCards = settings.masterCards;
                switchingTime = settings.switchingTime;
                minimumSwitchingTime = settings.minimumSwitchingTime;

                LoadCardImages(settings.cardImageFolder, settings.extensions, settings.displayCardRarities);
                currentCardDataSet = new List<CardData>(cardDatas).ToArray();
                unusedCardDataInSet = new List<CardData>(cardDatas);
                
                radio.musicLayer.onMusicChange += (music) =>
                  {
                      HashSet<CardData> usedCardSet = new HashSet<CardData>(currentCardDataSet);
                      usedCardSet.ExceptWith(this.unusedCardDataInSet);

                      currentCardDataSet = GetCardDataSet(music);
                      HashSet<CardData> unusedCardDataInSet = new HashSet<CardData>(currentCardDataSet);
                      unusedCardDataInSet.ExceptWith(usedCardSet);
                      this.unusedCardDataInSet = new List<CardData>(unusedCardDataInSet);
                  };

                radio.OnThemeChange += (theme) => bgCardName.Color = theme.color_UI; 

                Play();
            }
        }

        void LoadCardImages(string folder, string[] extensions , CardRarityType[] displayCardRarities)
        {
            HashSet<CardRarityType> displayCardRaritySet = new HashSet<CardRarityType>(displayCardRarities);
            List<CardData> cardDatas = new List<CardData>();
            string[] folders = Directory.GetDirectories(folder);
            string GetFolder(MasterCard masterCard)
            {
                foreach (var f in folders)
                {
                    if (Path.GetFileName(f).StartsWith(masterCard.assetbundleName))
                        return f;
                }
                return null;
            }
            foreach (var masterCard in masterCards)
            {
                if (!displayCardRaritySet.Contains(masterCard.RarityType))
                    continue;
                string dir = GetFolder(masterCard);
                if (string.IsNullOrEmpty(dir)) continue;
                foreach (var extension in extensions)
                {
                    string pathNormal = Path.Combine(dir, "card_normal" + extension);
                    if (File.Exists(pathNormal))
                    {
                        cardDatas.Add(new CardData(masterCard, false, pathNormal));
                        break;
                    }
                }
                foreach (var extension in extensions)
                {
                    string pathAT = Path.Combine(dir, "card_after_training" + extension);
                    if (File.Exists(pathAT))
                    {
                        cardDatas.Add(new CardData(masterCard, true, pathAT));
                        break;
                    }
                }
            }
            this.cardDatas = cardDatas.ToArray();
        }

        CardData[] GetCardDataSet(MusicInQueue musicInQueue)
        {
            HashSet<MusicTag> unitMusicTags = new HashSet<MusicTag>()
            {
                MusicTag.light_music_club,
                MusicTag.idol,
                MusicTag.street,
                MusicTag.theme_park,
                MusicTag.school_refusal
            };
            List<CardData> filteredCardDatas = new List<CardData>();
            HashSet<int> characters = new HashSet<int>();
            foreach (var singer in musicInQueue.vocalData.singers)
            {
                int id = ConstData.NameToId(singer);
                if (id > 0 && id < 27)
                    characters.Add(id);
            }
            foreach (var cardData in cardDatas)
            {
                if(characters.Contains(cardData.masterCard.characterId))
                {
                    MusicTag topPriorityTag = musicInQueue.musicData.musicTag.TopPriorityTag;
                    if (cardData.masterCard.characterId > 20 &&
                        unitMusicTags.Contains(topPriorityTag))
                    {
                        if(ConstData.MusicTagEqualsUnitType(topPriorityTag,cardData.masterCard.SupportUnitType))
                            filteredCardDatas.Add(cardData);
                    }
                    else
                    {
                        filteredCardDatas.Add(cardData);
                    }
                }
            }

            if (filteredCardDatas.Count == 0)
                filteredCardDatas = new List<CardData>(cardDatas);
            return filteredCardDatas.ToArray();
        }

        public new class Settings : Radio_OptionalLayer.Settings
        {
            public float switchingTime;
            public float minimumSwitchingTime;

            public string cardImageFolder;
            public string[] extensions;
            
            public MasterCard[] masterCards;

            public CardRarityType[] displayCardRarities;
        }

        void Play()
        {
            StartCoroutine(IPlay());
        }

        IEnumerator IPlay()
        {
            while (true)
            {
                if (unusedCardDataInSet.Count == 0)
                    unusedCardDataInSet = new List<CardData>(currentCardDataSet);

                int rdmIdx = Random.Range(0, unusedCardDataInSet.Count);
                CardData cardData = unusedCardDataInSet[rdmIdx];
                unusedCardDataInSet.RemoveAt(rdmIdx);
                ImageData imageData = new ImageData();
                yield return imageData.LoadFile(cardData.imageFilePath);

                DOTween.To(() => bgCardName.Alpha, (value) => bgCardName.Alpha = value, 0, fadeTime);
                textCardName.DOFade(0, fadeTime);

                yield return new WaitForSeconds(fadeTime);

                CardRarityType rarityType = cardData.masterCard.RarityType;
                string rarityStr;

                switch (rarityType)
                {
                    case CardRarityType.rarity_1:
                        rarityStr = "★1";
                        break;
                    case CardRarityType.rarity_2:
                        rarityStr = "★2";
                        break;
                    case CardRarityType.rarity_3:
                        rarityStr = "★3";
                        break;
                    case CardRarityType.rarity_4:
                        rarityStr = "★4";
                        break;
                    case CardRarityType.rarity_birthday:
                        rarityStr = "birthday";
                        break;
                    case CardRarityType.max:
                        rarityStr = "max";
                        break;
                    default:
                        rarityStr = "未知稀有度";
                        break;
                }

                if (rarityType == CardRarityType.rarity_3||rarityType == CardRarityType.rarity_4)
                    textCardName.text = $"{ConstData.characters[cardData.masterCard.characterId].Name.Replace(" ", "")} {rarityStr} {cardData.masterCard.prefix} {(cardData.afterTraining?"特训后":"特训前")}";
                else
                    textCardName.text = $"{ConstData.characters[cardData.masterCard.characterId].Name.Replace(" ", "")} {rarityStr} {cardData.masterCard.prefix}";
                yield return 1;
                contentSizeFitterCardName.enabled = false;
                contentSizeFitterCardName.enabled = true;
                DOTween.To(() => bgCardName.Alpha, (value) => bgCardName.Alpha = value, 1, fadeTime);
                textCardName.DOFade(1, fadeTime);

                switchableImage.ChangeImage(imageData.ValueArray[0]).OnCompleteFadeOut((so,sn)=>
                {
                    if (so)
                    {
                        Texture2D texture = so.texture;
                        Destroy(so);
                        Destroy(texture);
                    }
                });

                float leftTime = radio.musicLayer.audioSource.clip.length - radio.musicLayer.audioSource.time;
                if(leftTime > switchingTime)
                {
                    yield return new WaitForSeconds(switchingTime);
                    leftTime = radio.musicLayer.audioSource.clip.length - radio.musicLayer.audioSource.time;
                    if (leftTime < minimumSwitchingTime)
                        yield return radio.musicLayer.waitForMusicChanged;
                }
                else
                {
                    yield return radio.musicLayer.waitForMusicChanged;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}