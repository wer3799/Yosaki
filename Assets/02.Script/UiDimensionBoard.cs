using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiDimensionBoard: SingletonMono<UiDimensionBoard>
{
    [Header ("Ranking")]

    [SerializeField]
    private UiRankView uiRankViewPrefab;

    [SerializeField]
    private Transform rankViewParent;
    [SerializeField]
    private UiRankView myRankView;
    
    List<UiRankView> rankViewContainer = new List<UiRankView>();
    
    [SerializeField]
    private GameObject loadingMask;

    [SerializeField]
    private GameObject failObject;

    [SerializeField] private UiDimensionRankingSpecialReward reward;
    [SerializeField] private Transform rewardParent;

    [SerializeField] private TextMeshProUGUI dateText;
    private int seasonIdx = -1;

    [SerializeField] private UiDimensionRankingRewardCell rankReward;
    [SerializeField] private Transform rankRewardParent;
    private bool loadRank = false;
    [Header("Growth")]
    
    [SerializeField]
    private UiTopRankerCell playerCell;

    [SerializeField]
    private TextMeshProUGUI powerLevelText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField] private UiDimensionStatusUpgradeCell statusUpgradeCell0;
    [SerializeField] private Transform cellParent0;
    [SerializeField] private UiDimensionStatusUpgradeCell statusUpgradeCell1;
    [SerializeField] private Transform cellParent1;

    [SerializeField] private TextMeshProUGUI levelUpPriceText;
    [SerializeField] private TextMeshProUGUI attackStatusText;
    [SerializeField] private TextMeshProUGUI nonAttackStatusText;
    
    
    [SerializeField] private DimensionEquiepmentCollectionCell equipmentCell0;
    [SerializeField] private Transform equipmentParent0;
    [SerializeField] private TextMeshProUGUI cubeGainPerText;
    [SerializeField] private List<DimensionEquiepmentCollectionCell> equipmentCellContainer =new List<DimensionEquiepmentCollectionCell>();
    [SerializeField] private DimensionEquiepmentCollectionCell equipmentCell1;
    [Header("Dungeon")]
    [SerializeField] private TextMeshProUGUI dungeonLevelText; 
    [SerializeField] private TextMeshProUGUI getClearText; 
    [SerializeField] private TextMeshProUGUI recommendPowerText; 
    [SerializeField] private TextMeshProUGUI enterText;
    [SerializeField] private List<ItemView> clearRewards = new List<ItemView>();
    
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    
    
    private int currentRank = 0;
    private int currentId = 0;

    

    public int GetRank()
    {
        return currentRank;
    }
    private void OnEnable()
    {
        if (ServerData.userInfoTable.currentServerTime.Day == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("시즌 준비중입니다!\n2일부터 신규 시즌이 시작됩니다.");
            this.gameObject.SetActive(false);
            return;
        }

        UpdatePlayerView();
    }

    public void OnLoadRank()
    {
        UiDimensionRankerView.Instance.DisableAllCell();
        
        LoadRankInfo();
    }

    private void Start()
    {
        GetFreePass();
        
        Subscribe();
        
        Initialize();

        Refresh();
        
        SetPeriod();
    }

    private void GetFreePass()
    {
        var freePassKey = "dimensionpass0";
        if (ServerData.iapServerTable.TableDatas[freePassKey].buyCount.Value < 1)
        {
            ServerData.iapServerTable.TableDatas[freePassKey].buyCount.Value = 1;
            ServerData.iapServerTable.UpData(freePassKey);
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DE).Value += 100;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DC).Value += 100;
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value += 100;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value += 100;
        }
        
    }
