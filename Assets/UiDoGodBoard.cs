using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiDoGodBoard : MonoBehaviour
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
            transBefore.SetActive(e < GameBalance.doGodGraduate);
            transAfter.SetActive(e >= GameBalance.doGodGraduate);
        }).AddTo(this);
    }
    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.doGodScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        int grade = PlayerStats.GetDoGodGrade();

        if (grade != -1)
        {
            gradeText.SetText($"{grade + 1}단계");
        }
        else
        {
            gradeText.SetText("없음");
        }

        transAfterText.SetText($"각성효과로 강화됩니다(전투 능력치 효과만 적용).\n도깨비 신 능력치 {GameBalance.doGodGraduateValue}배 증가");
    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.TestDo);
        }, () => { });
    }
    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.doGodScore].Value *
            GameBalance.BossScoreConvertToOrigin < GameBalance.doGodGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage(
                $"데미지 {Utils.ConvertBigNum(GameBalance.doGodGraduateScore)} 이상일때 각성 가능!");
        }
        else if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value <
                 GameBalance.doGodGraduate - 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"이전 각성을 완료해주세요!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"도깨비신 전투 능력치 효과가 강화됩니다.({GameBalance.doGodGraduateValue * 100}%)\n" +
                "각성 하시겠습니까??", () =>
                {

                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value =
                        GameBalance.doGodGraduate;
                    ServerData.userInfoTable_2.UpData(UserInfoTable_2.GodTrialGraduateIdx, false);
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);

                }, null);
        }
    }
}
