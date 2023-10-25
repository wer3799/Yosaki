using System;
using UniRx;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.Analytics;
using UnityEngine.UI;
using System.Collections.Generic;
using BackEnd;

//박용진:
//0 무한
//1 일1회
//2 주2회
//3 월1회
//99 평생1회만

public enum BuyType
{
    NoLimit, DayOfOne, WeekOfTwo, MonthOfOne, AllTimeOne, MonthOfFive, WeekOfFive, DayOfFive, MonthOfTen, Pension, Fixed,WeekOfOne
}
public enum ShopCategory
{
    Gem, Limit1, Limit2, Pet, Costume, Pension, Chuseok, Limit3, Event, Event2, Event3, None, LevelUp, ChunFlower,
    Goods_Week,//도깨비불,수미꽃,영혼석소탕권
    Goods_PetSoul,//수호환
    Goods_NewGacha,//영혼석
    Goods_SpecialGoods,//불멸석,천계꽃,도꺠비불,수미꽃
    Goods_GrowthStone,//여우구슬,수련의돌,영혼열쇠,검조각
    Goods_BaseGoods,//여우구슬,수련의돌,영혼열쇠,검조각
    Goods_FoxFire,//여우불
    Goods_InstantClear,//봉인검
    Goods_Month,//월간상품
}

public enum PassProductType
{
    None,
    LevelPass,
    MonthPass,
    PetPass,
}
public class IAPManager : SingletonMono<IAPManager>, IStoreListener
{
    public static IStoreController m_StoreController;          // The Unity Purchasing system.
    public static IExtensionProvider m_StoreExtensionProvider { get; private set; } // The store-specific Purchasing subsystems.
    private static Product test_product = null;
    private ReactiveCommand<PurchaseEventArgs> whenBuyComplete = new ReactiveCommand<PurchaseEventArgs>();
    public IObservable<PurchaseEventArgs> WhenBuyComplete => whenBuyComplete.AsObservable();


    public ReactiveCommand disableBuyButton = new ReactiveCommand();
    public ReactiveCommand activeBuyButton = new ReactiveCommand();

    public Product[] PurchasedProducts { get; private set; }
    private Dictionary<string, int> PurchasedProductsDic = new Dictionary<string, int>();

    public Product GetProduct(string productId)
    {
        return m_StoreController.products.WithID(productId);
    }
    public void ResetDisableCallbacks()
    {
        disableBuyButton = new ReactiveCommand();
        activeBuyButton = new ReactiveCommand();
    }

    public bool HasProduct(string id)
    {
        return PurchasedProductsDic.ContainsKey(id);
    }
    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            Debug.LogError("인앱 초기화 안됨");
            return;
        }

        InitializeItems();
    }

    private void InitializeItems()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        AddProductsItem(builder);

        UnityPurchasing.Initialize(this, builder);
    }

    private void AddProductsItem(ConfigurationBuilder builder)
    {
        var tableData = TableManager.Instance.InAppPurchase.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Needlevel != 0) continue;

            string productId = string.Empty;

#if UNITY_ANDROID
            productId = tableData[i].Productid;
#endif

#if UNITY_IOS
            productId = tableData[i].Productidios;
