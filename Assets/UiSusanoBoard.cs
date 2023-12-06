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
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateEvilSeed).AsObservable().Subscribe(e =>
        {
            transBefore.SetActive(e<1);
            transAfter.SetActive(e > 0);
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.susanoGraduate).AsObservable().Subscribe(e =>
        {
            transObject.SetActive(e<1);
            transAfterObject.SetActive(e>0);
            if (e < 1)
            {
                contentsDesc.SetText($"악귀에게 피해를 입혀 점수를 등록하세요!\n등록된 점수에 맞는 악귀의 효과가 플레이어에게 적용 됩니다!");
            }
            else
            {
                contentsDesc.SetText($"각성효과로 강화됩니다.(전투 능력치 효과만 적용)\n능력치{GameBalance.susanoGraduateValue}배 증가");
            }
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
    public void OnClickSusanoTransButton()
    {
        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.susanoScore].Value * GameBalance.BossScoreConvertToOrigin;
        if (score < GameBalance.susanoGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {Utils.ConvertBigNumForRewardCell(GameBalance.susanoGraduateScore)} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"악귀 퇴치를 각성하려면 {Utils.ConvertNum(GameBalance.susanoGraduateScore)} 이상 이어야 합니다.\n" +
                $"각성 시 능력치 효과가 {Utils.ConvertNum(GameBalance.susanoGraduateValue * 100, 2)}% 강화 됩니다.\n" +
                $"각성하시겠습니까?", () =>
                {
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.susanoGraduate].Value ++;

                    ServerData.bossScoreTable.TableDatas[BossScoreTable.susanoScore].Value = (GameBalance.susanoGraduateAfterScore * GameBalance.BossScoreSmallizeValue).ToString();
                    
                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.susanoScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.susanoScore].Value);

                    List<TransactionValue> transactions = new List<TransactionValue>();
            
                    Param userinfo2Param = new Param();
                    userinfo2Param.Add(UserInfoTable_2.susanoGraduate,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.susanoGraduate).Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));
            
                    Param bossScoreParam = new Param();
                    bossScoreParam.Add(BossScoreTable.susanoScore,ServerData.bossScoreTable.GetTableData(BossScoreTable.susanoScore).Value);
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
