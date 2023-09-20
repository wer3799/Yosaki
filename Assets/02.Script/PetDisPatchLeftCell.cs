using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetDisPatchLeftCell : MonoBehaviour
{
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;


    public void Initialize(PetDispatchData petDispatchData)
    {
        leftText.SetText($"{petDispatchData.Id + 1}");
        rightText.SetText($"{petDispatchData.Minscore} ~ {petDispatchData.Maxscore}");
    }
    
    private void OnScrollbarValueChanged(float value)
    {
        
    }
}
