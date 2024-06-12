using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class UiPassiveSkill3Cell : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillName;

    [SerializeField]
    private TextMeshProUGUI skillDesc;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    private PassiveSkill3Data passiveSkillData;

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


    public void Refresh(PassiveSkill3Data passiveSkillData)
    {
        this.passiveSkillData = passiveSkillData;

        skillIcon.sprite = CommonResourceContainer.GetPassiveSkill3IconSprite(passiveSkillData);

        skillName.SetText(passiveSkillData.Skillname);

        int currentSkillLevel = ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        var statusType = (StatusType)passiveSkillData.Abilitytype;

        if (statusType.IsPercentStat())
        {
            if (statusType != StatusType.PenetrateDefense)
            {

                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {Utils.ConvertNum(PlayerStats.GetPassiveSkill3Value(statusType) * 100f)}");
            }
            else
            {
                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {Utils.ConvertNum(PlayerStats.GetPassiveSkill3Value(statusType) * 100f)}");
            }
        }
        else
        {
            skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {Utils.ConvertNum((PlayerStats.GetPassiveSkill3Value(statusType)))}");
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

        
        ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.AsObservable().Subscribe(e =>
        {
            Refresh(this.passiveSkillData);
        }).AddTo(this);
        if (string.IsNullOrEmpty(passiveSkillData.Needpassivekey))
        {
            lockMask.SetActive(false);
        }
        else
        {
            ServerData.passive3ServerTable.TableDatas[passiveSkillData.Needpassivekey].level.AsObservable().Subscribe(e =>
            {
                if (ServerData.passive3ServerTable.TableDatas[passiveSkillData.Needpassivekey].level.Value >= (int)passiveSkillData.Needpassivevalue)
                {
                    lockMask.SetActive(false);
                }
                else
                {
                    lockMask.SetActive(true);
                    var preTableData = TableManager.Instance.PassiveSkill3.dataArray[passiveSkillData.Id - 1];
                    lockMaskDesc.SetText($"{preTableData.Skillname} : {passiveSkillData.Needpassivevalue} 레벨 달성 필요!");
                }
            }).AddTo(this);
        }

    }

    public void OnClickUpgradeButton()
    {
        int currentLevel = ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        if (currentLevel >= passiveSkillData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }

        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < GameBalance.passive3UnlockLevel)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertNum(GameBalance.passive3UnlockLevel)} 레벨을 달성해야 합니다!");
            return;
        }


        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill3Point);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }

        //로컬
        ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.Value++;
        skillPoint.Value--;
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
    public void OnClickOneHundredUpgradeButton()
    {
        int currentLevel = ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.Value;
        
        int maxLevel = passiveSkillData.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }

        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill3Point);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }



        var skillPointRemain = skillPoint.Value;

        var upgradableAmount = Mathf.Min(skillPointRemain, maxLevel - currentLevel);

        upgradableAmount = Mathf.Min(upgradableAmount, 100);

        //로컬
        ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.Value += (int)upgradableAmount;
        skillPoint.Value -= (int)upgradableAmount;
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
    public void OnClickAllUpgradeButton()
    {
        int currentLevel = ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        int maxLevel = passiveSkillData.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }


        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill3Point);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }



        var skillPointRemain = skillPoint.Value;

        var upgradableAmount = Mathf.Min(skillPointRemain, maxLevel - currentLevel);

        //로컬
        ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].level.Value += (int)upgradableAmount;
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

        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.Skill3Point);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param passiveParam = new Param();
        passiveParam.Add(passiveSkillData.Stringid, ServerData.passive3ServerTable.TableDatas[passiveSkillData.Stringid].ConvertToString());
        transactions.Add(TransactionValue.SetUpdate(Passive3ServerTable.tableName, Passive3ServerTable.Indate, passiveParam));

        Param skillPointParam = new Param();
        skillPointParam.Add(StatusTable.Skill3Point, skillPoint.Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, skillPointParam));

        ServerData.SendTransaction(transactions);
    }
}
