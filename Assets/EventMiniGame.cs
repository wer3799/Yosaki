using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;
using BackEnd;
using Random = System.Random;

public class EventMiniGame : SingletonMono<EventMiniGame>
{
    [SerializeField]
    AbbObject abbObject;

    [SerializeField]
    private List<GameObject> bulletSpawnPosits;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject gameStartButton;

    [SerializeField]
    private GameObject jumpButton;
    
    [SerializeField]
    private GameObject hideObject;

    private ReactiveProperty<MiniGameState> gameState = new ReactiveProperty<MiniGameState>(MiniGameState.Wait);

    public MiniGameState GameState_Cur => gameState.Value;

    [SerializeField]
    private UiMiniGameBullet bulletPrefab;

    [SerializeField]
    private Transform bulletParents;

    private ObjectPool<UiMiniGameBullet> bulletPool;

    private ReactiveProperty<int> currentHp = new ReactiveProperty<int>();

    private ReactiveProperty<float> elapsedTime = new ReactiveProperty<float>();

    [SerializeField]
    private TextMeshProUGUI remainSec;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float jumpPower = 10f;

    private int currentJumpCount = 0;

    private void Start()
    {
        bulletPool = new ObjectPool<UiMiniGameBullet>(bulletPrefab, bulletParents, 10);

        Subscribe();
    }

    private string playTriggerName = "play";

    private void Subscribe()
    {
        elapsedTime.AsObservable().Subscribe(e => { remainSec.SetText($"버틴시간 : {e.ToString("F2")}초"); }).AddTo(this);

        gameState.AsObservable().Subscribe(e =>
        {
            hideObject.SetActive(e != MiniGameState.Playing);
            gameStartButton.SetActive(e != MiniGameState.Playing);
            jumpButton.SetActive(e == MiniGameState.Playing);

            switch (e)
            {
                case MiniGameState.Wait:
                {
                    ResetGame();
                }
                    break;
                case MiniGameState.Playing:
                {
                    abbObject.enabled = false;

                    ResetGame();

                    if (gameRoutine != null)
                    {
                        StopCoroutine(gameRoutine);
                    }

                    gameRoutine = StartCoroutine(GameRoutine());

                    if (bulletSpawnRoutine != null)
                    {
                        StopCoroutine(bulletSpawnRoutine);
                    }

                    bulletSpawnRoutine = StartCoroutine(BulletSpawnRoutine());
                }
                    break;
                case MiniGameState.End:
                {
                    abbObject.enabled = true;

                    StopAllRoutines();
                }
                    break;
            }
        }).AddTo(this);
    }

    private void StopAllRoutines()
    {
        if (gameRoutine != null)
        {
            StopCoroutine(gameRoutine);
        }

        if (bulletSpawnRoutine != null)
        {
            StopCoroutine(bulletSpawnRoutine);
        }
    }

    public enum MiniGameState
    {
        Wait,
        Playing,
        End
    }

    private Coroutine gameRoutine;


    private IEnumerator GameRoutine()
    {
        while (true)
        {
            elapsedTime.Value += Time.deltaTime;
            yield return null;
        }
    }

    private Coroutine bulletSpawnRoutine;

    private IEnumerator BulletSpawnRoutine()
    {
        while (true)
        {
            int spawnAmount = 1;

            for (int i = 0; i < spawnAmount; i++)
            {
                SpawnBullet();
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2.2f));
        }
    }

    private void SpawnBullet()
    {
        var bullet = bulletPool.GetItem();

        var RandomSpawnPos = bulletSpawnPosits[UnityEngine.Random.Range(0, bulletSpawnPosits.Count)];

        Vector3 fireDir = Vector3.left;

        bullet.transform.position = RandomSpawnPos.transform.position;

        bullet.transform.localScale = new Vector3(1f, UnityEngine.Random.Range(2f, 6.2f), 1f);

        bullet.Initialize(fireDir, GetMoveSpeed());
    }

    private float GetMoveSpeed()
    {
        float addTime = Mathf.Min(elapsedTime.Value, 100);
        
        float ret = 9f + addTime * 0.075f;
        
        return ret;
    }

    private void ResetGame()
    {
        StopAllRoutines();

        player.transform.localPosition = new Vector3(-280f, 0f, 0f);

        elapsedTime.Value = 0f;

        currentHp.Value = GetMaxHp();

        bulletPool.DisableAllObject();

        rb.velocity = Vector2.zero;

        currentJumpCount = 2;
    }

    private int GetMaxHp()
    {
        //체력은 1로 고정
        return 1;
    }

    public void OnClickGameStartButton()
    {
        ResetGame();
        
        if (gameState.Value != MiniGameState.Playing)
        {
            gameState.Value = MiniGameState.Playing;
        }
    }

    public void PlayerDead()
    {
        gameState.Value = MiniGameState.End;
        ReportScore();
        player.transform.localPosition = new Vector3(-280f, 0f, 0f);
        bulletPool.DisableAllObject();
    }

    public void PlayerDamaged()
    {
        if (gameState.Value != MiniGameState.Playing) return;

        currentHp.Value--;

        if (currentHp.Value <= 0)
        {
            PlayerDead();
        }
    }

    private void ReportScore()
    {
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param param = new Param();

        float prefTop = (float)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventMiniGameScore_TopRate].Value;

        float current = elapsedTime.Value;

        if (current > prefTop)
        {
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventMiniGameScore_TopRate].Value = elapsedTime.Value;
            param.Add(UserInfoTable_2.eventMiniGameScore_TopRate, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventMiniGameScore_TopRate].Value);
        }

        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventMiniGameScore_Total].Value += elapsedTime.Value;
        param.Add(UserInfoTable_2.eventMiniGameScore_Total, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventMiniGameScore_Total].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, param));

        ServerData.SendTransaction(transactions);

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"기록 : {elapsedTime.Value.ToString("F2")}초", null);
    }

    private void OnEnable()
    {
        gameState.Value = MiniGameState.Wait;
    }

    private void OnDisable()
    {
        gameState.Value = MiniGameState.Wait;
    }

    public void OnClickJumpButton()
    {
        if (gameState.Value != MiniGameState.Playing) return;

        //2번까지 점프
        if (currentJumpCount >= 2) return;

        currentJumpCount++;

        rb.velocity = Vector2.zero;

        rb.AddForce(Vector2.up * jumpPower);
    }

    public void ResetJumpCount()
    {
        currentJumpCount = 0;
    }
}