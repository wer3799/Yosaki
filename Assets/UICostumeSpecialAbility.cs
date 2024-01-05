using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;

public class UICostumeSpecialAbility : MonoBehaviour
{
    [SerializeField] private CostumeSpecialAbilityCell prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private TextMeshProUGUI pointText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        MakeCell();
     
        Subscribe();
    }

    private void OnEnable()
    {
        GetSkillPoint();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.CostumeSkillPoint).AsObservable().Subscribe(e =>
        {
            pointText.SetText($"강화 포인트 : {e}");
        }).AddTo(this);
    }

    public void ResetSkillPoint()
    {
        var tableData = TableManager.Instance.CostumeSpecialAbility.dataArray;
        var levelSum = 0;
        
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.CostumeSkillPoint);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param param = new Param();
        transactions.Add(TransactionValue.SetUpdate(CostumeSpecialAbilityServerTable.tableName, CostumeSpecialAbilityServerTable.Indate, param));


        for (int i = 0; i < tableData.Length; i++)
        {
            levelSum += ServerData.costumeSpecialAbilityServerTable.TableDatas[tableData[i].Stringid].level.Value;
            ServerData.costumeSpecialAbilityServerTable.TableDatas[tableData[i].Stringid].level.Value = 0;
            param.Add(tableData[i].Stringid, ServerData.costumeSpecialAbilityServerTable.TableDatas[tableData[i].Stringid].ConvertToString());
        }
        
        var maxLv = ServerData.costumeServerTable.GetCostumeHasAmount();

        ServerData.statusTable.GetTableData(StatusTable.CostumeSkillPoint).Value = maxLv;
        
        
        Param pointParam = new Param();
        pointParam.Add(StatusTable.CostumeSkillPoint, skillPoint.Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, pointParam));
        
        ServerData.SendTransactionV2(transactions);

        
    }
    private void GetSkillPoint()
    {
        var tableData = TableManager.Instance.CostumeSpecialAbility.dataArray;
        var levelSum = 0;
        for (int i = 0; i < tableData.Length; i++)
        {
            levelSum += ServerData.costumeSpecialAbilityServerTable.TableDatas[tableData[i].Stringid].level.Value;
        }
        
        var maxLv = ServerData.costumeServerTable.GetCostumeHasAmount();

        ServerData.statusTable.GetTableData(StatusTable.CostumeSkillPoint).Value = maxLv - levelSum;
    }

    private void MakeCell()
    {
        var tableData = TableManager.Instance.CostumeSpecialAbility.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate(prefab, parent);
            cell.Refresh(tableData[i]);
        }
    }
    
}
