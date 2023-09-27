using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using static UiGachaResultView;

[System.Serializable]
public class MagicBookServerData
{
    public int idx;
    public ReactiveProperty<int> hasItem;
    public ReactiveProperty<int> level;
    public ReactiveProperty<int> amount;
    public ReactiveProperty<int> collectLevel;

    public ReactiveProperty<int> getReward0;
    public ReactiveProperty<int> getReward1;
    public ReactiveProperty<int> trans;

    public string ConvertToString()
    {
        return $"{idx},{hasItem.Value},{level.Value},{amount.Value},{collectLevel.Value},{getReward0.Value},{getReward1.Value},{trans.Value}";
    }
}


public class MagicBookTable
{
    public static string Indate;
    public static string tableName = "MagicBook";


    private ReactiveDictionary<string, MagicBookServerData> tableDatas = new ReactiveDictionary<string, MagicBookServerData>();

    public ReactiveDictionary<string, MagicBookServerData> TableDatas => tableDatas;

    public float GetMagicBookEffectValue(string id, float baseValue, float addValue, int level = -1)
    {
        if (level == -1)
        {
            return baseValue + addValue * tableDatas[id].level.Value;
        }
        else
        {
            return baseValue + addValue * level;
        }
    }


    public MagicBookServerData GetMagicBookData(string idx)
    {
        if (tableDatas.TryGetValue(idx, out var data))
        {
            return data;
        }
        else
        {
            return null;
        }
    }
    public int GetCurrentMagicBookCount(string idx)
    {
        return tableDatas[idx].amount.Value;
    }

    public float GetMagicBookLevelUpPrice(string idx)
    {
        int level = tableDatas[idx].level.Value;
        int id = tableDatas[idx].idx;
        level += 1;

        if (id > 79)
        {
            id = 79;
        }
        
        if (id < 16)
        {
            return Mathf.Pow(level, 3.54f + (float)id * 0.015f);
        }
        //영물
        else if (id == 20)
        {
            return Mathf.Pow(level, 3.55f + (float)id * 0.015f);
        }
        else
        {
            return Mathf.Pow(level, 3.65f + (float)id * 0.015f);
        }


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

                var table = TableManager.Instance.MagicBookTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var magicBookData = new MagicBookServerData();
                    magicBookData.idx = table[i].Id;
                    magicBookData.hasItem = new ReactiveProperty<int>(0);
                    magicBookData.level = new ReactiveProperty<int>(0);
                    magicBookData.amount = new ReactiveProperty<int>(0);
                    magicBookData.collectLevel = new ReactiveProperty<int>(0);

                    magicBookData.getReward0 = new ReactiveProperty<int>(0);
                    magicBookData.getReward1 = new ReactiveProperty<int>(0);
                    magicBookData.trans = new ReactiveProperty<int>(0);

                    tableDatas.Add(table[i].Stringid, magicBookData);
                    defultValues.Add(table[i].Stringid, magicBookData.ConvertToString());
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

                var table = TableManager.Instance.MagicBookTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var magicBook = new MagicBookServerData();

                        var splitData = value.Split(',');

                        magicBook.idx = int.Parse(splitData[0]);
                        magicBook.hasItem = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        magicBook.level = new ReactiveProperty<int>(int.Parse(splitData[2]));
                        magicBook.amount = new ReactiveProperty<int>(int.Parse(splitData[3]));

                        if (splitData.Length >= 5)
                        {
                            magicBook.collectLevel = new ReactiveProperty<int>(int.Parse(splitData[4]));
                        }
                        else
                        {
                            magicBook.collectLevel = new ReactiveProperty<int>(0);
                        }
                        if (splitData.Length >= 6)
                        {
                            magicBook.getReward0 = new ReactiveProperty<int>(int.Parse(splitData[5]));
                            magicBook.getReward1 = new ReactiveProperty<int>(int.Parse(splitData[6]));
                        }
                        else
                        {
                            magicBook.getReward0 = new ReactiveProperty<int>(0);
                            magicBook.getReward1 = new ReactiveProperty<int>(0);
                            paramCount++;
                            defultValues.Add(table[i].Stringid, magicBook.ConvertToString());
                        }
                        if (splitData.Length >= 8)
                        {
                            magicBook.trans = new ReactiveProperty<int>(int.Parse(splitData[7]));
                        }
                        else
                        {
                            magicBook.trans = new ReactiveProperty<int>(0);
                            paramCount++;
                            defultValues.Add(table[i].Stringid, magicBook.ConvertToString());
                        }
                        tableDatas.Add(table[i].Stringid, magicBook);
                    }
                    else
                    {

                        var magicBookData = new MagicBookServerData();
                        magicBookData.idx = table[i].Id;
                        magicBookData.hasItem = new ReactiveProperty<int>(0);
                        magicBookData.level = new ReactiveProperty<int>(0);
                        magicBookData.amount = new ReactiveProperty<int>(0);
                        magicBookData.collectLevel = new ReactiveProperty<int>(0);
                        magicBookData.getReward0 = new ReactiveProperty<int>(0);
                        magicBookData.getReward1 = new ReactiveProperty<int>(0);
                        magicBookData.trans = new ReactiveProperty<int>(0);

                        tableDatas.Add(table[i].Stringid, magicBookData);
                        defultValues.Add(table[i].Stringid, magicBookData.ConvertToString());
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

    public void UpData(MagicBookData magicBookData, int addNum)
    {
        if (tableDatas[magicBookData.Stringid].hasItem.Value == 0)
        {
            tableDatas[magicBookData.Stringid].hasItem.Value = 1;
        }

        tableDatas[magicBookData.Stringid].amount.Value += addNum;
    }


    public void SyncToServerEach(string key)
    {
        Param defultValues = new Param();

        //hasitem 1
        defultValues.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, bro =>
           {
               if (bro.IsSuccess() == false)
               {
                   ServerData.ShowCommonErrorPopup(bro, () => { SyncToServerEach(key); });
                   return;
               }
           });
    }

    public void SyncToServerAll(List<int> updateList = null)
    {
        Param defultValues = new Param();

        var table = TableManager.Instance.MagicBookTable.dataArray;

        for (int i = 0; i < table.Length; i++)
        {
            if (updateList != null && updateList.Contains(table[i].Id) == false) continue;

            string key = table[i].Stringid;
            //hasitem 1
            defultValues.Add(key, tableDatas[key].ConvertToString());
        }

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, bro =>
           {
               if (bro.IsSuccess() == false)
               {
                   ServerData.ShowCommonErrorPopup(bro, () => { SyncToServerAll(updateList); });
                   return;
               }
           });
    }

}
