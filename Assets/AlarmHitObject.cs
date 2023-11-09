using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlarmHitObject : MonoBehaviour
{
    private double damage = 10;

    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float delay = 0f;
    
    [SerializeField]
    private Animator animator;

    private Coroutine _coroutine;
    public void AttackStart()
    {

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(startDelay+delay);;
        this.gameObject.SetActive(true);
        animator.SetTrigger("Attack");
        startDelay = 0f;
    }
    
    float percentDamage = 0f;

    public void SetDamage(double damage, float percentDamage = 0f)
    {
        this.damage = damage;
        this.percentDamage = percentDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals(Tags.Player) == false) return;

        PlayerStatusController.Instance.UpdateHp(-damage, percentDamage);
    }

    public void Shuffle()
    {
        float randomXIndex = Random.Range(8, 23);
        float randY = Random.Range(0, 3);
        transform.localPosition = new Vector3(randomXIndex, -7.87f + randY * 5.62f, 0);
    }

    public void MoveToPlayer()
    {
        int bossId = GameManager.Instance.bossId;
        if (bossId == 146||bossId == 147||bossId == 148||bossId == 149||bossId == 162||bossId == 163||bossId == 164||bossId == 165||bossId == 174||bossId == 175||bossId == 178||bossId == 179||bossId == 181||bossId == 183)
        {
            transform.Rotate(0, 0, 90);
            transform.position = PlayerMoveController.Instance.transform.position;
        }
        else if (bossId == 185)
        {
            transform.position = PlayerMoveController.Instance.transform.position;
        }
        else if (bossId == 167||bossId==168)
        {
            var addPostion = new Vector3(Random.Range(-3, 4), Random.Range(-3, 4), 0);
            transform.position = PlayerMoveController.Instance.transform.position + addPostion;
        }
        else if (bossId == 166)
        {
            var addPostion = new Vector3(Random.Range(-3, 4), Random.Range(-3, 4), 0);
            transform.position = PlayerMoveController.Instance.transform.position + addPostion;
            transform.Rotate(0, 0, 90);
        }
    }

    private bool isMove = false;
    private Coroutine movementCoroutine;
    public float movementSpeed = 20f; // 이동 속

    public void StartMovement()
    {
        isMove = true;
        movementCoroutine = StartCoroutine(MoveToTarget());
    }
    public void StartSatelliteAttack()
    {
        isMove = true;
        movementCoroutine = StartCoroutine(SatelliteAttackRoutine());
    }
    public void StartHorizonSatelliteAttack()
    {
        isMove = true;
        movementCoroutine = StartCoroutine(HorizonSatelliteAttackRoutine());
    }
    public void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        isMove = false;
    }

    IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        Vector3 target = PlayerMoveController.Instance.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            if (!isMove)
            {
                yield break; // 이동 멈춤 시 Coroutine 종료
            }

            elapsedTime += Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(startPosition, target, elapsedTime);
            yield return null;
        }

        // 이동 완료
        isMove = false;
    }
    IEnumerator SatelliteAttackRoutine()
    {
        Vector3 startPosition = Vector3.zero;
        startPosition += new Vector3(transform.position.x, 0, 0);
        
        Vector3 targetPosition = Vector3.zero;
        targetPosition += new Vector3(PlayerMoveController.Instance.transform.position.x, 0, 0);
        
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            if (!isMove)
            {
                yield break; // 이동 멈춤 시 Coroutine 종료
            }

            elapsedTime += Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            yield return null;
        }

        // 이동 완료
        isMove = false;
    }
    IEnumerator HorizonSatelliteAttackRoutine()
    {
        Vector3 startPosition = Vector3.zero;
        startPosition += new Vector3(0, transform.position.y, 0);
        
        Vector3 targetPosition = Vector3.zero;
        targetPosition += new Vector3(0, PlayerMoveController.Instance.transform.position.y, 0);
        
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            if (!isMove)
            {
                yield break; // 이동 멈춤 시 Coroutine 종료
            }

            elapsedTime += Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            yield return null;
        }

        // 이동 완료
        isMove = false;
    }
}
