using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class SingleRaidEnemy : BossEnemyBase
{
    private List<AlarmHitObject> enemyHitObjects;

    [SerializeField]
    private AlarmHitObject alarmHitObject_2;

    [SerializeField]
    private List<Transform> attack2SpawnPos;

    [SerializeField]
    private AlarmHitObject alarmHitObject_3;

    [SerializeField]
    private AlarmHitObject alarmHitObject_4;

    [SerializeField]
    private Transform atk_minPos;

    [SerializeField]
    private Transform atk_maxPos;

    [SerializeField]
    private List<Transform> alarmHitObjectSpawnPos;


    private void Start()
    {
        Initialize();
    }

    private void UpdateBossDamage()
    {
        var bossTableData = TableManager.Instance.BossTableData[GameManager.Instance.bossId];

        float ratio = (float)(SingleRaidManager.Instance.GetComponent<SingleRaidManager>().GetDamagedAmount() / bossTableData.Hp);

        float damage = Mathf.Lerp(bossTableData.Attackpowermin, bossTableData.Attackpowermax,Mathf.Min(1f,ratio));

        hitObject.SetDamage(damage);

        enemyHitObjects.ForEach(e => e.SetDamage(damage));
    }

    private IEnumerator BossAttackPowerUpdateRoutine()
    {
        var updateDelay = new WaitForSeconds(5.0f);

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
            int attackType = Random.Range(0, 3);

#if UNITY_EDITOR
            Debug.LogError($"AttackType {attackType}");
#endif

            if (attackType == 0)
            {
                yield return StartCoroutine(AttackRoutine_2(1.0f));
            }
            else if (attackType == 1)
            {
                yield return StartCoroutine(AttackRoutine_3(1.0f));
            }
            else if (attackType == 2)
            {
                yield return StartCoroutine(AttackRoutine_4(1.0f));
            }

            StartCoroutine(PlayAttackAnim());

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator PlayAttackAnim()
    {
        skeletonAnimation.AnimationName = "attack";
        yield return new WaitForSeconds(1.0f);
        skeletonAnimation.AnimationName = "idle";
    }

    private void Initialize()
    {
        enemyHitObjects = GetComponentsInChildren<AlarmHitObject>().ToList();

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

                break;
        }
        var bossTableData = TableManager.Instance.BossTableData[GameManager.Instance.bossId];
        agentHpController.SetDefense(bossTableData.Defense);

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
        alarmHitObject_2.transform.position = attack2SpawnPos[Random.Range(0, attack2SpawnPos.Count)].position;

        alarmHitObject_2.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill2"));

        yield return new WaitForSeconds(delay);
    }

    //?
    private IEnumerator AttackRoutine_3(float delay)
    {
        //  alarmHitObject_3.transform.position = alarmHitObjectSpawnPos[Random.Range(0, alarmHitObjectSpawnPos.Count)].position;

        alarmHitObject_3.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }

    //
    private IEnumerator AttackRoutine_4(float delay)
    {
        alarmHitObject_4.AttackStart();

        StartCoroutine(PlaySoundDelay(1f, "BossSkill3"));

        yield return new WaitForSeconds(delay);
    }
}