#endif
            //소모품
            if (tableData[i].Issubscribeitem == false)
            {
                builder.AddProduct(productId, ProductType.Consumable);
            }
            //비소모품            
            else
            {
                builder.AddProduct(productId, ProductType.NonConsumable);
            }
        }
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProduct(string shopId)
    {
        disableBuyButton?.Execute();
        BuyProductID(shopId);
    }

    public void SendLog(string prefix, string shopId)
    {
        Param param = new Param();
        param.Add(prefix, $"shopId : {shopId}");
        Backend.GameLog.InsertLog("IAP", param, (callback) =>
        {
            // 이후 처리
        });
    }

    public void CompletePurchase()
    {
        if (test_product == null)
            MyDebug("Cannot complete purchase, product not initialized.");
        else
        {
            m_StoreController.ConfirmPendingPurchase(test_product);
            MyDebug("Completed purchase with " + test_product.transactionID.ToString());
        }

    }

    public void RestorePurchases()
    {
        m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
        {
            if (result)
            {
                MyDebug("Restore purchases succeeded.");
            }
            else
            {
                MyDebug("Restore purchases failed.");
            }
        });
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                MyDebug(string.Format("Purchasing product:" + product.definition.id.ToString()));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("구매 불가 상품");
                MyDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("상품 초기화 안됨");
            MyDebug("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        MyDebug("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        List<Product> products = new List<Product>();

        if (controller.products.all != null)
        {
            PurchasedProducts = controller.products.all;
#if !PUBLISH
            Debug.Log("PurchasedProducts");
#endif
            for (int i = 0; i < controller.products.all.Length; i++)
            {
               //Debug.Log(controller.products.all[i].definition.id);
                if (controller.products.all[i].hasReceipt)
                {
                    products.Add(controller.products.all[i]);
                }
            }
        }

        PurchasedProducts = products.ToArray();

        if (PurchasedProducts.Length == 0)
        {
            Debug.Log("There is no Receipt item");
        }

        for (int i = 0; i < PurchasedProducts.Length; i++)
        {
            //Debug.Log($"{PurchasedProducts[i].definition.id} has hasReceipt");
        }

        //딕셔너리에 저장
        for (int i = 0; i < PurchasedProducts.Length; i++)
        {
            string key = PurchasedProducts[i].definition.id;

            if (PurchasedProductsDic.ContainsKey(key) == false)
            {
                PurchasedProductsDic.Add(key, 1);
            }
            else
            {
                PurchasedProductsDic[key]++;
            }
        }
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        MyDebug("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"OnInitializeFailed@@@@@@@@@@@@@@@@@@{error} {message}");
        //throw new NotImplementedException();
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        activeBuyButton.Execute();

        //BackendReturnObject validation = Backend.Receipt.IsValidateGooglePurchase(args.purchasedProduct.receipt, "receiptDescription", false);

        ////영수증 검증 실패
        //if (validation.IsSuccess() == false)
        //{
        //    PopupManager.Instance.ShowConfirmPopup("알림", "영수증 검증 실패", null);
        //    return PurchaseProcessingResult.Pending;
        //}
        //else
        //{
        //    // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        //    Debug.Log(string.Format("영수증 검증 실패 ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        //}

        test_product = args.purchasedProduct;

        bool isSuccess = true;
#if !UNITY_EDITOR && UNITY_ANDROID
		CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
		try
		{
			IPurchaseReceipt[] result = validator.Validate(args.purchasedProduct.receipt);
			for(int i = 0; i < result.Length; i++)
				Analytics.Transaction(result[i].productID, args.purchasedProduct.metadata.localizedPrice, args.purchasedProduct.metadata.isoCurrencyCode, result[i].transactionID, null);
		}
		catch (IAPSecurityException)
		{
			isSuccess = false;
		}
#endif
        if (isSuccess)
        {
            SendLog("상품 구매 성공", args.purchasedProduct.definition.id);

            MyDebug(string.Format("ProcessPurchase: Complete. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID.ToString()));

            string key = args.purchasedProduct.definition.id;

            if (PurchasedProductsDic.ContainsKey(key) == false)
            {
                PurchasedProductsDic.Add(key, 1);
            }
            else
            {
                PurchasedProductsDic[key]++;
            }

            whenBuyComplete.Execute(args);

            return PurchaseProcessingResult.Complete;
        }
        else
        {
            SendLog("상품 구매 실패", args.purchasedProduct.definition.id);

            MyDebug(string.Format("ProcessPurchase: Pending. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID.ToString()));
            return PurchaseProcessingResult.Pending;
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        activeBuyButton.Execute();
        MyDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    private void MyDebug(string debug)
    {
        Debug.Log(debug);
    }



}
