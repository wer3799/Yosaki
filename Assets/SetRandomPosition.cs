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

    private void OnEnable()
    {
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
        //조조
        if (_bossId == 111||_bossId == 112||_bossId == 123||_bossId == 133||_bossId == 134||_bossId == 135||_bossId==154||_bossId==155||_bossId==156||_bossId==169||_bossId==170||_bossId==171)
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

    }
    private IEnumerator SetRandPositionRoutine()
    {
        while (true)
        {
            float x = Random.Range(_minX, _maxX + 1);
            float y = Random.Range(_minY, _maxY + 1);
            //직접이동
            if (_bossId == 109 || _bossId == 110||_bossId==154)
            {
                transform.localPosition = new Vector3(x, y, 0);
            }
            //투사체 날아감.
            if (_bossId == 111||_bossId == 112 ||_bossId == 123||_bossId == 133||_bossId == 134||_bossId == 135||_bossId ==154||_bossId ==155||_bossId==156)
            {
                transform.localPosition = new Vector3(x, y, 0);
                SetDir();
                float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                projectileTr.transform.localPosition = Vector3.zero;
                projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
            //방향전환
            if (_bossId == 119||_bossId==120||_bossId==121||_bossId==122||_bossId == 136||_bossId==137||_bossId==138||_bossId==139)
            {
                SetDir();
                float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
            //방향전환
            if (_bossId==169||_bossId==170||_bossId==171)
            {
                SetDirFromRb();
                float angle = Mathf.Atan2(moveDir.normalized.y, moveDir.normalized.x) * Mathf.Rad2Deg;
                projectileTr.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
            
            yield return new WaitForSeconds(_delay+_startDelay);
            _startDelay = 0;
        }
    }
}
