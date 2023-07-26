using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;


public class UiSAMinigameRewardCell : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon_free;

    [SerializeField]
    private TextMeshProUGUI itemName_free;

    [SerializeField]
    private TextMeshProUGUI itemAmount_free;


    [SerializeField]
    private GameObject lockIcon_Free;


    private DamagePassInfo passInfo;

    [SerializeField]
    private GameObject rewardedObject_Free;

    [SerializeField]
    private Image gaugeImage;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private CompositeDisposable disposables = new CompositeDisposable();

    private SecondMiniGameData tableData;
    
    private void OnDestroy()
    {
        disposables.Dispose();
    }

    private void Subscribe()
    {
        disposables.Clear();

        ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_Free_Key, passInfo.id);
            rewardedObject_Free.SetActive(rewarded);
        }).AddTo(disposables);
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMiniGameScore_Total).Subscribe(e =>
        {
            lockIcon_Free.SetActive(CanGetReward()==false);
            float gaugeValue = 0; 
            if (passInfo.require - e > 100)
            {
                gaugeValue = 0;
            }
            else
            {
                gaugeValue = (float)(e - tableData.Accumlatescoregap) / (float)(passInfo.require - tableData.Accumlatescoregap);
            }
            UpdateGauge(gaugeValue);

        }).AddTo(disposables);

    }


    private void UpdateGauge(float newValue)
    {
        gaugeImage.fillAmount = newValue;
    }

    public void Initialize(DamagePassInfo passInfo)
    {
        this.passInfo = passInfo;

        tableData = TableManager.Instance.SecondMiniGame.dataArray[passInfo.id];

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

        Subscribe();
    }

    private void SetAmount()
    {
        itemAmount_free.SetText(Utils.ConvertBigNum(passInfo.rewardTypeValue_Free));
    }

    private void SetItemIcon()
    {
        itemIcon_free.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)passInfo.rewardType_Free);

        itemName_free.SetText(CommonString.GetItemName((Item_Type)(int)passInfo.rewardType_Free));
    }

    private void SetDescriptionText()
    {
        descriptionText.SetText($"{Utils.ConvertBigNumForRewardCell(passInfo.require)}");
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.coldSeasonPassServerTable.TableDatas[key].Value.Split(',').ToList();
    }

    public bool HasReward(string key, int data)
    {
        var splitData = GetSplitData(key);
        return splitData.Contains(data.ToString());
    }

    public void OnClickFreeRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("누적 점수가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_Free_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");

        GetFreeReward();

    }
    

    private void GetFreeReward()
    {
        //로컬
        ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value += $",{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_Free, passInfo.rewardTypeValue_Free);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_Free_Key, ServerData.coldSeasonPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(ColdSeasonPassServerTable.tableName, ColdSeasonPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_Free);
        transactionList.Add(rewardTransactionValue);

        ServerData.SendTransactionV2(transactionList);
    }
    private bool CanGetReward()
    {
        var score = ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMiniGameScore_Total).Value;
        return score >= (double)passInfo.require;   
        
    }
}
