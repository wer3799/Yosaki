using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EventMissionType
{
    FIRST,
    SECOND,
    THIRD,
    AFIRST,
    BSECOND,
    NORMALMARBLE,
    FINISHMARBLE,

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

   SMISSION1,
   SMISSION2,
   SMISSION3,
   SMISSION4,
   SMISSION5,
   SMISSION6,
   SMISSION7,
   SMISSION8,
   SMISSION9,
    
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
    
    
    AMISSION1,//문파점수
    AMISSION2,//견공
    AMISSION3,//십만대산

    BMISSION1,//반딧
    BMISSION2,//깨비전
    BMISSION3,//빠른전투
    BMISSION4,//검의산
    BMISSION5,//천계꽃
    BMISSION6,//도깨비
    BMISSION7,//수미산
    BMISSION8,//영혼석(반지)
    
    NMARBLEMISSION1,
    NMARBLEMISSION2,
    NMARBLEMISSION3,
    NMARBLEMISSION4,
    NMARBLEMISSION5,
    NMARBLEMISSION6,
    NMARBLEMISSION7,
    NMARBLEMISSION8,
    FMARBLEMISSION1,
    FMARBLEMISSION2,
    FMARBLEMISSION3,
    FMARBLEMISSION4,
    FMARBLEMISSION5,
    FMARBLEMISSION6,
    FMARBLEMISSION7,
    FMARBLEMISSION8,
    FMARBLEMISSION9,
    FMARBLEMISSION10,
    FMARBLEMISSION11,
    FMARBLEMISSION12

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
