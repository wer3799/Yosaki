using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class UiPassiveSkill2Cell : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillName;

    [SerializeField]
    private TextMeshProUGUI skillDesc;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    private PassiveSkill2Data passiveSkill2Data;

    private bool isSubscribed = false;

    [SerializeField]
    private WeaponView weaponView;

    private Coroutine syncRoutine;

    [SerializeField]
    private Image buttonImage;

    [SerializeField]
    private Sprite normalSprite;

    [SerializeField]
    private Sprite maxSprite;

    [SerializeField]
    private TextMeshProUGUI buttonDesc;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockMaskDesc;


    public void Refresh(PassiveSkill2Data passiveSkillData)
    {
        this.passiveSkill2Data = passiveSkillData;

        skillIcon.sprite = CommonResourceContainer.GetPassiveSkill2IconSprite(passiveSkillData);

        skillName.SetText(passiveSkillData.Skillname);

        int currentSkillLevel = ServerData.passive2ServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        var statusType = (StatusType)passiveSkillData.Abilitytype;

        if (statusType.IsPercentStat())
        {
            if (statusType != StatusType.PenetrateDefense)
            {

            skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {(PlayerStats.GetPassiveSkill2Value(statusType) * 100f).ToString()}");
            }
            else
            {
                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {(PlayerStats.GetPassiveSkill2Value(statusType) * 100f)}");
            }
        }
        else
        {
            skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {(PlayerStats.GetPassiveSkill2Value(statusType)).ToString()}");
        }

        levelDescription.SetText($"LV:{currentSkillLevel}/{passiveSkillData.Maxlevel}");

        if (currentSkillLevel >= passiveSkillData.Maxlevel)
        {
            buttonImage.sprite = maxSprite;
            buttonDesc.SetText("최고레벨");
        }
        else
        {
            buttonImage.sprite = normalSprite;
            buttonDesc.SetText("레벨업");
        }



        if (isSubscribed == false)
        {
            isSubscribed = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {

        
        ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].level.AsObservable().Subscribe(e =>
        {
            Refresh(this.passiveSkill2Data);
        }).AddTo(this);
        if (string.IsNullOrEmpty(passiveSkill2Data.Needpassivekey))
        {
            lockMask.SetActive(false);
        }
        else
        {
            ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Needpassivekey].level.AsObservable().Subscribe(e =>
            {
                if (ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Needpassivekey].level.Value >= (int)passiveSkill2Data.Needpassivevalue)
                {
                    lockMask.SetActive(false);
                }
                else
                {
                    lockMask.SetActive(true);
                    var preTableData = TableManager.Instance.PassiveSkill2.dataArray[passiveSkill2Data.Id - 1];
                    lockMaskDesc.SetText($"{preTableData.Skillname} : {passiveSkill2Data.Needpassivevalue} 레벨 달성 필요!");
                }
            }).AddTo(this);
        }

    }

    public void OnClickUpgradeButton()
    {
        int currentLevel = ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].level.Value;

        if (currentLevel >= passiveSkill2Data.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }


        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill2Point);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }

        //로컬
        ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].level.Value++;
        skillPoint.Value--;
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
    public void OnClickOneHundredUpgradeButton()
    {
        int currentLevel = ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].level.Value;
        
        int maxLevel = passiveSkill2Data.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }

        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill2Point);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }



        var skillPointRemain = skillPoint.Value;

        var upgradableAmount = Mathf.Min(skillPointRemain, maxLevel - currentLevel);

        upgradableAmount = Mathf.Min(upgradableAmount, 100);

        //로컬
        ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].level.Value += (int)upgradableAmount;
        skillPoint.Value -= (int)upgradableAmount;
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
    public void OnClickAllUpgradeButton()
    {
        int currentLevel = ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].level.Value;

        int maxLevel = passiveSkill2Data.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }


        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill2Point);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }



        var skillPointRemain = skillPoint.Value;

        var upgradableAmount = Mathf.Min(skillPointRemain, maxLevel - currentLevel);

        //로컬
        ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].level.Value += (int)upgradableAmount;
        skillPoint.Value -= (int)upgradableAmount;
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private IEnumerator SyncRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill2Point);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param passiveParam = new Param();
        passiveParam.Add(passiveSkill2Data.Stringid, ServerData.passive2ServerTable.TableDatas[passiveSkill2Data.Stringid].ConvertToString());
        transactions.Add(TransactionValue.SetUpdate(Passive2ServerTable.tableName, Passive2ServerTable.Indate, passiveParam));

        Param skill2PointParam = new Param();
        skill2PointParam.Add(StatusTable.Skill2Point, skillPoint.Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, skill2PointParam));

        ServerData.SendTransaction(transactions);
    }
}
