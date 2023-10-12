using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : SingletonMono<SaveManager>
{
    private WaitForSeconds updateDelay = new WaitForSeconds(1000.0f);

    private WaitForSeconds updateDelay_DailyMission = new WaitForSeconds(7000.0f);

    private WaitForSeconds versionCheckDelay = new WaitForSeconds(1200.0f);

    private WaitForSeconds getHotTimeGoodsdDelay = new WaitForSeconds(600.0f);

    //12시간
    private WaitForSeconds tockenRefreshDelay = new WaitForSeconds(43200f);


    public void StartAutoSave()
    {
        StartCoroutine(AutoSaveRoutine());
        StartCoroutine(AutoSaveRoutine_Mission());
        StartCoroutine(TockenRefreshRoutine());
        StartCoroutine(VersionCheckRoutine());
        StartCoroutine(HotTimeGoodsGetRoutine());
    }

    private IEnumerator HotTimeGoodsGetRoutine()
    {
        while (true)
        {
            yield return getHotTimeGoodsdDelay;

            if (ServerData.userInfoTable.IsHotTimeEvent())
            {
                GetHotTimeGoods();
            }
        }
    }

    private IEnumerator VersionCheckRoutine()
    {
        while (true)
        {
            yield return versionCheckDelay;

            CheckClientVersion();
        }
    }

    private void GetHotTimeGoods()
    {
        // if (Utils.HasHotTimeEventPass() == false)
        // {
        //     ServerData.goodsTable.GetTableData(GoodsTable.Event_HotTime).Value += 1;
        //     ServerData.goodsTable.GetTableData(GoodsTable.Event_HotTime_Saved).Value += 1;
        // }
        // else
        // {
        //     ServerData.goodsTable.GetTableData(GoodsTable.Event_HotTime).Value += 2;
        // }
        ServerData.goodsTable.GetTableData(GoodsTable.Event_HotTime).Value += 2;
    }

    private void CheckClientVersion()
    {
        SendQueue.Enqueue(Backend.Utils.GetLatestVersion, bro =>
        {
            if (bro.IsSuccess())
            {
                int clientVersion = int.Parse(Application.version);

                var jsonData = bro.GetReturnValuetoJSON();
                string serverVersion = jsonData["version"].ToString();

                //버전이 높거나 같음
                if (clientVersion >= int.Parse(serverVersion))
                {
                }
                else
                {
                    PopupManager.Instance.ShowVersionUpPopup(CommonString.Notice, "업데이트 버전이 있습니다. 스토어로 이동합니다.\n업데이트 버튼이 활성화 되지 않은 경우\n구글 플레이 스토어를 닫았다가 다시 열어 보세요!", () =>
                    {
                        SyncDatasForce();
#if UNITY_ANDROID
                        Application.OpenURL("https://play.google.com/store/apps/details?id=com.DragonGames.yoyo&hl=ko");
#endif

#if UNITY_IOS
                    Application.OpenURL("itms-apps://itunes.apple.com/app/id1587651736");
#endif
                    }, false);
                }
            }
        });
    }

    private IEnumerator TockenRefreshRoutine()
    {
        while (true)
        {
            yield return tockenRefreshDelay;
            BackEnd.Backend.BMember.LoginWithTheBackendToken(e =>
            {
                if (e.IsSuccess())
                {
                    Debug.Log("토큰 갱신 성공");
                }
                else
                {
                    Debug.Log("토큰 갱신 실패");
                }
            });
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            SyncDatasInQueue();
            yield return updateDelay;
            ;
        }
    }

    private IEnumerator AutoSaveRoutine_Mission()
    {
        while (true)
        {
            yield return updateDelay_DailyMission;
            SyncDailyMissions();
        }
    }

    private List<string> ignoreSyncGoodsList = new List<string>()
    {
        GoodsTable.DokebiKey,
        GoodsTable.WeaponUpgradeStone,
        GoodsTable.YomulExchangeStone,
        GoodsTable.TigerStone,
        GoodsTable.RabitStone,
        GoodsTable.DragonStone,
        GoodsTable.SnakeStone,
        GoodsTable.HorseStone,
        GoodsTable.SheepStone,
        GoodsTable.MonkeyStone,
        GoodsTable.CockStone,
        GoodsTable.DogStone,
        GoodsTable.PigStone,

        GoodsTable.Asura0,
        GoodsTable.Asura1,
        GoodsTable.Asura2,
        GoodsTable.Asura3,
        GoodsTable.Asura4,
        GoodsTable.Asura5,

        GoodsTable.Indra0,
        GoodsTable.Indra1,
        GoodsTable.Indra2,
        GoodsTable.IndraPower,
        GoodsTable.Aduk,

        GoodsTable.SinSkill0,
        GoodsTable.SinSkill1,
        GoodsTable.SinSkill2,
        GoodsTable.SinSkill3,

        GoodsTable.LeeMuGiStone,
        GoodsTable.ZangStone,


        GoodsTable.Hae_Norigae,
        GoodsTable.Hae_Pet,
        GoodsTable.NataSkill,
        GoodsTable.OrochiSkill,
        GoodsTable.GangrimSkill,

        GoodsTable.OrochiTooth0,
        GoodsTable.OrochiTooth1,

        GoodsTable.gumiho0,
        GoodsTable.gumiho1,
        GoodsTable.gumiho2,
        GoodsTable.gumiho3,
        GoodsTable.gumiho4,
        GoodsTable.gumiho5,
        GoodsTable.gumiho6,
        GoodsTable.gumiho7,
        GoodsTable.gumiho8,

        GoodsTable.h0,
        GoodsTable.h1,
        GoodsTable.h2,
        GoodsTable.h3,
        GoodsTable.h4,
        GoodsTable.h5,
        GoodsTable.h6,
        GoodsTable.h7,
        GoodsTable.h8,
        GoodsTable.h9,
        GoodsTable.Ym,
        GoodsTable.du,


        GoodsTable.d0,
        GoodsTable.d1,
        GoodsTable.d2,
        GoodsTable.d3,
        GoodsTable.d4,
        GoodsTable.d5,
        GoodsTable.d6,
        GoodsTable.d7,

        GoodsTable.Sun0,
        GoodsTable.Sun1,
        GoodsTable.Sun2,
        GoodsTable.Sun3,
        GoodsTable.Sun4,

        GoodsTable.Chun0,
        GoodsTable.Chun1,
        GoodsTable.Chun2,
        GoodsTable.Chun3,
        GoodsTable.Chun4,

        GoodsTable.DokebiSkill0,
        GoodsTable.DokebiSkill1,
        GoodsTable.DokebiSkill2,
        GoodsTable.DokebiSkill3,
        GoodsTable.DokebiSkill4,

        GoodsTable.FourSkill0,
        GoodsTable.FourSkill1,
        GoodsTable.FourSkill2,
        GoodsTable.FourSkill3,

        GoodsTable.FourSkill4,
        GoodsTable.FourSkill5,
        GoodsTable.FourSkill6,
        GoodsTable.FourSkill7,
        GoodsTable.FourSkill8,


        GoodsTable.VisionSkill0,
        GoodsTable.VisionSkill1,
        GoodsTable.VisionSkill2,
        GoodsTable.VisionSkill3,
        GoodsTable.VisionSkill4,
        GoodsTable.VisionSkill5,
        GoodsTable.VisionSkill6,
        GoodsTable.VisionSkill7,
        GoodsTable.VisionSkill8,
        GoodsTable.VisionSkill9,
        GoodsTable.VisionSkill10,
        GoodsTable.VisionSkill11,
        GoodsTable.VisionSkill12,
        GoodsTable.VisionSkill13,

        GoodsTable.c0,
        GoodsTable.c1,
        GoodsTable.c2,
        GoodsTable.c3,
        GoodsTable.c4,
        GoodsTable.c5,
        GoodsTable.c6,
        GoodsTable.FoxMaskPartial,
        GoodsTable.Event_Fall_Gold,
        GoodsTable.Event_NewYear,
        GoodsTable.Event_NewYear_All,
        GoodsTable.Mileage,
        GoodsTable.ClearTicket,

        GoodsTable.HellPowerUp,
        //GoodsTable.DokebiFire,
        GoodsTable.DokebiFireKey,
        GoodsTable.DokebiFireEnhance,
        GoodsTable.DokebiTreasure,
        GoodsTable.SusanoTreasure,
        GoodsTable.SahyungTreasure,
        GoodsTable.VisionTreasure,
        GoodsTable.DarkTreasure,
        GoodsTable.SinsunTreasure,
        GoodsTable.DragonScale,
        GoodsTable.SumiFire,
        GoodsTable.SumiFireKey,
        GoodsTable.NewGachaEnergy,
        GoodsTable.DokebiBundle,
        GoodsTable.SinsuRelic,
        GoodsTable.HyungsuRelic,
        GoodsTable.ChunguRelic,
        GoodsTable.FoxRelic,
        GoodsTable.FoxRelicClearTicket,
        GoodsTable.YoPowerGoods,
        GoodsTable.TransClearTicket,
        GoodsTable.EventDice,
        GoodsTable.Tresure,
        GoodsTable.SinsuMarble,
        GoodsTable.SuhoPetFeedClear,
        GoodsTable.SoulRingClear,
        GoodsTable.SuhoPetFeed,
        GoodsTable.GuildTowerClearTicket,
        GoodsTable.GuildTowerHorn,
        GoodsTable.DosulClear,
        GoodsTable.DosulGoods,
        GoodsTable.TransGoods,
    };


    //SendQueue에서 저장
    public void SyncDatasInQueue()
    {
        if (GrowthManager.Instance != null)
        {
            GrowthManager.Instance.SyncLevelUpDatas();
        }

        ServerData.goodsTable.SyncAllData(ignoreSyncGoodsList);

        ServerData.userInfoTable.AutoUpdateRoutine();
        ServerData.userInfoTable_2.AutoUpdateRoutine();

        //CollectionManager.Instance.SyncToServer();

        if (BuffManager.Instance != null)
        {
            BuffManager.Instance.UpdateBuffTime();
            ServerData.buffServerTable.SyncAllData();
        }
    }

    private void OnApplicationQuit()
    {
        SetNotification();
        SyncDatasForce();
        SyncDailyMissions();
    }

    public void SyncDailyMissions()
    {
        DailyMissionManager.SyncAllMissions();
    }

    public void SetNotification()
    {
        if (SettingData.ShowSleepPush.Value == 1)
        {
            GleyNotifications.SendNotification("휴식보상", "휴식 보상이 가득 찼어요!(24시간)", new System.TimeSpan(24, 0, 0));
        }
    }

    //동기로 저장
    public void SyncDatasForce()
    {
        if (BuffManager.Instance != null)
        {
            BuffManager.Instance.UpdateBuffTime();
            ServerData.buffServerTable.SyncAllDataForce();
        }

        ServerData.goodsTable.SyncAllDataForce();

        if (GrowthManager.Instance != null)
        {
            GrowthManager.Instance.SyncLevelUpDatas();
        }
    }

    //버프
    private WaitForSeconds buffDelay = new WaitForSeconds(30f);
    
    private Coroutine buffCoroutine = null;

    public void SyncBuffData()
    {
        if (buffCoroutine != null)
        {
            return;
        }

        buffCoroutine = StartCoroutine(BuffSyncRoutine());
    }

    private IEnumerator BuffSyncRoutine()
    {
        ServerData.buffServerTable.SyncAllData();

        yield return buffDelay;

        buffCoroutine = null;
    }
}