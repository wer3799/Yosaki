using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using LitJson;

public class AttendanceServerTable
{
    public static string Indate;
    public static string tableName = "Attendance";
    public const string rewardKey = "rewardKey2";
    public const string rewardKey_100 = "rewardKey_100";


    public static string attendFree = "f00";
    public static string attendAd = "a00";


    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        {rewardKey,""},
        {rewardKey_100,""},
        {attendFree,"0"},
        {attendAd,"0"},
    };

    private Dictionary<string, ReactiveProperty<string>> tableDatas = new Dictionary<string, ReactiveProperty<string>>();
    public Dictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

    public bool Attendance100AllReceived()
    {
        var receivedRewardList = ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.rewardKey_100].Value;

        var rewards = receivedRewardList.Split(',');

        var tableData = TableManager.Instance.AttendanceReward_100.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            string key = tableData[i].Id.ToString();

            bool hasKey = false;

            for (int j = 0; j < rewards.Length; j++)
            {
                if (rewards[j].Equals(key))
                {
                    hasKey = true;
                    break;
                }
            }

            if (hasKey == false)
            {
                return false;
            }
        }

        return true;
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

                using var e = tableSchema.GetEnumerator();

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

                using var e = tableSchema.GetEnumerator();

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
