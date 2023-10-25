using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class MonthlyTrainingCell : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI desc;

   public void SetText(string mainDesc, string subDesc)
   {
      string str = "";
      if (mainDesc.IsNullOrEmpty() == false)
      {
         str += mainDesc;
      }
      if (subDesc.IsNullOrEmpty() == false)
      {
         str += $"\n<size=15><color=yellow>{subDesc}</size></color>";
      }
      desc.SetText(str);
   }

}
