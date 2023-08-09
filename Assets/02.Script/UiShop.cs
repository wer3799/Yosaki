using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Purchasing;
using BackEnd;

public enum SellWhere
{
    Shop, StagePass
}

public class UiShop : SingletonMono<UiShop>
{
    private void Start()
    {
        Subscribe();
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.
                Keypad2))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value += 100000000000000000000000000000000000000f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += 1000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value -= 10000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += 10000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value -= 100000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += 100000;
        }
    }
#endif
    private void Subscribe()
    {
        IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(e =>
        {
            SoundManager.Instance.PlaySound("GoldUse");
            GetPackageItem(e.purchasedProduct.definition.id);
        }).AddTo(this);
    }

    public void BuyProduct(string id)
    {
        IAPManager.Instance.BuyProduct(id);
    }

    public bool HasSecondCostume()
    {
        if (ServerData.costumeServerTable.TableDatas["costume41"].hasCostume.Value ==true)
        {
            return true;
        }
        return false;
    }
    public void GetPackageItem(string productId)
    {
        if (productId.Equals("removeadios"))
        {
            productId = "removead";
        }

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
        if (productId.Equals("secondcostume0"))
        {
            if (HasSecondCostume())
            {
                ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value += 5;
                ServerData.goodsTable.UpDataV2(GoodsTable.Mileage,false);
            }
        }

        string logString = string.Empty;

        for (int i = 0; i < tableData.Rewardtypes.Length; i++)
        {
            Item_Type rewardType = (Item_Type)tableData.Rewardtypes[i];
            var rewardAmount = tableData.Rewardvalues[i];

            if (rewardType.IsGoodsItem())
            {
                AddGoodsParam(ref goodsParam, rewardType, rewardAmount);
            }
            else if (rewardType.IsStatusItem())
            {

            }
            else if (rewardType.IsCostumeItem())
            {
                AddCostumeParam(ref costumeParam, rewardType, rewardAmount);
            }
            else if (rewardType.IsPetItem())
            {
                AddPetParam(ref petParam, rewardType, rewardAmount);
            }
            else if (rewardType == Item_Type.MagicStoneBuff)
            {
                AddMagicStoneBuffParam(ref magicStoneBuffParam);
            }
            else if (rewardType.IsWeaponItem())
            {
                AddWeaponParam(ref weaponParam, rewardType);
            }
            else if (rewardType.IsNorigaeItem())
            {
                AddNorigaeParam(ref norigaeParam, rewardType);
            }
            else if (rewardType.IsSkillItem())
            {
                AddSkillParam(ref skillParam, rewardType);
            }
        }

        //재화
        if (goodsParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }
        //코스튬
        if (costumeParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        //펫
        if (petParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));
        }

        if (magicStoneBuffParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(BuffServerTable.tableName, BuffServerTable.Indate, magicStoneBuffParam));
        }

        if (weaponParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));
        }

        if (norigaeParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, norigaeParam));
        }

        if (skillParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));
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

        ServerData.SendTransactionV2(transactionList, successCallBack: WhenRewardSuccess);

        currentItemIdx = productId;
    }

    private string currentItemIdx;

    private void WhenRewardSuccess()
    {
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "구매 감사합니다!\n아이템이 성공적으로 지급됐습니다.", null);

        IAPManager.Instance.SendLog("상품 구매후 수령 성공", currentItemIdx);
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
                    param.Add(GoodsTable.LeeMuGiStone, ServerData.goodsTable.GetTableData(GoodsTable.LeeMuGiStone).Value);
                }
                break;
            case Item_Type.SP:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += amount;
                    param.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
                }
                break; 
            case Item_Type.HellPower:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.HellPowerUp).Value += amount;
                    param.Add(GoodsTable.HellPowerUp, ServerData.goodsTable.GetTableData(GoodsTable.HellPowerUp).Value);
                }
                break;   
            
            case Item_Type.DokebiTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiTreasure).Value += amount;
                    param.Add(GoodsTable.DokebiTreasure, ServerData.goodsTable.GetTableData(GoodsTable.DokebiTreasure).Value);
                }
                break;             
            case Item_Type.DokebiFireEnhance:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireEnhance).Value += amount;
                    param.Add(GoodsTable.DokebiFireEnhance, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireEnhance).Value);
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
            
            case Item_Type.Mileage:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value += amount;
                    param.Add(GoodsTable.Mileage, ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value);
                }
                break;  
            case Item_Type.ClearTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.ClearTicket).Value += amount;
                    param.Add(GoodsTable.ClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.ClearTicket).Value);
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
            case Item_Type.NewGachaEnergy:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += amount;
                    param.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
                }
                break;  
            
            case Item_Type.FoxMaskPartial:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FoxMaskPartial).Value += amount;
                    param.Add(GoodsTable.FoxMaskPartial, ServerData.goodsTable.GetTableData(GoodsTable.FoxMaskPartial).Value);
                }
                break; 
                        
            case Item_Type.SusanoTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SusanoTreasure).Value += amount;
                    param.Add(GoodsTable.SusanoTreasure, ServerData.goodsTable.GetTableData(GoodsTable.SusanoTreasure).Value);
                }
                break; 
            case Item_Type.VisionTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.VisionTreasure).Value += amount;
                    param.Add(GoodsTable.VisionTreasure, ServerData.goodsTable.GetTableData(GoodsTable.VisionTreasure).Value);
                }
                break;  
            case Item_Type.DarkTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DarkTreasure).Value += amount;
                    param.Add(GoodsTable.DarkTreasure, ServerData.goodsTable.GetTableData(GoodsTable.DarkTreasure).Value);
                }
                break;  
            case Item_Type.SinsunTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinsunTreasure).Value += amount;
                    param.Add(GoodsTable.SinsunTreasure, ServerData.goodsTable.GetTableData(GoodsTable.SinsunTreasure).Value);
                }
                break;  
            case Item_Type.GwisalTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GwisalTreasure).Value += amount;
                    param.Add(GoodsTable.GwisalTreasure, ServerData.goodsTable.GetTableData(GoodsTable.GwisalTreasure).Value);
                }
                break;  
            case Item_Type.ChunguTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.ChunguTreasure).Value += amount;
                    param.Add(GoodsTable.ChunguTreasure, ServerData.goodsTable.GetTableData(GoodsTable.ChunguTreasure).Value);
                }
                break;  
            case Item_Type.DosulClear:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DosulClear).Value += amount;
                param.Add(GoodsTable.DosulClear, ServerData.goodsTable.GetTableData(GoodsTable.DosulClear).Value);
            }
                break;  
            case Item_Type.DosulGoods:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DosulGoods).Value += amount;
                param.Add(GoodsTable.DosulGoods, ServerData.goodsTable.GetTableData(GoodsTable.DosulGoods).Value);
            }
                break;  
            case Item_Type.TransGoods:
            {
                ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value += amount;
                param.Add(GoodsTable.TransGoods, ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value);
            }
                break;  
            case Item_Type.GuildTowerClearTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GuildTowerClearTicket).Value += amount;
                    param.Add(GoodsTable.GuildTowerClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.GuildTowerClearTicket).Value);
                }
                break;  
            case Item_Type.GuildTowerHorn:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GuildTowerHorn).Value += amount;
                    param.Add(GoodsTable.GuildTowerHorn, ServerData.goodsTable.GetTableData(GoodsTable.GuildTowerHorn).Value);
                }
                break; 
            case Item_Type.SahyungTreasure:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SahyungTreasure).Value += amount;
                    param.Add(GoodsTable.SahyungTreasure, ServerData.goodsTable.GetTableData(GoodsTable.SahyungTreasure).Value);
                }
                break;    
            case Item_Type.SinsuMarble:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SinsuMarble).Value += amount;
                    param.Add(GoodsTable.SinsuMarble, ServerData.goodsTable.GetTableData(GoodsTable.SinsuMarble).Value);
                }
                break; 
            case Item_Type.Event_Collection:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Collection).Value += amount;
                    param.Add(GoodsTable.Event_Collection, ServerData.goodsTable.GetTableData(GoodsTable.Event_Collection).Value);
                }
                break;
            case Item_Type.Event_HotTime:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_HotTime).Value += amount;
                    param.Add(GoodsTable.Event_HotTime, ServerData.goodsTable.GetTableData(GoodsTable.Event_HotTime).Value);
                }
                break;
                
            case Item_Type.Event_Collection_All:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Collection_All).Value += amount;
                    param.Add(GoodsTable.Event_Collection_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Collection_All).Value);
                }
                break;

            case Item_Type.Event_Fall_Gold:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value += amount;
                    param.Add(GoodsTable.Event_Fall_Gold, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value);
                }
                break;
            case Item_Type.Event_NewYear:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear).Value += amount;
                    param.Add(GoodsTable.Event_NewYear, ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear).Value);
                }
                break;
            case Item_Type.Event_NewYear_All:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear_All).Value += amount;
                    param.Add(GoodsTable.Event_NewYear_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear_All).Value);
                }
                break;
            case Item_Type.Event_Mission:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission).Value += amount;
                    param.Add(GoodsTable.Event_Mission, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission).Value);
                }
                break;
            case Item_Type.Event_Mission_All:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission_All).Value += amount;
                    param.Add(GoodsTable.Event_Mission_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission_All).Value);
                }
                break;

            case Item_Type.du:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.du).Value += amount;
                    param.Add(GoodsTable.du, ServerData.goodsTable.GetTableData(GoodsTable.du).Value);
                }
                break;

            //

            case Item_Type.Hae_Norigae:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Hae_Norigae).Value += amount;
                    param.Add(GoodsTable.Hae_Norigae, ServerData.goodsTable.GetTableData(GoodsTable.Hae_Norigae).Value);
                }
                break;

            case Item_Type.Hae_Pet:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.Hae_Pet).Value += amount;
                    param.Add(GoodsTable.Hae_Pet, ServerData.goodsTable.GetTableData(GoodsTable.Hae_Pet).Value);
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
            // //
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
            // //
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
            // //
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
 
            //
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
            //
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

            case Item_Type.GuimoonRelic:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value += amount;
                    param.Add(GoodsTable.GuimoonRelic, ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value);
                }
                break;
            case Item_Type.GuimoonRelicClearTicket:
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelicClearTicket).Value += amount;
                    param.Add(GoodsTable.GuimoonRelicClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelicClearTicket).Value);
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

        }
    }

    public void AddCostumeParam(ref Param param, Item_Type type, float amount)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();

        if (ServerData.costumeServerTable.TableDatas.TryGetValue(key, out var serverData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"미등록된 아이디 {key}", null);
            return;
        }

        serverData.hasCostume.Value = true;

        param.Add(key, serverData.ConvertToString());
    }

    public void AddPetParam(ref Param param, Item_Type type, float amount)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();

        if (ServerData.petTable.TableDatas.TryGetValue(key, out var serverData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"미등록된 아이디 {key}", null);
            return;
        }

        serverData.hasItem.Value = 1;

        param.Add(key, $"{serverData.idx},{serverData.hasItem.Value},{serverData.level.Value},{serverData.remainSec.Value}");
    }

    public void AddMagicStoneBuffParam(ref Param param)
    {
        if (param == null)
        {
            param = new Param();
        }

        var buffTableData = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < buffTableData.Length; i++)
        {
            if (buffTableData[i].Bufftype == (int)StatusType.MagicStoneAddPer)
            {
                ServerData.buffServerTable.TableDatas[buffTableData[i].Stringid].remainSec.Value = -1;

                param.Add(buffTableData[i].Stringid, ServerData.buffServerTable.TableDatas[buffTableData[i].Stringid].ConvertToString());
                return;
            }
        }
    }

    public void AddWeaponParam(ref Param param, Item_Type type)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();
        var serverTableData = ServerData.weaponTable.TableDatas[key];
        serverTableData.hasItem.Value = 1;
        serverTableData.amount.Value += 1;

        param.Add(key, serverTableData.ConvertToString());
    }

    public void AddNorigaeParam(ref Param param, Item_Type type)
    {
        if (param == null)
        {
            param = new Param();
        }

        string key = type.ToString();
        var serverTableData = ServerData.magicBookTable.TableDatas[key];
        serverTableData.hasItem.Value = 1;
        serverTableData.amount.Value += 1;

        param.Add(key, serverTableData.ConvertToString());
    }

    public void AddSkillParam(ref Param param, Item_Type type)
    {
        if (param == null)
        {
            param = new Param();
        }

        int skillIdx = ((int)type) % 3000;

        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillIdx].Value = 1;
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAlreadyHas][skillIdx].Value = 1;

        List<int> skillAmountSyncData = new List<int>();
        List<int> skillAlreadyHasSyncData = new List<int>();

        for (int i = 0; i < ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
            skillAlreadyHasSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAlreadyHas][i].Value);
        }

        param.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);
        param.Add(SkillServerTable.SkillAlreadyHas, skillAlreadyHasSyncData);
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }

}
