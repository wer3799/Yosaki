using System.Collections;
using System.Collections.Generic;
using BackEnd;
using GoogleMobileAds.Api;
using Newtonsoft.Json;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiSpecialRequestSpecialRewardCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requireText;
    
    [SerializeField] private ItemView rewardView;

    [SerializeField] private GameObject rewardedObject;
    [SerializeField] private GameObject lockMask;
    
    private SpecialRequestRewardTableData tableData;
    public void Initialize(SpecialRequestRewardTableData data)
    {
        tableData = data;

        requireText.SetText($"{data.Starcondition}");

        rewardView.Initialize((Item_Type)tableData.Rewardtype,tableData.Rewardvalue);
        
        lockMask.SetActive(IsClear()==false);
        
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestSpecialRewardIdx)
            .AsObservable()
            .Subscribe(e =>
            {
                rewardedObject.SetActive(IsRewarded());
            })
            .AddTo(this);
    }
    private bool IsClear()
    {
        return ServerData.specialRequestBossServerTable.GetTotalStar() >= tableData.Starcondition;
    }
    private bool IsRewarded()
    {
        return ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestSpecialRewardIdx).Value >= tableData.Rewardidx;
    }
    private bool IsBeforeRewarded()
    {
        return (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestSpecialRewardIdx).Value == tableData.Rewardidx - 1;
    }
    
    public void OnClickButton()
    {
        if (IsClear() == false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Can);
            return;
        }

        if (IsRewarded() == true)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Has);
            return;
        }

        if (IsBeforeRewarded() == false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Before);
            return;
        }
        
        
        List<TransactionValue> transactionList = new List<TransactionValue>();
        Param userInfo2Param = new Param();
        Param rewardParam = new Param();
        UiRewardResultPopUp.Instance.Clear();
        
        
        var rewardType = (Item_Type)tableData.Rewardtype;
        
        // UiRewardResultPopUp.Instance
        //     .Clear()
        //     .AddOrUpdateReward(rewardType, tableData.Rewardvalue);

        if (rewardType.IsCostumeItem())
        {
            var costumeKey = rewardType.ToString();
            var costumeServerData = ServerData.costumeServerTable.TableDatas[costumeKey];

            costumeServerData.hasCostume.Value = true;

            rewardParam.Add(costumeKey, costumeServerData.ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, rewardParam));
        }
        else if (rewardType.IsNorigaeItem())
        {
            var key = rewardType.ToString();
            var serverData = ServerData.magicBookTable.TableDatas[key];

            serverData.hasItem.Value = 1;

            rewardParam.Add(key, serverData.ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, rewardParam));
        }
        else if (rewardType.IsWeaponItem())
        {
            var key = rewardType.ToString();
            var serverData = ServerData.weaponTable.TableDatas[key];

            serverData.hasItem.Value = 1;

            rewardParam.Add(key, serverData.ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, rewardParam));
        }
        else if (rewardType.IsGoodsItem())
        {
            var key = ServerData.goodsTable.ItemTypeToServerString(rewardType);
            var serverData = ServerData.goodsTable.TableDatas[key];

            serverData.Value += 1;

            rewardParam.Add(key, serverData.Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, rewardParam));
        }
        
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestSpecialRewardIdx).Value = tableData.Rewardidx;
        userInfo2Param.Add(UserInfoTable_2.specialRequestSpecialRewardIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestSpecialRewardIdx).Value);
        //패스 보상
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        
        
        ServerData.SendTransactionV2(transactionList,successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,$"{CommonString.GetItemName(rewardType)} 획득!" ,null);
            //UiRewardResultPopUp.Instance.Show().Clear();
        });
    }
}
