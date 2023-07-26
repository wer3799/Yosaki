using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGachaResultView;
using UniRx;

public class UiSkillGacha : MonoBehaviour
{
    private List<SkillTableData> skillTableDatas = new List<SkillTableData>();
    private List<float> probs = new List<float>();
    private List<GachaResultCellInfo> gachaResultCellInfos = new List<GachaResultCellInfo>();

    [SerializeField]
    private List<ObscuredInt> gachaAmount;

    [SerializeField]
    private List<ObscuredInt> gachaPrice;

    private ObscuredInt lastGachaIdx = 0;

    [SerializeField]
    private List<TextMeshProUGUI> gachaNumTexts;

    [SerializeField]
    private List<TextMeshProUGUI> priceTexts;

    [SerializeField]
    private TextMeshProUGUI freeButtonDesc;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Subscribe(e =>
        {
            freeButtonDesc.SetText(e == 0 ? "600번\n무료 뽑기!" : "내일 다시!");
        }).AddTo(this);
    }

    private void Initialize()
    {
        for (int i = 0; i < gachaNumTexts.Count; i++)
        {
            gachaNumTexts[i].SetText($"{gachaAmount[i]}번 소환");
        }

        for (int i = 0; i < priceTexts.Count; i++)
        {
            priceTexts[i].SetText($"{Utils.ConvertNum(gachaPrice[i])}");
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        WaitForSeconds randomizeDelay = new WaitForSeconds(1.0f);

        while (true)
        {
            Randomize();
            yield return randomizeDelay;
        }
    }

    private void Randomize()
    {
        gachaAmount.ForEach(e => e.RandomizeCryptoKey());
        gachaPrice.ForEach(e => e.RandomizeCryptoKey());
        lastGachaIdx.RandomizeCryptoKey();
    }

    private void OnApplicationPause(bool pause)
    {
        Randomize();
    }


    private bool CanGacha(float price)
    {
        float currentBlueStoneNum = ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;
        return currentBlueStoneNum >= price;
    }

    public void OnClickFreeGacha()
    {
        bool canFreeGacha = ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Value == 0;

        if (canFreeGacha == false)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받을 수 없습니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(() =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Value = 1;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();

            userInfoParam.Add(UserInfoTable.freeSkill, ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            int freeGachaIdx = 1;
            
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                this.lastGachaIdx = freeGachaIdx;
                int amount = gachaAmount[freeGachaIdx];
                int price = gachaPrice[freeGachaIdx];

                //무료라
                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += price;

                OnClickOpenButton(freeGachaIdx);

             //   LogManager.Instance.SendLogType("FreeGacha", "Skill", "");
            });
        });
    }

    public void OnClickOpenButton(int idx)
    {
        this.lastGachaIdx = idx;
        int amount = gachaAmount[idx];
        int price = gachaPrice[idx];

        //재화 체크
        if (CanGacha(price) == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            UiGachaResultView.Instance.autoToggle.isOn = false;
            return;
        }

        UiTutorialManager.Instance.SetClear(TutorialStep.GetSkill);

        this.skillTableDatas.Clear();
        probs.Clear();
        gachaResultCellInfos.Clear();

        var skillData = TableManager.Instance.SkillData;

        var e = skillData.GetEnumerator();

        int gachaLevel = UiGachaPopup.GachaLevel(UserInfoTable.gachaNum_Skill);

        while (e.MoveNext())
        {
            this.skillTableDatas.Add(e.Current.Value);

            if (gachaLevel == 0)
            {
                probs.Add(e.Current.Value.Gachalv1);
            }
            else if (gachaLevel == 1)
            {
                probs.Add(e.Current.Value.Gachalv2);
            }
            else if (gachaLevel == 2)
            {
                probs.Add(e.Current.Value.Gachalv3);
            }
            else if (gachaLevel == 3)
            {
                probs.Add(e.Current.Value.Gachalv4);
            }
            else if (gachaLevel == 4)
            {
                probs.Add(e.Current.Value.Gachalv5);
            }
            else if (gachaLevel == 5)
            {
                probs.Add(e.Current.Value.Gachalv6);
            }
            else if (gachaLevel == 6)
            {
                probs.Add(e.Current.Value.Gachalv7);
            }
            else if (gachaLevel == 7)
            {
                probs.Add(e.Current.Value.Gachalv8);
            }
            else if (gachaLevel == 8)
            {
                probs.Add(e.Current.Value.Gachalv9);
            }
            else if (gachaLevel == 9)
            {
                probs.Add(e.Current.Value.Gachalv10);
            }
        }

        //로컬 데이터 갱신
        //재화
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= price;

        //가챠갯수
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Skill).Value += amount;

        UiTutorialManager.Instance.SetClear(TutorialStep.GetSkill);

        //마법책
        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(probs);
            var cellInfo = new GachaResultCellInfo();

            cellInfo.amount = 1;
            cellInfo.skillData = this.skillTableDatas[randomIdx];
            gachaResultCellInfos.Add(cellInfo);

            ServerData.skillServerTable.UpdateSkillAmountLocal(this.skillTableDatas[randomIdx], cellInfo.amount);
        }

        SyncServer();

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.GachaSkillBook, amount);

        UiGachaResultView.Instance.Initialize(gachaResultCellInfos, () =>
        {
            OnClickOpenButton(lastGachaIdx);
        });

        SoundManager.Instance.PlaySound("Reward");
    }

    private void SyncServer()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

        //가챠횟수
        Param gachaNumParam = new Param();
        gachaNumParam.Add(UserInfoTable.gachaNum_Skill, ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Skill).Value);

        //스킬북
        Param skillParam = new Param();
        List<int> skillAmountSyncData = new List<int>();
        List<int> skillAlreadyHasSyncData = new List<int>();

        for (int i = 0; i < ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
            skillAlreadyHasSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAlreadyHas][i].Value);
        }

        skillParam.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);
        skillParam.Add(SkillServerTable.SkillAlreadyHas, skillAlreadyHasSyncData);

        //재화
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        //가챠횟수
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, gachaNumParam));
        //스킬
        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));

        ServerData.SendTransaction(transactionList);
    }
}
