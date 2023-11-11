using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiSusanoBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI gradeText;
    [SerializeField]
    private TextMeshProUGUI transAfterDesc;

    [SerializeField] private GameObject transBefore;
    [SerializeField] private GameObject transAfter;
    
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateEvilSeed).AsObservable().Subscribe(e =>
        {
            transBefore.SetActive(e<1);
            transAfter.SetActive(e > 0);
        }).AddTo(this);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.susanoScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        int grade = PlayerStats.GetSusanoGrade();

        if (grade != -1)
        {
            gradeText.SetText($"{grade + 1}단계");
            
        }
        else
        {
            gradeText.SetText("없음");
        }

        transAfterDesc.SetText($"각성 효과로 강화됩니다.\n악의 씨앗 능력치 {(GameBalance.EvilSeedGraduatePlusValue - 1) * 100}% 증가");
    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Susano);
        }, () => { });
    }
    public static string bossKey = "b84";
    public void OnClickTransButton()
    {
        
        if (double.Parse(ServerData.bossServerTable.TableDatas[bossKey].score.Value) < GameBalance.EvilSeedGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {Utils.ConvertBigNumForRewardCell(GameBalance.EvilSeedGraduateScore)} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"악의씨앗을 각성하려면 점수가 {Utils.ConvertBigNumForRewardCell(GameBalance.EvilSeedGraduateScore)}이상 이어야 합니다. \n" +
                $"각성시 악의씨앗 효과가 {(GameBalance.EvilSeedGraduatePlusValue-1)*100f}% 강화 됩니다.\n" +
                "각성 하시겠습니까??", () =>
                {
                    ServerData.userInfoTable.TableDatas[UserInfoTable.graduateEvilSeed].Value = 1;
                    
                    List<TransactionValue> transactions = new List<TransactionValue>();
                    
                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.graduateEvilSeed, ServerData.userInfoTable.TableDatas[UserInfoTable.graduateEvilSeed].Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName,UserInfoTable.Indate,userInfoParam));
                    
                    ServerData.SendTransaction(transactions,successCallBack: () =>
                    {
                        Initialize();
                    });
                    
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
              
                }, null);
        }
    }

}
