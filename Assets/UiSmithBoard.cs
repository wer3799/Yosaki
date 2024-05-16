using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiSmithBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreDescription;
    public Button enterButton;
    public Button registerButton;

    public TextMeshProUGUI getButtonDesc;
    public TextMeshProUGUI levelUpDesc;
    public TextMeshProUGUI expDescription;
    public TextMeshProUGUI abilDescription;
    public TextMeshProUGUI smithTreeAddLevel;

    [Header("Trans")]
    [SerializeField] private GameObject transObject;
    [SerializeField] private GameObject transAfterObject;
    
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.smithClear].AsObservable().Subscribe(e =>
        {

            scoreDescription.SetText($"{e}");
            
            bool isTrans = GameBalance.smithGraduateAfterScore<= e;
            
            transObject.SetActive(!isTrans);
            transAfterObject.SetActive(isTrans);
            
        }).AddTo(this);


        ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "오늘획득");
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).AsObservable().Subscribe(e =>
        {
            levelUpDesc.SetText($"+{e}");
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.smithExp).AsObservable().Subscribe(e =>
        {

            expDescription.SetText($"LV:{e}");

            UpdateAbilDescription();

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].AsObservable().Subscribe(e =>
        {

            int addAmount = (int)(e * GameBalance.smithTreeAddValue);

            smithTreeAddLevel.SetText($"장작 효과 : +{addAmount}");

        }).AddTo(this);
        
    }

    private void UpdateAbilDescription()
    {
        string abil = string.Empty;

        abil += $"<color=#ff00ffff>{CommonString.GetStatusName(StatusType.growthStoneUp)} {PlayerStats.GetSmithValue(StatusType.growthStoneUp)}개\n";
        abil += $"<color=red>{CommonString.GetStatusName(StatusType.WeaponHasUp)} {PlayerStats.GetSmithValue(StatusType.WeaponHasUp)}배\n";
        abil += $"<color=blue>{CommonString.GetStatusName(StatusType.NorigaeHasUp)} {PlayerStats.GetSmithValue(StatusType.NorigaeHasUp)}배\n";
        abil += $"<color=yellow>{CommonString.GetStatusName(StatusType.PetEquipHasUp)} {PlayerStats.GetSmithValue(StatusType.PetEquipHasUp)}배\n";
        abil += $"<color=#00ffffff>{CommonString.GetStatusName(StatusType.PetEquipProbUp)} {PlayerStats.GetSmithValue(StatusType.PetEquipProbUp)}%";

        abilDescription.SetText(abil);
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {

            GameManager.Instance.LoadContents(ContentsType.Smith);
            enterButton.interactable = false;
        }, () => { });
    }
    //

    public void OnClickEnterTreeButton() 
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.SmithTree);
            enterButton.interactable = false;
        }, () => { });
    }

    public void OnClickGetFireButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SmithFire)}은 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
         {
             if (ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value == 1)
             {
                 PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SmithFire)}은 하루에 한번만 획득 가능합니다!");
                 return;
             }

             ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value = 1;
             ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += score;

             List<TransactionValue> transactions = new List<TransactionValue>();

             Param userInfoParam = new Param();
             userInfoParam.Add(UserInfoTable.getSmith, ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value);

             Param goodsParam = new Param();
             goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);

             transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
             transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

             ServerData.SendTransaction(transactions, successCallBack: () =>
               {
                   LogManager.Instance.SendLogType("Smith", "_", score.ToString());
                   PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SmithFire)} {score}개 획득!", null);
               });
         }, null);
    }

    public void OnClickLevelUpButton()
    {
        int goodsAmount = (int)ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value;

        if (goodsAmount <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SmithFire)}이 없습니다.");
            return;
        }

        LogManager.Instance.SendLogType("Smith", "REQ", goodsAmount.ToString());

        ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value = 0;
        ServerData.userInfoTable.TableDatas[UserInfoTable.smithExp].Value += goodsAmount;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.smithExp, ServerData.userInfoTable.TableDatas[UserInfoTable.smithExp].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              LogManager.Instance.SendLogType("Smith", "OK", goodsAmount.ToString());
          });
    }
    public void OnClickTransButton()
    {
        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.smithClear].Value;
        if (score < GameBalance.smithGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {GameBalance.smithGraduateScore} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"도깨비 대장간을 각성하려면 {Utils.ConvertNum(GameBalance.smithGraduateScore)} 이상 이어야 합니다.\n" +
                $"각성 시 점수가 {Utils.ConvertNum(GameBalance.smithGraduateAfterScore)}점으로 고정됩니다.\n" +
                $"각성하시겠습니까?", () =>
                {

                    ServerData.userInfoTable.TableDatas[UserInfoTable.smithClear].Value = GameBalance.smithGraduateAfterScore;

                    List<TransactionValue> transactions = new List<TransactionValue>();
            
                    Param userinfoParam = new Param();
                    userinfoParam.Add(UserInfoTable.smithClear,ServerData.userInfoTable.GetTableData(UserInfoTable.smithClear).Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));
            
                    ServerData.SendTransactionV2(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
                    });
                }, null);
        }
    }
#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.smithExp].Value += 5000;
        }
    }
#endif
}
