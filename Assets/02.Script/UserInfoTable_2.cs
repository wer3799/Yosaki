using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;

public class UserInfoTable_2
{
    public static string Indate;
    public const string tableName = "UserInfo2";
    
    public const string monkeyGodScore = "monkeyGodScore";
    public const string swordGodScore = "swordGodScore";
    public const string hellGodScore = "hellGodScore";
    public const string chunGodScore = "chunGodScore";
    public const string doGodScore = "dokebiGodScore";
    public const string sumiGodScore = "sumiGodScore";
    public const string thiefGodScore = "thiefGodScore";
    public const string darkGodScore = "darkGodScore";
    public const string sinsunGodScore = "sinsunGodScore";
    public const string relicTestScore = "relicTestScore";
    public const string GangChulReset = "GangChulReset";
    public const string stagePassFree = "stagePassFree";
    public const string stagePassAd = "stagePassAd";
    public const string foxFirePassKill = "pass0"; //요도 여우불 같은거씀
    public const string dosulPassKill = "pass1"; // 도술꽃
    public const string petPassKill = "pass2"; // 펫



    //짝수 월간훈련(Monthlypass)
    public const string evenMonthKillCount = "even0";
    //홓수 월간훈련(Monthlypass2)
    public const string oddMonthKillCount = "odd0";

    public const string eventMission1AttendCount = "cac0";
    public const string commonAttend2Count = "cac1";
    public const string yorinAttendRewarded = "yar";
    public const string eventAttendRewarded = "sar";
    public const string eventMission2AttendCount = "sac"; // eventmission2Attend
    public const string eventAttendCount = "eac"; //2024 신년출석이벤트 
    
    public const string killCountTotalSeason0 = "ks0"; //혹서기 훈련
    
    public const string SealSwordAwakeScore = "SSASS";
    public const string DosulAwakeScore = "DASS";
    public const string taeguekTower = "tgtw";
    public const string taeguekLock = "tgll";
    public const string SansinTowerIdx = "sst";
    public const string DragonTowerIdx = "dti";
    public const string DragonPalaceTowerIdx = "dpti";
    public const string MurimTowerIdx = "mti";
    public const string KingTrialGraduateIdx = "ktgi";
    public const string GodTrialGraduateIdx = "gtgi";
    public const string darkScore = "ds";
    public const string sinsunScore = "ss";
    public const string graduateGold = "gg";
    public const string gyungRockTower3 = "grt3";
    public const string gyungRockTower4 = "grt4";
    public const string gyungRockTower5 = "grt5";
    public const string gyungRockTower6 = "grt6";
    public const string gyungRockTower7 = "grt7";
    public const string gyungRockTower8 = "grt8";
    public const string graduateSeolEvent = "gse";
    public const string hyunsangTowerScore = "hts";
    
    public const string towerFloorAdjust = "ta";
    public const string dosulScore = "dss";
    public const string dosulLevel = "dll";

    public const string dosulRewardIdx = "dri";
    public const string dosulStart = "dst";
    
    public const string visionTowerScore = "vts";
    public const string getDragonBracelet = "gdb";
    
    public const string guimoonRelicStart = "grs";
    public const string meditationStart = "ms";
    public const string meditationStartTime = "mst";
    public const string meditationIndex = "msi"; // 명상 단계
    public const string meditationTowerRewardIndex = "msti"; //명상 타워 보상 단계
    public const string meditationTowerScore = "msts"; // 명상 타워 점수
    public const string yoPowerIdx = "ypi"; // 요력개방
    
    public const string petDispatchStartTime = "pdst";

    
    public const string usedGuimoonRelicTicket = "ugrt";

    public const string transTowerIdx = "tti";

    public const string transTowerStart = "tts";
    public const string eventMiniGameScore_TopRate = "emgstr";
    public const string eventMiniGameScore_Total = "emgst";
    
    public const string getRelicUpgrade = "gru";
    public const string fullMoonRefund = "fmr0";
    
    public const string danjeonScore = "djs";
    public const string closedScore = "cs";
    
    public const string exchage_Daesan1 = "exchage_Daesan1";
    public const string exchage_Daesan2 = "exchage_Daesan2";
    public const string exchage_Daesan3 = "exchage_Daesan3";
    public const string exchage_Daesan4 = "exchage_Daesan4";
    public const string exchage_Daesan5 = "exchage_Daesan5";
    public const string exchage_Daesan6 = "exchage_Daesan6";
    
