using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using BackEnd;
using System;
using UniRx;
using LitJson;

public enum RankType
{
    Level, Stage, Boss, Real_Boss, Relic, MiniGame, GangChul, ChunmaTop, Dimension, WeeklyStage,None
}

public class RankManager : SingletonMono<RankManager>
{
    private Dictionary<RankType, List<RankInfo>> rankList = new Dictionary<RankType, List<RankInfo>>();

    public Dictionary<RankType, List<RankInfo>> RankList => rankList;

    public void ClearRankList()
    {
        rankList.Clear();
    }
    
    private Dictionary<RankType, RankInfo> myRankInfo = new Dictionary<RankType, RankInfo>()
    {
        //채팅 표기 우선순위
        { RankType.Real_Boss,null },
        { RankType.Stage,null },
        { RankType.Level,null },
        { RankType.Boss,null },
        { RankType.Relic,null },
        { RankType.MiniGame,null },
        { RankType.GangChul,null },
        { RankType.ChunmaTop,null },
        { RankType.Dimension,null },
    };


    public Dictionary<RankType, RankInfo> MyRankInfo => myRankInfo;

    public class RankInfo
    {
        public RankInfo(string NickName, string GuildName, int Rank, double Score, int costumeIdx, int petIddx, int weaponIdx, int magicbookIdx, int gumgiIdx, int maskIdx, int hornIdx,int suhoAnimal,int dimensionIdx=-1)
        {
#if UNITY_ANDROID
            this.NickName = NickName;
#endif

#if UNITY_IOS
            this.NickName = NickName.Replace(CommonString.IOS_nick, "");
#endif
            this.Rank = Rank;
            this.Score = Score;
            this.costumeIdx = costumeIdx;
            this.petIddx = petIddx;
            this.weaponIdx = weaponIdx;
            this.magicbookIdx = magicbookIdx;
            this.gumgiIdx = gumgiIdx;
            this.GuildName = GuildName;
            this.maskIdx = maskIdx;
            this.hornIdx = hornIdx;
            this.suhoAnimal = suhoAnimal;
            this.dimensionIdx = dimensionIdx;
        }

        public string NickName;
        public string GuildName;
        public int Rank;
        public double Score;
        public int costumeIdx;
        public int petIddx;
        public int weaponIdx;
        public int magicbookIdx;
        public int gumgiIdx;
        public int maskIdx;
        public int hornIdx;
        public int suhoAnimal;
        public int dimensionIdx;

    }

#if UNITY_ANDROID
    public const string Rank_Level_Uuid = "c1d70840-de7f-11eb-bc74-95875190be29";
    public const string Rank_Stage_Uuid = "68d8acb0-de81-11eb-9e66-25cb0ae9020d";
    public const string Rank_Boss_Uuid = "d3c49070-2e1c-11ed-96b7-6b63e308d9f4";
    public const string Rank_Real_Boss_Uuid = "3c3b18e0-433f-11ed-b7bf-19cd1c47b16d";
    public const string Rank_Relic_Uuid = "e24e0610-257c-11ed-9935-efa0a6ef95f0";
    public const string Rank_MiniGame_Uuid = "dd0a3b90-8285-11ed-bd0d-ebd5b059561f";
    public const string Rank_Guild_Uuid = "ff017920-698b-11ec-b243-8d1fccc57e3d";
    public const string Rank_Guild_Reset_Uuid = "be50a6d0-698b-11ec-b243-8d1fccc57e3d";
    public const string Rank_Guild_Reset_Uuid_Feed = "0fdf35b0-9115-11ec-a581-736e2455e958";

    public const string Rank_GangChul_Boss_Uuid = "9d88e4d0-b76f-11ec-8ef8-7f0c591b422a";
    public const string Rank_GangChul_Guild_Boss_Uuid = "a16b1790-559d-11ed-9936-adc34f7d653d"; //대산군
    public const string Rank_Party_Guild_Uuid = "a16b1790-559d-11ed-9936-adc34f7d653d";
    public const string Rank_ChunmaTop_Uuid = "8f5ebfc0-4341-11ed-844a-55337d4fa4d7";
    
    
    public const string Rank_ChunmaV2_Uuid = "fb16c4d0-4195-11ee-b1bd-f5f3eef03a18";
    public const string Rank_Dimension_Uuid = "1dd2fc90-060e-11ef-bac6-9bb9472c3406";
    
