﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiHellTowerResult : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resultText;

    [SerializeField]
    private DungeonRewardView dungeonRewardView;

    [SerializeField]
    private GameObject failObject;
    [SerializeField]
    private TextMeshProUGUI stageChangeText;
    [SerializeField]
    private TextMeshProUGUI clearText;
    [SerializeField]
    private GameObject stageChangeButton;
    [SerializeField]
    private GameObject successObject;

    public void Initialize(ContentsState state, List<RewardData> rewardDatas) 
    {
        resultText.SetText(GetTitleText(state));

        successObject.SetActive(state == ContentsState.Clear);
        failObject.SetActive(state != ContentsState.Clear);

        dungeonRewardView.gameObject.SetActive(rewardDatas != null);

        if (rewardDatas != null) 
        {
            dungeonRewardView.Initalize(rewardDatas);
        }

        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.GuildTower2:
                
                clearText.SetText($"{TableManager.Instance.GuildTowerTable2.dataArray[(int)(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value)-1].Description}\n클리어!");
                break;
        }
        
    }

    private string GetTitleText(ContentsState contentsState) 
    {
        switch (contentsState)
        {
            case ContentsState.Dead:
                stageChangeText.SetText("다시 도전");
                return "실패!";
            case ContentsState.TimerEnd:
                stageChangeText.SetText("다시 도전");
                return "시간초과!";
            case ContentsState.Clear:
                switch (GameManager.contentsType)
                {
                    case GameManager.ContentsType.DokebiTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value < (TableManager.Instance.towerTable3.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.DokebiTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.RoyalTombTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.RoyalTombFloorIdx).Value < (TableManager.Instance.royalTombTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.RoyalTombTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.SinsuTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx6).Value < (TableManager.Instance.sinsuTower.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.SinsuTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.SealSwordTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx9).Value < (TableManager.Instance.SealTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.SealSwordTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.FoxTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxTowerIdx).Value < (TableManager.Instance.FoxTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.FoxTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.DarkTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.DarkTowerIdx).Value < (TableManager.Instance.DarkTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.DarkTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.GuildTower when (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorGuildTower).Value < (TableManager.Instance.guildTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.GuildTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.SinsunTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.SansinTowerIdx).Value < (TableManager.Instance.sinsunTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.SinsunTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.TransTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.transTowerIdx).Value < (TableManager.Instance.TransTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.TransTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.DragonTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.DragonTowerIdx).Value < (TableManager.Instance.DragonTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.DragonTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.DragonPalaceTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.DragonPalaceTowerIdx).Value < (TableManager.Instance.DragonPlaceTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.DragonPalaceTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.MunhaTower2 when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaTower).Value < (TableManager.Instance.StudentTower.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.MunhaTower2:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.MurimTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.MurimTowerIdx).Value < (TableManager.Instance.MurimTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.MurimTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.GuildTower2 when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value < (TableManager.Instance.GuildTowerTable2.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.GuildTower2:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.YeonOkTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yeonokTowerIdx).Value < (TableManager.Instance.YeonokTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.YeonOkTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.ChunguTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.chunguTowerIdx).Value < (TableManager.Instance.TowerTable16.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.ChunguTower:
                        stageChangeButton.SetActive(false);
                        break;
                    case GameManager.ContentsType.ChunSangTower when (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.chunsangTowerIdx).Value < (TableManager.Instance.SkyTowerTable.dataArray.Length):
                        stageChangeText.SetText("다음 스테이지");
                        break;
                    case GameManager.ContentsType.ChunSangTower:
                        stageChangeButton.SetActive(false);
                        break;
                }
                
                return "클리어!!";
        }

        return "미등록";
    }
}