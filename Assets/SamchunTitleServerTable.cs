using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using UnityEngine.Serialization;

[System.Serializable]
public class SamchunServerData
{
    public int idx;
    public ReactiveProperty<float> hasReward;

    public string ConvertToString()
    {
        return $"{idx},{hasReward.Value}";
    }
}


public class SamchunTitleServerTable : MonoBehaviour
{
    public static string Indate;
    public static string tableName = "SamchunTable";

    private ReactiveDictionary<string, SamchunServerData> tableDatas = new ReactiveDictionary<string, SamchunServerData>();

    public ReactiveDictionary<string, SamchunServerData> TableDatas => tableDatas;

    
    
    
    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("Load Relic failed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defaultValues = new Param();

                var table = TableManager.Instance.Title_Samcheon.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var relicData = new SamchunServerData();
                    relicData.idx = table[i].Id;
                    relicData.hasReward = new ReactiveProperty<float>(0);

                    defaultValues.Add(table[i].Stringid, relicData.ConvertToString());
                    tableDatas.Add(table[i].Stringid, relicData);
                }

                var bro = Backend.GameData.Insert(tableName, defaultValues);

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

                var table = TableManager.Instance.Title_Samcheon.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var relicData = new SamchunServerData();

                        var splitData = value.Split(',');

                        relicData.idx = int.Parse(splitData[0]);
                        relicData.hasReward = new ReactiveProperty<float>(float.Parse(splitData[1]));

                        tableDatas.Add(table[i].Stringid, relicData);
                    }
                    else
                    {

                        var relicData = new SamchunServerData();
                        relicData.idx = table[i].Id;
                        relicData.hasReward = new ReactiveProperty<float>(0);

                        defultValues.Add(table[i].Stringid, relicData.ConvertToString());

                        tableDatas.Add(table[i].Stringid, relicData);
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
}


