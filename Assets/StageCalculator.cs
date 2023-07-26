using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StageCalculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Desc;
    [SerializeField] private TextMeshProUGUI Desc2;
    
    [SerializeField]
    private TMP_InputField adjustNum;
    // Start is called before the first frame update

    public void OnClickCalculate()
    {
#if UNITY_EDITOR
        if (int.TryParse(adjustNum.text, out var inputNum))
        {
            if (inputNum == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }

        Desc.SetText($"{Utils.ConvertStage(inputNum)}");
        Desc2.SetText($"{inputNum - 2}\n{Utils.ConvertStage(inputNum)}");

#endif
    }
}
