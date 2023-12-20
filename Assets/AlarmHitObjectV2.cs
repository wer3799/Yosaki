using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
public enum PatternShape
{
    Circle,
    Square,
    Rectangle
}
public enum PatternType
{
    Normal,
    AttackByPlayerTransform,
    ChaseByPlayerTransform,
}

public class AlarmHitObjectV2 : AlarmHitObject
{

    
    [SerializeField] private float startScale = 1f;
    [SerializeField] private float endScale = 2f;
    
    [SerializeField] private GameObject alarmObject;

    [SerializeField] private GameObject predictAlarmObject;
    [SerializeField] private List<ParticleSystem> _particleSystems;

    [SerializeField] private CircleCollider2D _collider2D;

    [SerializeField] private BoxCollider2D _boxCollider2D;

    [FormerlySerializedAs("_patternType")] [SerializeField]
    private PatternShape _patternShape;

    [SerializeField] private PatternType patternType;

    private string AnimationStopKey = "Stop";
    private string AnimationAttackKey = "Attack";


    private bool isCurve = false;

    private Vector3 initPos;

    Vector3[] m_points = new Vector3[4];

    private float m_timerMax = 0;
    private float m_timerCurrent = 0;

    private Vector2 dir;

    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float predictTime = 0.7f;

    [SerializeField] [Range(0.2f, 10f)] private float duration = 0.2f;
    private float animationDelay = 0.1f;

    private Transform playerTransform;

    private void Start()
    {
        initPos = transform.localPosition;
    }

    private void Update()
    {
        if (isCurve)
        {
            if (m_timerCurrent > m_timerMax)
            {
                return;
            }

            // 경과 시간 계산.
            m_timerCurrent += Time.deltaTime * moveSpeed;

            // 베지어 곡선으로 X,Y,Z 좌표 얻기.
            transform.position = new Vector3(
                CubicBezierCurve(m_points[0].x, m_points[1].x, m_points[2].x, m_points[3].x),
                CubicBezierCurve(m_points[0].y, m_points[1].y, m_points[2].y, m_points[3].y),
                CubicBezierCurve(m_points[0].z, m_points[1].z, m_points[2].z, m_points[3].z)
            );
        }


        if (isMove)
        {
            transform.Translate(SetDirToPlayer() * (moveSpeed * Time.deltaTime));
        }
        else
        {
        }
    }

    private void InitPosition()
    {
        transform.localPosition = initPos;
    }

    private void SetTarget()
    {
        if (playerTransform == null)
        {
            playerTransform = PlayerSkillCaster.Instance.PlayerMoveController.transform;
        }
    }

    private Vector2 SetDirToPlayer()
    {
        return playerTransform.position - transform.position;
    }

    private void SetBezier(Transform _startTr, Transform _endTr, float _speed, float _newPointDistanceFromStartTr,
        float _newPointDistanceFromEndTr)
    {
        moveSpeed = _speed;

        // 끝에 도착할 시간을 랜덤으로 줌.
        m_timerMax = Random.Range(0.7f, 0.9f) * moveSpeed;

        // 시작 지점.
        m_points[0] = _startTr.position;

        // 시작 지점을 기준으로 랜덤 포인트 지정.
        m_points[1] = _startTr.position +
                      (_newPointDistanceFromStartTr * Random.Range(-1.0f, 1.0f) * _startTr.right) + // X (좌, 우 전체)
                      (_newPointDistanceFromStartTr * Random.Range(-0.15f, 1.0f) * _startTr.up) + // Y (아래쪽 조금, 위쪽 전체)
                      (_newPointDistanceFromStartTr * Random.Range(-1.0f, -0.8f) * _startTr.forward); // Z (뒤 쪽만)

        // 도착 지점을 기준으로 랜덤 포인트 지정.
        m_points[2] = _endTr.position +
                      (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.right) + // X (좌, 우 전체)
                      (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.up) + // Y (위, 아래 전체)
                      (_newPointDistanceFromEndTr * Random.Range(0.8f, 1.0f) * _endTr.forward); // Z (앞 쪽만)

        // 도착 지점.
        m_points[3] = _endTr.position;

        transform.position = _startTr.position;

    }

