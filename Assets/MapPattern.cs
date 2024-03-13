using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class MapPattern : MonoBehaviour
{
    [SerializeField]
    private List<AlarmHitObject> enemyHitObjects;

    [SerializeField]
    private float attackInterval = 1f;

    [SerializeField]
    private float attackInterval_Real = 2f;

    [SerializeField]
    private List<AlarmHitObject> HitList_1;

    [SerializeField]
    private List<AlarmHitObject> HitList_2;

    [SerializeField]
    private List<AlarmHitObject> HitList_3;

    [SerializeField]
    private List<AlarmHitObject> HitList_4;

    [SerializeField]
    private List<AlarmHitObject> HitList_5;

    [SerializeField]
    private List<AlarmHitObject> HitList_6;


    private void Start()
    {
        Initialize();
    }
    [SerializeField]
    private float percentDamageValue = 0f;
    private void UpdateBossDamage()
    {
        float damage = 10f;
        enemyHitObjects.ForEach(e => e.SetDamage((float)damage, percentDamageValue));
    }
    private IEnumerator AttackRoutine()
    {
        UpdateBossDamage();
        //선딜
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            int attackType = Random.Range(0, 6);

            if (attackType == 0)
            {
                yield return StartCoroutine(AttackRoutine_2(attackInterval_Real));
            }
            else if (attackType == 1)
            {
                yield return StartCoroutine(AttackRoutine_3(attackInterval_Real));
            }
            else if (attackType == 2)
            {
                yield return StartCoroutine(AttackRoutine_4(attackInterval_Real));
            }
            else if (attackType == 3)
            {
                yield return StartCoroutine(AttackRoutine_5(attackInterval_Real));
            }
            else if (attackType == 4)
            {
                yield return StartCoroutine(AttackRoutine_6(attackInterval_Real));
            }
            else if (attackType == 5)
            {
                yield return StartCoroutine(AttackRoutine_7(attackInterval_Real));
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private void Initialize()
    {
        enemyHitObjects = GetComponentsInChildren<AlarmHitObject>(true).ToList();

        StartAttackRoutine_PartyRaid();
    }


    public void StartAttackRoutine_PartyRaid()
    {
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(AttackRoutine());
        }
    }


    private IEnumerator PlaySoundDelay(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.PlaySound(name);
    }


    private IEnumerator AttackRoutine_2(float delay)
    {
        for (int i = 0; i < HitList_1.Count; i++)
        {
            HitList_1[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill2"));
        
        yield return new WaitForSeconds(delay);
    }


    //?
    private IEnumerator AttackRoutine_3(float delay)
    {
        for (int i = 0; i < HitList_2.Count; i++)
        {
            HitList_2[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));
        
        yield return new WaitForSeconds(delay);
    }

    //
    private IEnumerator AttackRoutine_4(float delay)
    {
        for (int i = 0; i < HitList_3.Count; i++)
        {
            HitList_3[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));
        
        yield return new WaitForSeconds(delay);
    }

    //
    private IEnumerator AttackRoutine_5(float delay)
    {
        for (int i = 0; i < HitList_4.Count; i++)
        {
            HitList_4[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));
        
        yield return new WaitForSeconds(delay);
    }
    private IEnumerator AttackRoutine_6(float delay)
    {
        for (int i = 0; i < HitList_5.Count; i++)
        {
            HitList_5[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));
        
        yield return new WaitForSeconds(delay);
    }
    private IEnumerator AttackRoutine_7(float delay)
    {
        for (int i = 0; i < HitList_6.Count; i++)
        {
            HitList_6[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));
        
        yield return new WaitForSeconds(delay);
    }

}
