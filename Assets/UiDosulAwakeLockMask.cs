using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;
using TMPro;

public class UiDosulAwakeLockMask : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI levelDesc;

    [SerializeField]
    private GameObject rootObject;
    void Start()
    {
        levelDesc.SetText($"도술 강화 {GameBalance.DosulAwakeRequireLevel+1}레벨 이상 해금!");

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulLevel).AsObservable().Subscribe(currentLevel =>
        {
            rootObject.SetActive(currentLevel < GameBalance.DosulAwakeRequireLevel);
        }).AddTo(this);
    }
    
}
