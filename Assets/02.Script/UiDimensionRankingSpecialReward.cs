using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiDimensionRankingSpecialReward : MonoBehaviour
{
    [SerializeField] private ItemView itemView;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField]
    private GameObject canGetMask;

     [SerializeField]
    private GameObject rewardedMask;

    private DimensionRewardData tableData;

    public void Initialize(DimensionRewardData _tableData)
    {
        this.tableData = _tableData;
        itemView.Initialize((Item_Type)tableData.Rewardtype,tableData.Rewardvalue);
        text.SetText($"최고 {tableData.Dungeoncondition}단계 달성");
        Subscribe();
    }
        
    private bool CanGetReward()
    {
        return ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value >= tableData.Dungeoncondition;
    }
    private bool IsBeforeRewarded()
    {
        //0일때 1
        return (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionSpecialRewardIdx).Value + 1 == tableData.Rewardidx;
    }

    private bool IsRewarded()
    {
        return (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionSpecialRewardIdx).Value >= tableData.Rewardidx;
    }
    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionSpecialRewardIdx)
            .AsObservable()
            .Subscribe(e =>
            {
                //받은적이 있다면 true
                rewardedMask.SetActive(IsRewarded()==true);
            })
            .AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade)
            .AsObservable()
            .Subscribe(e =>
            {
                //받은적이 있다면 true
                canGetMask.SetActive(CanGetReward() == false);
            })
            .AddTo(this);
        
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

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionSpecialRewardIdx).Value++;
        userInfo2Param.Add(UserInfoTable_2.dimensionSpecialRewardIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dimensionSpecialRewardIdx].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
        
        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
        });

    }
}
