using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiDarkGodBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;


    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.darkGodScore].Value * GameBalance.BossScoreConvertToOrigin)}");
    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.TestDark);
        }, () => { });
    }

}
