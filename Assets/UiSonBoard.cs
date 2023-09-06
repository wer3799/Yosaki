using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiSonBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private UiSonRewardCell cellPrefab;

    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private TextMeshProUGUI sonLevelText;

    [SerializeField]
    private TextMeshProUGUI sonAbilText1;

    [SerializeField]
    private TextMeshProUGUI upgradePriceText;

    [SerializeField]
    private Image sonCharacterIcon;

    private List<UiSonRewardCell> rewardCells = new List<UiSonRewardCell>();

    [SerializeField]
    private GameObject sonSkillBoard;

    [SerializeField]
    private GameObject transGameObject;
    [SerializeField]
    private GameObject transDescObject;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Son_Level).AsObservable().Subscribe(level =>
        {
            sonLevelText.SetText($"LV : {Utils.ConvertNum(level)}");
            UpdateAbilText1(level);

            sonCharacterIcon.sprite = CommonUiContainer.Instance.sonThumbNail[GameBalance.GetSonIdx()];
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Peach).AsObservable().Subscribe(amount =>
        {
            upgradePriceText.SetText($"+{Utils.ConvertNum(amount)}");

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Ym).AsObservable().Subscribe(amount =>
        {
            UpdateAbilText1(ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value);
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).AsObservable().Subscribe(e =>
        {
            transGameObject.SetActive(e == 0);
            transDescObject.SetActive(e == 1);
        }).AddTo(this);
    }

    private void UpdateAbilText1(float currentLevel)
    {
        var tableData = TableManager.Instance.SonAbil.dataArray;

        string abilDesc = "보유 효과\n\n";

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentLevel < tableData[i].Unlocklevel) continue;

            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type.IsPercentStat() == false)
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetSonAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetSonAbilHasEffect(type) * 100f)}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        sonAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).Value >0) return;

        var tableData = TableManager.Instance.SonReward.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiSonRewardCell>(cellPrefab, cellParents);

            cell.Initialize(tableData[i]);

            rewardCells.Add(cell);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Son);
        }, () => { });
    }

    public void OnClickLevelUpButton()
    {
        float goodsNum = ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value;

        if (goodsNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.PeachReal)}가 없습니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value -= goodsNum;
        ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value += (int)goodsNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private int GetUpgradePrice()
    {
        return 1;
    }

    private Coroutine syncRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);
    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.Son_Level, ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              //    LogManager.Instance.SendLogType("Son", "Level", ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value.ToString());
          });
    }
    
    
    
    public void OnClickAllReceiveButton()
    {
        double score = ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin;

        var tableData = TableManager.Instance.SonReward.dataArray;

        var sonRewardedIdxList = ServerData.etcServerTable.GetSonRewardedIdxList();

        int rewardCount = 0;

        string addStringValue = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score < tableData[i].Score)
            {
                break;
            }
            else
            {
                if (sonRewardedIdxList.Contains(tableData[i].Id) == false)
                {
                    float amount = tableData[i].Rewardvalue;

                    addStringValue += $"{BossServerTable.rewardSplit}{tableData[i].Id}";

                    ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += (int)amount;

                    rewardCount++;
                }
            }
        }

        if (rewardCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.sonReward, ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                // LogManager.Instance.SendLogType("Son", "all", "");
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
        }

    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value = string.Empty;
            ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += 2000;
        }
    }
#endif

    public void OnClickSkillButton()
    {
        sonSkillBoard.SetActive(true);
    }
    public void OnClickTransButton()
    {


        if (ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin < GameBalance.sonGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"데미지 1갈 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                "각성시 부처님 손바닥에서 복숭아 획득이 더이상 불가능 합니다.\n" +
                $"대신 복숭아 보유효과가 강화 되고({PlayerStats.SonTransAddValue * 100}%)\n" +
                $"스테이지 일반 요괴 처치시 복숭아를 자동으로 획득 합니다.\n" +
                "각성 하시겠습니까??", () =>
              {

                  ServerData.userInfoTable.TableDatas[UserInfoTable.graduateSon].Value = 1;
                  ServerData.userInfoTable.UpData(UserInfoTable.graduateSon, false);
                  PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);

              }, null);
        }

    }
}
