using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UiTestCell2 : FancyCell<TestCellData_Fancy2>
{
   TestCellData_Fancy2 itemData;
   
   [SerializeField]
   private TextMeshProUGUI descriptionText;

   private CompositeDisposable disposables = new CompositeDisposable();
   
   
   public void UpdateUi(TestCellData_Fancy2 passInfo)
   {
      this.itemData = passInfo;

      SetText();
      
      
   }

   private void SetText()
   {
      descriptionText.SetText($"{TableManager.Instance.TwelveBossTable.dataArray[itemData.DataIdx].Name}({itemData.DataName})");}
   public override void UpdateContent(TestCellData_Fancy2 itemData)
   {
      if (this.itemData != null && this.itemData.DataIdx == itemData.DataIdx&&this.itemData.TableName==itemData.TableName)
      {
         return;
      }

      this.itemData = itemData;

//        Debug.LogError("DolpasS!");
        
      UpdateUi(this.itemData);
   }

   float currentPosition = 0;
   [SerializeField] Animator animator = default;

   static class AnimatorHash
   {
      public static readonly int Scroll = Animator.StringToHash("scroll");
   }

   public void OnClickCell()
   {
      itemData.ParentBoard.SetData(this.itemData);
   }
   
   public override void UpdatePosition(float position)
   {
      currentPosition = position;

      if (animator.isActiveAndEnabled)
      {
         animator.Play(AnimatorHash.Scroll, -1, position);
      }

      animator.speed = 0;
   }

   void OnEnable() => UpdatePosition(currentPosition);
}


