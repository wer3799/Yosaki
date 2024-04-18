using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiHaetalLevelCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gradeText;

    private HaetalTableData tableData;

    [SerializeField] private GameObject checkObject;

    public void Initialize(HaetalTableData _tableData)
    {
        this.tableData = _tableData;

        gradeText.SetText($"{tableData.Id+1}단계");

        var idx = ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.haetalGradeIdx).Value;
        
        checkObject.SetActive(idx>=tableData.Id);
    }
    
    public void OnClickButton()
    {
        UiHaetalBoard.Instance.SetUi(tableData);
    }
}
