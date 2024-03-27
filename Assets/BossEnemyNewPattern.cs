using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using UnityEngine.Serialization;

public class BossEnemyNewPattern : BossEnemyBase
{
    [SerializeField]
    private List<AlarmHitObject> enemyHitObjects;

    [SerializeField]
    private float attackInterval = 1f;

    [SerializeField]
    private float attackIntervalReal = 2f;

    [SerializeField] private bool isAttackStop = false;


    [SerializeField]
    private List<AlarmHitObject> RandomHit;

    [SerializeField]
    private List<AlarmHitObject> RandomHit2;
    [SerializeField]
    private List<AlarmHitObject> CustomHitList;

    [SerializeField]
    private List<AlarmHitObject> RandomHit3;
    [SerializeField]
    private List<GameObject> gameObjects;

    [SerializeField] private Transform _targetTransform;
    private void Start()
    {
        Initialize();
    }
    [SerializeField]
    private float percentDamageValue = 0f;
    private void UpdateBossDamage()
    {
        float damage = 10f;
        hitObject.SetDamage(damage, percentDamageValue);
        enemyHitObjects.ForEach(e => e.SetDamage((float)damage, percentDamageValue));
    }

    public void CustomAttack(int index)
    {
        CustomHitList[index].AttackStart();
    }

    public void EnableRandomGameObject()
    {
        gameObjects[UnityEngine.Random.Range(0, gameObjects.Count)].SetActive(true);
    }
    
    public void EnableGameObject(int _index)
    {
        gameObjects[_index].SetActive(true);
    }
    private IEnumerator AttackRoutine()
    {
        UpdateBossDamage();
        //선딜
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);

            yield return StartCoroutine(GuildBossHit(attackIntervalReal));

            yield return new WaitForSeconds(attackInterval);
        }
    }


    private void Initialize()
    {
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

        StartAttackRoutine();
    }

    public void StartAttackRoutine()
    {
        StartCoroutine(AttackRoutine());
    }



    private IEnumerator PlaySoundDelay(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.PlaySound(name);
    }



    private IEnumerator GuildBossHit(float delay)
    {

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        if (!isAttackStop)
        {
            PlayRandomHits_Guild();
        }
    

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

    }
}
