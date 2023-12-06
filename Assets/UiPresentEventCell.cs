using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPresentEventCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goodsAmount;
    [SerializeField] private Image goodsImage;
    
    [SerializeField] private Image buttonImage;
    [SerializeField] private List<Sprite> gradeImage;

    [SerializeField] private GameObject lockObject;
    
    private GachaEventData tableData;

    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].AsObservable().Subscribe(
            e =>
            {
                lockObject.SetActive(ServerData.etcServerTable.GachaEventRewarded(tableData.Id));
            }).AddTo(this);
    }
    public void Initialize(GachaEventData _data)
    {
        tableData = _data;

        goodsImage.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);
        
        goodsAmount.SetText($"{Utils.ConvertNum(tableData.Itemvalue)}");

        buttonImage.sprite = gradeImage[tableData.Grade];
        
        Subscribe();
    }
}
