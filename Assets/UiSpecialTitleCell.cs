using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiSpecialTitleCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private TextMeshProUGUI abilityValueText;
    [SerializeField] private TextMeshProUGUI lockMaskText;
    [SerializeField] private GameObject lockMaskObject;


    private SpecialTitleServerData _serverData;
    private Title_SpecialData _tableData;
    public void Initialize(Title_SpecialData tableData)
    {
        _tableData = tableData;
        
        _serverData = ServerData.specialTitleServerTable.TableDatas[_tableData.Stringid];
        
        abilityNameText.SetText($"{_tableData.Name}");
        var type = (StatusType)_tableData.Abiltype;

        if (type.IsPercentStat())
        {
            abilityValueText.SetText($"{CommonString.GetStatusName(type)} {Utils.ConvertNum(_tableData.Abilvalue*100,5)}");
        }
        else
        {
            abilityValueText.SetText($"{CommonString.GetStatusName(type)} {Utils.ConvertNum(_tableData.Abilvalue,5)}");
        }
        lockMaskText.SetText($"{_tableData.Lockmaskdescription}");
        
        Subscribe();
    }

    private void Subscribe()
    {
        _serverData.hasItem.AsObservable().Subscribe(e =>
        {
            lockMaskObject.SetActive(e < 1);
        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        if (CanGetTitle() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("조건을 충족하지 못하셨습니다.");
            return;
        }

        var key = _tableData.Stringid; 
        
        ServerData.specialTitleServerTable.TableDatas[key].hasItem.Value = 1;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param param = new Param();
        
        param.Add(key, ServerData.specialTitleServerTable.TableDatas[key].ConvertToString());
        
        transactionList.Add(TransactionValue.SetUpdate(SpecialTitleServerTable.tableName, SpecialTitleServerTable.Indate, param));

        ServerData.SendTransactionV2(transactionList);


    }

    private bool CanGetTitle()
    {
        switch (_tableData.SPECIALTITLEUNLOCKTYPE)
        {
            case SpecialTitleUnlockType.Weapon:
                return ServerData.weaponTable.TableDatas["weapon" + _tableData.Unlockamount].hasItem.Value > 0;
            case SpecialTitleUnlockType.Son_Level_Real:
                return ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value >= _tableData.Unlockamount;
            case SpecialTitleUnlockType.SP:
                return ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value >= _tableData.Unlockamount;
            case SpecialTitleUnlockType.SuhoPet:
                var key = TableManager.Instance.suhoPetTable.dataArray[(int)_tableData.Unlockamount].Stringid;
                return ServerData.suhoAnimalServerTable.TableDatas[key].hasItem.Value > 0;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
