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

    [Header("reward")]
    [SerializeField] private TextMeshProUGUI rewardText;
    private DimensionEquipData tableData;
    [SerializeField] private Animator animator;
    public void Initialize(DimensionEquipData _tableData, bool isChangeCell=false)
    {
        tableData = _tableData;
        
       nameText.SetText($"{tableData.Name}");
       gradeText.SetText($"{CommonUiContainer.Instance.ItemGradeName_SealSword[tableData.Grade]}");
       levelText.SetText($"{5-tableData.Level}등급");
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

       var buffValue = 1 + PlayerStats.GetDimensionCubeGainPer();


       itemView.Initialize((Item_Type)tableData.Decompositiontype, (int)(tableData.Decompositionvalue * buffValue));
       
       equipmentImage.sprite=CommonResourceContainer.GetDimensionEquipmentSprite(tableData.Id);
       
       if (rewardText == null) return;
       if (isChangeCell)
       {
           rewardText.SetText("교체");
           animator.enabled = false;
           itemView.gameObject.SetActive(false);
           equipmentImage.color=new Color(1f,1f,1f,1f);
       }
       else
       {
           rewardText.SetText("분해");
           animator.enabled = true;
           itemView.gameObject.SetActive(true);
       }

    }
}
