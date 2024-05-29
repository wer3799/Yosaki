using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

[System.Serializable]
public class MarbleServerData
{
    public int idx;
    public ReactiveProperty<int> hasItem;

    public string ConvertToString()
    {
        return $"{idx},{hasItem.Value}";
    }
}

public class MarbleServerTable
{
    public static string Indate;
    public static string tableName = "MarbleTable";

    private ReactiveDictionary<string, MarbleServerData> tableDatas = new ReactiveDictionary<string, MarbleServerData>();

    public ReactiveDictionary<string, MarbleServerData> TableDatas => tableDatas;

    public bool AllMarblesUnlocked()
    {
        using var e = TableDatas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.hasItem.Value == 0)
            {
                return false;
            }
        }

        return true;
    }

    //public float GetStatusValue(StatusType statusType)
    //{
    //    float ret = 0f;
    //    int status = (int)statusType;

    //    var e = tableDatas.GetEnumerator();
    //    while (e.MoveNext())
    //    {
    //        //무료펫 능력치XX
    //        if (e.Current.Value.idx == 0) continue;
    //        if (e.Current.Value.hasItem.Value == 0) continue;

    //        var petTableData = TableManager.Instance.PetDatas[e.Current.Value.idx];
    //        if (petTableData.Hastype1 == status)
    //        {
    //            ret += petTableData.Hasvalue1;
    //        }

    //        if (petTableData.Hastype2 == status)
    //        {
    //            ret += petTableData.Hasvalue2;
    //        }
    //    }

    //    return ret;
    //}


    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadMarbleFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.MarbleTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var marbleData = new MarbleServerData();
                    marbleData.idx = table[i].Id;
                    marbleData.hasItem = new ReactiveProperty<int>(0);

                    defultValues.Add(table[i].Stringid, marbleData.ConvertToString());
                    tableDatas.Add(table[i].Stringid, marbleData);
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

                var table = TableManager.Instance.MarbleTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var marbleData = new MarbleServerData();

                        var splitData = value.Split(',');

                        marbleData.idx = int.Parse(splitData[0]);
                        marbleData.hasItem = new ReactiveProperty<int>(int.Parse(splitData[1]));


                        tableDatas.Add(table[i].Stringid, marbleData);
                    }
                    else
                    {

                        var marbleData = new MarbleServerData();
                        marbleData.idx = table[i].Id;
                        marbleData.hasItem = new ReactiveProperty<int>(0);

                        defultValues.Add(table[i].Stringid, marbleData.ConvertToString());

                        tableDatas.Add(table[i].Stringid, marbleData);
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
