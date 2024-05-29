using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UiPackageShop : MonoBehaviour
{
    [SerializeField]
    private UiIapItemCell iapCellPrefab;

    [SerializeField]
    private UiLimitedCostumePackageCell costumeCellPrefab;

    [SerializeField]
    private Transform gemCategoryParent;

    [SerializeField]
    private Transform package1Parent;

    [SerializeField]
    private Transform package2Parent;

    [SerializeField]
    private Transform petCostumeParent;

    [FormerlySerializedAs("relicParent")] [SerializeField]
    private List<Transform> costumePetWeapon;

    [SerializeField]
    private Transform eventParent;

    [SerializeField]
    private Transform springEventParent;

    [SerializeField]
    private Transform chunFlower;
    
    //
    [SerializeField]
    private Transform Goods_BaseGoods; 
    [SerializeField]
    private Transform Goods_GrowthStone;
    
    [SerializeField]
    private Transform Goods_SpecialGoods;
    
    [SerializeField]
    private Transform Goods_NewGacha;
    
    [SerializeField]
    private Transform Goods_Week;
    [SerializeField]
    private Transform Goods_Month;
    
    [SerializeField]
    private Transform Goods_PetSoul;
    
    [SerializeField]
    private Transform Goods_FoxFire;
    [SerializeField]
    private Transform Goods_SealSword;

    private void Start()
    {
        Initialize();
    }
    private bool IsSellPeriod(InAppPurchaseData inAppPurchaseData)
    {
        var splitData = inAppPurchaseData.Sellperiod.Split('-');

        DateTime sellPeriod =
            new DateTime(int.Parse(splitData[0]), int.Parse(splitData[1]), int.Parse(splitData[2]));
        sellPeriod = sellPeriod.AddDays(1);//5월5일을 넣으면 5월6일00시에끝나야함.
        var result = DateTime.Compare(ServerData.userInfoTable.currentServerTime, sellPeriod);

        
        switch (result)
        {
            //아직 안지남
            case -1 :
            case 0:
                return true;
            //지남
            case 1:
                return false;
            default:
                return false;
        }
    }
    private void Initialize()
    {
        using var e = TableManager.Instance.InAppPurchaseData.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.Active == false) continue;
            //기한이 아니면 continue
            if (IsSellPeriod(e.Current.Value) == false) continue;
            if (e.Current.Value.SHOPCATEGORY == ShopCategory.Gem)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, gemCategoryParent);
                cell.Initialize(e.Current.Value);

            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit1)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, package1Parent);
                cell.Initialize(e.Current.Value);
            }

            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit2)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, package2Parent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Costume)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, costumePetWeapon[0]);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Pet)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, costumePetWeapon[1]);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit3)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, costumePetWeapon[2]);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Event2)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, eventParent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Event3)
            {
                if (e.Current.Value.Productid == "pinwheelset0")
                {
                    //currentserver가 3월이후(포함)
                    if (ServerData.userInfoTable.currentServerTime.Month >= 5)
                    {
                        continue;
                    }
                }
#if UNITY_EDITOR
#else

                if (e.Current.Value.Productid == "newyearset0" || e.Current.Value.Productid == "newyearset1" )
                {
                    //1월 20일 전에는 생성 x
                    if (ServerData.userInfoTable.currentServerTime.Month == 1 &&
                        ServerData.userInfoTable.currentServerTime.Day < 20)
                    {
                        continue;
                    }
                }
#endif

                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, springEventParent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.ChunFlower)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, chunFlower);
                cell.Initialize(e.Current.Value);//
            }
            //
            
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_BaseGoods)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_BaseGoods);
                cell.Initialize(e.Current.Value);//
            }  
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_GrowthStone)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_GrowthStone);
                cell.Initialize(e.Current.Value);//
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_SpecialGoods)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_SpecialGoods);
                cell.Initialize(e.Current.Value);//
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_NewGacha)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_NewGacha);
                cell.Initialize(e.Current.Value);//
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_Week)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_Week);
                cell.Initialize(e.Current.Value);//
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_PetSoul)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_PetSoul);
                cell.Initialize(e.Current.Value);//
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_FoxFire)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_FoxFire);
                cell.Initialize(e.Current.Value);//
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_InstantClear)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_SealSword);
                cell.Initialize(e.Current.Value);//
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Goods_Month)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, Goods_Month);
                cell.Initialize(e.Current.Value);//
            }
            
        }

        var costumeTableData = TableManager.Instance.Costume.dataArray;

        for (int i = 0; i < costumeTableData.Length; i++)
        {
            if(costumeTableData[i].Price<1) continue;
            
            var cell = Instantiate<UiLimitedCostumePackageCell>(costumeCellPrefab, costumePetWeapon[3]);
            cell.Initialize(costumeTableData[i]);
        }
        var petTableData = TableManager.Instance.PetTable.dataArray;

        for (int i = 0; i < petTableData.Length; i++)
        {
            if(petTableData[i].Shopprice<0) continue;
            
            var cell = Instantiate<UiLimitedCostumePackageCell>(costumeCellPrefab, costumePetWeapon[4]);
            cell.Initialize(petTableData[i]);
        }
    }
}
