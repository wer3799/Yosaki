using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

public enum WeaponType
{
    Basic,
    Normal,
    HasEffectOnly,
    View,
    RecommendView,
}
public enum MagicBookType
{
    Basic,
    View,
    Normal,
}
public enum RingType
{
    Basic,
    View,
    Normal,
}

public enum PetType
{
    Basic,
    Special,
}

public enum SuhoPetType
{
    Basic,
    Special,
}



public class EquipmentTable
{
    public static string Indate;
    public const string tableName = "Equipment";
    public static string Weapon = "Weapon";
    public static string Pet = "Pet";
    public static string MagicBook = "MagicBook";
    public static string SoulRing = "NG";
    public static string SealSword = "SS";
    public static string SealSword_View = "SSV";
    public static string Potion = "Potion";
    public static string CostumeSlot = "CostumeSlot";
    public static string CostumeLook = "CostumeLook";
    public static string CostumePresetId = "CostumePresetId";
    public static string TitleSelectId = "TitleSelectId";
    public static string WeaponEnhance = "WeaponEnhance";
    public static string WeaponE_View = "WeaponE_View";
    public static string Weapon_View = "Weapon_View";
    public static string MagicBook_View = "mv";

    public static string FoxMask = "fms";
    public static string FoxMaskView = "fmv";

    public static string DokebiHorn = "DokebiHorn";
    public static string DokebiHornView = "DokebiHornView";

    public static string CaveBelt = "CaveBelt";
    public static string CaveBeltView = "CaveBeltView";
    public static string SuhoAnimal = "SA";
    public static string DimensionEquipment = "DE";

    private Dictionary<string, int> tableSchema = new Dictionary<string, int>()
    {
        {Weapon,0},
        {Pet,-1},
        {MagicBook,-1},
        {SoulRing,-1},
        {SealSword,-1},
        {SealSword_View,-1},
        {Potion,2},
        {CostumeSlot,0},
        {CostumeLook,0},
        {CostumePresetId,0},
        {TitleSelectId,-1},
        {WeaponEnhance,0},
        {WeaponE_View,0},
        {Weapon_View,0},
        {MagicBook_View,0},
        {FoxMask,-1},
        {FoxMaskView,-1},
        {DokebiHorn,-1},
        {DokebiHornView,-1},
        {CaveBelt,-1},
        {CaveBeltView,-1},
        {SuhoAnimal,-1},
        {DimensionEquipment,0},
    };

    private ReactiveDictionary<string, ReactiveProperty<int>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<int>>();
    public ReactiveDictionary<string, ReactiveProperty<int>> TableDatas => tableDatas;

    public void ChangeEquip(string key, int idx)
    {
        if (key == Weapon)
        {
            UiTutorialManager.Instance.SetClear(TutorialStep.EquipWeapon);
        }

        tableDatas[key].Value = idx;

        SyncData(key);
    }

    public void SyncData(string key)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
#if UNITY_EDITOR
            if (e.IsSuccess() == false)
            {
                Debug.Log($"ChangeEquipe {key} {tableDatas[key].Value} up failed");
                return;
            }
            else
            {
                Debug.Log($"ChangeEquiped {key} {tableDatas[key].Value} up complete");
            }
#endif
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
                Debug.LogError("LoadStatusFailed");
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
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(e.Current.Value));
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
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(int.Parse(value)));
                        }
                        else
                        {
                            if (e.Current.Key != Weapon_View)
                            {
                                defultValues.Add(e.Current.Key, e.Current.Value);
                                tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(e.Current.Value));
                                paramCount++;
                            }
                            else if (e.Current.Key == Weapon_View)
                            {
                                int curEquip = TableDatas[Weapon].Value;
                                defultValues.Add(e.Current.Key, curEquip);
                                tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(curEquip));
                                paramCount++;
                            }
                            else if (e.Current.Key == MagicBook_View)
                            {
                                int curEquip = TableDatas[MagicBook].Value;
                                defultValues.Add(e.Current.Key, curEquip);
                                tableDatas.Add(e.Current.Key, new ReactiveProperty<int>(curEquip));
                                paramCount++;
                            }
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

    public string GetCurrentCostumePresetKey()
    {
        int presetId = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumePresetId].Value;

        if (presetId == 0)
        {
            return CostumePreset.preset_0;
        }
        else if (presetId == 1)
        {
            return CostumePreset.preset_1;
        }
        else if (presetId == 2)
        {
            return CostumePreset.preset_2;
        }

        return string.Empty;
    }
}
