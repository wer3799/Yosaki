using System.Collections;
using System.Collections.Generic;
//using System.Data.SqlClient;
using UniRx;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using UnityEngine.Serialization;

public class BossEnemyPattern : BossEnemyBase
{
    [Header("BossEnemyPattern")]

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
        switch (GameManager.contentsType)
        {
        case GameManager.ContentsType.TwelveDungeon:
            
            if (GameManager.Instance.bossId != 185)
            {
                Initialize();
            }

            if (GameManager.Instance.bossId >= 200)
            {
                UpdateBossDamage();
            }
            break;
        case GameManager.ContentsType.SpecialRequestBoss:
            break;
        }
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
    public void StartRandomAttackRoutine()
    {
        StartCoroutine(RandomAttackRoutine());
    }
    public void StartRandomAttackRoutineV2()
    {
        StartCoroutine(RandomAttackRoutineV2());
    }
    public void StartRandomAttackRoutineV3()
    {
        StartCoroutine(RandomAttackRoutineV3());
    }
    private IEnumerator RandomAttackRoutine()
    {
        UpdateBossDamage();
        while (true)
        {
            RandomPattern();
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            yield return new WaitForSeconds(0.70f);
            skeletonAnimation.AnimationState.SetAnimation(0, "attack1", false);   
            yield return new WaitForSeconds(0.13f);
            yield return new WaitForSeconds(0.25f);
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            //후딜
            yield return new WaitForSeconds(attackInterval);


        }
    }
    private IEnumerator RandomAttackRoutineV2()
    {
        UpdateBossDamage();
        while (true)
        {
            RandomPatternAttack(2);
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            yield return new WaitForSeconds(0.70f);
            skeletonAnimation.AnimationState.SetAnimation(0, "attack1", false);   
            yield return new WaitForSeconds(0.13f);
            yield return new WaitForSeconds(0.25f);
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            //후딜
            yield return new WaitForSeconds(attackInterval);


        }
    }
    private IEnumerator RandomAttackRoutineV3()
    {
        UpdateBossDamage();
        while (true)
        {
            RandomPatternAttack(2);
            yield return new WaitForSeconds(1.08f);
            //후딜
            yield return new WaitForSeconds(attackInterval);


        }
    }
    public void RandomPattern()
    {
        int patternCount = Random.Range(0,2);

        switch (patternCount)
        {
            case 0:
                foreach (var t in RandomHit)
                {
                    t.AttackStart();
                }
                break;
            case 1:
                foreach (var t in RandomHit2)
                {
                    t.AttackStart();
                }
                break;
        }
    }
    
    public void EnableRandomGameObject()
    {
        gameObjects[UnityEngine.Random.Range(0, gameObjects.Count)].SetActive(true);
    }
    
    public void EnableRandomGameObjectFromActiveFalse()
    {
        var randIdx = UnityEngine.Random.Range(0, gameObjects.Count);
        if (gameObjects[randIdx].activeSelf == true)
        {
            EnableRandomGameObjectFromActiveFalse();
        }
        else
        {
            gameObjects[randIdx].SetActive(true);
        }
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
        var data = TableManager.Instance.TwelveBossTable.dataArray[GameManager.Instance.bossId];
        if (data.Skipboss)
        {
            
        }
        else
        {

            agentHpController.SetHp(float.MaxValue);
            
            agentHpController.SetDefense(0);
          
        }
        enemyHitObjects = GetComponentsInChildren<AlarmHitObject>(true).ToList();
        
        if (GameManager.Instance.bossId < 188)
        {
            StartAttackRoutine();
        }
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
    
    private Queue<int> skillQueue = new Queue<int>();
    private int lastUsedSkill = -1; // 마지막으로 사용한 스킬
    private void ShuffleSkills(int patternCount)
        {
            
            List<int> shuffledSkills = new List<int>();
            for (int i = 0; i < patternCount; i++)
            {
                shuffledSkills.Add(i);
            }
            
            if (lastUsedSkill != -1)
            {
                shuffledSkills.Remove(lastUsedSkill); // 마지막으로 사용한 스킬을 제외합니다.
                shuffledSkills.Add(lastUsedSkill); // 마지막으로 사용한 스킬을 큐의 마지막에 추가합니다.
            }
            
            int n = shuffledSkills.Count;
     
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                (shuffledSkills[k], shuffledSkills[n]) = (shuffledSkills[n], shuffledSkills[k]);
            }
    
            // 섞인 스킬 목록을 큐에 추가합니다.
            foreach (int skill in shuffledSkills)
            {
                skillQueue.Enqueue(skill);
            }
        }
    
        public void RandomPatternAttack(int patternCount)
        {
            while (true)
            {
                var index = 0;
    
                if (skillQueue.Count > 0)
                {
                    index = skillQueue.Dequeue();
                    lastUsedSkill = index;
                }
                else
                {
                    ShuffleSkills(patternCount);
                    continue;
                }
    
                switch (index)
                {
                    case 0:
                        foreach (var t in RandomHit)
                        {
                            t.AttackStart();
                        }
    
                        break;
                    case 1:
                        foreach (var t in RandomHit2)
                        {
                            t.AttackStart();
                        }
    
                        break;
                    case 2:
                        foreach (var t in RandomHit3)
                        {
                            t.AttackStart();
                        }
                        break;
                }
    
                break;
            }
        }
}
