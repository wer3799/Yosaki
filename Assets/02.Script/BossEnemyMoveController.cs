using System;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
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
                moveSpeed = 25f; //이동속도
                idleDelay = 0.2f; // 정지 
                moveDelay = 0.75f; // 이동
                attack1Delay = 0.33f; // 공격
                break;
            case 202:
                moveSpeed = 35f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.5f; // 이동
                attack1Delay = 0.3f; // 공격
                break;
            case 203:
                moveSpeed = 15f; //이동속도
                idleDelay = 0.5f; // 정지 
                moveDelay = 0.3f; // 이동
                attack1Delay = 0.3f; // 공격
                break;
            default:
                throw new ArgumentOutOfRangeException();
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

            Debug.LogError($"state : {moveState}");
            
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
