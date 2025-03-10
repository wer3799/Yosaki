﻿using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UniRx;
using UnityEngine;

public class UiMeditationBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI meditationButtonText;
    [SerializeField] private TextMeshProUGUI meditationGrade;
    [SerializeField] private GameObject meditationGoods;
    private DateTime targetTime;
    
    private CompositeDisposable disposable = new CompositeDisposable();

    
    private void Start()
    {
        UpdateTimeText();
        
        Subscribe2();
    }

    private void OnEnable()
    {
        disposable.Clear();    
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value > 0)
        {
            Subscribe();
        }
    }
    private void OnDisable()
    {
        disposable.Clear();    
    }

    private void SetTargetTime()
    {
        var startTime =
            Utils.ConvertFromUnixTimestamp(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value);
        targetTime = startTime.AddHours(GameBalance.MeditationHour);
    }
    private void Subscribe()
    {
        disposable.Clear();
        
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => UpdateTimeText())
            .AddTo(disposable);
    }
    private void Subscribe2()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).AsObservable().Subscribe(e =>
        {
            meditationGrade.SetText($"명상 {e + 1} 단계");
        }).AddTo(this);
    }
    private void UpdateTimeText()
    {
        SetTargetTime();
        //명상 안누름
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value < 0)
        {
            timeText.SetText($"소요시간\n<color=white>{GameBalance.MeditationHour}시 00분</color>");
            meditationButtonText.SetText("명상 시작");
            meditationGoods.SetActive(false);
            return;
        }
        
        //명상누름
        
        TimeSpan timeRemaining = targetTime - DateTime.Now;        
        // var seconds = ElapsedTimeManager.Instance.GetElapsedTimeReal();
        // TimeSpan timeRemaining = targetTime - ServerData.userInfoTable.currentServerTime.AddSeconds(seconds);
        //
        // #if UNITY_EDITOR
        // Debug.LogError("앱이 실행된 시간 :" + timeRemaining.TotalSeconds + "초");
        // #endif
        
        if (timeRemaining.TotalSeconds > 3600)
        {
            string formattedTime = string.Format("남은시간\n<color=white>{0:D2}시:{1:D2}분</color>", timeRemaining.Hours, timeRemaining.Minutes);
            timeText.text = formattedTime;
            meditationButtonText.SetText($"시간 단축\n{Utils.ConvertNum((int)(timeRemaining.TotalSeconds/60) * GetNextGradeCost())}개 사용");
            if (meditationGoods != null)
            {
                meditationGoods.SetActive(true);
            }
        }
        else if(timeRemaining.TotalSeconds > 0)
        {
            string formattedTime = string.Format("남은시간\n<color=white>{0:D2}분:{1:D2}초</color>", timeRemaining.Minutes, timeRemaining.Seconds);
            timeText.SetText(formattedTime);
            meditationButtonText.SetText($"시간 단축\n{Utils.ConvertNum(Mathf.Max(1,(int)(timeRemaining.TotalSeconds/60)) * GetNextGradeCost())}개 사용");
            if (meditationGoods != null)
            {
                meditationGoods.SetActive(true);
            }
        }
        else
        {
            timeText.SetText($"남은시간\n<color=white>00분:00초</color>");

            meditationButtonText.SetText("명상 완료");
            if (meditationGoods != null)
            {
                meditationGoods.SetActive(false);
            }
        }
    }

    private int GetNextGradeCost()
    {
        var tableData = TableManager.Instance.Meditation.dataArray;
        var nextGrade =
            Mathf.Min((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value + 1, tableData.Length - 1);

        return tableData[nextGrade].Consume;
    }

    public void OnClickMeditation()
    {
        var tableDataLength = TableManager.Instance.Meditation.dataArray.Length;
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value >= tableDataLength-1)
        {
            PopupManager.Instance.ShowAlarmMessage("최고 단계입니다!");
            return;
        }
        //명상버튼 안누른 상태
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value < 0)
        {
            DateTime currentServerTime = Utils.GetBackendServerTime();
            var currentServerDate = (double)Utils.ConvertToUnixTimestamp(currentServerTime);
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value = currentServerDate;
            ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.meditationStartTime,false);
            SetTargetTime();
            UpdateTimeText();
            Subscribe();
            return;
                
        }
        //명상하는중
        else
        {
           
            
            TimeSpan timeRemaining = targetTime - Utils.GetBackendServerTime();
            //남은시간있음
            if (timeRemaining.TotalSeconds > 0)
            {
                int multiplyCost = GetNextGradeCost();

                var lastMinute = Mathf.Max(1,(int)(timeRemaining.TotalSeconds / 60));
                if (ServerData.goodsTable.GetTableData(GoodsTable.MeditationGoods).Value < lastMinute * multiplyCost)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.MeditationGoods)}이 부족합니다.");
                    return;
                }

                if (ServerData.goodsTable.GetTableData(GoodsTable.MeditationGoods).Value < lastMinute * multiplyCost)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.MeditationGoods)}이 부족합니다.");
                    return;
                }
                
                //두번 타는거 방지용
                if (timeRemaining.TotalSeconds < 0)
                {
                    PopupManager.Instance.ShowAlarmMessage("명상중이지 않습니다!");
                    return;
                }
                
                ServerData.goodsTable.GetTableData(GoodsTable.MeditationGoods).Value -=
                lastMinute * multiplyCost;

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value++;

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value = -1;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();

                goodsParam.Add(GoodsTable.MeditationGoods,
                    ServerData.goodsTable.GetTableData(GoodsTable.MeditationGoods).Value);

                transactions.Add(
                    TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                Param userinfo2Param = new Param();

                userinfo2Param.Add(UserInfoTable_2.meditationIndex,
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationIndex].Value);
                userinfo2Param.Add(UserInfoTable_2.meditationStartTime,
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStartTime].Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate,
                    userinfo2Param));

                ServerData.SendTransactionV2(transactions, successCallBack: () =>
                {
                    PlayerStats.ResetMeditationDictionary();
                    PopupManager.Instance.ShowAlarmMessage("명상 단계 상승");
                    UpdateTimeText();
                });
            }
            //시간다됨.
            else
            {
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value++;
                
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value = -1;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userinfo2Param = new Param();

                userinfo2Param.Add(UserInfoTable_2.meditationIndex, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationIndex].Value);
                userinfo2Param.Add(UserInfoTable_2.meditationStartTime, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStartTime].Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

                ServerData.SendTransactionV2(transactions, successCallBack: () =>
                {
                    PlayerStats.ResetMeditationDictionary();
                    PopupManager.Instance.ShowAlarmMessage("명상 단계 상승");
                    UpdateTimeText();
                });
            }
        }
    }
}
