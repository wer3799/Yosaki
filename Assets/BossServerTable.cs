using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class BossServerData
{
    public int idx;
    public ReactiveProperty<string> score;
    public ReactiveProperty<string> rewardedId;

    public string ConvertToString()
    {
        return $"{idx},{score.Value},{rewardedId.Value}";
    }
}
public enum TimeType
{
    //60
    Normal,
    //30
    Half,
    //15
    Quarter,
}
public enum NotImmuneType
{
    //Susano + Dokebi 
    Normal,
    //Dokebi
    Susano,
    // x 
    Dokebi
}
public class BossServerTable
{
    public static string Indate;
    public static string tableName = "TwelveBoss";
    public static char rewardSplit = '#';

    private ReactiveDictionary<string, BossServerData> tableDatas = new ReactiveDictionary<string, BossServerData>();

    public ReactiveDictionary<string, BossServerData> TableDatas => tableDatas;

    public void UpdateData(string key)
    {
        Param defultValues = new Param();

        //hasitem 1
        defultValues.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, e =>
        {

        });
    }


    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadBossFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.TwelveBossTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var bossTable = new BossServerData();
                    bossTable.idx = table[i].Id;
                    bossTable.score = new ReactiveProperty<string>(string.Empty);
                    bossTable.rewardedId = new ReactiveProperty<string>(string.Empty);

                    defultValues.Add(table[i].Stringid, bossTable.ConvertToString());
                    tableDatas.Add(table[i].Stringid, bossTable);
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

                var table = TableManager.Instance.TwelveBossTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var bossData = new BossServerData();

                        var splitData = value.Split(',');

                        bossData.idx = int.Parse(splitData[0]);
                        bossData.score = new ReactiveProperty<string>(splitData[1]);
                        bossData.rewardedId = new ReactiveProperty<string>(splitData[2]);

                        tableDatas.Add(table[i].Stringid, bossData);
                    }
                    else
                    {

                        var bossData = new BossServerData();
                        bossData.idx = table[i].Id;
                        bossData.score = new ReactiveProperty<string>(string.Empty);
                        bossData.rewardedId = new ReactiveProperty<string>(string.Empty);

                        defultValues.Add(table[i].Stringid, bossData.ConvertToString());

                        tableDatas.Add(table[i].Stringid, bossData);
                        paramCount++;
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

    public List<int> GetGuildBossRewardedIdxList()
    {
        var rewards = ServerData.bossServerTable.TableDatas["boss12"].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }
    public List<int> GetBossRewardedIdxList(string bossStrindId)
    {
        var rewards = ServerData.bossServerTable.TableDatas[bossStrindId].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }
    public List<int> GetChunmaBossRewardedIdxList()
    {
        var rewards = ServerData.bossServerTable.TableDatas["b55"].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }
    public List<int> GetGangChulBossRewardedIdxList()
    {
        var rewards = ServerData.bossServerTable.TableDatas["boss20"].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }
    public List<int> GetChunmaRewardedIdxList()
    {
        var rewards = ServerData.bossServerTable.TableDatas["b55"].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }

    public int GetGangChulBossRewardedIdx()
    {
        var rewards = ServerData.bossServerTable.TableDatas["boss20"].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();
        if (rewards.Count != 0)
        {
            return rewards.Max();
        }

        return -1;
        
    }

    public List<int> GetHellRelicRewardedIdxList()
    {
        var rewards = ServerData.bossServerTable.TableDatas["b53"].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }
    public List<int> GetGuildRaidBossRewardedIdxList()
    {
        var rewards = ServerData.bossServerTable.TableDatas["b73"].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }
}
