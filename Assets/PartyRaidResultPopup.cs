using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using static NetworkManager;
using System;
using Photon.Pun;
using BackEnd;

public class PartyRaidResultPopup : SingletonMono<PartyRaidResultPopup>
{
    [SerializeField]
    private TextMeshProUGUI totalText;

    [SerializeField]
    private GameObject recordButton;

    [SerializeField]
    private GameObject leaveOnlyButton;

    [SerializeField]
    private GameObject leaveOnlyButton_Tower;

    [SerializeField]
    private GameObject rewardChartButton;

    private void UpdateButton()
    {
        rewardChartButton.SetActive(PartyRaidManager.Instance.NetworkManager.IsGuildBoss());

        if (PartyRaidManager.Instance.NetworkManager.IsNormalBoss() == true)
        {
            recordButton.SetActive(true);
            leaveOnlyButton.SetActive(false);
        }
        else if (PartyRaidManager.Instance.NetworkManager.IsGuildBoss())
        {
            recordButton.SetActive(PhotonNetwork.IsMasterClient);
            leaveOnlyButton.SetActive(!PhotonNetwork.IsMasterClient);
        }
        else if (PartyRaidManager.Instance.NetworkManager.IsPartyTowerBoss() || PartyRaidManager.Instance.NetworkManager.IsPartyTower2Boss())
        {
            recordButton.SetActive(false);
            leaveOnlyButton.SetActive(false);
        }

        if (OnlineTowerManager.Instance != null)
        {
            var onlineTowerManager = OnlineTowerManager.Instance.GetComponent<OnlineTowerManager>();

            if (onlineTowerManager != null && onlineTowerManager.allPlayerEnd)
            {
                leaveOnlyButton_Tower.SetActive(true);
            }
        }

        if (PartyRaidManager.Instance.NetworkManager.IsPartyTower2Boss()) 
        {
            leaveOnlyButton_Tower.SetActive(true);
        }
    }

    public void ExitButtonActive()
    {
        Debug.LogError("ExitButtonActive");
        leaveOnlyButton_Tower.SetActive(true);
    }

    private void OnDisable()
    {
        leaveOnlyButton_Tower.SetActive(false);
    }


    private void OnEnable()
    {
        UpdateScoreBoard();

        UpdateButton();
    }

    public void OnClickRegistScoreButton()
    {
        if (PartyRaidManager.Instance.NetworkManager.IsGuildBoss() == false)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"점수를 등록하고 나갈가요?\n<color=red>(주의)현재 결과화면에 기록된 점수의 합으로 등록 됩니다.\n총 : {Utils.ConvertBigNum(PartyRaidManager.Instance.NetworkManager.GetTotalScore())}점", () =>
            {
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.MISSION3, 1);
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.AMISSION3, 1);
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.TMISSION3, 1);
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.ANMISSION3, 1);
                RecordPartyRaidScore();
                LeaveRoom();

            }, () => { });
        }
        else
        {
            int totalScore = GetGuildPartyTotalScore();

            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{totalScore}점을 갱신하고 나갈가요?\n<color=red>(주의)현재 결과화면에 기록된 점수의 합을 기반으로 계산 됩니다.", () =>
            {
                RecordGuildRaidScore();
                LeaveRoom();
            }, () => { });
        }


    }

    private void RecordPartyRaidScore()
    {
        double totalScore = PartyRaidManager.Instance.NetworkManager.GetTotalScore();

        //로컬 점수 등록

        var twelveBossTable = TableManager.Instance.TwelveBossTable.dataArray[55];

        var serverData = ServerData.bossServerTable.TableDatas[twelveBossTable.Stringid];


        /////

        if ((string.IsNullOrEmpty(ServerData.etcServerTable.TableDatas[EtcServerTable.chunmaTopScore].Value)) ||
            double.Parse(ServerData.etcServerTable.TableDatas[EtcServerTable.chunmaTopScore].Value) < totalScore)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.chunmaTopScore].Value = totalScore.ToString();
            //랭킹등록
            //RankManager.Instance.UpdateChunmaTop(totalScore);
        }
        else
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        ServerData.etcServerTable.UpdateData(EtcServerTable.chunmaTopScore);
        

        if (string.IsNullOrEmpty(serverData.score.Value) == false)
        {
            if (totalScore < double.Parse(serverData.score.Value))
            {
                //return;
            }
            else
            {

                serverData.score.Value = totalScore.ToString();

                ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
            }
        }
        else
        {


            serverData.score.Value = totalScore.ToString();

            ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
        }
    }

    private int GetGuildPartyTotalScore()
    {
        double totalScore = PartyRaidManager.Instance.NetworkManager.GetTotalScore();

        int ret = -1;

        var tableData = TableManager.Instance.TwelveBossTable.dataArray[74];

        for (int i = 0; i < tableData.Rewardcut.Length; i++)
        {
            if (totalScore < tableData.Rewardcut[i])
            {
                ret = i;
                break;
            }
        }

        if (ret == -1)
        {
            ret = tableData.Rewardcut.Length;
        }

        return ret + 30;
    }

    private void RecordGuildRaidScore()
    {
        bool isRankUpdateTime = ServerData.userInfoTable.IsRankUpdateTime();

#if UNITY_EDITOR
        isRankUpdateTime = true;
#endif
        if (isRankUpdateTime == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오전4시 ~ 5시는에는 랭킹을 등록하실 수 없습니다.", null);
            return;
        }

        int totalScore = GetGuildPartyTotalScore();

        if (totalScore == 0)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "추가할 점수가 없습니다.", null);
            return;
        }

        recordButton.gameObject.SetActive(false);


        if (totalScore == 0)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "추가할 점수가 없습니다.", null);
            recordButton.gameObject.SetActive(true);
            return;
        }

        var guildInfoBro = Backend.Social.Guild.GetMyGuildGoodsV3();

        if (guildInfoBro.IsSuccess())
        {
            var returnValue = guildInfoBro.GetReturnValuetoJSON();

            int currentScore = int.Parse(returnValue["goods"]["totalGoods7Amount"]["N"].ToString());

            int interval = totalScore - currentScore;

            if (interval > 0)
            {
                var bro2 = Backend.URank.Guild.ContributeGuildGoods(RankManager.Rank_GangChul_Guild_Boss_Uuid, goodsType.goods7, interval);

                if (bro2.IsSuccess())
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "점수 추가 완료!", null);
                    recordButton.gameObject.SetActive(true);
                    return;
                }
                else
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"점수 추가에 실패했습니다\n점수 갱신 시간이 아닙니다.\n({bro2.GetStatusCode()})", null);
                    recordButton.gameObject.SetActive(true);
                    return;
                }
            }
            //낮은점수
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"최고점수를 갱신하지 못했습니다\n최고점수 {currentScore}점", null);
                recordButton.gameObject.SetActive(true);
                return;
            }

        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오류가 발생했습니다. 잠시후 다시 시도해 주세요.", null);
            recordButton.gameObject.SetActive(true);
            return;
        }
    }

    public void LeaveRoom()
    {

        //서버 연결 끊기
        PartyRaidManager.Instance.OnClickCloseButton();

        //로비로 이동하기
        GameManager.Instance.LoadNormalField();
    }

    public void UpdateScoreBoard()
    {
        PartyRaidManager.Instance.NetworkManager.UpdateResultScore();

        totalText.SetText($"최종 피해량 : { Utils.ConvertBigNum(PartyRaidManager.Instance.NetworkManager.GetTotalScore())}");
    }
}
