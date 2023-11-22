using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiWeaponCollectionView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI weaponName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private WeaponData weaponData;

    private WeaponServerData weaponServerData;

    [SerializeField]
    private GameObject notHasObject;

    [SerializeField]
    private Image weaponIcon;

    [SerializeField]
    private Image reward0Icon;
    [SerializeField]
    private Image reward1Icon;

    [SerializeField]
    private TextMeshProUGUI reward0Value;
    [SerializeField]
    private TextMeshProUGUI reward1Value;

    [SerializeField]
    private Button reward0Button;
    [SerializeField]
    private Button reward1Button;

    [SerializeField]
    private TextMeshProUGUI reward0Description;
    [SerializeField]
    private TextMeshProUGUI reward1Description;

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }
    public void Initialize(WeaponData weaponData)
    {

        this.weaponData = weaponData;

        this.weaponServerData = ServerData.weaponTable.TableDatas[weaponData.Stringid];

        weaponName.SetText($"{weaponData.Name}");
        
        weaponIcon.sprite = CommonResourceContainer.GetWeaponSprite(weaponData.Id);        

        reward0Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)weaponData.Rewardtype0);
        reward1Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)weaponData.Rewardtype1);

        reward0Value.SetText(Utils.ConvertBigNum(weaponData.Rewardvalue0));
        reward1Value.SetText(Utils.ConvertBigNum(weaponData.Rewardvalue1));
        

        Subscribe();
    }

    private void Subscribe()
    {
        weaponServerData.hasItem.AsObservable().Subscribe(e =>
        {
            notHasObject.SetActive(e == 0);

            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);
        weaponServerData.getReward0.AsObservable().Subscribe(e =>
        {
            bool hasReward = e == 1;

            reward0Button.interactable = !hasReward;

            reward0Description.SetText(!hasReward ? "보상수령" : "수령완료");
        }).AddTo(this);
        weaponServerData.getReward1.AsObservable().Subscribe(e =>
        {
            bool hasReward = e == 1;

            reward1Button.interactable = !hasReward;

            reward1Description.SetText(!hasReward ? "보상수령" : "수령완료");
        }).AddTo(this);


    }

    public void OnClickGetRewardFreeButton()
    {
        if (weaponServerData.getReward0.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        weaponServerData.getReward0.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        //재화 획득
        Item_Type rewardType = (Item_Type)weaponData.Rewardtype0;

        float rewardValue = weaponData.Rewardvalue0;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));


        // 보상 획득 
        Param weaponParam = new Param();        
        string updateValue = ServerData.weaponTable.TableDatas[weaponData.Stringid].ConvertToString();
        weaponParam.Add(weaponData.Stringid, updateValue);

        transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
    public void OnClickGetRewardAdButton()
    {
        if (weaponServerData.getReward1.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (ServerData.iapServerTable.TableDatas[UiEquipmentCollectionPassBuyButton.collectionPassKey].buyCount.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("도감 패스권이 필요합니다.");
            return;
        }

        weaponServerData.getReward1.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        //재화 획득
        Item_Type rewardType = (Item_Type)weaponData.Rewardtype1;

        float rewardValue = weaponData.Rewardvalue1;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));


        // 보상 획득 
        Param weaponParam = new Param();        
        string updateValue = ServerData.weaponTable.TableDatas[weaponData.Stringid].ConvertToString();
        weaponParam.Add(weaponData.Stringid, updateValue);

        transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }

    public void OnClickDescription()
    {
        PopupManager.Instance.ShowAlarmMessage($"{weaponData.Acquiredescription}");
    }
}
