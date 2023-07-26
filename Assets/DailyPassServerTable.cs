using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using System.Linq;

public class DailyPassServerTable
{
    public static string Indate;
    public const string tableName = "DailyPass";


    public static string DailypassFreeReward = "Free_new3";
    public static string DailypassAdReward = "Ad_new3";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { DailypassFreeReward,string.Empty},
        { DailypassAdReward,string.Empty}
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;


    public void ResetDailyPassLocal()
    {
        var e = tableDatas.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.Value.Value = string.Empty;
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

public class MonthlyPassServerTable
{
    public static string Indate;
    public const string tableName = "MonthlyPass";


    public static string MonthlypassFreeReward = "f10";
    public static string MonthlypassAdReward = "a10";

    public static string MonthlypassAttendFreeReward = "af11";
    public static string MonthlypassAttendAdReward = "aa11";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { MonthlypassFreeReward,string.Empty},
        { MonthlypassAdReward,string.Empty},
        { MonthlypassAttendFreeReward,string.Empty},
        { MonthlypassAttendAdReward,string.Empty}
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

    public void Initialize()
    {
        //LoadTable(true);
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
//
public class MonthlyPassServerTable2
{
    public static string Indate;
    public const string tableName = "MonthlyPass2";


    public static string MonthlypassFreeReward = "f";
    public static string MonthlypassAdReward = "a";

    public static string MonthlypassAttendFreeReward = "af";
    public static string MonthlypassAttendAdReward = "aa";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { MonthlypassFreeReward,string.Empty},
        { MonthlypassAdReward,string.Empty},
        { MonthlypassAttendFreeReward,string.Empty},
        { MonthlypassAttendAdReward,string.Empty}
    };
    

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;
    private void LoadTable(bool isOddMonthlypass = false)
    {
        var datas= TableManager.Instance.InAppPurchase.dataArray;
        if (isOddMonthlypass)
        {
            List<int> evenList = new List<int>();
            foreach (var purchaseData in datas)
            {
                if (purchaseData.PASSPRODUCTTYPE == PassProductType.MonthPass)
                {
                    int num = int.Parse(purchaseData.Productid.Replace("monthpass", ""));
                    if (num % 2 == 1)
                    {
                        //홀수는 짝수월(monthpass2,4,6...)
                        //oddList.Add(num);
                    }
                    else
                    {
                        //짝수는 홀수월(1,3,5...)
                        evenList.Add(num);
                    }
                }
            }
            MonthlypassFreeReward += "_"+evenList.Last();
            MonthlypassAdReward += "_"+evenList.Last();
            MonthlypassAttendFreeReward +="_"+evenList.Last(); 
            MonthlypassAttendAdReward +="_"+evenList.Last();
        }
        else
        {
            List<int> oddList = new List<int>();
            foreach (var purchaseData in datas)
            {
                if (purchaseData.PASSPRODUCTTYPE == PassProductType.MonthPass)
                {
                    int num = int.Parse(purchaseData.Productid.Replace("monthpass", ""));
                    if (num % 2 == 1)
                    {
                        //홀수는 짝수월(monthpass2,4,6...)
                        oddList.Add(num);
                    }
                    else
                    {
                        //짝수는 홀수월(1,3,5...)
                        //evenList.Add(num);
                    }
                }
            }
            MonthlypassFreeReward += "_"+oddList.Last();
            MonthlypassAdReward += "_"+oddList.Last();
            MonthlypassAttendFreeReward +="_"+oddList.Last(); 
            MonthlypassAttendAdReward +="_"+oddList.Last();
        }
        tableSchema.Add(MonthlypassFreeReward,string.Empty);
        tableSchema.Add(MonthlypassAdReward,string.Empty);
        tableSchema.Add(MonthlypassAttendFreeReward,string.Empty);
        tableSchema.Add(MonthlypassAttendAdReward,string.Empty);
    }
    public void Initialize()
    {
        LoadTable(true);
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

public class SeolPassServerTable
{
    public static string Indate;
    public const string tableName = "SeolPass";


    public static string MonthlypassFreeReward = "free";
    public static string MonthlypassAdReward = "ad";  
    public static string MonthlypassFreeReward_dol = "free_d";
    public static string MonthlypassAdReward_dol = "ad_d";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { MonthlypassFreeReward,string.Empty},
        { MonthlypassAdReward,string.Empty},
        { MonthlypassFreeReward_dol,string.Empty},
        { MonthlypassAdReward_dol,string.Empty}
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

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


public class SulPassServerTable
{
    public static string Indate;
    public const string tableName = "SulPass";


    public static string MonthlypassFreeReward = "free";
    public static string MonthlypassAdReward = "ad";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { MonthlypassFreeReward,string.Empty},
        { MonthlypassAdReward,string.Empty}
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

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

public class ChildPassServerTable
{
    public static string Indate;
    public const string tableName = "ChildPass";


    public static string childFree = "f6";
    public static string childAd = "a6";

    public static string childFree_Atten = "atf";
    public static string childAd_Atten = "ata";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { childFree,string.Empty},
        { childAd,string.Empty},
        { childFree_Atten,string.Empty},
        { childAd_Atten,string.Empty}
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

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
//
public class ColdSeasonPassServerTable
{
    public static string Indate;
    public const string tableName = "ColdSeason";


    public static string coldseasonFree = "f2";
    public static string coldseasonAd = "a2";
    public static string seasonFree = "f8";
    public static string seasonAd = "a8";
    public static string suhoFree = "f4";
    public static string suhoAd = "a4";
    public static string foxfireFree = "f5";
    public static string foxfireAd = "a5";
    public static string sealSwordFree = "f6";
    public static string sealSwordAd = "a6";
    public static string gangChulFree = "f7";
    public static string gangChuldAd = "a7";
    public static string SoulForestFree = "f9";
    public static string SoulForestdAd = "a9";
    public static string SwordFree = "f10";
    public static string SwordAd = "a11";
    public static string dosulFree = "f12";
    public static string dosulAd = "a12";

    public static string guimoonFree = "f13";
    public static string guimoonAd = "a13";

    public static string secondAccumul = "f14";
    public static string secondTop = "a14";

    public static string coldseasonFree_Atten = "fa2";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { coldseasonFree,string.Empty},
        { coldseasonAd,string.Empty},
        { coldseasonFree_Atten,string.Empty},
        { seasonFree,string.Empty},
        { seasonAd,string.Empty},
        { suhoFree,string.Empty},
        { suhoAd,string.Empty},
        { foxfireFree,string.Empty},
        { foxfireAd,string.Empty},
        { sealSwordFree,string.Empty},
        { sealSwordAd,string.Empty},
        { gangChulFree,string.Empty},
        { gangChuldAd,string.Empty},
        { SoulForestFree,string.Empty},
        { SoulForestdAd,string.Empty},
        { SwordFree,string.Empty},
        { SwordAd,string.Empty},
        { dosulFree,string.Empty},
        { dosulAd,string.Empty},
        { guimoonFree,string.Empty},
        { guimoonAd,string.Empty},
        { secondAccumul,string.Empty},
        { secondTop,string.Empty},

    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

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
//

public class BokPassServerTable
{
    public static string Indate;
    public const string tableName = "BokPass";


    public static string childFree = "f3";
    public static string childAd = "a3";

    public static string springFree = "ff0";
    public static string springAd = "aa0";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { childFree,string.Empty},
        { childAd,string.Empty},

        { springFree,string.Empty},
        { springAd,string.Empty}
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

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

    public bool AttendanceBokPassAllReceived()
    {
        var receivedFreeRewardList = ServerData.bokPassServerTable.TableDatas[BokPassServerTable.childFree].Value;

        var receivedAdRewardList = ServerData.bokPassServerTable.TableDatas[BokPassServerTable.childAd].Value;


        var freeRewards = receivedFreeRewardList.Split(',');

        var adRewards = receivedAdRewardList.Split(',');

        var tableData = TableManager.Instance.bokPass.dataArray;


        if (tableData.Length > freeRewards.Length - 1)
        {
            return false;
        }

        if (tableData.Length > adRewards.Length - 1)
        {
            return false;
        }

        return true;
    }
    //

}
public class OneYearPassServerTable
{
    public static string Indate;
    public const string tableName = "onePass";


    //2023 봄나물 사용중
    public static string childFree = "f5";
    public static string childAd = "a5";

    //2023 어린이날 //2022눈사람 사용중
    public static string childFree_Snow = "fs1";
    public static string childAd_Snow = "as1";
    
    //2023 어린이날 //2022눈사람 사용중
    public static string commonEventFree = "cef0";
    public static string commonEventAd = "cea0";

    private Dictionary<string, string> tableSchema = new Dictionary<string, string>()
    {
        { childFree,string.Empty},
        { childAd,string.Empty},
        { childFree_Snow,string.Empty},
        { childAd_Snow,string.Empty},
        { commonEventFree,string.Empty},
        { commonEventAd,string.Empty},
    };

    private ReactiveDictionary<string, ReactiveProperty<string>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<string>>();
    public ReactiveDictionary<string, ReactiveProperty<string>> TableDatas => tableDatas;

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