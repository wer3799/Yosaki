using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;
using BackEnd;

public class UiMinigameBoard : SingletonMono<UiMinigameBoard>
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
    private GameObject hideObject;

    private ReactiveProperty<MiniGameState> gameState = new ReactiveProperty<MiniGameState>(MiniGameState.Wait);

    public MiniGameState GameState_Cur => gameState.Value;

    [SerializeField]
    private UiMiniGameBullet bulletPrefab;

    private ObjectPool<UiMiniGameBullet> bulletPool;

    private ReactiveProperty<int> currentHp = new ReactiveProperty<int>();

    private ReactiveProperty<float> elapsedTime = new ReactiveProperty<float>();

    [SerializeField]
    private TextMeshProUGUI remainSec;

    [SerializeField]
    private TextMeshProUGUI remainHp;

    [SerializeField]
    private Animator hpAnim;

    private void Start()
    {
        bulletPool = new ObjectPool<UiMiniGameBullet>(bulletPrefab, this.transform, 10);

        Subscribe();
    }

    private string playTriggerName = "play";
    private void Subscribe()
    {
        currentHp.AsObservable().Subscribe(e =>
        {
            remainHp.SetText($"남은 체력 : {e}");
            hpAnim.SetTrigger(playTriggerName);
        }).AddTo(this);

        elapsedTime.AsObservable().Subscribe(e =>
        {
            remainSec.SetText($"버틴시간 : {e.ToString("F2")}초");

        }).AddTo(this);

        gameState.AsObservable().Subscribe(e =>
        {
            hideObject.SetActive(e != MiniGameState.Playing);
            gameStartButton.SetActive(e != MiniGameState.Playing);

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
        Wait, Playing, End
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
        WaitForSeconds spawnDelay = new WaitForSeconds(0.6f);

        while (true)
        {
            int spawnAmount = (int)(elapsedTime.Value);

            spawnAmount = 2 + Mathf.Max(spawnAmount, 1);

            for (int i = 0; i < spawnAmount; i++)
            {
                SpawnBullet();
            }

            yield return spawnDelay;
        }
    }

    private void SpawnBullet()
    {
        var bullet = bulletPool.GetItem();

        var RandomSpawnPos = bulletSpawnPosits[UnityEngine.Random.Range(0, bulletSpawnPosits.Count)];

        Vector3 fireDir = Vector3.down;

        bullet.transform.position = RandomSpawnPos.transform.position;

        bullet.Initialize(fireDir, GetMoveSpeed());
    }

    private float GetMoveSpeed()
    {
        float ret = UnityEngine.Random.Range(3, 3 + (elapsedTime.Value / 4f));

        return ret;
    }

    private void ResetGame()
    {
        StopAllRoutines();

        player.transform.localPosition = Vector3.zero;

        elapsedTime.Value = 0f;

        currentHp.Value = GetMaxHp();

        bulletPool.DisableAllObject(); ;
    }

    private int GetMaxHp()
    {
        return Mathf.Max(1,(int)(ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value / 1000));
    }

    public void OnClickGameStartButton()
    {
        if (gameState.Value != MiniGameState.Playing)
        {
            gameState.Value = MiniGameState.Playing;
        }
    }
    public void PlayerDead()
    {
        gameState.Value = MiniGameState.End;
        ReportScore();
        player.transform.localPosition = Vector3.zero;
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
        RankManager.Instance.UpdateMiniGame_Score(elapsedTime.Value);

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"기록 : {elapsedTime.Value.ToString("F2")}초", null);
    }

    private void OnEnable()
    {
        gameState.Value = MiniGameState.Wait;

        RankManager.Instance.ResetMiniGameScore();
    }

    public void OnClickCloseButton()
    {
        ResetGame();

        this.gameObject.SetActive(false);
    }

    //보상정보
    public List<int> rewardType;
    public List<float> rewardAmount;

    public void OnClickGetRewardButton()
    {
        int rewardTicketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward).Value;

        if (rewardTicketNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.MiniGameReward)}이 부족합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();


        Param goodsParam = new Param();

        ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward).Value -= 1;

        goodsParam.Add(GoodsTable.MiniGameReward, ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward).Value);

        int randIdx = UnityEngine.Random.Range(0, rewardType.Count);

        Item_Type itemType = (Item_Type)rewardType[randIdx];

        float amount = rewardAmount[randIdx];

        switch (itemType)
        {
            case Item_Type.Ticket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += amount;
                    goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
                }
                break;
            case Item_Type.Marble:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += amount;
                    goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                }
                break;
            case Item_Type.RelicTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += amount;
                    goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
                }
                break;
            case Item_Type.PeachReal:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += amount;
                    goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
                }
                break;
            case Item_Type.GrowthStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                    goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
                }
                break;
        }

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(itemType)} {Utils.ConvertBigNum(rewardAmount[randIdx])}개 획득!", null);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              //  LogManager.Instance.SendLogType("Mini", "R", rewardTicketNum.ToString());
          });

    }
    public void OnClickGetNewRewardButton()
    {
        int rewardTicketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward2).Value;

        if (rewardTicketNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.MiniGameReward2)}이 부족합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();


        Param goodsParam = new Param();

        ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward2).Value -= 1;

        goodsParam.Add(GoodsTable.MiniGameReward2, ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward2).Value);

        int randIdx = UnityEngine.Random.Range(0, rewardType.Count);

        Item_Type itemType = (Item_Type)rewardType[randIdx];

        float amount = rewardAmount[randIdx];

        switch (itemType)
        {
            case Item_Type.Ticket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += amount;
                    goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
                }
                break;
            case Item_Type.Marble:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += amount;
                    goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                }
                break;
            case Item_Type.RelicTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += amount;
                    goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
                }
                break;
            case Item_Type.PeachReal:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += amount;
                    goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
                }
                break;
            case Item_Type.GrowthStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                    goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
                }
                break;
        }

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(itemType)} {Utils.ConvertBigNum(rewardAmount[randIdx])}개 획득!", null);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              //  LogManager.Instance.SendLogType("Mini", "R", rewardTicketNum.ToString());
          });

    }
    
    public void OnClickGetNewRewardAllRecieveButton()
    {
        int rewardTicketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward2).Value;

        if (rewardTicketNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.MiniGameReward2)}이 부족합니다.");
            return;
        }
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{rewardTicketNum} 회 사용하시겠습니까?", () =>
        {


            List<TransactionValue> transactions = new List<TransactionValue>();


            Param goodsParam = new Param();

            ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward2).Value -= rewardTicketNum;


            goodsParam.Add(GoodsTable.MiniGameReward2, ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward2).Value);

            var rewardArray = new Dictionary<Item_Type, float>();



            while (rewardTicketNum > 0)
            {
                rewardTicketNum--;

                int randIdx = UnityEngine.Random.Range(0, rewardType.Count);

                Item_Type itemType = (Item_Type)rewardType[randIdx];

                float amount = rewardAmount[randIdx];

                if (rewardArray.ContainsKey(itemType) == false)
                {
                    rewardArray.Add(itemType, 0f);
                }

                rewardArray[itemType] += amount;
            }

            using var e = rewardArray.GetEnumerator();

            string Description = string.Empty;

            while (e.MoveNext())
            {
                ServerData.AddLocalValue(e.Current.Key, e.Current.Value);
                ServerData.AddParam(goodsParam, e.Current.Key);
                Description += $"{CommonString.GetItemName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value)}개 획득!\n";
            }


            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, Description, null);
            });
        }, () => { });


    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward).Value++;
            ServerData.goodsTable.GetTableData(GoodsTable.MiniGameReward2).Value++;
        }
    }
#endif
}
