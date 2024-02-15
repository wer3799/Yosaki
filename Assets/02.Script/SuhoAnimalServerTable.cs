using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using System.Linq;
using UniRx;

public class SuhoSuhoPetServerData
{
    public int idx;
    public ReactiveProperty<int> hasItem;
    public ReactiveProperty<int> level;
    public ReactiveProperty<string> score;
    public ReactiveProperty<string> rewardedItem;

    public string ConvertToString()
    {
        return $"{idx},{hasItem.Value},{level.Value},{score.Value},{rewardedItem.Value}";
    }
}

public class SuhoAnimalServerTable
{
    public static string Indate;
    public static string tableName = "SuhoAnimal";

    private ReactiveDictionary<string, SuhoSuhoPetServerData> tableDatas = new ReactiveDictionary<string, SuhoSuhoPetServerData>();

    public ReactiveDictionary<string, SuhoSuhoPetServerData> TableDatas => tableDatas;

    private static Dictionary<StatusType, float> PetHasValue = new Dictionary<StatusType, float>();

    public static void ResetPetHas()
    {
        PetHasValue.Clear();
    }

    public void UpdateData(string key)
    {
        Param defultValues = new Param();

        //hasitem 1
        defultValues.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, defultValues, e => { });
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadPetFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry,
                    Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.suhoPetTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var petData = new SuhoSuhoPetServerData();
                    petData.idx = table[i].Id;
                    petData.hasItem = new ReactiveProperty<int>(0);
                    petData.level = new ReactiveProperty<int>(0);
                    petData.score = new ReactiveProperty<string>(string.Empty);
                    petData.rewardedItem =new ReactiveProperty<string>(string.Empty);

                    defultValues.Add(table[i].Stringid, petData.ConvertToString());
                    tableDatas.Add(table[i].Stringid, petData);
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

                var table = TableManager.Instance.suhoPetTable.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();

                        var petData = new SuhoSuhoPetServerData();

                        var splitData = value.Split(',');

                        petData.idx = int.Parse(splitData[0]);
                        petData.hasItem = new ReactiveProperty<int>(int.Parse(splitData[1]));
                        petData.level = new ReactiveProperty<int>(int.Parse(splitData[2]));
                        petData.score = new ReactiveProperty<string>((splitData[3]));
                        petData.rewardedItem = new ReactiveProperty<string>((splitData[4]));

                        tableDatas.Add(table[i].Stringid, petData);
                    }
                    else
                    {
                        var petData = new SuhoSuhoPetServerData();
                        petData.idx = table[i].Id;
                        petData.hasItem = new ReactiveProperty<int>(0);
                        petData.level = new ReactiveProperty<int>(0);
                        petData.score = new ReactiveProperty<string>(string.Empty);
                        petData.rewardedItem = new ReactiveProperty<string>(string.Empty);

                        defultValues.Add(table[i].Stringid, petData.ConvertToString());

                        tableDatas.Add(table[i].Stringid, petData);
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

    public int GetLastPetId()
    {
        int lastIdx = -1;
        
        var tableData = TableManager.Instance.suhoPetTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (ServerData.suhoAnimalServerTable.TableDatas[tableData[i].Stringid].hasItem.Value != 0 &&
                tableData[i].SUHOPETTYPE!=SuhoPetType.Special)
            {
                lastIdx = i;
            }
        }

        return lastIdx;
    }
    public int GetSuhoAnimalAwakeSkillIdx()
    {
        int ret = -1;

        var tableData = TableManager.Instance.suhoPetTable.dataArray;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableDatas[tableData[i].Stringid].level.Value >= GameBalance.suhoAnimalAwakeLevel)
            {
                ret = tableData[i].Awakeskillid;
            }
        }
        
        return ret;

    }
}