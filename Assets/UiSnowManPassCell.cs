using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;
using System;
using UnityEngine.UI.Extensions;

public class UiSnowManPassCell : FancyCell<SnowPassData_Fancy>
{
    SnowPassData_Fancy itemData;
    
    [SerializeField]
    private Image itemIcon_free;

    [SerializeField]
    private TextMeshProUGUI itemName_free;

    [SerializeField]
    private Image itemIcon_ad;

    [SerializeField]
    private TextMeshProUGUI itemName_ad;

    [SerializeField]
    private TextMeshProUGUI itemAmount_free;

    [SerializeField]
    private TextMeshProUGUI itemAmount_ad;

    [SerializeField]
    private GameObject lockIcon_Free;

    [SerializeField]
    private GameObject lockIcon_Ad;

    private PassInfo passInfo;

    [SerializeField]
    private GameObject rewardedObject_Free;

    [SerializeField]
    private GameObject rewardedObject_Ad;

    [SerializeField]
    private GameObject gaugeImage;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnDestroy()
    {
        disposables.Dispose();
    }
    private void Subscribe()
    {
        disposables.Clear();

        //무료보상 데이터 변경시
        ServerData.oneYearPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_Free_Key, passInfo.id);
            rewardedObject_Free.SetActive(rewarded);
        }).AddTo(disposables);

        //광고보상 데이터 변경시
        ServerData.oneYearPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_IAP_Key, passInfo.id);
            rewardedObject_Ad.SetActive(rewarded);

        }).AddTo(disposables);

        //킬카운트 변경될때
        ServerData.userInfoTable.GetTableData(UserInfoTable.usedSnowManCollectionCount).AsObservable().Subscribe(e =>
        {
            RefreshData();
        }).AddTo(disposables);
    }



    public void Initialize(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

        Subscribe();

        RefreshParent();

        RefreshData();
    }

    private void SetAmount()
    {
        itemAmount_free.SetText(Utils.ConvertBigNum(passInfo.rewardTypeValue_Free));
        itemAmount_ad.SetText(Utils.ConvertBigNum(passInfo.rewardTypeValue_IAP));
    }

    private void SetItemIcon()
    {
        itemIcon_free.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)passInfo.rewardType_Free);
        itemIcon_ad.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)passInfo.rewardType_IAP);

        itemName_free.SetText(CommonString.GetItemName((Item_Type)(int)passInfo.rewardType_Free));
        itemName_ad.SetText(CommonString.GetItemName((Item_Type)(int)passInfo.rewardType_IAP));
    }

    private void SetDescriptionText()
    {
        descriptionText.SetText($"{Utils.ConvertBigNum(passInfo.require)}");
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.oneYearPassServerTable.TableDatas[key].Value.Split(',').ToList();
    }

    public bool HasReward(string key, int data)
    {
        return int.Parse(ServerData.oneYearPassServerTable.TableDatas[key].Value) >= data;

    }
    private bool GetBeforeRewarded(string key,int data)
    {
        return int.Parse(ServerData.oneYearPassServerTable.TableDatas[key].Value) == data - 1;
    }
    public void OnClickFreeRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"교환한 {CommonString.GetItemName(Item_Type.Event_Item_SnowMan)}가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_Free_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (GetBeforeRewarded(passInfo.rewardType_Free_Key,passInfo.id) == false)
        {
            PopupManager.Instance.ShowAlarmMessage("이전 보상을 받아주세요!");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");

        GetFreeReward();

    }

    //광고아님
    public void OnClickAdRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"교환한 {CommonString.GetItemName(Item_Type.Event_Item_SnowMan)}이(가) 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_IAP_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }
        if (GetBeforeRewarded(passInfo.rewardType_IAP_Key,passInfo.id) == false)
        {
            PopupManager.Instance.ShowAlarmMessage("이전 보상을 받아주세요!");
            return;
        }

        if (HasPassItem())
        {
            GetAdReward();
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"패스권이 필요합니다.");
            return;
        }
        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
    }

    static public bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiSnowManEventBuyButton.fallPassKey].buyCount.Value > 0;

        return hasIapProduct;
    }
    private void GetFreeReward()
    {
        //로컬
        ServerData.oneYearPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value = $"{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_Free, passInfo.rewardTypeValue_Free);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_Free_Key, ServerData.oneYearPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(OneYearPassServerTable.tableName, OneYearPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_Free);
        transactionList.Add(rewardTransactionValue);

        //킬카운트
        Param userInfoParam = new Param();
        //userInfoParam.Add(UserInfoTable.attenCountOne, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountOne).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            //  LogManager.Instance.SendLogType("월간", "무료", $"{passInfo.id}");
        });
    }
    private void GetAdReward()
    {
        //로컬
        ServerData.oneYearPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value = $"{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_IAP, passInfo.rewardTypeValue_IAP);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_IAP_Key, ServerData.oneYearPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(OneYearPassServerTable.tableName, OneYearPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_IAP);
        transactionList.Add(rewardTransactionValue);

        //킬카운트
        Param userInfoParam = new Param();
        //userInfoParam.Add(UserInfoTable.attenCountOne, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountOne).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            //   LogManager.Instance.SendLogType("월간", "유료", $"{passInfo.id}");
        });

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
    }

    private bool CanGetReward()
    {
        int killCountTotalBok = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.usedSnowManCollectionCount).Value;
        return killCountTotalBok >= passInfo.require;
    }

    private void RefreshData()
    {
        if (passInfo == null) return;
        lockIcon_Free.SetActive(!CanGetReward());
        lockIcon_Ad.SetActive(!CanGetReward());
        gaugeImage.SetActive(CanGetReward());
    }

    public void RefreshParent()
    {
        if (passInfo == null) return;

        if (CanGetReward() == true && HasReward(passInfo.rewardType_Free_Key, passInfo.id) == false)
        {
            this.transform.SetAsFirstSibling();
        }
    }

    public void UpdateUi(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

        Subscribe();
        
        RefreshData();
    }

    public override void UpdateContent(SnowPassData_Fancy itemData)
    {
        if (this.itemData != null && this.itemData.passInfo.id == itemData.passInfo.id)
        {
            return;
        }

        this.itemData = itemData;

        
        UpdateUi(this.itemData.passInfo);
    }

    float currentPosition = 0;
    [SerializeField] Animator animator = default;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;

        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }

        animator.speed = 0;
    }

    void OnEnable() => UpdatePosition(currentPosition);
}
