﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine.UI.Extensions;


public class UiPetPassCell : FancyCell<PassData_Fancy>
{
    private PassData_Fancy itemData;
    
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

    [SerializeField] private Image passIcon;

    [SerializeField] private List<Sprite> petIcons;
    private void OnDestroy()
    {
        disposables.Dispose();
    }
    private void Subscribe()
    {
        disposables.Clear();
        //
        //무료보상 데이터 변경시
        ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = IsFreeRewarded();
            rewardedObject_Free.SetActive(rewarded);
        
        }).AddTo(disposables);
        
        //광고보상 데이터 변경시
        ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Subscribe(e =>
        {
            bool rewarded = IsAdRewarded();
            rewardedObject_Ad.SetActive(rewarded);
        
        }).AddTo(disposables);
        
        //킬카운트 변경될때
        ServerData.userInfoTable_2.GetTableData(passInfo.key0).AsObservable().Subscribe(e =>
        {
            lockIcon_Free.SetActive(!CanGetReward());
            lockIcon_Ad.SetActive(!CanGetReward());
            gaugeImage.SetActive(CanGetReward());
        }).AddTo(disposables);
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

        passIcon.sprite = GetPassIconSprite();
    }

    private Sprite GetPassIconSprite()
    {
        var passNum = int.Parse(passInfo.shopId.ToString().Replace("petpass", ""));
        return petIcons[passNum];
    }
    
    private void SetDescriptionText()
    {
        descriptionText.SetText($"{Utils.ConvertNum(passInfo.require)}");
    }


    public void OnClickFreeRewardButton()
    {
        if (CanGetReward()==false)
        {
            PopupManager.Instance.ShowAlarmMessage("킬 수가 부족합니다!");
            return;
        }
        else if (IsFreeRewarded()==true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }
        else if (IsBeforeRewardedFree()==false)
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
        if (CanGetReward()==false)
        {
            PopupManager.Instance.ShowAlarmMessage("킬 수가 부족합니다!");
            return;
        }
        else if (IsAdRewarded()==true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }
        else if (IsBeforeRewardedAd()==false)
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
        }
    }
    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[TableManager.Instance.PetPass.dataArray[passInfo.id].Shopid].buyCount.Value > 0;

        return hasIapProduct;
    }

    private bool HasPassProduct()
    {
        return IAPManager.Instance.HasProduct(passInfo.shopId);
    }

    private void GetFreeReward()
    {
        //로컬
        var rewardCount = int.Parse(ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value) + 1;
        ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value = rewardCount.ToString();
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_Free, passInfo.rewardTypeValue_Free);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_Free_Key, ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(SeolPassServerTable.tableName, SeolPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_Free);
        transactionList.Add(rewardTransactionValue);

        ServerData.SendTransactionV2(transactionList);
    }
    private void GetAdReward()
    {
        //로컬
        var rewardCount = int.Parse(ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value) + 1;
        ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value = rewardCount.ToString();

        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_IAP, passInfo.rewardTypeValue_IAP);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_IAP_Key, ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(SeolPassServerTable.tableName, SeolPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_IAP);
        transactionList.Add(rewardTransactionValue);

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
          {
           //   LogManager.Instance.SendLog("패스 광고보상", "보상획득");
          });

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
    }
    private bool IsBeforeRewardedFree()
    {
        var idx = int.Parse(ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
        return idx+1 == passInfo.passGrade;
    }
    private bool IsBeforeRewardedAd()
    {
        var idx = int.Parse(ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value);
        return idx+1 == passInfo.passGrade;
    }
    private bool CanGetReward()
    {
        var killCount = (int)ServerData.userInfoTable_2.GetTableData(passInfo.key0).Value;
        return killCount >= passInfo.require;
    }
    private bool IsAdRewarded()
    {
        return int.Parse(ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value) >= passInfo.passGrade;
    }
    private bool IsFreeRewarded()
    {
        return int.Parse(ServerData.seolPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value) >= passInfo.passGrade;
    }
    public void UpdateUi(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

       Subscribe();
    }

    public override void UpdateContent(PassData_Fancy itemData)
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