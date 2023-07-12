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

    void Start()
    {
        levelDesc.SetText($"{Utils.ConvertStage(unlockStage)}스테이지에 해금!");

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).AsObservable().Subscribe(currentLevel =>
        {
            this.gameObject.SetActive(currentLevel+2 < unlockStage);
        }).AddTo(this);
    }

}
