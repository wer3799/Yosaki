using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class SasinsuRaidEnemy : BossEnemyBase
{
    [SerializeField]
    private List<AlarmHitObject> enemyHitObjects;

    [SerializeField]
    private AlarmHitObject horizontalHit;

    [SerializeField]
    private AlarmHitObject verticalHit;

    [SerializeField]
    private int attackTypeMax = 2;

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
    private List<AlarmHitObject> RandomHit;

    [SerializeField]
    private List<AlarmHitObject> RandomHit2;

    [SerializeField]
    private List<AlarmHitObject> RandomHit3_Guild;

    [SerializeField]
    private List<AlarmHitObject> RandomHit3_Guild2;

    [SerializeField]
    private bool isGuildBoss = false;

    [SerializeField]
    private bool playAnim = true;

    private void Start()
    {
        Initialize();
    }


    private void UpdateBossDamage()
    {
       
            var bossTableData = TableManager.Instance.sasinsuTable.dataArray[GameManager.Instance.bossId];

            double ratio = SasinsuManager.Instance.GetComponent<SasinsuManager>().GetDamagedAmount() / 4000000000000000000;

            double damage = 1;
            float percentDamage = 0.5f;

            hitObject.SetDamage(damage, percentDamage);

            enemyHitObjects.ForEach(e => e.SetDamage((float)damage, percentDamage));
        
    }

    private IEnumerator BossAttackPowerUpdateRoutine()
    {
        var updateDelay = new WaitForSeconds(1.0f);

        while (true)
        {
            UpdateBossDamage();
            yield return updateDelay;
        }
    }

    private IEnumerator AttackRoutine()
    {

        UpdateBossDamage();
        //선딜
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            int attackType = Random.Range(0, attackTypeMax);

#if UNITY_EDITOR
            //  Debug.LogError($"AttackType {attackType}");
#endif

            if (attackType == 0)
            {
                if (isGuildBoss == false)
                {
                    yield return StartCoroutine(AttackRoutine_2(attackInterval_Real));
                }
                else
                {
                    yield return StartCoroutine(GuildBossHit(attackInterval_Real));

                }
            }
            else if (attackType == 1)
            {
                if (isGuildBoss == false)
                {
                    yield return StartCoroutine(AttackRoutine_3(attackInterval_Real));
                }
                else
                {
                    yield return StartCoroutine(GuildBossHit(attackInterval_Real));
                }
            }
            else if (attackType == 2)
            {
                if (isGuildBoss == false)
                {
                    yield return StartCoroutine(AttackRoutine_4(attackInterval_Real));
                }
                else
                {
                    yield return StartCoroutine(GuildBossHit(attackInterval_Real));
                }
            }

            if (playAnim)
            {
                //나타
                if (GameManager.Instance.bossId == 26)
                {
                    StartCoroutine(PlayAttackAnim_Nata());
                }
                else
                {
                    StartCoroutine(PlayAttackAnim());
                }
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private IEnumerator PlayAttackAnim()
    {
        //산신령 예외
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationName = "attack";
        }

        yield return new WaitForSeconds(1.5f);

        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationName = "walk";
        }
    }

    private IEnumerator PlayAttackAnim_Nata()
    {

        skeletonAnimation.AnimationName = "attack2";
        yield return new WaitForSeconds(1.5f);
        skeletonAnimation.AnimationName = "idle";
    }

    private void Initialize()
    {
        enemyHitObjects = GetComponentsInChildren<AlarmHitObject>(true).ToList();
        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.SpecialRequestBoss:
                break;
            case GameManager.ContentsType.TwelveDungeon:
                if (TableManager.Instance.TwelveBossTable.dataArray[GameManager.Instance.bossId].Skipboss)
                {
                    
                }
                else
                {
                    agentHpController.SetHp(float.MaxValue);
                }

                agentHpController.SetDefense(0);
                break;
        }
        enemyHitObjects.ForEach(e => e.SetDamage(1f));

        StartCoroutine(BossAttackPowerUpdateRoutine());

        StartCoroutine(AttackRoutine());
    }


    private IEnumerator PlaySoundDelay(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.PlaySound(name);
    }


    //?
    private IEnumerator AttackRoutine_2(float delay)
    {
        if (HitList_1.Count == 0 && horizontalHit != null)
        {

            horizontalHit.AttackStart();
        }

        for (int i = 0; i < HitList_1.Count; i++)
        {
            HitList_1[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill2"));

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }

    public void SasinsuAttackPattern(int index)
    {
        HitList_1[index].AttackStart();
    }

    //?
    private IEnumerator AttackRoutine_3(float delay)
    {
        if (HitList_2.Count == 0 && verticalHit != null)
        {
            verticalHit.AttackStart();
        }

        for (int i = 0; i < HitList_2.Count; i++)
        {
            HitList_2[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }

    //
    private IEnumerator AttackRoutine_4(float delay)
    {
        //alarmHitObject_4.AttackStart();

        for (int i = 0; i < HitList_3.Count; i++)
        {
            HitList_3[i].AttackStart();
        }

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits();

        yield return new WaitForSeconds(delay);
    }

    private void PlayRandomHits()
    {
        if (RandomHit.Count != 0)
        {
            int rankIdx = Random.Range(0, RandomHit.Count);

            for (int i = 0; i < RandomHit.Count; i++)
            {
                if (i != rankIdx)
                {
                    RandomHit[i].AttackStart();
                }
            }
        }

        if (RandomHit2.Count != 0)
        {
            int rankIdx = Random.Range(0, RandomHit2.Count);

            for (int i = 0; i < RandomHit2.Count; i++)
            {
                if (i != rankIdx)
                {
                    RandomHit2[i].AttackStart();
                }
            }
        }
    }

    private IEnumerator GuildBossHit(float delay)
    {

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        PlayRandomHits_Guild();

        yield return new WaitForSeconds(delay);
    }

    private int idx1 = 0;

    private int idx2 = 0;
    private int idx3 = 0;
    private void PlayRandomHits_Guild()
    {
        int rankIdx = Random.Range(0, RandomHit.Count);

        if (RandomHit.Count != 0)
        {
            if (idx1 >= RandomHit.Count) idx1 = 0;

            for (int i = 0; i < RandomHit.Count; i++)
            {
                if (i == rankIdx)
                {
                    RandomHit[i].AttackStart();
                }
            }

            idx1++;
        }

        if (RandomHit2.Count != 0)
        {
            if (idx2 >= RandomHit.Count) idx2 = 0;

            for (int i = 0; i < RandomHit2.Count; i++)
            {
                if (i == rankIdx)
                {
                    RandomHit2[i].AttackStart();
                }
            }

            idx2++;
        }

        if (RandomHit3_Guild.Count != 0)
        {
            if (idx3 >= RandomHit3_Guild.Count) idx3 = 0;

            for (int i = 0; i < RandomHit3_Guild.Count; i++)
            {
                if (i == idx3)
                {
                    RandomHit3_Guild[i].AttackStart();
                }
            }

            idx3++;
        }

        if (RandomHit3_Guild2 != null && RandomHit3_Guild2.Count != 0)
        {
            RandomHit3_Guild2[Random.Range(0, RandomHit3_Guild2.Count)].AttackStart();
        }
    }
}
