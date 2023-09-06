using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BackEnd;
using LitJson;
using System.Linq;

public class SkillServerTable
{
    public static string Indate;
    public static string tableName = "Skill";

    public static string SkillHasAmount = "SkillHasAmount";
    public static string SkillAlreadyHas = "SkillAlreadyHas";
    public static string SkillAwakeNum = "SkillAwakeNum";
    public static string SkillLevel = "SkillLevel";
    public static string SkillCollectionLevel = "SkillCollectionLevel";
    public static string SkillSlotIdx_0 = "SkillSlotIdx_0";
    public static string SkillSlotIdx_1 = "SkillSlotIdx_1";
    public static string SkillSlotIdx_2 = "SkillSlotIdx_2";

    private Dictionary<string, List<int>> tableSchema = new Dictionary<string, List<int>>()
    {
        { SkillHasAmount, new List<int>() },
        { SkillAlreadyHas, new List<int>() },
        { SkillAwakeNum, new List<int>() },
        { SkillLevel, new List<int>() },
        { SkillCollectionLevel, new List<int>() },
        { SkillSlotIdx_0, new List<int>() { 0, -1, -1, -1, -1 } },
        { SkillSlotIdx_1, new List<int>() { -1, -1, -1, -1, -1 } },
        { SkillSlotIdx_2, new List<int>() { -1, -1, -1, -1, -1 } }
    };

    private Dictionary<string, List<ReactiveProperty<int>>> tableDatas = new Dictionary<string, List<ReactiveProperty<int>>>();
    public Dictionary<string, List<ReactiveProperty<int>>> TableDatas => tableDatas;

    public ReactiveCommand<List<ReactiveProperty<int>>> whenSelectedSkillIdxChanged = new ReactiveCommand<List<ReactiveProperty<int>>>();

    public List<ReactiveProperty<int>> GetSelectedSkillIdx(int skillGroup)
    {
        return tableDatas[GetSkillGroupKey(skillGroup)];
    }

