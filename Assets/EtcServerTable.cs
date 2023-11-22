using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;
using System.Runtime.CompilerServices;

public class EtcServerTable
{
    public static string Indate;
    public const string tableName = "Etc";
    public const string email = "email";
    public const string yoguiSogulReward = "ys";
    public const string oldDokebi2Reward = "od";
    public const string sonReward = "sonRewardReal";
    public const string iosCoupon = "iosCoupon";
    public const string guildAttenReward = "gar";
    public const string hellReward = "hr";
    public const string DokebiHornReward = "dokebiHornReward";
    public const string PetHomeReward = "PetHomeReward";
    public const string PetHomeReward1 = "PetHomeReward1";
    public const string chunmaTopScore = "chunmaTopScore";
    public const string CostumeCollectionFreeReward = "CCFR";
    public const string CostumeCollectionAdReward = "CCAR";
    public const string GuideMissionReward = "GMR";
    public const string GuideMissionClear = "GMC";
    public const string AdReward = "AdReward";
    public const string battleWinScore = "bws";


    private Dictionary<string, ReactiveProperty<string>> tableSchema = new Dictionary<string, ReactiveProperty<string>>()
    {
        {email,new ReactiveProperty<string>(GoogleManager.email)},
        {yoguiSogulReward,new ReactiveProperty<string>(string.Empty)},
        {oldDokebi2Reward,new ReactiveProperty<string>(string.Empty)},
        {sonReward,new ReactiveProperty<string>(string.Empty)},
        {iosCoupon,new ReactiveProperty<string>(string.Empty)},
        {guildAttenReward,new ReactiveProperty<string>(string.Empty)},
        {hellReward,new ReactiveProperty<string>(string.Empty)},
        {DokebiHornReward,new ReactiveProperty<string>(string.Empty)},
        {PetHomeReward,new ReactiveProperty<string>(string.Empty)},
        {PetHomeReward1,new ReactiveProperty<string>(string.Empty)},
        {chunmaTopScore,new ReactiveProperty<string>(string.Empty)},
        {CostumeCollectionFreeReward,new ReactiveProperty<string>(string.Empty)},
        {CostumeCollectionAdReward,new ReactiveProperty<string>(string.Empty)},
        {GuideMissionReward,new ReactiveProperty<string>(string.Empty)},
        {GuideMissionClear,new ReactiveProperty<string>(string.Empty)},
        {AdReward,new ReactiveProperty<string>(string.Empty)},
        {battleWinScore,new ReactiveProperty<string>("#0#0#0#0#0")},
    };

