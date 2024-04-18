using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTowerRewardView : MonoBehaviour
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

    private int currentId;

    [SerializeField] private GameManager.ContentsType type = GameManager.ContentsType.YeonOkTower;
   
    
    public void UpdateRewardView(int idx)
    {
        currentId = idx;

        stageDescription.SetText($"{currentId + 1}층 보상");

        Item_Type rewardType = Item_Type.None; 
        float rewardValue = 0f; 
        
        switch (type)
        {
            case GameManager.ContentsType.YeonOkTower :
                var yeonOk = TableManager.Instance.YeonokTowerTable.dataArray[idx];
                rewardType = (Item_Type)yeonOk.Rewardtype;
                rewardValue = yeonOk.Rewardvalue;
                break;
            case GameManager.ContentsType.ChunguTower :
                var chunGu = TableManager.Instance.TowerTable16.dataArray[idx];
                rewardType = (Item_Type)chunGu.Rewardtype;
                rewardValue = chunGu.Rewardvalue;
                break;
        }

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon(rewardType);
        rewardDescription.SetText($"{Utils.ConvertBigNum(rewardValue)}개");
        UpdateButtonState();
    }

    private int GetLength()
    {
        switch (type)
        {
            case GameManager.ContentsType.YeonOkTower:
                return TableManager.Instance.YeonokTowerTable.dataArray.Length - 1;
            case GameManager.ContentsType.ChunguTower:
                return TableManager.Instance.TowerTable16.dataArray.Length - 1;
            default:
                return 0;
        }
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

        currentId = Mathf.Min(currentId, GetLength());

        UpdateRewardView(currentId);

        UpdateButtonState();
    }

  
    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != GetLength();
    }
}
