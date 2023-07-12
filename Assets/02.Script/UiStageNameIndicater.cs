using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiStageNameIndicater : SingletonMono<UiStageNameIndicater>
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject buttonRoot;

    [SerializeField]
    private GameObject leftButton;

    [SerializeField]
    private GameObject rightButtln;

    [SerializeField]
    private GameObject monsterCountView;

    [SerializeField]
    private GameObject fieldBossRemainObj;

    [SerializeField]
    private GameObject stageObj;

    [SerializeField]
    private TextMeshProUGUI fieldBossRemainSec;

    public ReactiveCommand whenFieldBossTimerEnd = new ReactiveCommand();

    [SerializeField]
    private Image fieldBossTimer;

    private Coroutine bossTimerRoutine;

    [SerializeField]
    private GameObject hotTimeBuffObject;
    [SerializeField]
    private GameObject hotTimeEventBuffObject;



    public void StartFieldBossTimer(int timer)
    {
        StopFieldBossTimer();

        fieldBossRemainObj.SetActive(true);
        stageObj.SetActive(false);

        bossTimerRoutine = StartCoroutine(FieldBossRoutine(timer));
    }
    public void SerFieldBossTimerDefault()
    {
        fieldBossRemainObj.SetActive(true);
        stageObj.SetActive(false);
        fieldBossTimer.fillAmount = 1f;
        fieldBossRemainSec.SetText("대기중");
    }

    public void StopFieldBossTimer()
    {
        fieldBossRemainObj.SetActive(false);
        stageObj.SetActive(true);
        if (bossTimerRoutine != null)
        {
            StopCoroutine(bossTimerRoutine);
        }
    }

    private IEnumerator FieldBossRoutine(int timer)
    {
        float remainSec = timer;

        fieldBossTimer.fillAmount = 1f;

        while (remainSec > 0)
        {
            yield return null;
            remainSec -= Time.deltaTime;
            fieldBossTimer.fillAmount = (remainSec / (float)timer);
            fieldBossRemainSec.SetText($"남은시간 {(int)remainSec}");
        }

        fieldBossTimer.fillAmount = 0f;

        whenFieldBossTimerEnd.Execute();
    }


    void Start()
    {
        if (GameManager.contentsType != GameManager.ContentsType.NormalField)
        {
            this.gameObject.SetActive(false);
            return;
        }

        int stageId = GameManager.Instance.CurrentStageData.Id;

        description.SetText($"스테이지 {Utils.ConvertStage(stageId + 1)}");

        monsterCountView.SetActive(GameManager.contentsType == GameManager.ContentsType.NormalField);

        SetArrowButtons();

        stageObj.SetActive(true);
        fieldBossRemainObj.SetActive(false);

        Subscribe();
    }

    private void Subscribe()
    {

        hotTimeBuffObject.SetActive(ServerData.userInfoTable.IsHotTime());
        hotTimeEventBuffObject.SetActive(ServerData.userInfoTable.IsHotTimeEvent());
        ServerData.userInfoTable.whenServerTimeUpdated.AsObservable().Subscribe(e =>
        {
            hotTimeEventBuffObject.SetActive(ServerData.userInfoTable.IsHotTimeEvent());
            hotTimeBuffObject.SetActive(ServerData.userInfoTable.IsHotTime());
        }).AddTo(this);
    }

    private void SetArrowButtons()
    {
        if (GameManager.contentsType != GameManager.ContentsType.NormalField)
        {
            buttonRoot.SetActive(false);
        }
        else
        {
            leftButton.SetActive(!GameManager.Instance.IsFirstScene());
            rightButtln.SetActive(!GameManager.Instance.IsLastScene());
        }
    }

    public string GetStageName(int id)
    {
        if (GameManager.contentsType == GameManager.ContentsType.NormalField)
        {
            if (id == 0)
            {
                return CommonString.ThemaName[0];
            }
            else
            {
                return $"스테이지 : {id}";
            }
        }
        else
        {
            return CommonString.GetContentsName(GameManager.contentsType);
        }
    }

    public void OnClickLeftButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "이전 스테이지로 이동합니까?", () =>
        {
            SoundManager.Instance.PlayButtonSound();
            GameManager.Instance.LoadBackScene();
        }, null);
    }
    public void OnClickRightButton()
    {
        int lastClearStage = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;

        if (lastClearStage == TableManager.Instance.GetLastStageIdx())
        {
            PopupManager.Instance.ShowAlarmMessage("최고 단계 입니다. 다음 업데이트를 기다려주세요!");
            return;
        }

        int nextStageId = GameManager.Instance.CurrentStageData.Id + 1;

        if (nextStageId > lastClearStage + 1)
        {
            PopupManager.Instance.ShowAlarmMessage("현재 스테이지를 클리어 하지 못했습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "다음 스테이지로 이동합니까?", () =>
        {
            SoundManager.Instance.PlayButtonSound();
            GameManager.Instance.LoadNextScene();
        }, null);
    }
}
