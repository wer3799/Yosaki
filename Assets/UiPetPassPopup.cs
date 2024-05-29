using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using GoogleMobileAds.Api;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UiPetPassPopup : FancyScrollView<PassData_Fancy>
{
    
    private List<List<PassData_Fancy>> _passInfosList = new List<List<PassData_Fancy>>();

    [FormerlySerializedAs("_prefab")] [SerializeField] private SelectablePetPassButton prefab;
    [FormerlySerializedAs("cellParent")] [SerializeField] private Transform leftCellParent;

    [FormerlySerializedAs("_petPassBuyButton")] [SerializeField] private UiPetPassBuyButton petPassBuyButton;


    [FormerlySerializedAs("_goodsIndicator")] [SerializeField] private List<UiAutoGoodsIndicator> goodsIndicator = new List<UiAutoGoodsIndicator>();

    [SerializeField] private SeasonKillCountIndicator2 _seasonKillCountIndicator2;

    [SerializeField] private SkeletonGraphic pet0;
    
    [SerializeField] private SkeletonGraphic pet1;

    [SerializeField] private TextMeshProUGUI petDesc0;
    [SerializeField] private TextMeshProUGUI petDesc1;

    [SerializeField] private TextMeshProUGUI petName0;
    [SerializeField] private TextMeshProUGUI petName1;

    [SerializeField] private Image passImage;
    
    private int _selectedIdx = 0;
    private void Initialize()
    {
        CreateLeftCellButton();
    }

    private void CreateLeftCellButton()
    {
        var tableData = TableManager.Instance.InAppPurchase.dataArray;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].PASSPRODUCTTYPE != PassProductType.PetPass) continue;
            var prefab = Instantiate<SelectablePetPassButton>(this.prefab, leftCellParent);
            prefab.Initialize(tableData[i].Absoluteid,this);
        }
    }

    public void OnSelectPetPassButton(int idx)
    {
        _selectedIdx = idx;
        SetPetDescription();
        this.UpdateContents(_passInfosList[idx].ToArray());
        scroller.SetTotalCount(_passInfosList[idx].Count);
        scroller.JumpTo(0);
        petPassBuyButton.SetPassKey(_passInfosList[idx][0].passInfo.shopId);
        petPassBuyButton.Subscribe();
        GetGoodsIndicator(idx);
        _seasonKillCountIndicator2.ChangeKey(_passInfosList[idx][0].passInfo.key0);
    }

    private void SetPetDescription()
    {
        var tableData = TableManager.Instance.PetPass.dataArray;

        var addCount = _selectedIdx*40;
        for (int i = 0 + addCount; i < tableData.Length; i++)
        {
            if (tableData[i].REWARDITEMTYPE == RewardItemType.PassItem)
            {
                var pet0Type = (Item_Type)(tableData[i].Reward1);
                var pet1Type = (Item_Type)(tableData[i].Reward2);
                var petString = (pet0Type).ToString().Replace("pet","");
                var petString2 = (pet1Type).ToString().Replace("pet","");

                var petIdx =  int.Parse(petString);
                var petIdx2 =  int.Parse(petString2);
                pet0.Clear();
                pet0.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[petIdx];
                pet0.Initialize(true);
                pet0.SetMaterialDirty();
                if (pet0Type == Item_Type.pet53)
                {
                    var scale = 1f;
                    pet0.gameObject.transform.localScale = new Vector3(scale,scale,1f);
                    pet0.gameObject.transform.localPosition = new Vector3(0f,-98f,1f);

                }
                else if (pet0Type == Item_Type.pet55)
                {
                    var scale = 1f;
                    pet0.gameObject.transform.localScale = new Vector3(scale,scale,1f);
                    pet0.gameObject.transform.localPosition = new Vector3(2.5f,-13,1f);
                }
                else if (pet0Type == Item_Type.pet57)
                {
                    var scale = 1f;
                    pet0.gameObject.transform.localScale = new Vector3(scale,scale,1f);
                    pet0.gameObject.transform.localPosition = new Vector3(2.5f,-72f,1f);
                }
                pet1.Clear();
                pet1.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[petIdx2];
                pet1.Initialize(true);
                pet1.SetMaterialDirty();
                if (pet1Type == Item_Type.pet54)
                {
                    var scale = 1f;
                    pet1.gameObject.transform.localScale = new Vector3(scale,scale,1f);
                    pet1.gameObject.transform.localPosition = new Vector3(0f,-234,1f);
                }
                else if (pet1Type == Item_Type.pet56)
                {
                    var scale = 1f;
                    pet1.gameObject.transform.localScale = new Vector3(scale,scale,1f);
                    pet1.gameObject.transform.localPosition = new Vector3(0f,-180.4f,1f);
                }
                else if (pet1Type == Item_Type.pet58)
                {
                    var scale = 1f;
                    pet1.gameObject.transform.localScale = new Vector3(scale,scale,1f);
                    pet1.gameObject.transform.localPosition = new Vector3(0f,-211.4f,1f);
                }


                var petTableData = TableManager.Instance.PetTable.dataArray;

                var pet0Data = petTableData[petIdx];
                
                string desc0 = "";
                if (pet0Data.Hasvalue1 > 0)
                {
                    desc0 +=
                        $"{CommonString.GetStatusName((StatusType)pet0Data.Hastype1)} ({Utils.ConvertNum(pet0Data.Hasvalue1 * 100)})";
                }

                if (pet0Data.Hasvalue2 > 0)
                {
                    desc0 +=
                        $"\n{CommonString.GetStatusName((StatusType)pet0Data.Hastype2)} ({Utils.ConvertNum(pet0Data.Hasvalue2 * 100)})";
                }

                if (pet0Data.Hasvalue3 > 0)
                {
                    desc0 +=
                        $"\n{CommonString.GetStatusName((StatusType)pet0Data.Hastype3)} ({Utils.ConvertNum(pet0Data.Hasvalue3 * 100)})";
                }

                if (pet0Data.Hasvalue4 > 0)
                {
                    desc0 +=
                        $"\n{CommonString.GetStatusName((StatusType)pet0Data.Hastype4)} ({Utils.ConvertNum(pet0Data.Hasvalue4 * 100)})";
                }

                petDesc0.SetText(desc0);
                petName0.SetText(pet0Data.Name);
                var pet1Data = petTableData[petIdx2];
                
                string desc1 = "";
                if (pet1Data.Hasvalue1 > 0)
                {
                    desc1 +=
                        $"{CommonString.GetStatusName((StatusType)pet1Data.Hastype1)} ({Utils.ConvertNum(pet1Data.Hasvalue1 * 100)})";
                }

                if (pet1Data.Hasvalue2 > 0)
                {
                    desc1 +=
                        $"\n{CommonString.GetStatusName((StatusType)pet1Data.Hastype2)} ({Utils.ConvertNum(pet1Data.Hasvalue2 * 100)})";
                }

                if (pet1Data.Hasvalue3 > 0)
                {
                    desc1 +=
                        $"\n{CommonString.GetStatusName((StatusType)pet1Data.Hastype3)} ({Utils.ConvertNum(pet1Data.Hasvalue3 * 100)})";
                }

                if (pet1Data.Hasvalue4 > 0)
                {
                    desc1 +=
                        $"\n{CommonString.GetStatusName((StatusType)pet1Data.Hastype4)} ({Utils.ConvertNum(pet1Data.Hasvalue4 * 100)})";
                }

                petDesc1.SetText(desc1);
                petName1.SetText(pet1Data.Name);

                passImage.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData[i].Reward2);
                
                
                
                
                
                break;
            }
        }
    }
    
    private void GetGoodsIndicator(int idx)
    {
        var tableData = TableManager.Instance.PetPass.dataArray;
        
        List<RewardItem> rewardItems = new List<RewardItem>(); 
        
        for (int i = 0; i < tableData.Length; i++)
        {
            var passNum = int.Parse(tableData[i].Shopid.Replace("petpass", ""));
            if (passNum > idx) break;
            if (passNum < idx) continue;
            if(Utils.IsPetItem((Item_Type)tableData[i].Reward1))continue;
            if(Utils.IsPetItem((Item_Type)tableData[i].Reward2))continue;
            
            Utils.AddOrUpdateReward(ref rewardItems,(Item_Type)tableData[i].Reward1,tableData[i].Reward1_Value);
            Utils.AddOrUpdateReward(ref rewardItems,(Item_Type)tableData[i].Reward2,tableData[i].Reward2_Value);
        }

        using var e = rewardItems.GetEnumerator();
        var count = rewardItems.Count;
        e.MoveNext();
        for (int i = 0; i < goodsIndicator.Count; i++)
        {
            if (i<count)
            {
                goodsIndicator[i].gameObject.SetActive(true);
                goodsIndicator[i].Initialize(e.Current.ItemType);
                e.MoveNext();
            }
            else
            {
                goodsIndicator[i].gameObject.SetActive(false);
            }
        }
    }
    private bool CanGetReward(double require,string killKey)
    {
        var killCount = (int)ServerData.userInfoTable_2.GetTableData(killKey).Value;
        return killCount >= require;
    }
    private bool IsAdRewarded(string key,int id)
    {
        return int.Parse(ServerData.seolPassServerTable.TableDatas[key].Value) >= id;
    }
    private bool IsFreeRewarded(string key, int id)
    {
        return int.Parse(ServerData.seolPassServerTable.TableDatas[key].Value) >= id;
    }
    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[_passInfosList[_selectedIdx][0].passInfo.shopId].buyCount.Value > 0;

        return hasIapProduct;
    }
    public void OnClickAllReceive()
    {
        
        var tableData = TableManager.Instance.PetPass.dataArray;

        int rewardedNum = 0;

        List<int> typeList = new List<int>();
        
        string free = ServerData.seolPassServerTable.TableDatas[_passInfosList[_selectedIdx][0].passInfo.rewardType_Free_Key].Value;
        string ad = ServerData.seolPassServerTable.TableDatas[_passInfosList[_selectedIdx][0].passInfo.rewardType_IAP_Key].Value;

        int freeIdx = int.Parse(free);
        int adIdx = int.Parse(ad);

        bool isPassItem = false;
        var addCount = _selectedIdx*40;
        for (int i = freeIdx+1+addCount; i < tableData.Length; i++)
        {
            if (CanGetReward(tableData[i].Unlockamount,tableData[i].Unlock_Key) == false) break;

            if (tableData[i].REWARDITEMTYPE != RewardItemType.GoodsItem)
            {
                isPassItem = true;
                break;
            }

            //무료보상
            if (IsFreeRewarded(tableData[i].Reward1_Key, tableData[i].Id) == false)
            {
                freeIdx++;
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                rewardedNum++;
                if(!typeList.Contains(tableData[i].Reward1))
                {
                    typeList.Add(tableData[i].Reward1);
                }
            }
        }
        for (int i = adIdx+1+addCount; i < tableData.Length; i++)
        {
            if (HasPassItem() == false) break;

            if (CanGetReward(tableData[i].Unlockamount,tableData[i].Unlock_Key) == false) break;
            
            if (tableData[i].REWARDITEMTYPE != RewardItemType.GoodsItem)
            {
                isPassItem = true;
                break;
            }
            
            //유료보상
            if (IsAdRewarded(tableData[i].Reward2_Key, tableData[i].Id) == false)
            {
                adIdx++;
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                rewardedNum++;
                if(!typeList.Contains(tableData[i].Reward2))
                {
                    typeList.Add(tableData[i].Reward2);
                }
            }
        }

        if (rewardedNum > 0)
        {
            ServerData.seolPassServerTable.TableDatas[_passInfosList[_selectedIdx][0].passInfo.rewardType_Free_Key].Value = freeIdx.ToString();
            ServerData.seolPassServerTable.TableDatas[_passInfosList[_selectedIdx][0].passInfo.rewardType_IAP_Key].Value = adIdx.ToString();

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();

            using var e = typeList.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(_passInfosList[_selectedIdx][0].passInfo.rewardType_Free_Key, ServerData.seolPassServerTable.TableDatas[_passInfosList[_selectedIdx][0].passInfo.rewardType_Free_Key].Value);
            passParam.Add(_passInfosList[_selectedIdx][0].passInfo.rewardType_IAP_Key, ServerData.seolPassServerTable.TableDatas[_passInfosList[_selectedIdx][0].passInfo.rewardType_IAP_Key].Value);

            transactions.Add(TransactionValue.SetUpdate(SeolPassServerTable.tableName, SeolPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);

                //LogManager.Instance.SendLogType("GCPass", "A", "A");
            });
        }
        else
        {
            if (isPassItem)
            {
                PopupManager.Instance.ShowAlarmMessage("환수를 직접 수령해주세요!");
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
            }
        }

    }
    
    [SerializeField]
    private Scroller scroller;
    
    
    [SerializeField] GameObject cellPrefab = default;

    protected override GameObject CellPrefab => cellPrefab;
    
    private void Start()
    {
        Initialize();
        
        scroller.Initialize(TypeScroll.None);
            
        scroller.OnValueChanged(UpdatePosition);
    
        var tableData = TableManager.Instance.PetPass.dataArray;
    
        List<PassData_Fancy> passInfos = new List<PassData_Fancy>();
    
        for (int i = 0; i < tableData.Length; i++)
        {
            var passInfo = new PassInfo();
    
            passInfo.require = (int)tableData[i].Unlockamount;
            passInfo.id = tableData[i].Id;
    
            passInfo.rewardType_Free = tableData[i].Reward1;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = tableData[i].Reward1_Key;
    
            passInfo.rewardType_IAP = tableData[i].Reward2;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = tableData[i].Reward2_Key;

            passInfo.shopId = tableData[i].Shopid;

            passInfo.passGrade = tableData[i].Reward_Id;

            passInfo.key0 = tableData[i].Unlock_Key;
            
            passInfos.Add(new PassData_Fancy(passInfo));
            
            if (i + 1 >= tableData.Length || tableData[i + 1].Shopid.Equals(tableData[i].Shopid) == false)
            {
                _passInfosList.Add(new List<PassData_Fancy>(passInfos));
                passInfos.Clear();
            }
        }
        
        OnSelectPetPassButton(0);
    }
}
