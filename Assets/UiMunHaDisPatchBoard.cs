using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiMunHaDisPatchBoard : MonoBehaviour
{
    [SerializeField] private ItemView prefab;

    [SerializeField] private Transform parent;
    
    [SerializeField] private TextMeshProUGUI timeText;

     [SerializeField] private GameObject dispatchButton;
     [FormerlySerializedAs("petDispatchButtonText")] [SerializeField] private TextMeshProUGUI dispatchButtonText;

    private DateTime targetTime;
    private List<ItemView> cellContainer = new List<ItemView>();
    private int currentIdx = 0;
    [SerializeField] private TextMeshProUGUI gradeText;
    private CompositeDisposable disposable = new CompositeDisposable();
    [SerializeField] private UiRewardResultView _uiRewardResultView;

    // Start is called before the first frame update
    void Start()
    {
        currentIdx = PlayerStats.GetMunhaDispatchGrade();
        
        UpdateUi();
    }
    private void Subscribe()
    {
        disposable.Clear();

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => UpdateTimeText())
            .AddTo(disposable);


    }

    public void UpdateUi()
    {
        UpdateText();
        UpdateTimeText();
    }
    public void UpdateText()
    {
        string desc = "";
        
        desc += $"파견 {currentIdx + 1}단계";

        var tableData = TableManager.Instance.StudentDispatch.dataArray;
        
        var equipIdx = PlayerStats.GetMunhaDispatchGrade();;
        
        var currentData = tableData[currentIdx];

        desc += $"\n<color=orange>레벨({currentData.Minlevel + 1}~{currentData.Maxlevel + 1})</color>";
        
        if (currentIdx == equipIdx)
        {
            desc += $"\n<color=yellow>현재 단계</color>";
        }
        
        gradeText.SetText(desc);

        string result = string.Empty;

        int maxGrade = 0;
        int minGrade = 0;

        int slotNum = 0;

        for (int i = 0; i < currentData.Rewardtype.Length; i++)
        {
            if (currentData.Rewardtype[i] == -1)
            {
                break;
            }

            slotNum++;
        }

        while (cellContainer.Count < slotNum)
        {
            ItemView rewardCell = Instantiate<ItemView>(prefab, parent);
            cellContainer.Add(rewardCell);
        }

        for (int i = 0; i < cellContainer.Count; i++)
        {
            if (i < slotNum)
            {
                RewardItem rewardItem = new RewardItem((Item_Type)currentData.Rewardtype[i], currentData.Rewardvalue[i]);
                cellContainer[i].gameObject.SetActive(true);
                cellContainer[i].Initialize(rewardItem.ItemType,rewardItem.ItemValue);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(false);
            }
        }
    }
    public void OnClickRightButton()
    {
        var tableData = TableManager.Instance.StudentDispatch.dataArray;

        currentIdx++;

        if (currentIdx == tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("최고단계 입니다.");
        }

        currentIdx = Mathf.Min(currentIdx, tableData.Length - 1);

        UpdateText();
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        if (currentIdx <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("처음단계 입니다.");
        }

        currentIdx = Mathf.Max(currentIdx, 0);

        UpdateText();
    }
    
     public void OnClickDispatchButton()
    {
        var grade = PlayerStats.GetMunhaDispatchGrade();

        //명상버튼 안누른 상태
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaDispatchStartTime).Value < 0)
        {
            DateTime currentServerTime = Utils.GetBackendServerTime();
            
            var currentServerDate = (double)Utils.ConvertToUnixTimestamp(currentServerTime);
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaDispatchStartTime).Value = (int)currentServerDate;
            ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.munhaDispatchStartTime,false);
            SetTargetTime();
            UpdateTimeText();
            Subscribe();
            return;
                
        }
        //명상하는중
        else
        {
           
            
            TimeSpan timeRemaining = targetTime - Utils.GetBackendServerTime();
            //남은시간있음
            if (timeRemaining.TotalSeconds > 0)
            {
                var lastMinute = Mathf.Max(1,(int)(timeRemaining.TotalSeconds / 60));
                
                PopupManager.Instance.ShowAlarmMessage($"제자가 파견에서 돌아오지 않았습니다!");
                return;
                
            }
            //시간다됨.
            else
            {

                var currentData = TableManager.Instance.StudentDispatch.dataArray[grade];

                List<RewardItem> rewardItems = new List<RewardItem>();
                
                for (int i = 0; i < currentData.Rewardtype.Length; i++)
                {
                    if (currentData.Rewardtype[i] < 0) break;
                    
                    rewardItems.Add(new RewardItem((Item_Type)currentData.Rewardtype[i],currentData.Rewardvalue[i]));
                }
                
         
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaDispatchStartTime).Value = -1;

                List<TransactionValue> transactions = new List<TransactionValue>();
                
                Param goodsParam = new Param();

                var e = rewardItems.GetEnumerator();

                while (e.MoveNext())
                {
                    ServerData.goodsTable.AddLocalData(ServerData.goodsTable.ItemTypeToServerString(e.Current.ItemType), e.Current.ItemValue);
                    goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.ItemType), ServerData.goodsTable.GetTableData(e.Current.ItemType).Value);
                    
                }

                Param userinfo2Param = new Param();

                userinfo2Param.Add(UserInfoTable_2.munhaDispatchStartTime, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaDispatchStartTime].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

                ServerData.SendTransactionV2(transactions, successCallBack: () =>
                {
                    
                    List<RewardData> rewardData = new List<RewardData>();
                    var e = rewardItems.GetEnumerator();
                    for (int i = 0 ;  i < rewardItems.Count;i++)
                    {
                        if (e.MoveNext())
                        {
                            rewardData.Add(new RewardData(e.Current.ItemType,e.Current.ItemValue));
                        }                    
                    }
                    if (rewardData.Count > 0)
                    {
                        _uiRewardResultView.gameObject.SetActive(true);
                        _uiRewardResultView.Initialize(rewardData);
                    }
                    
                    UpdateTimeText();
                });
            }
        }
    }
     
     private void UpdateTimeText()
     {
         SetTargetTime();
         //명상 안누름
         if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaDispatchStartTime).Value < 0)
         {
             timeText.SetText($"소요시간\n<color=white>{GameBalance.MunhaDispatchHour}시 00분</color>");
             dispatchButtonText.SetText("제자 훈련");
             dispatchButton.SetActive(true);
             return;
         }
        
         //명상누름
        
         TimeSpan timeRemaining = targetTime - ServerData.userInfoTable.currentServerTime;
        
         if (timeRemaining.TotalSeconds > 3600)
         {
             string formattedTime = string.Format("남은시간\n<color=white>{0:D2}시:{1:D2}분</color>", timeRemaining.Hours, timeRemaining.Minutes);
             timeText.text = formattedTime;
             dispatchButton.SetActive(false);
         }
         else if(timeRemaining.TotalSeconds > 0)
         {
             string formattedTime = string.Format("남은시간\n<color=white>{0:D2}분:{1:D2}초</color>", timeRemaining.Minutes, timeRemaining.Seconds);
             timeText.SetText(formattedTime);
             dispatchButton.SetActive(false);
         }
         else
         {
             timeText.SetText($"남은시간\n<color=white>00분:00초</color>");

             dispatchButtonText.SetText("보상 획득");
             dispatchButton.SetActive(true);
         }
     }
     private void SetTargetTime()
     {
         var startTime =
             Utils.ConvertFromUnixTimestamp(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaDispatchStartTime).Value);
         targetTime = startTime.AddHours(GameBalance.MunhaDispatchHour);
     }
}
