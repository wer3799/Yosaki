using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiMunhaSkillCell : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI descText;

    [SerializeField] private GameObject lockMask;
    [SerializeField] private TextMeshProUGUI lockMaskText;

    private SkillTableData tableData;
    
    public void Initialize(SkillTableData _tableData)
    {
        tableData = _tableData;
        
        SetText();
        
        Subscribe();
    }

    private void SetText()
    {
        var desc = "";
        desc += tableData.Skilldesc;

        desc += $"\n비기 피해량 : {Utils.ConvertNum(tableData.Damageper * 100)}";
            
        descText.SetText(desc);        
        
        lockMaskText.SetText($"제자 레벨 \n{tableData.Sonunlocklevel+1} 달성 시 개방");
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(tableData.Sonunlocklevel > e);
        }).AddTo(this);
    }
}
