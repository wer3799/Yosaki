using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

public static class BattleContestData
{
    public static List<RankManager.RankInfo> RankList0 =new List<RankManager.RankInfo>();
    public static List<RankManager.RankInfo> RankList1=new List<RankManager.RankInfo>();
    public static List<RankManager.RankInfo> RankList2=new List<RankManager.RankInfo>();
    public static List<RankManager.RankInfo> RankList3=new List<RankManager.RankInfo>();
    public static List<RankManager.RankInfo> RankList4=new List<RankManager.RankInfo>();

    public static void ClearData()
    {
        RankList0.Clear();
        RankList1.Clear();
        RankList2.Clear();
        RankList3.Clear();
        RankList4.Clear();
    }
    public static List<RankManager.RankInfo> MakeRankData()
    {
        var bossId = GameManager.Instance.bossId;
        var tableData = TableManager.Instance.BattleContestTable.dataArray;
        switch (bossId)
        {
            case 0 :
                if (RankList0.Count < 1)
                {
                    RankManager.Instance.GetRankerList(RankManager.Rank_Stage_Uuid, 100, WhenAllRankerLoadComplete);
                }

                return RankList0;
            case 1 :
                if (RankList1.Count < 1)
                {
                    RankManager.Instance.GetRankerList(RankManager.Rank_Stage_Uuid, 100, WhenAllRankerLoadComplete);
                }
                return RankList1;
                
            case 2 :
                if (RankList2.Count < 1)
                {
                    RankManager.Instance.GetRankerList(RankManager.Rank_Stage_Uuid, 100, tableData[bossId].Maxrank, WhenAllRankerLoadComplete);
                }
                return RankList2;
                
            case 3 :
                if (RankList3.Count < 1)
                {
                    RankManager.Instance.GetRankerList(RankManager.Rank_Stage_Uuid, 100, tableData[bossId].Maxrank, WhenAllRankerLoadComplete);
                }
                return RankList3;
                
            case 4 :
                if (RankList4.Count < 1)
                {
                    RankManager.Instance.GetRankerList(RankManager.Rank_Stage_Uuid, 100, tableData[bossId].Maxrank, WhenAllRankerLoadComplete);
                }
                return RankList4;
                
            default:
                break;
        }

        return null;
    }
    
    public static void WhenAllRankerLoadComplete(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {

                for (int i = 0; i < rows.Count; i++)
                {
                    if (i < rows.Count)
                    {
                        JsonData data = rows[i];

                        var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                        string nickName = data["nickname"][ServerData.format_string].ToString();
                        int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                        int level = int.Parse(data["score"][ServerData.format_Number].ToString());
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

#if UNITY_IOS
                    nickName = nickName.Replace(CommonString.IOS_nick, "");
#endif

                        //myRankView.Initialize($"{e.Rank}", e.NickName, $"Lv {e.Score}");
                        string guildName = string.Empty;
                        if (splitData.Length >= 8)
                        {
                            guildName = splitData[7];
                        }
                        var bossId = GameManager.Instance.bossId;

                        switch (bossId)
                        {
                            case 0 :
                            case 1 :
                                if (i < 15)
                                {
                                    RankList0.Add(new RankManager.RankInfo( NickName:nickName, GuildName:guildName, Rank:rank, Score:level,costumeIdx:costumeId,  petIddx:petId,  weaponIdx:weaponId,  magicbookIdx:magicBookId,  gumgiIdx:gumgiIdx,  maskIdx:maskIdx,  hornIdx:hornIdx, suhoAnimal:suhoAnimal));
                                }
                                else
                                {
                                    RankList1.Add(new RankManager.RankInfo( NickName:nickName, GuildName:guildName, Rank:rank, Score:level,costumeIdx:costumeId,  petIddx:petId,  weaponIdx:weaponId,  magicbookIdx:magicBookId,  gumgiIdx:gumgiIdx,  maskIdx:maskIdx,  hornIdx:hornIdx, suhoAnimal:suhoAnimal));
                                }
                                break;
                            case 2 :
                                RankList2.Add(new RankManager.RankInfo( NickName:nickName, GuildName:guildName, Rank:rank, Score:level,costumeIdx:costumeId,  petIddx:petId,  weaponIdx:weaponId,  magicbookIdx:magicBookId,  gumgiIdx:gumgiIdx,  maskIdx:maskIdx,  hornIdx:hornIdx, suhoAnimal:suhoAnimal));
                                break;
                            case 3 :
                                RankList3.Add(new RankManager.RankInfo( NickName:nickName, GuildName:guildName, Rank:rank, Score:level,costumeIdx:costumeId,  petIddx:petId,  weaponIdx:weaponId,  magicbookIdx:magicBookId,  gumgiIdx:gumgiIdx,  maskIdx:maskIdx,  hornIdx:hornIdx, suhoAnimal:suhoAnimal));
                                break;
                            case 4 :
                                RankList4.Add(new RankManager.RankInfo( NickName:nickName, GuildName:guildName, Rank:rank, Score:level,costumeIdx:costumeId,  petIddx:petId,  weaponIdx:weaponId,  magicbookIdx:magicBookId,  gumgiIdx:gumgiIdx,  maskIdx:maskIdx,  hornIdx:hornIdx, suhoAnimal:suhoAnimal));
                                break;
                            default:
                                break;
                        }
                        
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                //데이터 없을때
            }

        }
    }
}
