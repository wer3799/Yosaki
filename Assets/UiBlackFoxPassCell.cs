using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;


public class UiBlackFoxPassCell : MonoBehaviour
{
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
        ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_Free_Key);
            rewardedObject_Free.SetActive(rewarded);

        }).AddTo(disposables);

        //광고보상 데이터 변경시
        ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_IAP_Key);
            rewardedObject_Ad.SetActive(rewarded);

        }).AddTo(disposables);

    }

    private void OnEnable()
    {
        if (isInitialized)
        {
            OnOffIcon();
        }
    }

    private void OnOffIcon()
    {
        lockIcon_Free.SetActive(!CanGetReward());
        lockIcon_Ad.SetActive(!CanGetReward());
        gaugeImage.SetActive(CanGetReward());
    }

    private bool isInitialized = false;

    public void Initialize(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();
        
        OnOffIcon();

        SetDescriptionText();

        Subscribe();
        
        if (isInitialized == false)
        {
            isInitialized = true;
        }
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
        descriptionText.SetText($"{passInfo.require + 1}단계");
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.coldSeasonPassServerTable.TableDatas[key].Value.Split(',').ToList();
    }

    private bool HasReward(string key)
    {
        return int.Parse(ServerData.coldSeasonPassServerTable.TableDatas[key].Value) >= passInfo.id;
    }
    private bool IsBeforeRewarded(string key)
    {
        //0일때 1
        return int.Parse(ServerData.coldSeasonPassServerTable.TableDatas[key].Value) + 1 == passInfo.id;
    }
    
    public void OnClickFreeRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("검은 구미호전 단계가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_Free_Key))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (IsBeforeRewarded(passInfo.rewardType_Free_Key)==false)
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
            PopupManager.Instance.ShowAlarmMessage("검은 구미호전 단계가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_IAP_Key))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (IsBeforeRewarded(passInfo.rewardType_IAP_Key)==false)
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
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiBlackFoxPassBuyButton.seasonPassKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    private void GetFreeReward()
    {

        var type = (Item_Type)(int)passInfo.rewardType_Free;
        //로컬
        ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value = $"{passInfo.id}";
        //검은구미호전패스
        ServerData.AddLocalValue(type, passInfo.rewardTypeValue_Free);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_Free_Key, ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(ColdSeasonPassServerTable.tableName, ColdSeasonPassServerTable.Indate, passParam));
        
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BlackFoxGoods, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value);
        
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        
        ServerData.SendTransactionV2(transactionList);
    }
    private void GetAdReward()
    {
        
        var type = (Item_Type)(int)passInfo.rewardType_IAP;

        //로컬
        ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value = $"{passInfo.id}";
        ServerData.AddLocalValue(type, passInfo.rewardTypeValue_IAP);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_IAP_Key, ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(ColdSeasonPassServerTable.tableName, ColdSeasonPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_IAP);
        transactionList.Add(rewardTransactionValue);

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
          {
              
          });

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
    }

    private bool CanGetReward()
    {
        return PlayerStats.GetBlackFoxGrade() >= passInfo.require;
    }
}
