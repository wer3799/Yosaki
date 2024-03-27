using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using static UiTwelveRewardPopup;

public class UiSuhoAnimalRewardView : MonoBehaviour
{
    private TwelveBossRewardInfo rewardInfo;

    public TwelveBossRewardInfo RewardInfo => rewardInfo;

    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private Button rewardButton;

    [SerializeField] private TextMeshProUGUI rewardButtonDescription;

    [SerializeField] private TextMeshProUGUI rewardAmount;

    [SerializeField] private GameObject rewardLockMask;

    [SerializeField] private TextMeshProUGUI lockDescription;

    [SerializeField] private TextMeshProUGUI gradeText;

    private SuhoSuhoPetServerData bossServerData;

    [SerializeField] private GameObject rewardedIcon;

    private CompositeDisposable disposable = new CompositeDisposable();

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    public void Initialize(TwelveBossRewardInfo rewardInfo, SuhoSuhoPetServerData bossServerData)
    {
        this.rewardInfo = rewardInfo;

        this.bossServerData = bossServerData;

        rewardLockMask.SetActive(rewardInfo.currentDamage < rewardInfo.damageCut);

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)rewardInfo.rewardType);

        itemDescription.SetText($"{CommonString.GetItemName((Item_Type)rewardInfo.rewardType)}");

        rewardAmount.SetText($"{Utils.ConvertNum(rewardInfo.rewardAmount)}개");
#if UNITY_EDITOR
        lockDescription.SetText($"{Utils.ConvertNum(rewardInfo.damageCut)}에 해금");
#else
        lockDescription.SetText($"{rewardInfo.rewardCutString}에 해금");
#endif

        if (gradeText != null)
        {
            gradeText.SetText($"{rewardInfo.idx + 1}단계\n({rewardInfo.idx + 1}점)");
        }

        Subscribe();
    }

    private void Subscribe()
    {
        disposable.Clear();

        bossServerData.rewardedItem.AsObservable().Subscribe(e =>
        {
            var rewards = e.Split(BossServerTable.rewardSplit).ToList();

            bool rewarded = rewards.Contains(rewardInfo.idx.ToString());

            rewardButtonDescription.SetText(rewarded ? "완료" : "받기");

            rewardedIcon.SetActive(rewarded);
            
        }).AddTo(disposable);
    }

    public void OnClickGetButton()
    {
        Item_Type type = (Item_Type)rewardInfo.rewardType;

        if (rewardInfo.currentDamage < rewardInfo.damageCut)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 피해량이 부족 합니다.");
            return;
        }

        
        var rewards = bossServerData.rewardedItem.Value.Split(BossServerTable.rewardSplit).ToList();

        if (rewards.Contains(rewardInfo.idx.ToString()))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        rewardButton.interactable = false;

        float amount = rewardInfo.rewardAmount;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param bossParam = new Param();

        bossServerData.rewardedItem.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

        var localTableData = TableManager.Instance.suhoPetTable.dataArray[bossServerData.idx];

        bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, bossParam));

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)amount));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
            SoundManager.Instance.PlaySound("Reward");
            if (rewardButton != null)
            {
                rewardButton.interactable = true;
            }
        });
    }

    public bool GetRewardByScript()
    {
        if (rewardInfo.currentDamage < rewardInfo.damageCut)
        {
            return false;
        }

        var rewards = bossServerData.rewardedItem.Value.Split(BossServerTable.rewardSplit).ToList();

        if (rewards.Contains(rewardInfo.idx.ToString()))
        {
            return false;
        }

        Item_Type type = (Item_Type)rewardInfo.rewardType;

        float amount = rewardInfo.rewardAmount;

        bossServerData.rewardedItem.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";
        ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value += amount;

        return true;
    }
}