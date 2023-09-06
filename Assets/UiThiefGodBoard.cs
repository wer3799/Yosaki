using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiThiefGodBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;


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
            transBefore.SetActive(e < GameBalance.thiefGodGraduate);
            transAfter.SetActive(e >= GameBalance.thiefGodGraduate);
        }).AddTo(this);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.thiefGodScore].Value * GameBalance.BossScoreConvertToOrigin)}");
        transAfterText.SetText($"각성효과로 강화됩니다(전투 능력치 효과만 적용).\n도적 신 능력치 {GameBalance.thiefGodGraduateValue}배 증가");

    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.TestThief);
        }, () => { });
    }

    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.thiefGodScore].Value *
            GameBalance.BossScoreConvertToOrigin < GameBalance.thiefGodGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage(
                $"데미지 {Utils.ConvertBigNum(GameBalance.thiefGodGraduateScore)} 이상일때 각성 가능!");
        }
        else if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value <
                 GameBalance.thiefGodGraduate - 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"이전 각성을 완료해주세요!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"도적신 전투 능력치 효과가 강화됩니다.({GameBalance.thiefGodGraduateValue * 100}%)\n" +
                "각성 하시겠습니까??", () =>
                {

                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value =
                        GameBalance.thiefGodGraduate;
                    ServerData.userInfoTable_2.UpData(UserInfoTable_2.GodTrialGraduateIdx, false);
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);

                }, null);
        }
    }
}
