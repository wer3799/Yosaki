using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EventMissionType
{
    FIRST,
    SECOND,
    THIRD,
}
public enum EventMissionKey
{
    MISSION1,//문파점수
    MISSION2,//견공
    MISSION3,//십만대산
    MISSION4,//
    MISSION5,//
    MISSION6,//
    MISSION7,//
    MISSION8,//

    /////

    S_ClearBandit=8,//반딧불전
    S_ClearOni,//도깨비전
    S_ClearFast,//빠른전투
    S_ClearSwordPartial,//검조각 보상 ★
    S_ClearHell,//불멸석 보상 ★
    S_ClearChunFlower,//천계꽃 보상 ★
    S_ClearDokebiFire,//도깨비나라 보상 ★
    S_ClearSumiFire,//수미산 보상 ★
    S_ClearSoulStone,//영혼석 보상
    
    TMISSION1,//반딧
    TMISSION2,//깨비전
    TMISSION3,//빠른전투
    TMISSION4,//검의산
    TMISSION5,//불멸석
    TMISSION6,//천계꽃
    TMISSION7,//도깨비
    TMISSION8,//수미산
    TMISSION9,//영혼석(반지)
    TMISSION10,//문파점수등록
    TMISSION11,//견공
}
public enum MonthMissionKey
{
    ClearGangChul,//강철이전 초기화
    ClearBandit,//반딧불전
    ClearOni,//도깨비전
    ClearFast,//빠른전투
    ClearSwordPartial,//검조각 보상 ★
    ClearHell,//불멸석 보상 ★
    ClearChunFlower,//천계꽃 보상 ★
    ClearDokebiFire,//도깨비나라 보상 ★
    ClearSumiFire,//수미산 보상 ★
    ClearSoulStone,//영혼석 보상
}
public enum MonthMission2Key
{
    ClearGangChul,//강철이전 초기화
    ClearBandit,//반딧불전
    ClearOni,//도깨비전
    ClearFast,//빠른전투
    ClearSwordPartial,//검조각 보상 ★
    ClearHell,//불멸석 보상 ★
    ClearChunFlower,//천계꽃 보상 ★
    ClearDokebiFire,//도깨비나라 보상 ★
    ClearSumiFire,//수미산 보상 ★
    ClearSoulStone,//영혼석 보상
}

public static class EventMissionManager
{
    private static Dictionary<EventMissionKey, Coroutine> SyncRoutines = new Dictionary<EventMissionKey, Coroutine>();
    private static Dictionary<MonthMissionKey, Coroutine> SyncRoutines2 = new Dictionary<MonthMissionKey, Coroutine>();
    private static Dictionary<MonthMission2Key, Coroutine> SyncRoutines3 = new Dictionary<MonthMission2Key, Coroutine>();

    private static WaitForSeconds syncDelay = new WaitForSeconds(3.0f);

    private static WaitForSeconds syncDelay_slow = new WaitForSeconds(300.0f);

