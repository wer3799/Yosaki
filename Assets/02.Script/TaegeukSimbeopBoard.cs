using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TaegeukSimbeopBoard : MonoBehaviour
{
    [FormerlySerializedAs("grade")] [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private Image gaugeImage;
    [SerializeField] private Image addGaugeImage;
    [SerializeField] private TextMeshProUGUI gaugeText;
    [SerializeField] private TextMeshProUGUI currentAbilityDesc;
    [SerializeField] private TextMeshProUGUI nextAbilityDesc;

    [SerializeField] private GameObject currentDescGameObject;
    [SerializeField] private GameObject nextDescGameObject;

    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private Button upgradeButton;
    
    [SerializeField] private TextMeshProUGUI elixirText;
    
    private int currentGrade = -1;
    private ReactiveProperty<int> currentElixir = new ReactiveProperty<int>();
    private void Start()
    {
        Initialize();
        Subscribe();
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value += 10000;
        }  
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekElixir).Value += 10000;
        }  
        
    }
#endif
    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < GameBalance.TaegeukSimbeopUnlockStage)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertStage(GameBalance.TaegeukSimbeopUnlockStage+2)} 스테이지 달성시 개방!");
            this.gameObject.SetActive(false);
        }
    }
    private void Initialize()
    {
        currentElixir.Value = 0;
        GetGrade();
    }
        private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).AsObservable().Subscribe(e =>
        {
            UpdateGauge();
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taegeukSimbeopIdx).AsObservable().Subscribe(e =>
        {
            UpdateUi();
        }).AddTo(this);
        currentElixir.AsObservable().Subscribe(e =>
        {
            UpdateUi();
            elixirText.SetText($"{e}");
        }).AddTo(this);
    }

    private void GetGrade()
    {
        currentGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taegeukSimbeopIdx).Value;
    }

    private void UpdateUi()
    {
        UpdateGauge();
        UpdateText();
    }
    private void UpdateGauge()
    {
        var tableData = TableManager.Instance.TaegeukSimbeop.dataArray;

        float currentValue = ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value;
        
        if (currentGrade + 1 >= tableData.Length)
        {
            gaugeText.SetText($"{Utils.ConvertNum(currentValue)}");
            upgradeText.SetText($"Max");
            upgradeButton.interactable = false;

            return;
        }
        
        float maxValue = tableData[currentGrade + 1].Conditoin_Value;

        float addValue = SleepRewardReceiver.Instance.GetUseTaegeukGoodsPerElixir();

        float addValueSum = addValue * currentElixir.Value;
        //엘릭서사용
        if (addValueSum > 0)
        {
            gaugeText.SetText($"{Utils.ConvertNum(currentValue)}(+{Utils.ConvertNum(addValueSum)}) / {maxValue}");
            gaugeImage.fillAmount = currentValue / maxValue;
            addGaugeImage.fillAmount = (currentValue+addValueSum) / maxValue;
            upgradeText.SetText($"단련하기 <color=yellow>({Utils.ConvertNum(Mathf.Min((currentValue+addValueSum)/maxValue,1)*100)}%)");
        }
        //엘릭서 미사용
        else
        {
            gaugeText.SetText($"{Utils.ConvertNum(currentValue)} / {Utils.ConvertNum(maxValue)}");
            gaugeImage.fillAmount = currentValue / maxValue;
            addGaugeImage.fillAmount = 0;
            upgradeText.SetText($"단련하기 ({Utils.ConvertNum(Mathf.Min(currentValue/maxValue,1)*100)}%)");
        }

        //확률이 10%미만이면 업글불가능
        bool isUpgradable = (Mathf.Min((currentValue+addValueSum) / maxValue, 1) * 100) >= 10;
        
        upgradeButton.interactable = isUpgradable;
    }

    private void UpdateText()
    {
        var tableData = TableManager.Instance.TaegeukSimbeop.dataArray;
        
        gradeText.SetText($"태극 심법 {currentGrade+1}장");

        //0단계일때
        if (currentGrade < 0)
        {
            currentDescGameObject.SetActive(false);
        }
        else
        {
            currentDescGameObject.SetActive(true);
            currentAbilityDesc.SetText($"{CommonString.GetStatusName((StatusType)tableData[currentGrade].Abiltype)}{Utils.ConvertNum(tableData[currentGrade].Abilvalue*100, 2)} 증가");
        }
        //max달성
        if (currentGrade+1 >= tableData.Length)
        {
            nextAbilityDesc.SetText($"Max 단계 달성\n <color=white>다음 업데이트를 기다려 주세요");
            upgradeButton.interactable = false;
        }
        else
        {
            nextAbilityDesc.SetText($"{CommonString.GetStatusName((StatusType)tableData[currentGrade + 1].Abiltype)}{Utils.ConvertNum(tableData[currentGrade + 1].Abilvalue*100, 2)} 증가");
        }
    }

    public void AddElixirButton()
    {
        if (SleepRewardReceiver.Instance.GetUseTaegeukGoodsPerElixir() < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TaeguekElixir)}을(를) 사용할 수 없습니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.TaeguekElixir).Value < currentElixir.Value+1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TaeguekElixir)}이(가) 부족합니다.");
            return;
        }
        var tableData = TableManager.Instance.TaegeukSimbeop.dataArray;
        
        if (currentGrade + 1 >= tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage($"최대 단계입니다!");

            return;
        }
        
        var currentGoods = ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value;

        var maxRequire = tableData[currentGrade + 1].Conditoin_Value;
        
        float addValue = SleepRewardReceiver.Instance.GetUseTaegeukGoodsPerElixir();

        float addValueSum = addValue * currentElixir.Value;
        
        if (currentGoods+addValueSum >= maxRequire)
        {
            PopupManager.Instance.ShowAlarmMessage($"최대 확률입니다!");
            return;
        }
        
        currentElixir.Value++;
    }

    public void ReduceElixirButton()
    {
        if (currentElixir.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("최소 1개부터 사용 가능합니다.");
            return;
        }
        currentElixir.Value--;
    }
    
    public void OnClickUpgradeButton()
    {
        GetGrade();
        
        var tableData = TableManager.Instance.TaegeukSimbeop.dataArray;
        
        //마지막 단계인 경우
        if (currentGrade + 1 >= tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage($"최종단계입니다.");
            return;
        }
        
        var currentGoods = ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value;

        var maxRequire = tableData[currentGrade + 1].Conditoin_Value;
        
        float addValue = SleepRewardReceiver.Instance.GetUseTaegeukGoodsPerElixir();

        float addValueSum = addValue * currentElixir.Value;

        var onePercentRequireValue = (int)(maxRequire / 100);
        
        

        //10%가 안될경우
        if (currentGoods+addValueSum < maxRequire/10)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TaeguekGoods)}이(가) 부족합니다.");
            return;
        }

        upgradeButton.interactable = false;
        
        bool isUpgrade = false;
        var successProb= (int)((currentGoods + addValueSum) / maxRequire*100);//10~99
        //엘릭서 없는 강화 확률 100%
        if (currentGoods > maxRequire)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value -= (int)maxRequire;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taegeukSimbeopIdx).Value++;
            isUpgrade = true;
        }
        //엘릭서 있는 강화확률 100%
        else if (currentGoods+addValueSum > maxRequire)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value += (int)addValueSum;
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value -= (int)maxRequire;
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekElixir).Value -= (int)currentElixir.Value;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taegeukSimbeopIdx).Value++;
            isUpgrade = true;

        }
        //확률강화
        else
        {
            
            var random = Random.Range(0, 100)+1;//1~100 랜덤
            
            //성공
            if (random <= successProb)
            {
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taegeukSimbeopIdx).Value++;
                isUpgrade = true;
            }
            //실패
            else
            {
                isUpgrade = false;
            }

            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value += (int)addValueSum;
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value -= (int)(onePercentRequireValue * successProb);
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekElixir).Value -= currentElixir.Value;
        }

        List<TransactionValue> transactionList = new List<TransactionValue>();

        if (isUpgrade == true)
        {
            Param userinfo2Param = new Param();
            //심법 강화
            userinfo2Param.Add(UserInfoTable_2.taegeukSimbeopIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taegeukSimbeopIdx).Value);
            transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));   
        }

        Param goodsParam = new Param();
        //재화 감소
        goodsParam.Add(GoodsTable.TaeguekGoods, ServerData.goodsTable.GetTableData(GoodsTable.TaeguekGoods).Value);
        if (currentElixir.Value > 0)
        {
            goodsParam.Add(GoodsTable.TaeguekElixir, ServerData.goodsTable.GetTableData(GoodsTable.TaeguekElixir).Value);
        }
        
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            LogManager.Instance.SendLogType("TaegeukSimbeop", isUpgrade.ToString(), $"성공확률 : {Mathf.Min(successProb,100)}%");
            currentElixir.Value = 0;
            upgradeButton.interactable = true;
            if (isUpgrade)
            {
                PopupManager.Instance.ShowAlarmMessage($"태극 심법 단련에 성공하셨습니다.");
                GetGrade();
                UpdateUi();
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage($"태극 심법 단련에 실패하셨습니다.");
            }
        });
    }
    
}
