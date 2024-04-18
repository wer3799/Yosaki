using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class TreasureDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleDescription;


    [SerializeField]
    private bool useSpace = true;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {

        ServerData.goodsTable.GetTableData(GoodsTable.ChunguTreasure).AsObservable().Subscribe(e =>
        {
            if (useSpace)
            {
                marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.ChunguTreasure)} 1개당\n {CommonString.GetStatusName(StatusType.SuperCritical20DamPer)} 효과 {Utils.ConvertNum(GameBalance.chunguAbil * 100f,2)}% 강화");
                
            }
            else
            {
                
                marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.ChunguTreasure)} 1개당 {CommonString.GetStatusName(StatusType.SuperCritical20DamPer)} 효과 {Utils.ConvertNum(GameBalance.chunguAbil * 100f,2)}% 강화");
            }
        }).AddTo(this);

    }
}
