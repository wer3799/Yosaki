using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;

public class UiRankHonorRewardCell : MonoBehaviour
{
    [SerializeField] private ItemView itemView;

    [SerializeField] private GameObject lockMaskObject;
    [SerializeField] private GameObject rewardedObject;
    [SerializeField] private TextMeshProUGUI requireText;
    private int require;
    private int id;
    private HonorRewardData data;
    public void Initialize(HonorRewardData tableData)
    {
        data = tableData;
        
        itemView.Initialize((Item_Type)data.Rewardtype, data.Rewardvalue);

        require = data.Stageid+2;
        id = data.Id;
        
        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).AsObservable().Subscribe(SetUi).AddTo(this);
    }    
    private void SetUi(double idx)
    {
        lockMaskObject.SetActive(CanGetReward()==false);
        rewardedObject.SetActive(idx >= id);
        requireText.SetText($"스테이지 {Utils.ConvertStage(require)} 클리어");
    }
    private bool CanGetReward()
    {
        return require <= UiRankHonorBoard.Instance.rankInfo.Score+2;
    }
    private bool GetBeforeRewarded()
    {
        return id==(int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).Value+1;
    }
    private bool HasRewarded()
    {
        return id <= (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).Value;
    }
    
    
    public void OnClickButton()
    {
        if (HasRewarded() == true)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Has);
            return;
        }
        
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Can);
            return;
        }

        if (GetBeforeRewarded() == false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Before);
            return;
        }


        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).Value = id;

        List<TransactionValue> transactions = new List<TransactionValue>();
        
        var type = (Item_Type)data.Rewardtype;
        
        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (float)data.Rewardvalue));

        Param userInfo2Param = new Param();
        
        userInfo2Param.Add(UserInfoTable_2.rankHonorRewardIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        
        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상 획득!");
        });
    } 
}
