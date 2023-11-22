using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class UiYachaUpgradeBoard : SingletonMono<UiYachaUpgradeBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private WeaponView yachaWeaponView;

    [SerializeField]
    private TextMeshProUGUI basicAbilDescription;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
        {

            UpdateDescriptionText();

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.cockAwake].AsObservable().Subscribe(e =>
        {

            if (e == 1)
            {
                UpdateDescriptionText();
            }

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].AsObservable().Subscribe(e =>
        {

            if (e == 1)
            {
                UpdateDescriptionText();
            }

        }).AddTo(this);
    }

    private void UpdateDescriptionText()
    {
        string desc = $"{CommonString.GetStatusName(StatusType.SkillDamage)} {Utils.ConvertBigNum(PlayerStats.GetYachaSkillPercentValue() * 100f)} 증가";

        if (ServerData.userInfoTable.TableDatas[UserInfoTable.cockAwake].Value == 1)
        {
            desc += $"\n<color=red>{CommonString.GetStatusName(StatusType.IgnoreDefense)} {Utils.ConvertBigNum(PlayerStats.GetYachaIgnoreDefenseValue())}증가</color>";
        }

        if (ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].Value == 1)
        {
            desc += $"\n<color=blue>{CommonString.GetStatusName(StatusType.SuperCritical1DamPer)} {Utils.ConvertBigNum(PlayerStats.GetYachaChunSlashValue() * 100f)}증가</color>";
        }

        basicAbilDescription.SetText(desc);
    }

    private void Initialize()
    {
        yachaWeaponView.Initialize(TableManager.Instance.WeaponTable.dataArray[21], null, null);
    }


    public void ShowUpgradePopup(bool show)
    {


        rootObject.SetActive(show);
    }
}
