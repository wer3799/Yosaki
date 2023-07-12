using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BonusDefenseEnterView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI increaseByStage;
    [SerializeField]
    private GameObject enterButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        enterButton.SetActive(true);

        //int increaseJade = GameBalance.bandiPlusStageJadeValue * (int)Mathf.Floor((float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value / GameBalance.bandiPlusStageDevideValue);
        if (increaseByStage != null)
        {
            increaseByStage.SetText($"10 챕터를 돌파할 때 마다 획득량 {GameBalance.bandiPlusStageJadeValue * 100}% 증가!"); 
        }
    }
}
