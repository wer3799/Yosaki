using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiContentsPopup : MonoBehaviour
{
    [SerializeField]
    private List<UiBossContentsView> bossContentsViews;

    [SerializeField]
    private List<GameObject> bandit1;

    [SerializeField]
    private List<GameObject> bandit2;

    [SerializeField]
    private List<GameObject> tower1;

    [SerializeField]
    private List<GameObject> tower2;

    [SerializeField]
    private List<GameObject> tower3;

    [SerializeField]
    private List<TextMeshProUGUI> banditDescription;

    void Start()
    {
        foreach (var t in bossContentsViews)
        {
            t.Initialize(TableManager.Instance.BossTable.dataArray[0]);
        }


        Subscribe();
    }

    private void Awake()
    {
        tower1.ForEach(e => e.gameObject.SetActive(ServerData.userInfoTable.IsLastFloor() == false));
        tower2.ForEach(e => e.gameObject.SetActive(ServerData.userInfoTable.IsLastFloor() && ServerData.userInfoTable.IsLastFloor2() == false));
        tower3.ForEach(e => e.gameObject.SetActive(ServerData.userInfoTable.IsLastFloor2()));
        
        RefreshBandit();
    }

    [SerializeField]
    private GameObject newUi;

    [SerializeField]
    private GameObject oldUi;

    private void Subscribe()
    {
        SettingData.newUi.AsObservable().Subscribe(e =>
        {
            newUi.SetActive(e == 1);

            oldUi.SetActive(e == 0);
        }).AddTo(this);
    }


    private void OnEnable()
    {
        //RefreshBandit();
    }

    private void RefreshBandit()
    {
        int level = ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        int requireLv = GameBalance.banditUpgradeLevel;
   
            bandit1.ForEach(e => e.SetActive(level < requireLv));
            bandit2.ForEach(e => e.SetActive(level >= requireLv));

        banditDescription.ForEach(e => e.SetText($"레벨 {Utils.ConvertBigNum(GameBalance.banditUpgradeLevel)}에 대왕반딧불전 해금!"));
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }
}