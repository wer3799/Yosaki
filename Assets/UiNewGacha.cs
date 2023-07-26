using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGachaResultView;
using UniRx;

public class UiNewGacha : MonoBehaviour
{
    private List<NewGachaTableData> newGachaTableDatas = new List<NewGachaTableData>();
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

    [SerializeField]
    private TextMeshProUGUI getRingDesc;

    [SerializeField]
    private GameObject getRingLockMask;

    public TMP_InputField inputField;
    
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Subscribe(e => { freeButtonDesc.SetText(e == 0 ? "100번\n무료 뽑기!" : "내일 다시!"); }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Subscribe(e =>
        {
            if (e >= GameBalance.GraduateSoulRing)
            {
                getRingLockMask.SetActive(false);
                getRingDesc.SetText($"뽑은 횟수 : {Utils.ConvertBigNum(e - GameBalance.GraduateSoulRing)}");
            }
            else
            {
                getRingLockMask.SetActive(true);
                getRingDesc.SetText($"-");
            }
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
            priceTexts[i].SetText($"{gachaPrice[i]}");
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
        float currentBlueStoneNum = ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value;
        return currentBlueStoneNum >= price;
    }

    public void OnClickFreeGacha()
    {
        bool canFreeGacha = ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value == 0;

        if (canFreeGacha == false)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받을 수 없습니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(() =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value = 1;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();

            userInfoParam.Add(UserInfoTable.freeNewGacha, ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                this.lastGachaIdx = 1;
                int amount = gachaAmount[1];
                int price = gachaPrice[1];

                //무료라
                ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += price;

                OnClickOpenButton(1);
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
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}이 부족합니다.");
            UiGachaResultView.Instance.autoToggle.isOn = false;
            return;
        }

        this.newGachaTableDatas.Clear();
        probs.Clear();
        gachaResultCellInfos.Clear();

        var newGachaData = TableManager.Instance.NewGachaData;

        var e = newGachaData.GetEnumerator();

        int gachaLevel = UiGachaPopup.GachaLevel(UserInfoTable.gachaNum_NewGacha);

        while (e.MoveNext())
        {
            this.newGachaTableDatas.Add(e.Current.Value);

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
        ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value -= price;

        //가챠갯수
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value += amount;


        //마법책
        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(probs);
            var cellInfo = new GachaResultCellInfo();

            cellInfo.amount = 1;
            cellInfo.newGachaData = this.newGachaTableDatas[randomIdx];
            gachaResultCellInfos.Add(cellInfo);

            ServerData.newGachaServerTable.UpData(this.newGachaTableDatas[randomIdx], cellInfo.amount);
            serverUpdateList.Add(newGachaTableDatas[randomIdx].Id);
        }

        SyncServer(serverUpdateList, price, serverUpdateList.Count);

        UiGachaResultView.Instance.Initialize(gachaResultCellInfos, () => { OnClickOpenButton(lastGachaIdx); });

        SoundManager.Instance.PlaySound("Reward");
    }

    private void SyncServer(List<int> serverUpdateList, int price, int gachaCount)
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);

        //가챠횟수
        Param gachaNumParam = new Param();
        gachaNumParam.Add(UserInfoTable.gachaNum_NewGacha, ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value);

        //마법책
        Param newGachaParam = new Param();
        var table = TableManager.Instance.NewGachaTable.dataArray;
        var tableDatas = ServerData.newGachaServerTable.TableDatas;
        for (int i = 0; i < table.Length; i++)
        {
            if (serverUpdateList != null && serverUpdateList.Contains(table[i].Id) == false) continue;

            string key = table[i].Stringid;
            //hasitem 1
            newGachaParam.Add(key, tableDatas[key].ConvertToString());
        }
        //

        //재화
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        //가챠횟수
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, gachaNumParam));
        //마법책
        transactionList.Add(TransactionValue.SetUpdate(NewGachaServerTable.tableName, NewGachaServerTable.Indate, newGachaParam));

        ServerData.SendTransaction(transactionList);
    }

    public void GetRingButton()
    {
        
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value - GameBalance.GraduateSoulRing < GameBalance.GraduateSoulRingGetInterval)
        {
            PopupManager.Instance.ShowAlarmMessage("뽑은 횟수가 부족합니다!");
            return;
        }

        //차감
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value -= GameBalance.GraduateSoulRingGetInterval;

        var localTableData = TableManager.Instance.NewGachaTable.dataArray;

        ServerData.newGachaServerTable.UpData(localTableData[GameBalance.GraduateSoulRingGetIndex], 1);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();

        userInfoParam.Add(UserInfoTable.gachaNum_NewGacha, ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value);

        string key = localTableData[GameBalance.GraduateSoulRingGetIndex].Stringid;

        var tableDatas = ServerData.newGachaServerTable.TableDatas;

        Param newGachaParam = new Param();

        newGachaParam.Add(key, tableDatas[key].ConvertToString());

        //가챠횟수
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        //반지
        transactionList.Add(TransactionValue.SetUpdate(NewGachaServerTable.tableName, NewGachaServerTable.Indate, newGachaParam));

        ServerData.SendTransaction(transactionList, successCallBack: () => { PopupManager.Instance.ShowAlarmMessage("전설1 반지 획득!!"); });
        
    }
    
    
    public void OnClickGetManyEnergyButton()
    {

        if (!string.IsNullOrEmpty(inputField.text))
        {
            if (int.TryParse(inputField.text, out int result))
            {
                if(result < 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                    return;
                }
                if (ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value < result)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SoulRingClear)}이 부족합니다!");
                    return;
                }

                int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value;

                if (score == 0)
                {
                    PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
                    return;
                }
                

                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score * result}개 획득 합니까?\n<color=red>({score} x {result} 획득 가능)</color>", () =>
                {
                    if (result < 1)
                    {
                        PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                        return;
                    }
                    if (ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value < result)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SoulRingClear)}이 부족합니다!");
                        return;
                    }


                    ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += score * result;
                    ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value -= result;

                    List<TransactionValue> transactions = new List<TransactionValue>();


                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
                    goodsParam.Add(GoodsTable.SoulRingClear, ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value);

                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                    ServerData.SendTransaction(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.NewGachaEnergy)} {score * result}개 획득!", null);
                    });
                }, null);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("소탕 횟수를 입력해주세요.");
            }
        }


    }
}