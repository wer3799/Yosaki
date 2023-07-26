using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTransTowerRewardView : MonoBehaviour
{
    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;


    [SerializeField]
    private TextMeshProUGUI stageDescription;

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private TextMeshProUGUI rewardName;

    [SerializeField]
    private TextMeshProUGUI sweepAmount;

    
    private int currentId;

    public void UpdateRewardView(int idx)
    {
        currentId = idx;

        stageDescription.SetText($"{currentId + 1}층 보상");

        var towerTableData = TableManager.Instance.TransTowerTable.dataArray[idx];

       rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)towerTableData.Rewardtype);
       
       rewardDescription.SetText($"클리어 보상 : {Utils.ConvertBigNum(towerTableData.Rewardvalue)}개");

       sweepAmount.SetText($"소탕 보상 : {Utils.ConvertBigNum(towerTableData.Sweepvalue)}개");
  
       rewardName.SetText(CommonString.GetItemName((Item_Type)towerTableData.Rewardtype));
        
        UpdateButtonState();
    }

    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        currentId++;

        currentId = Mathf.Min(currentId, TableManager.Instance.TransTowerTable.dataArray.Length - 1);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != TableManager.Instance.TransTowerTable.dataArray.Length - 1;
    }
}
