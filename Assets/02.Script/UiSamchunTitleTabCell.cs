using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiSamchunTitleTabCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    private int tabIdx;

    public void Initialize(int idx)
    {
        tabIdx = idx;
        
        titleText.SetText(GameBalance.samchunTitle[idx]);
    }
    
    public void OnClickButton()
    {
        UiSamchunTitleBoard.Instance.OnSelectTab(tabIdx);
    }

}
