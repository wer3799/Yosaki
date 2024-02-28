using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum YorinMissionKey
{
    YMission1_1,//BuyStartSet=0,
    YMission1_2,//PlayTower0_10,
    YMission1_3,//AwakeBlack,
    YMission1_4,//PlayBandit,
    YMission1_5,//PlayDokebi,
    YMission1_6,//AchieveStage50,
    YMission1_7,//전설4등급무
    YMission1_8,//전설4등급노
    
    
    YMission2_1,//PlaySmith,
    YMission2_2,//CraftHyunMuNorigae,
    YMission2_3,//AwakeMarble,
    YMission2_4,//PlayRelicDungeon,
    YMission2_5,//PlayTaeguk=10,
    
    YMission2_6,//GetOffReward,
    YMission3_1,//PlaySon,
    YMission3_2,//GetHim,//힘의 증표
    YMission3_3,//CraftYomulWeapon,
    YMission3_4,//GetHyunMuPetEquipment,
    YMission3_5,//PlayGangChul,
    YMission3_6,//PlayBackGui,
    YMission4_1,//GetGangHam,//강함의 증표
    YMission4_2,//AchieveRowRelic100,
    YMission4_3,//AchievePetEquip10=20,
    
    YMission4_4,//PlaySusano,
    YMission4_5,//PlayFoxMask,
    YMission5_1,//GetPokju,//폭주석
    YMission5_2,//CraftYachaSword,
    YMission5_3,//PlayHyunMu,
    YMission5_4,//ChangeCostumeAbil,
    YMission5_5,//PlayRoullet,
    YMission5_6,//PlayRoullet,도술1레벨
    YMission6_1,//Get12,//십이지석
    YMission6_2,//PlayHell,//지옥불꽃
    YMission6_3,//PlaySword=30,//검산
    
    YMission6_4, //PlayYeoRae,
    YMission6_5, //GetLoot,
    YMission7_1, //CraftPilMyulSword,
    YMission7_2, //JoinGuild,
    YMission7_3, //FeedGyungong,
    YMission7_4, //PlayTower1_30,
    YMission7_5, //PlayCave
    
    YMission1_9, //뽑기레벨
    YMission2_7, //기술흡수
    YMission1_10, //도움말확인


}
public static class YorinMissionManager
{
    private static Dictionary<YorinMissionKey, Coroutine> SyncRoutines = new Dictionary<YorinMissionKey, Coroutine>();

    private static WaitForSeconds syncDelay = new WaitForSeconds(3.0f);

    private static WaitForSeconds syncDelay_slow = new WaitForSeconds(300.0f);

    public static void UpdateYorinMissionClear(YorinMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.YorinMissionDatas[(int)missionKey].Stringid;
        
        
        if (ServerData.yorinMissionServerTable.TableDatas[key].clearCount.Value >= 1) return;

        //로컬 데이터 갱신
        ServerData.yorinMissionServerTable.UpdateMissionClearCount(key, count);



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
    public static void UpdateYorinMissionReward(YorinMissionKey missionKey, int count)
    {
        string key = TableManager.Instance.YorinMissionDatas[(int)missionKey].Stringid;

        //로컬 데이터 갱신
        ServerData.yorinMissionServerTable.UpdateMissionRewardCount(key, count);



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



    private static IEnumerator SyncToServerRoutine(string key, YorinMissionKey missionKey)
    {
        ServerData.yorinMissionServerTable.SyncToServerEach(key);

        SyncRoutines[missionKey] = null;
        yield return null;
    }

    public static void SyncAllMissions()
    {
 
        var tableData = TableManager.Instance.YorinMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            ServerData.yorinMissionServerTable.SyncToServerEach(tableData[i].Stringid);
        }
    }
}
