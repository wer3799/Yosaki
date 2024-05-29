using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using LitJson;

public class CostumeServerData
{
    public int idx;
    public ReactiveProperty<bool> hasCostume = new ReactiveProperty<bool>();
    public List<ReactiveProperty<int>> abilityIdx;
    public List<ReactiveProperty<int>> lockIdx;


    public string ConvertToString()
    {
        string ret = string.Empty;

        ret += $"{idx}#{(bool)hasCostume.Value}#";

        for (int i = 0; i < abilityIdx.Count; i++)
        {
            if (i == 0)
            {
                ret += $"{abilityIdx[i]}";
            }
            else
            {
                ret += $",{abilityIdx[i]}";
            }
        }

        ret += "#";

        for (int i = 0; i < lockIdx.Count; i++)
        {
            if (i == 0)
            {
                ret += $"{lockIdx[i]}";
            }
            else
            {
                ret += $",{lockIdx[i]}";
            }
        }

        return ret;
    }

    public string ConvertToExcludeHasString()
    {
        string ret = string.Empty;

        ret += $"{idx}#";

        for (int i = 0; i < abilityIdx.Count; i++)
        {
            if (i == 0)
            {
                ret += $"{abilityIdx[i]}";
            }
            else
            {
                ret += $",{abilityIdx[i]}";
            }
        }

        ret += "#";

        for (int i = 0; i < lockIdx.Count; i++)
        {
            if (i == 0)
            {
                ret += $"{lockIdx[i]}";
            }
            else
            {
                ret += $",{lockIdx[i]}";
            }
        }

        return ret;
    }



    public static CostumeServerData GetCostumeClass(string data)
    {
        var ret = new CostumeServerData();

        var split = data.Split('#');

        ret.idx = int.Parse(split[0]);
        ret.hasCostume.Value = bool.Parse(split[1]);

        var abilityList = split[2].Split(',').Select(e => int.Parse(e)).ToList().Select(e => new ReactiveProperty<int>(e)).ToList();
        var lockList = split[3].Split(',').Select(e => int.Parse(e)).ToList().Select(e => new ReactiveProperty<int>(e)).ToList();

        ret.abilityIdx = abilityList;
        ret.lockIdx = lockList;

        return ret;
    }

    public static List<ReactiveProperty<int>> GetCostumeAbilOnly(string data)
    {
        var split = data.Split('#');

        var abilityList = split[1].Split(',').Select(e => int.Parse(e)).ToList().Select(e => new ReactiveProperty<int>(e)).ToList();

        return abilityList;
    }
    public static List<ReactiveProperty<int>> GetCostumeLockOnly(string data)
    {
        var split = data.Split('#');

        var lockList = split[2].Split(',').Select(e => int.Parse(e)).ToList().Select(e => new ReactiveProperty<int>(e)).ToList();

        return lockList;
    }

    public static CostumeServerData GetDefaultCostumeClass(CostumeData tableData)
    {
        var costumeData = new CostumeServerData();

        List<ReactiveProperty<int>> abilityIdx = new List<ReactiveProperty<int>>();
        List<ReactiveProperty<int>> lockIdx = new List<ReactiveProperty<int>>();

        for (int i = 0; i < tableData.Slotnum; i++)
        {
            abilityIdx.Add(new ReactiveProperty<int>(-1));
            lockIdx.Add(new ReactiveProperty<int>(0));
        }

        costumeData.idx = tableData.Id;
        costumeData.hasCostume.Value = (tableData.Id == 0); //0번인덱스는 항상 보유

        costumeData.abilityIdx = abilityIdx;
        costumeData.lockIdx = lockIdx;

        return costumeData;
    }
}

public class CostumeServerTable
{
    public static string Indate;
    public static string tableName = "Costume";

    private ReactiveDictionary<string, CostumeServerData> tableDatas = new ReactiveDictionary<string, CostumeServerData>();
    public ReactiveDictionary<string, CostumeServerData> TableDatas => tableDatas;

    public ReactiveCommand WhenCostumeOptionChanged = new ReactiveCommand();

    public const char SplitText = '|';

    public Dictionary<int, float> abilValues = new Dictionary<int, float>();

    public string ConvertAllCostumeDataToString()
    {
        string ret = string.Empty;

        using var e = tableDatas.GetEnumerator();

        while (e.MoveNext())
        {
            ret += e.Current.Value.ConvertToExcludeHasString() + SplitText;
        }

        return ret;
    }




