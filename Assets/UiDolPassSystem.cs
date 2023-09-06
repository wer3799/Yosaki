using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UnityEngine.UI.Extensions;
using static UiRewardView;

public class DolPassData_Fancy
{
    public PassInfo passInfo { get; private set; }
    public DolPassData_Fancy(PassInfo passData)
    {
        this.passInfo = passData;
    }
}

public class UiDolPassSystem : FancyScrollView<DolPassData_Fancy>
{



   [SerializeField]
    private UiDolPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField] private Image diceImage;
    [SerializeField] private List<Sprite> diceSprites;
    
    private List<UiDolPassCell> uiPassCellContainer = new List<UiDolPassCell>();

    private ObscuredString passShopId;

    [SerializeField] private TextMeshProUGUI diceNum;

    [SerializeField] private ParticleSystem diceParticle;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value += 1;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value += 1;
        }
    }
#endif


    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.EventDice).AsObservable().Subscribe(e =>
        {
            diceNum.SetText($"주사위 {ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value}개");
        }).AddTo(this);
    }


    private void Initialize()
    {
        // var tableData = TableManager.Instance.dolPass.dataArray;
        //
        // int interval = tableData.Length - uiPassCellContainer.Count;
        //
        // for (int i = 0; i < interval; i++)
        // {
        //     var prefab = Instantiate<UiDolPassCell>(uiPassCellPrefab, cellParent);
        //     uiPassCellContainer.Add(prefab);
        // }
        //
        // for (int i = 0; i < uiPassCellContainer.Count; i++)
        // {
        //     if (i < tableData.Length)
        //     {
        //         var passInfo = new PassInfo();
        //
        //         passInfo.require = tableData[i].Unlockamount;
        //         passInfo.id = tableData[i].Id;
        //
        //         passInfo.rewardType_Free = tableData[i].Reward1;
        //         passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
        //         passInfo.rewardType_Free_Key = SeolPassServerTable.MonthlypassFreeReward_dol;
        //
        //         passInfo.rewardType_IAP = tableData[i].Reward2;
        //         passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
        //         passInfo.rewardType_IAP_Key = SeolPassServerTable.MonthlypassAdReward_dol;
        //
        //         uiPassCellContainer[i].gameObject.SetActive(true);
        //         uiPassCellContainer[i].Initialize(passInfo);
        //         
        //     }
        //     else
        //     {
        //         uiPassCellContainer[i].gameObject.SetActive(false);
        //     }
        // }

        // cellParent.transform.localPosition = new Vector3(0f, cellParent.transform.localPosition.y, cellParent.transform.localPosition.z);
    }

    public void OnClickDice()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.EventDice)}가 부족합니다.");
            return;
        }
        diceParticle.gameObject.SetActive((true));
        diceParticle.Play();
        
        ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value--;
        //2,4,6
        int diceNum =  Random.Range(0, 3);
        switch (diceNum)
        {
            case 0:
                diceNum = 2;
                ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value += diceNum;
                diceImage.sprite = diceSprites[0];
                break;
            case 1:
                diceNum = 4;
                ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value += diceNum;
                diceImage.sprite = diceSprites[1];
                break;
            case 2:
                diceNum = 6;
                ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value += diceNum;
                diceImage.sprite = diceSprites[2];
                break;
        }
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.EventDice, ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value);

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable.attendanceCount_Dol, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value);

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage($"주사위 {diceNum} 획득!");
        });
    }
    public void OnClickAllDice()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.EventDice)}가 부족합니다.");
            return;
        }
        diceParticle.gameObject.SetActive((true));
        diceParticle.Play();
        var amount = ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value;
        var diceNumAmount = 0;
        for (int i = 0; i < amount; i++)
        {
            //2,4,6
            int diceNum =  Random.Range(0, 3);
            switch (diceNum)
            {
                case 0:
                    diceNum = 2;
                    diceNumAmount += diceNum;
                    diceImage.sprite = diceSprites[0];
                    break;
                case 1:
                    diceNum = 4;
                    diceNumAmount += diceNum;
                    diceImage.sprite = diceSprites[1];
                    break;
                case 2:
                    diceNum = 6;
                    diceNumAmount += diceNum;
                    diceImage.sprite = diceSprites[2];
                    break;
            }
        }
        ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value-=amount;
        ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value += diceNumAmount;
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.EventDice, ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value);

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable.attendanceCount_Dol, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value);

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,$"주사위 {amount}개 사용\n 주사위 눈 {diceNumAmount}획득!", null);
        });
    }
    [SerializeField]
    private UiRewardResultView uiRewardResultView;
    
    public void OnClickAllReceiveButton()
    {
        string freeKey = SeolPassServerTable.MonthlypassFreeReward_dol;
        string adKey = SeolPassServerTable.MonthlypassAdReward_dol;

        List<int> splitData_Free = GetSplitData(SeolPassServerTable.MonthlypassFreeReward_dol);
        List<int> splitData_Ad = GetSplitData(SeolPassServerTable.MonthlypassAdReward_dol);

        Dictionary<Item_Type,float> rewardTypeList = new Dictionary<Item_Type,float>();

        var tableData = TableManager.Instance.dolPass.dataArray;

        int rewardedNum = 0;

        string free = ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassFreeReward_dol].Value;
        string ad = ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassAdReward_dol].Value;

        bool hasCostumeItem = false;

        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                
                if (rewardTypeList.ContainsKey((Item_Type)tableData[i].Reward1) == false)
                {
                    rewardTypeList.Add((Item_Type)tableData[i].Reward1,tableData[i].Reward1_Value);
                }
                else
                {
                    rewardTypeList[(Item_Type)tableData[i].Reward1] += tableData[i].Reward1_Value;
                }
                rewardedNum++;
            }

            ////유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                ad += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                if (rewardTypeList.ContainsKey((Item_Type)tableData[i].Reward2) == false)
                {
                    rewardTypeList.Add((Item_Type)tableData[i].Reward2,tableData[i].Reward2_Value);
                }
                else
                {
                    rewardTypeList[(Item_Type)tableData[i].Reward2] += tableData[i].Reward2_Value;
                }
                rewardedNum++;
            }
        }

        if (hasCostumeItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
            return;
        }

        if (rewardedNum > 0)
        {
            ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassFreeReward_dol].Value = free;
            ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassAdReward_dol].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            var e = rewardTypeList.GetEnumerator();

            List<RewardData> rewardDatas = new List<RewardData>();
            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                rewardDatas.Add(new RewardData(e.Current.Key,e.Current.Value));
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.Key), ServerData.goodsTable.GetTableData(e.Current.Key).Value);
            }
            

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(SeolPassServerTable.MonthlypassFreeReward_dol, ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassFreeReward_dol].Value);
            passParam.Add(SeolPassServerTable.MonthlypassAdReward_dol, ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassAdReward_dol].Value);

            transactions.Add(TransactionValue.SetUpdate(SeolPassServerTable.tableName, SeolPassServerTable.Indate, passParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                // uiRewardResultView.gameObject.SetActive(true);
                //
                // uiRewardResultView.Initialize(rewardDatas);
                //LogManager.Instance.SendLogType("ChildPass", "A", "A");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }
    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiDolPassBuyButton.monthPassKey].buyCount.Value > 0;
        
        return hasIapProduct;
    }
    private bool CanGetReward(int require)
    {
        int attendCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Dol).Value;
        return attendCount >= require;
    }
    public bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.seolPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }
    
    //
        [SerializeField]
        private Scroller scroller;
    
    
        [SerializeField] GameObject cellPrefab = default;

        protected override GameObject CellPrefab => cellPrefab;
    
        private void Start()
        {
            Subscribe();
            
            scroller.Initialize(TypeScroll.DolPass);
            
            scroller.OnValueChanged(UpdatePosition);
    
            var tableData = TableManager.Instance.dolPass.dataArray;
    
            List<DolPassData_Fancy> passInfos = new List<DolPassData_Fancy>();
    
            for (int i = 0; i < tableData.Length; i++)
            {
                var passInfo = new PassInfo();
    
                passInfo.require = tableData[i].Unlockamount;
                passInfo.id = tableData[i].Id;
    
                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = SeolPassServerTable.MonthlypassFreeReward_dol;
    
                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = SeolPassServerTable.MonthlypassAdReward_dol;
                passInfos.Add(new DolPassData_Fancy(passInfo));
    
            }
    
    
            this.UpdateContents(passInfos.ToArray());
            scroller.SetTotalCount(passInfos.Count);
        }
}
