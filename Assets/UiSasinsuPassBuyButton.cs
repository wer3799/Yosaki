﻿using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Photon.Pun.Demo.Cockpit;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using WebSocketSharp;

public class UiSasinsuPassBuyButton : PassBuyButton
{
    [SerializeField]
    private TextMeshProUGUI descText;
    private CompositeDisposable disposable = new CompositeDisposable();

    private Button buyButton;


    void Start()
    {
        SetPassKey(sasinsuPassKey);
        
        Subscribe();
    }


    private void OnDestroy()
    {
        disposable.Dispose();
    }

    private void Subscribe()
    {
        buyButton = GetComponent<Button>();

        disposable.Clear();

        ServerData.iapServerTable.TableDatas[seasonPassKey].buyCount.AsObservable().Subscribe(e =>
        {
            descText.SetText(e >= 1 ? "구매완료" : "패스권 구매");
            this.gameObject.SetActive(e <= 0);
        }).AddTo(disposable);

        IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(e =>
        {
            SoundManager.Instance.PlaySound("GoldUse");
            GetPackageItem(e.purchasedProduct.definition.id);
        }).AddTo(disposable);

        IAPManager.Instance.disableBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = false;
        }).AddTo(disposable);

        IAPManager.Instance.activeBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = true;
        }).AddTo(disposable);

    }

    private void GetTableData()
    {
        if (TableManager.Instance.InAppPurchaseData.TryGetValue(seasonPassKey, out tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {seasonPassKey}", null);
        }
    }
    public void OnClickBuyButton()
    {
        if (ServerData.iapServerTable.TableDatas[seasonPassKey].buyCount.Value >= 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 구매 했습니다.");
            return;
        }

#if UNITY_EDITOR|| TEST
        GetPackageItem(seasonPassKey);
        return;
#endif

        IAPManager.Instance.BuyProduct(seasonPassKey);
    }

}
