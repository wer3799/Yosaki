using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiHotTimeBoard : SingletonMono<UiMileageBoard>
{
    [SerializeField]
    private UiHotTimeShopCell cell;

    [SerializeField]
    private Transform parents;

    public void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.xMasCollection.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Active == false) continue;
            if (tableDatas[i].COMMONTABLEEVENTTYPE != CommonTableEventType.HotTime) continue;
            //프리미엄
            var button = Instantiate<UiHotTimeShopCell>(cell, parents);
            button.Initialize(i);
        }
    }


}