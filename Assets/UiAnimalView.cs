using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalView : MonoBehaviour
{
    [SerializeField]
    private Image gradeFrame;

    [SerializeField]
    private Image petIcon;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    [SerializeField]
    private TextMeshProUGUI gradeDescription;

    private SuhopetTableData suhoPetTableData;

    public void Initialize(SuhopetTableData suhoPetTableData)
    {
        this.suhoPetTableData = suhoPetTableData;

        int grade = suhoPetTableData.Grade;

        gradeDescription.SetText($"{this.suhoPetTableData.Gradedescription}");

        gradeFrame.sprite = CommonUiContainer.Instance.itemGradeFrame[grade];

        petIcon.sprite = CommonResourceContainer.GetSuhoAnimalSprite(suhoPetTableData.Id);

        Subscribe();
    }

    

    private void Subscribe()
    {
        var serverData = ServerData.suhoAnimalServerTable.TableDatas[suhoPetTableData.Stringid].level.Subscribe
            (
                e => { levelDescription.SetText(($"LV.{e}")); }
            )
            .AddTo(this);
    }
}