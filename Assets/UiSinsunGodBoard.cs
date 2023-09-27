using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiSinsunGodBoard : MonoBehaviour
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
            transBefore.SetActive(e < GameBalance.sinsunGodGraduate);
            transAfter.SetActive(e >= GameBalance.sinsunGodGraduate);
        }).AddTo(this);
    }
    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.sinsunGodScore].Value * GameBalance.BossScoreConvertToOrigin)}");
        transAfterText.SetText($"각성효과로 강화됩니다(전투 능력치 효과만 적용).\n신선 신 능력치 {GameBalance.sinsunGodGraduateValue}배 증가");

    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.TestSin);
        }, () => { });
    }

    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.sinsunGodScore].Value *
            GameBalance.BossScoreConvertToOrigin < GameBalance.sinsunGodGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage(
                $"데미지 {Utils.ConvertBigNum(GameBalance.sinsunGodGraduateScore)} 이상일때 각성 가능!");
        }
        else if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value <
                 GameBalance.sinsunGodGraduate - 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"이전 각성을 완료해주세요!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"신선 신 전투 능력치 효과가 강화됩니다.({GameBalance.sinsunGodGraduateValue * 100}%)\n" +
                "각성 하시겠습니까??", () =>
                {

                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GodTrialGraduateIdx].Value =
                        GameBalance.sinsunGodGraduate;
                    ServerData.userInfoTable_2.UpData(UserInfoTable_2.GodTrialGraduateIdx, false);
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);

                }, null);
        }
    }
}
