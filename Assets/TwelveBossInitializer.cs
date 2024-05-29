using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TwelveBossData_Fancy
{
   public int DataIdx;
   public string DataName;
   public double DataScore;
   public string DataReward;
   public TwelveBossData_Fancy(int dataIdx,string dataName,double dataScore,string dataReward)
   {
      DataIdx = dataIdx;
      DataName = dataName;
      DataScore = dataScore;
      DataReward = dataReward;
   }
}
public class TwelveBossInitializer : FancyScrollView<TwelveBossData_Fancy>
{
   private void OnEnable()
   {
      #if UNITY_EDITOR

      #else
         gameObject.SetActive(false);
      #endif
   }
   [SerializeField]
   private Scroller scroller;
    
    
   [SerializeField] GameObject cellPrefab = default;

   protected override GameObject CellPrefab => cellPrefab;

   [SerializeField] private TextMeshProUGUI ItemValue;

   [SerializeField] private TextMeshProUGUI ItemDesc;

   private TwelveBossData_Fancy selectedData;
   [SerializeField]
   private TMP_InputField adjustNum;
   
   List<TwelveBossData_Fancy> userInfoTableDatas = new List<TwelveBossData_Fancy>();
   List<TwelveBossData_Fancy> userInfoTable2Datas = new List<TwelveBossData_Fancy>();
   List<TwelveBossData_Fancy> goodsTableDatas = new List<TwelveBossData_Fancy>();
   public void SetData(TwelveBossData_Fancy data)
   {
      selectedData = data;
      SetUi();
   }

   private void SetUi()
   {
            ItemValue.SetText($"{Utils.ConvertNum(ServerData.userInfoTable.GetTableData(selectedData.DataName).Value)}");
   }
   
   private void Start()
   {
      scroller.Initialize(TypeScroll.None);
            
      scroller.OnValueChanged(UpdatePosition);

      SetTableData();

      this.UpdateContents(userInfoTableDatas.ToArray());
      scroller.SetTotalCount(userInfoTableDatas.Count);
   }

   private void SetTableData()
   {
      using var userInfoKeys = ServerData.userInfoTable.TableDatas.Keys.GetEnumerator();
      using var userInfoValues = ServerData.userInfoTable.TableDatas.Values.GetEnumerator();
    
      int i = 0;
      while (userInfoKeys.MoveNext()&&userInfoValues.MoveNext())
      {
         if (userInfoValues.Current != null)
         {
            //userInfoTableDatas.Add(new TwelveBossData_Fancy(TableName.UserInfoTable, i,userInfoKeys.Current, userInfoValues.Current.Value,this));
            i++;
         }
      }
      

      SetData(userInfoTableDatas.First());
   }
   
   public void OnClickTable(int idx)
   {
      scroller.Initialize(TypeScroll.None);
            
      scroller.OnValueChanged(UpdatePosition);
      // switch (tableIdx)
      // {
      //    //case TableName.UserInfoTable:
      //       UpdateContents(userInfoTableDatas.ToArray());
      //       scroller.SetTotalCount(userInfoTableDatas.Count);
      //       selectedData = userInfoTableDatas.First();
      //       break;
      //    default:
      //       throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
      // }
      Refresh();
      SetUi();
   }

   public void OnClickLocal()
   {
      #if UNITY_EDITOR
      if (int.TryParse(adjustNum.text, out var inputNum))
      {
         if (inputNum < 0)
         {
            PopupManager.Instance.ShowAlarmMessage(CommonString.InstantClear_Minus);
            return;
         }
         if (inputNum == 0)
         {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
         }
      }
      else
      {
         PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
         return;
      }

            //ServerData.userInfoTable.GetTableData(selectedData.DataName).Value = inputNum;
      
      SetUi();
      #endif
   }
   
   public void OnClickServer()
   {
      #if UNITY_EDITOR
      if (int.TryParse(adjustNum.text, out var inputNum))
      {
         if (inputNum == 0)
         {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
         }
      }
      else
      {
         PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
         return;
      }

            
            ServerData.userInfoTable.GetTableData(selectedData.DataName).Value = inputNum;
            ServerData.userInfoTable.UpData(selectedData.DataName,false);
      SetUi();
      #endif
     
   }
   
}


