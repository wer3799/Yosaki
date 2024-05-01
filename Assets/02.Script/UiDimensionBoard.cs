using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

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

    private int seasonIdx = -1;
    
    [Header("Growth")]
    
    [SerializeField]
    private UiTopRankerCell playerCell;

    [SerializeField]
    private TextMeshProUGUI powerLevelText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField] private UiDimensionStatusUpgradeCell statusUpgradeCell0;
    [SerializeField] private Transform cellParent0;
    
    [Header("Dungeon")]
    private int currentRank = 0;
    private void OnEnable()
    {
        UiDimensionRankerView.Instance.DisableAllCell();
        LoadRankInfo();
        
        UpdatePlayerView();
    }


    private void Start()
    {
        Subscribe();
        
        Initialize();

        Refresh();
        
    }
    
    private void Initialize()
    {
        seasonIdx = Utils.GetCurrentDimensionSeasonIdx();
        
        SetSpecialReward();
        
        MakeCell();
    }

    private void MakeCell()
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
    private void SetSpecialReward()
    {
        var tableData = TableManager.Instance.DimensionReward.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Seasonid != seasonIdx) continue;
            var cell = Instantiate(reward, rewardParent);
            cell.Initialize(tableData[i],currentRank);
        }
    }

    private void Refresh()
    {
        powerLevelText.SetText($"무력 : {Utils.ConvertNum(PlayerStats.GetDimensionTotalPower())}");
    }
    
    private void Subscribe()
    {
        RankManager.Instance.WhenMyDimensionRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                myRankView.Initialize($"{e.Rank}", e.NickName, $"{e.Score}단계", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.gumgiIdx, e.GuildName,e.maskIdx,e.hornIdx,e.suhoAnimal,rankType:UiRankView.RankType.Dimension,false);
                currentRank = (int)e.Score;

            }
            else
            {
                myRankView.Initialize("나", "미등록", "미등록", 0, -1, -1, -1, -1, -1, string.Empty,-1,-1,-1);
                currentRank = 0;
            }
        }).AddTo(this);

        ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level)
            .AsObservable()
            .Subscribe(e =>
            {
                levelText.SetText($"LV : {Utils.ConvertNum(e)}");
            })
            .AddTo(this);
    }
    private void LoadRankInfo()
    {
        rankViewParent.gameObject.SetActive(false);
        
        //loadingMask.SetActive(false);
        //failObject.SetActive(false);
        
        LoadRank(RankType.Dimension);
        
        RankManager.Instance.RequestMyDimensionRank();
    }

    private void LoadRank(RankType type)
    {
        //리스트 없으면 Get
        if (RankManager.Instance.RankList.ContainsKey(type)==false)
        {
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
                
                rankViewContainer[i].Initialize($"{rankList[i].Rank}", $"{rankList[i].NickName}", $"{rankList[i].Score}단계", rankList[i].Rank, rankList[i].costumeIdx, rankList[i].petIddx, rankList[i].weaponIdx, rankList[i].magicbookIdx, rankList[i].gumgiIdx,rankList[i].GuildName, rankList[i].maskIdx,rankList[i].hornIdx,rankList[i].suhoAnimal,rankType:UiRankView.RankType.Dimension);
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
            ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value, -1);
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
                        rankViewContainer[i].Initialize($"{rank}", $"{nickName}", $"{score}단계", rank, costumeId, petId, weaponId, magicBookId, gumgiIdx, guildName, maskIdx,hornIdx,suhoAnimal,rankType:UiRankView.RankType.Dimension);
                        RankManager.Instance.RankList[RankType.Dimension].Add(new RankManager.RankInfo(NickName: nickName, GuildName: guildName, Rank: rank, Score: score, costumeIdx: costumeId, petIddx: petId, weaponIdx: weaponId, magicbookIdx: magicBookId, gumgiIdx: gumgiIdx, maskIdx: maskIdx, hornIdx: hornIdx, suhoAnimal: suhoAnimal));

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
    public void OnClickEnterButton()
    {

        PopupManager.Instance.ShowYesNoPopup("알림", "도전 할까요?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Dimension);
            GameManager.Instance.SetBossId(currentRank);
        }, () => { });
    } 
}
