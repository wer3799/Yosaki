using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using BackEnd;
using TMPro;
using UnityEngine.UIElements;

public class StageJumpingButton : MonoBehaviour
{
    private int GetStage()
    {
        //강철이스코어
        var gangChulScore = double.Parse(ServerData.bossServerTable.TableDatas["boss20"].score.Value);


        var tableDatas = TableManager.Instance.EnemyTable.dataArray;

        int stage = GameBalance.JumpStageAdjustValue;

        if (gangChulScore > (tableDatas[tableDatas.Length - 1].Hp * tableDatas[tableDatas.Length - 1].Bosshpratio) * (1 - PlayerStats.DecreaseBossHp()) * 2)
        {
            return tableDatas[tableDatas.Length - 1].Id;
        }

        for (int i = 0; i < tableDatas.Length; i += GameBalance.JumpPoint)
        {
            //스테이지 보스 체력의 2배
            var bossHp = (tableDatas[i].Hp * tableDatas[i].Bosshpratio) * (1 - PlayerStats.DecreaseBossHp()) * 2;
            //내가 더쎔
            if (bossHp < gangChulScore)
            {
                continue;
            }

            //내가더 약함
            stage = i;
            break;
        }

        return stage - GameBalance.JumpStageAdjustValue;
    }

    public void OnClickJumpButton()
    {
        if ((int)ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].Value < GameBalance.JumpStageStartValue - 2)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertStage(GameBalance.JumpStageStartValue)} 스테이지부터 사용 가능합니다.");
            return;
        }

        if (string.IsNullOrEmpty(ServerData.bossServerTable.TableDatas["boss20"].score.Value))
        {
            PopupManager.Instance.ShowAlarmMessage("메뉴 - 강철이전에서 점수를 등록해 주세요!");
            return;
        }

        var arriveStageNum = GetStage();

        //도달 스테이지가 너무 높지 않게 조정.
        arriveStageNum = Mathf.Min(arriveStageNum,
            TableManager.Instance.GetLastStageIdx() - GameBalance.JumpStageLimit);

        if (arriveStageNum <= GameManager.Instance.CurrentStageData.Id)
        {
            PopupManager.Instance.ShowAlarmMessage("권장 스테이지 입니다!");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"<size=75><color=yellow>권장 스테이지 : {Utils.ConvertStage(arriveStageNum + 1)}</color></size>\n바로 도전 할까요?\n<color=red>(권장 스테이지는 강철이전 점수 기준 입니다)</color>", () =>
        {
            GameManager.Instance.IsJumpBoss = true;
            PlayerSkillCaster.Instance.InitializeVisionSkill();
            PlayerSkillCaster.Instance.InitializeMunhaSkill();
            GameManager.Instance.MoveMapByIdx(arriveStageNum);
        }, null);
    }
}