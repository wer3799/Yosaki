using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;

public class BossScoreTable
{
    public static string Indate;
    public const string tableName = "BossScoreTable";

    //도술
    public const string dosulScore = "s0";
 

    //요도강화
    public const string SealSwordAwakeScore = "s1";

    //악귀퇴치
    public const string susanoScore = "s3";

    //사냥꾼시험
    public const string gradeScore = "s4";

    //영숲영혼사냥
    public const string relicTestScore = "s5";

    //단전
    public const string danjeonScore = "s6";

    //폐관
    public const string closedScore = "s7";

    //귀인곡
    public const string hyunsangTowerScore = "s8";

    //검은구미호
    public const string blackFoxScore = "s9";
    
    //도술강화
    public const string DosulAwakeScore = "s10";


    public bool isInitialize = false;

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { dosulScore, String.Empty },
        { SealSwordAwakeScore, string.Empty },

        { susanoScore, string.Empty },
        { gradeScore, string.Empty },
        { relicTestScore, string.Empty },

        { danjeonScore, string.Empty },
        { closedScore, string.Empty },
        { hyunsangTowerScore, string.Empty },

        { blackFoxScore, string.Empty },
        { DosulAwakeScore, string.Empty },
    };

    private Dictionary<string, ReactiveProperty<string>> tableDatas = new Dictionary<string, ReactiveProperty<string>>();
    private Dictionary<string, ReactiveProperty<double>> tableDatas_Double = new Dictionary<string, ReactiveProperty<double>>();

    public Dictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;
    public Dictionary<string, ReactiveProperty<double>> TableDatas_Double => tableDatas_Double;

    public void UpdateNumberValue(string key, double score)
    {
        if (tableDatas_Double.ContainsKey(key) == false)
        {
            tableDatas_Double.Add(key, new ReactiveProperty<double>(score));
            return;
        }

        tableDatas_Double[key].Value = score;
    }
    
    public void UpdateNumberValue(string key, string score)
    {
        if (tableDatas_Double.ContainsKey(key) == false)
        {
            tableDatas_Double.Add(key, new ReactiveProperty<double>(double.Parse(score)));
            return;
        }

        tableDatas_Double[key].Value = double.Parse(score);
    }

    public void UpdateScoreToServer(string key, double score)
    {
        tableDatas[key].Value = score.ToString("0");

        UpdateNumberValue(key, score);

        Param param = new Param();
        param.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess() == false)
            {
                Debug.LogError($"BossScoreTable {key} up failed");
                return;
            }
            else
            {
                Debug.LogError($"BossScoreTable {key} up Success({score})");
            }
        });
    }

    private string GetDefaultValue(string currentKey)
    {
        switch (currentKey)
        {
            case dosulScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulScore].Value).ToString("0");
            }
                break;
            case SealSwordAwakeScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.SealSwordAwakeScore].Value).ToString("0");
            }
                break;
            case susanoScore: //
            {
                return (ServerData.userInfoTable.TableDatas[UserInfoTable.susanoScore].Value).ToString("0");
            }
                break;
            case gradeScore:
            {
                return (ServerData.userInfoTable.TableDatas[UserInfoTable.gradeScore].Value).ToString("0");
            }
                break;
            case relicTestScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.relicTestScore].Value).ToString("0");
            }
                break;
            case danjeonScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.danjeonScore].Value).ToString("0");
            }
                break;
            case closedScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.closedScore].Value).ToString("0");
            }
                break;
            case hyunsangTowerScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.hyunsangTowerScore].Value).ToString("0");
            }
                break;
            case blackFoxScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.blackFoxScore].Value).ToString("0");
            }
                break;   
            case DosulAwakeScore: //
            {
                return (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.DosulAwakeScore].Value).ToString("0");
            }
                break;
            default:

            {
                PopupManager.Instance.ShowConfirmPopup("알림", "없는키값 {currentKey}", null);
            }
                break;
        }


        return "0";
    }


    public ReactiveProperty<string> GetTableData(string key)
    {
        return tableDatas[key];
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

            //맨처음 초기화(캐릭터생성)
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {
                    string defaultValue = GetDefaultValue(e.Current.Key);

                    defultValues.Add(e.Current.Key, defaultValue);
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(defaultValue));

                    UpdateNumberValue(e.Current.Key, defaultValue);
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
                            var value = data[e.Current.Key][ServerData.format_string].ToString();
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(value));

                            UpdateNumberValue(e.Current.Key, tableDatas[e.Current.Key].Value);
                        }
                        else
                        {
                            var defaultValue = GetDefaultValue(e.Current.Key);

                            defultValues.Add(e.Current.Key, defaultValue);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<string>(defaultValue));
                            UpdateNumberValue(e.Current.Key, defaultValue);

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
                        return; //
                    }
                }
            }

            isInitialize = true;
        });
    }
}