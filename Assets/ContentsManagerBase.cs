using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ContentsState
{
    Fight, Dead, TimerEnd, Clear, AllPlayerEnd, Retire
}

public class ContentsManagerBase : SingletonMono<ContentsManagerBase>
{
    [SerializeField]
    private PolygonCollider2D cameracollider;

    [SerializeField]
    protected ObscuredInt playTime = 60;

    protected Coroutine timerRoutine;

    [SerializeField]
    protected TextMeshProUGUI timerText;

    protected ObscuredFloat remainSec = 0;
    public ObscuredFloat RemainSec => remainSec;
    public ObscuredInt PlayTime => playTime;

    public virtual Transform GetMainEnemyObjectTransform()
    {
        return null;
    }

    public virtual double GetDamagedAmount()
    {
        return 0f;
    }

    public virtual double GetBossRemainHpRatio()
    {
        return 0f;
    }

    protected virtual IEnumerator ModeTimer()
    {
        remainSec = playTime;

        while (remainSec >= 0)
        {
            timerText.SetText($"남은시간 : {(int)remainSec}");
            yield return null;
            remainSec -= Time.deltaTime;
        }

        TimerEnd();
    }


    protected virtual void TimerEnd() { }

    protected void Start()
    {
        SetCameraCollider();

        remainSec = 30;
        
        timerRoutine = StartCoroutine(ModeTimer());

        UiSubMenues.Instance.gameObject.SetActive(false);
    }

    protected void StopTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
        }
    }

    private void SetCameraCollider()
    {
        var cameraConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        cameraConfiner.m_BoundingShape2D = cameracollider;
    }

    public virtual void DiscountRelicDungeonHp()
    {

    }
}
