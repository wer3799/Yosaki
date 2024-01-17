using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using UnityEngine.Serialization;

[System.Serializable]
public class SpecialTitleServerData
{
    public int idx;
    public ReactiveProperty<float> hasItem;

    public string ConvertToString()
    {
        return $"{idx},{hasItem.Value}";
    }
}

public enum SpecialTitleUnlockType
{
    Weapon,
    Son_Level_Real,
    SP,
    SuhoPet,
}

public class SpecialTitleServerTable : MonoBehaviour
{
    public static string Indate;
    public static string tableName = "SpecialTitle";

    private ReactiveDictionary<string, SpecialTitleServerData> tableDatas = new ReactiveDictionary<string, SpecialTitleServerData>();

    public ReactiveDictionary<string, SpecialTitleServerData> TableDatas => tableDatas;

    
    
    
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
                Param defultValues = new Param();

                var table = TableManager.Instance.Title_Special.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var relicData = new SpecialTitleServerData();
                    relicData.idx = table[i].Id;
                    relicData.hasItem = new ReactiveProperty<float>(0);

                    defultValues.Add(table[i].Stringid, relicData.ConvertToString());
                    tableDatas.Add(table[i].Stringid, relicData);
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

                var table = TableManager.Instance.Title_Special.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var relicData = new SpecialTitleServerData();

                        var splitData = value.Split(',');

                        relicData.idx = int.Parse(splitData[0]);
                        relicData.hasItem = new ReactiveProperty<float>(float.Parse(splitData[1]));

                        tableDatas.Add(table[i].Stringid, relicData);
                    }
                    else
                    {

                        var relicData = new SpecialTitleServerData();
                        relicData.idx = table[i].Id;
                        relicData.hasItem = new ReactiveProperty<float>(0);

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


