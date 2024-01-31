using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

//십만대산
public class UiRealBossRankBoard : MonoBehaviour
{
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

    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private List<TextMeshProUGUI> rewardTexts;


    private void OnEnable()
    {
        UiTopRankerView.Instance.DisableAllCell();
        SetTitle();
        LoadRankInfo();
    }

    private void SetTitle()
    {
        title.SetText($"랭킹({CommonString.RankPrefix_Real_Boss})");
    }

    private void Start()
    {
        Subscribe();
        
        rewardTexts[0].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_1}개");
        rewardTexts[1].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_2}개");
        rewardTexts[2].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_3}개");
        rewardTexts[3].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_4}개");
        rewardTexts[4].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_5}개");
        rewardTexts[5].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_6_10}개");
        rewardTexts[6].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_11_20}개");
        rewardTexts[7].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_21_50}개");
        rewardTexts[8].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_51_100 }개");
        rewardTexts[9].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_101_500}개");
        rewardTexts[10].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_501_1000}개");
        rewardTexts[11].SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {GameBalance.murgePartyRaidRankRewardTicket_1_1001_5000}개");
    }

    private void Subscribe()
    {
        RankManager.Instance.WhenMyRealBossRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                myRankView.Initialize($"{e.Rank}", e.NickName, $"{Utils.ConvertBigNum(e.Score)}", e.Rank, e.costumeIdx, e.petIddx, e.weaponIdx, e.magicbookIdx, e.gumgiIdx, e.GuildName,e.maskIdx,e.hornIdx,e.suhoAnimal);
            }
            else
            {
                myRankView.Initialize("나", "미등록", "미등록", 0, -1, -1, -1, -1, -1, string.Empty,-1,-1,-1);
            }


        }).AddTo(this);
    }
    private void LoadRankInfo()
    {
        rankViewParent.gameObject.SetActive(false);
        loadingMask.SetActive(false);
        failObject.SetActive(false);
        
        LoadRank(RankType.Real_Boss);
        
        RankManager.Instance.RequestMyRealBossRank();
    }

    private void LoadRank(RankType type)
    {
        //리스트 없으면 Get
        if (RankManager.Instance.RankList.ContainsKey(type)==false)
        {
            RankManager.Instance.RankList[type] = new List<RankManager.RankInfo>();
            RankManager.Instance.GetRankerList(RankManager.Rank_ChunmaV2_Uuid, 100, WhenAllRankerLoadComplete);
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
        var rankList = RankManager.Instance.RankList[RankType.Real_Boss];
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
                
                rankViewContainer[i].Initialize($"{rankList[i].Rank}", $"{rankList[i].NickName}", $"{Utils.ConvertBigNum(rankList[i].Score)}", rankList[i].Rank, rankList[i].costumeIdx, rankList[i].petIddx, rankList[i].weaponIdx, rankList[i].magicbookIdx, rankList[i].gumgiIdx,rankList[i].GuildName, rankList[i].maskIdx,rankList[i].hornIdx,rankList[i].suhoAnimal);
            }
            else
            {
                rankViewContainer[i].gameObject.SetActive(false);
            }
        }
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
                        score *= GameBalance.BossScoreConvertToOrigin * GameBalance.BossScoreAdjustValue;
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
                        rankViewContainer[i].Initialize($"{rank}", $"{nickName}", $"{Utils.ConvertBigNum(score)}", rank, costumeId, petId, weaponId, magicBookId, gumgiIdx, guildName, maskIdx,hornIdx,suhoAnimal);
                        RankManager.Instance.RankList[RankType.Real_Boss].Add(new RankManager.RankInfo(NickName: nickName, GuildName: guildName, Rank: rank, Score: score, costumeIdx: costumeId, petIddx: petId, weaponIdx: weaponId, magicbookIdx: magicBookId, gumgiIdx: gumgiIdx, maskIdx: maskIdx, hornIdx: hornIdx, suhoAnimal: suhoAnimal));

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
            failObject.SetActive(true);
        }
    }
}
