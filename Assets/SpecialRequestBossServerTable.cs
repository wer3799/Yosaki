using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using GoogleMobileAds.Api;
using UnityEngine.Serialization;

[System.Serializable]
public class SRBossServerData
{
    public int idx;
    public ReactiveProperty<int> score;
    public ReactiveProperty<int> isRewarded;

    public string ConvertToString()
    {
        return $"{idx},{score.Value},{isRewarded.Value}";
    }
}
public class SpecialRequestBossServerTable
{
    public static string Indate;
    public static string tableName = "SpecialRequestBoss";
    public static char rewardSplit = '#';

    private ReactiveDictionary<string, SRBossServerData> tableDatas = new ReactiveDictionary<string, SRBossServerData>();

    public ReactiveDictionary<string, SRBossServerData> TableDatas => tableDatas;

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

                var table = Utils.GetCurrentSeasonSpecialRequestData();

                for (int i = 0; i < table.Stringid.Length; i++)
                {
                    var bossTable = new SRBossServerData();
                    bossTable.idx = i;
                    bossTable.score = new ReactiveProperty<int>(-1);
                    bossTable.isRewarded = new ReactiveProperty<int>(0);

                    defultValues.Add(table.Stringid[i], bossTable.ConvertToString());
                    tableDatas.Add(table.Stringid[i], bossTable);
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

                var table = Utils.GetCurrentSeasonSpecialRequestData();

                for (int i = 0; i < table.Stringid.Length; i++)
                {
                    if (data.Keys.Contains(table.Stringid[i]))
                    {
                        //값로드
                        var value = data[table.Stringid[i]][ServerData.format_string].ToString();

                        var bossData = new SRBossServerData();

                        var splitData = value.Split(',');

                        bossData.idx = int.Parse(splitData[0]);
                        bossData.score = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        bossData.isRewarded = new ReactiveProperty<int>(int.Parse(splitData[2]));

                        tableDatas.Add(table.Stringid[i], bossData);
                    }
                    else
                    {

                        var bossData = new SRBossServerData();
                        bossData.idx = i;
                        bossData.score = new ReactiveProperty<int>(-1);
                        bossData.isRewarded = new ReactiveProperty<int>(0);

                        defultValues.Add(table.Stringid[i], bossData.ConvertToString());

                        tableDatas.Add(table.Stringid[i], bossData);
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

    public List<int> GetBossRewardedIdxList(string strindId)
    {
        var rewards = ServerData.bossServerTable.TableDatas[strindId].rewardedId.Value
            .Split(BossServerTable.rewardSplit)
            .Where(e => string.IsNullOrEmpty(e) == false)
            .Select(e => int.Parse(e))
            .ToList();

        return rewards;
    }

    public int GetTotalStar()
    {
        using var e = tableDatas.GetEnumerator();

        var sum = 0;
        while (e.MoveNext())
        {
            sum += e.Current.Value.score.Value + 1;
        }

        return sum;
    }
}