#endif
    
    private void Initialize()
    {
        seasonIdx = Utils.GetCurrentDimensionSeasonIdx();
        
        SetSpecialReward();
        
        MakeStatusCell();
        
        MakeEquipmentCell();
        
        SetRankingReward();

        if (Utils.IsBuyDimensionPass())
        {
            getClearText.SetText($"소탕권은 매일 {GameBalance.DCTDailyGetAmount}(+{GameBalance.DCTDailyPassGetAmount})개씩 자동으로 획득 됩니다!");
        }
        else
        {
            getClearText.SetText($"소탕권은 매일 {GameBalance.DCTDailyGetAmount}개씩 자동으로 획득 됩니다!");
        }
    }
    
    public void OnClickStatResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "시간 무공 능력치를 초기화 합니까?", () =>
        {
            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.A_DS).Value = 0;
            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.AP_DS).Value = 0;
            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.SD_DS).Value = 0;

            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value = (int)(ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level).Value) * GameBalance.dimensionStatusGetPointByLevelUp;
            
            ServerData.dimensionStatusTable.SyncAllData();
            
        }, null);


    }

    private void MakeStatusCell()
    {
        var tableData = TableManager.Instance.DimensionStatus.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            switch (tableData[i].STATUSWHERE)
            {
                case StatusWhere.dimension:
                    var cell0 = Instantiate(statusUpgradeCell0, cellParent0);
                    cell0.Initialize(tableData[i]);
                    break;
                case StatusWhere.special:
                    
                    var cell1 = Instantiate(statusUpgradeCell1, cellParent1);
                    cell1.Initialize(tableData[i]);
                    break;
            }

        }
    }

    private void MakeEquipmentCell()
    {
        var tableData = TableManager.Instance.DimensionEquip.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate(equipmentCell0, equipmentParent0);
            cell.Initialize(tableData[i]);
            equipmentCellContainer.Add(cell);
        }
    }

    private void ReInitializeEquipmentCell()
    {
        var tableData = TableManager.Instance.DimensionEquip.dataArray;
        for (int i = 0; i < equipmentCellContainer.Count; i++)
        {
            equipmentCellContainer[i].Initialize(tableData[i]);
        }
    }
    private void SetSpecialReward()
    {
        var tableData = TableManager.Instance.DimensionReward.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Seasonid != seasonIdx) continue;
            var cell = Instantiate(reward, rewardParent);
            cell.Initialize(tableData[i]);
        }
    }
    private void SetRankingReward()
    {
        var startNum = (int)Item_Type.Dimension_Ranking_Reward_1-(int)Item_Type.Dimension_Ranking_Reward_1;
        var endNum = (int)Item_Type.Dimension_Ranking_Reward_1001_10000-(int)Item_Type.Dimension_Ranking_Reward_1;
        
        for (int i = startNum; i <= endNum; i++)
        {
            var cell = Instantiate(rankReward, rankRewardParent);
            cell.Initialize(GameBalance.Dimension_Ranking_Rewards_Range[i],(int)GameBalance.Dimension_Ranking_Rewards[i]);        
        }
    }

    public void Refresh()
    {
        powerLevelText.SetText($"무력 : {Utils.ConvertNum(PlayerStats.GetDimensionTotalPower())}");
        string attack = "";
        attack += $"{CommonString.GetStatusName(DimensionStatusType.BaseAttackDam)}:{Utils.ConvertNum(PlayerStats.GetDimensionBaseAttackDam())}";
        attack += $"\n{CommonString.GetStatusName(DimensionStatusType.AttackAddPer)}:{Utils.ConvertNum(PlayerStats.GetDimensionAttackAddPer()*100)}";
        attack += $"\n{CommonString.GetStatusName(DimensionStatusType.AddSkillDamPer)}:{Utils.ConvertNum(PlayerStats.GetDimensionAddSkillDamPer()*100)}";
        attackStatusText.SetText(attack);
        
        
        string nonAttack = "";
        nonAttack += $"{CommonString.GetStatusName(DimensionStatusType.AddHp)}:{Utils.ConvertNum(PlayerStats.GetDimensionAddHp())}";
        nonAttack += $"\n{CommonString.GetStatusName(DimensionStatusType.ReduceSkillCoolTimePer)}:{Utils.ConvertNum(PlayerStats.GetDimensionReduceSkillCoolTime()*100)}";
        nonAttack += $"\n{CommonString.GetStatusName(DimensionStatusType.CubeGainPer)}:{Utils.ConvertNum(PlayerStats.GetDimensionCubeGainPer()*100)}";
        nonAttack += $"\n{CommonString.GetStatusName(DimensionStatusType.EssenceGainPer)}:{Utils.ConvertNum(PlayerStats.GetDimensionEssenceGainPer()*100)}";
        
        cubeGainPerText.SetText($"{CommonString.GetStatusName(DimensionStatusType.CubeGainPer)}\n{PlayerStats.GetDimensionCubeGainPer()*100}% 적용중");

        nonAttackStatusText.SetText(nonAttack);
        
        var idx = Mathf.Min((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value, GetLength());

        SetDungeonUI(idx);
    }

    private bool seasonEnd=false;
    private void Subscribe()
    {
        ServerData.userInfoTable.whenServerTimeUpdated.AsObservable().Subscribe(e =>
        {
            
            if (ServerData.userInfoTable.currentServerTime.Day == 1&&seasonEnd==false)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,"시즌이 종료되었습니다.",null);
                this.gameObject.SetActive(false);
                seasonEnd = true;
                return;
            }
        }).AddTo(this);

        RankManager.Instance.WhenMyDimensionRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                myRankView.Initialize($"{e.Rank}", e.NickName, $"{e.Score}단계", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.gumgiIdx, e.GuildName,e.maskIdx,e.hornIdx,e.suhoAnimal,e.dimensionIdx,rankType:UiRankView.RankType.Dimension,false);
                
                if ((int)e.Score != (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value)
                {
                    RankManager.Instance.UpdateDimension_Score((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value);
                }
            }
            else
            {
                myRankView.Initialize("나", "미등록", "미등록", 0, -1, -1, -1, -1, -1, string.Empty,-1,-1,-1,-1);
                if (0 != (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value)
                {
                    RankManager.Instance.UpdateDimension_Score((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value);
                }
            }
            currentRank = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value;

            loadingMask.SetActive(false);
        }).AddTo(this);

        ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level)
            .AsObservable()
            .Subscribe(SetLevelText)
            .AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment]
            .AsObservable()
            .Subscribe(e =>
            {
                e = Mathf.Max(0, e);
                equipmentCell1.Initialize(TableManager.Instance.DimensionEquip.dataArray[e]);
                Refresh();
            }).AddTo(this);
        ServerData.iapServerTable.TableDatas[Utils.GetCurrentDimensionSeasonData().Productid].buyCount
            .AsObservable()
            .Subscribe(e =>
            {
                Refresh();
                ReInitializeEquipmentCell();
            }).AddTo(this);
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dimensionGrade]
            .AsObservable()
            .Subscribe(e =>
            {
                if ((int)e > GetLength())
                {
                    currentId =GetLength();
                    enterText.SetText($"최고 단계");

                }
                else
                {
                    currentId = (int)e;

                    enterText.SetText($"{e+1}단계 입장");
                }
                

                var idx = Mathf.Min((int)e, GetLength());

                SetDungeonUI(idx);
            })
            .AddTo(this);
        
        
    }
    private void SetPeriod()
    {
        var endData = Utils.GetCurrentDimensionSeasonData().Enddate.Split('-');
        DateTime endDate = new DateTime(int.Parse(endData[0]), int.Parse(endData[1]), int.Parse(endData[2]));
        endDate = endDate.AddDays(1);//5월5일을 넣으면 5월6일00시에끝나야함.
        var date = endDate - ServerData.userInfoTable.currentServerTime;
            
        dateText.SetText($"{date.Days}일 {date.Hours}시간");
        
    }
    private void SetLevelText(float idx)
    {
        levelText.SetText($"LV : {Utils.ConvertNum(idx)}");
        if (idx >= TableManager.Instance.DimensionLevel.dataArray.Length)
        {
            levelUpPriceText.SetText($"최대레벨");
        }
        else
        {
            var tableData= TableManager.Instance.DimensionLevel.dataArray[(int)(idx)];
            levelUpPriceText.SetText($"{tableData.Conditionvalue}");    
        }
        
    }

    public void OnClickLevelUpButton()
    {
        var tableData = TableManager.Instance.DimensionLevel.dataArray;

        var requireLv = (int)ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level).Value ;

        if (requireLv > tableData.Length-1)
        {
            PopupManager.Instance.ShowAlarmMessage($"최대레벨입니다.");
            return;
        }
        if (ServerData.goodsTable.GetTableData((Item_Type)tableData[requireLv].Conditiontype).Value < tableData[requireLv].Conditionvalue)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName((Item_Type)tableData[requireLv].Conditiontype),JongsungType.Type_IGA)} 부족합니다.");
            return;
        }
        
   
        ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level).Value++;
        ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value += GameBalance.dimensionStatusGetPointByLevelUp;
        ServerData.goodsTable.GetTableData((Item_Type)tableData[requireLv].Conditiontype).Value -= tableData[requireLv].Conditionvalue;

        
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

        Debug.LogError($"@@@@@@@@@@@@@@@Dimension SyncComplete@@@@@@@@@@@@@@");

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param dimensionParam = new Param();
        Param goodsParam = new Param();
        
        dimensionParam.Add(DimensionStatusTable.Level, ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level).Value);
        dimensionParam.Add(DimensionStatusTable.DSP, ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value);
        
        transactionList.Add(TransactionValue.SetUpdate(DimensionStatusTable.tableName, DimensionStatusTable.Indate, dimensionParam));

            
        goodsParam.Add(GoodsTable.DE, ServerData.goodsTable.GetTableData(GoodsTable.DE).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
        });
    }
    
    private void LoadRankInfo()
    {
        rankViewParent.gameObject.SetActive(false);
        
        LoadRank(RankType.Dimension);

        if (loadRank == false)
        {
            RankManager.Instance.RequestMyDimensionRank();
            loadRank = true;
        }
    }

    private void LoadRank(RankType type)
    {
        //리스트 없으면 Get
        if (RankManager.Instance.RankList.ContainsKey(type)==false)
        {
            loadingMask.SetActive(true);
            RankManager.Instance.RankList[type] = new List<RankManager.RankInfo>();
            RankManager.Instance.GetRankerList(RankManager.Rank_Dimension_Uuid, 100, WhenAllRankerLoadComplete);
        }
        else
        {
            LoadRankList();
        }
    }
    private void LoadRankList()
    {
        UiRankView.rank1Count = 0;

        rankViewParent.gameObject.SetActive(true);
        var rankList = RankManager.Instance.RankList[RankType.Dimension];
        int interval = rankList.Count - rankViewContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var view = Instantiate<UiRankView>(uiRankViewPrefab, rankViewParent);
            rankViewContainer.Add(view);
        }
        for (int i = 0; i < rankViewContainer.Count; i++)
        {
            if (i < rankList.Count)
            {
                rankViewContainer[i].gameObject.SetActive(true);
                
                rankViewContainer[i].Initialize($"{rankList[i].Rank}", $"{rankList[i].NickName}", $"{rankList[i].Score}단계", rankList[i].Rank, rankList[i].costumeIdx, rankList[i].petIddx, rankList[i].weaponIdx, rankList[i].magicbookIdx, rankList[i].gumgiIdx,rankList[i].GuildName, rankList[i].maskIdx,rankList[i].hornIdx,rankList[i].suhoAnimal,rankList[i].dimensionIdx,rankType:UiRankView.RankType.Dimension);
            }
            else
            {
                rankViewContainer[i].gameObject.SetActive(false);
            }
        }
    }
    private void UpdatePlayerView()
    {
        int costumeId = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petId = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponId = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookId = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        playerCell.Initialize(string.Empty, string.Empty, costumeId, petId, weaponId, magicBookId,
            ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value, string.Empty,
            ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value,
            ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value,
            ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value, 
            -1,
            ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value);
    }
    
    private void WhenAllRankerLoadComplete(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            var te = JsonUtility.ToJson(bro.GetReturnValue());
            

            if (rows.Count > 0)
            {
                rankViewParent.gameObject.SetActive(true);

                int interval = rows.Count - rankViewContainer.Count;

                for (int i = 0; i < interval; i++)
                {
                    var view = Instantiate<UiRankView>(uiRankViewPrefab, rankViewParent);
                    rankViewContainer.Add(view);
                }

                for (int i = 0; i < rankViewContainer.Count; i++)
                {
                    if (i < rows.Count)
                    {
                        JsonData data = rows[i];

                        var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                        rankViewContainer[i].gameObject.SetActive(true);
                        string nickName = data["nickname"][ServerData.format_string].ToString();
                        int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                        double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                        int costumeId = int.Parse(splitData[0]);
                        int petId = int.Parse(splitData[1]);
                        int weaponId = int.Parse(splitData[2]);
                        int magicBookId = int.Parse(splitData[3]);
                        int gumgiIdx = int.Parse(splitData[4]);
                        int maskIdx = int.Parse(splitData[6]);
                        int hornIdx = -1;
                        int dimensionIdx = -1;

                        if (splitData.Length >= 11)
                        {
                            dimensionIdx = int.Parse(splitData[10]);
                        }
                        if (splitData.Length >= 9)
                        {
                            hornIdx = int.Parse(splitData[8]);
                        }
                        
                        int suhoAnimal = -1;

                        if (splitData.Length >= 10)
                        {
                            suhoAnimal = int.Parse(splitData[9]);
                        }

                        Color color1 = Color.white;
                        Color color2 = Color.white;

                        //1등
                        if (i == 0)
                        {
                            color1 = Color.yellow;
                        }
                        //2등
                        else if (i == 1)
                        {
                            color1 = Color.yellow;
                        }
                        //3등
                        else if (i == 2)
                        {
                            color1 = Color.yellow;
                        }

#if UNITY_IOS
                    nickName = nickName.Replace(CommonString.IOS_nick, "");
#endif

                        string guildName = string.Empty;
                        if (splitData.Length >= 8)
                        {
                            guildName = splitData[7];
                        }
                        //myRankView.Initialize($"{e.Rank}", e.NickName, $"Lv {e.Score}");
                        rankViewContainer[i].Initialize($"{rank}", $"{nickName}", $"{score}단계", rank, costumeId, petId, weaponId, magicBookId, gumgiIdx, guildName, maskIdx,hornIdx,suhoAnimal,dimensionIdx:dimensionIdx,rankType:UiRankView.RankType.Dimension);
                        RankManager.Instance.RankList[RankType.Dimension].Add(new RankManager.RankInfo(NickName: nickName, GuildName: guildName, Rank: rank, Score: score, costumeIdx: costumeId, petIddx: petId, weaponIdx: weaponId, magicbookIdx: magicBookId, gumgiIdx: gumgiIdx, maskIdx: maskIdx, hornIdx: hornIdx, suhoAnimal: suhoAnimal,dimensionIdx:dimensionIdx ));

                    }
                    else
                    {
                        rankViewContainer[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                //데이터 없을때
            }

        }
        else
        {
            //failObject.SetActive(true);
        }
    }

    private void SetDungeonUI(int idx)
    {
        var tableData = TableManager.Instance.DimensionDungeon.dataArray[idx];
        dungeonLevelText.SetText($"{idx+1}단계");

        var power= PlayerStats.GetDimensionTotalPower();
        if (power >= tableData.Requireforce)
        {
            recommendPowerText.color = Color.green;
        }
        else
        {
            recommendPowerText.color = Color.red;
        }
        recommendPowerText.SetText($"<color=white>무력 : {Utils.ConvertNum(power)}</color>\n권장 무력 : {Utils.ConvertNum(tableData.Requireforce)}");
        
        using var e = clearRewards.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(false);
        }
        for (int i = 0; i < tableData.Rewardtype.Length; i++)
        {
            clearRewards[i].gameObject.SetActive(true);
            clearRewards[i].Initialize((Item_Type)tableData.Rewardtype[i],tableData.Rewardvalue[i]);
        }
    }
    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

        SetDungeonUI(currentId);

        UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        currentId++;

        currentId = Mathf.Min(currentId, GetLength());

        SetDungeonUI(currentId);

        UpdateButtonState();
    }
    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != GetLength();
    }   
    private int GetLength()
    {
        return TableManager.Instance.DimensionDungeon.dataArray.Length - 1;
    }
    public void OnClickEnterButton()
    {
        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value;

        if (grade > GetLength())
        {
            PopupManager.Instance.ShowAlarmMessage("최고 단계입니다!");
            return;
        }
        
        PopupManager.Instance.ShowYesNoPopup("알림", "도전 할까요?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Dimension);
            GameManager.Instance.SetBossId(grade);
        }, () => { });
    } 
}
