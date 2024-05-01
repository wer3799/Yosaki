using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using UnityEngine.Serialization;
using static UiGachaResultView;


public class YorinPassServerTable
{
    public static string Indate;
    public static string tableName = "YorinPass";
    
    public static string sealsword_free = "ssf";
    public static string ring_free = "rf";

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();

    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

    private Dictionary<string, ReactiveProperty<string>> tableSchema = new Dictionary<string, ReactiveProperty<string>>()
    {
        {sealsword_free,new ReactiveProperty<string>("-1")},
        {ring_free,new ReactiveProperty<string>("-1")},
    };

    private bool AllReceiveRewards(string productId)
    {
        var tableData = TableManager.Instance.YorinPass.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Productid == productId)
            {
                if (tableData[i].Adrewardcount <= int.Parse(tableDatas[productId].Value))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("Load YorinPass Fail");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.YorinPass.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var titleData = new ReactiveProperty<string>("-1");
                    defultValues.Add(table[i].Productid, titleData.Value);
                    tableDatas.Add(table[i].Productid, titleData);
                }

                var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {
                    defultValues.Add(e.Current.Key, e.Current.Value.Value);
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(e.Current.Value.Value));
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
                Param defultValues2 = new Param();

                int paramCount = 0;

                JsonData data = rows[0];

                if (data.Keys.Contains(ServerData.inDate_str))
                {
                    Indate = data[ServerData.inDate_str][ServerData.format_string].ToString();
                }

                var table = TableManager.Instance.YorinPass.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Productid))
                    {
                        var titleData = data[table[i].Productid][ServerData.format_string].ToString();

                        tableDatas.Add(table[i].Productid, new ReactiveProperty<string>(titleData));
                    }
                    else
                    {
                        if (paramCount < 280) 
                        {
                            var titleData = new ReactiveProperty<string>("-1");

                            defultValues.Add(table[i].Productid, titleData.Value);
                            tableDatas.Add(table[i].Productid, titleData);
                            paramCount++;
                        }
                        else 
                        {
                            var titleData = new ReactiveProperty<string>("-1");
                            defultValues2.Add(table[i].Productid, titleData.Value);
                            tableDatas.Add(table[i].Productid, titleData);
                            paramCount++;
                        }
                
                    }
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

                    if (paramCount >= 280) 
                    {
                        var bro2 = Backend.GameData.Update(tableName, Indate, defultValues2);

                        if (bro2.IsSuccess() == false)
                        {
                            ServerData.ShowCommonErrorPopup(bro, Initialize);
                            return;
                        }
                    }
                }

            }
        });
    }
}
