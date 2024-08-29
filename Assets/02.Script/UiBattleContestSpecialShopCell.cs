using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Spine.Unity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiBattleContestSpecialShopCell : MonoBehaviour
{
    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI requireText;

    private BattleContestSpecialExchangeData tableData;
    [SerializeField] private SkeletonGraphic skeletonGraphic;

    private void ChangeCostume()
    {
        var idx = ServerData.costumeServerTable.TableDatas[((Item_Type)tableData.Itemtype).ToString()].idx;
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }
    private void Subscribe()
    {
        string itemKey = ((Item_Type)tableData.Itemtype).ToString();

        if (IsCostumeItem())
        {
            ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.AsObservable().Subscribe(e =>
            {
                if (e == false)
                {
                    if (IsRequireTotalScore())
                    {
                        requireText.SetText($"총 {tableData.Wincount}승 이상 달성시 획득 가능\n난이도 무관 ");
                    }
                    else
                    {
                        var contestData = TableManager.Instance.BattleContestTable.dataArray[tableData.Level];
                        requireText.SetText($"{contestData.Name}에서 {tableData.Wincount}승 이상 달성시 획득 가능");    
                    }
                }
                else
                {
                    requireText.SetText("보유중!");
                }
            }).AddTo(this);
        }
        else if (IsPassWeaponItem())
        {
            ServerData.weaponTable.TableDatas[itemKey].hasItem.AsObservable().Subscribe(e =>
            {
                if (e == 0)
                {
                    var contestData = TableManager.Instance.BattleContestTable.dataArray[tableData.Level];
                    requireText.SetText($"{contestData.Name}에서 {tableData.Wincount}승 이상 달성시 획득 가능");
                }
                else
                {
                    requireText.SetText("보유중!");
                }
            }).AddTo(this);
        }
        else if (IsPassNoriGaeItem())
        {
            ServerData.magicBookTable.TableDatas[itemKey].hasItem.AsObservable().Subscribe(e =>
            {
                if (e == 0)
                {
                    var contestData = TableManager.Instance.BattleContestTable.dataArray[tableData.Level];
                    requireText.SetText($"{contestData.Name}에서 {tableData.Wincount}승 이상 달성시 획득 가능");
                }
                else
                {
                    requireText.SetText("보유중!");
                }
            }).AddTo(this);
        }
    }
    public void Initialize(BattleContestSpecialExchangeData data)
    {
        tableData = data;

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);
        
        titleText.SetText($"{CommonString.GetItemName((Item_Type)tableData.Itemtype)}");

        if (((Item_Type)tableData.Itemtype).IsCostumeItem())
        {
            ChangeCostume();
        }
        Subscribe();
    }
    private bool IsCostumeItem()
    {
        return ((Item_Type)tableData.Itemtype).IsCostumeItem();
    }
    private bool IsPassWeaponItem()
    {
        return ((Item_Type)tableData.Itemtype).IsWeaponItem();
    }

    private bool IsPassNoriGaeItem()
    {
        return ((Item_Type)tableData.Itemtype).IsNorigaeItem();
    }

    private bool CanGetReward()
    {
        if (IsRequireTotalScore())
        {
            var totalScore=ServerData.etcServerTable.GetBattleContestTotalScore();
        
            return tableData.Wincount <= totalScore;
        }
        var score = ServerData.etcServerTable.GetBattleContestTotalScoreFromIdx(tableData.Level);

        Debug.LogError(score);
        return tableData.Wincount <= score;

    }

    private bool IsRequireTotalScore()
    {
        return tableData.Level == -1;
    }
    public void OnClickExchangeButton()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }


        if (IsCostumeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if (ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.Value)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                return;
            }
        }

        if (IsPassWeaponItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            //무기
            if (ServerData.weaponTable.TableDatas[itemKey].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                return;
            }
        }
        if (IsPassNoriGaeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            //무기
            if (ServerData.magicBookTable.TableDatas[itemKey].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                return;
            }
        }

        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"승리 수가 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");


        ServerData.AddLocalValue((Item_Type)tableData.Itemtype, tableData.Itemvalue);

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
        private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.6f);

    public IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        if (IsCostumeItem())
        {
            Param costumeParam = new Param();

            string costumeKey = ((Item_Type)tableData.Itemtype).ToString();

            costumeParam.Add(costumeKey.ToString(), ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else if (IsPassWeaponItem())
        {
            Param weaponParam = new Param();

            string weaponKey = ((Item_Type)tableData.Itemtype).ToString();

            weaponParam.Add(weaponKey.ToString(), ServerData.weaponTable.TableDatas[weaponKey].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));
        }
        else if (IsPassNoriGaeItem())
        {
            Param magicbookParam = new Param();

            string key = ((Item_Type)tableData.Itemtype).ToString();

            magicbookParam.Add(key.ToString(), ServerData.magicBookTable.TableDatas[key].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicbookParam));
        }


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            if (IsCostumeItem())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
            }
            else if (IsPassWeaponItem())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!!", null);
            }
            else if (IsPassNoriGaeItem())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "노리개 획득!!", null);
            }
        });

    }
}
