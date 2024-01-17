using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Spine.Unity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiMunHaBoard : MonoBehaviour
{
   [SerializeField] private UiMunhaSkillCell prefab;
   [SerializeField] private Transform parent;

   [SerializeField] private TextMeshProUGUI abilityText;

   [SerializeField] private TextMeshProUGUI gradeText;
   
   [SerializeField] private TextMeshProUGUI timeText;
   
   [SerializeField] private TextMeshProUGUI priceText;
   
   [SerializeField] private TextMeshProUGUI buttonText;
   [SerializeField] private GameObject ButtonObject;
   
   [SerializeField]private SkeletonGraphic costumeGraphic;

   private int currentIdx = 0;
   private DateTime targetTime;

   private void Start()
   {
      Initialize();

      UpdateUi();
      
      Subscribe();
   }

   private void Subscribe()
   {
      Observable.Interval(TimeSpan.FromSeconds(1))
         .Subscribe(_ => UpdateTimeText())
         .AddTo(this);

   }
   private void Initialize()
   {
      currentIdx = Math.Max(0, (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value);
      
      MakeSkillCells();
   }

   private void UpdateUi()
   {
      SetGradeText();
      SetAbilText();
      SetPriceText();
      UpdateTimeText();
      SetCostume();
   }
   private void SetCostume()
   {
      var data = TableManager.Instance.StudentTable.dataArray[currentIdx];
      
      costumeGraphic.Clear();

      costumeGraphic.gameObject.SetActive(true);
      costumeGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[data.Change_Costume];
      costumeGraphic.Initialize(true);
      costumeGraphic.SetMaterialDirty();
   }
   private void SetPriceText()
   {
      var desc = "";

      var level = Math.Max(0,(int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value);
      
      var tableData = TableManager.Instance.StudentTable.dataArray;

      var nextLevel = level + 1;
      
      if (level >= tableData.Length - 1)
      {
         priceText.SetText("Max");
      }
      else
      {
         desc+=$"{Utils.ConvertNum(tableData[nextLevel].Conditoin_Value)}";
      
         priceText.SetText("레벨업\n"+desc);   
      }

      
   }
   
   private void SetGradeText()
   {
      var tableData = TableManager.Instance.StudentTable.dataArray[currentIdx];

      var str = "";
      str += $"Lv.{currentIdx + 1} {tableData.Student_Name}";
      if (currentIdx == (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value)
      {
         str += "\n<color=yellow>(현재 레벨)</color>";
      }

      gradeText.SetText(str);
   }
   private void SetAbilText()
   {
      var tableData = TableManager.Instance.StudentTable.dataArray[currentIdx];

      var str = "";
      str += $"{CommonString.GetStatusName((StatusType)tableData.Abil_Type)} {Utils.ConvertNum(tableData.Abil_Value*100)}";

      abilityText.SetText(str);
   }

   private void MakeSkillCells()
   {
      var tableData = TableManager.Instance.SkillTable.dataArray;

      for (int i = 0; i < tableData.Length; i++)
      {
         if (tableData[i].SKILLCASTTYPE != SkillCastType.Student) continue;

         var cell = Instantiate(prefab, parent);
         cell.Initialize(tableData[i]);
      }
   }
   
   public void OnClickRightButton()
   {
      var tableData = TableManager.Instance.StudentTable.dataArray;

      currentIdx++;

      if (currentIdx == tableData.Length)
      {
         PopupManager.Instance.ShowAlarmMessage("최고단계 입니다.");
      }

      currentIdx = Mathf.Min(currentIdx, tableData.Length - 1);

      UpdateUi();
   }

   public void OnClickLeftButton()
   {
      currentIdx--;

      if (currentIdx <= 0)
      {
         PopupManager.Instance.ShowAlarmMessage("처음단계 입니다.");
      }

      currentIdx = Mathf.Max(currentIdx, 0);

      UpdateUi();
   }
 public void OnClickLevelUpButton()
    {

        var currentlv = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value;
        var tableData = TableManager.Instance.StudentTable.dataArray;
        if (currentlv + 1 >= tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다.");

            return;
        }

        var nextData = tableData[currentlv + 1];

        if (ServerData.goodsTable.GetTableData(GoodsTable.SB).Value < nextData.Conditoin_Value)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SB)}가 부족합니다.");

            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.SB).Value -= nextData.Conditoin_Value;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value++;

        currentIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value;
        
        UpdateUi();
        
        if (syncRoutine != null)
        {
           CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
 
 private WaitForSeconds syncDelay = new WaitForSeconds(0.6f);

 private Coroutine syncRoutine;
 private IEnumerator SyncRoutine()
 {
    yield return syncDelay;

    Debug.LogError($"@@@@@@@@@@@@@@@Munha SyncComplete@@@@@@@@@@@@@@");

    List<TransactionValue> transaction = new List<TransactionValue>();

    Param goodsParam = new Param();
    goodsParam.Add(GoodsTable.SB,ServerData.goodsTable.GetTableData(GoodsTable.SB).Value);
        
        
    Param userinfo2Param = new Param();
    userinfo2Param.Add(UserInfoTable_2.munhaLevel,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value);

    transaction.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
    transaction.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));
        
    ServerData.SendTransactionV2(transaction,successCallBack:(() =>
    {
    }));
 }
 
 private void UpdateTimeText()
 {
    SetTargetTime();
    //명상 안누름
    if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaDispatchStartTime).Value < 0)
    {
       timeText.SetText($"");
       buttonText.SetText("훈련");
       ButtonObject.SetActive(true);
       return;
    }

    TimeSpan timeRemaining = targetTime - DateTime.Now;  
    
    if (timeRemaining.TotalSeconds > 3600)
    {
       string formattedTime = string.Format("훈련중\n<color=white>남은시간 {0:D2}시:{1:D2}분</color>", timeRemaining.Hours, timeRemaining.Minutes);
       timeText.text = formattedTime;
       ButtonObject.SetActive(false);
    }
    else if(timeRemaining.TotalSeconds > 0)
    {
       string formattedTime = string.Format("훈련중\n<color=white>남은시간 {0:D2}분:{1:D2}초</color>", timeRemaining.Minutes, timeRemaining.Seconds);
       timeText.SetText(formattedTime);
       ButtonObject.SetActive(false);
    }
    else
    {
       timeText.SetText($"남은시간\n<color=white>00분:00초</color>");
       buttonText.SetText("보상 획득");
       ButtonObject.SetActive(true);
    }
 }
 
 private void SetTargetTime()
 {
    var startTime =
       Utils.ConvertFromUnixTimestamp(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaDispatchStartTime).Value);
    targetTime = startTime.AddHours(GameBalance.MunhaDispatchHour);
 }
}