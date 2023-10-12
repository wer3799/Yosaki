using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGachaResultView;
using UniRx;

public class UiMagicBookGacha : MonoBehaviour
{
    private List<MagicBookData> magicBookDatas = new List<MagicBookData>();
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
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Subscribe(e =>
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

    public void OnClickFreeGacha()
    {
        bool canFreeGacha = ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Value == 0;

        if (canFreeGacha == false)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받을 수 없습니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(() =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Value = 1;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();

            userInfoParam.Add(UserInfoTable.freeNorigae, ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                this.lastGachaIdx = 1;
                int amount = gachaAmount[1];
                int price = gachaPrice[1];

                //무료라
                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += price;

                OnClickOpenButton(1);

               // LogManager.Instance.SendLogType("FreeGacha", "Norigae", "");
            });
        });


    }


    private bool CanGacha(float price)
    {
        float currentBlueStoneNum = ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;
        return currentBlueStoneNum >= price;
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

        this.magicBookDatas.Clear();
        probs.Clear();
        gachaResultCellInfos.Clear();

        var magicBookData = TableManager.Instance.MagicBoocDatas;

        var e = magicBookData.GetEnumerator();

        int gachaLevel = UiGachaPopup.GachaLevel(UserInfoTable.gachaNum_Norigae);

        while (e.MoveNext())
        {
            this.magicBookDatas.Add(e.Current.Value);

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

        List<int> serverUpdateList = new List<int>();

        //로컬 데이터 갱신
        //재화
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= price;

        //가챠갯수
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Norigae).Value += amount;

        //마법책
        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(probs);
            var cellInfo = new GachaResultCellInfo();

            cellInfo.amount = 1;
            cellInfo.magicBookData = this.magicBookDatas[randomIdx];
            gachaResultCellInfos.Add(cellInfo);

            ServerData.magicBookTable.UpData(this.magicBookDatas[randomIdx], cellInfo.amount);
            serverUpdateList.Add(magicBookDatas[randomIdx].Id);
        }

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine(serverUpdateList, price, serverUpdateList.Count));
        //gachaResultCellInfos.Sort((a, b) =>
        //{
        //    if (a.magicBookData.Grade < b.magicBookData.Grade)
        //        return -1;

        //    return 1;

        //});

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.GachaMagicBook, amount);

        UiGachaResultView.Instance.Initialize(gachaResultCellInfos, () =>
         {
             OnClickOpenButton(lastGachaIdx);
         });

        SoundManager.Instance.PlaySound("Reward");
    }
    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.4f);
    public IEnumerator SyncRoutine(List<int> serverUpdateList, int price, int gachaCount)
    {
        yield return syncDelay;
        SyncServer(serverUpdateList, price, gachaCount);
    }
    private void SyncServer(List<int> serverUpdateList, int price, int gachaCount)
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

        //가챠횟수
        Param gachaNumParam = new Param();
        gachaNumParam.Add(UserInfoTable.gachaNum_Norigae, ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Norigae).Value);

        //마법책
        Param magicBookParam = new Param();
        var table = TableManager.Instance.MagicBookTable.dataArray;
        var tableDatas = ServerData.magicBookTable.TableDatas;
        for (int i = 0; i < table.Length; i++)
        {
            if (serverUpdateList != null && serverUpdateList.Contains(table[i].Id) == false) continue;

            string key = table[i].Stringid;
            //hasitem 1
            magicBookParam.Add(key, tableDatas[key].ConvertToString());
        }
        //

        //재화
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        //가챠횟수
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, gachaNumParam));
        //마법책
        transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

        ServerData.SendTransactionV2(transactionList);
    }
}
