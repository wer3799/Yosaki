using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UiLimitPeriod : MonoBehaviour
{
    [SerializeField] private int year;
    [SerializeField] private int month;
    [SerializeField] private int day;

    [SerializeField] private Item_Type type = Item_Type.None;

    private void OnEnable()
    {
        var servertime = ServerData.userInfoTable.currentServerTime;

        DateTime limitTime;
        if (type != Item_Type.None)
        {
            limitTime = GameBalance.GetEventDate(type);
        }
        else
        {
            limitTime = new DateTime(year, month, day);
        }
        
        if (servertime > limitTime.AddDays(1))
        {
            gameObject.SetActive(false);
        }
    }
}
