using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

public enum StatusWhere
{
    gold, statpoint, memory,goldbar
}


public class StatusTable
{
    public static string Indate;
    public const string tableName = "Status";
    public const string Level = "Level";
    public const string SkillPoint = "SkillPoint";
    public const string Skill2Point = "Skill2Point";
    public const string StatPoint = "StatPoint";
    public const string Memory = "memory";
    public const string CostumeSkillPoint = "CostumeSkillPoint";

    public const string AttackLevel_Gold = "AttackLevel_Gold";
    public const string CriticalLevel_Gold = "CriticalLevel_Gold";
    public const string CriticalDamLevel_Gold = "CriticalDamLevel_Gold";
    public const string HpLevel_Gold = "HpLevel_Gold";
    public const string MpLevel_Gold = "MpLevel_Gold";
    public const string HpRecover_Gold = "HpRecover_Gold";
    public const string MpRecover_Gold = "MpRecover_Gold";

    public const string IntLevelAddPer_StatPoint = "IntLevelAddPer_StatPoint";
    public const string CriticalLevel_StatPoint = "CriticalLevel_StatPoint";
    public const string CriticalDamLevel_StatPoint = "CriticalDamLevel_StatPoint";
    public const string GoldGain_memory = "GoldGain_memory";
    public const string ExpGain_memory = "ExpGain_memory";
    public const string HpPer_StatPoint = "HpPer_StatPoint";
    public const string MpPer_StatPoint = "MpPer_StatPoint";

    public const string DamageBalance_memory = "DamageBalance_memory";
    public const string SkillDamage_memory = "SkillDamage_memory";
    public const string SkillCoolTime_memory = "SkillCoolTime_memory";
    public const string CriticalLevel_memory = "CriticalLevel_memory";
    public const string CriticalDamLevel_memory = "CriticalDamLevel_memory";
    public const string IgnoreDefense_memory = "IgnoreDefense_memory";
    public const string BossDamage_memory = "BossDamage_memory";
    public const string Son_Level = "Son_Level_Real";
    public const string PetEquip_Level = "PetEquip_Level";
    public const string RingEnhance_Level = "RingEnhance_Level";
    public const string SuhoEnhance_Level = "SuhoEnhance_Level";
    public const string Trans_Level = "Trans_Level";
    
    public const string ChunSlash_memory = "ChunSlash_memory";
    public const string PetAwakeLevel = "PetAwakeLevel";
    public const string FeelSlash_memory = "FeelSlash_memory";

    public const string ZSlash_memory = "ZSlash_memory";
    public const string Cslash_memory = "Cslash_memory";
    public const string GiSlash_memory = "GiSlash_memory";
    public const string Gum_memory = "Gum_memory";
    
    public const string Sum_memory = "Sum_memory";
    public const string Sim_memory = "Sim_memory";
    public const string Sin_memory = "Sin_memory";
    public const string Dragon_memory = "Dragon_memory";
    public const string DragonPlace_memory = "DragonPlace_memory";
    
    //public const string IgnoreDefense_GoldBar = "IgnoreDefense_GoldBar";
    //public const string GoldBarGain_GoldBar = "GoldBarGain_GoldBar";
    public const string Special0_GoldBar = "Special0_GoldBar";
    public const string Special1_GoldBar = "Special1_GoldBar";
    public const string Special2_GoldBar = "Special2_GoldBar";
    public const string Special3_GoldBar = "Special3_GoldBar";
    public const string Special4_GoldBar = "Special4_GoldBar";
    public const string Special5_GoldBar = "Special5_GoldBar";
    public const string Special6_GoldBar = "Special6_GoldBar";
    public const string Special7_GoldBar = "Special7_GoldBar";
    public const string Special8_GoldBar = "Special8_GoldBar";
    
    public const string Sin_StatPoint = "Sin_StatPoint";
    public const string Hyung_StatPoint = "Hyung_StatPoint";
    public const string Chungu_StatPoint = "Chungu_StatPoint";


    public const string Skill0_AddValue = "Sk0_Add";
    public const string Skill1_AddValue = "Sk1_Add";
    public const string Skill2_AddValue = "Sk2_Add";
    public const string SkillAdPoint = "Sk_AdPoint";
    public const string FeelMul = "FeelMul";
    public const string LeeMuGi = "LeeMuGi";


