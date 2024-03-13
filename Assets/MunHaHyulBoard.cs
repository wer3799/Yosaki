using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class MunHaHyulBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI currentAbilityText;
    [SerializeField] private TextMeshProUGUI nextAbilityText;
    [SerializeField] private TextMeshProUGUI totalAbilityText;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private List<Image> hyulImages = new List<Image>();
    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;
    void Start()
    {
        totalAbilityText.SetText("획득한 능력치가 없습니다.");
        UpdateUi((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.studentSpotGrade).Value);

        Subscribe();
    }
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.HYG).Value += 1000;
        }
    }
#endif

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.studentSpotGrade)
            .AsObservable()
            .Subscribe(e =>
            {
                if (this.gameObject.activeSelf)
                {
                    StartCoroutine(UpdatUiRoutine((int)e));
                }
            })
            .AddTo(this);
    }

    private IEnumerator UpdatUiRoutine(int idx)
    {
        yield return new WaitForSeconds(0.1f);
        UpdateUi(idx);
    }

    private void UpdateUi(int grade)
    {

        var tableData = TableManager.Instance.StudentSpot.dataArray;

        var current = Mathf.Max(grade, 0);
        var lastId = tableData.Last().Id;
        var next = Mathf.Min(grade+1, lastId);
        
        var currentData = tableData[current];
        var nextData = tableData[next];
        
        
        gradeText.SetText($"제자 혈자리({currentData.Level}단계)");

        var nextType = (StatusType)nextData.Abil_Type;
        var currentType = (StatusType)currentData.Abil_Type;
        
        //등급 없음.
        if (grade < 0)
        {
            currentAbilityText.SetText("미적용");
            if (nextType.IsPercentStat())
            {
                nextAbilityText.SetText($"{CommonString.GetStatusName(nextType)} {Utils.ConvertNum(nextData.Abil_Value * 100,2)}");
            }
            else
            {
                nextAbilityText.SetText($"{CommonString.GetStatusName(nextType)} {Utils.ConvertNum(nextData.Abil_Value)}");
            }
            priceText.SetText($"혈자리 개방\n{Utils.ConvertNum(nextData.Conditoin_Value)}");
            foreach (var t in hyulImages)
            {
                t.color = disableColor;
            }
            return;
        }
        //최대치
        else if (next == lastId)
        {
            if (currentType.IsPercentStat())
            {
                currentAbilityText.SetText($"{CommonString.GetStatusName((StatusType)currentData.Abil_Type)} {Utils.ConvertNum(currentData.Abil_Value*100,2)}");
            }
            else
            {
                currentAbilityText.SetText($"{CommonString.GetStatusName((StatusType)currentData.Abil_Type)} {Utils.ConvertNum(currentData.Abil_Value)}");
            }
            nextAbilityText.SetText("최대 레벨");
            priceText.SetText("최대 레벨");
        }
        else
        {

            if (currentType.IsPercentStat())
            {
                currentAbilityText.SetText($"{CommonString.GetStatusName((StatusType)currentData.Abil_Type)} {Utils.ConvertNum(currentData.Abil_Value*100,2)}");
            }
            else
            {
                currentAbilityText.SetText($"{CommonString.GetStatusName((StatusType)currentData.Abil_Type)} {Utils.ConvertNum(currentData.Abil_Value)}");
            }
            if (nextType.IsPercentStat())
            {
                nextAbilityText.SetText($"{CommonString.GetStatusName(nextType)} {Utils.ConvertNum(nextData.Abil_Value * 100,2)}");
            }
            else
            {
                nextAbilityText.SetText($"{CommonString.GetStatusName(nextType)} {Utils.ConvertNum(nextData.Abil_Value)}");
            }
            priceText.SetText($"혈자리 개방\n{Utils.ConvertNum(nextData.Conditoin_Value)}");

        }
        
        var typeGrade = currentData.Type;

        for (int i = 0; i < hyulImages.Count; i++)
        {
            if (i <= typeGrade)
            {
                hyulImages[i].color = enableColor;
            }
            else
            {
                hyulImages[i].color = disableColor;
            }
        }
        string desc = String.Empty;
        using var e = PlayerStats.GetMunhaHyulDictionary().GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.Key.IsPercentStat())
            {
                desc += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value*100,2)}\n";
            }
            else
            {
                desc += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value)}\n";
            }
        }

        if (!desc.IsNullOrEmpty())
        {
            totalAbilityText.SetText(desc);
        }
    }
    
     public void OnClickLevelUpButton()
    {

        var currentlv = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.studentSpotGrade).Value;
        var tableData = TableManager.Instance.StudentSpot.dataArray;
        if (currentlv + 1 >= tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다.");

            return;
        }

        var nextData = tableData[currentlv + 1];

        if (ServerData.goodsTable.GetTableData(GoodsTable.HYG).Value < nextData.Conditoin_Value)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.HYG)}이 부족합니다.");

            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.HYG).Value -= nextData.Conditoin_Value;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.studentSpotGrade).Value++;

        if (syncRoutine != null)
        {
           CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }
        
        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }
 
 private WaitForSeconds syncDelay = new WaitForSeconds(0.6f);

 private Coroutine syncRoutine;
 private IEnumerator SyncRoutine()
 {
    yield return syncDelay;

    Debug.LogError($"@@@@@@@@@@@@@@@Munha SyncComplete@@@@@@@@@@@@@@");

    List<TransactionValue> transaction = new List<TransactionValue>();

    Param goodsParam = new Param();
    goodsParam.Add(GoodsTable.HYG,ServerData.goodsTable.GetTableData(GoodsTable.HYG).Value);
        
        
    Param userinfo2Param = new Param();
    userinfo2Param.Add(UserInfoTable_2.studentSpotGrade,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.studentSpotGrade).Value);

    transaction.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
    transaction.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));
        
    ServerData.SendTransactionV2(transaction,successCallBack:(() =>
    {
    }));
 }
}