    public const string Rank_WeeklyStage_Uuid = "4e5e96c0-489a-11ef-921d-a15482c8f621";

    
    

    //
    public const string Rank_Level_TableName = "Rank_Level"; //레벨
    public const string Rank_Stage = "Rank_Stage"; //스테이지
    public const string Rank_Boss = "HellWar_And"; //지옥탈환전
    public const string Rank_Real_Boss = "Party_Solo_And"; //십만대산 개인
    public const string Rank_Relic = "HelRelicRank"; //지옥 영혼의숲
    public const string Rank_MiniGame = "Rank_MiniGame2"; //미니게임
    public const string Rank_GangChul = "Guild_Boss_And"; // 대산군
    public const string Rank_ChunmaTop = "Party_Top_AND"; // 십만대산 파티
    
    public const string Rank_ChunmaV2 = "PartyRaid_AND"; // 십만대산V2
    public const string Rank_Dimension = "DimensionRanking_AND"; // 차원
    
#endif

#if UNITY_IOS
    public const string Rank_Level_Uuid = "ea8c9430-38cf-11ec-955f-fb3c68e97f2a";
    public const string Rank_Stage_Uuid = "f865e900-31b6-11ec-b4ab-713be46ddb60";
    public const string Rank_Boss_Uuid = "e1c32ce0-2e1c-11ed-96b7-6b63e308d9f4";
    public const string Rank_Real_Boss_Uuid = "483029b0-433f-11ed-b7bf-19cd1c47b16d";
    public const string Rank_Relic_Uuid = "ee37f350-257c-11ed-9935-efa0a6ef95f0";
    public const string Rank_MiniGame_Uuid = "e817ab80-8285-11ed-bd0d-ebd5b059561f";
    public const string Rank_Guild_Uuid = "10619010-698c-11ec-b243-8d1fccc57e3d";
    public const string Rank_Guild_Reset_Uuid = "d248cdc0-698b-11ec-b243-8d1fccc57e3d";
    public const string Rank_Guild_Reset_Uuid_Feed = "41c34620-9115-11ec-a581-736e2455e958";

    public const string Rank_GangChul_Boss_Uuid = "c09780d0-b76f-11ec-8ac4-7dc9d81a6e2f";
    public const string Rank_GangChul_Guild_Boss_Uuid = "b7238c20-559d-11ed-9936-adc34f7d653d";
    public const string Rank_Party_Guild_Uuid = "b7238c20-559d-11ed-9936-adc34f7d653d";
    public const string Rank_ChunmaTop_Uuid = "aa2dfff0-4341-11ed-844a-55337d4fa4d7";

    public const string Rank_ChunmaV2_Uuid = "0a4296a0-4196-11ee-b1bd-f5f3eef03a18";
    public const string Rank_Dimension_Uuid = "3ab16db0-060e-11ef-bac6-9bb9472c3406";
    public const string Rank_StageReward_Uuid = "6dc95810-489a-11ef-bcf8-5b1b2e68e387";


    public const string Rank_Level_TableName = "Level_Rank_IOS";
    public const string Rank_Stage = "Rank_Stage_IOS";
    public const string Rank_Boss = "HellWar_IOS";
    public const string Rank_Real_Boss = "Party_Solo_IOS";
    public const string Rank_Relic = "HelRelicRank_IOS";
    public const string Rank_MiniGame = "Rank_MiniGame2_IOS";
    public const string Rank_GangChul = "Guild_Boss_IOS";
    public const string Rank_ChunmaTop = "Party_Top_IOS";

    public const string Rank_ChunmaV2 = "PartyRaid_IOS"; // 십만대산V2
    public const string Rank_Dimension = "DimensionRanking_IOS"; // 십만대산V2
    
        public const string Rank_WeeklyStage_Uuid = "6dc95810-489a-11ef-bcf8-5b1b2e68e387";

#endif



    public ReactiveCommand<RankInfo> WhenMyLevelRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyStageRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyWeeklyStageRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyBossRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyRealBossRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyDimensionRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyRelicRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyMiniGameRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyRealGangChulBossRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyChunMaTopRankLoadComplete = new ReactiveCommand<RankInfo>();

    public void GetRankerList(string uuid, int count, Backend.BackendCallback callback)
    {
        //Debug.LogError("랭크서버 호출");
        UiRankView.rank1Count = 0;
        Backend.URank.User.GetRankList(uuid, count, callback);
    }

