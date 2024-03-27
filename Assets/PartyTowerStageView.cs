using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyTowerStageView : MonoBehaviour
{
    private TwoCaveData tableData;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI gradeDescription;

    public void Initialize(TwoCaveData tableData)
    {
        this.tableData = tableData;

        gradeDescription.SetText($"{tableData.Id + 1}단계");

        description.SetText($"관문체력 : {Utils.ConvertNum(this.tableData.Firstbosshp)}\n보스체력 : {Utils.ConvertNum(this.tableData.Lastbosshp)}");
    
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{tableData.Id + 1}단계 입장 하시겠습니까?", () =>
        {

            PartyRaidManager.Instance.NetworkManager.MakePartyTowerRoom2(2, tableData.Id);

            UiPartyTowerStageSelectBoard.Instance.gameObject.SetActive(false);

        }, null);
    }
}
