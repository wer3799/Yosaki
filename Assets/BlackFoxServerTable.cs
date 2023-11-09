using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

[System.Serializable]
public class  BlackFoxServerData
{
    public int idx;
    public ReactiveProperty<float> level;

    public string ConvertToString()
    {
        return $"{idx},{level.Value}";
    }
}


public class BlackFoxServerTable : MonoBehaviour
{
    public static string Indate;
    public static string tableName = "BlackFox";

    private ReactiveDictionary<string, BlackFoxServerData> tableDatas = new ReactiveDictionary<string, BlackFoxServerData>();

    public ReactiveDictionary<string, BlackFoxServerData> TableDatas => tableDatas;

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("Load BlackFox failed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }
        
            var rows = callback.Rows();
        
            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();
        
                var table = TableManager.Instance.BlackFoxAbil.dataArray;
        
                for (int i = 0; i < table.Length; i++)
                {
                    var blackFoxData = new BlackFoxServerData();
                    blackFoxData.idx = table[i].Id;
                    blackFoxData.level = new ReactiveProperty<float>(0);
        
                    defultValues.Add(table[i].Stringid, blackFoxData.ConvertToString());
                    tableDatas.Add(table[i].Stringid, blackFoxData);
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
        
                var table = TableManager.Instance.BlackFoxAbil.dataArray;
        
                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();
        
                        var blackFoxData = new BlackFoxServerData();
        
                        var splitData = value.Split(',');
        
                        blackFoxData.idx = int.Parse(splitData[0]);
                        blackFoxData.level = new ReactiveProperty<float>(float.Parse(splitData[1]));
        
                        tableDatas.Add(table[i].Stringid, blackFoxData);
                    }
                    else
                    {
        
                        var blackFoxData = new BlackFoxServerData();
                        blackFoxData.idx = table[i].Id;
                        blackFoxData.level = new ReactiveProperty<float>(0);
        
                        defultValues.Add(table[i].Stringid, blackFoxData.ConvertToString());
        
                        tableDatas.Add(table[i].Stringid, blackFoxData);
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


