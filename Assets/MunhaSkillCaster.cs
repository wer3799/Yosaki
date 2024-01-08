using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MunhaSkillCaster : SingletonMono<MunhaSkillCaster>
{
    public Transform skillSpawnPos;

    private Coroutine skillRoutine;

    public static ReactiveProperty<float> currentHitCount = new ReactiveProperty<float>();

    private SkillTableData currentSkillData = null;

    [SerializeField]
    private GameObject gaugeRoot;

    [SerializeField]
    private Image gauge;

    [SerializeField]
    private Animator animator;

    private string maxAnimTrigger = "Play";

    private float chargeCount = 1f;
    
    void Start()
    {
        currentHitCount.Value = 0;

        StartCoroutine(UserFourSkillRoutine());

        StartCoroutine(SkillCountAnimRoutine());

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).AsObservable().Subscribe(e =>
        {
            if ((int)e != -1)
            {
                gaugeRoot.SetActive(true);
            }
            else
            {
                gaugeRoot.SetActive(false);
            }
                
              
        }).AddTo(this);


    }

    private int count_Real;
    private int count_Max = 100;
    private int useSkillCount = 0;
    private int useSkillCount_Max = 1;
    private float count_Showing;

    private WaitForSeconds delay = new WaitForSeconds(0.01f);
    private WaitForSeconds directionDelay = new WaitForSeconds(0.3f);


    public void InitializeSkillCount()
    {
        int currentValue = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value;

        if (currentValue < 0)
        {
            return;
        }
        useSkillCount = 0;


        var skillIdx = TableManager.Instance.StudentTable.dataArray[currentValue].Unlock_Skill_Id;

        if (skillIdx >= 0 && skillIdx < TableManager.Instance.SkillTable.dataArray.Length)
        {
            currentSkillData = TableManager.Instance.SkillTable.dataArray[skillIdx];

            count_Max = currentSkillData.Requirehit;
        }
    }

    private IEnumerator SkillCountAnimRoutine()
    {
        while (true)
        {
            if(useSkillCount>=useSkillCount_Max)
            {
                yield return null;
            }
            else if (AutoManager.Instance.IsAutoMode)
            {
                if (count_Showing <= currentHitCount.Value)
                {
                    count_Showing += chargeCount;
                }

                if (count_Showing >= count_Max -10)
                {
                    animator.SetTrigger(maxAnimTrigger);

                    count_Showing = 0;

                    gauge.fillAmount = 1f;

                    yield return directionDelay;

                    count_Showing = currentHitCount.Value;
                }

                gauge.fillAmount = count_Showing / (float)count_Max;
            }

            yield return null;
        }
    }

    private IEnumerator UserFourSkillRoutine()
    {
        var skillTableDatas = TableManager.Instance.SkillData;

        while (true)
        {
            int currentValue = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value;

            if (currentValue == -1)
            {
                yield return null;
                continue;
            }

            if (useSkillCount >= useSkillCount_Max)
            {
                yield return null;
            }
            else
            {
                var skillIdx = TableManager.Instance.StudentTable.dataArray[currentValue].Unlock_Skill_Id;

                if (skillIdx >= 0 && skillIdx < TableManager.Instance.SkillTable.dataArray.Length)
                {
                    currentSkillData = TableManager.Instance.SkillTable.dataArray[skillIdx];

                    count_Max = currentSkillData.Requirehit;

                    if (currentSkillData.Requirehit <= currentHitCount.Value)
                    {
                        //   Debug.LogError("Use Mini Ult SKill");
                    
                        PlayerSkillCaster.Instance.UseSkill(skillIdx);

                        currentHitCount.Value = 0;
                        useSkillCount++;
                    }
                }
            }
        

            yield return null;
        }
    }
}