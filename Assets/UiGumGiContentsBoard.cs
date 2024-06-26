using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiGumGiContentsBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreDescription;
    public TextMeshProUGUI scoreDescription_soul;
    public Button enterButton;
    public Button registerButton;

    public TextMeshProUGUI getButtonDesc;
    public TextMeshProUGUI expDescription;
    public TextMeshProUGUI abilDescription;
    
    
    [SerializeField] private GameObject transBeforeObject;
    [SerializeField] private GameObject transAfterObject;
    
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].AsObservable().Subscribe(e =>
        {

            scoreDescription.SetText($"{e}");

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiSoulClear].AsObservable().Subscribe(e =>
        {

            scoreDescription_soul.SetText($"최고 점수 : {e}");

        }).AddTo(this);



        ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "획득함");
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).AsObservable().Subscribe(e =>
        {
            expDescription.SetText($"{e}");
        }).AddTo(this);

        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGumgi).AsObservable().Subscribe(e =>
        {
            transBeforeObject.SetActive(e < 1);
            //transAfterObject.SetActive(e >= 1);
        }).AddTo(this);
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.GumGi);
            enterButton.interactable = false;
        }, () => { });


    }

    public void OnClickEnterSoulButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.GumGiSoul);
            enterButton.interactable = false;
        }, () => { });
    }

    public void OnClickEnterSoulButton_Sansilryung()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.TwelveDungeon);
            GameManager.Instance.SetBossId(57);
            enterButton.interactable = false;
        }, () => { });
    }

    public void OnClickGetFireButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SP)}은 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}", () =>
        {
            if (ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SP)}은 하루에 한번만 획득 가능합니다!");
                return;
            }

            ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += score + Utils.GetDokebiTreasureAddValue();

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getGumGi, ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION4, 1);
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.ANMISSION7, 1);

            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSwordPartial, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSwordPartial, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                LogManager.Instance.SendLogType("GumGi", "_", score.ToString());
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SP)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
            });
        }, null);
    }
    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value < GameBalance.GumgiGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {GameBalance.GumgiGraduateScore} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"검의 산 각성시 최고점수가 {GameBalance.GumgiFixedScore}로 고정 됩니다. \n" +
                "각성 하시겠습니까??", () =>
                {
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateGumgi].Value = 1;
                    ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value = GameBalance.GumgiFixedScore;
                    
                    List<TransactionValue> transactions = new List<TransactionValue>();
                    
                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.gumGiClear, ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName,UserInfoTable.Indate,userInfoParam));
                    
                    Param userInfo2Param = new Param();
                    userInfo2Param.Add(UserInfoTable_2.graduateGumgi, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateGumgi].Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userInfo2Param));
                    
                    ServerData.SendTransaction(transactions,successCallBack: () =>
                    {
                        
                    });
                    
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
              
                }, null);
        }
    }
}
