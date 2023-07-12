using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class UiQuickMoveBoard : MonoBehaviour
{
    [SerializeField]
    private UiStageCell stageCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiStageCell> stageCellList = new List<UiStageCell>();

    private ReactiveProperty<int> currentPresetId = new ReactiveProperty<int>(1);

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private Button allReceiveButton;

    void Start()
    {
        Subscribe();

        SetMyStageInfo();
    }

    private void SetMyStageInfo()
    {
        int myLastStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].Value;

        if (myLastStageId == -1)
        {
            myLastStageId = 0;
        }
        else
        {
            myLastStageId++;
        }

        int lastStageId = TableManager.Instance.GetLastStageIdx();

        if (myLastStageId >= lastStageId)
        {
            myLastStageId = lastStageId;
        }

        var stageTableData = TableManager.Instance.StageMapData[myLastStageId];
        currentPresetId.Value = stageTableData.Chapteridx;

        RefreshStage(currentPresetId.Value);
    }

    private void Subscribe()
    {
        currentPresetId.AsObservable().Subscribe(e =>
        {
            titleText.SetText($"{e}단계");

            int lastPreset = TableManager.Instance.GetLastStagePreset();

            rightButton.interactable = e != lastPreset;
            leftButton.interactable = e != 1;
        }
        ).AddTo(this);
    }
    public void RefreshStage(int mapPreset)
    {
        var stageDatas = TableManager.Instance.StageMapTable.dataArray;

        var selectedPresets = stageDatas.ToList().Where(e => e.Chapteridx == mapPreset).Select(cell => cell).ToList();

        int makeCount = selectedPresets.Count - stageCellList.Count;

        for (int i = 0; i < makeCount; i++)
        {
            stageCellList.Add(Instantiate<UiStageCell>(stageCellPrefab, cellParent));
        }

        for (int i = 0; i < stageCellList.Count; i++)
        {
            if (i < selectedPresets.Count)
            {
                stageCellList[i].gameObject.SetActive(true);
                stageCellList[i].Initialize(selectedPresets[i]);
            }
            else
            {
                stageCellList[i].gameObject.SetActive(false);
            }
        }

    }

    public void OnClickLeftButton()
    {
        if (currentPresetId.Value == 1) return;
        currentPresetId.Value--;
        RefreshStage(currentPresetId.Value);
    }

    public void OnClickRightButton()
    {
        int lastThema = TableManager.Instance.GetLastStagePreset();
        if (currentPresetId.Value == lastThema) return;
        currentPresetId.Value++;
        RefreshStage(currentPresetId.Value);
    }

    public void OnClickAllReceiveButton()
    {
        allReceiveButton.interactable = false;

        //값 가져올때 -1로 가져와서 +1로 보정
        int lastClearData = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value + 1;
        var tableData = TableManager.Instance.StageMapTable.dataArray;
        //현재 index가 -1이라면 0번째 보상을 받아야하기 때문에 +1
        var passValue = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassFree).Value + 1;
        var adValue = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassAd).Value + 1;

        //스테이지 패스
        if (ServerData.iapServerTable.TableDatas[UiStagePassBuyButton.stagePassKey].buyCount.Value > 0)
        {
            //내가 안받은 보상(value+1)부터 현재 스테이지까지.
            for (int i = passValue; i < lastClearData; i++)
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Pre_Bossrewardtype, tableData[i].Pre_Bossrewardvalue);
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassFree).Value++;
            }
        }

        //광고
        if (AdManager.Instance.HasRemoveAdProduct())
        {
            for (int i = adValue; i < lastClearData; i++)
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Ad_Bossrewardtype, tableData[i].Ad_Bossrewardvalue);
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassAd).Value++;
            }
        }


        for (int i = 0; i < stageCellList.Count; i++)
        {
            stageCellList[i].Subscribe();
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfo2Param = new Param();
        userInfo2Param.Add(UserInfoTable_2.stagePassAd, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.stagePassAd].Value);
        userInfo2Param.Add(UserInfoTable_2.stagePassFree, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.stagePassFree].Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            allReceiveButton.interactable = true;
            PopupManager.Instance.ShowAlarmMessage("처리 성공!(광고 보상은 광고제거가 있어야 수령 됩니다!)");
        });
    }

}
