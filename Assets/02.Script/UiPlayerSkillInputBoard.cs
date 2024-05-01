using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiPlayerSkillInputBoard : SingletonMono<UiPlayerSkillInputBoard>
{
    [SerializeField]
    private List<Image> skillIcons;
    [SerializeField]
    private List<Image> dimensionSkillIcons;

    [SerializeField]
    private List<GameObject> maskObjects;

    [SerializeField]
    private Sprite emptySprite;

    private List<ReactiveProperty<int>> selectedSkillIdxList;

    private CompositeDisposable disposables = new CompositeDisposable();

    [SerializeField]
    private GameObject autoDesc;

    [SerializeField]
    private List<Toggle> skillGroupToggles;

    private int currentSelectedSkillGroup = 0;

    [SerializeField]
    private GameObject changeMaskingButton;

    private const float coolTimeDelay = 3.0f;

    private WaitForSeconds coolTimeDelayWs = new WaitForSeconds(coolTimeDelay);

    private Coroutine coolTimeMaskRoutine;

    public void OnClickChangeMaskButton()
    {
        PopupManager.Instance.ShowAlarmMessage($"{(int)coolTimeDelay}초에 한번만 바꿀수 있습니다.");
    }

    private IEnumerator CoolTimeRoutine()
    {
        changeMaskingButton.SetActive(true);

        yield return coolTimeDelayWs;

        changeMaskingButton.SetActive(false);
    }
    public void WhenSkillGroupChangedByScript(int group)
    {
        if (coolTimeMaskRoutine != null)
        {
            StopCoroutine(coolTimeMaskRoutine);
        }

        coolTimeMaskRoutine = StartCoroutine(CoolTimeRoutine());

        currentSelectedSkillGroup = group;

        //ServerData.userInfoTable.UpData(UserInfoTable.selectedSkillGroupId, currentSelectedSkillGroup, false);

        LoadSkillSlotData();
        // RefreshUi();

        if (AutoManager.Instance.IsAutoMode)
        {
            AutoManager.Instance.ResetSkillQueue();
        }
    }
    private bool isFirstEnter = true;
    public void WhenSkillGroupChanged(int group)
    {
        if (coolTimeMaskRoutine != null)
        {
            StopCoroutine(coolTimeMaskRoutine);
        }

        coolTimeMaskRoutine = StartCoroutine(CoolTimeRoutine());

        currentSelectedSkillGroup = 0;

        if (isFirstEnter == false)
        {
            ServerData.userInfoTable.UpData(UserInfoTable.selectedSkillGroupId, currentSelectedSkillGroup, false);
        }

        isFirstEnter = false;

        LoadSkillSlotData();
        // RefreshUi();

        if (AutoManager.Instance.IsAutoMode)
        {
            AutoManager.Instance.ResetSkillQueue();
        }
    }

    private new void Awake()
    {
        base.Awake();

        Subscribe();

        SetSkillGroup();
    }

    private void SetSkillGroup()
    {
        int currentSelectedGroupId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.selectedSkillGroupId].Value;

        skillGroupToggles[currentSelectedGroupId].isOn = true;

        WhenSkillGroupChangedByScript(currentSelectedGroupId);
    }


    private void LoadSkillSlotData()
    {
        int currentSelectedGroupId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.selectedSkillGroupId].Value;

        WhenSelectedSkillIdxChanged(ServerData.skillServerTable.GetSelectedSkillIdx(currentSelectedGroupId));
    }

    private void Subscribe()
    {
        ServerData.skillServerTable.whenSelectedSkillIdxChanged.AsObservable().Subscribe(WhenSelectedSkillIdxChanged).AddTo(this);

        AutoManager.Instance.AutoMode.AsObservable().Subscribe(e =>
        {
            //autoDesc.SetActive(e);
            autoDesc.SetActive(false);
        }).AddTo(this);
    }

    private void WhenSelectedSkillIdxChanged(List<ReactiveProperty<int>> list)
    {
        disposables.Clear();

        selectedSkillIdxList = list;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Value == -1)
            {
                skillIcons[i].sprite = emptySprite;

                skillIcons[i].fillAmount = 0f;

                //  maskObjects[i].SetActive(false);
            }
            else
            {
                // maskObjects[i].SetActive(true);

                skillIcons[i].sprite = CommonResourceContainer.GetSkillIconSprite(list[i].Value);

                //쿨타임 관리
                SubscribeCoolTimeMask(list[i], i);
            }
        }
    }

    private void SubscribeCoolTimeMask(ReactiveProperty<int> skillId, int idx)
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            if (SkillCoolTimeManager.remainDimensionCool.ContainsKey(0) == false)
            {
                SkillCoolTimeManager.SetActiveDimensionSkillCool(0, 1f);
            }

            SkillCoolTimeManager.remainDimensionCool[0].AsObservable().Subscribe(remainTime =>
            {
                float coolTimeMax = 1;

                dimensionSkillIcons[0].fillAmount = 1f - (remainTime / coolTimeMax);

            }).AddTo(disposables);
        }
        else
        {
            if (SkillCoolTimeManager.remainCool.ContainsKey(skillId.Value) == false)
            {
                SkillCoolTimeManager.SetActiveSkillCool(skillId.Value, 0f);
            }

            SkillCoolTimeManager.remainCool[skillId.Value].AsObservable().Subscribe(remainTime =>
            {
                float coolTimeMax = SkillCoolTimeManager.GetSkillCoolTimeMax(skillId.Value);

                skillIcons[idx].fillAmount = 1f - (remainTime / coolTimeMax);

            }).AddTo(disposables);
        }

    }

    public void OnClickSKillToggle(int idx)
    {
        //if (AutoManager.Instance.IsAutoMode)
        //{
        //    //체크박스
        //    if (SkillCoolTimeManager.registeredSkillIdx[idx].Value == 0)
        //    {
        //        SkillCoolTimeManager.SetUseSkill(idx);
        //        PopupManager.Instance.ShowAlarmMessage($"자동 스킬 등록 완료!");
        //    }
        //    else
        //    {
        //        SkillCoolTimeManager.RemoveUseSkill(idx);
        //        PopupManager.Instance.ShowAlarmMessage($"자동 스킬 등록 해제!");
        //    }
        //    return;
        //}
    }

    public void OnClickSkillSlot(int idx)
    {

        if (selectedSkillIdxList[idx].Value == -1)
        {
#if UNITY_EDITOR
            Debug.LogError("Slot is empty");
#endif
            return;
        }

        PlayerSkillCaster.Instance.UseSkill(selectedSkillIdxList[idx].Value);

    }
    public void OnClickDimensionSkillSlot()
    {
        PlayerSkillCaster.Instance.UseDimensionSkill();

    }
    public void OnClickVisionSkillSlot()
    {

    }
    
    

    private new void OnDestroy()
    {
        base.OnDestroy();
        disposables.Dispose();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            OnClickSkillSlot(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            OnClickSkillSlot(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            OnClickSkillSlot(2);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            OnClickSkillSlot(3);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            OnClickSkillSlot(4);
        }
    }

#endif
}
