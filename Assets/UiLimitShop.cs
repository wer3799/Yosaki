using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLimitShop : MonoBehaviour
{
    [SerializeField]
    private UiIapItemCell iapCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        using var e = TableManager.Instance.InAppPurchaseData.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit1) continue;
            if (e.Current.Value.SHOPCATEGORY == ShopCategory.Gem) continue;
            var cell = Instantiate<UiIapItemCell>(iapCellPrefab, cellParent);
            cell.Initialize(e.Current.Value);
        }
    }
}
