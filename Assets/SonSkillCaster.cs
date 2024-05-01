using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

public class SonSkillCaster : SingletonMono<SonSkillCaster>
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

                skillRoutine = StartCoroutine(UserSonSkillRoutine());
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

    public void SonSkillAnim()
    {

    }

    private IEnumerator UserSonSkillRoutine()
    {
        var skillTableDatas = TableManager.Instance.SkillData;

        var sonSkills = skillTableDatas.Where(e => e.Value.SKILLCASTTYPE == SkillCastType.Son).Select(e=>e.Value).ToList();

        while (true)
        {
            var sonLevel = ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value;

            for(int i=0;i< sonSkills.Count; i++) 
            {
                if (sonLevel < sonSkills[i].Sonunlocklevel) continue;
                if (AutoManager.Instance.canAttack == false && GameManager.Instance.IsNormalField == true) continue;

                PlayerSkillCaster.Instance.UseSkill(sonSkills[i].Id);
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
