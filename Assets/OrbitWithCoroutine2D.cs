using UnityEngine;
using System.Collections;

public class OrbitWithCoroutine2D : MonoBehaviour
{
    public Transform target; // 공전의 중심이 되는 게임 오브젝트
    public float orbitSpeed = 10.0f; // 공전 속도
    public float orbitDistance = 5.0f; // 공전 반경
    public Vector2 addPosition = Vector2.zero;

    private float angle = 0f;

    public bool isReverse = false;
    void Start()
    {
        if (target != null)
        {
            // 코루틴 시작
            StartCoroutine(OrbitRoutine());
        }
    }

    private IEnumerator OrbitRoutine()
    {
        while (true)
        {
            // 각도를 증가시켜 회전
            if (!isReverse)
            {
                angle += orbitSpeed * Time.deltaTime;
            }
            else
            {
                angle -= orbitSpeed * Time.deltaTime;
            }

            // 새로운 위치 계산
            float x = Mathf.Cos(angle) * orbitDistance;
            float y = Mathf.Sin(angle) * orbitDistance;

            // 목표 위치를 기준으로 공전
            transform.position = new Vector2(target.position.x + x+addPosition.x, target.position.y + y+addPosition.y);

            // 다음 프레임까지 대기
            yield return new WaitForSeconds(0.02f);
        }
    }
}