using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static UiRewardView;
public class UiTwelveBossContentsView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject lockObject;

    [SerializeField]
    private Image bossIcon;

    private TwelveBossTableData bossTableData;

    [SerializeField]
    private Button enterButton;

    [SerializeField]
    private GameObject buttons;

    [SerializeField]
    private bool initByInspector = false;

    [SerializeField]
    private int bossId = 0;

    [SerializeField]
    private SkeletonGraphic costumeGraphic;

    [SerializeField]
    private SkeletonGraphic petGraphic;
    [SerializeField]
    private Image image0;
    [SerializeField]
    private Image image1;

    [SerializeField] private GameObject allClearMark; 
    
    private void Start()
    {
        InitByInspector();
        if (initByInspector)
        {
            Subscribe();
        }
    }

    private void Subscribe()
    {
        ServerData.bossServerTable.TableDatas[bossTableData.Stringid].rewardedId.AsObservable().Subscribe(e =>
        {
            OnOffAllClearMark();
        }).AddTo(this);
        
    }
    
    private void InitByInspector()
    {
        if (initByInspector)
        {
            Initialize(TableManager.Instance.TwelveBossTable.dataArray[bossId]);
        }
    }

    public void Initialize(TwelveBossTableData bossTableData)
    {
        this.bossTableData = bossTableData;

        if (title != null)
        {
            title.SetText(bossTableData.Name);
        }

        var score = ServerData.bossServerTable.TableDatas[bossTableData.Stringid].score.Value;
        if (string.IsNullOrEmpty(score) == false)
        {
            description.SetText($"최고 피해량 : {Utils.ConvertBigNum(double.Parse(score))}");
        }
        else
        {
            description.SetText("기록 없음");
        }

        lockObject.SetActive(bossTableData.Islock);
        buttons.SetActive(bossTableData.Islock == false);

        if (bossTableData.Id>=124&&bossTableData.Id<=131)
        {
            bossIcon.sprite = CommonResourceContainer.GetDarkIconSprite(bossTableData.Id - 124);
        }
        else if (bossTableData.Id < CommonUiContainer.Instance.bossIcon.Count)
        {
            bossIcon.sprite = CommonUiContainer.Instance.bossIcon[bossTableData.Id];
        }
        //수호동물
        if (bossTableData.Id >= 190 && bossTableData.Id <= 199)
        {
            var suho0 = bossTableData.Displayskeletondata[0];
            var suho1 = bossTableData.Displayskeletondata[1];
            
            image0.sprite=CommonResourceContainer.GetSuhoAnimalSprite(suho0);
            image1.sprite=CommonResourceContainer.GetSuhoAnimalSprite(suho1);

        }
        else
        {
            var costume = bossTableData.Displayskeletondata[0];
            if (costume != -1)
            {
                costumeGraphic.Clear();

                petGraphic.gameObject.SetActive(false);
                costumeGraphic.gameObject.SetActive(true);
                costumeGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[costume];
                costumeGraphic.Initialize(true);
                costumeGraphic.SetMaterialDirty();
            }
        
            var pet = bossTableData.Displayskeletondata[1];
            if (pet != -1)
            {
                costumeGraphic.gameObject.SetActive(false);
                petGraphic.gameObject.SetActive(true);
                petGraphic.Clear();
                petGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[pet];
                petGraphic.Initialize(true);
                petGraphic.SetMaterialDirty();
            }
        }


        OnOffAllClearMark();
    }

    private void OnOffAllClearMark()
    {
        if (allClearMark != null)
        {
            var rewardTotalCount = TableManager.Instance.TwelveBossTable.dataArray[bossId].Rewardtype.Length;

            var rewardedListTotalCount = ServerData.bossServerTable.GetBossRewardedIdxList(bossTableData.Stringid).Count;

            allClearMark.SetActive(rewardedListTotalCount >= rewardTotalCount);

        }
    }
    
    public void OnClickRewardButton()
    {
        UiTwelveRewardPopup.Instance.Initialize(bossTableData.Id);

        UiContentsEnterPopup.Instance.transform.SetAsLastSibling();
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "도전 할까요?", () =>
        {
            enterButton.interactable = false;
            GameManager.Instance.SetBossId(bossTableData.Id);
            GameManager.Instance.LoadContents(GameManager.ContentsType.TwelveDungeon);
        }, () => { });
    }
}
