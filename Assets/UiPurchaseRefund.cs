using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiPurchaseRefund : MonoBehaviour
{



    private void Start()
    {
        //Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.purchaseRefund0].Value != 0)
        {
            return;
        }

        
        int sun0 = ServerData.iAPServerTableTotal.TableDatas["sun0"].buyCount.Value;
        int sun1 = ServerData.iAPServerTableTotal.TableDatas["sun1"].buyCount.Value;
        int sun2 = ServerData.iAPServerTableTotal.TableDatas["sun2"].buyCount.Value;
        int sun3 = ServerData.iAPServerTableTotal.TableDatas["sun3"].buyCount.Value;
        int sun4 = ServerData.iAPServerTableTotal.TableDatas["sun4"].buyCount.Value;
        int sun5 = ServerData.iAPServerTableTotal.TableDatas["sun5"].buyCount.Value;


        int sword1 = ServerData.iAPServerTableTotal.TableDatas["sword1"].buyCount.Value;
        int sword2 = ServerData.iAPServerTableTotal.TableDatas["sword2"].buyCount.Value;
        int sword3 = ServerData.iAPServerTableTotal.TableDatas["sword3"].buyCount.Value;
        int sword4 = ServerData.iAPServerTableTotal.TableDatas["sword4"].buyCount.Value;

        int hellset = ServerData.iAPServerTableTotal.TableDatas["hellset"].buyCount.Value;


        int chunflower0 = ServerData.iAPServerTableTotal.TableDatas["chunflower0"].buyCount.Value;
        int chunflower1 = ServerData.iAPServerTableTotal.TableDatas["chunflower1"].buyCount.Value;


        int dokebifire0 = ServerData.iAPServerTableTotal.TableDatas["dokebifire0"].buyCount.Value;
        int dokebifire1 = ServerData.iAPServerTableTotal.TableDatas["dokebifire1"].buyCount.Value;

        int dokebifirekey0 = ServerData.iAPServerTableTotal.TableDatas["dokebifirekey0"].buyCount.Value;


        int chris0 = ServerData.iAPServerTableTotal.TableDatas["chris0"].buyCount.Value;
        int chris1 = ServerData.iAPServerTableTotal.TableDatas["chris1"].buyCount.Value;

        int coldpackage0 = ServerData.iAPServerTableTotal.TableDatas["coldpackage0"].buyCount.Value;
        int coldpackage1 = ServerData.iAPServerTableTotal.TableDatas["coldpackage1"].buyCount.Value;

        if (sun0==0&&sun1==0&&sun2==0&&sun3==0&&sun4==0&&sun5==0
            && sword1==0&& sword2==0&& sword3==0&& sword4==0
            && hellset==0
            && chunflower0 == 0&& chunflower1 == 0
            && dokebifire0 == 0&& dokebifire1 == 0
            && dokebifirekey0 == 0
            && chris0 == 0&& chris1 == 0
            && coldpackage0 == 0&& coldpackage1 == 0
            )
        {

            ServerData.userInfoTable.GetTableData(UserInfoTable.purchaseRefund0).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.purchaseRefund0, ServerData.userInfoTable.GetTableData(UserInfoTable.purchaseRefund0).Value);
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


        float spRefund = 0;
        float helRefund = 0;
        float cwRefund = 0;
        float dokebifireRefund = 0;
        float dokebifireKeyRefund = 0;

        //검조각
        spRefund += sun0 * 20000;
        spRefund += sun1 * 60000;
        spRefund += sun4 * 60000;
        spRefund += sun5 * 60000;
        spRefund += sword1 * 6000;
        spRefund += sword2 * 10000;
        spRefund += sword3 * 30000;
        spRefund += sword4 * 60000;
        spRefund += chunflower0 * 10000;
        spRefund += chunflower1 * 20000;
        spRefund += dokebifire0 * 10000;
        spRefund += dokebifire1 * 20000;
        spRefund += dokebifirekey0 * 10000;
        spRefund += coldpackage0 * 20000;
        spRefund += coldpackage1 * 40000;
        spRefund += hellset * 20000;

        //불멸석
        helRefund += sun2 * 2000;
        helRefund += sun3 * 6000;
        helRefund += sun4 * 6000;
        helRefund += coldpackage0 * 2000;
        helRefund += coldpackage1 * 4000;
        helRefund += hellset * 5000;

        //천계꽃
        cwRefund += sun5 * 6000;
        cwRefund += chunflower0 * 2000;
        cwRefund += chunflower1 * 5000;
        cwRefund += chris0 * 2000;
        cwRefund += chris1 * 4000;
        cwRefund += coldpackage0 * 2000;
        cwRefund += coldpackage1 * 4000;

        //도깨비불
        dokebifireRefund += dokebifire0 * 2000;
        dokebifireRefund += dokebifire1 * 5000;
        dokebifireRefund += chris0 * 2000;
        dokebifireRefund += chris1 * 4000;

        //소탕권
        dokebifireKeyRefund += dokebifirekey0 * 3;


        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += spRefund;
        ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value += helRefund;
        ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += cwRefund;
        ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += dokebifireRefund;
        ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value += dokebifireKeyRefund;

        ServerData.userInfoTable.TableDatas[UserInfoTable.purchaseRefund0].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
        goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
        goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);
        goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
        goodsParam.Add(GoodsTable.DokebiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value);
        

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.purchaseRefund0, ServerData.userInfoTable.TableDatas[UserInfoTable.purchaseRefund0].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        PopupManager.Instance.ShowConfirmPopup("구매 상품 추가 소급",
            $"검조각 {Utils.ConvertBigNum(spRefund)}개 소급됨\n" +
             $"불멸석 {Utils.ConvertBigNum(helRefund)}개 소급됨\n" +
              $"천계꽃 {Utils.ConvertBigNum(cwRefund)}개 소급됨\n" +
               $"도깨비불 {Utils.ConvertBigNum(dokebifireRefund)}개 소급됨\n" +
                $"도깨비불 소탕권 {Utils.ConvertBigNum(dokebifireKeyRefund)}개 소급됨\n" , null);
        });


    }

}
