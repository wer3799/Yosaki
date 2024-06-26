using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class UiContentsExitButton : MonoBehaviour
{
    [SerializeField]
    private bool ShowWarningMessage = true;
    [SerializeField]
    private GameObject buttonRootObject;

    private void OnEnable()
    {
        if (buttonRootObject != null)
        {
            buttonRootObject.SetActive(NextStageCheck());
        }
    }
    private bool NextStageCheck()
    {
                switch (GameManager.contentsType)
                {
                    case GameManager.ContentsType.InfiniteTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value < (TableManager.Instance.TowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.InfiniteTower:
                        return false;
                    case GameManager.ContentsType.InfiniteTower2 when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value < (TableManager.Instance.TowerTable2.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.InfiniteTower2:
                        return false;
                    case GameManager.ContentsType.DokebiTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value < (TableManager.Instance.towerTable3.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.DokebiTower:
                        return false;
                    case GameManager.ContentsType.FoxMask when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxMask).Value <
                                                               (TableManager.Instance.FoxMask.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.FoxMask:
                        return false;
                    case GameManager.ContentsType.RoyalTombTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.RoyalTombFloorIdx).Value <
                                                                      (TableManager.Instance.royalTombTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.RoyalTombTower:
                        return false;
                    case GameManager.ContentsType.SinsuTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx6).Value <
                                                                  (TableManager.Instance.sinsuTower.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.SinsuTower:
                        return false;
                    case GameManager.ContentsType.GuildTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorGuildTower).Value <
                                                                  (TableManager.Instance.guildTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.GuildTower:
                        return false;
                    case GameManager.ContentsType.SumisanTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx4).Value <
                                                                  (TableManager.Instance.sumisanTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.SumisanTower:
                        return false;
                    
                    case GameManager.ContentsType.FoxTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxTowerIdx).Value <
                                                                (TableManager.Instance.FoxTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.FoxTower:
                        return false;
                    case GameManager.ContentsType.DarkTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.DarkTowerIdx).Value <
                                                                (TableManager.Instance.DarkTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.DarkTower:
                        return false;
                    
                    case GameManager.ContentsType.SealSwordTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx9).Value <
                                                                 (TableManager.Instance.SealTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.SealSwordTower:
                        return false;
                    
                    case GameManager.ContentsType.TaeguekTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taeguekTower).Value <
                                                                      (TableManager.Instance.taegeukTitle.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.TaeguekTower:
                        return false;
                    case GameManager.ContentsType.SinsunTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.SansinTowerIdx).Value <
                                                                      (TableManager.Instance.sinsunTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.SinsunTower:
                        return false;
                    case GameManager.ContentsType.TransTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.transTowerIdx).Value <
                                                                      (TableManager.Instance.TransTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.TransTower:
                        return false;
                    case GameManager.ContentsType.DragonTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.DragonTowerIdx).Value <
                                                                      (TableManager.Instance.DragonTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.DragonTower:
                        return false;
                    case GameManager.ContentsType.DragonPalaceTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.DragonPalaceTowerIdx).Value <
                                                                      (TableManager.Instance.DragonPlaceTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.DragonPalaceTower:
                        return false;
                    case GameManager.ContentsType.MunhaTower2 when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaTower).Value <
                                                                      (TableManager.Instance.StudentTower.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.MunhaTower2:
                        return false;
                    case GameManager.ContentsType.MurimTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.MurimTowerIdx).Value <
                                                                      (TableManager.Instance.MurimTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.MurimTower:
                        return false;
                    case GameManager.ContentsType.OffLine_Tower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value <
                                                                     (TableManager.Instance.towerTableMulti.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.OffLine_Tower:
                        return false;
                    case GameManager.ContentsType.GuildTower2 when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value <
                                                                   (TableManager.Instance.GuildTowerTable2.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.GuildTower2:
                        return false;
                    case GameManager.ContentsType.YeonOkTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yeonokTowerIdx).Value <
                                                                   (TableManager.Instance.YeonokTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.YeonOkTower:
                        return false;
                    case GameManager.ContentsType.ChunSangTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.chunsangTowerIdx).Value <
                                                                   (TableManager.Instance.SkyTowerTable.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.ChunSangTower:
                        return false;
                    case GameManager.ContentsType.ChunguTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.chunguTowerIdx).Value <
                                                                   (TableManager.Instance.TowerTable16.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.ChunguTower:
                        return false;
                    case GameManager.ContentsType.Dimension when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value <
                                                                   (TableManager.Instance.DimensionDungeon.dataArray.Length):
                        return true;
                    case GameManager.ContentsType.Dimension:
                        return false;
                    
                }
        switch (GameManager.contentsType)
        {
            //점수로 측정
            case GameManager.ContentsType.Yum:
            case GameManager.ContentsType.Ok:
            case GameManager.ContentsType.Do:
            case GameManager.ContentsType.Sumi:
            case GameManager.ContentsType.Thief:
            case GameManager.ContentsType.Dark:
            case GameManager.ContentsType.Sinsun:
            case GameManager.ContentsType.GradeTest:
            case GameManager.ContentsType.Sasinsu:
            case GameManager.ContentsType.SumisanTower:
            case GameManager.ContentsType.GyungRockTower:
            case GameManager.ContentsType.GyungRockTower2:
            case GameManager.ContentsType.GyungRockTower3:
            case GameManager.ContentsType.GyungRockTower4:
            case GameManager.ContentsType.GyungRockTower5:
            case GameManager.ContentsType.GyungRockTower6:
            case GameManager.ContentsType.GyungRockTower7:
            case GameManager.ContentsType.GyungRockTower8:
            case GameManager.ContentsType.TestSword:
            case GameManager.ContentsType.TestMonkey:
            case GameManager.ContentsType.TestHell:
            case GameManager.ContentsType.TestChun:
            case GameManager.ContentsType.TestDo:
            case GameManager.ContentsType.TestSumi:
            case GameManager.ContentsType.TestThief:
            case GameManager.ContentsType.TestDark:
            case GameManager.ContentsType.TestSin:
            case GameManager.ContentsType.RelicTest:
            case GameManager.ContentsType.MeditationTower:
            case GameManager.ContentsType.SpecialRequestBoss:
            case GameManager.ContentsType.Dimension:
                return true;
            case GameManager.ContentsType.TwelveDungeon:
            {
                var tableData = TableManager.Instance.TwelveBossTable.dataArray[GameManager.Instance.bossId];
                //악귀퇴치 등의 이뮨을 무시하는 컨텐츠라면.
                if (tableData.NOTIMMUNETYPE != NotImmuneType.Normal)
                {
                    return true;
                }

                break;
            }
        }

        switch (GameManager.contentsType)
        {
            //산신령 & 서재 & 지키미 & 보도 & 수미산 지키미  & 도적단지키미
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 57:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 72:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 82:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 83:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 92:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 96:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 118:
            //도깨비 보스 & 수미산 사천왕
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 85:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 86:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 87:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 88:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 93:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 94:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 95:
            case GameManager.ContentsType.TwelveDungeon when GameManager.Instance.bossId == 97:
                return true;
            default:
                return false;
        }
    }
    public void OnClickExitButton()
    {
        if (ShowWarningMessage == true)
        {
            if (GameManager.contentsType == GameManager.ContentsType.BattleContest)
            {
                PopupManager.Instance.ShowYesNoPopup("알림", "포기하고 나가시겠습니까?", () =>
                {
                    BuffOff();
                    GameManager.Instance.LoadNormalField();
                }, null);   
            }
            else
            {
                PopupManager.Instance.ShowYesNoPopup("알림", "포기하고 나가시겠습니까?", () =>
                {
                    BuffOff();
                    GameManager.Instance.LoadNormalField();
                }, null);   
            }
        }
        else
        {
            BuffOff();
            GameManager.Instance.LoadNormalField();
        }
    }
    public void OnClickNextStageButton()
    {


        //if (GameManager.contentsType == GameManager.ContentsType.InfiniteTower)
        //{
        //    if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value < (TableManager.Instance.TowerTable.dataArray.Length))
        //    {
        //        GameManager.Instance.LoadContents(GameManager.ContentsType.InfiniteTower);
        //    }
        //}
        //다음스테이지가 있는 타워인 경우
        
        if (GameManager.contentsType == GameManager.ContentsType.InfiniteTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value < (TableManager.Instance.TowerTable.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.InfiniteTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.InfiniteTower2)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value < (TableManager.Instance.TowerTable2.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.InfiniteTower2);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.DokebiTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value < (TableManager.Instance.towerTable3.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.DokebiTower);
            }  
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.FoxMask)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxMask).Value <
                (TableManager.Instance.FoxMask.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.FoxMask);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.TaeguekTower)
        {
            if ((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taeguekTower).Value <
                (TableManager.Instance.taegeukTitle.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.TaeguekTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.RoyalTombTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.RoyalTombFloorIdx).Value <
                (TableManager.Instance.royalTombTowerTable.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.RoyalTombTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.SinsuTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx6).Value <
                (TableManager.Instance.sinsuTower.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.SinsuTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.ChunguTower)
        {
            if ((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.chunguTowerIdx).Value <
                (TableManager.Instance.TowerTable16.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.ChunguTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.GuildTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorGuildTower).Value <
                (TableManager.Instance.guildTowerTable.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.GuildTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.FoxTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxTowerIdx).Value <
                (TableManager.Instance.FoxTowerTable.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.FoxTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.DarkTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.DarkTowerIdx).Value <
                (TableManager.Instance.DarkTowerTable.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.DarkTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.SealSwordTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx9).Value <
                (TableManager.Instance.SealTowerTable.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.SealSwordTower);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.GuildTower2)
        {
            if ((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value <
                (TableManager.Instance.GuildTowerTable2.dataArray.Length))
            {
                var score = GameManager.Instance.currentTowerScore;
                var unlockScore = TableManager.Instance.GuildTowerTable2
                    .dataArray[(int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value]
                    .Unlockscore;
                if (unlockScore > score)
                {
                    PopupManager.Instance.ShowAlarmMessage("문파 총점이 부족하여 입장할 수 없습니다.");
                    return;
                }
                
                GameManager.Instance.LoadContents(GameManager.contentsType);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("최종 단계 입니다.");
            }
        }
        else
        {
            var type = GameManager.contentsType;
            GameManager.Instance.LoadContents(type);
            if (buttonRootObject != null)
            {
                buttonRootObject.SetActive(NextStageCheck());
            }
        }



    }

    public void OnClickExitButton_ForPartyRaid()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "포기하고 나가시겠습니까?", () =>
        {
            BuffOff();

            PartyRaidManager.Instance.OnClickCloseButton();
            GameManager.Instance.LoadNormalField();
        }, null);
    }


    private void BuffOff()
    {
        UiSusanoBuff.isImmune.Value = false;
        UiDokebiBuff.isImmune.Value = false;
    }
    private void BattleContenstGetLoseReward()
    {
        var data = TableManager.Instance.BattleContestTable.dataArray[GameManager.Instance.bossId];
        
        List<RewardData> loseData = new List<RewardData>();

        for (int i = 0; i < data.Losevalue.Length; i++)
        {
            var lose = new RewardData((Item_Type)data.Rewardtype[i],data.Losevalue[i]);
            loseData.Add(lose);
        }
        Param goodsParam = new Param();

        for (int i = 0; i < loseData.Count; i++)
        {
            ServerData.goodsTable.GetTableData((Item_Type)loseData[i].itemType).Value += loseData[i].amount;
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(loseData[i].itemType),
                ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(loseData[i].itemType))
                    .Value);
        }

        List<TransactionValue> transactionList = new List<TransactionValue>();


        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            //  StartCoroutine(AutoPlayRoutine());
        });
    }
}
