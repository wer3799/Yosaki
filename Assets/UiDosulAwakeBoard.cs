using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;
public class UiDosulAwakeBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI gradeText;
 
     private void Start()
    {
        Initialize();
    }

     
    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.DosulAwakeScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        int grade = PlayerStats.GetDosulAwakeGrade();

        if (grade != -1)
        {
            gradeText.SetText($"{grade + 1}단계");
            
        }
        else
        {
            gradeText.SetText("없음");
        }
    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.DosulAwake);
        }, () => { });
    }
  
}
