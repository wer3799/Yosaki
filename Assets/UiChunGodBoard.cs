using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiChunGodBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI transAfterText;
    [SerializeField] private GameObject transBefore;
    [SerializeField] private GameObject transAfter;
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).AsObservable().Subscribe(e =>
        {
            transBefore.SetActive(e < GameBalance.chunGodGraduate);
            transAfter.SetActive(e >= GameBalance.chunGodGraduate);
        }).AddTo(this);
    }
    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.chunGodScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        int grade = PlayerStats.GetChunGodGrade();

        if (grade != -1)
        {
            gradeText.SetText($"{grade + 1}단계");
        }
        else
        {
            gradeText.SetText("없음");
        }

        transAfterText.SetText($"각성효과로 강화됩니다(전투 능력치 효과만 적용).\n천계 신 능력치 {GameBalance.chunGodGraduateValue}배 증가");
    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.TestChun);
        }, () => { });
    }

    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.chunGodScore].Value *
            GameBalance.BossScoreConvertToOrigin < GameBalance.chunGodGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage(
                $"데미지 {Utils.ConvertBigNum(GameBalance.chunGodGraduateScore)} 이상일때 각성 가능!");
        }
        else if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value <
                 GameBalance.chunGodGraduate - 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"이전 각성을 완료해주세요!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"천계신 전투 능력치 효과가 강화됩니다.({GameBalance.chunGodGraduateValue * 100}%)\n" +
                "각성 하시겠습니까??", () =>
                {

                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value =
                        GameBalance.chunGodGraduate;
                    ServerData.userInfoTable_2.UpData(UserInfoTable_2.GodTrialGraduateIdx, false);
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);

                }, null);
        }
    }
}