    public const string exchage_Guild1 = "exchage_Guild1";
    public const string exchage_Guild2 = "exchage_Guild2";
    public const string exchage_Guild3 = "exchage_Guild3";
    public const string exchage_Guild4 = "exchage_Guild4";
    public const string exchage_Guild5 = "exchage_Guild5";
    
    
    public const string enhanceRelicIdx = "eri";
    
    public const string awakeVisionSkill = "avs";
    public const string awakeSealSword   = "ass";
    public const string awakeDosulSkill = "ads";
    
    public const string yosakiMarbleScore = "yms";
    public const string taegeukSimbeopIdx = "tsi";
    public const string dailyPetDispatchReceiveCount = "dpdrc";
    
    public const string sealSwordEvolutionIdx = "ssei";
    public const string sealSwordEvolutionExp = "ssee";

    public const string blackFoxScore = "bfx";
    public const string usedblackFoxClearNum = "ubfcn";
    public const string blackFoxRefund = "brf";

    public const string byeolhoLevelIdx = "bhli";
    public const string byeolhoTowerRewardIndex = "bhtri"; //명상 타워 보상 단계
    
    public const string battleContestGradeLevel = "bcgl"; //비무 승급
    public const string foxMaskGraduate = "fmg"; //요괴탈
    public const string susanoGraduate = "sg"; //악귀퇴치
    public const string gradeTestGraduate = "gtg"; //사냥꾼시험
    public const string relicTestGraduate = "rtg"; //요괴탈

    public const string sasinsuAwakeGrade = "sag"; //요괴탈
    public const string munhaLevel = "mhl"; //문하생 레벨

    public const string munhaDispatchStartTime = "mdst";
    public const string munhaTower = "mt";//문하타워2
    
    public const string eventPackageRewardIdx = "epri";//어린이날패키지
    
    public const string rankHonorRewardIdx = "rhri";//랭크명예보상

    public const string suhoUpgradeGraduateIdx = "sugi";//수호보주강화 졸업
    public const string graduateSumiFire = "gsf";//수미산졸업
    public const string graduateGumgi = "ggg";//검의산졸업
    public const string graduateVisionTower = "gvt";//검의산졸업
    public const string graduateBackGui = "gbg";//백귀야행졸업

    public const string studentSpotGrade = "ssg";//제자 혈자리
    public const string hyulTowerRewardIndex = "htri"; //명상 타워 보상 단계
    
    public const string specialRequestTotalRewardIdx = "srtri"; //누적보상
    public const string specialRequestSpecialRewardIdx = "srsri"; //특별보상
    public const string currentSeasonIdx = "csi"; //현재시즌
    
    public const string specialRequestExchangeKey_0 = "sre_0"; //특별의뢰보상교환키
    public const string specialRequestExchangeKey_1 = "sre_1"; //특별의뢰보상교환키
    public const string specialRequestExchangeKey_2 = "sre_2"; //특별의뢰보상교환키
    public const string specialRequestExchangeKey_3 = "sre_3"; //특별의뢰보상교환키
    public const string specialRequestExchangeKey_4 = "sre_4"; //특별의뢰보상교환키

    public const string guildTower2ClearIndex = "gtci"; //길드타워2 황금 전갈굴 단계
    
    public const string guildPetWeeklyRewardIndex = "gpwri"; //길드타워2 황금 전갈굴 단계
    
    public const string yeonokTowerIdx = "yoti";
    public const string chunguTowerIdx = "cgti";
    public const string haetalGradeIdx = "hgi";
    
    public const string dimensionSpecialRewardIdx= "dsri";
    public const string munhaRefund= "mrf";
    public const string dimensionGrade= "dg";
    public const string currentDimensionSeasonIdx= "cdsi";

