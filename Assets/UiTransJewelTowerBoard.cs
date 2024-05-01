using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UiRewardView;

public class UiTransJewelTowerBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentStageText_Real;

    [SerializeField]
    private TMP_InputField instantClearNum;

    [SerializeField] private List<ItemView> itemViews =new List<ItemView>();
    private int currentId;
    [SerializeField]
    private TextMeshProUGUI stageDescription;
    [SerializeField]
    private TextMeshProUGUI stageNeedDamageDescription;
    [SerializeField]
    private TextMeshProUGUI stageNeedEquipmentDescription;
    [SerializeField]
    private TextMeshProUGUI currentEquipmentDescription;
    [SerializeField]
    private TextMeshProUGUI damageDescrpition;
    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;
    private bool initialized = false;
    private void Start()
    {
        initialized = true;
    }

    void OnEnable()
    {
        var idx = Mathf.Max(PlayerStats.GetTransJewelTowerGrade(), 0);

        UpdateRewardView(idx);
        SetStageText();
        currentEquipmentDescription.SetText($"초월한 장비 수 : {PlayerStats.GetTransEquipmentCount()}");
    }


    private void SetStageText()
    {
        int currentFloor = PlayerStats.GetTransJewelTowerGrade();
        if (currentFloor < 0)
        {
            currentStageText_Real.SetText($"단계 미충족");
        }
        else
        {
            currentStageText_Real.SetText($"현재 {currentFloor+1}단계");
        }
        
        damageDescrpition.SetText($"{Utils.ConvertNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.transJewelScore].Value * GameBalance.BossScoreConvertToOrigin)}");
    }


    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.TransJewelTower); }, () => { });
    }
    public void UpdateRewardView(int idx)
    {
        currentId = idx;

        stageDescription.SetText($"초월 광산 {currentId + 1}단계");

        Item_Type rewardType = Item_Type.None; 
        float rewardValue = 0f;

        var day = Utils.GetDayOfWeek();
        var tableData = TableManager.Instance.TransJewelTower.dataArray[day];
        
        stageNeedEquipmentDescription.SetText($"필요 초월 장비 수 : { tableData.Unlocktranscount[idx]}");
        stageNeedDamageDescription.SetText($"{Utils.ConvertNum(tableData.Rewardcut[idx])}");
        var length = tableData.Rewardtype.Length;

        using var e = itemViews.GetEnumerator();
        
        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(false);    
        }

        for (int i = 0; i < length; i++)
        {
            itemViews[i].gameObject.SetActive(true);
            itemViews[i].Initialize((Item_Type)tableData.Rewardtype[i],tableData.Rewardvalue[idx]);
        }
        UpdateButtonState();
    }
    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        currentId++;

        currentId = Mathf.Min(currentId,TableManager.Instance.TransJewelTower.dataArray[0].Rewardvalue.Length - 1);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != TableManager.Instance.TransJewelTower.dataArray[0].Rewardvalue.Length - 1;
    }
    public void OnClickInstantClearButton()
    {

        int currentClearStageId = PlayerStats.GetTransJewelTowerGrade();

        if (currentClearStageId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("초월 광산을 클리어 해야 합니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.TJCT].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(Item_Type.TJCT),JongsungType.Type_IGA)} 부족합니다.");

            return;
        }

        if (int.TryParse(instantClearNum.text, out var inputNum))
        {
            if (inputNum < 0)
            {
                PopupManager.Instance.ShowAlarmMessage(CommonString.InstantClear_Minus);
                return;
            }
            if (inputNum == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                return;
            }
            if (inputNum > 200)
            {
                PopupManager.Instance.ShowAlarmMessage("소탕권은 200개 미만으로 사용가능합니다!");
                return;
            }
            else if (remainItemNum < inputNum)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(Item_Type.TJCT),JongsungType.Type_IGA)} 부족합니다.");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }


        var week = Utils.GetDayOfWeek();
        var tableData = TableManager.Instance.TransJewelTower.dataArray[week];
        int instantClearGetNum = (int)tableData.Rewardvalue[currentClearStageId] * inputNum;

        string desc = "";

        string rewardsName = "";
        string each = "";

        for (int i = 0; i < tableData.Rewardtype.Length; i++)
        {
            rewardsName += $"{CommonString.GetItemName((Item_Type)tableData.Rewardtype[i])},";
        }

        if (tableData.Rewardtype.Length > 1)
        {
            each += "각각";
        }
        
        rewardsName = rewardsName.Remove(rewardsName.Length - 1);

        desc += $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{rewardsName} {each} {instantClearGetNum}개를\n획득 하시겠습니까?";

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
        desc,
        () =>
        {
            int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.TJCT].Value;
    
            if (remainItemNum <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(Item_Type.TJCT),JongsungType.Type_IGA)} 부족합니다.");

    
                return;
            }
    
            if (int.TryParse(instantClearNum.text, out var inputNum))
            {
                if (inputNum < 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(CommonString.InstantClear_Minus);
                    return;
                }
                if (inputNum == 0)
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }
                else if (remainItemNum < inputNum)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(Item_Type.TJCT),JongsungType.Type_IGA)} 부족합니다.");

                    return;
                }
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                return;
            }
            Param goodsParam = new Param();


            UiRewardResultPopUp.Instance.Clear();

            for (int i = 0; i < tableData.Rewardtype.Length; i++)
            {
                UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)tableData.Rewardtype[i], instantClearGetNum);
            }


            //실제소탕
            ServerData.goodsTable.TableDatas[GoodsTable.TJCT].Value -= inputNum;
            
            using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator(); 
            while (e.MoveNext())
            {
                var str = ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType);
                ServerData.goodsTable.TableDatas[str].Value += instantClearGetNum;
                goodsParam.Add(str, ServerData.goodsTable.TableDatas[str].Value);
            }
            
            List<TransactionValue> transactions = new List<TransactionValue>();
    
            goodsParam.Add(GoodsTable.TJCT, ServerData.goodsTable.TableDatas[GoodsTable.TJCT].Value);
    
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
    
            ServerData.SendTransactionV2(transactions,
                successCallBack: () =>
                {
                    UiRewardResultPopUp.Instance.Show().Clear();
                });
        }, null);
     }
    public void AutoModeOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.towerAutoMode.Value = on ? 1 : 0;
    }
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TJCT).Value += 1;
        }
    }
#endif
}