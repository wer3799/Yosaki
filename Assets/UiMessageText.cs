using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiMessageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Animator scaleAnim;
    
    private static string fadeAnimName = "Fade";
    private static string scaleAnimName = "Anim";

    private string nickName = string.Empty;


    [SerializeField]
    private Image costumeIcon;

    [SerializeField]
    private Image costumeIconFrame;

    [SerializeField]
    private List<GameObject> frame;

    [SerializeField]
    private GameObject banButtonRoot;

    private bool subscribed = false;

    public void Initialize(string description, bool isSystem, int frameId = 0)
    {
        for (int i = 0; i < frame.Count; i++)
        {
            frame[i].SetActive(!isSystem && i == frameId);
        }

        nickName = string.Empty;

        costumeIcon.gameObject.SetActive(isSystem == false);

        animator.SetTrigger(fadeAnimName);

        if (scaleAnim != null)
        {
            scaleAnim.SetTrigger(scaleAnimName);
        }

        costumeIconFrame.gameObject.SetActive(false);

        if (isSystem)
        {
            this.description.SetText(description);
        }
        else
        {
            if (description.Contains($"{CommonString.ChatSplitChar}"))
            {
                var nickSplit = description.Split('>');

                if (nickSplit.Length >= 3)
                {
                    var nickFind = nickSplit[2].Split(':');

                    if (nickFind.Length > 0)
                    {
                        nickName = nickFind[0];
                    }
                }

                var split = description.Split(CommonString.ChatSplitChar);

                if (split.Length > 0)
                {
                    int costumeIdx = int.Parse(split[0]);
                    costumeIcon.gameObject.SetActive(true);
                    costumeIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail(costumeIdx);

                    costumeIconFrame.gameObject.SetActive(costumeIdx != 0 && costumeIdx != 1);
                }
                else
                {
                    costumeIcon.gameObject.SetActive(false);
                }


                if (split.Length > 1)
                {
                    this.description.SetText(description.Split(CommonString.ChatSplitChar)[1]);
                }
            }
            else
            {
                costumeIcon.gameObject.SetActive(false);
                this.description.SetText(description);
            }

        }

        banButtonRoot.SetActive(nickName != string.Empty);

        if (subscribed == false)
        {

            subscribed = true;

            Subscribe();

        }
    }

    private void Subscribe()
    {
        // ChatManager.Instance.WhenBanned.AsObservable().Subscribe(e =>
        // {
        //     if (e.Equals(this.nickName))
        //     {
        //         description.SetText("차단됨");
        //     }
        //
        // }).AddTo(this);
    }

    public void OnClickBanButton()
    {
        description.SetText("차단됨");

    //    ChatManager.Instance.WhenBanned.Execute(nickName);
    }

    public void OnClickReportButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "채팅을 신고 할까요?", () => 
        {
            PopupManager.Instance.ShowAlarmMessage("신고 완료");
        }, () => { });
    }

}
