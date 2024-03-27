using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiYorinSpecialMissionCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionTitle;
    [SerializeField] private TextMeshProUGUI missionDesc;

    [SerializeField] private ItemView expView;
    [SerializeField] private ItemView goodsView;

    [SerializeField] private Button button;
    private YorinSpecialMissionData tableData;
    private CompositeDisposable disposable = new CompositeDisposable();

    public void Initialize(YorinSpecialMissionData _tableData)
    {
        tableData = _tableData;
        
        missionTitle.SetText(tableData.Title);
        
        missionDesc.SetText(tableData.Description);
        
        expView.Initialize(Item_Type.Exp,tableData.Exp);
        
        goodsView.Initialize((Item_Type)tableData.Reward1, tableData.Reward1_Value);

        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        disposable.Clear();
    }

    private void Subscribe()
    {
        disposable.Clear();
        
        if (tableData == null) return;

        ServerData.yorinSpecialMissionServerTable.TableDatas[tableData.Stringid].clearCount
            .AsObservable()
            .Subscribe(e=>
            {
                button.interactable =tableData.Rewardrequire <= e ;
            })
            .AddTo(disposable);
        ServerData.yorinSpecialMissionServerTable.TableDatas[tableData.Stringid].rewardCount
            .AsObservable()
            .Subscribe(e=>
            {
                button.gameObject.SetActive(e < 1);
            })
            .AddTo(disposable);

    }
    
    public void OnClickButton()
    {
        if (ServerData.yorinSpecialMissionServerTable.CheckMissionRewardCount(tableData.Stringid) > 0)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Has);
            return;
        }

        if (ServerData.yorinSpecialMissionServerTable.TableDatas[tableData.Stringid].clearCount.Value <
            tableData.Rewardrequire)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Can);
            return;
        }
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param missionParam = new Param();
        Param goodsParam = new Param();
        UiRewardResultPopUp.Instance.Clear();
        UiRewardResultPopUp.Instance.RewardList.Add(new RewardData(Item_Type.Exp,(float)tableData.Exp));
        UiRewardResultPopUp.Instance.RewardList.Add(new RewardData((Item_Type)tableData.Reward1,(float)tableData.Reward1_Value));
        
        ServerData.yorinSpecialMissionServerTable.TableDatas[tableData.Stringid].rewardCount.Value = 1;
        missionParam.Add(tableData.Stringid, ServerData.yorinSpecialMissionServerTable.TableDatas[tableData.Stringid].ConvertToString());
        transactions.Add(TransactionValue.SetUpdate(YorinSpecialMissionServerTable.tableName, YorinSpecialMissionServerTable.Indate, missionParam));


        var goods = ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Reward1);
        ServerData.goodsTable.GetTableData(goods).Value += tableData.Reward1_Value;
        goodsParam.Add(goods, ServerData.goodsTable.GetTableData(goods).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            GrowthManager.Instance.GetExpBySleep((float)tableData.Exp);
            UiRewardResultPopUp.Instance
                .Show()
                .Clear();
        });
    }

}
