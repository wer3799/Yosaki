using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UiTransJewelBoard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> jewelLvText=new List<TextMeshProUGUI>();

    [SerializeField] private TextMeshProUGUI currentAbilityText;
    [SerializeField] private TextMeshProUGUI nextAbilityText;
    [SerializeField] private TextMeshProUGUI totalAbilityText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image requireGoodsImage;

    [SerializeField] private List<Button> buttonList;
    
    private int currentIdx = 0;
    // Start is called before the first frame update
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.jewelLevel].AsObservable().Subscribe(e =>
        {
            SetLevelText();
            SetUI();
        }).AddTo(this);
    }

    private void SetUI()
    {
        SetAbilityText(currentIdx);
        SetPrice(currentIdx);
    }
    
    
    private void SetLevelText()
    {
        for (int i = 0; i < jewelLvText.Count; i++)
        {
            jewelLvText[i].SetText($"{ServerData.etcServerTable.GetTransJewelLevel(i)+1} 레벨");
        }
    }

    private void SetImage(int idx)
    {
        requireGoodsImage.gameObject.SetActive(true);
        switch (idx)
        {
            case 0 :
                requireGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.RJ);
                break;
            case 1 :
                requireGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.YJ);
                break;
            case 2 :
                requireGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.BJ);
                break;
            case 3 :
                requireGoodsImage.gameObject.SetActive(false);
                var lv = ServerData.etcServerTable.GetTransJewelLevel(idx) + 1;
                priceText.SetText($"모든 보옥 레벨 {lv+1} 달성 필요");

                break;
        }
    }
    
    private void SetAbilityText(int idx)
    {
        var lv = ServerData.etcServerTable.GetTransJewelLevel(idx)+1;

        var value = TableManager.Instance.TransJewelAbil.dataArray[idx].Abiladdvalue;

        var currentSum = lv * value;
        var nextSum = (lv+1) * value;
        
        if (lv < 1)
        {
            currentAbilityText.SetText($"획득한 능력치가 없습니다.");
        }
        else
        {
            currentAbilityText.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical37DamPer)} {Utils.ConvertNum(currentSum * 100)}");
        }
        nextAbilityText.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical37DamPer)} {Utils.ConvertNum(nextSum * 100)}");
        var sum = 0f;
        for (int i = 0; i < buttonList.Count; i++)
        {
            sum += PlayerStats.GetTransJewelAbility(StatusType.SuperCritical37DamPer, i);

        }
        
        totalAbilityText.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical37DamPer)} {Utils.ConvertNum(sum * 100)}");
    }

    private void SetPrice(int idx)
    {
        var nextLv = ServerData.etcServerTable.GetTransJewelLevel(idx)+1;

        if (TableManager.Instance.TransJewelLevel.dataArray.Length <= nextLv)
        {
            priceText.SetText("최대레벨입니다.");            
        }
        else
        {
            var price = TableManager.Instance.TransJewelLevel.dataArray[nextLv].Conditionvalue;
            priceText.SetText($"{price}");
        }

        SetImage(idx);
    }

    public void OnClickJewelButton(int idx)
    {
        currentIdx = idx;
        SetUI();
    }

    public void OnClickLevelUpButton()
    {
        var nextLv = ServerData.etcServerTable.GetTransJewelLevel(currentIdx)+1;

        switch (currentIdx)
        {
            case 0:
            case 1:
            case 2:
                if (TableManager.Instance.TransJewelLevel.dataArray.Length <= nextLv)
                {
                    PopupManager.Instance.ShowAlarmMessage("최대레벨입니다.");
                    return;
                }
                else
                {
                    var price = TableManager.Instance.TransJewelLevel.dataArray[nextLv].Conditionvalue;

                    var goodsKey = "";
                    switch (currentIdx)
                    {
                        case 0:
                            goodsKey = GoodsTable.RJ;
                            break;
                        case 1:
                            goodsKey = GoodsTable.YJ;
                            break;
                        case 2:
                            goodsKey = GoodsTable.BJ;
                            break;
                    }

                    if (ServerData.goodsTable.GetTableData(goodsKey).Value < price)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(goodsKey),JongsungType.Type_IGA)} 부족합니다.");
                        return;
                    }

                    ServerData.goodsTable.GetTableData(goodsKey).Value -= price;
                    string winString = ServerData.etcServerTable.TableDatas[EtcServerTable.jewelLevel].Value;
        
                    var scoreList = winString.Split(BossServerTable.rewardSplit).Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();

                    scoreList[currentIdx]++;

                    string newString = "";
                    for (int i = 0; i < scoreList.Count; i++)
                    {
                        newString += $"{BossServerTable.rewardSplit}{scoreList[i]}";
                    }

                    ServerData.etcServerTable.TableDatas[EtcServerTable.jewelLevel].Value = newString;
                    
                    if (syncRoutine != null)
                    {
                        CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
                    }

                    syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
                }
                
                break;
            case 3:
                if (nextLv > ServerData.etcServerTable.GetTransJewelLowestLevel())
                {
                    PopupManager.Instance.ShowAlarmMessage("조건에 만족하지 못했습니다.");
                    return;
                }
                else
                {
                    string winString = ServerData.etcServerTable.TableDatas[EtcServerTable.jewelLevel].Value;
        
                    var scoreList = winString.Split(BossServerTable.rewardSplit).Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();

                    scoreList[currentIdx]++;

                    string newString = "";
                    for (int i = 0; i < scoreList.Count; i++)
                    {
                        newString += $"{BossServerTable.rewardSplit}{scoreList[i]}";
                    }

                    ServerData.etcServerTable.TableDatas[EtcServerTable.jewelLevel].Value = newString;
                    
                    if (syncRoutine != null)
                    {
                        CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
                    }
                    syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

                }
                break;
                
                
        }
    }
    private WaitForSeconds syncDelay = new WaitForSeconds(0.6f);

    private Coroutine syncRoutine;
    private IEnumerator SyncRoutine()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (i == currentIdx) continue;
            buttonList[i].interactable = false;
        }
        
        yield return syncDelay;
        var goodsKey = "";
        switch (currentIdx)
        {
            case 0:
                goodsKey = GoodsTable.RJ;
                break;
            case 1:
                goodsKey = GoodsTable.YJ;
                break;
            case 2:
                goodsKey = GoodsTable.BJ;
                break;
        }
        
        Debug.LogError($"@@@@@@@@@@@@@@@TransJewel SyncComplete@@@@@@@@@@@@@@");
        
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param etcParam = new Param();
        
        if (currentIdx != 3)
        {
            Param goodsParam = new Param();
            goodsParam.Add(goodsKey, ServerData.goodsTable.GetTableData(goodsKey).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }
            
        etcParam.Add(EtcServerTable.jewelLevel,ServerData.etcServerTable.TableDatas[EtcServerTable.jewelLevel].Value);
        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            using var e = buttonList.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.interactable = true;
            }
        });
    }

    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.RJ).Value += 100;
            ServerData.goodsTable.GetTableData(GoodsTable.YJ).Value += 100;
            ServerData.goodsTable.GetTableData(GoodsTable.BJ).Value += 100;
        }    
    }
#endif
}
