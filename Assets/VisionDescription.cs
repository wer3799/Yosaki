using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class VisionDescription : MonoBehaviour
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

        ServerData.goodsTable.GetTableData(GoodsTable.VisionTreasure).AsObservable().Subscribe(e =>
        {
            if (useSpace)
            {
            marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.VisionTreasure)} 1개당\n {CommonString.GetStatusName(StatusType.EnhanceVisionSkill)} {GameBalance.VisionTreasurePerDamage * 100f}");
                
            }
            else
            {
                
            marbleDescription.SetText($"{CommonString.GetItemName(Item_Type.VisionTreasure)} 1개당 {CommonString.GetStatusName(StatusType.EnhanceVisionSkill)} {GameBalance.VisionTreasurePerDamage * 100f}");
            }
        }).AddTo(this);

    }
}
