using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiContentsStageLockMask : MonoBehaviour
{
    [FormerlySerializedAs("unlockLevel")] [SerializeField]
    private int unlockStage;

    [SerializeField]
    private TextMeshProUGUI levelDesc;

    [SerializeField] private bool isReverse = false;
    
    void Start()
    {
        if (levelDesc != null)
        {
            levelDesc.SetText($"{Utils.ConvertStage(unlockStage)}스테이지에 해금!");
        }

        Subscribe();
    }

    private void Subscribe()
    {
        if (isReverse == true)
        {
        }
        else
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).AsObservable().Subscribe(currentLevel =>
            {
                this.gameObject.SetActive(currentLevel+2 < unlockStage);
            }).AddTo(this);
        }
   
    }

    private void OnEnable()
    {
        if (isReverse == true)
        {
            if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value + 2 >= unlockStage)
            {
                    
            }
            else
            {
                this.gameObject.SetActive(false);
                PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertStage(unlockStage+2)}스테이지에 해금!");
            }
            
        }    
    }
}