    public override void AttackStart()
    {
        switch (_patternShape)
        {
            case PatternShape.Circle:
                _collider2D.radius = endScale / 2; // 반지름
                break;
            case PatternShape.Square:
                _boxCollider2D.size = Vector2.one * endScale; // 반지름
                break;
            case PatternShape.Rectangle:
                _boxCollider2D.size = new Vector3(endScale, startScale, 1f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        this.gameObject.SetActive(true);
        switch (patternType)
        {
            case PatternType.Normal:
                StartAnimation();
                break;
            case PatternType.AttackByPlayerTransform:
                StartAnimationFromPlayerTransform();
                break;
            case PatternType.ChaseByPlayerTransform:
                StartSatelliteBeamAnimation();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public override void SetDamage(double damage, float percentDamage = 0f)
    {
        this.damage = damage;
        this.percentDamage = percentDamage;
        switch (_patternShape)
        {
            case PatternShape.Circle:
            case PatternShape.Square:
                predictAlarmObject.transform.localScale = new Vector3(endScale, endScale, 1f);
                break;
            case PatternShape.Rectangle:
                predictAlarmObject.transform.localScale = new Vector3(endScale, startScale, 1f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    private Coroutine animation;

    //일반 패턴
    public void StartAnimation()
    {
        if (animation != null)
        {
            StopCoroutine(animation);
        }

        animation = StartCoroutine(nameof(PlayAnimation));
    }

    //일반 패턴
    public void StartAnimationFromPlayerTransform()
    {
        if (animation != null)
        {
            StopCoroutine(animation);
        }

        animation = StartCoroutine(nameof(PlayAnimationFromPlayerTransform));
    }

    //새틀라이트 빔 패턴
    public void StartSatelliteBeamAnimation()
    {
        if (animation != null)
        {
            StopCoroutine(animation);
        }

        animation = StartCoroutine(nameof(PlaySatelliteBeamAnimation));
    }

    //닌자공격
    public void StartNinjaAttackAnimation()
    {
        if (animation != null)
        {
            StopCoroutine(animation);
        }

        animation = StartCoroutine(nameof(PlayNinjaAttackAnimation));
    }

    private void PlayScaleAnimation()
    {
        switch (_patternShape)
        {
            case PatternShape.Circle:
                alarmObject.transform.localScale = new Vector3(startScale, startScale, 1f);
                predictAlarmObject.SetActive(true);
                alarmObject.SetActive(true);
                LeanTween.scale(alarmObject, Vector3.one * endScale, predictTime);
                break;
            case PatternShape.Square:
                alarmObject.transform.localScale = new Vector3(endScale, 0, 1f);
                predictAlarmObject.SetActive(true);
                alarmObject.SetActive(true);

                LeanTween.scale(alarmObject, Vector3.one * endScale, predictTime);
                break;
            //직사각형은 가로 end 세로 start
            case PatternShape.Rectangle:
                alarmObject.transform.localScale = new Vector3(endScale, 0, 1f);
                predictAlarmObject.SetActive(true);
                alarmObject.SetActive(true);
               LeanTween.scale(alarmObject, new Vector3(endScale, startScale, 1f), predictTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AfterAnimation()
    {
        alarmObject.transform.localScale = Vector3.zero;

        StopCoroutine(animation);
    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(startDelay);

        PlayScaleAnimation();
        yield return new WaitForSeconds(predictTime);
        animator.SetTrigger(AnimationAttackKey);

        alarmObject.SetActive(false);
        predictAlarmObject.SetActive(false);

        var e = _particleSystems.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(true);
            e.Current.Play();
        }

        yield return new WaitForSeconds(duration - animationDelay);
        animator.SetTrigger(AnimationStopKey);
        yield return new WaitForSeconds(animationDelay);
        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayAnimationFromPlayerTransform()
    {
        SetTarget();
        yield return new WaitForSeconds(startDelay);

        transform.position = playerTransform.position;
        PlayScaleAnimation();
        yield return new WaitForSeconds(predictTime);
        animator.SetTrigger(AnimationAttackKey);

        alarmObject.SetActive(false);
        predictAlarmObject.SetActive(false);

        var e = _particleSystems.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(true);
            e.Current.Play();
        }

        yield return new WaitForSeconds(duration - animationDelay);
        animator.SetTrigger(AnimationStopKey);
        yield return new WaitForSeconds(animationDelay);
        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(false);
        }

    }

    private IEnumerator PlaySatelliteBeamAnimation()
    {
        SetTarget();
        yield return new WaitForSeconds(startDelay);
        InitPosition();

        //추적
        isMove = true;
        PlayScaleAnimation();
        int chaseCount = 7;
        for (int i = 0; i < chaseCount; i++)
        {
            SetDirToPlayer();

            yield return new WaitForSeconds(0.1f);
        }

        isMove = false;
        //추적끝 이펙트시작

        animator.SetTrigger(AnimationAttackKey);

        alarmObject.SetActive(false);
        predictAlarmObject.SetActive(false);

        var e = _particleSystems.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(true);
            e.Current.Play();
        }


        yield return new WaitForSeconds(0.13f);
        //데미지 발생
        yield return new WaitForSeconds(duration - animationDelay);

        animator.SetTrigger(AnimationStopKey);

        yield return new WaitForSeconds(animationDelay);

        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(false);
        }
        //this.gameObject.SetActive(false);


    }

    private IEnumerator PlayNinjaAttackAnimation()
    {
        InitPosition();
        SetTarget();
        SetBezier(this.gameObject.transform, playerTransform, moveSpeed, 6f, 3f);
        isMove = false;
        isCurve = true;
        m_timerCurrent = 0f;
        PlayScaleAnimation();

        while (true)
        {
            animator.SetTrigger(AnimationAttackKey);

            yield return new WaitForSeconds(3f);
            alarmObject.SetActive(false);
            predictAlarmObject.SetActive(false);
        }

        yield return null;
    }

    private float CubicBezierCurve(float a, float b, float c, float d)
    {
        float t = m_timerCurrent / moveSpeed;

        // 이해한대로 편하게 쓰면.
        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals(Tags.Player, StringComparison.OrdinalIgnoreCase) == false) return;

        PlayerStatusController.Instance.UpdateHp(-damage, percentDamage);

    }
}