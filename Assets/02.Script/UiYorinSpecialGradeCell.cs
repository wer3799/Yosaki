using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiYorinSpecialGradeCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gradeText;

    private int idx;


    public void Initialize(int _idx)
    {
        idx = _idx;

        gradeText.SetText($"{idx + 1}단계");
    }

    public void OnClickButton()
    {
        UiYorinSpecialMissionBoard.Instance.MakeMissionCell(idx);
    }
}