    public void SyncCostumeData(string key)
    {
        WhenCostumeOptionChanged.Execute();

        Param param = new Param();

        param.Add(key, tableDatas[key].ConvertToString());

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess())
            {
            }
            else if (e.IsSuccess() == false)
            {
                Debug.LogError($"SyncCostumeData {key} sync failed");
                return;
            }
        });
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadCostumeFail");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var table = TableManager.Instance.Costume.dataArray;

                for (int i = 0; i < table.Length; i++)
                {
                    var costumeData = CostumeServerData.GetDefaultCostumeClass(table[i]);

                    tableDatas.Add(table[i].Stringid, costumeData);
                    defultValues.Add(table[i].Stringid, costumeData.ConvertToString());
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

                var table = TableManager.Instance.Costume.dataArray;

                bool needSync = false;

                for (int i = 0; i < table.Length; i++)
                {
                    if (data.Keys.Contains(table[i].Stringid))
                    {
                        //값로드
                        var value = data[table[i].Stringid][ServerData.format_string].ToString();
                        var costumeData = CostumeServerData.GetCostumeClass(value);

                        if (table[i].Stringid.Equals("costume25"))
                        {
                            if (costumeData.abilityIdx.Count == 6)
                            {
                                if (costumeData.hasCostume.Value == true)
                                {
                                    costumeData = CostumeServerData.GetDefaultCostumeClass(table[i]);
                                    costumeData.hasCostume.Value = true;
                                }
                                else
                                {
                                    costumeData = CostumeServerData.GetDefaultCostumeClass(table[i]);
                                }

                                needSync = true;
                            }
                        }

                        tableDatas.Add(table[i].Stringid, costumeData);
                    }
                    //새로운 아이템 추가시
                    else
                    {
                        var costumeData = CostumeServerData.GetDefaultCostumeClass(table[i]);
                        defultValues.Add(table[i].Stringid, costumeData.ConvertToString());
                        tableDatas.Add(table[i].Stringid, costumeData);

                        paramCount++;
                    }
                }

                if (needSync)
                {
                    Param newParam = new Param();

                    newParam.Add("costume25", tableDatas["costume25"].ConvertToString());

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

            }
        });
    }

    public int GetCostumeHasAmount()
    {
        var serverData = ServerData.costumeServerTable.TableDatas;
        var tableData = TableManager.Instance.Costume.dataArray;
        int costumeCount = 0;
        for (int i = 0; i < serverData.Count; i++)
        {
            if (serverData[tableData[i].Stringid].hasCostume.Value == true)
            {
                costumeCount++;
             }
        }
        return costumeCount;
    }
    public void ApplyAbilityByCurrentSelectedPreset()
    {
        string currentSelectedData = ServerData.costumePreset.TableDatas[ServerData.equipmentTable.GetCurrentCostumePresetKey()];
        
        var costumeAbilities = currentSelectedData.Split(SplitText);

        for (int i = 0; i < costumeAbilities.Length; i++)
        {
            if (string.IsNullOrEmpty(costumeAbilities[i])) continue;

            var costumeTableData = TableManager.Instance.Costume.dataArray[i];
            
            tableDatas[costumeTableData.Stringid].abilityIdx = CostumeServerData.GetCostumeAbilOnly(costumeAbilities[i]);
            tableDatas[costumeTableData.Stringid].lockIdx = CostumeServerData.GetCostumeLockOnly(costumeAbilities[i]);
        }

        abilValues = new Dictionary<int, float>();

        using var e = tableDatas.GetEnumerator();

        while (e.MoveNext())
        {
            for (int i = 0; i < e.Current.Value.abilityIdx.Count; i++)
            {
                var abilityInfo = TableManager.Instance.CostumeAbilityData[e.Current.Value.abilityIdx[i].Value];

                if (abilValues.ContainsKey(abilityInfo.Abilitytype) == false)
                {
                    abilValues.Add(abilityInfo.Abilitytype, 0f);
                }

                abilValues[abilityInfo.Abilitytype] += abilityInfo.Abilityvalue;
            }
        }
    }

    public float GetCostumeAbility(StatusType type)
    {
        int key = (int)type;

        if (abilValues.ContainsKey(key))
        {
            if(PlayerStats.IsChunFlowerCostumeEnhance())
            {
                return (abilValues[(int)type] * (PlayerStats.IsCostumeCollectionEnhance() + TableManager.Instance.chunMarkAbil.dataArray[5].Abiladdvalue));
            }
            else
            {
                return abilValues[(int)type] * PlayerStats.IsCostumeCollectionEnhance();
            }            
        }
        else
        {
            return 0f;
        }
    }
}
