using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : SingletonMono<PopupManager>
{
    [SerializeField]
    private UiConfirmPopup confirmPopupPrefab;

    [SerializeField]
    private UiConfirmPopup deadPopupPrefab;

    [SerializeField]
    private UiConfirmPopup versionUpPopupPrefab;

    [SerializeField]
    private UiYesNoPopup yesNoPopupPrefab;

    [SerializeField]
    private UiAlarmMessage alarmMessagePrefab;
    [SerializeField]
    private UiAlarmMessage alarmMessageVer2Prefab;

    [SerializeField]
    private UiReviewPopup uiReviewPopup;

    [SerializeField]
    private Animator fadeMask;

    [SerializeField]
    private GameObject whiteEffect;

    [SerializeField]
    private GameObject chatBoard;

    private List<GameObject> popupList =new List<GameObject>();
    
    public bool ignoreAlarmMessage { get; private set; }

    public void SetIgnoreAlarmMessage(bool ignore)
    {
        ignoreAlarmMessage = ignore;
    }

    public void SetChatBoardPopupManager()
    {
        chatBoard.SetActive(false);
        chatBoard.transform.SetParent(this.transform);

        var rc = chatBoard.GetComponent<RectTransform>();
        rc.offsetMin = Vector3.zero;
        rc.offsetMax = Vector3.zero;
        rc.localScale = Vector3.one;
    }
    public void SetChatBoardMainGameCanvas()
    {
        //chatBoard.SetActive(true);
      //  chatBoard.transform.SetParent(MessageBoardParent.Instance.transform);

        var rc = chatBoard.GetComponent<RectTransform>();
        rc.offsetMin = Vector3.zero;
        rc.offsetMax = Vector3.zero;
        rc.localScale = Vector3.one;
    }

    public void PlayFade()
    {
        fadeMask.SetTrigger("Fade");
    }

    public void ShowAlarmMessage(string description)
    {
        if (ignoreAlarmMessage == true) return;

        var alarmMessage = Instantiate<UiAlarmMessage>(alarmMessagePrefab, this.transform);
        alarmMessage.Initialize(description);
    }

    public void ShowAlarmMessage2(string description)
    {
        if (ignoreAlarmMessage == true) return;

        var alarmMessage = Instantiate<UiAlarmMessage>(alarmMessageVer2Prefab, this.transform);
        alarmMessage.Initialize(description);
    }

    public void ShowAlarmMessage(string description, float delay)
    {
        if (ignoreAlarmMessage == true) return;

        StartCoroutine(SlowAlarmRoutine(description, delay));
    }//1497

    IEnumerator SlowAlarmRoutine(string description, float delay)
    {
        yield return new WaitForSeconds(delay);

        var alarmMessage = Instantiate<UiAlarmMessage>(alarmMessagePrefab, this.transform);
        alarmMessage.Initialize(description);
    }

    public void ShowConfirmPopup(string title, string description, Action confirmCallBack, bool closeWhenConfirm = true)
    {
        #if UNITY_EDITOR
        
        Debug.LogError("ShowConfirmPopup");
        
        #endif
        
        var confirmPopup = Instantiate<UiConfirmPopup>(confirmPopupPrefab, this.transform);
        confirmPopup.Initialize(title, description, confirmCallBack, closeWhenConfirm);
    }

    public void ShowDeadConfirmPopup(string title, string description, Action confirmCallBack, bool closeWhenConfirm = true)
    {
        var confirmPopup = Instantiate<UiConfirmPopup>(deadPopupPrefab, this.transform);
        confirmPopup.Initialize(title, description, confirmCallBack, closeWhenConfirm);
    }

    public void ShowVersionUpPopup(string title, string description, Action confirmCallBack, bool closeWhenConfirm = true)
    {
        var confirmPopup = Instantiate<UiConfirmPopup>(versionUpPopupPrefab, this.transform);
        confirmPopup.Initialize(title, description, confirmCallBack, closeWhenConfirm);
    }

    public void ShowYesNoPopup(string title, string description, Action yesCalLBack, Action noCallBack)
    {
        var yesNoPopup = Instantiate<UiYesNoPopup>(yesNoPopupPrefab, this.transform);
        yesNoPopup.Initialize(title, description, yesCalLBack, noCallBack);
        
        popupList.Add(yesNoPopup.gameObject);
    }

    public void ShowReviewPopup()
    {
        Instantiate<UiReviewPopup>(uiReviewPopup, this.transform);
    }

    public void ShowWhiteEffect()
    {
        if (SettingData.GachaWhiteEffect.Value == 0) return;

        whiteEffect.SetActive(true);
    }

    public void DestroyAllPopup()
    {
        //우선 YesNo만
        foreach (var popup in popupList)
        {
            Destroy(popup);
        }
        popupList.Clear();
    }

}
