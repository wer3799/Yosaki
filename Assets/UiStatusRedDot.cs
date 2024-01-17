using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UiStatusRedDot : UiRedDotBase
{
    private ReactiveProperty<bool> hasRedDot = new ReactiveProperty<bool>();

    protected override void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.StatPoint).AsObservable().Subscribe(e =>
        {
            hasRedDot.Value = e > 0;
        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.Memory).AsObservable().Subscribe(e =>
        {
            hasRedDot.Value = e > 0;
        }).AddTo(this);

        hasRedDot.AsObservable().Subscribe(e=> 
        {
            rootObject.SetActive(e);
        }).AddTo(this);
        
        Observable.Interval(TimeSpan.FromSeconds(60))
            .Subscribe(_ => hasRedDot.Value = CheckMeditation())
            .AddTo(this);
    }

    private bool CheckMeditation()
    {
        var startTime =
            Utils.ConvertFromUnixTimestamp(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationStartTime).Value);
        var targetTime = startTime.AddHours(GameBalance.MeditationHour);
        
        TimeSpan timeRemaining = Utils.GetTimeRemaining(targetTime);

        
        if (timeRemaining.TotalSeconds >0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
