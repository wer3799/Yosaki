using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UnityEngine.UI.Extensions;

public class UiMonthlyPassCell : FancyCell<MonthlyPassData_Fancy>
{
    private MonthlyPassData_Fancy itemData;
    
    [SerializeField]
    private Image itemIcon_free;

    [SerializeField]
    private TextMeshProUGUI itemName_free;

    [SerializeField]
    private Image itemIcon_ad;

    [SerializeField]
    private TextMeshProUGUI itemName_ad;


    
    [SerializeField]
    private TextMeshProUGUI itemAmount_free;

    [SerializeField]
    private TextMeshProUGUI itemAmount_ad;

    
    [SerializeField]
    private GameObject lockIcon_Free;

    [SerializeField]
    private GameObject lockIcon_Ad;

    private PassInfo passInfo;

    [SerializeField]
    private GameObject rewardedObject_Free;

    [SerializeField]
    private GameObject rewardedObject_Ad;

    
    [SerializeField]
    private GameObject gaugeImage;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private CompositeDisposable disposables = new CompositeDisposable();

    private void ReGetItem()
    {
        if (passInfo.id == 5)
        {
            bool rewarded = HasReward(passInfo.rewardType_IAP_Key, passInfo.id);

            if (HasPassItem() && rewarded && ServerData.costumeServerTable.TableDatas["costume28"].hasCostume.Value == false)
            {
                GetAdReward();
            }
        }
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
    private void Subscribe()
    {
        disposables.Clear();

        //무료보상 데이터 변경시
        ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_Free_Key, passInfo.id);
            rewardedObject_Free.SetActive(rewarded);

        }).AddTo(disposables);

        //광고보상 데이터 변경시
        ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_IAP_Key, passInfo.id);
            rewardedObject_Ad.SetActive(rewarded);

        }).AddTo(disposables);

        //킬카운트 변경될때
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).AsObservable().Subscribe(e =>
        {
                lockIcon_Free.SetActive(!CanGetReward());
                lockIcon_Ad.SetActive(!CanGetReward());
                gaugeImage.SetActive(CanGetReward());
            
        }).AddTo(disposables);
    }

    private void SetAmount()
    {
        itemAmount_free.SetText(Utils.ConvertBigNum(passInfo.rewardTypeValue_Free));
        itemAmount_ad.SetText(Utils.ConvertBigNum(passInfo.rewardTypeValue_IAP));
    }

    private void SetItemIcon()
    {
        itemIcon_free.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)passInfo.rewardType_Free);
        itemIcon_ad.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)passInfo.rewardType_IAP);

        itemName_free.SetText(CommonString.GetItemName((Item_Type)(int)passInfo.rewardType_Free));
        itemName_ad.SetText(CommonString.GetItemName((Item_Type)(int)passInfo.rewardType_IAP));
    }

    private void SetDescriptionText()
    {
        descriptionText.SetText($"{Utils.ConvertBigNum(passInfo.require)}");
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.monthlyPassServerTable.TableDatas[key].Value.Split(',').ToList();
    }

    public bool HasReward(string key, int data)
    {
        return int.Parse(ServerData.monthlyPassServerTable.TableDatas[key].Value) >= data;

    }

    public void OnClickFreeRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("몹 처치가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_Free_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }
        if (GetBeforeRewarded(passInfo.rewardType_Free_Key,passInfo.id) == false)
        {
            PopupManager.Instance.ShowAlarmMessage("이전 보상을 받아주세요!");
            return;
        }
        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");

        GetFreeReward();

    }

    //광고아님
    public void OnClickAdRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("몹 처치가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_IAP_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }
        if (GetBeforeRewarded(passInfo.rewardType_IAP_Key,passInfo.id) == false)
        {
            PopupManager.Instance.ShowAlarmMessage("이전 보상을 받아주세요!");
            return;
        }
        if (HasPassItem())
        {
            GetAdReward();
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"월간 훈련권이 필요합니다.");
        }
    }
    
    

    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton.monthPassKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    private void GetFreeReward()
    {
        if (((Item_Type)(int)passInfo.rewardType_Free).IsPassNorigaeItem())
        {
            //로컬
            ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value = $"{passInfo.id}";

            string key = ((Item_Type)(int)passInfo.rewardType_Free).ToString();
            
            if (ServerData.magicBookTable.TableDatas[key].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"이미 보유하고 있습니다.");
                return;
            }

            List<TransactionValue> transactionList = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas[key].amount.Value += 1;
            ServerData.magicBookTable.TableDatas[key].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add(key, ServerData.magicBookTable.TableDatas[key].ConvertToString());

            transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

            //패스 보상
            Param passParam = new Param();
            passParam.Add(passInfo.rewardType_Free_Key, ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
            transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, passParam));


            //킬카운트
            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable_2.evenMonthKillCount, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value);
            transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfoParam));
            ServerData.SendTransactionV2(transactionList, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(((Item_Type)(int)passInfo.rewardType_Free))} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }   
        else
        {
            //로컬
            ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value = $"{passInfo.id}";
            ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_Free, passInfo.rewardTypeValue_Free);

            List<TransactionValue> transactionList = new List<TransactionValue>();

            //패스 보상
            Param passParam = new Param();
            passParam.Add(passInfo.rewardType_Free_Key, ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
            transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, passParam));

            var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_Free);
            transactionList.Add(rewardTransactionValue);

            //킬카운트
            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable_2.evenMonthKillCount, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value);
            transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfoParam));

            ServerData.SendTransactionV2(transactionList, successCallBack: () =>
            {
                //  LogManager.Instance.SendLogType("월간", "무료", $"{passInfo.id}");
            });

        }

    }
    private void GetAdReward()
    {

        //로컬
        ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value = $"{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_IAP, passInfo.rewardTypeValue_IAP);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_IAP_Key, ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_IAP);
        transactionList.Add(rewardTransactionValue);

        //킬카운트
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable_2.evenMonthKillCount, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfoParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            //   LogManager.Instance.SendLogType("월간", "유료", $"{passInfo.id}");
        });

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
        
    }
    
    private void GetNewReward()
    {

        //로컬
        ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_New_Key].Value = $"{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_New, passInfo.rewardTypeValue_New);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_New_Key, ServerData.monthlyPassServerTable.TableDatas[passInfo.rewardType_New_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_New);
        transactionList.Add(rewardTransactionValue);

        //킬카운트
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable_2.evenMonthKillCount, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfoParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            //   LogManager.Instance.SendLogType("월간", "유료", $"{passInfo.id}");
        });

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
        
    }

    private bool CanGetReward()
    {
        int killCountTotal = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value;
        return killCountTotal >= passInfo.require;
    }
    private bool GetBeforeRewarded(string key,int data)
    {
        return int.Parse(ServerData.monthlyPassServerTable.TableDatas[key].Value) == data - 1;
    }
    public void RefreshParent()
    {
        if (passInfo == null) return;

        if (HasPassItem() == false)
        {
            if (CanGetReward() == true && HasReward(passInfo.rewardType_Free_Key, passInfo.id) == false)
            {
                this.transform.SetAsFirstSibling();
            }
        }
        else
        {
            if (CanGetReward() == true &&
                (HasReward(passInfo.rewardType_Free_Key, passInfo.id) == false || HasReward(passInfo.rewardType_IAP_Key, passInfo.id) == false))
            {
                this.transform.SetAsFirstSibling();
            }
        }
    }
    

    public void UpdateUi(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

        Subscribe();
    }

    public override void UpdateContent(MonthlyPassData_Fancy itemData)
    {
        if (this.itemData != null && this.itemData.passInfo.id == itemData.passInfo.id)
        {
            return;
        }

        this.itemData = itemData;

//        Debug.LogError("DolpasS!");
        
        UpdateUi(this.itemData.passInfo);
    }

    float currentPosition = 0;
    [SerializeField] Animator animator = default;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;

        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }

        animator.speed = 0;
    }

    void OnEnable() => UpdatePosition(currentPosition);
}