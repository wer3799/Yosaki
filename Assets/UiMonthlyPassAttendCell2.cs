using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;

public class UiMonthlyPassAttendCell2 : MonoBehaviour
{
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

    private void OnDestroy()
    {
        disposables.Dispose();
    }
    private void Subscribe()
    {
        disposables.Clear();

        //무료보상 데이터 변경시
        ServerData.monthlyPassServerTable2.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_Free_Key);
            rewardedObject_Free.SetActive(rewarded);

        }).AddTo(disposables);

        //광고보상 데이터 변경시
        ServerData.monthlyPassServerTable2.TableDatas[passInfo.rewardType_IAP_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_IAP_Key);
            rewardedObject_Ad.SetActive(rewarded);

        }).AddTo(disposables);

        //킬카운트 변경될때
        ServerData.userInfoTable.GetTableData(UserInfoTable.monthAttendCount).AsObservable().Subscribe(e =>
        {
            if (this.gameObject.activeInHierarchy) 
            {
                lockIcon_Free.SetActive(!CanGetReward());
                lockIcon_Ad.SetActive(!CanGetReward());
                gaugeImage.SetActive(CanGetReward());
            }
      
        }).AddTo(disposables);
    }

    public void Initialize(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

        Subscribe();

        RefreshParent();
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
        descriptionText.SetText($"{Utils.ConvertBigNum(passInfo.require)}일차");
    }

    public bool HasReward(string key)
    {
        return int.Parse(ServerData.monthlyPassServerTable2.TableDatas[key].Value) >= passInfo.id;

    }
    private bool IsBeforeRewarded(string key)
    {
        //0일때 1
        return int.Parse(ServerData.monthlyPassServerTable2.TableDatas[key].Value) + 1 == passInfo.id;
    }
    public void OnClickFreeRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Can);
            return;
        }

        if (HasReward(passInfo.rewardType_Free_Key))
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Has);
            return;
        }
        if (IsBeforeRewarded(passInfo.rewardType_Free_Key)==false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Before);
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
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Can);
            return;
        }

        if (HasReward(passInfo.rewardType_IAP_Key))
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Has);
            return;
        }
        if (IsBeforeRewarded(passInfo.rewardType_IAP_Key)==false)
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Before);
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
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton2.monthPassKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    private void GetFreeReward()
    {
        //로컬
        ServerData.monthlyPassServerTable2.TableDatas[passInfo.rewardType_Free_Key].Value= $"{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_Free, passInfo.rewardTypeValue_Free);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_Free_Key, ServerData.monthlyPassServerTable2.TableDatas[passInfo.rewardType_Free_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable2.tableName, MonthlyPassServerTable2.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_Free);
        transactionList.Add(rewardTransactionValue);

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            // LogManager.Instance.SendLogType("월간", "무료", $"{passInfo.id}");
        });
        
    }
    private void GetAdReward()
    {
            //로컬
            ServerData.monthlyPassServerTable2.TableDatas[passInfo.rewardType_IAP_Key].Value = $"{passInfo.id}";
            ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_IAP, passInfo.rewardTypeValue_IAP);

            List<TransactionValue> transactionList = new List<TransactionValue>();

            //패스 보상
            Param passParam = new Param();
            passParam.Add(passInfo.rewardType_IAP_Key, ServerData.monthlyPassServerTable2.TableDatas[passInfo.rewardType_IAP_Key].Value);
            transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable2.tableName, MonthlyPassServerTable2.Indate, passParam));

            var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_IAP);
            transactionList.Add(rewardTransactionValue);

            ServerData.SendTransaction(transactionList, successCallBack: () =>
            {
                // LogManager.Instance.SendLogType("월간", "유료", $"{passInfo.id}");
            });

            PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
        

    }

    private bool CanGetReward()
    {
        int attendCountTotal2 = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.monthAttendCount).Value;
        return attendCountTotal2 >= passInfo.require;
    }

    private void OnEnable()
    {
        RefreshParent();
    }

    public void RefreshParent()
    {
        if (passInfo == null) return;

        if (HasPassItem() == false)
        {
            if (CanGetReward() == true && HasReward(passInfo.rewardType_Free_Key) == false)
            {
                this.transform.SetAsFirstSibling();
            }
        }
        else
        {
            if (CanGetReward() == true &&
                (HasReward(passInfo.rewardType_Free_Key) == false || HasReward(passInfo.rewardType_IAP_Key) == false))
            {
                this.transform.SetAsFirstSibling();
            }
        }
    }
}
