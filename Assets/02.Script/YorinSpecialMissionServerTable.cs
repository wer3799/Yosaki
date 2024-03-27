using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

[System.Serializable]
public class YorinSpecialMissionServerTableData

{
    public int idx;
    public ReactiveProperty<int> clearCount;
    public ReactiveProperty<int> rewardCount;

    public string ConvertToString()
    {
        return $"{idx},{clearCount.Value},{rewardCount.Value}";
    }
}

public class YorinSpecialMissionServerTable
{
    public static string Indate;
    public const string tableName = "YorinSpecialMission";

    private ReactiveDictionary<string, YorinSpecialMissionServerTableData> tableDatas = new ReactiveDictionary<string, YorinSpecialMissionServerTableData>();
    public ReactiveDictionary<string, YorinSpecialMissionServerTableData> TableDatas => tableDatas;

    public int CheckMissionClearCount(string key)
    {
        return tableDatas[key].clearCount.Value;
    } 
    public int CheckMissionRewardCount(string key)
    {
        return tableDatas[key].rewardCount.Value;
    } 
    public void UpdateMissionClearCount(string key, int amount)
    {
        tableDatas[key].clearCount.Value += amount;
    } 
    public void UpdateMissionClearToCount(string key, int amount)
    {
        tableDatas[key].clearCount.Value = amount;
    } 
    public void UpdateMissionRewardCount(string key, int amount)
    {
        tableDatas[key].rewardCount.Value += amount;
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

                //이벤트미션
                YorinSpecialMissionData[] table = TableManager.Instance.YorinSpecialMission.dataArray;
                for (int i = 0; i < table.Length; i++)
                {
                    var missionData = new YorinSpecialMissionServerTableData();
                    missionData.idx = table[i].Id;
                    missionData.clearCount = new ReactiveProperty<int>(0);
                    missionData.rewardCount = new ReactiveProperty<int>(0);
                   

                    tableDatas.Add(table[i].Stringid, missionData);
                    defultValues.Add(table[i].Stringid, missionData.ConvertToString());
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
                //이벤트미션
                var table = TableManager.Instance.YorinSpecialMission.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();


                        var splitData = value.Split(',');

                        var missionData = new YorinSpecialMissionServerTableData();

                        missionData.idx = int.Parse(splitData[0]);
                        missionData.clearCount = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        missionData.rewardCount = new ReactiveProperty<int>(int.Parse(splitData[2]));

                        tableDatas.Add(table[i].Stringid, missionData);
                    }
                    else
                    {
                        var missionData = new YorinSpecialMissionServerTableData();
                        missionData.idx = table[i].Id;
                        missionData.clearCount = new ReactiveProperty<int>(0);
                        missionData.rewardCount = new ReactiveProperty<int>(0);

                        defultValues.Add(table[i].Stringid, missionData.ConvertToString());
                        tableDatas.Add(table[i].Stringid, missionData);
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
  
    public void SyncToServerEach(string key)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess() == false)
            {
                Debug.Log($"요린이 미션 {key} up failed");
                return;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError($"요린이 미션 Key {key} sync complete");
            }
#endif
        });
    }
}