    public static void UpdateEventMissionClear(EventMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.EventMissionDatas[(int)missionKey].Stringid;

        
        
        if (ServerData.eventMissionTable.TableDatas[key].clearCount.Value >= 1&&Utils.IsDailyEventMissionKey(missionKey)==true) return;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionClearCount(key, count);



        //서버저장
        if (SyncRoutines.ContainsKey(missionKey) == false)
        {
            SyncRoutines.Add(missionKey, null);
        }

        if (SyncRoutines[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines[missionKey]);
        }

        SyncRoutines[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }
    //월간미션
    public static void UpdateEventMissionClear(MonthMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.MonthMissionDatas[(int)missionKey].Stringid;
        
        //if (missionKey.IsIgnoreMissionKey()) return;
        
        if (ServerData.eventMissionTable.TableDatas[key].clearCount.Value >= TableManager.Instance.MonthMissionDatas[(int)missionKey].Monthmaxclear) return;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionClearCount(key, count);



        //서버저장
        if (SyncRoutines2.ContainsKey(missionKey) == false)
        {
            SyncRoutines2.Add(missionKey, null);
        }

        if (SyncRoutines2[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines2[missionKey]);
        }

        SyncRoutines2[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }
    //월간미션2
    public static void UpdateEventMissionClear(MonthMission2Key missionKey, int count)
    {
        string key = TableManager.Instance.MonthMission2Datas[(int)missionKey].Stringid;
        
        //if (missionKey.IsIgnoreMissionKey()) return;
        
        if (ServerData.eventMissionTable.TableDatas[key].clearCount.Value >= TableManager.Instance.MonthMission2Datas[(int)missionKey].Monthmaxclear) return;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionClearCount(key, count);



        //서버저장
        if (SyncRoutines3.ContainsKey(missionKey) == false)
        {
            SyncRoutines3.Add(missionKey, null);
        }

        if (SyncRoutines3[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines3[missionKey]);
        }

        SyncRoutines3[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }
    public static void UpdateEventMissionReward(EventMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.EventMissionDatas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionRewardCount(key, count);



        //서버저장
        if (SyncRoutines.ContainsKey(missionKey) == false)
        {
            SyncRoutines.Add(missionKey, null);
        }

        if (SyncRoutines[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines[missionKey]);
        }

        SyncRoutines[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }
    public static void UpdateEventMissionReward(MonthMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.MonthMissionDatas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionRewardCount(key, count);



        //서버저장
        if (SyncRoutines2.ContainsKey(missionKey) == false)
        {
            SyncRoutines2.Add(missionKey, null);
        }

        if (SyncRoutines2[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines2[missionKey]);
        }

        SyncRoutines2[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }
    public static void UpdateEventMissionReward(MonthMission2Key missionKey, int count)
    {
        string key = TableManager.Instance.MonthMission2Datas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionRewardCount(key, count);



        //서버저장
        if (SyncRoutines3.ContainsKey(missionKey) == false)
        {
            SyncRoutines3.Add(missionKey, null);
        }

        if (SyncRoutines3[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines3[missionKey]);
        }

        SyncRoutines3[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }
    public static void UpdateEventMissionAdReward(MonthMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.MonthMissionDatas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionAdRewardCount(key, count);



        //서버저장
        if (SyncRoutines2.ContainsKey(missionKey) == false)
        {
            SyncRoutines2.Add(missionKey, null);
        }

        if (SyncRoutines2[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines2[missionKey]);
        }

        SyncRoutines2[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }

    public static void UpdateEventMissionAdReward(MonthMission2Key missionKey, int count)
    {
        string key = TableManager.Instance.MonthMission2Datas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.eventMissionTable.UpdateMissionAdRewardCount(key, count);



        //서버저장
        if (SyncRoutines3.ContainsKey(missionKey) == false)
        {
            SyncRoutines3.Add(missionKey, null);
        }

        if (SyncRoutines3[missionKey] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutines3[missionKey]);
        }

        SyncRoutines3[missionKey] = CoroutineExecuter.Instance.StartCoroutine(SyncToServerRoutine(key, missionKey));
    }




    private static IEnumerator SyncToServerRoutine(string key, EventMissionKey missionKey)
    {
        ServerData.eventMissionTable.SyncToServerEach(key);

        SyncRoutines[missionKey] = null;
        yield return null;
    }
    private static IEnumerator SyncToServerRoutine(string key, MonthMissionKey missionKey)
    {
        ServerData.eventMissionTable.SyncToServerEach(key);

        SyncRoutines2[missionKey] = null;
        yield return null;
    }
    private static IEnumerator SyncToServerRoutine(string key, MonthMission2Key missionKey)
    {
        ServerData.eventMissionTable.SyncToServerEach(key);

        SyncRoutines3[missionKey] = null;
        yield return null;
    }

    public static void SyncAllMissions()
    {
 
        var tableData = TableManager.Instance.EventMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            ServerData.eventMissionTable.SyncToServerEach(tableData[i].Stringid);
        }
    }
}
