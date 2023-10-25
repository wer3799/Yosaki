using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PetDispatchBoard : MonoBehaviour
{
    [SerializeField]
    private PetDisPatchLeftCell prefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private PetDispatchRewardCell rewardPrefab;

    [SerializeField]
    private Transform rewardCellParent;
    
    private List<PetDispatchRewardCell> cellContainer = new List<PetDispatchRewardCell>();

    [SerializeField]
    private TextMeshProUGUI currentGradeDescription;
    [SerializeField]
    private TextMeshProUGUI petScoreDescription;

    [SerializeField] private TextMeshProUGUI adReduceTimeButtonText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject petDispatchButton;
    [SerializeField] private GameObject adReduceTimeButtonObject;
    [SerializeField] private Button adReduceTimeButton;
    
    private int currentIdx = 0;
    
    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField] private UiRewardResultView _uiRewardResultView;


    private void Start()
    {
        CreateLeftCells();


        currentIdx = Mathf.Max(0, PlayerStats.GetPetDispatchGrade());

        Subscribe2();
        Initialize(currentIdx);
    }
    private void OnEnable()
    {
        disposable.Clear();    
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value > 0)
        {
            Subscribe();
        }
        
        petScoreDescription.SetText($"환수 파견 점수 : {ServerData.petTable.GetPetDispatchScore()}점");

        UpdateTimeText();
    }
    private void OnDisable()
    {
        disposable.Clear();    
    }
    
    private void Subscribe()
    {
        disposable.Clear();
        
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => UpdateTimeText())
            .AddTo(disposable);

    }
    private void Subscribe2()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dailyPetDispatchReceiveCount).AsObservable().Subscribe(
            e =>
            {
                adReduceTimeButtonText.SetText($"남은시간\n2시간 감소({e}/2)");
                adReduceTimeButton.interactable = e < 2;
            }).AddTo(this);
    }
    private void CreateLeftCells()
    {
        var tableData = TableManager.Instance.PetDispatch.dataArray;


        for (int i = 0; i < tableData.Length; i++)
        {
            PetDisPatchLeftCell leftCell = Instantiate<PetDisPatchLeftCell>(prefab, cellParent);
            leftCell.Initialize(tableData[i]);
        }
    }
    

    public void Initialize(int currentIdx)
    {
        this.currentIdx = currentIdx;

        UpdateUi();
    }

    private bool HasRemoveAd()
    {
        return ServerData.iapServerTable.TableDatas["removead"].buyCount.Value > 0;

    }
    public void UpdateUi()
    {
        string desc = "";
        
        desc += $"{currentIdx + 1}단계";

        if (currentIdx == PlayerStats.GetPetDispatchGrade())
        {
            desc += $"\n<color=yellow>적용중</color>";
        }
        
        currentGradeDescription.SetText(desc);

        var tableData = TableManager.Instance.PetDispatch.dataArray;
        var currentData = tableData[currentIdx];


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
            PetDispatchRewardCell rewardCell = Instantiate<PetDispatchRewardCell>(rewardPrefab, rewardCellParent);
            cellContainer.Add(rewardCell);
        }

        for (int i = 0; i < cellContainer.Count; i++)
        {
            RewardItem rewardItem = new RewardItem((Item_Type)currentData.Rewardtype[i], currentData.Rewardvalue[i]);
            if (i < slotNum)
            {
                cellContainer[i].gameObject.SetActive(true);
                cellContainer[i].Initialize(rewardItem);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickRightButton()
    {
        var tableData = TableManager.Instance.PetDispatch.dataArray;

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
    
    [SerializeField] private TextMeshProUGUI petDispatchButtonText;

    private DateTime targetTime;

    private void SetTargetTime()
    {
        var startTime =
            Utils.ConvertFromUnixTimestamp(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value);
        targetTime = startTime.AddHours(GameBalance.PetDispatchHour);
    }
    
    private void UpdateTimeText()
    {
        SetTargetTime();
        //명상 안누름
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value < 0)
        {
            timeText.SetText($"소요시간\n<color=white>{GameBalance.PetDispatchHour}시 00분</color>");
            petDispatchButtonText.SetText("환수 파견");
            petDispatchButton.SetActive(true);
            return;
        }
        
        //명상누름
        
        TimeSpan timeRemaining = targetTime - DateTime.Now;
        
        // #if UNITY_EDITOR
        // Debug.LogError("앱이 실행된 시간 :" + timeRemaining.TotalSeconds + "초");
        // #endif
        
        if (timeRemaining.TotalSeconds > 3600)
        {
            string formattedTime = string.Format("남은시간\n<color=white>{0:D2}시:{1:D2}분</color>", timeRemaining.Hours, timeRemaining.Minutes);
            timeText.text = formattedTime;
            petDispatchButton.SetActive(false);
            adReduceTimeButtonObject.SetActive(true);
        }
        else if(timeRemaining.TotalSeconds > 0)
        {
            string formattedTime = string.Format("남은시간\n<color=white>{0:D2}분:{1:D2}초</color>", timeRemaining.Minutes, timeRemaining.Seconds);
            timeText.SetText(formattedTime);
            petDispatchButton.SetActive(false);
            adReduceTimeButtonObject.SetActive(true);
        }
        else
        {
            timeText.SetText($"남은시간\n<color=white>00분:00초</color>");

            petDispatchButtonText.SetText("보상 획득");
            petDispatchButton.SetActive(true);
            adReduceTimeButtonObject.SetActive(false);
        }
    }
    
    private DateTime GetBackendServerTime()
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        return DateTime.Parse(time).ToUniversalTime().AddHours(9);
    }

    public void OnClickReducePetDispatchTimeButton()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dailyPetDispatchReceiveCount).Value >= 2)
        {
            PopupManager.Instance.ShowAlarmMessage("이번 파견에서는 더 이상 시간을 감소하실 수 없습니다.");
            return;
        }
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("환수 파견을 시작해주세요!");
            return;
        }
        AdManager.Instance.ShowRewardedReward(RewardRoutine);
        
    }

    private void RewardRoutine()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dailyPetDispatchReceiveCount).Value >= 2)
        {
            PopupManager.Instance.ShowAlarmMessage("이번 파견에서는 더 이상 시간을 감소하실 수 없습니다.");
            return;
        }
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("환수 파견을 시작해주세요!");
            return;
            
        }

        adReduceTimeButton.interactable = false;
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dailyPetDispatchReceiveCount).Value++;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value-=3600*2;
        
        List<TransactionValue> transactions = new List<TransactionValue>();
                
        Param userinfo2Param = new Param();

        userinfo2Param.Add(UserInfoTable_2.dailyPetDispatchReceiveCount, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dailyPetDispatchReceiveCount].Value);
        userinfo2Param.Add(UserInfoTable_2.petDispatchStartTime, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.petDispatchStartTime].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            UpdateTimeText();
            adReduceTimeButton.interactable = ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dailyPetDispatchReceiveCount).Value<2;
        });
    }

    public void OnClickPetDispatchButton()
    {
        var grade = PlayerStats.GetPetDispatchGrade();

        if (grade < 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"환수파견 점수가 부족합니다!");
            return;
        }
        //명상버튼 안누른 상태
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value < 0)
        {
            DateTime currentServerTime = GetBackendServerTime();
            var currentServerDate = (double)Utils.ConvertToUnixTimestamp(currentServerTime);
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value = (int)currentServerDate;
            ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.petDispatchStartTime,false);
            SetTargetTime();
            UpdateTimeText();
            Subscribe();
            return;
                
        }
        //명상하는중
        else
        {
           
            
            TimeSpan timeRemaining = targetTime - GetBackendServerTime();
            //남은시간있음
            if (timeRemaining.TotalSeconds > 0)
            {
                var lastMinute = Mathf.Max(1,(int)(timeRemaining.TotalSeconds / 60));
                
                PopupManager.Instance.ShowAlarmMessage($"환수가 파견에서 돌아오지 않았습니다!");
                return;
                
            }
            //시간다됨.
            else
            {

                var currentData = TableManager.Instance.PetDispatch.dataArray[grade];

                List<RewardItem> rewardItems = new List<RewardItem>();
                
                for (int i = 0; i < currentData.Rewardtype.Length; i++)
                {
                    if (currentData.Rewardtype[i] < 0) break;
                    
                    rewardItems.Add(new RewardItem((Item_Type)currentData.Rewardtype[i],currentData.Rewardvalue[i]));
                }
                
         
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.petDispatchStartTime).Value = -1;
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dailyPetDispatchReceiveCount).Value = 0;

                List<TransactionValue> transactions = new List<TransactionValue>();
                
                Param goodsParam = new Param();

                var e = rewardItems.GetEnumerator();

                while (e.MoveNext())
                {
                    ServerData.goodsTable.AddLocalData(ServerData.goodsTable.ItemTypeToServerString(e.Current.ItemType), e.Current.ItemValue);
                    goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.ItemType), ServerData.goodsTable.GetTableData(e.Current.ItemType).Value);
                    
                }

                Param userinfo2Param = new Param();

                userinfo2Param.Add(UserInfoTable_2.petDispatchStartTime, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.petDispatchStartTime].Value);
                userinfo2Param.Add(UserInfoTable_2.dailyPetDispatchReceiveCount, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dailyPetDispatchReceiveCount].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

                ServerData.SendTransactionV2(transactions, successCallBack: () =>
                {
                    
                    List<UiRewardView.RewardData> rewardData = new List<UiRewardView.RewardData>();
                    var e = rewardItems.GetEnumerator();
                    for (int i = 0 ;  i < rewardItems.Count;i++)
                    {
                        if (e.MoveNext())
                        {
                            rewardData.Add(new UiRewardView.RewardData(e.Current.ItemType,e.Current.ItemValue));
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
}