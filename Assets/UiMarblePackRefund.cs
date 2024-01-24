using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMarblePackRefund : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;

    private void Start()
    {
        return;
        Check();
    }

    private void Check()
    {
        rootObject.SetActive(false);

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.monthreset).Value == 1) return;

        int buyCount = ServerData.iAPServerTableTotal.TableDatas["monthpass9ins"].buyCount.Value;

        if (buyCount == 0 )
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.monthreset).Value = 1;

            List<TransactionValue> tr = new List<TransactionValue>();

            Param marbleParam = new Param();

            marbleParam.Add(UserInfoTable.monthreset, ServerData.userInfoTable.GetTableData(UserInfoTable.monthreset).Value);
            tr.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, marbleParam));

            ServerData.SendTransaction(tr, successCallBack: () =>
              {
#if UNITY_EDITOR
                  Debug.LogError("소급 없음");
#endif
              });

            return;
        }

        rootObject.SetActive(true);

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += 15000000;
        ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += 2000;
        ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += 3000;
        ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += 50;
        ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += 2000;

        ServerData.userInfoTable.TableDatas[UserInfoTable.monthreset].Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
        goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);
        goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
        goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
        //goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.monthreset, ServerData.userInfoTable.TableDatas[UserInfoTable.monthreset].Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "소급 완료!", null);
          });
    }
}
