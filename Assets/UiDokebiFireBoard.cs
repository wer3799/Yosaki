using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiDokebiFireBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI dokebiLevelText;

    [SerializeField]
    private TextMeshProUGUI dokebiAbilText1;

    [SerializeField]
    private TextMeshProUGUI graduateDescription;


    public Button registerButton;
    
    
    [SerializeField] private GameObject transBeforeObject;
    [SerializeField] private GameObject transAfterObject;
    

    public TextMeshProUGUI getButtonDesc;

    public TMP_InputField inputField;

    private void Start()
    {
        Initialize();
        Subscribe();

        graduateDescription.SetText($"각성 효과로 효과 {GameBalance.dokebiGraduatePlusValue*100f}% 증가!");

    }


    private void OnEnable()
    {
        UpdateAbilText1((int)ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);

        if (inputField!=null)
        {
            inputField.text = $"소탕 횟수 입력";
        }
    }
    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).AsObservable().Subscribe(level =>
        {
            dokebiLevelText.SetText($"LV : {level}");
            UpdateAbilText1((int)level);

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "오늘 획득함");
        }).AddTo(this);
        
        
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).AsObservable().Subscribe(e =>
        {
            transBeforeObject.SetActive(e < 1);
            transAfterObject.SetActive(e >= 1);
        }).AddTo(this);
    }

    private void UpdateAbilText1(int currentLevel)
    {
        var tableData = TableManager.Instance.dokebiAbilBase.dataArray;

        string abilDesc = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type == StatusType.AttackAddPer)
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetDokebiFireAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {PlayerStats.GetDokebiFireAbilHasEffect(type) * 100f}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        dokebiAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value)}");

    }

    public void OnClickDokebiEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.DokebiFire);
        }, () => { });
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += 2000;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value += 1;
        }
    }
#endif

    public void OnClickGetDokebiFireButton()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFire)}은 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}", () =>
        {
            if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFire)}은 하루에 한번만 획득 가능합니다!");
                return;
            }

            ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += score+Utils.GetDokebiTreasureAddValue();

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getDokebiFire, ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION7, 1);

            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearDokebiFire, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearDokebiFire, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.DokebiFire)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
            });
        }, null);
    }

    public void OnClickGetAllDokebiFireButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFireKey)}이 부족합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value + (int)Utils.GetDokebiTreasureAddValue();

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score * ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value}개 획득 합니까?\n<color=red>({score} * {ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value} 획득 가능)</color>", () =>
        {
            int clearCount = (int)ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value;
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += score * clearCount;
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value -= clearCount;

            List<TransactionValue> transactions = new List<TransactionValue>();


            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
            goodsParam.Add(GoodsTable.DokebiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value);
                        
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.DokebiFire)} {score * clearCount}개 획득!", null);
            });
        }, null);
    }
    public void OnClickGetManyDokebiFireButton()
    {

        if (!string.IsNullOrEmpty(inputField.text))
        {
            if (int.TryParse(inputField.text, out int result))
            {
                if(result < 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                    return;
                }
                if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value < result)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFireKey)}이 부족합니다!");
                    return;
                }

                int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value;

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
                    if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value < result)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFireKey)}이 부족합니다!");
                        return;
                    }


                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += score * result;
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value -= result;

                    List<TransactionValue> transactions = new List<TransactionValue>();


                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
                    goodsParam.Add(GoodsTable.DokebiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value);

                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                    ServerData.SendTransaction(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.DokebiFire)} {score * result}개 획득!", null);
                    });
                }, null);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("소탕 횟수를 입력해주세요.");
            }
        }


    }
    
    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value < GameBalance.dokebiFireGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"처치 수 {GameBalance.dokebiFireGraduateScore} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                "각성시 도깨비 숲에서 도깨비불 획득이 더이상 불가능 합니다.\n" +
                $"대신 도깨비불 효과가 강화되고({GameBalance.dokebiGraduatePlusValue*100}%)\n" +
                $"도깨비숲 각성시 최고점수가 {GameBalance.dokebiFireFixedScore}로 고정 됩니다. \n" +
                $"그리고 스테이지 일반요괴 처치시 도깨비불을 자동 획득 합니다.\n" +
                "각성 하시겠습니까?", () =>
                {
                    ServerData.userInfoTable.TableDatas[UserInfoTable.graduateDokebiFire].Value = 1;
                    ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value = GameBalance.dokebiFireFixedScore;
                    
                    List<TransactionValue> transactions = new List<TransactionValue>();
                    
                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.graduateDokebiFire, ServerData.userInfoTable.TableDatas[UserInfoTable.graduateDokebiFire].Value);
                    userInfoParam.Add(UserInfoTable.DokebiFireClear, ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value);

                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName,UserInfoTable.Indate,userInfoParam));
                    
                    ServerData.SendTransaction(transactions,successCallBack: () =>
                    {
                        UpdateAbilText1((int)ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
                        Initialize();
                    });
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
              
                    
                }, null);
        }
    }
}
