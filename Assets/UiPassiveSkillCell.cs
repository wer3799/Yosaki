using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class UiPassiveSkillCell : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillName;

    [SerializeField]
    private TextMeshProUGUI skillDesc;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    private PassiveSkillData passiveSkillData;
    private MagicBookData magicBookData;
    private MagicBookServerData magicBookServerData;

    private string lvTextFormat = "LV : {0}/{1}";

    private bool isSubscribed = false;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockDesc;

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



    public void Refresh(PassiveSkillData passiveSkillData)
    {
        this.passiveSkillData = passiveSkillData;

        skillIcon.sprite = CommonResourceContainer.GetPassiveSkillIconSprite(passiveSkillData);

        skillName.SetText(passiveSkillData.Skillname);
        if (string.IsNullOrEmpty(passiveSkillData.Lockmaskdescription))
        {
            lockDesc.SetText("");
        }
        else
        {
            lockDesc.SetText($"{passiveSkillData.Lockmaskdescription}");
        }    
        
        
        int currentSkillLevel = ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        var statusType = (StatusType)passiveSkillData.Abilitytype;

        if (statusType.IsPercentStat())
        {
            //방무
            if (statusType == StatusType.PenetrateDefense)
            {
                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {(PlayerStats.GetPassiveSkillValue(statusType) * 100f)}");
            }
            //단전베기
            else if(statusType == StatusType.SuperCritical8DamPer||statusType == StatusType.SuperCritical13DamPer||statusType == StatusType.SuperCritical18DamPer)
            {
                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {Utils.ConvertNum(PlayerStats.GetPassiveSkillValue(statusType,passiveSkillData.Avoidduplicationidx) * 100f, 2)}");
 
            }
            else
            {
                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum(PlayerStats.GetPassiveSkillValue(statusType) * 100f)}");
            }
        }
        else
        {
            skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum(PlayerStats.GetPassiveSkillValue(statusType))}");
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

        magicBookData = TableManager.Instance.MagicBoocDatas[passiveSkillData.Requiremagicbookidx];

        magicBookServerData = ServerData.magicBookTable.TableDatas[magicBookData.Stringid];

        bool isClearRequireAbil = true;
        if (passiveSkillData.Requiremaxabil != -1)
        {
            //요구스킬
            var requireSkillData = TableManager.Instance.PassiveSkill.dataArray[passiveSkillData.Requiremaxabil];
            isClearRequireAbil = ServerData.passiveServerTable.TableDatas[requireSkillData.Stringid].level.Value >= requireSkillData.Maxlevel;
        }
        
        
        //NeedGoods가 null이 아니면
        if (string.IsNullOrEmpty(passiveSkillData.Needgoods) == false)
        {
            //둘중하나라도 충족하면 Mask.SetActive = true
            lockMask.SetActive((ServerData.goodsTable.GetTableData(passiveSkillData.Needgoods).Value == 0) ||
                               isClearRequireAbil == false);
        }
        //Needuserinfo가 null이 아니면
        else if (string.IsNullOrEmpty(passiveSkillData.Needuserinfo)==false)
        {
            if (passiveSkillData.Userinfotabletype == 1)
            {
                lockMask.SetActive(
                    (ServerData.userInfoTable.GetTableData(passiveSkillData.Needuserinfo).Value <
                     passiveSkillData.Requireuserinfo) || isClearRequireAbil == false);
            }
            else if(passiveSkillData.Userinfotabletype == 2)
            {
                lockMask.SetActive(
                    (ServerData.userInfoTable_2.GetTableData(passiveSkillData.Needuserinfo).Value <
                     passiveSkillData.Requireuserinfo) || isClearRequireAbil == false);
            }
        }
        else
        {
    
            //백귀패시브
            if (passiveSkillData.Requiremagicbookidx == 12)
            {
                
                int floor = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value;

                lockMask.SetActive(floor < PlayerStats.baekPassiveLock);

            }
            //일반
            else
            {
                if (magicBookServerData.hasItem.Value == 0)
                {
                    weaponView.Initialize(null, magicBookData);
                    weaponView.gameObject.SetActive(true);
                    lockMask.SetActive(true);
                }
                else
                {
                    lockMask.SetActive(false);
                }
            }
        }


        //

        if (isSubscribed == false)
        {
            isSubscribed = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {
        magicBookServerData.hasItem.AsObservable().Subscribe(e =>
        {

            Refresh(this.passiveSkillData);

        }).AddTo(this);

        ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.AsObservable().Subscribe(e =>
        {
            Refresh(this.passiveSkillData);
        }).AddTo(this);

        if (string.IsNullOrEmpty(passiveSkillData.Needgoods) == false)
        {
            ServerData.goodsTable.GetTableData(passiveSkillData.Needgoods).AsObservable().Subscribe(e =>
            {
                Refresh(this.passiveSkillData);
            }).AddTo(this);
        }
        if (string.IsNullOrEmpty(passiveSkillData.Needuserinfo) == false)
        {
            if (passiveSkillData.Userinfotabletype == 1)
            {
                ServerData.userInfoTable.GetTableData(passiveSkillData.Needuserinfo).AsObservable().Subscribe(e =>
                {
                    Refresh(this.passiveSkillData);
                }).AddTo(this);
            }

            else if (passiveSkillData.Userinfotabletype == 2)
            {
                ServerData.userInfoTable_2.GetTableData(passiveSkillData.Needuserinfo).AsObservable().Subscribe(e =>
                {
                    Refresh(this.passiveSkillData);
                }).AddTo(this);
            }

        }
        if (passiveSkillData.Requiremaxabil != -1)
        {
            //요구스킬
            var requireSkillData = TableManager.Instance.PassiveSkill.dataArray[passiveSkillData.Requiremaxabil];
            ServerData.passiveServerTable.TableDatas[requireSkillData.Stringid].level.AsObservable().Subscribe(e =>
            {
                Refresh(this.passiveSkillData);
            }).AddTo(this);
        }
    }

    public void OnClickUpgradeButton()
    {
        int currentLevel = ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        if (currentLevel >= passiveSkillData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }

        if (magicBookServerData.hasItem.Value != 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마도서가 없습니다.");
            return;
        }

        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.SkillPoint);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }

        //로컬
        ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value++;
        skillPoint.Value--;
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
    public void OnClickOneHundredUpgradeButton()
    {
        int currentLevel = ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        if (currentLevel >= passiveSkillData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }

        if (magicBookServerData.hasItem.Value != 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마도서가 없습니다.");
            return;
        }

        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.SkillPoint);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }

        int maxLevel = passiveSkillData.Maxlevel;

        var skillPointRemain = skillPoint.Value;

        var upgradableAmount = Mathf.Min(skillPointRemain, passiveSkillData.Maxlevel - currentLevel);

        upgradableAmount = Mathf.Min(upgradableAmount, 100);

        //로컬
        ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value += (int)upgradableAmount;
        skillPoint.Value -= (int)upgradableAmount;
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
    public void OnClickAllUpgradeButton()
    {
        int currentLevel = ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value;

        if (currentLevel >= passiveSkillData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }

        if (magicBookServerData.hasItem.Value != 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마도서가 없습니다.");
            return;
        }

        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.SkillPoint);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }

        int maxLevel = passiveSkillData.Maxlevel;

        var skillPointRemain = skillPoint.Value;

        var upgradableAmount = Mathf.Min(skillPointRemain, passiveSkillData.Maxlevel - currentLevel);

        //로컬
        ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].level.Value += (int)upgradableAmount;
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

        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.SkillPoint);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param passiveParam = new Param();
        passiveParam.Add(passiveSkillData.Stringid, ServerData.passiveServerTable.TableDatas[passiveSkillData.Stringid].ConvertToString());
        transactions.Add(TransactionValue.SetUpdate(PassiveServerTable.tableName, PassiveServerTable.Indate, passiveParam));

        Param skillPointParam = new Param();
        skillPointParam.Add(StatusTable.SkillPoint, skillPoint.Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, skillPointParam));

        ServerData.SendTransactionV2(transactions);
    }
}
