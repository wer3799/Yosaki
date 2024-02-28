using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;

public class UiSamchunTitleAbilityCell : MonoBehaviour
{

    [SerializeField] private GameObject lockMask;
    [SerializeField] private TextMeshProUGUI lockMaskText;
    [SerializeField] private TextMeshProUGUI abilityDescText;

    private Title_SamcheonData tableData;
    private CompositeDisposable disposable = new CompositeDisposable();

    private void Subscribe()
    {
        disposable.Clear();
        
        ServerData.samchunTitleServerTable.TableDatas[tableData.Stringid].hasReward.AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(e < 1);
        }).AddTo(disposable);
    }

    private void OnDestroy()
    {
        disposable.Clear();
    }

    public void Initialize(Title_SamcheonData _tableData)
    {
        tableData = _tableData;
        
        lockMaskText.SetText(tableData.Lockmaskdescription);

        var type = (StatusType)tableData.Abiltype;

        var desc = tableData.Name;
        if (type.IsPercentStat())
        {
            abilityDescText.SetText($"{desc}\n{CommonString.GetStatusName(type)} {Utils.ConvertNum(tableData.Abilvalue * 100, 4)}");
        }
        else
        {
            abilityDescText.SetText($"{desc}\n{CommonString.GetStatusName(type)} {Utils.ConvertNum(tableData.Abilvalue)}");
        }

        Subscribe();
    }

    public void OnClickButton()
    {
        var list = tableData.Bossid;

        var twelveData = TableManager.Instance.TwelveBossTable.dataArray;

        bool allClear = false;
        
        var bossName = string.Empty;
        for (int i = 0; i < list.Length; i++)
        {
            var rewardsCount = ServerData.bossServerTable.GetBossRewardedIdxList(twelveData[list[i]].Stringid).Count;
            var maxCount = twelveData[list[i]].Rewardtype.Length;
            
            //Debug.LogError($"보스 ID : {twelveData[list[i]].Stringid}\n보상카운트 : {rewardsCount} / 맥스카운트 :  {maxCount}");
            if (rewardsCount < maxCount)
            {
                allClear = false;
                bossName += twelveData[list[i]].Name;
                break;
            }

            allClear = true;
        }

        if (allClear)
        {
            List<TransactionValue> transactionList = new List<TransactionValue>();
            Param param = new Param();
            
            ServerData.samchunTitleServerTable.TableDatas[tableData.Stringid].hasReward.Value = 1;
            param.Add(tableData.Stringid, ServerData.samchunTitleServerTable.TableDatas[tableData.Stringid].ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(SamchunTitleServerTable.tableName, SamchunTitleServerTable.Indate, param));

            ServerData.SendTransactionV2(transactionList, successCallBack: () =>
            {
                UiSamchunTitleBoard.Instance.RefreshUi();
                
                PopupManager.Instance.ShowAlarmMessage("삼천 세계 업적 획득!");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"삼천 세계 업적 획득 조건을 만족하지 못했습니다!\n조건 미달성 : {bossName} 보상 획득");
        }

    }
}
