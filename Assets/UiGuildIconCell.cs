using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildIconCell : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private GameObject selectedFrame;

    [SerializeField]
    private TextMeshProUGUI gradeDescription;

    private int idx;

    public void Initialize(int idx)
    {
        this.idx = idx;
        icon.sprite = CommonUiContainer.Instance.guildIcon[idx];
        
        int exp = GuildManager.Instance.GetGuildIconExp(CommonUiContainer.Instance.guildIconGrade[idx]);
        
        gradeDescription.SetText($"명성{exp}이상");
        
        gradeDescription.color = CommonUiContainer.Instance.itemGradeColor[CommonUiContainer.Instance.guildIconGrade[idx]];

        
        //본인길드 아닌건 비활성화
        //30번부터 고유아이콘
        if (idx>=30)
        {
            gradeDescription.SetText($"{icon.sprite.name} 문파");
        
            gradeDescription.color = CommonUiContainer.Instance.itemGradeColor[6];

            if (icon.sprite.name == GuildManager.Instance.myGuildName)
            {
                this.transform.SetAsFirstSibling();
            }
        }
        
        Subscribe();
    }

    private void Subscribe()
    {

        GuildManager.Instance.guildIconIdx.AsObservable().Subscribe(e =>
        {

            selectedFrame.SetActive(idx == e);

        }).AddTo(this);

    }

    public void OnCliCkIconButton()
    {
        if (idx>=30 && icon.sprite.name != GuildManager.Instance.myGuildName)
        {
            PopupManager.Instance.ShowAlarmMessage("해당 아이콘은 장착하실 수 없습니다");
            return;
        }
        
        if (GuildManager.Instance.HasGuildIcon(CommonUiContainer.Instance.guildIconGrade[idx]) == false)
        {
            PopupManager.Instance.ShowAlarmMessage("문파 명성이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "문파 아이콘을 변경 합니까?", () =>
        {
            Param param = new Param();

            param.Add("guildIcon", idx);

            var bro = Backend.Social.Guild.ModifyGuildV3(param);

            if (bro.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "문파 아이콘 변경 완료!", null);
                GuildManager.Instance.guildIconIdx.Value = idx;
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"아이콘 변경에 실패했습니다.\n({bro.GetStatusCode()})", null);
            }
        }, null);
    }
}
