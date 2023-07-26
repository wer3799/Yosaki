using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiSealSwordCollectionView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI equipmentName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private SealSwordData tableData;

    private WeaponServerData serverData;

    [SerializeField]
    private GameObject notHasObject;

    [SerializeField]
    private Image equipmentIcon;

    [SerializeField]
    private Image reward0Icon;
    [SerializeField]
    private Image reward1Icon;

    [SerializeField]
    private TextMeshProUGUI reward0Value;
    [SerializeField]
    private TextMeshProUGUI reward1Value;

    [SerializeField]
    private Button reward0Button;
    [SerializeField]
    private Button reward1Button;

    [SerializeField]
    private TextMeshProUGUI reward0Description;
    [SerializeField]
    private TextMeshProUGUI reward1Description;


    public void Initialize(SealSwordData data)
    {

        this.tableData = data;

        this.serverData = ServerData.sealSwordServerTable.TableDatas[data.Stringid];

        equipmentName.SetText($"{data.Name}");
        
        equipmentIcon.sprite = CommonResourceContainer.GetSealSwordIconSprite(data.Id);        

        reward0Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)data.Rewardtype0);
        reward1Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)data.Rewardtype1);

        reward0Value.SetText(Utils.ConvertBigNum(data.Rewardvalue0));
        reward1Value.SetText(Utils.ConvertBigNum(data.Rewardvalue1));
        

        Subscribe();
    }

    private void Subscribe()
    {
        serverData.hasItem.AsObservable().Subscribe(e =>
        {
            notHasObject.SetActive(e == 0);

            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);
        serverData.getReward0.AsObservable().Subscribe(e =>
        {
            bool hasReward = e == 1;

            reward0Button.interactable = !hasReward;

            reward0Description.SetText(!hasReward ? "보상수령" : "수령완료");
        }).AddTo(this);
        serverData.getReward1.AsObservable().Subscribe(e =>
        {
            bool hasReward = e == 1;

            reward1Button.interactable = !hasReward;

            reward1Description.SetText(!hasReward ? "보상수령" : "수령완료");
        }).AddTo(this);


    }

    public void OnClickGetRewardFreeButton()
    {
        if (serverData.getReward0.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        serverData.getReward0.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        //재화 획득
        Item_Type rewardType = (Item_Type)tableData.Rewardtype0;

        float rewardValue = tableData.Rewardvalue0;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));


        // 보상 획득 
        Param equipmentParam = new Param();        
        string updateValue = ServerData.sealSwordServerTable.TableDatas[tableData.Stringid].ConvertToString();
        equipmentParam.Add(tableData.Stringid, updateValue);

        transactions.Add(TransactionValue.SetUpdate(SealSwordServerTable.tableName, SealSwordServerTable.Indate, equipmentParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
    public void OnClickGetRewardAdButton()
    {
        if (serverData.getReward1.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (ServerData.iapServerTable.TableDatas[UiEquipmentCollectionPassBuyButton.collectionPassKey].buyCount.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("도감 패스권이 필요합니다.");
            return;
        }

        serverData.getReward1.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        //재화 획득
        Item_Type rewardType = (Item_Type)tableData.Rewardtype1;

        float rewardValue = tableData.Rewardvalue1;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));


        // 보상 획득 
        Param equipmentParam = new Param();        
        string updateValue = ServerData.sealSwordServerTable.TableDatas[tableData.Stringid].ConvertToString();
        equipmentParam.Add(tableData.Stringid, updateValue);

        transactions.Add(TransactionValue.SetUpdate(SealSwordServerTable.tableName, SealSwordServerTable.Indate, equipmentParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
}
