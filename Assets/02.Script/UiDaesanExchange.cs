                                              
using UnityEngine;
using WebSocketSharp;

public class UiDaesanExchange : MonoBehaviour
{
    
    [SerializeField]
    private Transform weeklyCellParent;
    [SerializeField]
    private Transform normalCellParent;
    [SerializeField]
    private UiDaesanShopCell shopCell;


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.DaesanExchange.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Active == false) continue;
            if (tableData[i].Exchangekey.IsNullOrEmpty() == true)
            {
                var cell = Instantiate(shopCell, normalCellParent);
                cell.Initialize(i);
            }
            else
            {
                var cell = Instantiate(shopCell, weeklyCellParent);
                cell.Initialize(i);
            }

        }
    }
}
