using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiSonSkillCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    private SkillTableData skillTableData;

    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;


    public void Initialize(SkillTableData skillTableData)
    {
        this.skillTableData = skillTableData;

        skillIcon.sprite = CommonResourceContainer.GetSkillIconSprite(skillTableData.Id);



        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Son_Level).AsObservable().Subscribe(sonLevel =>
        {
            bool active = sonLevel >= skillTableData.Sonunlocklevel;

            lockMask.SetActive(active == false);

            if (active)
            {
                var currentLevel = sonLevel - skillTableData.Sonunlocklevel;
                levelDescription.SetText($"LV : {currentLevel}");
                description.SetText(skillTableData.Skilldesc + $"\n피해량 :  {Utils.ConvertBigNum(ServerData.skillServerTable.GetSkillDamagePer(skillTableData.Id, applySkillDamAbility: false) * 100f)}%");
            }
            else
            {
                levelDescription.SetText($"LV : {0}");
                lockDescription.SetText($"손오공 레벨 {skillTableData.Sonunlocklevel}에 개방");
                description.SetText(skillTableData.Skilldesc + $"\n피해량 : {0}%");
            }

        }).AddTo(this);
    }
}
