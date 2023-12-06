using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class UiRelicTestBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField] private TextMeshProUGUI contentsDesc;
    [SerializeField] private GameObject transObject;
    [SerializeField] private GameObject transAfterObject;
    private void Start()
    {
        Initialize();

        Subscribe();
    }

    private void Subscribe() 
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.relicTestGraduate).AsObservable().Subscribe(e =>
        {
            transObject.SetActive(e<1);
            transAfterObject.SetActive(e>0);
            if (e < 1)
            {
                contentsDesc.SetText($"점수를 기록하고 영혼의숲 능력치를 강화하세요!");
            }
            else
            {
                contentsDesc.SetText($"각성효과로 강화됩니다.\n" +
                                     $"능력치{GameBalance.relicTestGraduateValue}배 증가");
            }
        }).AddTo(this);
    }


    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.relicTestScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        int grade = PlayerStats.GetRelicTestGrade();

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
            GameManager.Instance.LoadContents(GameManager.ContentsType.RelicTest);
        }, () => { });
    }
    
    public void OnClickTransButton()
    {
        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.relicTestScore].Value * GameBalance.BossScoreConvertToOrigin;
        if (score < GameBalance.relicTestGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {Utils.ConvertBigNumForRewardCell(GameBalance.relicTestGraduateScore)} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"영혼 사냥을 각성하려면 {Utils.ConvertNum(GameBalance.relicTestGraduateScore)} 이상 이어야 합니다.\n" +
                $"각성 시 능력치 효과가 {Utils.ConvertNum(GameBalance.relicTestGraduateValue * 100, 2)}% 강화 됩니다.\n" +
                $"각성하시겠습니까?", () =>
                {
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.relicTestGraduate].Value ++;
                    
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.relicTestScore].Value = (GameBalance.relicTestGraduateAfterScore * GameBalance.BossScoreSmallizeValue).ToString();
                    
                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.relicTestScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.relicTestScore].Value);

                    List<TransactionValue> transactions = new List<TransactionValue>();
            
                    Param userinfo2Param = new Param();
                    userinfo2Param.Add(UserInfoTable_2.relicTestGraduate,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.relicTestGraduate).Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));
            
                    Param bossScoreParam = new Param();
                    bossScoreParam.Add(BossScoreTable.relicTestScore,ServerData.bossScoreTable.GetTableData(BossScoreTable.relicTestScore).Value);
                    transactions.Add(TransactionValue.SetUpdate(BossScoreTable.tableName, BossScoreTable.Indate, bossScoreParam));

                    ServerData.SendTransactionV2(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
                        Initialize();
                    });
                }, null);
        }
    }
}
