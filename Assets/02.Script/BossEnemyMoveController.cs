﻿using System;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossEnemyMoveController : MonoBehaviour
{
    [SerializeField]
    private SkeletonAnimation skeletonAnimation;

    [SerializeField] private List<AlarmHitObject> _alarmHitObjects0;
    [SerializeField] private List<AlarmHitObject> _alarmHitObjects1;
    [SerializeField] private List<AlarmHitObject> _alarmHitObjects2;
    [SerializeField] private List<AlarmHitObject> _alarmHitObjects3;
    [SerializeField] private Rigidbody2D rb;
    
    
    private enum State
    {
        Stop,
        Move,
        Attack1,
        None,
    }

    private ReactiveProperty<MoveDirection> moveDirectionType = new ReactiveProperty<MoveDirection>();

    private Vector3 moveDirection;

    [SerializeField]
    private Transform characterView;


    private float moveSpeed = 0f;
    private ReactiveProperty<State> moveState = new ReactiveProperty<State>();
    private Coroutine returnState;

    private Queue<List<AlarmHitObject>> _alarmHitObjectsQueue =new Queue<List<AlarmHitObject>>();
    private bool isInitialized = false;

    private Transform playerTr;
    private new void Start()
    {
        skeletonAnimation.AnimationState.Complete += OnAnimationComplete;

        playerTr = PlayerMoveController.Instance.transform;
        
        SetDifficulty();
        
        MakeQueue();

        Subscribe();
        
        isInitialized = true;
    }

    
    private void MakeQueue()
    {
        if (_alarmHitObjects0 != null)
        {
            _alarmHitObjectsQueue.Enqueue(_alarmHitObjects0);
        }
        if (_alarmHitObjects1 != null)
        {
            _alarmHitObjectsQueue.Enqueue(_alarmHitObjects1);
        }
        if (_alarmHitObjects2 != null)
        {
            _alarmHitObjectsQueue.Enqueue(_alarmHitObjects2);
        }
        if (_alarmHitObjects3 != null)
        {
            _alarmHitObjectsQueue.Enqueue(_alarmHitObjects3);
        }
    }
    
    private void SetDifficulty()
    {
        var bossId = (int)GameManager.Instance.bossId; 
        switch (bossId)
        {
            case 200:
            case 201:
            case 314:
            case 315:
                moveSpeed = 25f; //이동속도
                idleDelay = 0.2f; // 정지 
                moveDelay = 0.75f; // 이동
                attack1Delay = 1.2f; // 공격
                break;
            case 202:
                moveSpeed = 15f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.5f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 203:
                moveSpeed = 15f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 205:
            case 206:
                moveSpeed = 15f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 0.8f; // 공격
                break;
            case 212:
            case 213:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 0.8f; // 공격
                break;
            //용왕
            case 214:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 215:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.1f; // 공격
                break;
            case 216:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 217:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.3f; // 공격
                break;
            case 218:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 219:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; //
                break;
            case 220:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 261:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.2f; // 공격
                break;
            case 262:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.2f; // 공격
                break;
            case 263:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 0.8f; // 공격
                break;
            case 264:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.2f; // 공격
                break;
            case 292:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.5f; // 공격
                break;
            case 293:
            case 294:
            case 295:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.9f; // 공격
                break;
            case 306:
                moveSpeed = 25f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.9f; // 공격
                break;
            case 307:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.3f; // 공격
                break;
            case 308:
            case 309:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            case 317:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            case 318:
            case 319:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            case 338:
            case 339:
            case 340:
            case 377:
            case 378:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.35f; // 공격
                break;
            case 341:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            case 342:
            case 343:
            case 344:
            case 381:
            case 382:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            case 345:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            
            case 346:
                moveSpeed = 15f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.5f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 347:
                moveSpeed = 15f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 348:
            case 349:
                moveSpeed = 15f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 0.8f; // 공격
                break;
            case 350:
            case 351:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 0.8f; // 공격
                break;
            //용왕
            case 352:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 353:
            case 354:
            case 355:
            case 383:
            case 384:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.45f; // 공격
                break;
            case 356:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 2.3f; // 공격
                break;
            case 357:
            case 386:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.2f; // 공격
                break;
            case 358:
            case 359:
            case 387:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.5f; // 공격
                break;
            case 360:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 2.3f; // 공격
                break;
            
            case 361:
            case 362:
            case 363:
            case 364:
            case 388:
            case 389:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 2.3f; // 공격
                break;
            case 365:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; // 공격
                break;
            case 366:
            case 367:
            case 368:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1f; //
                break;
            case 369:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 2.3f; // 공격
                break;
            
            case 370:
            case 371:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.9f; // 공격
                break;
            
            case 372:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.3f; // 공격
                break;
            case 373:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            case 374:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.55f; // 공격
                break;
            case 379:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.9f; // 공격
                break;
            case 380:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 1.6f; // 공격
                break;
            case 385:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.2f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 2f; // 공격
                break;
            default:
                moveSpeed = 20f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 0.8f; // 공격
                break;
                
        }
        
    }

    
    private void ResetState()
    {
        moveState.Value = State.Stop;
    }

    private Coroutine moveRoutine;

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            MoveCharacterByDirection();
            
            
            yield return null;
        }
    }

    private float stateDelay = 1f;
    private float idleDelay = 0.1f;
    private float moveDelay = 0.5f;
    private float attack1Delay = 0.6f;
    private IEnumerator StateRoutine()
    {

        while (true)
        {
            RandomState();
            
            yield return new WaitForSeconds(stateDelay);

        }
    }

    private void RandomState()
    {
        moveState.Value = (State)Random.Range(0, (int)State.None);
    }
    private void SetState(State state)
    {
        moveState.Value = state;
    }
    
    
    protected virtual void FollowPlayer()
    {
        var playerPositionX = PlayerMoveController.Instance.transform.position.x;

        moveDirectionType.Value = this.transform.position.x > playerPositionX ? MoveDirection.Left : MoveDirection.Right;
    }

    private void MoveCharacterByDirection()
    {
        rb.velocity = moveDirection;
    }
    private void StopMove()
    {
        rb.velocity = Vector2.zero;
    }

    private void Subscribe()
    {
        moveState.AsObservable().Subscribe(e =>
        {
            if (isInitialized == false)
                return;
            FollowPlayer();
            WhenDirectionChanged(moveDirectionType.Value);

            //Debug.LogError($"state : {moveState}");
            
            switch (e)
            {
                case State.Stop:
                    if (moveRoutine != null)
                    {
                        StopCoroutine(moveRoutine);
                    }
                    if (skeletonAnimation != null)
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
                    }

                    StopMove();
                    stateDelay = idleDelay;
                    break;
                case State.Move:
                    if (moveRoutine != null)
                    {
                        StopCoroutine(moveRoutine);
                    }
                    if (skeletonAnimation != null)
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "run", true);
                    }
                    moveRoutine = StartCoroutine(MoveRoutine());
                    MoveCharacterByDirection();
                    EqualizeHeight();

                    stateDelay = moveDelay;

                    break;
                case State.Attack1:
                    if (moveRoutine != null)
                    {
                        StopCoroutine(moveRoutine);
                    }
                    if (skeletonAnimation != null)
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "attack1", false);
                    }
                    StopMove();
                    stateDelay = attack1Delay;
                    Attack1Pattern();
                    break;
                case State.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }).AddTo(this);

    }
    void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "attack1")
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        }
    }

    private void Attack1Pattern()
    {
        var list = _alarmHitObjectsQueue.Peek();

        foreach (var t in list)
        {
            t.AttackStart();
        }
        _alarmHitObjectsQueue.Enqueue(_alarmHitObjectsQueue.Dequeue());
    }

    private void EqualizeHeight()
    {
        
        //몬스터가 위칸
        if (PlayerMoveController.Instance.transform.position.y - this.transform.position.y < -3)
        {
            //아래로
            transform.Translate(0, (PlayerMoveController.Instance.transform.position.y-1.65f)-this.transform.position.y, 0);
        }
        else if (PlayerMoveController.Instance.transform.position.y - this.transform.position.y > 3)
        {
            //위로
            transform.Translate(0, (PlayerMoveController.Instance.transform.position.y-1.65f)-this.transform.position.y, 0);

        }
    }


    protected virtual void WhenDirectionChanged(MoveDirection moveDirectionType)
    {
        moveDirection = moveDirectionType == MoveDirection.Left ? Vector3.left : Vector3.right;
   
        moveDirection *= moveSpeed;

        FlipCharacter();
    }


    private void FlipCharacter()
    {
        if (moveDirectionType.Value == MoveDirection.Left)
        {
            characterView.transform.localScale = Vector3.one;
            characterView.transform.rotation = Quaternion.identity;
        }
        else
        {
            characterView.transform.localScale = new Vector3(1f, 1f, -1f);
            characterView.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void OnEnable()
    {
        ResetState();
        SetRandomDirection();
        StartCoroutine(StateRoutine());
    }



    private void SetRandomDirection()
    {
        moveDirectionType.Value = (MoveDirection)Random.Range((int)MoveDirection.Left, (int)MoveDirection.Max);
    }







}