    public void UpdateSelectedSkillIdx(List<int> selectedSkillIdx, int groupId)
    {
        List<ReactiveProperty<int>> skillData = new List<ReactiveProperty<int>>();

        for (int i = 0; i < selectedSkillIdx.Count; i++)
        {
            skillData.Add(new ReactiveProperty<int>(selectedSkillIdx[i]));
        }

        tableDatas[GetSkillGroupKey(groupId)] = skillData;

        SyncSkillSlotIdxToServer(groupId);


        int currentSelectedGroupId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.selectedSkillGroupId].Value;
        if (currentSelectedGroupId == groupId)
        {
            whenSelectedSkillIdxChanged.Execute(tableDatas[GetSkillGroupKey(groupId)]);
        }
    }

    public static string GetSkillGroupKey(int groupId)
    {
        if (groupId == 0)
        {
            return SkillSlotIdx_0;
        }
        else if (groupId == 1)
        {
            return SkillSlotIdx_1;
        }
        else
        {
            return SkillSlotIdx_2;
        }
    }

    public void RemoveSkillInEquipList(int idx, int groupId)
    {
        for (int i = 0; i < tableDatas[GetSkillGroupKey(groupId)].Count; i++)
        {
            if (tableDatas[GetSkillGroupKey(groupId)][i].Value == idx)
            {
                tableDatas[GetSkillGroupKey(groupId)][i].Value = -1;
            }
        }

        int currentSelectedGroupId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.selectedSkillGroupId].Value;
        if (currentSelectedGroupId == groupId)
        {
            whenSelectedSkillIdxChanged.Execute(tableDatas[GetSkillGroupKey(groupId)]);
        }

        SyncSkillSlotIdxToServer(groupId);
    }

    public bool AlreadyEquipedSkill(int idx)
    {
        for (int i = 0; i < GameBalance.skillSlotGroupNum; i++)
        {
            string key = GetSkillGroupKey(i);

            for (int j = 0; j < tableDatas[key].Count; j++)
            {
                if (tableDatas[key][j].Value == idx)
                {
                    return true;
                }
            }
        }

        return false;
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
                    List<ReactiveProperty<int>> firstData = new List<ReactiveProperty<int>>();

                    if (e.Current.Key != SkillSlotIdx_0 && e.Current.Key != SkillSlotIdx_1 && e.Current.Key != SkillSlotIdx_2)
                    {
                        var tableData = TableManager.Instance.SkillData.GetEnumerator();

                        if (e.Current.Key != SkillAwakeNum)
                        {
                            while (tableData.MoveNext())
                            {
                                firstData.Add(new ReactiveProperty<int>(0));
                            }
                        }
                        else
                        {
                            bool isFirst = true;
                            while (tableData.MoveNext())
                            {
                                int initValue = isFirst ? 1 : 0;
                                firstData.Add(new ReactiveProperty<int>(initValue));
                                isFirst = false;
                            }
                        }

                        defultValues.Add(e.Current.Key, firstData.Select(reactiveValue => reactiveValue.Value).ToList());
                    }
                    //스킬 슬롯 인덱스
                    else
                    {
                        defultValues.Add(e.Current.Key, e.Current.Value);

                        for (int i = 0; i < e.Current.Value.Count; i++)
                        {
                            var datavalue = new ReactiveProperty<int>(e.Current.Value[i]);
                            firstData.Add(datavalue);
                        }
                    }

                    tableDatas.Add(e.Current.Key, firstData);
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
                            var value = data[e.Current.Key][ServerData.format_list];
                            if (value.IsArray)
                            {
                                List<ReactiveProperty<int>> loadedData = new List<ReactiveProperty<int>>();
                                for (int j = 0; j < value.Count; j++)
                                {
                                    var intData = Int32.Parse(value[j][ServerData.format_Number].ToString());
                                    loadedData.Add(new ReactiveProperty<int>(intData));
                                }

                                tableDatas.Add(e.Current.Key, loadedData);
                            }
                        }
                        else
                        {
                            List<ReactiveProperty<int>> inputData = new List<ReactiveProperty<int>>();
                            for (int j = 0; j < e.Current.Value.Count; j++)
                            {
                                inputData.Add(new ReactiveProperty<int>(e.Current.Value[j]));
                            }

                            tableDatas.Add(e.Current.Key, inputData);
                        }
                    }
                }


                //스킬테이블 값 추가됐을때 처리
                e = tableSchema.GetEnumerator();
                while (e.MoveNext())
                {
                    if (e.Current.Key == SkillHasAmount || e.Current.Key == SkillAlreadyHas || e.Current.Key == SkillAwakeNum || e.Current.Key == SkillLevel || e.Current.Key == SkillCollectionLevel)
                    {
                        //신규스킬
                        List<ReactiveProperty<int>> firstData = new List<ReactiveProperty<int>>();

                        int needAddCount = TableManager.Instance.SkillData.Count - tableDatas[e.Current.Key].Count;

                        for (int i = 0; i < tableDatas[e.Current.Key].Count; i++)
                        {
                            firstData.Add(new ReactiveProperty<int>(tableDatas[e.Current.Key][i].Value));
                        }

                        for (int j = 0; j < needAddCount; j++)
                        {
                            firstData.Add(new ReactiveProperty<int>(0));
                            tableDatas[e.Current.Key].Add(new ReactiveProperty<int>(0));
                            paramCount++;
                        }

                        defultValues.Add(e.Current.Key, firstData.Select(reactiveValue => reactiveValue.Value).ToList());
                    }
                }
                //


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

    private List<int> skillSlotSyncData = new List<int>();
    private List<int> skillAmountSyncData = new List<int>();

    public void SyncSkillSlotIdxToServer(int groupId)
    {
        skillSlotSyncData.Clear();

        Param param = new Param();

        for (int i = 0; i < tableDatas[GetSkillGroupKey(groupId)].Count; i++)
        {
            skillSlotSyncData.Add(tableDatas[GetSkillGroupKey(groupId)][i].Value);
        }

        param.Add(GetSkillGroupKey(groupId), skillSlotSyncData);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess() == false)
            {
                Debug.LogError($"Skill slot update failed");
                return;
            }
        });
    }

    public bool HasSkill(int skillIdx)
    {
        return true;
    }

    public int GetSkillAwakeNum(int idx)
    {
        return (tableDatas[SkillAwakeNum][idx].Value);
    }

    public int GetSkillMaxLevel(int idx)
    {
        return (tableDatas[SkillAwakeNum][idx].Value) * GameBalance.SkillAwakePlusNum;
    }

    public float GetSkillCurrentLevel(int idx)
    {
        var tableData = TableManager.Instance.SkillData[idx];

        if (tableData.SKILLCASTTYPE == SkillCastType.Player)
        {
            int originLevel = tableDatas[SkillLevel][idx].Value;

            return originLevel;
        }
        else if (tableData.SKILLCASTTYPE == SkillCastType.Son)
        {
            return ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value - tableData.Sonunlocklevel;
        }
        //사천왕
        else if (tableData.SKILLCASTTYPE == SkillCastType.Four)
        {
            //return ServerData.goodsTable.GetFourSkillHasCount() - tableData.Sonunlocklevel;
            return (int)(ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value * 0.1f);
        }

        return 0;
    }

    //UI팝업에서도 쓰는부분이라 함부로 건들면 안됨
    public float GetSkillDamagePer(int idx, int addLevel = 0, bool applySkillDamAbility = true)
    {
        var currentLevel = GetSkillCurrentLevel(idx);

        //
        var tableData = TableManager.Instance.SkillData[idx];

        var plusAddValue = 0f;

        if (tableData.SKILLCASTTYPE == SkillCastType.Player)
        {
            plusAddValue = ServerData.statusTable.GetTableData(StatusTable.Skill0_AddValue).Value;
            plusAddValue += ServerData.statusTable.GetTableData(StatusTable.Skill1_AddValue).Value;
            plusAddValue += ServerData.statusTable.GetTableData(StatusTable.Skill2_AddValue).Value;
        }
        //인드라 제외
        //if (tableData.Skilltype == 5)
        //{
        //    plusAddValue = 0;
        //}
        //

        currentLevel += addLevel;

        currentLevel += plusAddValue;

        float originDamage = (TableManager.Instance.SkillData[idx].Damageper + TableManager.Instance.SkillData[idx].Damageaddvalue * (currentLevel));

        float addDamageValue = 0f;

        //도술은 제외
        if (applySkillDamAbility)
        {
            addDamageValue = PlayerStats.GetSkillDamagePercentValue();
        }


        // 추가기술데미지가 적용되기 전에 요도 각성효과 적용
        if (tableData.SKILLCASTTYPE == SkillCastType.SealSword)
        {
            int getSealAwakeGrade = PlayerStats.GetSealSwordAwakeGrade();

            if (getSealAwakeGrade != -1)
            {
                var sealTableData = TableManager.Instance.SealSwordAwakeTable.dataArray[getSealAwakeGrade];
                //ret->originDamage       
                originDamage += sealTableData.Awakevalue + PlayerStats.GetSealSwordDam();
            }
        }

        //
        float ret = originDamage + originDamage * addDamageValue;

        if (tableData.SKILLCASTTYPE == SkillCastType.Dosul)
        {
            int getDosulAwakeGrade = PlayerStats.GetDosulAwakeGrade();

            if (getDosulAwakeGrade != -1)
            {
                var sealTableData = TableManager.Instance.DosulAwakeTable.dataArray[getDosulAwakeGrade];
                //ret->originDamage       
                return (ret + sealTableData.Awakevalue* addDamageValue) * (1 + PlayerStats.GetDosulDamPer());
            }
            return ret * (1 + PlayerStats.GetDosulDamPer());
        }

        if (tableData.SKILLCASTTYPE == SkillCastType.Vision)
        {
            
            var grade = PlayerStats.GetVisionTowerGrade();
            if (grade > -1)
            {
                var data = TableManager.Instance.visionTowerTable.dataArray[grade];
                return ret * (1 + (GameBalance.VisionTreasurePerDamage * ServerData.goodsTable.GetTableData(GoodsTable.VisionTreasure).Value)+data.Abilvalue0);    
            }
            else
            {
                return ret * (1 + (GameBalance.VisionTreasurePerDamage * ServerData.goodsTable.GetTableData(GoodsTable.VisionTreasure).Value));
            }
            
        }


        return ret;
    }

    public void UpdateSkillAmountLocal(SkillTableData skillData, int addAmount)
    {
        tableDatas[SkillHasAmount][skillData.Id].Value += addAmount;
        tableDatas[SkillAlreadyHas][skillData.Id].Value = 1;
    }

    public void UpdateSKillAmount()
    {
        skillAmountSyncData.Clear();

        Param param = new Param();

        for (int i = 0; i < tableDatas[SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(tableDatas[SkillHasAmount][i].Value);
        }

        param.Add(SkillHasAmount, skillAmountSyncData);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            if (e.IsSuccess() == false)
            {
                Debug.LogError($"Skill slot update failed");
                return;
            }
        });
    }

    public bool IsFistSlotSkill(int skillId)
    {
        int currentSelectedGroupId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.selectedSkillGroupId].Value;
        var selectedSkills = ServerData.skillServerTable.TableDatas[SkillServerTable.GetSkillGroupKey(currentSelectedGroupId)];

        if (selectedSkills.Count > 0 && selectedSkills[0].Value == skillId) return true;

        return false;
    }
}