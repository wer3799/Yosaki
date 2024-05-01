using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class FourSkillCaster : SingletonMono<FourSkillCaster>
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

        var fourSkills = skillTableDatas.Where(e => e.Value.SKILLCASTTYPE == SkillCastType.Four).Select(e => e.Value).ToList();

        while (true)
        {
            int fourLevel = ServerData.goodsTable.GetFourSkillHasCount();             

            for (int i = 0; i < fourSkills.Count; i++)
            {
                if (fourLevel < fourSkills[i].Sonunlocklevel) continue;
                if (AutoManager.Instance.canAttack == false && GameManager.Instance.IsNormalField == true) continue;

                PlayerSkillCaster.Instance.UseSkill(fourSkills[i].Id);
            }

            yield return null;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            StopCoroutine(skillRoutine);
        }

    }
#endif

}
