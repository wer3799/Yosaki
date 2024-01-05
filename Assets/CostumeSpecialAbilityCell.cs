using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class CostumeSpecialAbilityCell : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI skillName;

    [SerializeField]
    private TextMeshProUGUI skillDesc;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    private CostumeSpecialAbilityData abilityData;

    private bool isSubscribed = false;

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


    public void Refresh(CostumeSpecialAbilityData abilityData)
    {
        this.abilityData = abilityData;

        skillIcon.sprite = CommonResourceContainer.GetCostumeSpecialAbilityIconSprite(abilityData);

        skillName.SetText(abilityData.Skillname);

        int currentSkillLevel = ServerData.costumeSpecialAbilityServerTable.TableDatas[abilityData.Stringid].level.Value;

        var statusType = (StatusType)abilityData.Abilitytype;

        if (statusType.IsPercentStat())
        {
            if (statusType != StatusType.PenetrateDefense)
            {
                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {(PlayerStats.GetCostumeSpecialAbilityValue(statusType) * 100f).ToString()}");
            }
            else
            {
                skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {(PlayerStats.GetCostumeSpecialAbilityValue(statusType) * 100f)}");
            }
        }
        else
        {
            skillDesc.SetText($"{CommonString.GetStatusName(statusType)} : {(PlayerStats.GetCostumeSpecialAbilityValue(statusType)).ToString()}");
        }

        levelDescription.SetText($"LV:{currentSkillLevel}/{abilityData.Maxlevel}");

        if (currentSkillLevel >= abilityData.Maxlevel)
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

        
        ServerData.costumeSpecialAbilityServerTable.TableDatas[abilityData.Stringid].level.AsObservable().Subscribe(e =>
        {
            Refresh(this.abilityData);
        }).AddTo(this);
        if (string.IsNullOrEmpty(abilityData.Needpassivekey))
        {
            lockMask.SetActive(false);
        }
        else
        {
            ServerData.costumeSpecialAbilityServerTable.TableDatas[abilityData.Needpassivekey].level.AsObservable().Subscribe(e =>
            {
                if (ServerData.costumeSpecialAbilityServerTable.TableDatas[abilityData.Needpassivekey].level.Value >= (int)abilityData.Needpassivevalue)
                {
                    lockMask.SetActive(false);
                }
                else
                {
                    lockMask.SetActive(true);
                    var preTableData = TableManager.Instance.CostumeSpecialAbility.dataArray[abilityData.Id - 1];
                    lockMaskDesc.SetText($"{preTableData.Skillname} : {abilityData.Needpassivevalue} 레벨 달성 필요!");
                }
            }).AddTo(this);
        }

    }
    public void OnClickAllUpgradeButton()
    {
        int currentLevel = ServerData.costumeSpecialAbilityServerTable.TableDatas[abilityData.Stringid].level.Value;

        int maxLevel = abilityData.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            return;
        }


        //스킬포인트 체크
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.CostumeSkillPoint);
        if (skillPoint.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }



        var skillPointRemain = skillPoint.Value;

        var upgradableAmount = Mathf.Min(skillPointRemain, maxLevel - currentLevel);

        //로컬
        ServerData.costumeSpecialAbilityServerTable.TableDatas[abilityData.Stringid].level.Value += (int)upgradableAmount;
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

        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.CostumeSkillPoint);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param param = new Param();
        param.Add(abilityData.Stringid, ServerData.costumeSpecialAbilityServerTable.TableDatas[abilityData.Stringid].ConvertToString());
        transactions.Add(TransactionValue.SetUpdate(CostumeSpecialAbilityServerTable.tableName, CostumeSpecialAbilityServerTable.Indate, param));

        Param pointParam = new Param();
        pointParam.Add(StatusTable.CostumeSkillPoint, skillPoint.Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, pointParam));

        ServerData.SendTransactionV2(transactions);
    }
}
