using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using GoogleMobileAds.Api;
using UnityEngine.Serialization;
using Random = System.Random;

public class UiYoSaKiMarbleBoard : MonoBehaviour
{
   [SerializeField]
    private UiYoSaKiMarbleMissionCell missionCell;
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private Transform cellParent2;

    [FormerlySerializedAs("finishConut")] [SerializeField] private TextMeshProUGUI finishCount;
    
    private Dictionary<int, UiYoSaKiMarbleMissionCell> cellContainer = new Dictionary<int, UiYoSaKiMarbleMissionCell>();
    [SerializeField] private List<UiMarblePassCell> _marblePassCells = new List<UiMarblePassCell>();

    [SerializeField] private Image diceImage;

    [SerializeField] private List<Sprite> diceImages;
    
    [SerializeField] private ParticleSystem diceParticle;
    
    [SerializeField] private UiRewardResultView _uiRewardResultView;

    [SerializeField] private TextMeshProUGUI resultText;
    
    private void OnEnable()
    {
        MissionCheck();
    }


    private void MissionCheck()
    {
                
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION5].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }    
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION6].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION7].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        } 
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION1].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION1].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION2].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dailySleepRewardReceiveCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION3].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getGumGi).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION4].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getFlower).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION5].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION6].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION7].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.NMARBLEMISSION8].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
    }

    private void Awake()
    {
        Initialize();
        
        MakeProb();
    }

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yosakiMarbleScore).AsObservable().Subscribe(e =>
        {
            if (e < 0)
            {
                for (int i = 0; i < _marblePassCells.Count; i++)
                {
                    _marblePassCells[i].SetEffect(false);
                }
            }
            else
            {
                SetEffect((int)e%18);
            }
            
            finishCount.SetText($"{(int)(e/18)}");

            if ((int)(e / 18) >= 0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION1].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION2].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION3].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION4].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION5].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION6].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION7].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION8].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION9].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION10].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >= 0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION11].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            if ((int)(e / 18) >=  0)
            {
                string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMARBLEMISSION12].Stringid;
                ServerData.eventMissionTable.UpdateMissionClearToCount(key,  (int)(e / 18));
            }
            
        }).AddTo(this);
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.EventMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.NORMALMARBLE) continue;
            var cell = Instantiate<UiYoSaKiMarbleMissionCell>(missionCell, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.FINISHMARBLE) continue;
            var cell = Instantiate<UiYoSaKiMarbleMissionCell>(missionCell, cellParent2);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
        
    }

    private List<Item_Type> _syncItemTypes = new List<Item_Type>();
    private List<float> probList = new List<float>();

    private void MakeProb()
    {
        probList.Clear();
        probList.Add(GameBalance.YutNori_0);
        probList.Add(GameBalance.YutNori_1);
        probList.Add(GameBalance.YutNori_2);
        probList.Add(GameBalance.YutNori_3);
        probList.Add(GameBalance.YutNori_4);
    }
    public void OnClickRollDice()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_Item_0)}이(가) 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value--;

        int diceCount = Utils.GetRandomIdx(probList) + 1;//0부터라 

        diceImage.sprite = diceImages[diceCount-1];
        diceParticle.gameObject.SetActive((true));
        diceParticle.Play();
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yosakiMarbleScore).Value += diceCount;

        var rewardIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yosakiMarbleScore).Value % 18;

        var rewardItem = GetRewardByTable2(rewardIdx,diceCount);
        var str = "";
        bool isOneMore = false;
        switch (diceCount)
        {
            case 1:
                str += "도";
                break;
            case 2:
                str += "개";
                break;
            case 3:
                str += "걸";
                break;
            case 4:
                str += "윷";
                ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value++;
                isOneMore = true;
                break;
            case 5:
                str += "모";
                ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value++;
                isOneMore = true;
                break;
        }

        var addText = isOneMore?$"({CommonString.GetItemName(Item_Type.Event_Item_0)} 1개 획득)":"";
        PopupManager.Instance.ShowAlarmMessage($"{str} 굴리기 완료!\n{CommonString.GetItemName((Item_Type)rewardItem.ItemType)} {Utils.ConvertNum(rewardItem.ItemValue)}개 획득!{addText}");

        if (_syncItemTypes.Contains(rewardItem.ItemType)==false)
        {
            _syncItemTypes.Add(rewardItem.ItemType);
        }
        

        if (syncRoutine != null)
        {
            StopCoroutine(syncRoutine);
        }

        syncRoutine = StartCoroutine(SyncRoutine(_syncItemTypes));
        
        

    }
    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);
    
    private RewardItem OnClickRollDice_All()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value--;

        int diceCount = Utils.GetRandomIdx(probList) + 1;//0부터라 

        Utils.AddOrUpdateValue(ref GetYutList,diceCount-1,1);
        
        
        if (diceCount >= 4)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value++;
        }

        diceImage.sprite = diceImages[diceCount - 1];
        diceParticle.gameObject.SetActive((true));
        diceParticle.Play();
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yosakiMarbleScore).Value += diceCount;

        var rewardIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yosakiMarbleScore).Value % 18;
     
        
        return GetRewardByTable2(rewardIdx,diceCount);
    }
    
    public IEnumerator SyncRoutine(List<Item_Type> types)
    {
        yield return syncDelay;



        List<TransactionValue> transactions = new List<TransactionValue>();
                
        Param goodsParam = new Param();

        using var e = types.GetEnumerator();
        while (e.MoveNext())
        {
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current), ServerData.goodsTable.GetTableData(e.Current).Value);
        }
            
        goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);


        Param userinfo2Param = new Param();

        userinfo2Param.Add(UserInfoTable_2.yosakiMarbleScore, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.yosakiMarbleScore].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            types.Clear();
        });
        
    }

    private Coroutine diceCoroutine;
    private List<RewardItem> _rewardItems = new List<RewardItem>();
    private Dictionary<int, int> GetYutList = new Dictionary<int, int>();

    private void OnClickAllDice()
    {
        _rewardItems.Clear();
        GetYutList.Clear();
        
        for (int i = 0; i < 5; i++)
        {
            Utils.AddOrUpdateValue(ref GetYutList,i,0);
        }
        while (ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value>0)
        {
            var rewardItem = OnClickRollDice_All();
            Utils.AddOrUpdateReward(ref _rewardItems,rewardItem.ItemType,rewardItem.ItemValue);
        }

        List<TransactionValue> transactions = new List<TransactionValue>();
                
        Param goodsParam = new Param();

        goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);

        using var e = _rewardItems.GetEnumerator();
        while (e.MoveNext())
        {
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.ItemType), ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(e.Current.ItemType)).Value);
        }

        Param userinfo2Param = new Param();

        userinfo2Param.Add(UserInfoTable_2.yosakiMarbleScore, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.yosakiMarbleScore].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            List<RewardData> rewardData = new List<RewardData>();
            using var e = _rewardItems.GetEnumerator();
            for (int i = 0 ;  i < _rewardItems.Count;i++)
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
                resultText.SetText($"도:{GetYutList[0]} 개:{GetYutList[1]} 걸:{GetYutList[2]} 윷:{GetYutList[3]} 모:{GetYutList[4]} ");

            }
            
            diceParticle.gameObject.SetActive((true));
            diceParticle.Play();
            PopupManager.Instance.ShowAlarmMessage("보상 획득!");
        });

        if (diceCoroutine != null)
        {
            StopCoroutine(diceCoroutine);
        }
    }
    
    public void OnClickRollAllDice()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_Item_0)}이(가) 부족합니다.");
            return;
        }
        //
        // if (diceCoroutine != null)
        // {
        //     StopCoroutine(diceCoroutine);
        // }
        // diceCoroutine = StartCoroutine(AllDiceRoutine());
        OnClickAllDice();
    }

    private void SetEffect(int idx)
    {
        for (int i = 0; i < _marblePassCells.Count; i++)
        {
            _marblePassCells[i].SetEffect(idx == i);
        }
    }
    
    private void GetRewardByTable(int idx)
    {
        var tableData = TableManager.Instance.MarbleEvent.dataArray[idx];
        
        ServerData.goodsTable.AddLocalData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype),tableData.Itemvalue);
        
        ServerData.goodsTable.UpDataV2(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype),false);
    }
    private RewardItem GetRewardByTable2(int idx,int diceNum=1)
    {
        var tableData = TableManager.Instance.MarbleEvent.dataArray[idx];

        ServerData.goodsTable.AddLocalData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), tableData.Itemvalue);

        return new RewardItem((Item_Type)tableData.Itemtype,tableData.Itemvalue);
    }

    public void OnClickRewardAllMission()
    {
        var tableData = TableManager.Instance.EventMission.dataArray;
        int rewardedNum = 0;
        List<int> rewardTypeList = new List<int>();
        
        List<string> stringIdList = new List<string>();
        for (int i = 0; i < tableData.Length; i++)
        {
            
            //일일미션 아니라면
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.NORMALMARBLE) continue;
            //Enable을 껐다면
            if (tableData[i].Enable == false) continue;
            //보상을 받았다면
            if (ServerData.eventMissionTable.CheckMissionRewardCount(tableData[i].Stringid) > 0) continue;
            //깨지 않았다면
            if (ServerData.eventMissionTable.CheckMissionClearCount(tableData[i].Stringid) < tableData[i].Rewardrequire) continue;

            //보상
            ServerData.eventMissionTable.TableDatas[tableData[i].Stringid].rewardCount.Value++;
            //패스사면 두배
         
            ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue);
            

            if (rewardTypeList.Contains(tableData[i].Rewardtype) == false)
            {
                rewardTypeList.Add(tableData[i].Rewardtype);
            }
            if (stringIdList.Contains(tableData[i].Stringid) == false)
            {
                stringIdList.Add(tableData[i].Stringid);
            }
            rewardedNum++;
        }

        if (rewardedNum > 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();
            
            using var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        
            using var stringName = stringIdList.GetEnumerator();
        
            Param eventMissionParam = new Param();
            while(stringName.MoveNext())
            {
                string updateValue = ServerData.eventMissionTable.TableDatas[stringName.Current].ConvertToString();
                eventMissionParam.Add(stringName.Current, updateValue);
            }
            transactions.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));
    
            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                //LogManager.Instance.SendLogType("ChildPass", "A", "A");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    
    }
}
