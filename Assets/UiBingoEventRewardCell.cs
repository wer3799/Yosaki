using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using BackEnd;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiBingoEventRewardCell : MonoBehaviour
{
    [SerializeField] private ItemView _itemView;
    
    [SerializeField] private GameObject lockMaskObject;
    [SerializeField] private GameObject rewardedObject;

    [SerializeField] private Image bgImage; 
    
    
    private BingoEventData tableData;
    private int bingoIdx = 0;
    public void Initialize(BingoEventData data,int idx=0)
    {
        tableData = data;
        bingoIdx = idx;

        _itemView.Initialize((Item_Type)tableData.Itemtype, tableData.Itemvalue);
        
        SetSprite();
        
        Subscribe();
    }

    private void SetSprite()
    {
        bgImage.sprite = CommonUiContainer.Instance.itemGradeFrame[tableData.Grade];
    }
    
    private void Subscribe()
    {
        switch (tableData.BINGOEVENTREWARDTYPE)
        {
            case BingoEventRewardType.Normal:
                ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].AsObservable().Subscribe(e =>
                {
                    SetMask();
                }).AddTo(this);
                break;
            case BingoEventRewardType.HorizontalBingo:
            case BingoEventRewardType.VerticalBingo:
            case BingoEventRewardType.DiagonalBingo:
                ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].AsObservable().Subscribe(e =>
                {
                    SetMask();
                }).AddTo(this);
                ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventBingoReward].AsObservable().Subscribe(e =>
                {
                    SetMask();
                }).AddTo(this);
                break;
            case BingoEventRewardType.Finish:
                ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventBingoReward].AsObservable().Subscribe(e =>
                {
                    SetMask();
                }).AddTo(this);
                break;
        }
    }

    private void SetMask()
    {
        lockMaskObject.SetActive(CanGetReward()==false);
        rewardedObject.SetActive(IsRewarded());
    }

    private bool IsRewarded()
    {
        switch (tableData.BINGOEVENTREWARDTYPE)
        {
            case BingoEventRewardType.Normal:
                return ServerData.etcServerTable.GachaEventRewarded(tableData.Id);
            case BingoEventRewardType.HorizontalBingo:
            case BingoEventRewardType.VerticalBingo:
            case BingoEventRewardType.DiagonalBingo:
            case BingoEventRewardType.Finish:
                return ServerData.etcServerTable.GachaEventBingoRewarded(tableData.Id);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private bool CanGetReward()
    {
        var length = 0;
        switch (tableData.BINGOEVENTREWARDTYPE)
        {
            case BingoEventRewardType.Normal:
                return true;
            case BingoEventRewardType.HorizontalBingo:
                length = (bingoIdx * 5) + 4;
                for (int i = bingoIdx*5; i <= length; i++)
                {
                    if (ServerData.etcServerTable.GachaEventRewarded(i))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            case BingoEventRewardType.VerticalBingo:
                length = (bingoIdx) + 20;
                for (int i = bingoIdx; i <=length ; i += 5)
                {
                    if (ServerData.etcServerTable.GachaEventRewarded(i))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            case BingoEventRewardType.DiagonalBingo:
                for (int i = 0; i <= 24; i +=6)
                {
                    if (ServerData.etcServerTable.GachaEventRewarded(i))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            case BingoEventRewardType.Finish:
                var list = ServerData.etcServerTable.GetGachaEventBingoRewardedList();
                return list.Count >= 11;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnClickButton()
    {
        if (tableData.BINGOEVENTREWARDTYPE == BingoEventRewardType.Normal)
            return;
        //PopupManager.Instance.ShowAlarmMessage($"{tableData.BINGOEVENTREWARDTYPE.ToString()} ID : {tableData.Id} bingoIdx : {bingoIdx}");
        
        var rewardedList = ServerData.etcServerTable.GetGachaEventRewardedList();
        
        
        if (CanGetReward()==false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Can);
            return;        
        }

        if (IsRewarded())
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Has);
            return;
        }
        var type = (Item_Type)tableData.Itemtype;
        var value = tableData.Itemvalue;
        ServerData.AddLocalValue(type, value);



        List<TransactionValue> transactionList = new List<TransactionValue>();
        
        Param goodsParam = new Param();
        var rewardString = ServerData.goodsTable.ItemTypeToServerString(type);
        goodsParam.Add(rewardString, ServerData.goodsTable.GetTableData(rewardString).Value);

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        
        Param etcParam = new Param();
        
        if (tableData.BINGOEVENTREWARDTYPE == BingoEventRewardType.Finish)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value = string.Empty;
            ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventBingoReward].Value = string.Empty;
            etcParam.Add(EtcServerTable.gachaEventReward, ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value);
        }
        else
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventBingoReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";
        }
        etcParam.Add(EtcServerTable.gachaEventBingoReward, ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventBingoReward].Value);
        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        var valueStr= value >= 10000 ? Utils.ConvertNum(value) : value.ToString();

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(type)} {valueStr}개 획득!");
            SetMask();
        });
    }
}
