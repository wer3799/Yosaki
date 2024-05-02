using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;


public class DimensionStatusTable
{
    public static string Indate;
    public const string tableName = "DimensionStatus";
    public const string Level = "Level";
    public const string DSP = "DSP"; //Dimension  Stat Point
    public const string A_DS = "A_DS";
    public const string AP_DS = "AP_DS";
    public const string SD_DS = "SD_DS";
    public const string S0_DC = "S0_DC";
    public const string S1_DC = "S1_DC";
    public const string S2_DC = "S2_DC";


    private Dictionary<string, float> tableSchema = new Dictionary<string, float>()
    {
        { Level, 1 },
        { DSP, GameBalance.dimensionStatusGetPointByLevelUp },
        { A_DS, 0 },
        { AP_DS, 0 },
        { SD_DS, 0 },
        { S0_DC, 0 },
        { S1_DC, 0 },
        { S2_DC, 0 },

    };

    private Dictionary<string, ReactiveProperty<float>> tableDatas = new Dictionary<string, ReactiveProperty<float>>();

    public void SyncAllData()
    {
        Param param = new Param();

        var e = tableSchema.GetEnumerator();
        while (e.MoveNext())
        {
            param.Add(e.Current.Key, tableDatas[e.Current.Key].Value);
        }

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, bro =>
        {
            if (bro.IsSuccess() == false)
            {
                PopupManager.Instance.ShowAlarmMessage("데이터 동기화 실패\n재접속 후에 다시 시도해보세요");
                return;
            }
        });
    }

    public ReactiveProperty<float> GetTableData(string key)
    {
        return tableDatas[key];
    }

    public float GetStatusValue(string key, float level)
    {
        if (TableManager.Instance.DimensionStatusDatas.TryGetValue(key, out var data))
        {
            switch (key)
            {
                case A_DS:
                case AP_DS:
                case SD_DS:
                case S0_DC:
                case S1_DC:
                case S2_DC:
                {
                    return level * data.Addvalue;
                }
                default:
                    {
                        return 0f;
                    }
                    break;
            }
        }
        else
        {
            return 0f;
        }

        return 0f;
    }
    public float GetStatusValue(string key)
    {
        if (TableManager.Instance.DimensionStatusDatas.TryGetValue(key, out var data))
        {
            switch (key)
            {
                case A_DS:
                case AP_DS:
                case SD_DS:
                case S0_DC:
                case S1_DC:
                case S2_DC:
                {
                    var level = tableDatas[key].Value;
                    
                    return level * data.Addvalue;
                }
                default:
                    {
                        return 0f;
                    }
                    break;
            }
        }
        else
        {
            return 0f;
        }

        return 0f;
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry,
                    Initialize);
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
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(e.Current.Value));
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
                            var value = data[e.Current.Key][ServerData.format_Number].ToString();
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(float.Parse(value)));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(e.Current.Value));
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

    public void UpData(string key, bool localOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }

        UpData(key, tableDatas[key].Value, localOnly);
    }

    public void UpData(string key, float data, bool localOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }

        tableDatas[key].Value = data;

        if (localOnly == false)
        {
            Param param = new Param();
            param.Add(key, tableDatas[key].Value);

            SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
            {
                if (e.IsSuccess() == false)
                {
                    Debug.Log($"Status {key} up failed");
                    return;
                }
            });
        }
    }
}