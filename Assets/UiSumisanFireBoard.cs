using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiSumisanFireBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI dokebiLevelText;

    [SerializeField]
    private TextMeshProUGUI dokebiAbilText1;


    public Button registerButton;


    public TextMeshProUGUI getButtonDesc;

    public TMP_InputField inputField;

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
        UpdateAbilText1((int)ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);

        if (inputField != null)
        {
            inputField.text = $"소탕 횟수 입력";
        }
    }
    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).AsObservable().Subscribe(level =>
        {
            dokebiLevelText.SetText($"LV : {level}");
            UpdateAbilText1((int)level);

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.getSumiFire].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "오늘 획득함");
        }).AddTo(this);
    }

    private void UpdateAbilText1(int currentLevel)
    {
        var tableData = TableManager.Instance.sumiAbilBase.dataArray;

        string abilDesc = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type == StatusType.AttackAddPer)
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetSumiFireAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {PlayerStats.GetSumiFireAbilHasEffect(type) * 100f}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        dokebiAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.sumiFireClear].Value)}");

    }

    public void OnClickDokebiEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.SumiFire);
        }, () => { });
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += 2000;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value += 1;
        }
    }
#endif

    public void OnClickGetDokebiFireButton()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SumiFire)}은 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.sumiFireClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}", () =>
        {
            if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SumiFire)}은 하루에 한번만 획득 가능합니다!");
                return;
            }

            ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += score + Utils.GetDokebiTreasureAddValue();

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getSumiFire, ServerData.userInfoTable.TableDatas[UserInfoTable.getSumiFire].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            
            
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION8, 1);

            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSumiFire, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSumiFire, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SumiFire)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
            });
        }, null);
    }

    public void OnClickGetAllDokebiFireButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SumiFireKey)}이 부족합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.sumiFireClear].Value + (int)Utils.GetDokebiTreasureAddValue();

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score * ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value}개 획득 합니까?\n<color=red>({score} * {ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value} 획득 가능)</color>", () =>
        {
            int clearCount = (int)ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value;
            ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += score * clearCount;
            ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value -= clearCount;

            List<TransactionValue> transactions = new List<TransactionValue>();


            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);
            goodsParam.Add(GoodsTable.SumiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SumiFire)} {score * clearCount}개 획득!", null);
            });
        }, null);
    }
    public void OnClickGetManyDokebiFireButton()
    {

        if (!string.IsNullOrEmpty(inputField.text))
        {
            if (int.TryParse(inputField.text, out int result))
            {
                if (result < 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                    return;
                }
                if (ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value < result)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SumiFireKey)}이 부족합니다!");
                    return;
                }

                int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.sumiFireClear].Value;

                if (score == 0)
                {
                    PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
                    return;
                }


                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score * result}개 획득 합니까?\n<color=red>({score} x {result} 획득 가능)</color>", () =>
                {
                    if (result < 1)
                    {
                        PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                        return;
                    }
                    if (ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value < result)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SumiFireKey)}이 부족합니다!");
                        return;
                    }


                    ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += score * result;
                    ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value -= result;

                    List<TransactionValue> transactions = new List<TransactionValue>();


                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);
                    goodsParam.Add(GoodsTable.SumiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value);

                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                    ServerData.SendTransaction(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SumiFire)} {score * result}개 획득!", null);
                    });
                }, null);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("소탕 횟수를 입력해주세요.");
            }
        }


    }
}
