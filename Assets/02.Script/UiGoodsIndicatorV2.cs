using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
public class UiGoodsIndicatorV2 : MonoBehaviour
{
    [SerializeField]
    private string goodsKey;

    [SerializeField]
    private TextMeshProUGUI goodsText;

    [SerializeField]
    private Button clickButton;

    [SerializeField] private Image goodsImage;

    [SerializeField] private Color textColor=Color.white;
    [SerializeField] private bool initializeByInspector = true;
    private int itemIdx = -1;
    void Start()
    {
        if (initializeByInspector)
        {
            Initialize();
        }
        else
        {
            
        }
    }

    private void Initialize()
    {
        if (goodsImage != null)
        {
            var type = ServerData.goodsTable.ServerStringToItemType(goodsKey);
            goodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(type);
            goodsText.color = textColor;
        }
        Subscribe();
    }
    public void Initialize(string key)
    {
        goodsKey = key;
        
        if (goodsImage != null)
        {
            var type = ServerData.goodsTable.ServerStringToItemType(goodsKey);
            goodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(type);
            goodsText.color = textColor;
        }
        
        Subscribe();
    }
    
    private void AddDescription()
    {
        if (clickButton == null)
        {
            if (this.GetComponent<Button>() != null)
            {
                clickButton = this.GetComponent<Button>();
            }
            else
            {
                clickButton = this.gameObject.AddComponent<Button>();
            }
            clickButton.onClick.AddListener(OnClickButton);
        }
    }

    private void Subscribe()
    {
        if (ServerData.goodsTable.TableDatas.ContainsKey(goodsKey))
        {
            ServerData.goodsTable.GetTableData(goodsKey).AsObservable().Subscribe(goods =>
            {
                goodsText.SetText($"{Utils.ConvertBigNum(goods).ToString()}");
            }).AddTo(this);
        }
    }

    public void OnClickButton()
    {
        //item 없을떄
        if (itemIdx == -1)
        { 
            Item_Type a = ServerData.goodsTable.ServerStringToItemType(goodsKey);
            var tableData = TableManager.Instance.choboTable.dataArray;

            for (int i = 0; i < tableData.Length; i++)
            {
                if ((Item_Type)tableData[i].Itemtype == a)
                {
                    itemIdx = -1;
                    PopupManager.Instance.ShowConfirmPopup($"{CommonString.GetItemName((Item_Type)tableData[i].Itemtype)}", $"{tableData[i].Description0}", null);
                    break;
                }    
            }
            
        }
        //item 찾아놓음
        else
        {
            var tableData = TableManager.Instance.choboTable.dataArray;
            PopupManager.Instance.ShowConfirmPopup($"{CommonString.GetItemName((Item_Type)tableData[itemIdx].Itemtype)}", $"{tableData[itemIdx].Description0}", null);
        }

    }
}
