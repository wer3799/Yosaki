using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Purchasing;
using BackEnd;
using TMPro;

public class UiLevelUpEventShop : SingletonMono<UiLevelUpEventShop>
{
    [SerializeField]
    private UiIapItemCell iapCellPrefab;

    [SerializeField]
    private Transform cellRoot;

    [SerializeField]
    private TextMeshProUGUI topClearStageId;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].AsObservable().Subscribe(e =>
        {
            topClearStageId.SetText($"최고 스테이지 : {Utils.ConvertStage((int)e + 1)}");
        }).AddTo(this);
    }

    private void Initialize()
    {
        var e = TableManager.Instance.InAppPurchaseData.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.SHOPCATEGORY == ShopCategory.LevelUp)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, cellRoot);
                cell.Initialize(e.Current.Value);
                cell.gameObject.SetActive(true);
            }
        }
    }

    public void GetPackageItem(string productId)
    {

        if (TableManager.Instance.InAppPurchaseData.TryGetValue(productId, out var tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {productId}", null);
            return;
        }
        else
        {
            // PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{tableData.Title} 구매 성공!", null);
        }

        if (tableData.SELLWHERE != SellWhere.Shop) return;
        if (tableData.BUYTYPE == BuyType.Pension) return;

        //아이템 수령처리
        Param goodsParam = null;
        Param costumeParam = null;
        Param petParam = null;
        Param iapParam = new Param();
        Param iapTotalParam = new Param();
        Param magicStoneBuffParam = null;
        Param weaponParam = null;
        Param norigaeParam = null;
        Param skillParam = null;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        string logString = string.Empty;

        for (int i = 0; i < tableData.Rewardtypes.Length; i++)
        {
            Item_Type rewardType = (Item_Type)tableData.Rewardtypes[i];
            var rewardAmount = tableData.Rewardvalues[i];

            if (rewardType.IsGoodsItem())
            {
                AddGoodsParam(ref goodsParam, rewardType, rewardAmount);
            }
        }

        //재화
        if (goodsParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }

        if (ServerData.iapServerTable.TableDatas.ContainsKey(tableData.Productid) == false)
        {
            Debug.LogError($"@@@product Id {tableData.Productid}");
            return;
        }
        else
        {
            ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;
            ServerData.iAPServerTableTotal.TableDatas[tableData.Productid].buyCount.Value++;
        }

        iapParam.Add(tableData.Productid, ServerData.iapServerTable.TableDatas[tableData.Productid].ConvertToString());

        iapTotalParam.Add(tableData.Productid, ServerData.iAPServerTableTotal.TableDatas[tableData.Productid].ConvertToString());

        transactionList.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));

        transactionList.Add(TransactionValue.SetUpdate(IAPServerTableTotal.tableName, IAPServerTableTotal.Indate, iapTotalParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상 획득 완료!", null);
          });
    }

    public void AddGoodsParam(ref Param param, Item_Type type, float amount)
    {
        if (param == null)
        {
            param = new Param();
        }

        switch (type)
        {
            case Item_Type.Jade:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += amount;
                    param.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
                }
                break;
            case Item_Type.GrowthStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += amount;
                    param.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
                }
                break;
            case Item_Type.Ticket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += amount;
                    param.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
                }
                break;
            case Item_Type.Marble:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += amount;
                    param.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                }
                break;
            case Item_Type.Songpyeon:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Songpyeon).Value += amount;
                    param.Add(GoodsTable.Songpyeon, ServerData.goodsTable.GetTableData(GoodsTable.Songpyeon).Value);
                }
                break;
            case Item_Type.RelicTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += amount;
                    param.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
                }
                break;
            case Item_Type.Event_Item_0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value += amount;
                    param.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
                }
                break;

            case Item_Type.Event_Item_1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value += amount;
                    param.Add(GoodsTable.Event_Item_1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value);
                }
                break;   
            case Item_Type.Event_Item_SnowMan:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_SnowMan).Value += amount;
                    param.Add(GoodsTable.Event_Item_SnowMan, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_SnowMan).Value);
                }
                break;
            case Item_Type.Event_Item_SnowMan_All:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_SnowMan_All).Value += amount;
                    param.Add(GoodsTable.Event_Item_SnowMan_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_SnowMan_All).Value);
                }
                break;

            case Item_Type.SulItem:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SulItem).Value += amount;
                    param.Add(GoodsTable.SulItem, ServerData.goodsTable.GetTableData(GoodsTable.SulItem).Value);
                }
                break;

            case Item_Type.FeelMulStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FeelMulStone).Value += amount;
                    param.Add(GoodsTable.FeelMulStone, ServerData.goodsTable.GetTableData(GoodsTable.FeelMulStone).Value);
                }
                break;

            case Item_Type.Asura0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura0).Value += amount;
                    param.Add(GoodsTable.Asura0, ServerData.goodsTable.GetTableData(GoodsTable.Asura0).Value);
                }
                break;

            case Item_Type.Asura1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura1).Value += amount;
                    param.Add(GoodsTable.Asura1, ServerData.goodsTable.GetTableData(GoodsTable.Asura1).Value);
                }
                break;

            case Item_Type.Asura2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura2).Value += amount;
                    param.Add(GoodsTable.Asura2, ServerData.goodsTable.GetTableData(GoodsTable.Asura2).Value);
                }
                break;

            case Item_Type.Asura3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura3).Value += amount;
                    param.Add(GoodsTable.Asura3, ServerData.goodsTable.GetTableData(GoodsTable.Asura3).Value);
                }
                break;
            case Item_Type.Asura4:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura4).Value += amount;
                    param.Add(GoodsTable.Asura4, ServerData.goodsTable.GetTableData(GoodsTable.Asura4).Value);
                }
                break;

            case Item_Type.Asura5:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Asura5).Value += amount;
                    param.Add(GoodsTable.Asura5, ServerData.goodsTable.GetTableData(GoodsTable.Asura5).Value);
                }
                break;
            case Item_Type.Aduk:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Aduk).Value += amount;
                    param.Add(GoodsTable.Aduk, ServerData.goodsTable.GetTableData(GoodsTable.Aduk).Value);
                }
                break;

            case Item_Type.LeeMuGiStone:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value += amount;
                    param.Add(GoodsTable.Aduk, ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value);
                }
                break;

            //
            case Item_Type.SinSkill0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill0).Value += amount;
                    param.Add(GoodsTable.SinSkill0, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill0).Value);
                }
                break;
            case Item_Type.SinSkill1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill1).Value += amount;
                    param.Add(GoodsTable.SinSkill1, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill1).Value);
                }
                break;
            case Item_Type.SinSkill2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill2).Value += amount;
                    param.Add(GoodsTable.SinSkill2, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill2).Value);
                }
                break;
            case Item_Type.SinSkill3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinSkill3).Value += amount;
                    param.Add(GoodsTable.SinSkill3, ServerData.goodsTable.GetTableData(GoodsTable.SinSkill3).Value);
                }
                break;     
            case Item_Type.NataSkill:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.NataSkill).Value += amount;
                    param.Add(GoodsTable.NataSkill, ServerData.goodsTable.GetTableData(GoodsTable.NataSkill).Value);
                }
                break;  
            case Item_Type.OrochiSkill:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.OrochiSkill).Value += amount;
                    param.Add(GoodsTable.OrochiSkill, ServerData.goodsTable.GetTableData(GoodsTable.OrochiSkill).Value);
                }
                break;
            //
            case Item_Type.Sun0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Sun0).Value += amount;
                    param.Add(GoodsTable.Sun0, ServerData.goodsTable.GetTableData(GoodsTable.Sun0).Value);
                }
                break;
            case Item_Type.Sun1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Sun1).Value += amount;
                    param.Add(GoodsTable.Sun1, ServerData.goodsTable.GetTableData(GoodsTable.Sun1).Value);
                }
                break;
            case Item_Type.Sun2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Sun2).Value += amount;
                    param.Add(GoodsTable.Sun2, ServerData.goodsTable.GetTableData(GoodsTable.Sun2).Value);
                }
                break;
            case Item_Type.Sun3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Sun3).Value += amount;
                    param.Add(GoodsTable.Sun3, ServerData.goodsTable.GetTableData(GoodsTable.Sun3).Value);
                }
                break;
            case Item_Type.Sun4:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Sun4).Value += amount;
                    param.Add(GoodsTable.Sun4, ServerData.goodsTable.GetTableData(GoodsTable.Sun4).Value);
                }
                break;
            //
            case Item_Type.Chun0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Chun0).Value += amount;
                    param.Add(GoodsTable.Chun0, ServerData.goodsTable.GetTableData(GoodsTable.Chun0).Value);
                }
                break;
            case Item_Type.Chun1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Chun1).Value += amount;
                    param.Add(GoodsTable.Chun1, ServerData.goodsTable.GetTableData(GoodsTable.Chun1).Value);
                }
                break;
            case Item_Type.Chun2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Chun2).Value += amount;
                    param.Add(GoodsTable.Chun2, ServerData.goodsTable.GetTableData(GoodsTable.Chun2).Value);
                }
                break;
            case Item_Type.Chun3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Chun3).Value += amount;
                    param.Add(GoodsTable.Chun3, ServerData.goodsTable.GetTableData(GoodsTable.Chun3).Value);
                }
                break;
            case Item_Type.Chun4:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Chun4).Value += amount;
                    param.Add(GoodsTable.Chun4, ServerData.goodsTable.GetTableData(GoodsTable.Chun4).Value);
                }
                break;
            //
            //
            case Item_Type.DokebiSkill0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill0).Value += amount;
                    param.Add(GoodsTable.DokebiSkill0, ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill0).Value);
                }
                break;
            case Item_Type.DokebiSkill1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill1).Value += amount;
                    param.Add(GoodsTable.DokebiSkill1, ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill1).Value);
                }
                break;
            case Item_Type.DokebiSkill2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill2).Value += amount;
                    param.Add(GoodsTable.DokebiSkill2, ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill2).Value);
                }
                break;
            case Item_Type.DokebiSkill3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill3).Value += amount;
                    param.Add(GoodsTable.DokebiSkill3, ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill3).Value);
                }
                break;
            case Item_Type.DokebiSkill4:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill4).Value += amount;
                    param.Add(GoodsTable.DokebiSkill4, ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill4).Value);
                }
                break;
            //            //
            case Item_Type.FourSkill0:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill0).Value += amount;
                    param.Add(GoodsTable.FourSkill0, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill0).Value);
                }
                break;
            case Item_Type.FourSkill1:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill1).Value += amount;
                    param.Add(GoodsTable.FourSkill1, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill1).Value);
                }
                break;
            case Item_Type.FourSkill2:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill2).Value += amount;
                    param.Add(GoodsTable.FourSkill2, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill2).Value);
                }
                break;
            case Item_Type.FourSkill3:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill3).Value += amount;
                    param.Add(GoodsTable.FourSkill3, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill3).Value);
                }
                break;
            //
            //            //
            case Item_Type.FourSkill4:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill4).Value += amount;
                    param.Add(GoodsTable.FourSkill4, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill4).Value);
                }
                break;
            case Item_Type.FourSkill5:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill5).Value += amount;
                    param.Add(GoodsTable.FourSkill5, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill5).Value);
                }
                break;
            case Item_Type.FourSkill6:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill6).Value += amount;
                    param.Add(GoodsTable.FourSkill6, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill6).Value);
                }
                break;
            case Item_Type.FourSkill7:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill7).Value += amount;
                    param.Add(GoodsTable.FourSkill7, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill7).Value);
                }
                break;
            case Item_Type.FourSkill8:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FourSkill8).Value += amount;
                    param.Add(GoodsTable.FourSkill8, ServerData.goodsTable.GetTableData(GoodsTable.FourSkill8).Value);
                }
                break;
            //            //
            case Item_Type.VisionSkill0:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill0).Value += amount;
                param.Add(GoodsTable.VisionSkill0, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill0).Value);
            }
                break;
            case Item_Type.VisionSkill1:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill1).Value += amount;
                param.Add(GoodsTable.VisionSkill1, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill1).Value);
            }
                break;
            case Item_Type.VisionSkill2:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill2).Value += amount;
                param.Add(GoodsTable.VisionSkill2, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill2).Value);
            }
                break;
            case Item_Type.VisionSkill3:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill3).Value += amount;
                param.Add(GoodsTable.VisionSkill3, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill3).Value);
            }
                break;
            case Item_Type.VisionSkill4:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill4).Value += amount;
                param.Add(GoodsTable.VisionSkill4, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill4).Value);
            }
                break;
            case Item_Type.VisionSkill5:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill5).Value += amount;
                param.Add(GoodsTable.VisionSkill5, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill5).Value);
            }
                break;
            case Item_Type.VisionSkill6:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill6).Value += amount;
                param.Add(GoodsTable.VisionSkill6, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill6).Value);
            }
                break;
            case Item_Type.VisionSkill7:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill7).Value += amount;
                param.Add(GoodsTable.VisionSkill7, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill7).Value);
            }
                break;
            case Item_Type.VisionSkill8:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill8).Value += amount;
                param.Add(GoodsTable.VisionSkill8, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill8).Value);
            }
                break;
            case Item_Type.VisionSkill9:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill9).Value += amount;
                param.Add(GoodsTable.VisionSkill9, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill9).Value);
            }
                break;
            case Item_Type.VisionSkill10:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill10).Value += amount;
                param.Add(GoodsTable.VisionSkill10, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill10).Value);
            }
                break;
            case Item_Type.VisionSkill11:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill11).Value += amount;
                param.Add(GoodsTable.VisionSkill11, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill11).Value);
            }
                break;
            case Item_Type.VisionSkill12:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill12).Value += amount;
                param.Add(GoodsTable.VisionSkill12, ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill12).Value);
            }
                break;
            //    //            //
            case Item_Type.ThiefSkill0:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill0).Value += amount;
                param.Add(GoodsTable.ThiefSkill0, ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill0).Value);
            }
                break;
            case Item_Type.ThiefSkill1:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill1).Value += amount;
                param.Add(GoodsTable.ThiefSkill1, ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill1).Value);
            }
                break;
            case Item_Type.ThiefSkill2:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill2).Value += amount;
                param.Add(GoodsTable.ThiefSkill2, ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill2).Value);
            }
                break;
            case Item_Type.ThiefSkill3:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill3).Value += amount;
                param.Add(GoodsTable.ThiefSkill3, ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill3).Value);
            }
                break;
            case Item_Type.ThiefSkill4:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill4).Value += amount;
                param.Add(GoodsTable.ThiefSkill4, ServerData.goodsTable.GetTableData(GoodsTable.ThiefSkill4).Value);
            }
                break;
            //
            //    //            //
            case Item_Type.DarkSkill0:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill0).Value += amount;
                param.Add(GoodsTable.DarkSkill0, ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill0).Value);
            }
                break;
            case Item_Type.DarkSkill1:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill1).Value += amount;
                param.Add(GoodsTable.DarkSkill1, ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill1).Value);
            }
                break;
            case Item_Type.DarkSkill2:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill2).Value += amount;
                param.Add(GoodsTable.DarkSkill2, ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill2).Value);
            }
                break;
            case Item_Type.DarkSkill3:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill3).Value += amount;
                param.Add(GoodsTable.DarkSkill3, ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill3).Value);
            }
                break;
            case Item_Type.DarkSkill4:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill4).Value += amount;
                param.Add(GoodsTable.DarkSkill4, ServerData.goodsTable.GetTableData(GoodsTable.DarkSkill4).Value);
            }
                break;
            //
            case Item_Type.GangrimSkill:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GangrimSkill).Value += amount;
                    param.Add(GoodsTable.GangrimSkill, ServerData.goodsTable.GetTableData(GoodsTable.GangrimSkill).Value);
                }
                break;
            //

            case Item_Type.SmithFire:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += amount;
                    param.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);
                }
                break;

            case Item_Type.StageRelic:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value += amount;
                    param.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);
                }
                break;

            case Item_Type.PeachReal:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += amount;
                    param.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
                }
                break;
            case Item_Type.GuildReward:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value += amount;
                    param.Add(GoodsTable.GuildReward, ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value);
                }
                break;    
            case Item_Type.SP:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += amount;
                    param.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
                }
                break;   
            case Item_Type.Hel:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value += amount;
                    param.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
                }
                break;    
            case Item_Type.Ym:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Ym).Value += amount;
                    param.Add(GoodsTable.Ym, ServerData.goodsTable.GetTableData(GoodsTable.Ym).Value);
                }
                break;   
            case Item_Type.Fw:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Fw).Value += amount;
                    param.Add(GoodsTable.Fw, ServerData.goodsTable.GetTableData(GoodsTable.Fw).Value);
                }
                break;   
            
            case Item_Type.Cw:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += amount;
                    param.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);
                }
                break;
            case Item_Type.DokebiFire:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += amount;
                    param.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
                }
                break;   
            case Item_Type.SuhoPetFeed:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value += amount;
                    param.Add(GoodsTable.SuhoPetFeed, ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value);
                }
                break;   
            case Item_Type.SuhoPetFeedClear:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeedClear).Value += amount;
                    param.Add(GoodsTable.SuhoPetFeedClear, ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeedClear).Value);
                }
                break;   
            case Item_Type.SoulRingClear:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value += amount;
                    param.Add(GoodsTable.SoulRingClear, ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value);
                }
                break;   
            case Item_Type.SumiFire:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += amount;
                    param.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);
                }
                break;       
            case Item_Type.SealWeaponClear:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value += amount;
                    param.Add(GoodsTable.SealWeaponClear, ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value);
                }
                break;     
            case Item_Type.Tresure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Tresure).Value += amount;
                    param.Add(GoodsTable.Tresure, ServerData.goodsTable.GetTableData(GoodsTable.Tresure).Value);
                }
                break;  
                
            case Item_Type.SinsuRelic:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinsuRelic).Value += amount;
                    param.Add(GoodsTable.SinsuRelic, ServerData.goodsTable.GetTableData(GoodsTable.SinsuRelic).Value);
                }
                break;
            case Item_Type.HyungsuRelic:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.HyungsuRelic).Value += amount;
                    param.Add(GoodsTable.HyungsuRelic, ServerData.goodsTable.GetTableData(GoodsTable.HyungsuRelic).Value);
                }
                break;
            case Item_Type.ChunguRelic:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.ChunguRelic).Value += amount;
                    param.Add(GoodsTable.ChunguRelic, ServerData.goodsTable.GetTableData(GoodsTable.ChunguRelic).Value);
                }
                break;
            case Item_Type.FoxRelic:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value += amount;
                    param.Add(GoodsTable.FoxRelic, ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value);
                }
                break;
            case Item_Type.FoxRelicClearTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FoxRelicClearTicket).Value += amount;
                    param.Add(GoodsTable.FoxRelicClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.FoxRelicClearTicket).Value);
                }
                break;
            case Item_Type.TransClearTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value += amount;
                    param.Add(GoodsTable.TransClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value);
                }
                break;
            case Item_Type.MeditationGoods:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.MeditationGoods).Value += amount;
                    param.Add(GoodsTable.MeditationGoods, ServerData.goodsTable.GetTableData(GoodsTable.MeditationGoods).Value);
                }
                break;
            case Item_Type.MeditationClearTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.MeditationClearTicket).Value += amount;
                    param.Add(GoodsTable.MeditationClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.MeditationClearTicket).Value);
                }
                break;;
            case Item_Type.DaesanGoods:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DaesanGoods).Value += amount;
                    param.Add(GoodsTable.DaesanGoods, ServerData.goodsTable.GetTableData(GoodsTable.DaesanGoods).Value);
                }
                break;
            case Item_Type.HonorGoods:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.HonorGoods).Value += amount;
                    param.Add(GoodsTable.HonorGoods, ServerData.goodsTable.GetTableData(GoodsTable.HonorGoods).Value);
                }
                break;
            case Item_Type.EventDice:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value += amount;
                    param.Add(GoodsTable.EventDice, ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value);
                }
                break;
            case Item_Type.Event_SA:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_SA).Value += amount;
                    param.Add(GoodsTable.Event_SA, ServerData.goodsTable.GetTableData(GoodsTable.Event_SA).Value);
                }
                break;

            case Item_Type.NewGachaEnergy:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += amount;
                    param.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
                }
                break;
            case Item_Type.DokebiBundle:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiBundle).Value += amount;
                    param.Add(GoodsTable.DokebiBundle, ServerData.goodsTable.GetTableData(GoodsTable.DokebiBundle).Value);
                }
                break;
            case Item_Type.DokebiFireKey:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value += amount;
                    param.Add(GoodsTable.DokebiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value);
                }
                break;   
            case Item_Type.SumiFireKey:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value += amount;
                    param.Add(GoodsTable.SumiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.SumiFireKey).Value);
                }
                break;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.statusTable.GetTableData(StatusTable.Level).Value += 10000;
        }
    }

#endif
}
