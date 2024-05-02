using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DimensionEquiepmentCollectionCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI equipEffectText;

    [SerializeField] private Image equipmentImage;
    [SerializeField] private Image bgImage;
    [SerializeField] private ItemView itemView;

    private DimensionEquipData tableData;

    public void Initialize(DimensionEquipData _tableData)
    {
        tableData = _tableData;
        
       nameText.SetText($"{tableData.Name} {tableData.Level}");
       gradeText.SetText($"{CommonUiContainer.Instance.ItemGradeName_Weapon[tableData.Grade]}");
       levelText.SetText($"{tableData.Level}등급");
       bgImage.sprite = CommonUiContainer.Instance.itemGradeFrame[tableData.Grade];
       string effect = "";
       var type1 = (DimensionStatusType)tableData.Equipeffecttype1;
       var type2 = (DimensionStatusType)tableData.Equipeffecttype2;
       var type3 = (DimensionStatusType)tableData.Equipeffecttype3;
       if (type1 != DimensionStatusType.None)
       {
           if (type1.IsPercentStat())
           {
            effect += $"{CommonString.GetStatusName(type1)} {Utils.ConvertNum(tableData.Equipeffectvalue1*100,2)}";
           }
           else
           {
            effect += $"{CommonString.GetStatusName(type1)} {Utils.ConvertNum(tableData.Equipeffectvalue1,2)}";
           }
       }

       if (type2 != DimensionStatusType.None)
       {      
           if (type2.IsPercentStat())
           {
               effect += $"\n{CommonString.GetStatusName(type2)} {Utils.ConvertNum(tableData.Equipeffectvalue2*100,2)}";
           }
           else
           {
               effect += $"\n{CommonString.GetStatusName(type2)} {Utils.ConvertNum(tableData.Equipeffectvalue2, 2)}";
           }
       }

       if (type3 != DimensionStatusType.None)
       {
           if (type3.IsPercentStat())
           {
               effect += $"\n{CommonString.GetStatusName(type3)} {Utils.ConvertNum(tableData.Equipeffectvalue3 * 100, 2)}";
           }
           else
           {
               effect += $"\n{CommonString.GetStatusName(type3)} {Utils.ConvertNum(tableData.Equipeffectvalue3, 2)}";
           }
       }

       equipEffectText.SetText(effect);
       
       itemView.Initialize((Item_Type)tableData.Decompositiontype,tableData.Decompositionvalue);
       
       equipmentImage.sprite=CommonResourceContainer.GetDimensionEquipmentSprite(tableData.Id);
    }
}
