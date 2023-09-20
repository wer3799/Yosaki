using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiHellFireBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private UiHellRewardCell cellPrefab;

    [SerializeField] private Transform cellParents;

    [SerializeField] private TextMeshProUGUI sonLevelText;

    [SerializeField] private TextMeshProUGUI sonAbilText1;

    private List<UiHellRewardCell> rewardCells = new List<UiHellRewardCell>();

    [SerializeField] private GameObject transGameObject;
    [SerializeField] private GameObject transDescObject;

    [SerializeField] private TextMeshProUGUI hellAwakeAbilDescription;


    private void Start()
    {
        Initialize();
        Subscribe();
    }


    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Hel).AsObservable().Subscribe(level =>
        {
            sonLevelText.SetText($"LV : {level}");
            UpdateAbilText1((int)level);
        }).AddTo(this);


        ServerData.goodsTable.GetTableData(GoodsTable.HellPowerUp).AsObservable().Subscribe(level =>
        {
            UpdateAbilText1((int)ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).AsObservable().Subscribe(e =>
        {
            transGameObject.SetActive(e == 0);
            transDescObject.SetActive(e == 1);
        }).AddTo(this);
    }

    private void UpdateAbilText1(int currentLevel)
    {
        var tableData = TableManager.Instance.hellAbilBase.dataArray;

        string abilDesc = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            StatusType type = (StatusType)tableData[i].Abiltype;

            if (type == StatusType.AttackAddPer)
            {
                abilDesc +=
                    $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(PlayerStats.GetHellAbilHasEffect(type))}\n";
            }
            else
            {
                abilDesc += $"{CommonString.GetStatusName(type)} {PlayerStats.GetHellAbilHasEffect(type) * 100f}\n";
            }
        }

        abilDesc.Remove(abilDesc.Length - 2, 2);

        sonAbilText1.SetText(abilDesc);
    }

    private void Initialize()
    {
        hellAwakeAbilDescription.SetText(($"지옥불꽃 효과 {PlayerStats.HelTransAddValue * 100f}% 강화됨"));

        
        scoreText.SetText(
            $"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.hellScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).Value > 0) return;

        var tableData = TableManager.Instance.hellReward.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiHellRewardCell>(cellPrefab, cellParents);

            cell.Initialize(tableData[i]);

            rewardCells.Add(cell);
        }
    }
    
    
    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Hell);
        }, () => { });
    }

    public void OnClickAllReceiveButton()
    {
        double score = ServerData.userInfoTable.TableDatas[UserInfoTable.hellScore].Value *
                       GameBalance.BossScoreConvertToOrigin;

        var tableData = TableManager.Instance.hellReward.dataArray;

        var sonRewardedIdxList = ServerData.etcServerTable.GetHellRewardedIdxList();

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

                    ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value += (int)amount;

                    rewardCount++;
                }
            }
        }

        if (rewardCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.hellReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION5, 1);
            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearHell, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearHell, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
        }

        /////////////

        //bool hasreward = false;
        //for (int i = 0; i < rewardCells.Count; i++)
        //{
        //    hasreward |= rewardCells[i].OnClickGetButtonByScript();
        //}

        //if (hasreward)
        //{
        //    List<TransactionValue> transactions = new List<TransactionValue>();

        //    Param rewardParam = new Param();

        //    rewardParam.Add(EtcServerTable.hellReward, ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value);

        //    transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

        //    Param goodsParam = new Param();
        //    goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
        //    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //    EventMissionManager.UpdateEventMissionClear(EventMissionKey.ClearHell, 1);
        //    ServerData.SendTransaction(transactions, successCallBack: () =>
        //    {
        //        //  LogManager.Instance.SendLogType("Son", "all", "");
        //        PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
        //        SoundManager.Instance.PlaySound("Reward");
        //    });
        //}
        //else
        //{
        //    PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
        //}
    }

    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.hellScore].Value * GameBalance.BossScoreConvertToOrigin <
            GameBalance.helGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"데미지 1000갈 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                "각성시 지옥불꽃에서 불멸석 획득이 더이상 불가능 합니다.\n" +
                $"대신 불멸석 보유효과가 강화 되고({PlayerStats.HelTransAddValue * 100}%)\n" +
                $"스테이지 일반 요괴 처치시 불멸석을 자동으로 획득 합니다.\n" +
                "각성 하시겠습니까??", () =>
                {
                    ServerData.userInfoTable.TableDatas[UserInfoTable.graduateHel].Value = 1;
                    ServerData.userInfoTable.UpData(UserInfoTable.graduateHel, false);
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
                }, null);
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value += 2000;
        }
    }
#endif
}