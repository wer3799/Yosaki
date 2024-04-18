using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMarbleRelicRefund : MonoBehaviour
{
    private void Start()
    {
        RefundGrowthStone();
        //Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value != 0)
        {
            return;
        }

        int sumiset0buyCount = ServerData.iAPServerTableTotal.TableDatas["sumiset0"].buyCount.Value;
        int sumiset1buyCount = ServerData.iAPServerTableTotal.TableDatas["sumiset1"].buyCount.Value;
        int sumiset2buyCount = ServerData.iAPServerTableTotal.TableDatas["sumiset2"].buyCount.Value;

        if (sumiset0buyCount == 0 &&
            sumiset1buyCount == 0 &&
            sumiset2buyCount == 0
           )
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.marRelicRefund).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.marRelicRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.marRelicRefund).Value);
            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, marbleParam));

            ServerData.SendTransaction(tr, successCallBack: () =>
            {
#if UNITY_EDITOR
                Debug.LogError("소급 없음");
#endif
            });

            return;
        }

        //소급코드


        //
        //수미꽃
        float _0DiffsumiFlower = 20000;

        //소탕권
        float _1DiffsumiFlower = 2;
        float _2DiffsumiFlower = 2;

        //수미꽃
        float sumiset0Add = sumiset0buyCount * _0DiffsumiFlower;

        //소탕권
        float sumiset1Add = sumiset1buyCount * _1DiffsumiFlower;
        float sumiset2Add = sumiset2buyCount * _2DiffsumiFlower;


        float addSumiTotal = sumiset0Add;
        float addSumiKeyTotal = sumiset1Add + sumiset2Add;

        //LogManager.Instance.SendLogType("SumiRefund", "Sumi", $"{sumiset0buyCount},{sumiset1buyCount}.{sumiset2buyCount}");
        //

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += addSumiTotal;
        ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value += addSumiKeyTotal;

        ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);
        goodsParam.Add(GoodsTable.SumiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value);


        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.marRelicRefund, ServerData.userInfoTable.TableDatas[UserInfoTable.marRelicRefund].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup("수미꽃 세트 소급 완료!",
                $"수미꽃 {Utils.ConvertBigNum(addSumiTotal)}개 소급됨\n" +
                $"수미꽃 소탕권 {Utils.ConvertBigNum(addSumiKeyTotal)}개 소급됨"
                , null);

            //PopupManager.Instance.ShowConfirmPopup("수미 세트 수미꽃 추가 소급",
            //    $"수미꽃 세트0 수미꽃 :  {Utils.ConvertBigNum(sumiset2Add)}개 소급됨\n" +
            //    $"수미꽃 세트1 수미꽃 :  {Utils.ConvertBigNum(sumiset1Add)}개 소급됨" ,null);
        });
    }
    private void RefundGrowthStone()
    {
        
#if UNITY_EDITOR
        return;
#endif
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.growthStoneRefund].Value != 0)
        {
            return;
        }

        int growthStonePackageBuyCount = ServerData.iAPServerTableTotal.TableDatas["growthstoneset0"].buyCount.Value;
        int weeklyGrowthStonePackageBuyCount = ServerData.iAPServerTableTotal.TableDatas["weeklygrowstonepackage"].buyCount.Value;

        if (growthStonePackageBuyCount == 0 &&
            weeklyGrowthStonePackageBuyCount == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.growthStoneRefund).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.growthStoneRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.growthStoneRefund).Value);
            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, marbleParam));

            ServerData.SendTransaction(tr, successCallBack: () =>
            {
#if UNITY_EDITOR
                Debug.LogError("소급 없음");
#endif
            });

            return;
        }

        //소급코드


        //상품
        float growthStonePackageAdd = 2500000000000;

        //주간
        float weeklyGrowthStonePackageAdd = 2000000000000;


        //패키지
        float growthStonePackageSum = growthStonePackageBuyCount * growthStonePackageAdd;

        //주간
        float weeklyGrowthStonePackageSum = weeklyGrowthStonePackageBuyCount * weeklyGrowthStonePackageAdd;


        float addGrowthStoneTotal = growthStonePackageSum + weeklyGrowthStonePackageSum;


        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += addGrowthStoneTotal;

        ServerData.userInfoTable.TableDatas[UserInfoTable.growthStoneRefund].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);


        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.growthStoneRefund, ServerData.userInfoTable.TableDatas[UserInfoTable.growthStoneRefund].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup("수련의돌 세트 소급 완료!",
                $"수련의돌 {Utils.ConvertBigNum(addGrowthStoneTotal)}개 소급됨"
                , null);

            //PopupManager.Instance.ShowConfirmPopup("수미 세트 수미꽃 추가 소급",
            //    $"수미꽃 세트0 수미꽃 :  {Utils.ConvertBigNum(sumiset2Add)}개 소급됨\n" +
            //    $"수미꽃 세트1 수미꽃 :  {Utils.ConvertBigNum(sumiset1Add)}개 소급됨" ,null);
        });
    }
}