    public void GetRankerList(string uuid, int count,int interval, Backend.BackendCallback callback)
    {
        UiRankView.rank1Count = 0;
        Backend.URank.User.GetRankList(uuid, count, interval,callback);
    }

    #region LevelRank
    public void RequestMyLevelRank()
    {
        Backend.URank.User.GetMyRank(RankManager.Rank_Level_Uuid, MyLevelRankLoadComplete);
    }

    private void MyLevelRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

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
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }
                
                
                if (splitData.Length >= 9)
                {
                    hornIdx = int.Parse(splitData[8]);
                }
                
            

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, level, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        if (myRankInfo != null)
        {
            this.myRankInfo[RankType.Level] = myRankInfo;

            WhenMyLevelRankLoadComplete.Execute(myRankInfo);
        }
    }

    public void UpdateUserRank_Level()
    {
        if (UpdateRank() == false) return;

        Param param = new Param();
        param.Add("Level", ServerData.statusTable.GetTableData(StatusTable.Level).Value);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int weaponEnhance = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{weaponEnhance}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Level_Uuid, Rank_Level_TableName, RankTable_Level.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                // Debug.LogError($"랭킹 등록 성공! UpdateUserRank_Level");
            }
            else
            {
                //  Debug.LogError($"랭킹 등록 실패 UpdateUserRank_Level {bro.GetStatusCode()}");
            }

        });
    }
    #endregion

    #region Stage
    private Action<RankInfo> whenLoadSuccess_Stage;
    public void RequestMyStageRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Stage = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Stage_Uuid, MyStageRankLoadComplete);
    }
    private void MyStageRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

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
                
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }

                if (splitData.Length >= 9)
                {
                    hornIdx = int.Parse(splitData[8]);
                }

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Stage?.Invoke(myRankInfo);
            WhenMyStageRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Stage] = myRankInfo;
        }
    }

    public void UpdateStage_Score(double score)
    {
        if (UpdateRank() == false) return;
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;
        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Stage_Uuid, Rank_Stage, RankTable_Stage.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateStage_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateStage_Score {bro.GetStatusCode()}");
            }

        });
    }
    //주간초기화스테이지랭킹
    public void UpdateStageRanking_Score(double score)
    {
        if (UpdateRank() == false) return;
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;
        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_WeeklyStage_Uuid, Rank_Stage, RankTable_Stage.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateStageRanking_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateStageRanking_Score {bro.GetStatusCode()}");
            }

        });
    }
    #endregion


    #region Boss
    private Action<RankInfo> whenLoadSuccess_Boss;
    public void RequestMyBossRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Boss = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Boss_Uuid, MyBossRankLoadComplete);
    }
    private void MyBossRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                score *= GameBalance.BossScoreConvertToOrigin *GameBalance.BossScoreAdjustValue;
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int gumgiIdx = int.Parse(splitData[4]);
                int maskIdx = int.Parse(splitData[6]);
                int hornIdx = -1;
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }
                if (splitData.Length >= 9)
                {
                    hornIdx = int.Parse(splitData[8]);
                }
                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        //소탕팝업뜸이래야
        whenLoadSuccess_Boss?.Invoke(myRankInfo);

        if (myRankInfo != null)
        {
            WhenMyBossRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Boss] = myRankInfo;
        }
    }

    public void UpdateBoss_Score(double score)
    {
        return;
        // if (UpdateRank() == false) return;
        //
        // if (score == 0) return;
        //
        // if (this.myRankInfo[RankType.Boss] != null && score < this.myRankInfo[RankType.Boss].Score * GameBalance.BossScoreSmallizeValue)
        // {
        //     Debug.LogError("점수가 더 낮음");
        //     return;
        // }
        //
        // // score *= GameBalance.BossScoreSmallizeValue;
        //
        // Param param = new Param();
        // param.Add("Score", score);
        //
        // int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        // int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        // int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        // int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        // int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        // int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        // int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        //
        // param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}");
        //
        // SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Boss_Uuid, Rank_Boss, RankTable_Boss.Indate, param, bro =>
        // {
        //     // 이후처리
        //     if (bro.IsSuccess())
        //     {
        //         Debug.LogError($"랭킹 등록 성공! UpdateBoss0_Score");
        //     }
        //     else
        //     {
        //         Debug.LogError($"랭킹 등록 실패 UpdateBoss0_Score {bro.GetStatusCode()}");
        //     }
        // });
    }

    #endregion

    #region RealBoss
    private Action<RankInfo> whenLoadSuccess_Real_Boss;
    public void RequestMyRealBossRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Real_Boss = whenLoadSuccess;

        //Backend.URank.User.GetMyRank(RankManager.Rank_Real_Boss_Uuid, MyRealBossRankLoadComplete);
        Backend.URank.User.GetMyRank(RankManager.Rank_ChunmaV2_Uuid, MyRealBossRankLoadComplete);
    }
    private void MyRealBossRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                score *= GameBalance.BossScoreConvertToOrigin *GameBalance.BossScoreAdjustValue;

                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int gumgiIdx = int.Parse(splitData[4]);
                int maskIdx = int.Parse(splitData[6]);
                int hornIdx = -1;
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }
                if (splitData.Length >= 9)
                {
                    hornIdx = int.Parse(splitData[8]);
                }
                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }


                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Real_Boss?.Invoke(myRankInfo);
            WhenMyRealBossRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Real_Boss] = myRankInfo;
        }
    }
    //십만대산
    public void UpdateRealBoss_Score(double score)
    {
        if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.Real_Boss] != null && score < this.myRankInfo[RankType.Real_Boss].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        score *= GameBalance.BossScoreSmallizeValue / GameBalance.BossScoreAdjustValue;
        
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_ChunmaV2_Uuid, Rank_ChunmaV2, RankTable_ChunmaV2.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateBoss0_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateBoss0_Score {bro.GetStatusCode()}");
            }
        });
    }

    #endregion
    #region WeeklyStage
    private Action<RankInfo> whenLoadSuccess_Weekly_Stage;
    public void RequestMyWeeklyStageRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Weekly_Stage = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_WeeklyStage_Uuid, MyWeeklyStageRankLoadComplete);
    }
    private void MyWeeklyStageRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

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
                
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }

                if (splitData.Length >= 9)
                {
                    hornIdx = int.Parse(splitData[8]);
                }

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Weekly_Stage?.Invoke(myRankInfo);
            WhenMyWeeklyStageRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.WeeklyStage] = myRankInfo;
        }
    }
    //
    public void UpdateWeeklyStage_Score(double score)
    {
        if (UpdateRank() == false) return;
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;
        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_WeeklyStage_Uuid, Rank_Stage, RankTable_Stage.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateStageRanking_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateStageRanking_Score {bro.GetStatusCode()}");
            }

        });
    }

    #endregion
    #region Dimension
    private Action<RankInfo> whenLoadSuccess_Dimension;
    public void RequestMyDimensionRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Dimension = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Dimension_Uuid, MyDimensionRankLoadComplete);
    }
    private void MyDimensionRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

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
                int suhoAnimal = -1;
                int dimensionIdx = -1;
                if (splitData.Length >= 11)
                {
                    dimensionIdx = int.Parse(splitData[10]);
                }
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }
                if (splitData.Length >= 9)
                {
                    hornIdx = int.Parse(splitData[8]);
                }
                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }


                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal,dimensionIdx:dimensionIdx);

            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Dimension?.Invoke(myRankInfo);
            WhenMyDimensionRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Dimension] = myRankInfo;
        }
        else
        {
            WhenMyDimensionRankLoadComplete.Execute(null);
        }
    }
    //차원
    public void UpdateDimension_Score(double score)
    {
        //if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.Dimension] != null && score < this.myRankInfo[RankType.Dimension].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;
        int dimension = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}{CommonString.ChatSplitChar}{dimension}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Dimension_Uuid, Rank_Dimension, RankTable_Dimension.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! Dimension_Score : {score}");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 Dimension_Score {bro.GetStatusCode()}");
            }
        });
    }

    #endregion
    #region GangChul
    private Action<RankInfo> whenLoadSuccess_Real_Boss_GangChul;
    public void RequestMyRealBossGangChulRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Real_Boss_GangChul = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_GangChul_Boss_Uuid, MyRealBossRankLoadComplete_GangChul);
    }
    private void MyRealBossRankLoadComplete_GangChul(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                score *= GameBalance.BossScoreConvertToOrigin;
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
                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Real_Boss_GangChul?.Invoke(myRankInfo);
            WhenMyRealGangChulBossRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.GangChul] = myRankInfo;
        }
    }

    public void UpdateRealBoss_Score_GangChul(double score)
    {
        if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.GangChul] != null && score < this.myRankInfo[RankType.GangChul].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        score *= GameBalance.BossScoreSmallizeValue;

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_GangChul_Boss_Uuid, Rank_GangChul, RankTable_Real_Boss_GangChul.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateBoss0_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateBoss0_Score {bro.GetStatusCode()}");
            }
        });
    }

    #endregion
    //천마
    private Action<RankInfo> whenLoadSuccess_ChunmaTop;
    public void RequestChunMaTopRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_ChunmaTop = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_ChunmaTop_Uuid, MyChunmaTopRankLoadComplete_GangChul);
    }
    private void MyChunmaTopRankLoadComplete_GangChul(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                score *= GameBalance.BossScoreConvertToOrigin;
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int gumgiIdx = int.Parse(splitData[4]);
                int maskIdx = int.Parse(splitData[6]);
                int hornIdx = -1;
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }
                if (splitData.Length >= 9)
                {
                    hornIdx = int.Parse(splitData[8]);
                }
                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }


                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_ChunmaTop?.Invoke(myRankInfo);
            WhenMyChunMaTopRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.ChunmaTop] = myRankInfo;
        }
    }
    //십만대산 명예의전당
    public void UpdateChunmaTop(double score)
    {
        if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.ChunmaTop] != null && score < this.myRankInfo[RankType.ChunmaTop].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        score *= GameBalance.BossScoreSmallizeValue / GameBalance.BossScoreAdjustValue;

    
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_ChunmaTop_Uuid, Rank_ChunmaTop, RankTable_ChunmaTop.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateBoss0_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateBoss0_Score {bro.GetStatusCode()}");
            }
        });
    }

    //
    #region Relic
    private Action<RankInfo> whenLoadSuccess_Relic;
    public void RequestMyRelicRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Relic = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Relic_Uuid, MyRelicLoadComplete);
    }
    private void MyRelicLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

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
                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                } 
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Relic?.Invoke(myRankInfo);
            WhenMyRelicRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Relic] = myRankInfo;
        }
    }

    public void UpdateRelic_Score(double score)
    {
        if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.Relic] != null && score < this.myRankInfo[RankType.Relic].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Relic_Uuid, Rank_Relic, RankTable_YoguiSogul.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateBoss0_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateBoss0_Score {bro.GetStatusCode()}");
            }
        });
    }

    #endregion

    #region MiniGame
    private Action<RankInfo> whenLoadSuccess_MiniGame;
    public void RequestMyMiniGameRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_MiniGame = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_MiniGame_Uuid, MyMiniGameLoadComplete);
    }

    public void ResetMiniGameScore()
    {
        SendQueue.Enqueue(Backend.URank.User.GetMyRank, RankManager.Rank_MiniGame_Uuid, MyMiniGameLoadComplete);
    }

    private void MyMiniGameLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

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
                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }
                int suhoAnimal = -1;
                if (splitData.Length >= 10)
                {
                    suhoAnimal = int.Parse(splitData[9]);
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, gumgiIdx, maskIdx, hornIdx,suhoAnimal);
            }

            if (myRankInfo != null)
            {
                whenLoadSuccess_MiniGame?.Invoke(myRankInfo);
                WhenMyMiniGameRankLoadComplete.Execute(myRankInfo);

                this.myRankInfo[RankType.MiniGame] = myRankInfo;
            }

        }
        else
        {
            if (bro.GetStatusCode().Equals("404"))
            {
                if (this.myRankInfo[RankType.MiniGame] != null)
                {
                    this.myRankInfo[RankType.MiniGame].Score = 0f;
                }
            }
        }


    }

    public void UpdateMiniGame_Score(double score)
    {
        if (this.myRankInfo[RankType.MiniGame] != null && score < this.myRankInfo[RankType.MiniGame].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int gumgiIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int wingIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int hornIdx = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        int suhoAnimal = (int)ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{gumgiIdx}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{hornIdx}{CommonString.ChatSplitChar}{suhoAnimal}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_MiniGame_Uuid, Rank_MiniGame, RankTable_MiniGame.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateBoss0_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateBoss0_Score {bro.GetStatusCode()}");
            }
        });
    }

    #endregion

    private bool UpdateRank()
    {
#if UNITY_EDITOR
        return false;
#endif

        return true;
    }
}