    public bool isInitialize = false;
    private Dictionary<string, double> tableSchema = new Dictionary<string, double>()
    {
        { GangChulReset, 0f },
        { stagePassFree, -1f },
        { stagePassAd, -1f },
        { killCountTotalSeason0, 0f },
        { eventMission1AttendCount, 1f },
        { commonAttend2Count, 0f },
        { yorinAttendRewarded, 0f },
        { eventAttendRewarded, 0f },
        { eventMission2AttendCount,1f },
        { eventAttendCount, 1f },
        
        { monkeyGodScore, 0f },
        { swordGodScore, 0f },
        { hellGodScore, 0f },
        { chunGodScore, 0f },
        { doGodScore, 0f },
        { sumiGodScore, 0f },
        { thiefGodScore, 0f },
        { darkGodScore, 0f },
        { sinsunGodScore, 0f },
        { relicTestScore, 0f },
        { foxFirePassKill, 0f },
        { dosulPassKill, 0f },
        { petPassKill, 0f },
        { evenMonthKillCount, 0f },
        { oddMonthKillCount, 0f },
        { SealSwordAwakeScore, 0f },
        { DosulAwakeScore, 0f },
        { taeguekTower, 0f },
        { taeguekLock, 0f },
        { SansinTowerIdx, 0f },
        { DragonTowerIdx, 0f },
        { DragonPalaceTowerIdx, 0f },
        { MurimTowerIdx, 0f },
        { yeonokTowerIdx, 0f },
        { chunguTowerIdx, 0f },
        { KingTrialGraduateIdx, 0f },
        { GodTrialGraduateIdx, 0f },
        { darkScore, 0f },
        { sinsunScore, 0f },
        { graduateGold, 0f },
        { gyungRockTower3, 0f },
        { gyungRockTower4, 0f },
        { gyungRockTower5, 0f },
        { gyungRockTower6, 0f },
        { gyungRockTower7, 0f },
        { gyungRockTower8, 0f },
        { hyunsangTowerScore, 0f },
        { graduateSeolEvent, 0f },
        { towerFloorAdjust, 0f },
        { dosulScore, 0f },
        { dosulLevel, -1f },
        { dosulRewardIdx, -1f },
        { dosulStart, 0f },
        { visionTowerScore, 0f },
        { getDragonBracelet, 0f },
        { guimoonRelicStart, 0f },
        { meditationStart, 0f },
        { meditationStartTime, -1f },
        { meditationIndex, -1f },
        { meditationTowerRewardIndex, -1f },
        { hyulTowerRewardIndex, -1f },
        { specialRequestTotalRewardIdx, -1f },
        { specialRequestSpecialRewardIdx, -1f },
        { currentSeasonIdx, 0f },
        { meditationTowerScore, 0f },
        { usedGuimoonRelicTicket, 0f },
        { getRelicUpgrade, 0f },
        { petDispatchStartTime, -1f },
        { munhaDispatchStartTime, -1f },
        { yoPowerIdx, -1f },

        { transTowerIdx, 0f },
        { transTowerStart, 0f },
        { eventMiniGameScore_TopRate, 0f },
        { eventMiniGameScore_Total, 0f },
        { fullMoonRefund, 0f },
        
        { danjeonScore, 0f },
        { closedScore, 0f },
        
        { exchage_Daesan1, 0f },
        { exchage_Daesan2, 0f },
        { exchage_Daesan3, 0f },
        { exchage_Daesan4, 0f },
        { exchage_Daesan5, 0f },
        { exchage_Daesan6, 0f },
        
        { exchage_Guild1, 0f },
        { exchage_Guild2, 0f },
        { exchage_Guild3, 0f },
        { exchage_Guild4, 0f },
        { exchage_Guild5, 0f },
        
        { enhanceRelicIdx, -1f },
        { awakeVisionSkill, -1f },
        { awakeSealSword, -1f },
        { awakeDosulSkill, -1f },
        { yosakiMarbleScore, -1f },
        { taegeukSimbeopIdx, -1f },
        { dailyPetDispatchReceiveCount, 0f },
        { sealSwordEvolutionIdx, -1f },
        { sealSwordEvolutionExp, 0f },
        { blackFoxScore, 0f },
        { usedblackFoxClearNum, 0f },
        { blackFoxRefund, 0f },
        
        { byeolhoLevelIdx, -1f },
        { byeolhoTowerRewardIndex, -1f },
        { battleContestGradeLevel, -1f },
        { foxMaskGraduate, 0f },
        { susanoGraduate, 0f },
        { gradeTestGraduate, 0f },
        { relicTestGraduate, 0f },
        { sasinsuAwakeGrade, 0f },
        { munhaLevel, -1f },
        { munhaTower, 0f },
        { eventPackageRewardIdx, -1f },
        { rankHonorRewardIdx, -1f },
        { suhoUpgradeGraduateIdx, 0f },
        { graduateSumiFire, 0f },
        { graduateGumgi, 0f },
        { graduateVisionTower, 0f },
        { graduateBackGui, 0f },
        { studentSpotGrade, -1f },
        { specialRequestExchangeKey_0, 0f },
        { specialRequestExchangeKey_1, 0f },
        { specialRequestExchangeKey_2, 0f },
        { specialRequestExchangeKey_3, 0f },
        { specialRequestExchangeKey_4, 0f },
        
        { guildTower2ClearIndex, 0f },
        { guildPetWeeklyRewardIndex, -1f },
        { haetalGradeIdx, -1f },
        { dimensionSpecialRewardIdx, -1f },
        { munhaRefund, 0f },
        { dimensionGrade, 0f },
        { currentDimensionSeasonIdx, 0f },
    };

