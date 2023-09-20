using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UniRx;
using UnityEngine.Serialization;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

public class KingBossMoveController : MonoBehaviour
{
    private float moveSpeed = 0f;

    private Vector3 moveDir;

    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField]
    protected Rigidbody2D rb;

    private bool initialized = false;


    private bool isDamaged = false;

    private Transform playerTr;

    
    [SerializeField]
    private Transform viewTr;
    [SerializeField]
    private Transform targetTransform;

    [SerializeField] private CapsuleCollider2D _capsuleCollider2D;
    
    [SerializeField] private float _moveSpeed = 1;
    private int _bossId;
    public bool isMoving;

    private void Start()
    {
        _bossId = GameManager.Instance.bossId;
        playerTr = PlayerMoveController.Instance.transform;

        InitializePattern();
    }

    private void OnDisable()
    {
        StopMove();
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }

    public void ShowTip()
    {
        if (GameManager.Instance.bossId == 155)
        {
            if(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower3).Value<GameBalance.TwelveBoss_155_RequireTower10)
            {
                
                PopupManager.Instance.ShowAlarmMessage2($"{TableManager.Instance.TwelveBossTable.dataArray[155].Name}의 힘에 의해 시야가 차단됩니다.\n시야 차단을 풀기 위해선 상단전 {GameBalance.TwelveBoss_155_RequireTower10}단계 개방이 필요합니다!");
            }
        }
        else if (GameManager.Instance.bossId == 156)
        {
            if(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower3).Value<GameBalance.TwelveBoss_156_RequireTower10)
            {
                PopupManager.Instance.ShowAlarmMessage2($"{TableManager.Instance.TwelveBossTable.dataArray[156].Name}의 힘에 의해 시야가 차단됩니다.\n시야 차단을 풀기 위해선 상단전 {GameBalance.TwelveBoss_156_RequireTower10}단계 개방이 필요합니다!");
            }
        }
        else if (GameManager.Instance.bossId == 157)
        {
            if(ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower3).Value<GameBalance.TwelveBoss_157_RequireTower10)
            {
                PopupManager.Instance.ShowAlarmMessage2($"{TableManager.Instance.TwelveBossTable.dataArray[157].Name}의 힘에 의해 시야가 차단됩니다.\n시야 차단을 풀기 위해선 상단전 {GameBalance.TwelveBoss_157_RequireTower10}단계 개방이 필요합니다!");
            }
        }
    }

    public void InitializePattern()
    {
        if (_bossId == 109)
        {
            isMoving = true;
            var transform1 = transform;
            transform1.localPosition = targetTransform.localPosition;

            SetMoveDir(playerTr.position  - (transform1.position));

            if (initialized == false)
            {
                initialized = true;
            }
        }
        else if (_bossId == 110)
        {
            isMoving = true;

            SetMoveDir(playerTr.position - transform.position);

            if (initialized == false)
            {
                initialized = true;
            }
        }
        else if (_bossId == 154||_bossId == 155||_bossId == 157)
        {
            isMoving = true;

            SetMoveDir(playerTr.position - transform.position);
            _skeletonAnimation.AnimationState.SetAnimation(0, "run", true);
            if (initialized == false)
            {
                initialized = true;
            }
        }
        //펫기반보스
        else if (_bossId == 162||_bossId == 163||_bossId == 164||_bossId == 165)
        {
            isMoving = true;

            SetMoveDir(playerTr.position - transform.position);
            _skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
            if (initialized == false)
            {
                initialized = true;
            }
        }
        //인간형기반보스(수인전)
        else if (_bossId == 174||_bossId == 175||_bossId == 178||_bossId == 179)
        {
            isMoving = true;

            SetMoveDir(playerTr.position - transform.position);
            if (initialized == false)
            {
                initialized = true;
            }
        }
        //인간형기반보스(수인전)
        else if (_bossId == 181||_bossId == 183)
        {
            SetMoveDir(playerTr.position - transform.position);
            if (initialized == false)
            {
                initialized = true;
            }
        }
        else if (_bossId == 158||_bossId == 159||_bossId == 160||_bossId == 161)
        {
            MoveToPlayerVer2();
        }

    }



    private void Update()
    {
        //측천무후
         if (_bossId == 109)
        {
            if (isMoving)
            {
                rb.velocity = moveDir.normalized * moveSpeed;
            }
        }

          //항우
        else if (_bossId == 110)
        {
            if (isMoving)
            {
                rb.velocity = moveDir.normalized * moveSpeed;
            }
        }

        else if (_bossId == 154||_bossId == 155||_bossId == 157||_bossId == 162||_bossId == 163||_bossId == 164||_bossId == 165||_bossId == 174||_bossId == 175||_bossId == 178||_bossId == 179||_bossId == 181||_bossId == 183)
        {
             if (isMoving)
             {
                 rb.velocity = (playerTr.position - transform.position).normalized * moveSpeed;
             }
        }

        // float playerDist = Vector3.Distance(playerTr.position, this.transform.position);
            //
            // if (playerDist >= 0.1f) 
            // {
            //     Vector3 moveDir = playerTr.position - this.transform.position;
            //     rb.velocity = moveDir.normalized * moveSpeed * 1.5f;
            // }


            if (_bossId == 154||_bossId == 155||_bossId == 157)
            {
                viewTr.transform.localScale = new Vector3(rb.velocity.x < 0 ? -1 : 1, 1, 1);   
            }
            else if(_bossId == 162||_bossId == 163||_bossId == 164||_bossId == 165)
            {
                viewTr.transform.localScale = new Vector3(rb.velocity.x < 0 ? 3 : -3, 3, 1);   
            }
            else if(_bossId == 174||_bossId == 175||_bossId == 178||_bossId == 179||_bossId == 181||_bossId == 183)
            {
                viewTr.transform.localScale = new Vector3(rb.velocity.x < 0 ? 1 : -1, 1, 1);   
            }
            else
            {
                viewTr.transform.localScale = new Vector3(rb.velocity.x > 0 ? -1 : 1, 1, 1);
            }

    }

    private Coroutine moveCoroutine;
    
    private void MoveToPlayer()
    {
        moveCoroutine = StartCoroutine(MoveAttackToPlayer());
    }
    
    private void MoveToPlayerVer2()
    {
        moveCoroutine = StartCoroutine(MoveAttackToPlayerVer2());
    }



    private IEnumerator MoveAttackToPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            
            _capsuleCollider2D.enabled = false;
            transform.position = playerTr.position;
            _skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            yield return new WaitForSeconds(0.3f);
            _capsuleCollider2D.enabled = true;   
            _skeletonAnimation.AnimationState.SetAnimation(0, "attack3", false);
        }
    }

    [SerializeField] private float idleDelay = 1.2f;
    [SerializeField] private float continuelAttackDelay = 0.2f;
    [SerializeField] private float attack1Delay = 0f;
    [SerializeField] private float attack2Delay = 0f;
    [SerializeField] private float attack3Delay = 0f;
    private IEnumerator MoveAttackToPlayerVer2()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleDelay);
            
            _capsuleCollider2D.enabled = false;
            transform.position = playerTr.position;
            _skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            yield return new WaitForSeconds(attack1Delay);
            _capsuleCollider2D.enabled = true;   
            _skeletonAnimation.AnimationState.SetAnimation(0, "attack1", false);
            
            if(attack2Delay==0)continue;
            yield return new WaitForSeconds(continuelAttackDelay);
            _capsuleCollider2D.enabled = false;
            yield return new WaitForSeconds(attack2Delay);
            transform.position = playerTr.position;
            _skeletonAnimation.AnimationState.SetAnimation(0, "attack2", false);
            yield return new WaitForSeconds(continuelAttackDelay);
            _capsuleCollider2D.enabled = true;   
            
            if(attack3Delay==0)continue;
            yield return new WaitForSeconds(continuelAttackDelay);
            _capsuleCollider2D.enabled = false;
            yield return new WaitForSeconds(attack3Delay);
            transform.position = playerTr.position;
            _skeletonAnimation.AnimationState.SetAnimation(0, "attack3", false);
            yield return new WaitForSeconds(continuelAttackDelay);
            _capsuleCollider2D.enabled = true;   
        }
    }

    private void SetAnimationIdle()
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
    }
    
    private void StopMove()
    {
        rb.velocity=Vector2.zero;
        isMoving = false;
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