    private Dictionary<string, float> tableSchema = new Dictionary<string, float>()
    {
        { Level, 1 },
        { SkillPoint, GameBalance.SkillPointGet },
        { Skill2Point, 0 },
        { CostumeSkillPoint, 0 },
        { StatPoint, 0 },
        { Memory, 0 },
        { AttackLevel_Gold, 0 },
        { CriticalLevel_Gold, 0 },
        { CriticalDamLevel_Gold, 0 },
        { HpLevel_Gold, 0 },
        { MpLevel_Gold, 0 },
        { HpRecover_Gold, 0 },
        { MpRecover_Gold, 0 },

        //스텟초기화도 같이추가해
        { IntLevelAddPer_StatPoint, 0 },
        { CriticalLevel_StatPoint, 0 },
        { CriticalDamLevel_StatPoint, 0 },
        { HpPer_StatPoint, 0 },
        { MpPer_StatPoint, 0 },
        //스텟초기화도 같이추가해

        { DamageBalance_memory, 0 },
        { SkillDamage_memory, 0 },
        { SkillCoolTime_memory, 0 },
        { CriticalLevel_memory, 0 },
        { CriticalDamLevel_memory, 0 },
        { IgnoreDefense_memory, 0 },
        { BossDamage_memory, 0 },
        { GoldGain_memory, 0 },
        { ExpGain_memory, 0 },
        { Son_Level, 0 },
        { PetEquip_Level, 0 },
        { RingEnhance_Level, 0 },
        { SuhoEnhance_Level, 0 },
        { Trans_Level, 0 },
        { ChunSlash_memory, 0 },
        { PetAwakeLevel, 0 },
        { FeelSlash_memory, 0 },
        { ZSlash_memory, 0 },
        { Cslash_memory, 0 },
        { GiSlash_memory, 0 },
        { Gum_memory, 0 },
        { Sum_memory, 0 },
        { Sim_memory, 0 },
        { Sin_memory, 0 },
        { Dragon_memory, 0 },
        { DragonPlace_memory, 0 },
        //{ IgnoreDefense_GoldBar, 0 }, 
        //{ GoldBarGain_GoldBar, 0 },
        { Special0_GoldBar, 0 },
        { Special1_GoldBar, 0 },
        { Special2_GoldBar, 0 },
        { Special3_GoldBar, 0 },
        { Special4_GoldBar, 0 },
        { Special5_GoldBar, 0 },
        { Special6_GoldBar, 0 },
        { Special7_GoldBar, 0 },
        { Special8_GoldBar, 0 },
        
        { Sin_StatPoint, 0 },
        { Hyung_StatPoint, 0 },
        { Chungu_StatPoint, 0 },
        
        { Skill0_AddValue, 0 },
        { Skill1_AddValue, 0 },
        { Skill2_AddValue, 0 },
        { SkillAdPoint, 0 },
        { FeelMul, 0 },
        { LeeMuGi, 0 },
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

    public float GetStatusValue(string key)
    {
        return GetStatusValue(key, tableDatas[key].Value);
    }

    public float GetStatusValue(string key, float level)
    {
        if (TableManager.Instance.StatusDatas.TryGetValue(key, out var data))
        {
            switch (key)
            {
                #region Gold

                case AttackLevel_Gold:
                    {
                        float goldAbilRatio = PlayerStats.GetGoldAbilAddRatio();

                        float goldAbilRatio_Soul = PlayerStats.GetNorigaeSoulGradeValue();

                        return (Mathf.Pow(level, 1.07f) * 2f + 10) * goldAbilRatio * goldAbilRatio_Soul;
                    }
                    break;
                case CriticalLevel_Gold:
                    {
                        return level * 0.0002f;
                    }
                    break;
                case CriticalDamLevel_Gold:
                    {
                        float goldAbilRatio = PlayerStats.GetGoldAbilAddRatio();
                            
                        float goldAbilRatio_Soul = PlayerStats.GetNorigaeSoulGradeValue();

                        return level * 0.01f * goldAbilRatio * goldAbilRatio_Soul;
                    }
                    break;

                case HpLevel_Gold:
                    {
                        return GameBalance.initHp + level * GameBalance.HpLevel_Gold;
                    }
                    break;
                case MpLevel_Gold:
                    {
                        //합 무조건 100
                        return GameBalance.initMp + level * 50f;
                    }
                    break;
                case HpRecover_Gold:
                    {
                        return level * 0.0002f;
                    }
                    break;
                case MpRecover_Gold:
                    {
                        return level * 0.0001f;
                    }
                    break;

                #endregion

                #region Stat

                case IntLevelAddPer_StatPoint:
                    {
                        return level * 0.03f * GameBalance.forestValue;
                    }
                    break;
                case CriticalLevel_StatPoint:
                    {
                        return level * 0.0005f;
                    }
                    break;
                case CriticalDamLevel_StatPoint:
                    {
                        return level * 0.03f * GameBalance.forestValue;
                    }
                    break;
                case ExpGain_memory:
                    {
                        if (level == 0)
                        {
                            return 0f;
                        }
                        else
                        {
                            return 6f;
                        }
                    }
                    break;
                case GoldGain_memory:
                    {
                        if (level == 0)
                        {
                            return 0f;
                        }
                        else
                        {
                            return 6f;
                        }
                    }
                    break;
                case HpPer_StatPoint:
                    {
                        return level * GameBalance.HpPer_StatPoint;
                    }
                    break;
                case MpPer_StatPoint:
                    {
                        return level * 0.005f;
                    }
                    break;  
                case Sin_StatPoint:
                    {
                        return level * GameBalance.Stat_Sin_Slash;
                    }
                    break;
                case Hyung_StatPoint:
                    {
                        return level * GameBalance.Stat_Hyung_Slash;
                    }
                    break;

                case Chungu_StatPoint:
                    {
                        return level * GameBalance.Stat_Chungu_Slash;
                    }
                    break;

                #endregion

                #region Memory

                case DamageBalance_memory:
                    {
                        return level * 0.002f;
                    }
                    break;
                case SkillDamage_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * 0.2f * spcialAbilRatio;
                    }
                    break;
                case SkillCoolTime_memory:
                    {
                        return level * 0.0005f;
                    }
                    break;
                case CriticalLevel_memory:
                    {
                        return level * 0.001f;
                    }
                    break;
                case CriticalDamLevel_memory:
                    {
                        return level * 0.01f;
                    }
                    break;
                case IgnoreDefense_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * spcialAbilRatio;
                    }
                    break;
                case BossDamage_memory:
                    {
                        return level * 0.05f;
                    }
                case ChunSlash_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * 0.006f * spcialAbilRatio;
                    }

                case FeelSlash_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * 0.0025f * spcialAbilRatio;
                    }
                //
                case ZSlash_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * 0.000015f * spcialAbilRatio;
                    }
                case Cslash_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * 0.00001f * spcialAbilRatio;
                    }
                case GiSlash_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * 0.000005f * spcialAbilRatio;
                    }
                case Gum_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * GameBalance.Gum_memory * spcialAbilRatio;
                    }
                case Sum_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * GameBalance.Sum_memory * spcialAbilRatio;
                    }
                case Sim_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * GameBalance.Sim_memory * spcialAbilRatio;
                    }

                case Sin_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * GameBalance.Sin_memory * spcialAbilRatio;
                    }

                case Dragon_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * GameBalance.Dragon_memory * spcialAbilRatio;
                    }
                case DragonPlace_memory:
                    {
                        float spcialAbilRatio = PlayerStats.GetSpecialAbilRatio();

                        return level * GameBalance.DragonPlace_memory * spcialAbilRatio;
                    }

                #endregion

                #region GoldBar

                //
                // case IgnoreDefense_GoldBar:
                // {
                //     float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetSpecialAbilRatio() / 10000);
                //     float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 10000);
                //     return level * GameBalance.ignoreDefense_GoldBar * specialAbilityRatio * goldAbilRatio;
                // }
                
                
                // case GoldBarGain_GoldBar:
                // {
                //    // float specialAbilityRatio = Mathf.Min(1, PlayerStats.GetSpecialAbilRatio() / 10000);
                //    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetSpecialAbilRatio() / 10000);
                //    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 10000);
                //     return level * GameBalance.GoldBarGain_GoldBar* specialAbilityRatio * goldAbilRatio;
                // }
                case Special0_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special0_GoldBar * specialAbilityRatio * goldAbilRatio;
                }
                case Special1_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special1_GoldBar * specialAbilityRatio * goldAbilRatio;
                }
                case Special2_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special2_GoldBar * specialAbilityRatio * goldAbilRatio;
                }
                case Special3_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special3_GoldBar * specialAbilityRatio * goldAbilRatio;
                }
                case Special4_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special4_GoldBar * specialAbilityRatio * goldAbilRatio;
                }

                case Special5_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special5_GoldBar * specialAbilityRatio * goldAbilRatio;
                }

                case Special6_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special6_GoldBar * specialAbilityRatio * goldAbilRatio;
                }
                case Special7_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special7_GoldBar * specialAbilityRatio * goldAbilRatio;
                }
                case Special8_GoldBar:
                {
                    float specialAbilityRatio = Mathf.Max(1, PlayerStats.GetNorigaeSoulGradeValue() / 100);
                    float goldAbilRatio = Mathf.Max(1,PlayerStats.GetGoldAbilAddRatio() / 100);
                    return level * GameBalance.Special8_GoldBar * specialAbilityRatio * goldAbilRatio;
                }

                #endregion
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

    public float GoldBarUpgradeSum()
    {

        var soul0 = tableDatas[Special0_GoldBar].Value;
        var soul1 = tableDatas[Special1_GoldBar].Value;
        var soul2 = tableDatas[Special2_GoldBar].Value;
        var soul3 = tableDatas[Special3_GoldBar].Value;
        var soul4 = tableDatas[Special4_GoldBar].Value;
        var soul5 = tableDatas[Special5_GoldBar].Value;
        var soul6 = tableDatas[Special6_GoldBar].Value;
        var soul7 = tableDatas[Special7_GoldBar].Value;
        var soul8 = tableDatas[Special8_GoldBar].Value;
        var sum = soul0 + soul1 + soul2 + soul3 + soul4 + soul5 + soul6+ soul7+ soul8;
        
        return sum;
    }
    
    public float GetStatusUpgradePrice(string key, int level)
    {
        if (TableManager.Instance.StatusDatas.TryGetValue(key, out var data))
        {
            switch (key)
            {
                case AttackLevel_Gold:
                    {
                        //7월 12일버전
                        //return (Mathf.Pow(level, 2.9f + (level / 1000) * 0.1f));

                        return (Mathf.Pow(level, 2.7f + (level / 1000) * 0.1f));
                    }
                    break;
                case CriticalDamLevel_Gold:
                case CriticalLevel_Gold:
                case HpLevel_Gold:
                case MpLevel_Gold:
                case HpRecover_Gold:
                case MpRecover_Gold:
                    {
                        //7월 12일버전
                        //return Mathf.Pow(level, 3.0f + (level / 100) * 0.1f);
                        if (data.Maxlv <= level)
                        {
                            return 0f;
                        }
                        return Mathf.Pow(level, 2.9f + (level / 100) * 0.1f);
                    }
                    break;
            }
        }
        else
        {
            Debug.LogError($"key {key} is not exist in GetStatusUpgradePrice");
            return -1f;
        }

        return -1f;
    }
    
    public float GetStatusUpgradePrice(string key, float level)
    {
        if (TableManager.Instance.StatusDatas.TryGetValue(key, out var data))
        {
            switch (key)
            {
                case AttackLevel_Gold:
                    {
                        //7월 12일버전
                        //return (Mathf.Pow(level, 2.9f + (level / 1000) * 0.1f));

                        return (Mathf.Pow(level, 2.7f + (level / 1000) * 0.1f));
                    }
                    break;
                case CriticalDamLevel_Gold:
                case CriticalLevel_Gold:
                case HpLevel_Gold:
                case MpLevel_Gold:
                case HpRecover_Gold:
                case MpRecover_Gold:
                    {
                        //7월 12일버전
                        //return Mathf.Pow(level, 3.0f + (level / 100) * 0.1f);
                        if (data.Maxlv <= level)
                        {
                            return 0f;
                        }
                        return Mathf.Pow(level, 2.9f + (level / 100) * 0.1f);
                    }
                    break;
            }
        }
        else
        {
            Debug.LogError($"key {key} is not exist in GetStatusUpgradePrice");
            return -1f;
        }

        return -1f;
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