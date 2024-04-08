using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;

public class UiGuildPetWeeklyRewardView : MonoBehaviour
{
    [SerializeField] private ItemView itemView;
    [SerializeField] private TextMeshProUGUI requireText;
    
    [SerializeField] private GameObject rewardedMask;
    [SerializeField] private GameObject canGetMask;
    
    private GuildPetData tableData;
    public void Initialize(GuildPetData _tableData)
    {
        tableData = _tableData;
        
        itemView.Initialize((Item_Type)tableData.Rewardtype,tableData.Rewardvalue);
        
        requireText.SetText($"{tableData.Requirelevel}에 해금");

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildPetWeeklyRewardIndex)
            .AsObservable()
            .Subscribe(e =>
            {
                //받은적이 있다면 true
                rewardedMask.SetActive(IsRewarded()==true);
            })
            .AddTo(this);
        GuildManager.Instance.guildPetExp.AsObservable().Subscribe(e =>
        {
            //얻는게 불가능하면 true
            canGetMask.SetActive(CanGetReward() == false);
        }).AddTo(this);
    }
    
    private bool CanGetReward()
    {
        return GuildManager.Instance.guildPetExp.Value >= tableData.Requirelevel;
    }

    private bool IsBeforeRewarded()
    {
        //0일때 1
        return (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildPetWeeklyRewardIndex).Value + 1 == tableData.Id;
    }

    private bool IsRewarded()
    {
        return (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildPetWeeklyRewardIndex).Value >= tableData.Id;
    }

    public void OnClickButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Can);
            return;
        }
        else if (IsRewarded() == true)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Has);
            return;
        }
        else if (IsBeforeRewarded() == false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Before);
            return;
        }


        List<TransactionValue> transactions = new List<TransactionValue>();
        
        Param goodsParam = new Param();
        Param userInfo2Param = new Param();
        
        var stringId = ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Rewardtype);
        ServerData.goodsTable.GetTableData(stringId).Value += tableData.Rewardvalue;
        goodsParam.Add(stringId, ServerData.goodsTable.GetTableData(stringId).Value);

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildPetWeeklyRewardIndex).Value++;
        userInfo2Param.Add(UserInfoTable_2.guildPetWeeklyRewardIndex, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.guildPetWeeklyRewardIndex].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
        });
    }
}
