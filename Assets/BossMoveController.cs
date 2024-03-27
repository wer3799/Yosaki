using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

public class BossMoveController : MonoBehaviour
{
    private float moveSpeed = 0f;

    private Vector3 moveDir;

    [SerializeField]
    protected Rigidbody2D rb;

    private bool initialized = false;


    private bool isDamaged = false;

    private Transform playerTr;

    
    [SerializeField]
    private Transform viewTr;
    [SerializeField]
    private Transform targetTransform;

    [SerializeField] private Transform secondTransform;
    [SerializeField] private float _moveSpeed = 1;
    private int _bossId;
    public float stoppingDistance = 0.1f;
    public ReactiveProperty<bool> isMoving;
    private Vector3 dummyPosition;
    [SerializeField]
    protected SkeletonAnimation skeletonAnimation;

    private int SpecialPatternCount = 1;

    [SerializeField] private int minPatternCount = 1;
    [SerializeField] private int maxPatternCount = 3;
    [SerializeField] private float minSecond = 1;
    [SerializeField] private float maxSecond = 2;
    [SerializeField] private moveType type;
    enum moveType
    {
        None,
        SparkMove,
    }
    private void Start()
    {
        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.TwelveDungeon: 
                
                _bossId = GameManager.Instance.bossId;
                playerTr = PlayerMoveController.Instance.transform;
                InitializePattern();

                break;
            case GameManager.ContentsType.SpecialRequestBoss:

                break;
        }

    }


    public void InitializePattern()
    {
        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.TwelveDungeon: 
                if (_bossId == 109)
                {
                    isMoving.Value = true;

                    SetMoveDir(playerTr.position - (transform.position = targetTransform.position));

                    if (initialized == false)
                    {
                        initialized = true;
                    }
                }
                else if (_bossId == 110)
                {
                    isMoving.Value = true;

                    SetMoveDir(playerTr.position - transform.position);

                    if (initialized == false)
                    {
                        initialized = true;
                    }
                }
                else if (_bossId == 114)
                {
                    isMoving.Value = true;

                    dummyPosition = playerTr.position;
            
                    SetMoveDir(new Vector3(0,dummyPosition.y - secondTransform.position.y));

                    if (initialized == false)
                    {
                        initialized = true;
                    }
                }
                else if (_bossId == 115)
                {
                    isMoving.Value = true;

                    dummyPosition = playerTr.position;
            
                    SetMoveDir(new Vector3(dummyPosition.x - secondTransform.position.x,0));

                    if (initialized == false)
                    {
                        initialized = true;
                    }
                }
                else if (_bossId == 116)
                {
                    isMoving.Value = true;

                    dummyPosition = playerTr.position;
            
                    SetMoveDir(new Vector3(0,dummyPosition.y - secondTransform.position.y));

                    if (initialized == false)
                    {
                        initialized = true;
                    }
                }
                else if (_bossId == 180||_bossId == 182||_bossId == 184)
                {
                    StartCoroutine(EnemyMovementRoutine());
                    if (initialized == false)
                    {
                        initialized = true;
                    }
                }
                break;
            
            case GameManager.ContentsType.SpecialRequestBoss:
                playerTr = PlayerMoveController.Instance.transform;

                switch (type)
                {
                    case moveType.None:
                        break;
                    case moveType.SparkMove:
                        StartCoroutine(EnemyMovementRoutine());
                        if (initialized == false)
                        {
                            initialized = true;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
        }

    }
    //유사 벽력일섬
    private IEnumerator EnemyMovementRoutine()
    {
        while (true)
        {
            float blinkSecond=1f;
            switch (GameManager.contentsType)
            {
                case GameManager.ContentsType.TwelveDungeon:
                    if (_bossId == 180)
                    {
                        SpecialPatternCount = (int)Random.Range(1,3);
                        blinkSecond = 1f;
                    }
                    if (_bossId == 182)
                    {
                        SpecialPatternCount = (int)Random.Range(3,6);
                        blinkSecond = Random.Range(1,2);
                    }
                    if (_bossId == 184)
                    {
                        SpecialPatternCount = (int)Random.Range(6,9);
                        blinkSecond = Random.Range(0.8f,1.5f);
                    }
                    break;
                case GameManager.ContentsType.SpecialRequestBoss:
                    switch (type)
                    {
                        case moveType.None:
                            break;
                        case moveType.SparkMove:
                            if (maxSecond - minSecond < 0.1)
                            {
                                blinkSecond = minSecond;
                            }
                            else
                            {
                                blinkSecond = Random.Range(minSecond,maxSecond);
                            }
                            SpecialPatternCount = (int)Random.Range(minPatternCount,maxPatternCount);
                                
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                
            }
            float max = 6;
            float exclude = 4;

            for (int i = 0; i < SpecialPatternCount; i++)
            {
                // 플레이어와 닿지 않는 랜덤한 위치로 이동
                Vector3 random = new Vector3(Utils.GetRandomExcluding(-max,max,-exclude,exclude),Utils.GetRandomExcluding(-max,max,-exclude,exclude), 0f);
                var position = playerTr.position;
                var randomPosition = position + random;
                viewTr.position = randomPosition;
                viewTr.transform.localScale = new Vector3(viewTr.position.x-position.x> 0 ? -1 : 1, 1, 1);

                yield return new WaitForSeconds(blinkSecond/SpecialPatternCount);   
            }
            skeletonAnimation.AnimationState.SetAnimation(0, "attack1", true);
            SetMoveDir(playerTr.position - viewTr.position);
            yield return new WaitForSeconds(0.4f);
            // 플레이어 쪽으로 이동
            isMoving.Value = true;
            skeletonAnimation.AnimationState.SetAnimation(0, "run", true);
            // 3초 동안 이동
            yield return new WaitForSeconds(2f);
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);

            isMoving.Value = false;

        }
    }
    private void Update()
    {
        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.TwelveDungeon:
                //측천무후
                if (_bossId == 109)
                {
                    if (isMoving.Value)
                    {
                        rb.velocity = moveDir.normalized * moveSpeed;
                    }
                }

                //항우
                else if (_bossId == 110)
                {
                    if (isMoving.Value)
                    {
                        rb.velocity = moveDir.normalized * moveSpeed;
                    }
                }
                //화룡
                else if (_bossId == 114)
                {
                    if (isMoving.Value)
                    {
                        rb.velocity = moveDir.normalized * moveSpeed;

                        if ( Mathf.Abs(dummyPosition.y-secondTransform.position.y) <stoppingDistance)
                        {
                            StopMove();
                        }
                    }
                }
                //전룡
                else if (_bossId == 115)
                {
                    if (isMoving.Value)
                    {
                        rb.velocity = moveDir.normalized * moveSpeed;

                        if ( Mathf.Abs(dummyPosition.x-secondTransform.position.x) <stoppingDistance)
                        {
                            StopMove();
                        }
                    }
                }
                //흑룡
                else if (_bossId == 116)
                {
                    if (isMoving.Value)
                    {
                        rb.velocity = moveDir.normalized * moveSpeed;

                        if ( Mathf.Abs(dummyPosition.y-secondTransform.position.y) <stoppingDistance)
                        {
                            StopMove();
                        }
                    }
                }

                if (_bossId == 109 || _bossId == 110)
                {
                    viewTr.transform.localScale = new Vector3(rb.velocity.x > 0 ? -1 : 1, 1, 1);
                }
                if (_bossId == 180 || _bossId == 182|| _bossId == 184)
                {
                    if (isMoving.Value)
                    {
                        rb.velocity = moveDir.normalized * moveSpeed;
                        viewTr.transform.localScale = new Vector3(rb.velocity.x < 0 ? -1 : 1, 1, 1);
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }

                }
                break;
            case GameManager.ContentsType.SpecialRequestBoss:
                switch (type)
                {
                    case moveType.None:
                        break;
                    case moveType.SparkMove:
                        if (isMoving.Value)
                        {
                            rb.velocity = moveDir.normalized * moveSpeed;
                            viewTr.transform.localScale = new Vector3(rb.velocity.x < 0 ? -1 : 1, 1, 1);
                        }
                        else
                        {
                            rb.velocity = Vector2.zero;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
        }
        
    }

    public void StopMove()
    {
        rb.velocity=Vector2.zero;
        isMoving.Value = false;
    }
    private void SetMoveDir(Vector3 moveDir)
    {
        this.moveSpeed = _moveSpeed;
        this.moveDir = moveDir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDamaged == true) return;

        // if (collision.gameObject.layer == LayerMask.NameToLayer(EnemyMoveController.DefenseWall_str))
        // {
        //     rb.velocity = -moveDir.normalized * moveSpeed;
        // }
    }
}
