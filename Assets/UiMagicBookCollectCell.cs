using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;

public class UiMagicBookCollectCell : MonoBehaviour
{
    [SerializeField]
    private WeaponView weaponView;

    private SkillTableData skillData;

    [SerializeField]
    private Image upgradeButton;

    [SerializeField]
    private Sprite normalSprite;

    [SerializeField]
    private Sprite maxLevelSprite;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI abilName;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI buttonText;


    public void Initialize(SkillTableData skillData)
    {
        this.skillData = skillData;

        weaponView.Initialize(null, null, this.skillData);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][skillData.Id].AsObservable().Subscribe(WhenCollectionLevelChanged).AddTo(this);
    }

    private void WhenCollectionLevelChanged(int level)
    {
        if (level >= skillData.Collectionabiltmaxlevel)
        {
            upgradeButton.sprite = maxLevelSprite;

            levelText.SetText($"LV : {level}(MAX)");
            buttonText.SetText("최고레벨");

            float collectionAbilValue = skillData.Collectionvalue * level;

            StatusType statusType = (StatusType)skillData.Collectionabiltype;
            abilName.SetText(CommonString.GetStatusName(statusType));
            if (statusType.IsPercentStat() == false)
            {
                abilDescription.SetText($"+{collectionAbilValue}");
            }
            else
            {
                abilDescription.SetText($"+{(collectionAbilValue * 100f).ToString("F2")}%");
            }
        }
        else
        {
            upgradeButton.sprite = normalSprite;

            levelText.SetText($"LV : {level}");
            buttonText.SetText("레벨업");

            float collectionAbilValue = skillData.Collectionvalue * level;
            float collectionAbilNextValue = skillData.Collectionvalue * (level + 1);

            StatusType statusType = (StatusType)skillData.Collectionabiltype;
            abilName.SetText(CommonString.GetStatusName(statusType));
            if (statusType.IsPercentStat() == false)
            {
                abilDescription.SetText($"+{collectionAbilValue}->{collectionAbilNextValue}");
            }
            else
            {
                abilDescription.SetText($"+{(collectionAbilValue * 100f).ToString("F2")}%->+{(collectionAbilNextValue * 100f).ToString("F2")}%");
            }
        }


    }

    public void OnClickUpgradeButton()
    {
        int currentSkillCollectionLevel = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][skillData.Id].Value;
        int currentSkillHasAmount = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].Value;

        if (currentSkillCollectionLevel >= this.skillData.Collectionabiltmaxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
            return;
        }

        if (currentSkillHasAmount <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술이 부족 합니다.");
            return;
        }

        SoundManager.Instance.PlayButtonSound();

        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].Value--;
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][skillData.Id].Value++;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    public void OnClickAllLevelUpButton()
    {
        int currentSkillCollectionLevel = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][skillData.Id].Value;
        int currentSkillHasAmount = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].Value;

        if (currentSkillCollectionLevel >= this.skillData.Collectionabiltmaxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
            return;
        }

        if (currentSkillHasAmount <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술이 부족 합니다.");
            return;
        }

        SoundManager.Instance.PlayButtonSound();

        int upgradableLevel = this.skillData.Collectionabiltmaxlevel - currentSkillCollectionLevel;

        int upgradeApply = 0;

        if (currentSkillHasAmount >= upgradableLevel)
        {
            upgradeApply = upgradableLevel;
        }
        else
        {
            upgradeApply = currentSkillHasAmount;
        }

        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].Value -= upgradeApply;
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][skillData.Id].Value += upgradeApply;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }

    private Coroutine syncRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(2.0f);

    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param skillParam = new Param();
        List<int> skillAmountSyncData = new List<int>();
        List<int> collectionLevel = new List<int>();

        for (int i = 0; i < ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
            collectionLevel.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][i].Value);
        }

        skillParam.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);
        skillParam.Add(SkillServerTable.SkillCollectionLevel, collectionLevel);

        //스킬
        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));

        ServerData.SendTransaction(transactionList);
        
        YorinMissionManager.UpdateYorinMissionClear(YorinMissionKey.YMission2_7, 1);

        syncRoutine = null;
    }
}
