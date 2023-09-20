using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiChunFlowerBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI sonLevelText;

    [SerializeField]
    private TextMeshProUGUI sonAbilText1;

    public Button registerButton;

    public TextMeshProUGUI getButtonDesc;


    [SerializeField] private GameObject transGameObject;
    [SerializeField] private GameObject transDescObject;

    private void Start()
    {
        Initialize();
        Subscribe();
        SetFlowerReward();
    }

    //기능 보류
    private void SetFlowerReward()
    {
        //chunFlowerReward.Initialize(TableManager.Instance.TwelveBossTable.dataArray[65]);
    }

    private void OnEnable()
    {
        UpdateAbilText1((int)ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Cw).AsObservable().Subscribe(level => { UpdateAbilUi(); }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "오늘 획득함");
        }).AddTo(this);


        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).AsObservable().Subscribe(e =>
        {
            transGameObject.SetActive(e < 1);
            transDescObject.SetActive(e >= 1);
            UpdateAbilUi();
        }).AddTo(this);
    }

    private void UpdateAbilUi()
    {
        float level = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        sonLevelText.SetText($"LV : {level}");
        UpdateAbilText1((int)level);
    }

    private void UpdateAbilText1(int currentLevel)
    {
        var tableData = TableManager.Instance.chunAbilBase.dataArray;

        string abilDesc = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type == StatusType.AttackAddPer)
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetChunAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {PlayerStats.GetChunAbilHasEffect(type) * 100f}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        sonAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.flowerClear].Value)}");
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.ChunFlower); }, () => { });
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += 2000;
        }
    }
#endif

    public void OnClickGetFireButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Cw)}은 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.flowerClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}", () =>
        {
            if (ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Cw)}은 하루에 한번만 획득 가능합니다!");
                return;
            }

            ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += score + Utils.GetDokebiTreasureAddValue();

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getFlower, ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION6, 1);
            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearChunFlower, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearChunFlower, 1);
            }

            ServerData.SendTransaction(transactions, successCallBack: () => { PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Cw)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null); });
        }, null);
    }

    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.flowerClear].Value < GameBalance.flowerGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"처치 수 {GameBalance.flowerGraduateScore} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                "각성시 하늘 꽃밭에서 천계꽃 획득이 더이상 불가능 합니다.\n" +
                $"대신 천계꽃 보유효과가 강화 되고({PlayerStats.ChunTransAddValue * 100}%)\n" +
                $"스테이지 일반 요괴 처치시 천계꽃을 자동으로 획득 합니다.\n" +
                "각성 하시겠습니까??", () =>
                {
                    ServerData.userInfoTable.TableDatas[UserInfoTable.graduateChun].Value = 1;
                    ServerData.userInfoTable.UpData(UserInfoTable.graduateChun, false);
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
                }, null);
        }
    }
}