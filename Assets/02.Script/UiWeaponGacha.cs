using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGachaResultView;
using UniRx;

public class UiWeaponGacha : MonoBehaviour
{
    private List<WeaponData> weaponDatas = new List<WeaponData>();
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

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Subscribe(e =>
        {
            freeButtonDesc.SetText(e == 0 ? "600번\n무료 뽑기!" : "내일 다시!");
        }).AddTo(this);
    }

    private void Start()
    {
        Initialize();
        Subscribe();
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
        bool canFreeGacha = ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value == 0;

        if (canFreeGacha == false)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받을 수 없습니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(() =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value = 1;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();

            userInfoParam.Add(UserInfoTable.freeWeapon, ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                this.lastGachaIdx = 1;
                int amount = gachaAmount[1];
                int price = gachaPrice[1];

                //무료라
                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += price;

                OnClickOpenButton(1);

               // LogManager.Instance.SendLogType("FreeGacha", "Weapon", "");
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
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(Item_Type.Jade),JongsungType.Type_IGA)} 부족합니다.");

            UiGachaResultView.Instance.autoToggle.isOn = false;
            return;
        }

        UiTutorialManager.Instance.SetClear(TutorialStep.GetWeapon);

        weaponDatas.Clear();
        probs.Clear();
        gachaResultCellInfos.Clear();

        var weaponTable = TableManager.Instance.WeaponData;

        var e = weaponTable.GetEnumerator();

        int gachaLevel = UiGachaPopup.GachaLevel(UserInfoTable.gachaNum_Weapon);

        while (e.MoveNext())
        {
            weaponDatas.Add(e.Current.Value);

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
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Weapon).Value += amount;

        //무기
        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(probs);
            var cellInfo = new GachaResultCellInfo();

            cellInfo.amount = 1;
            cellInfo.weaponData = weaponDatas[randomIdx];
            gachaResultCellInfos.Add(cellInfo);

            ServerData.weaponTable.UpData(weaponDatas[randomIdx], cellInfo.amount);
            serverUpdateList.Add(weaponDatas[randomIdx].Id);
        }

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine(serverUpdateList, price, serverUpdateList.Count));
        
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.GachaWeapon, amount);

        UiGachaResultView.Instance.Initialize(gachaResultCellInfos, () =>
        {
            OnClickOpenButton(lastGachaIdx);
        });

        //Debug.LogError($"{amount}개 뽑기!");
        
        SoundManager.Instance.PlaySound("Reward");

        //  UiTutorialManager.Instance.SetClear(TutorialStep._10_GetWeaponInShop);
    }

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.4f);
    public IEnumerator SyncRoutine(List<int> serverUpdateList, int price, int gachaCount)
    {
        yield return syncDelay;
        SyncServer(serverUpdateList, price, gachaCount);
    }
    //서버 갱신만
    private void SyncServer(List<int> serverUpdateList, int price, int gachaCount)
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

        //가챠횟수
        Param gachaNumParam = new Param();
        gachaNumParam.Add(UserInfoTable.gachaNum_Weapon, ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Weapon).Value);

        //무기
        Param weaponParam = new Param();
        var table = TableManager.Instance.WeaponTable.dataArray;
        var tableDatas = ServerData.weaponTable.TableDatas;
        for (int i = 0; i < table.Length; i++)
        {
            if (serverUpdateList != null && serverUpdateList.Contains(table[i].Id) == false) continue;

            string key = table[i].Stringid;
            //hasitem 1
            weaponParam.Add(key, tableDatas[key].ConvertToString());
        }
        //

        //재화
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        //가챠횟수
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, gachaNumParam));
        //무기
        transactionList.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

        ServerData.SendTransactionV2(transactionList);
    }
}
