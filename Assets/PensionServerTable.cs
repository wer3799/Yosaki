using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using System.Linq;

public class PensionServerTable
{
    public static string Indate;
    public const string tableName = "PensionTable";


    public static string oakpension = "oakpension";
    public static string marblepension = "marblepension";
    public static string relicpension = "relicpension";
    public static string peachpension = "peachpension";
    public static string smithpension = "smithpension";
    public static string weaponpension = "weaponpension";
    public static string hellpension = "hellpension";
    public static string chunpension = "chunpension";
    public static string dokebipension = "dokebipension";
    public static string sumipension = "sumipension";
    public static string ringpension = "ringpension";
    public static string suhopetfeedclearpension = "suhopetfeedclearpension";
    public static string foxfirepension = "foxfirepension";
    public static string sealswordpension = "sealswordpension";
    public static string dosulpension = "dosulpension";
    public static string guimoonpension = "guimoonpension";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { oakpension,string.Empty},
        { marblepension,string.Empty},
        { relicpension,string.Empty},
        { peachpension,string.Empty},
        { smithpension,string.Empty},
        { weaponpension,string.Empty},
        { hellpension,string.Empty},
        { chunpension,string.Empty},
        { dokebipension,string.Empty},
        { sumipension,string.Empty},
        { ringpension,string.Empty},
        { suhopetfeedclearpension,string.Empty},
        { foxfirepension,string.Empty},
        { sealswordpension,string.Empty},
        { dosulpension,string.Empty},
        { guimoonpension,string.Empty},
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

    public bool HasReward(string key, int data)
    {
        var splitData = GetSplitData(key);
        return splitData.Contains(data.ToString());
    }
    public List<string> GetSplitData(string key)
    {
        return TableDatas[key].Value.Split(',').ToList();
    }
    public int RewarededCount(string key)
    {
        return GetSplitData(key).Count;
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
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
                    defultValues.Add(e.Current.Key, e.Current.Value);
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(e.Current.Value));
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
                        //새로운값
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(e.Current.Value));
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
}
