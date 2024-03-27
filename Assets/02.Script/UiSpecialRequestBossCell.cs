using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSpecialRequestBossCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private SkeletonGraphic costumeGraphic;

    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI hpText;

    [SerializeField] private List<Image> stars = new List<Image>();

    private SpecialRequestBossTableData tableData;

    [SerializeField] private Sprite starOn;
    [SerializeField] private Sprite starOff;

    private int cellIdx = 0;
    public void Initialize(SpecialRequestBossTableData _tableData, int idx=0)
    {
        tableData = _tableData;

        cellIdx = idx;
        titleText.SetText(tableData.Bossname);
        
        costumeGraphic.Clear();

        costumeGraphic.gameObject.SetActive(true);
        costumeGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[tableData.Costumeid];
        costumeGraphic.Initialize(true);
        costumeGraphic.SetMaterialDirty();

        gradeText.SetText($"{idx+1}단계");
        
        var data = Utils.GetCurrentSeasonSpecialRequestData();
        
        var score = ServerData.specialRequestBossServerTable.TableDatas[data.Stringid[cellIdx]].score.Value;
        
        var hp= data.Specialrequestbosshp[cellIdx];

        hpText.SetText($"HP : {Utils.ConvertNum(hp)}");
        for (var i = 0; i < stars.Count; i++)
        {
            stars[i].sprite = i > score ? starOff : starOn;
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "도전 할까요?", () =>
        {
            //0~19
            GameManager.Instance.SetBossId(tableData.Id);
            //0~9
            GameManager.Instance.SetSpecialRequestBossId(cellIdx);
            GameManager.Instance.LoadContents(GameManager.ContentsType.SpecialRequestBoss);
        }, () => { });
    }
}
