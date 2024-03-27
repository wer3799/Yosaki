using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSasinsuDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;
    
    [SerializeField]
    private TextMeshProUGUI gradeDescription;
    
    [SerializeField]
    private TextMeshProUGUI damageCutDescription;

    [SerializeField]
    private Image image;

    private int starType;

    private int sasinType;
    public void Initialize(int starIdx,int sasinIdx)
    {
        if (starIdx == -1) starIdx = 0;


        var tableData = TableManager.Instance.sasinsuTable.dataArray[sasinIdx];

        starType = starIdx;

        sasinType = sasinIdx;

        abilDescription.SetText($"{CommonString.GetStatusName((StatusType)tableData.Abiltype0[starIdx])}{Utils.ConvertNum(tableData.Abilvalue0[starIdx] * 100f)}");

        gradeDescription.SetText($"{tableData.Name} {starIdx+1}단계");
#if UNITY_EDITOR
        damageCutDescription.SetText($"{Utils.ConvertNum(tableData.Score[starIdx])} 이상 기록시");
#else
        damageCutDescription.SetText($"{tableData.Scoredescription[starIdx]} 이상 기록시");
#endif
        // image.sprite = Resources.Load<Sprite>($"SasinsuStars/{starType}");
        SetImageColor();
    }

    public void OnClickButton()
    {  
        Initialize(starType, sasinType);        
    }

    private void SetImageColor()
    {
        Color color = Color.white;

        switch (sasinType)
        {
            
            case 0:
                image.color = new Color(0.1960784f,1f, 0.6392157f);
                break;
            case 1:
                image.color = new Color(1f, 1f, 1f);
                break;
            case 2:
                image.color = new Color(1f, 0.5490196f, 0f);
                break;
            case 3:
                image.color = new Color(0f, 0.6901961f, 1f);
                break;
        }
     //
    }
}
