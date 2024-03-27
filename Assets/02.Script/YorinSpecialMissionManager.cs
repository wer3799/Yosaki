using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum YorinSpecialMissionKey
{
    YSMission1_1,  //영혼의 숲 피해 감소 능력치 MAX 달성
    YSMission1_2,  //외형 특수 능력치 무기강화 5레벨 달성
    YSMission1_3,  //필멸 천 무기 획득
    YSMission1_4,  //흑룡 노리개 제작
    YSMission2_1,  //필멸 극 무기 획득
    YSMission2_2,  //요괴 사냥 - 보상 모음 - 모두 받기 버튼 클릭 해보기
    YSMission2_3,  //필멸 패 무기 획득
    YSMission2_4,  //금화 각성 진행
    YSMission3_1,  //검의 영혼 각성
    YSMission3_2,  //혈자리 하단전 10단계 달성
    YSMission3_3,  //왕의 시련 - 염라대왕 각성
    YSMission3_4,  //노리개 수호령 각성



}
public static class YorinSpecialMissionManager
{
    private static Dictionary<YorinSpecialMissionKey, Coroutine> SyncRoutines = new Dictionary<YorinSpecialMissionKey, Coroutine>();

    private static WaitForSeconds syncDelay = new WaitForSeconds(3.0f);

    private static WaitForSeconds syncDelay_slow = new WaitForSeconds(300.0f);

    public static void UpdateMissionClear(YorinSpecialMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.YorinSpecialMissionDatas[(int)missionKey].Stringid;
        
        
        if (ServerData.yorinSpecialMissionServerTable.TableDatas[key].clearCount.Value >= TableManager.Instance.YorinSpecialMissionDatas[(int)missionKey].Maxclear) return;

        //로컬 데이터 갱신
        ServerData.yorinSpecialMissionServerTable.UpdateMissionClearCount(key, count);

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
    public static void UpdateMissionReward(YorinSpecialMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.YorinSpecialMissionDatas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.yorinSpecialMissionServerTable.UpdateMissionRewardCount(key, count);
        
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



    private static IEnumerator SyncToServerRoutine(string key, YorinSpecialMissionKey missionKey)
    {
        ServerData.yorinSpecialMissionServerTable.SyncToServerEach(key);

        SyncRoutines[missionKey] = null;
        yield return null;
    }

    public static void SyncAllMissions()
    {
 
        var tableData = TableManager.Instance.YorinSpecialMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            ServerData.yorinSpecialMissionServerTable.SyncToServerEach(tableData[i].Stringid);
        }
    }
}
