using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTrainingPassPopup : MonoBehaviour
{

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_WinterPass_0;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_WinterPass_1;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        uiBuffPopupView_WinterPass_0.Initalize(tableDatas[21]);

        uiBuffPopupView_WinterPass_1.Initalize(tableDatas[22]); ;
    }


}
