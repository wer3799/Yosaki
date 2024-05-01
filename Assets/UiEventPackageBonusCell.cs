using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiEventPackageBonusCell : ItemView
{
    private int id;
    private int require;
    private EventPackageBonusData data;

    [SerializeField] private GameObject lockMaskObject;
    [SerializeField] private GameObject rewardedObject;
    [SerializeField] private GameObject isCostumeTextObject;
    [SerializeField] private TextMeshProUGUI requireText;

    public void Initialize(EventPackageBonusData _data)
    {
        data = _data;
        
        id = data.Id;
        require = data.Requirebuycount;
        var itemType = (Item_Type)data.Rewardtype;
        Initialize(itemType,data.Rewardvalue);

        isCostumeTextObject.SetActive(itemType.IsCostumeItem());
        
        Subscribe();
    }

    private void Subscribe()
    {
        IAPManager.Instance.WhenBuyComplete.AsObservable()
            .Throttle(TimeSpan.FromSeconds(0.5f))
            .Subscribe(e =>
            {
                SetUi(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventPackageRewardIdx).Value);
            }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventPackageRewardIdx).AsObservable().Subscribe(SetUi).AddTo(this);
    }

    private void SetUi(double idx)
    {
        lockMaskObject.SetActive(CanGetReward()==false);
        rewardedObject.SetActive(idx >= id);
        requireText.SetText($"{require}회");
    }

    private bool GetBeforeReward()
    {
        return id == (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventPackageRewardIdx).Value + 1;
    }

    private int GetEventPackageBuyCount()
    {
        var sum = 0;
        sum += (int)ServerData.iapServerTable.TableDatas["childpackage0"].buyCount.Value;
        sum += (int)ServerData.iapServerTable.TableDatas["childpackage1"].buyCount.Value;
        sum += (int)ServerData.iapServerTable.TableDatas["childpackage2"].buyCount.Value;
        
        return sum;
    }

    private bool CanGetReward()
    {
        return require <= GetEventPackageBuyCount();
    }
    
    public void OnClickButton()
    {
        if (GetBeforeReward()==false)
        {
            PopupManager.Instance.ShowAlarmMessage("이전 보상을 수령해주세요!");
            return;
        }
        if (CanGetReward()==false)
        {
            PopupManager.Instance.ShowAlarmMessage("조건이 부족합니다.");
            return;
        }

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventPackageRewardIdx).Value = id;

        List<TransactionValue> transactions = new List<TransactionValue>();
        
        var type = (Item_Type)data.Rewardtype;
        
        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (float)data.Rewardvalue));

        Param userInfo2Param = new Param();
        
        userInfo2Param.Add(UserInfoTable_2.eventPackageRewardIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventPackageRewardIdx).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        
        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("어린이날 한정 보너스 보상 획득!");
        });
    }
}
