using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using BackEnd;
using UnityEditor;

public class UiYoPowerIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI currentLevel;

    [SerializeField]
    private TextMeshProUGUI currentLevelDesc;

    [SerializeField]
    private TextMeshProUGUI nextLevelDesc;

    [SerializeField]
    private TextMeshProUGUI currentAbilAmount;
    [SerializeField]
    private TextMeshProUGUI contentDesc;


    [SerializeField]
    private List<Image> marbleCircles;

    [SerializeField] private List<Color> _colors;

    private void Start()
    {
        Initialize();
        Subscribe();
        
        contentDesc.SetText($"{CommonString.GetItemName(Item_Type.YoPowerGoods)}는 스테이지 요괴를 처치 시 획득할 수 있습니다.\n" +
                            $"요력 강화 시 {CommonString.GetStatusName(StatusType.SuperCritical25DamPer)} 능력치를 획득하실 수 있습니다.\n" +
                            $"요력 단계가 올라가면 능력치가 크게 상승합니다.");
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yoPowerIdx).AsObservable().Subscribe(e => { UpdateUi(); }).AddTo(this);
    }

    private void Initialize()
    {
        UpdateUi();
    }

    public void UpdateUi()
    {
        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yoPowerIdx).Value;

        var tableData = TableManager.Instance.YokaiPowerOpen.dataArray;

        string currentDesc = string.Empty;

        string nextDesc = string.Empty;

        if (idx < 0)
        {
            currentLevel.SetText($"0\n단\n계");
            currentLevelDesc.SetText($"없음");
        }
        else
        {
            currentDesc +=
                $"{CommonString.GetStatusName(StatusType.SuperCritical25DamPer)} {tableData[idx].Abil_Value * 100} 증가";

            currentLevel.SetText($"{tableData[idx].Level}\n단\n계");

            currentLevelDesc.SetText(currentDesc);
        }

        if (tableData.Length > idx + 1)
        {
            nextDesc += $"{CommonString.GetStatusName(StatusType.SuperCritical25DamPer)} {tableData[idx + 1].Abil_Value * 100} 증가";

            nextLevelDesc.SetText(nextDesc);
            
            priceText.SetText($"{tableData[idx + 1].Conditoin_Value}");
        }
        else
        {
            nextLevelDesc.SetText("최종단계 달성!");
            priceText.SetText("최종단계 달성!");
        }

        currentAbilAmount.SetText(
            $"{CommonString.GetStatusName(StatusType.SuperCritical25DamPer)} {Utils.ConvertNum(PlayerStats.GetYoPowerEffect(StatusType.SuperCritical25DamPer) * 100,2)} 증가");

        //그림
        if (idx < 0)
        {
            marbleCircles[0].color = _colors[0];
            marbleCircles[1].color = _colors[0];
            marbleCircles[2].color = _colors[0];
            marbleCircles[3].color = _colors[0];
            marbleCircles[4].color = _colors[0];
            marbleCircles[5].color = _colors[0];
            marbleCircles[6].color = _colors[0];
            return;
        }

        var type = tableData[idx].Type;
        switch (type)
        {
            case 1:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[0];
                marbleCircles[2].color = _colors[0];
                marbleCircles[3].color = _colors[0];
                marbleCircles[4].color = _colors[0];
                marbleCircles[5].color = _colors[0];
                marbleCircles[6].color = _colors[0];
                break;
            case 2:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[0];
                marbleCircles[3].color = _colors[0];
                marbleCircles[4].color = _colors[0];
                marbleCircles[5].color = _colors[0];
                marbleCircles[6].color = _colors[0];
                break;
            case 3:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[1];
                marbleCircles[3].color = _colors[0];
                marbleCircles[4].color = _colors[0];
                marbleCircles[5].color = _colors[0];
                marbleCircles[6].color = _colors[0];
                break;
            case 4:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[1];
                marbleCircles[3].color = _colors[1];
                marbleCircles[4].color = _colors[0];
                marbleCircles[5].color = _colors[0];
                marbleCircles[6].color = _colors[0];
                break;
            case 5:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[1];
                marbleCircles[3].color = _colors[1];
                marbleCircles[4].color = _colors[1];
                marbleCircles[5].color = _colors[0];
                marbleCircles[6].color = _colors[0];
                break;
            case 6:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[1];
                marbleCircles[3].color = _colors[1];
                marbleCircles[4].color = _colors[1];
                marbleCircles[5].color = _colors[1];
                marbleCircles[6].color = _colors[0];
                break;
            case 7:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[1];
                marbleCircles[3].color = _colors[1];
                marbleCircles[4].color = _colors[1];
                marbleCircles[5].color = _colors[1];
                marbleCircles[6].color = _colors[1];
                break;
            case 8:
                marbleCircles[0].color = _colors[0];
                marbleCircles[1].color = _colors[0];
                marbleCircles[2].color = _colors[0];
                marbleCircles[3].color = _colors[0];
                marbleCircles[4].color = _colors[0];
                marbleCircles[5].color = _colors[0];
                marbleCircles[6].color = _colors[0];
                break;
        }
    }

    public void OnClickUpgradeButton()
    {
        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yoPowerIdx).Value;

        var tableData = TableManager.Instance.YokaiPowerOpen.dataArray;

        if (tableData.Length <= idx + 1)
        {
            PopupManager.Instance.ShowAlarmMessage("최종 단계를 달성하셨습니다!");
            return;
        }

        var require = tableData[idx + 1].Conditoin_Value;

        //조건
        if (require > ServerData.goodsTable.GetTableData(GoodsTable.YoPowerGoods).Value)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.YoPowerGoods)}이 부족합니다.");
            return;
        }
        
        ServerData.goodsTable.GetTableData(GoodsTable.YoPowerGoods).Value -= require;

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yoPowerIdx).Value++;

        //-초기인 1일 때
        if (idx < 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"요력 개방 단계 상승!!");
        }
        else
        {
            if (tableData[idx + 1].Level != tableData[idx].Level)
            {
                PopupManager.Instance.ShowAlarmMessage($"요력 개방 단계 상승!!");
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage($"요력 개방 강화 성공!!");
            }
        }

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

        Debug.LogError($"@@@@@@@@@@@@@@@YokaiPowerOpen SyncComplete@@@@@@@@@@@@@@");

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfo2Param = new Param();
        
        userInfo2Param.Add(UserInfoTable_2.yoPowerIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yoPowerIdx).Value);
        
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));

        Param goodsParam = new Param();
        
        goodsParam.Add(GoodsTable.YoPowerGoods, ServerData.goodsTable.GetTableData(GoodsTable.YoPowerGoods).Value);
        
        
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () => { });
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.YoPowerGoods).Value += 10000;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yoPowerIdx).Value = -1;
        }
    }

#endif
}