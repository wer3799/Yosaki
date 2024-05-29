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

public class TestCellData_Fancy2
{
   public UiTestBoard2 ParentBoard;
   public UiTestBoard2.TableName2 TableName;
   public int DataIdx;
   public string DataName;
   public string DataValue;
   public TestCellData_Fancy2( UiTestBoard2.TableName2 tableName, int dataIdx,string dataName,string dataValue,UiTestBoard2 parentBoard)
   {
      TableName = tableName;
      DataIdx = dataIdx;
      DataName = dataName;
      DataValue = dataValue;
      ParentBoard = parentBoard;
   }
}
public class UiTestBoard2 : FancyScrollView<TestCellData_Fancy2>
{
   public enum TableName2
   {
      TwelveBossTable,
   }
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


   private TestCellData_Fancy2 selectedData;
   
   List<TestCellData_Fancy2> twelveBossTableDatas = new List<TestCellData_Fancy2>();
   public void SetData(TestCellData_Fancy2 data)
   {
      selectedData = data;
      SetUi();
   }

   private void SetUi()
   {
      switch (selectedData.TableName)
      {
         case TableName2.TwelveBossTable:
            ItemValue.SetText($"{ServerData.bossServerTable.TableDatas[selectedData.DataName].ConvertToString()}");
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
   
   private void Start()
   {
      scroller.Initialize(TypeScroll.None);
            
      scroller.OnValueChanged(UpdatePosition);

      SetTableData();

      this.UpdateContents(twelveBossTableDatas.ToArray());
      scroller.SetTotalCount(twelveBossTableDatas.Count);
   }

   private void SetTableData()
   {
      int i = 0;
      
      using var twelveBossKeys = ServerData.bossServerTable.TableDatas.Keys.GetEnumerator();
      using var twelveBossValues = ServerData.bossServerTable.TableDatas.Values.GetEnumerator();
    
      while (twelveBossKeys.MoveNext()&&twelveBossValues.MoveNext())
      {
         if (twelveBossValues.Current != null)
         {
            twelveBossTableDatas.Add(new TestCellData_Fancy2(TableName2.TwelveBossTable, i,twelveBossKeys.Current, twelveBossValues.Current.ConvertToString(),this));
            i++;
         }
      }

      SetData(twelveBossTableDatas.First());
   }
   
   public void OnClickTable(int idx)
   {
      TableName2 tableIdx = (TableName2)idx;
      scroller.Initialize(TypeScroll.None);
            
      scroller.OnValueChanged(UpdatePosition);
      switch (tableIdx)
      {
         case TableName2.TwelveBossTable:
            UpdateContents(twelveBossTableDatas.ToArray());
            scroller.SetTotalCount(twelveBossTableDatas.Count);
            selectedData = twelveBossTableDatas.First();
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
      }
      Refresh();
      SetUi();
   }

   public void OnClickLocal()
   {
      #if UNITY_EDITOR

      switch (selectedData.TableName)
      {
         case TableName2.TwelveBossTable:
            ServerData.bossServerTable.TableDatas[selectedData.DataName].rewardedId.Value = "";
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
      
      SetUi();
      #endif
   }
   
   public void OnClickServer()
   {
      #if UNITY_EDITOR

      switch (selectedData.TableName)
      {
         case TableName2.TwelveBossTable:
            ServerData.bossServerTable.TableDatas[selectedData.DataName].rewardedId.Value = "";
            ServerData.bossServerTable.UpdateData(selectedData.DataName);
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
      SetUi();
      #endif
     
   }
   
}


