using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiVisionTowerBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    
    private void Start()
    {
        Initialize();
        
        GainVisionSkills();
    }

    private void GainVisionSkills()
    {
        //첫번째 기술 획득
        if (PlayerStats.GetVisionTowerGrade() >=GameBalance.visionSkill6GainIdx )
        {
            if (ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill6).Value<1)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill6).Value = 1;
                ServerData.goodsTable.UpData(GoodsTable.VisionSkill6, false);    
                PopupManager.Instance.ShowConfirmPopup("알림","궁극기 획득!",null);
                LogManager.Instance.SendLogType("Funnel_Tutorial", "complete", $"Gain First VisionSkill");
            }
        }
        //두번째 기술 획득
        if (PlayerStats.GetVisionTowerGrade() >=GameBalance.visionSkill7GainIdx )
        {
            if (ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill7).Value<1)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill7).Value = 1;
                ServerData.goodsTable.UpData(GoodsTable.VisionSkill7, false);
                PopupManager.Instance.ShowConfirmPopup("알림","궁극기 획득!",null);
            }
        }
        
    }
    private void Initialize()
    {
        scoreText.SetText($"최고 등급 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.visionTowerScore].Value)}");
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.VisionTower);
        }, () => { });
    }
    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.visionTowerScore].Value < GameBalance.VisionTowerGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 등급 {GameBalance.VisionTowerGraduateScore} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"비전 전수 각성시 최고 등급이 {GameBalance.VisionTowerFixedScore}로 고정 됩니다. \n" +
                $"그리고 비전전수 효과가 {GameBalance.VisionTowerGraduatePlusValue}배 강화 됩니다.\n" +
                "각성 하시겠습니까??", () =>
                {
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.visionTowerScore].Value = GameBalance.VisionTowerFixedScore;
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateVisionTower].Value = 1;
                    
                    List<TransactionValue> transactions = new List<TransactionValue>();
                    
                    Param userinfo2Param = new Param();
                    userinfo2Param.Add(UserInfoTable_2.visionTowerScore, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.visionTowerScore].Value);
                    userinfo2Param.Add(UserInfoTable_2.graduateVisionTower, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateVisionTower].Value);

                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userinfo2Param));
                    
                    ServerData.SendTransaction(transactions,successCallBack: () =>
                    {
                        
                    });
                    
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
              
                }, null);
        }
    }
}
