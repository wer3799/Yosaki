using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UiHellRelicBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bestScoreText;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private GameObject TransBeforeObject;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value;
        bestScoreText.SetText($"최고점수:{score}");

        abilDescription.SetText($"최고점수 {PlayerStats.HellRelicAbilDivide}당 지옥베기 피해량 {PlayerStats.HellRelicAbilValue * 100f}% 증가\n<color=red>{PlayerStats.GetHellRelicAbilValue() * 100f}%증가됨</color>");
        
        TransBeforeObject.SetActive(score<GameBalance.hellRelicGraduateValue);
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.HellRelic);
        }, () => { });
    }
    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value< GameBalance.hellRelicGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage(
                $"최고점수 {Utils.ConvertBigNum(GameBalance.hellRelicGraduateScore)} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"각성 후 최고 점수가 {GameBalance.hellRelicGraduateValue}점으로 고정됩니다.\n" +
                "각성 하시겠습니까??", () =>
                {
                    ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value =
                        GameBalance.hellRelicGraduateValue;
                    ServerData.userInfoTable.UpData(UserInfoTable.hellRelicKillCount, false);
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);

                    Initialize();
                }, null);
        }
    }
}
