using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SetRandomPosition : MonoBehaviour
{
        
    [SerializeField]
    protected Rigidbody2D rb;
    
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    [SerializeField] private float _delay = 0;
    [SerializeField] private float _startDelay = 0;

    [SerializeField] private float _moveSpeed = 5f;
    public bool isMove;
    private Vector2 moveDir;
    
    private Transform playerTr;
    [SerializeField]
    private Transform projectileTr;
    private int _bossId;

    private bool _initiliazed;
    // Update is called once per frame
    //private Coroutine randPositionRoutine;

    [SerializeField ]private ProjectileType projectileType=ProjectileType.None;
    private enum ProjectileType
    {
        None,
        Boss,
        Projectile,
        TargetingPlayerDir,
        InfinityDuration,
        InfinityDurationAroundPlayer,
    }
    private void OnEnable()
    {
        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.TwelveDungeon:
                if (_bossId == 0)
                {
                    _bossId = GameManager.Instance.bossId;
                    playerTr = PlayerMoveController.Instance.transform;
                }
                if (//_bossId == 109||
                    _bossId == 110
                   )
                {
                    SetRandPosition();
                }

                if (_delay > 0)
                {
                    SetCoroutine();
                }
                break;
            case GameManager.ContentsType.SpecialRequestBoss:

                playerTr = PlayerMoveController.Instance.transform;

                if (_delay > 0)
                {
                    SetCoroutine();
                }
                break;
        }

    }

    private void OnDisable()
    {
        StopCoroutine(SetRandPositionRoutine());
    }

    public void SetRandPosition()
    {
        float x = Random.Range(_minX, _maxX + 1);
        float y = Random.Range(_minY, _maxY + 1);
        transform.position = new Vector3(x, y, 0);
    }

    public void SetDir()
    {
        moveDir = playerTr.position - transform.position;
    }
    public void SetDirFromRb()
    {
        moveDir = playerTr.position - rb.transform.position;
    }
    
    private void SetCoroutine()
    {
        StartCoroutine(SetRandPositionRoutine());
    }
    
    private void Update()
    {
        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.TwelveDungeon:
                //조조
                if (_bossId == 111 || _bossId == 112 ||
                    _bossId == 123 ||
                    _bossId == 133 || _bossId == 134 || _bossId == 135 ||
                    _bossId == 154 || _bossId == 155 || _bossId == 156 ||
                    _bossId == 169 ||
                    _bossId == 170 || _bossId == 171 || _bossId == 172 || _bossId == 173 || _bossId == 177 ||
                    _bossId == 274 || _bossId == 275 ||
                    _bossId == 280 || _bossId == 289 ||
                    _bossId == 290 || _bossId == 291
                   )
                {
                    if (isMove)
                    {
                        rb.velocity = moveDir.normalized * _moveSpeed;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
                break;
            case GameManager.ContentsType.SpecialRequestBoss:
                switch (projectileType)
                {
                    case ProjectileType.Boss:
                    case ProjectileType.Projectile:
                    case ProjectileType.TargetingPlayerDir:
                    case ProjectileType.InfinityDuration:
                    case ProjectileType.InfinityDurationAroundPlayer:
                        if (isMove)
                        {
                            rb.velocity = moveDir.normalized * _moveSpeed;
                        }
                        else
                        {
                            rb.velocity = Vector2.zero;
                        }

                        break;
                    default:
                        break;
                }
 
                break;
        }

 

    }
    private float GenerateValueExcludingRange(float minValue, float maxValue, float excludedMin, float excludedMax)
    {
        float randomValue;
        do {
            randomValue = Random.Range(minValue, maxValue);
        } while (randomValue >= excludedMin && randomValue <= excludedMax);
        
        return randomValue;
    }
    private IEnumerator SetRandPositionRoutine()
    {
        
        float x = Random.Range(_minX, _maxX + 1);
        float y = Random.Range(_minY, _maxY + 1);
        switch (GameManager.contentsType)
        {
            case GameManager.ContentsType.TwelveDungeon:

                while (true)
                {
                    //직접이동
                    if (_bossId == 109 || _bossId == 110 ||
                        _bossId == 154 ||
                        _bossId == 272 || _bossId == 273
                       )
                    {
                        transform.localPosition = new Vector3(x, y, 0);
                    }

                    //투사체 날아감.
                    if (_bossId == 111 || _bossId == 112 ||
                        _bossId == 123 ||
                        _bossId == 133 || _bossId == 134 || _bossId == 135 ||
                        _bossId == 154 || _bossId == 155 || _bossId == 156 ||
                        _bossId == 274 || _bossId == 275 ||
                        _bossId == 280 || _bossId == 289 ||
                        _bossId == 290 || _bossId == 291
                       )
                    {
                        transform.localPosition = new Vector3(x, y, 0);
                        SetDir();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.transform.localPosition = Vector3.zero;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    //방향전환
                    if (_bossId == 119 ||
                        _bossId == 120 || _bossId == 121 || _bossId == 122 ||
                        _bossId == 136 || _bossId == 137 || _bossId == 138 || _bossId == 139 ||
                        _bossId == 276 || _bossId == 277 || _bossId == 278 || _bossId == 279)
                    {
                        SetDir();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    //방향전환
                    if (_bossId == 169 || _bossId == 170 || _bossId == 171 || _bossId == 172 || _bossId == 173)
                    {
                        SetDirFromRb();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    //플레이어 중심기준
                    if (_bossId == 177)
                    {
                        float distance = GenerateValueExcludingRange(_minX, _maxX, -1f, 1f);
                        transform.localPosition = new Vector3(playerTr.position.x + distance,
                            playerTr.position.y + distance, 0);
                        SetDirFromRb();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    yield return new WaitForSeconds(_delay + _startDelay);
                    _startDelay = 0;
                }

                break;
            case GameManager.ContentsType.SpecialRequestBoss:
                while (true)
                {
                    //직접이동
                    if (projectileType==ProjectileType.Projectile)
                    {
                        transform.localPosition = new Vector3(x, y, 0);
                    }

                    //투사체 날아감.
                    if (projectileType==ProjectileType.Projectile)
                    {
                        transform.localPosition = new Vector3(x, y, 0);
                        SetDir();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.transform.localPosition = Vector3.zero;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    //방향전환
                    if (projectileType==ProjectileType.TargetingPlayerDir)
                    {
                        SetDir();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    //구슬 계속 이동
                    if (projectileType==ProjectileType.InfinityDuration)
                    {
                        SetDirFromRb();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    //플레이어 중심기준
                    if (projectileType==ProjectileType.InfinityDurationAroundPlayer)
                    {
                        float distance = GenerateValueExcludingRange(_minX, _maxX, -1f, 1f);
                        transform.localPosition = new Vector3(playerTr.position.x + distance,
                            playerTr.position.y + distance, 0);
                        SetDirFromRb();
                        float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                        projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    yield return new WaitForSeconds(_delay + _startDelay);
                    _startDelay = 0;
                }
                break;

        }


    }
}
