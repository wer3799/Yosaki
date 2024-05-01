using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;



public class IndraSkillCaster : SingletonMono<IndraSkillCaster>
{
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

    private IEnumerator UserSonSkillRoutine()
    {
        yield return null;

        var skillTableDatas = TableManager.Instance.SkillTable.dataArray;

        while (true)
        {
            if (AutoManager.Instance.canAttack == false && GameManager.Instance.IsNormalField == true) 
            {

            }
            else 
            {
                if (ServerData.goodsTable.GetTableData(GoodsTable.IndraPower).Value != 0)
                {
                    PlayerSkillCaster.Instance.UseSkill(skillTableDatas[19].Id);
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
            StartCoroutine(UserSonSkillRoutine());
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            StopCoroutine(skillRoutine);
        }

    }
#endif
}
