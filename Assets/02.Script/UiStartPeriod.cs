using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiStartPeriod : MonoBehaviour
{
    [SerializeField] private int year; 
    [SerializeField] private int month; 
    [SerializeField] private int day;


    private void OnEnable()
    {
        var servertime =  ServerData.userInfoTable.currentServerTime;
        
        DateTime startTime = new DateTime(year, month, day);
        
        if (servertime < startTime)
        {
            gameObject.SetActive(false);
        }
    }

}
