using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;

public class CostumePreset
{
    public static string Indate;
    public static string tableName = "CostumePreset";

    public const string preset_0 = "preset_0";
    public const string preset_1 = "preset_1";
    public const string preset_2 = "preset_2";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        {preset_0,""},
        {preset_1,""},
        {preset_2,""}
    };

    private Dictionary<string, string> tableDatas = new Dictionary<string, string>();
    public Dictionary<string, string> TableDatas => tableDatas;

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

                using var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {
                    string allCostumeInfo = ServerData.costumeServerTable.ConvertAllCostumeDataToString();

                    defultValues.Add(e.Current.Key, allCostumeInfo);

                    tableDatas.Add(e.Current.Key, allCostumeInfo);
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

                    // data.
                    // statusIndate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
                }

                LoadSavedCostumePreset();

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

                bool reUpdate = false;

                using var e = tableSchema.GetEnumerator();

                for (int i = 0; i < data.Keys.Count; i++)
                {
                    while (e.MoveNext())
                    {
                        if (data.Keys.Contains(e.Current.Key))
                        {
                            //값로드
                            var value = data[e.Current.Key][ServerData.format_string].ToString();

                            //
                            var splits = value.Split('|');

                            if (splits.Length >= 26)
                            {

                                for (int j = 0; j < splits.Length; j++)
                                {
                                    if (string.IsNullOrEmpty(splits[j]) == false)
                                    {
                                        var splits2 = splits[j].Split('#');


                                        if (splits2[0] == "25") 
                                        {
                                            int length = splits2[1].Split(',').Length;

                                            if (length == 6)
                                            {
                                                reUpdate = true;

                                                splits2[1] += ",-1";
                                                splits2[2] += ",0";

                                                string newValue = string.Empty;

                                                for (int k = 0; k < 25; k++)
                                                {
                                                    if (string.IsNullOrEmpty(splits[k]) == false)
                                                    {
                                                        newValue += splits[k] + "|";
                                                    }
                                                }

                                                newValue += splits2[0] + "#" + splits2[1] + "#" + splits2[2] + "|";

                                                value = newValue;
                                            }
                                        } 
                                       
                                    }
                                }

                       
                            }

                            //

                            tableDatas.Add(e.Current.Key, value);
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);

                            string allCostumeInfo = ServerData.costumeServerTable.ConvertAllCostumeDataToString();

                            tableDatas.Add(e.Current.Key, allCostumeInfo);

                            paramCount++;
                        }
                    }
                }

                if (reUpdate)
                {
                    Param newParam = new Param();

                    newParam.Add("preset_0", TableDatas["preset_0"]);
                    newParam.Add("preset_1", TableDatas["preset_1"]);
                    newParam.Add("preset_2", TableDatas["preset_2"]);

                    Backend.GameData.Update(tableName, Indate, newParam);
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

                LoadSavedCostumePreset();
            }
        });
    }

    public void LoadSavedCostumePreset()
    {
        ServerData.costumeServerTable.ApplyAbilityByCurrentSelectedPreset();
    }
}
