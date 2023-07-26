using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiClearTicketBoard : SingletonMono<UiClearTicketBoard>
{
    [SerializeField]
    private UiClearTicketCell cell;

    [SerializeField]
    private Transform parents;

    public void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.ClearTicket.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if(tableDatas[i].Active==false) continue;
            
            var button = Instantiate<UiClearTicketCell>(cell, parents);
            button.Initialize(i);
        }
    }


}