    private Dictionary<string, ReactiveProperty<string>> tableDatas = new Dictionary<string, ReactiveProperty<string>>();
    public Dictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;
    public void UpdateData(string key)
    {
        Param defultValues = new Param();

        //hasitem 1
        defultValues.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, e =>
        {

        });
    }
    public bool YoguiSoguilRewarded(int stageId)
    {
        var rewards = tableDatas[yoguiSogulReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }
    public bool AdRewardRewarded(int stageId)
    {
        var rewards = tableDatas[AdReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }
    
    public bool OldDokebi2Rewarded(int stageId)
    {
        var rewards = tableDatas[oldDokebi2Reward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }
    public bool GuideMissionRewarded(int stageId)
    {
        var rewards = tableDatas[GuideMissionReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }
    public bool GuideMissionCleared(int stageId)
    {
        var rewards = tableDatas[GuideMissionClear].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public List<int> GetYoguiSoguilRewardedList()
    {
        var rewards = tableDatas[yoguiSogulReward].Value
       .Split(BossServerTable.rewardSplit)
       .Where(e => string.IsNullOrEmpty(e) == false)
       .Select(e => int.Parse(e))
       .ToList();

        return rewards;
    }
    public List<int> GetOldDokebi2RewardedList()
    {
        var rewards = tableDatas[oldDokebi2Reward].Value
       .Split(BossServerTable.rewardSplit)
       .Where(e => string.IsNullOrEmpty(e) == false)
       .Select(e => int.Parse(e))
       .ToList();

        return rewards;
    }

    public bool SonRewarded(float stageId)
    {
        var rewards = tableDatas[sonReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public List<int> GetSonRewardedIdxList()
    {
        var rewards = tableDatas[sonReward].Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }
    public List<int> GetHellRewardedIdxList()
    {
        var rewards = tableDatas[hellReward].Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }


    public bool HellRewarded(float stageId)
    {
        var rewards = tableDatas[hellReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public bool HasPetHomeReward(float id)
    {
        var rewards = tableDatas[PetHomeReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(id.ToString());
    }
    public bool HasPetHomeReward1(float id)
    {
        var rewards = tableDatas[PetHomeReward1].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(id.ToString());
    }
    public bool HasCostumeColectionFreeReward(float id)
    {
        var rewards = tableDatas[CostumeCollectionFreeReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(id.ToString());
    }
    public bool HasCostumeColectionAdReward(float id)
    {
        var rewards = tableDatas[CostumeCollectionAdReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(id.ToString());
    }

    public bool DokebiHornRewarded(float stageId)
    {
        var rewards = tableDatas[DokebiHornReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public bool GuildAttenRewarded(float stageId)
    {
        var rewards = tableDatas[guildAttenReward].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(stageId.ToString());
    }

    public bool IosCouponRewarded(float id)
    {
        var rewards = tableDatas[iosCoupon].Value.Split(BossServerTable.rewardSplit).ToList();

        return rewards.Contains(id.ToString());
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadStatusFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {
                    if (e.Current.Key.Equals(email))
                    {
                        defultValues.Add(e.Current.Key, GoogleManager.email);
                        tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(GoogleManager.email));

                    }
                    else
                    {
                        defultValues.Add(e.Current.Key, e.Current.Value.Value);
                        tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(string.Empty));
                    }


                }

                var bro = Backend.GameData.Insert(tableName, defultValues);

                if (bro.IsSuccess() == false)
                {
                    // 이후 처리
                    ServerData.ShowCommonErrorPopup(bro, Initialize);
                    return;
                }
                else
                {

                    var jsonData = bro.GetReturnValuetoJSON();
                    if (jsonData.Keys.Count > 0)
                    {

                        Indate = jsonData[0].ToString();

                    }

                    // data.
                    // statusIndate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
                }

                return;
            }
            //나중에 칼럼 추가됐을때 업데이트
            else
            {
                Param defultValues = new Param();
                int paramCount = 0;

                JsonData data = rows[0];

                if (data.Keys.Contains(ServerData.inDate_str))
                {
                    Indate = data[ServerData.inDate_str][ServerData.format_string].ToString();
                }

                var e = tableSchema.GetEnumerator();

                for (int i = 0; i < data.Keys.Count; i++)
                {
                    while (e.MoveNext())
                    {
                        if (data.Keys.Contains(e.Current.Key))
                        {
                            //값로드
                            var value = data[e.Current.Key][ServerData.format_string].ToString();
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(value));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value.Value);
                            tableDatas.Add(e.Current.Key, e.Current.Value);
                            paramCount++;
                        }
                    }
                }

                if (paramCount != 0)
                {
                    var bro = Backend.GameData.Update(tableName, Indate, defultValues);

                    if (bro.IsSuccess() == false)
                    {
                        ServerData.ShowCommonErrorPopup(bro, Initialize);
                        return;
                    }
                }

            }
        });
    }

    public void UpdateGuideMissionClear(GuideMissionKey key)
    {
        ServerData.etcServerTable.TableDatas[GuideMissionClear].Value += $"{BossServerTable.rewardSplit}{(int)key}";
    }
    public void UpdateGuideMissionReward(GuideMissionKey key)
    {
        ServerData.etcServerTable.TableDatas[GuideMissionReward].Value += $"{BossServerTable.rewardSplit}{(int)key}";
    }

    public int GetBattleContestScore(int difficulty)
    {
        var splitData = ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value.Split(BossServerTable.rewardSplit);
            
        var scoreList = splitData.Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();

        return scoreList[difficulty];
    }
    public int GetBattleContestTotalScore()
    {
        var splitData = ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value.Split(BossServerTable.rewardSplit);
            
        var scoreList = splitData.Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();

        return scoreList.Sum();
    }
}
