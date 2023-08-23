using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CostumeCollectionCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI requireText;

    [SerializeField]
    private TextMeshProUGUI plusValueText;

    [SerializeField]
    private Image reward0Icon;
    [SerializeField]
    private Image reward1Icon;
    [SerializeField]
    private TextMeshProUGUI reward0Text;
    [SerializeField]
    private TextMeshProUGUI reward1Text;

    [SerializeField]
    private GameObject freeRewardComplete;
    [SerializeField]
    private GameObject adRewardComplete;

    [SerializeField]
    private Button freeRewardButton;
    [SerializeField]
    private Button adRewardButton;

    private CostumeCollectionData tableData;
    private void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {


        ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionFreeReward].AsObservable().Subscribe(e =>
        {

            bool hasReward = ServerData.etcServerTable.HasCostumeColectionFreeReward(tableData.Id);

            freeRewardComplete.SetActive(hasReward);
            freeRewardButton.interactable = !hasReward;
        }).AddTo(this);

        ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionAdReward].AsObservable().Subscribe(e =>
        {

            bool hasReward = ServerData.etcServerTable.HasCostumeColectionAdReward(tableData.Id);

            adRewardComplete.SetActive(hasReward);
            adRewardButton.interactable = !hasReward;

        }).AddTo(this);
    } 
    private bool HasCostumePassItem()
    {
        return ServerData.iapServerTable.TableDatas[UiCostumeCollectionPassBuyButton.costumePassKey].buyCount.Value > 0;
    }

    public void OnClickFreeRewardButton()
    {
        int costumeAmount = ServerData.costumeServerTable.GetCostumeHasAmount();

        if (costumeAmount < tableData.Require)
        {
            PopupManager.Instance.ShowAlarmMessage($"외형이 {tableData.Require}개 필요 합니다.");
            return;
        }

        if (ServerData.etcServerTable.HasCostumeColectionFreeReward(tableData.Id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Item_Type rewardType = (Item_Type)tableData.Rewardtype;

        float rewardValue = tableData.Rewardvalue;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));

        ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionFreeReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

        Param etcParam = new Param();
        etcParam.Add(EtcServerTable.CostumeCollectionFreeReward, ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionFreeReward].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage("보상 획득!");
          });
    }
    public void OnClickAdRewardButton()
    {
        int costumeAmount = ServerData.costumeServerTable.GetCostumeHasAmount();

       
        if (costumeAmount < tableData.Require)
        {
            PopupManager.Instance.ShowAlarmMessage($"외형이 {tableData.Require}개 필요 합니다.");
            return;
        }

        if (HasCostumePassItem() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("외형 패스 아이템이 필요합니다.");
            return;
        }

        if (ServerData.etcServerTable.HasCostumeColectionAdReward(tableData.Id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Item_Type rewardType = (Item_Type)tableData.Rewardtype;

        float rewardValue = tableData.Rewardvalue;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));

        ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionAdReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

        Param etcParam = new Param();
        etcParam.Add(EtcServerTable.CostumeCollectionAdReward, ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionAdReward].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage("보상 획득!");
          });
    }

    public void Initialize(CostumeCollectionData _costumeCollectionData)
    {
        tableData = _costumeCollectionData;

        reward0Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Rewardtype);
        reward1Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Rewardtype2);

        SetText();
    }

    private void SetText()
    {
        requireText.SetText($"{tableData.Require}개 보유시");
        plusValueText.SetText($"외형 효과 {tableData.Plusvalue}배");
        reward0Text.SetText($"{tableData.Rewardvalue}개");
        reward1Text.SetText($"{tableData.Rewardvalue2}개");
    }

}
