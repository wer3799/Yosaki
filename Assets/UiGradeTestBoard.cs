using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class UiGradeTestBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private Image costumeIcon;
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
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            costumeIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail((int)e);
        }).AddTo(this);
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gradeTestGraduate).AsObservable().Subscribe(e =>
        {
            transObject.SetActive(e<1);
            transAfterObject.SetActive(e>0);
            if (e < 1)
            {
                contentsDesc.SetText($"사냥꾼 시험 에서는 악귀 무적 효과가 발동 되지 않습니다.\n" +
                                     $"점수를 등록하면, 점수에 맞는 케릭터 테두리와 (채팅X)\n" +
                                     $"효과를 획득 하실 수 있습니다!");
            }
            else
            {
                contentsDesc.SetText($"각성효과로 강화됩니다.\n" +
                                     $"능력치{GameBalance.gradeTestGraduateValue}배 증가");
            }
        }).AddTo(this);
    }


    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.gradeScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        int grade = PlayerStats.GetGradeTestGrade();

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
            GameManager.Instance.LoadContents(GameManager.ContentsType.GradeTest);
        }, () => { });
    }
    
    public void OnClickTransButton()
    {
        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.gradeScore].Value * GameBalance.BossScoreConvertToOrigin;
        if (score < GameBalance.gradeTestGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {Utils.ConvertBigNumForRewardCell(GameBalance.gradeTestGraduateScore)} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"사냥꾼 시험을 각성하려면 {Utils.ConvertNum(GameBalance.gradeTestGraduateScore)} 이상 이어야 합니다.\n" +
                $"각성 시 능력치 효과가 {Utils.ConvertNum(GameBalance.gradeTestGraduateValue * 100, 2)}% 강화 됩니다.\n" +
                $"각성하시겠습니까?", () =>
                {
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.gradeTestGraduate].Value ++;

                    ServerData.bossScoreTable.TableDatas[BossScoreTable.gradeScore].Value = (GameBalance.gradeTestGraduateAfterScore * GameBalance.BossScoreSmallizeValue).ToString();
                    
                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.gradeScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.gradeScore].Value);

                    List<TransactionValue> transactions = new List<TransactionValue>();
            
                    Param userinfo2Param = new Param();
                    userinfo2Param.Add(UserInfoTable_2.gradeTestGraduate,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gradeTestGraduate).Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));
            
                    Param bossScoreParam = new Param();
                    bossScoreParam.Add(BossScoreTable.gradeScore,ServerData.bossScoreTable.GetTableData(BossScoreTable.gradeScore).Value);
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