    private Dictionary<string, ReactiveProperty<double>> tableDatas = new Dictionary<string, ReactiveProperty<double>>();
    public Dictionary<string, ReactiveProperty<double>> TableDatas => tableDatas;

    public ReactiveProperty<double> GetTableData(string key)
    {
        return tableDatas[key];
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

            //맨처음 초기화(캐릭터생성)
            if (rows.Count <= 0)
            {
                
                Param defultValues = new Param();

                var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {                        
               
                    defultValues.Add(e.Current.Key, e.Current.Value);
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(e.Current.Value));
               
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
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(double.Parse(value)));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(e.Current.Value));

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
                        return; //
                    }
                }
            }

            isInitialize = true;
        });
    }
    public void AutoUpdateRoutine()
    {
        if (isInitialize)
        {
            UpdatekillCount();
        }
    }

    private void UpdatekillCount()
    {
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfo_2Param = new Param();
        if (ServerData.userInfoTable.IsMonthlyPass2() == false)
        {
            userInfo_2Param.Add(evenMonthKillCount, tableDatas[evenMonthKillCount].Value);
        }
        else
        {
            userInfo_2Param.Add(oddMonthKillCount, tableDatas[oddMonthKillCount].Value);
        }
        userInfo_2Param.Add(foxFirePassKill, tableDatas[foxFirePassKill].Value);
        userInfo_2Param.Add(dosulPassKill, tableDatas[dosulPassKill].Value);
        userInfo_2Param.Add(petPassKill, tableDatas[petPassKill].Value);
        
        if (ServerData.userInfoTable.IsEventPassPeriod())
        {
            userInfo_2Param.Add(killCountTotalSeason0, tableDatas[killCountTotalSeason0].Value);
        }


        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo_2Param));

        ServerData.SendTransaction(transactions);
    }
    static int totalKillCount = 0;
    static double updateRequireNum = 100;
    public void GetKillCountTotal()
    {
        totalKillCount += (int)GameManager.Instance.CurrentStageData.Marbleamount;

        if (totalKillCount < updateRequireNum)
        {
        }
        else
        {
            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                tableDatas[evenMonthKillCount].Value += updateRequireNum;
            }
            else
            {
                tableDatas[oddMonthKillCount].Value += updateRequireNum;
            }

            if (ServerData.userInfoTable.IsEventPassPeriod())
            {
                tableDatas[killCountTotalSeason0].Value += updateRequireNum;
            }
            totalKillCount = 0;

            tableDatas[foxFirePassKill].Value += updateRequireNum;
            tableDatas[dosulPassKill].Value += updateRequireNum;
            tableDatas[petPassKill].Value += updateRequireNum;
        }
    }
    public void UpData(string key, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }

        UpData(key, tableDatas[key].Value, LocalOnly);
    }

    public void UpData(string key, double data, bool LocalOnly, Action failCallBack = null)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }

        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            Param param = new Param();
            param.Add(key, tableDatas[key].Value);

            SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
            {
                if (e.IsSuccess() == false)
                {
                    failCallBack?.Invoke();
                    Debug.LogError($"UserInfoTable {key} up failed");
                    return;
                }
            });
        }
    }
    public void UpDataV2(string key, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }

        UpDataV2(key, tableDatas[key].Value, LocalOnly);
    }

    public void UpDataV2(string key, double data, bool LocalOnly, Action failCallBack = null, Action successCallBack = null)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }

        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            Param param = new Param();
            param.Add(key, tableDatas[key].Value);

            SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
            {
                #if UNITY_EDITOR
                    Debug.LogError($"테이블 : {tableName} / 키 : {key} / 수량 : {tableDatas[key].Value}");
#endif
                if (e.IsSuccess() == false)
                {
                    failCallBack?.Invoke();
                    Debug.LogError($"UserInfoTable {key} up failed");
                    return;
                }
                else
                {
                    successCallBack?.Invoke();
                }
            });
        }
    }
    
    public bool IsLastFloor4()
    {
        return tableDatas[chunguTowerIdx].Value >= TableManager.Instance.TowerTable16.dataArray.Length;
    }
}