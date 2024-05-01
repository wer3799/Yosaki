using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class SuhoAnimalSkillCaster : SingletonMono<SuhoAnimalSkillCaster>
{
    public Transform skillSpawnPos;

    private Coroutine skillRoutine;

    void Start()
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            
        }
        else
        {
            Subscribe();
        }
    }

    private void Subscribe()
    {
        AutoManager.Instance.AutoMode.AsObservable().Subscribe(e =>
        {
            if (e)
            {
                if (skillRoutine != null)
                {
                    StopCoroutine(skillRoutine);
                }

                skillRoutine = StartCoroutine(UserFourSkillRoutine());
            }
            else
            {
                if (skillRoutine != null)
                {
                    StopCoroutine(skillRoutine);
                }
            }
        }).AddTo(this);
    }


    private IEnumerator UserFourSkillRoutine()
    {
        var skillTableDatas = TableManager.Instance.SkillData;

        var suhoAnimalSkills = skillTableDatas.Where(e => e.Value.SKILLCASTTYPE == SkillCastType.SuhoAnimal).Select(e => e.Value).ToList();

        while (true)
        {
            int skillId = ServerData.suhoAnimalServerTable.GetSuhoAnimalAwakeSkillIdx();

            if (skillId != -1)
            {
                if (AutoManager.Instance.canAttack == false && GameManager.Instance.IsNormalField == true)
                {
                }
                else
                {
                    PlayerSkillCaster.Instance.UseSkill(skillId);
                }
            }

            yield return null;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (skillRoutine != null)
            {
                StopCoroutine(skillRoutine);
            }

            skillRoutine = StartCoroutine(UserFourSkillRoutine());
        }
    }
#endif
}