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

public class TestCellData_Fancy
{
   public UiTestBoard ParentBoard;
   public UiTestBoard.TableName TableName;
   public int DataIdx;
   public string DataName;
   public double DataValue;
   public TestCellData_Fancy( UiTestBoard.TableName tableName, int dataIdx,string dataName,double dataValue,UiTestBoard parentBoard)
   {
      TableName = tableName;
      DataIdx = dataIdx;
      DataName = dataName;
      DataValue = dataValue;
      ParentBoard = parentBoard;
   }
}
public class UiTestBoard : FancyScrollView<TestCellData_Fancy>
{
   public enum TableName
   {
      UserInfoTable,
      UserInfoTable2,
      GoodsTable
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

   [SerializeField] private TextMeshProUGUI ItemDesc;

   private TestCellData_Fancy selectedData;
   [SerializeField]
   private TMP_InputField adjustNum;
   
   List<TestCellData_Fancy> userInfoTableDatas = new List<TestCellData_Fancy>();
   List<TestCellData_Fancy> userInfoTable2Datas = new List<TestCellData_Fancy>();
   List<TestCellData_Fancy> goodsTableDatas = new List<TestCellData_Fancy>();
   public void SetData(TestCellData_Fancy data)
   {
      selectedData = data;
      SetUi();
   }

   private void SetUi()
   {
      switch (selectedData.TableName)
      {
         case TableName.UserInfoTable:
            ItemValue.SetText($"{Utils.ConvertNum(ServerData.userInfoTable.GetTableData(selectedData.DataName).Value)}");
            break;
         case TableName.UserInfoTable2:
            ItemValue.SetText($"{Utils.ConvertNum(ServerData.userInfoTable_2.GetTableData(selectedData.DataName).Value)}");
            break;
         case TableName.GoodsTable:
            ItemValue.SetText($"{Utils.ConvertNum(ServerData.goodsTable.GetTableData(selectedData.DataName).Value)}");
            ItemDesc.SetText($"{CommonString.GetItemName(ServerData.goodsTable.ServerStringToItemType(selectedData.DataName))}");
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
            userInfoTableDatas.Add(new TestCellData_Fancy(TableName.UserInfoTable, i,userInfoKeys.Current, userInfoValues.Current.Value,this));
            i++;
         }
      }
      
      using var userInfo2Keys = ServerData.userInfoTable_2.TableDatas.Keys.GetEnumerator();
      using var userInfo2Values = ServerData.userInfoTable_2.TableDatas.Values.GetEnumerator();
    
      i = 0;
      while (userInfo2Keys.MoveNext()&&userInfo2Values.MoveNext())
      {
         if (userInfo2Values.Current != null)
         {
            userInfoTable2Datas.Add(new TestCellData_Fancy(TableName.UserInfoTable2, i,userInfo2Keys.Current, userInfo2Values.Current.Value,this));
            i++;
         }
      }
      
      using var goodsKeys = ServerData.goodsTable.TableDatas.Keys.GetEnumerator();
      using var goodsValues = ServerData.goodsTable.TableDatas.Values.GetEnumerator();
    
      i = 0;
      while (goodsKeys.MoveNext()&&goodsValues.MoveNext())
      {
         if (goodsValues.Current != null)
         {
            goodsTableDatas.Add(new TestCellData_Fancy(TableName.GoodsTable, i,goodsKeys.Current, goodsValues.Current.Value,this));
            i++;
         }
      }

      SetData(userInfoTableDatas.First());
   }
   
   public void OnClickTable(int idx)
   {
      TableName tableIdx = (TableName)idx;
      scroller.Initialize(TypeScroll.None);
            
      scroller.OnValueChanged(UpdatePosition);
      switch (tableIdx)
      {
         case TableName.UserInfoTable:
            UpdateContents(userInfoTableDatas.ToArray());
            scroller.SetTotalCount(userInfoTableDatas.Count);
            selectedData = userInfoTableDatas.First();
            break;
         case TableName.UserInfoTable2:
            UpdateContents(userInfoTable2Datas.ToArray());
            scroller.SetTotalCount(userInfoTable2Datas.Count);
            selectedData = userInfoTable2Datas.First();
            break;
         case TableName.GoodsTable:
            UpdateContents(goodsTableDatas.ToArray());
            scroller.SetTotalCount(goodsTableDatas.Count);
            selectedData = goodsTableDatas.First();
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
      if (double.TryParse(adjustNum.text, out double inputNum))
      {
      }
      else
      {
         PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
         return;
      }

      switch (selectedData.TableName)
      {
         case TableName.UserInfoTable:
            ServerData.userInfoTable.GetTableData(selectedData.DataName).Value = inputNum;
            break;
         case TableName.UserInfoTable2:
            ServerData.userInfoTable_2.GetTableData(selectedData.DataName).Value = inputNum;
            break;
         case TableName.GoodsTable:
            ServerData.goodsTable.GetTableData(selectedData.DataName).Value = (float)inputNum;
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
      if (float.TryParse(adjustNum.text, out var inputNum))
      {
      }
      else
      {
         PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
         return;
      }

      switch (selectedData.TableName)
      {
         case TableName.UserInfoTable:
            
            ServerData.userInfoTable.GetTableData(selectedData.DataName).Value = inputNum;
            ServerData.userInfoTable.UpData(selectedData.DataName,false);
            break;
         case TableName.UserInfoTable2:
            ServerData.userInfoTable_2.GetTableData(selectedData.DataName).Value = inputNum;
            ServerData.userInfoTable_2.UpData(selectedData.DataName,false);
            break;
         case TableName.GoodsTable:
            
            ServerData.goodsTable.GetTableData(selectedData.DataName).Value = inputNum;
            ServerData.goodsTable.UpData(selectedData.DataName,false);
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
      SetUi();
      #endif
     
   }
   
}


