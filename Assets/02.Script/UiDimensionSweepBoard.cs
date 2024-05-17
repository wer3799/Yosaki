using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UiDimensionSweepBoard : MonoBehaviour
{
    
    [SerializeField] private DimensionEquiepmentCollectionCell sweepCell;
    [SerializeField] private Transform sweepCellParent;
    [SerializeField] private TextMeshProUGUI titleText; 
    [SerializeField] private TextMeshProUGUI gradeProbText; 
    [SerializeField] private TextMeshProUGUI getEquipmentText; 
    [SerializeField] private TextMeshProUGUI levelProbText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI sweepValue;
    private List<DimensionEquiepmentCollectionCell> cellContainer = new List<DimensionEquiepmentCollectionCell>();

    [Header("Rewards")] 
    [SerializeField] private GameObject rewardPopup;
    
    [SerializeField] private ItemView rewardCell0;
    [SerializeField] private Transform rewardCellParent0;
    [SerializeField] private DimensionEquiepmentCollectionCell rewardCell1;
    [SerializeField] private Transform rewardCellParent1;
    private List<ItemView> rewardCellContainer0 = new List<ItemView>();
    private List<DimensionEquiepmentCollectionCell> rewardCellContainer1 = new List<DimensionEquiepmentCollectionCell>();
      
    private void OnEnable()
    {
        var idx = Mathf.Min((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value, GetLength());

        currentId = idx;
        SetUI(currentId);
        UpdateButtonState();
    }

    private int currentId = 0;
    private void SetUI(int idx)
    {
        var tableData = TableManager.Instance.DimensionDungeon.dataArray[idx];
        titleText.SetText($"{idx+1}단계 소탕 정보");

        var prob = string.Empty;


        for (int i = 0; i < GameBalance.dimensionGachaProbs.Count; i++)
        {
            prob +=
                $"{GameBalance.dimensionGachaProbs.Count - i}등급 {GameBalance.dimensionGachaProbs[GameBalance.dimensionGachaProbs.Count - i - 1] * 100}% ";
        }
        levelProbText.SetText(prob+"확률로 획득");

        var essenceBuff = PlayerStats.GetDimensionEssenceGainPer();
        
        sweepValue.SetText($"{tableData.Sweepvalue}(+{tableData.Sweepvalue*essenceBuff})");
        getEquipmentText.SetText($"소탕권 1개당 장비 {tableData.Gachacount}개 획득");
        
        var minimum = 8;
        var maximum = 0;

        var gradeProb= string.Empty;
        
        if (tableData.Gachalv1 > 0)
        {
            minimum = Mathf.Min(0, minimum);
            maximum = Mathf.Max(0, maximum);
            gradeProb += $"하급 : {Utils.ConvertNum(tableData.Gachalv1*100,3)}% ";
        }
        if (tableData.Gachalv2 > 0)
        {
            minimum = Mathf.Min(1, minimum);
            maximum = Mathf.Max(1, maximum);
            gradeProb += $"중급 : {Utils.ConvertNum(tableData.Gachalv2*100,3)}% ";

        }
        if (tableData.Gachalv3 > 0)
        {
            minimum = Mathf.Min(2, minimum);
            maximum = Mathf.Max(2, maximum);
            gradeProb += $"상급 : {Utils.ConvertNum(tableData.Gachalv3*100,3)}% ";
        }
        if (tableData.Gachalv4 > 0)
        {
            minimum = Mathf.Min(3, minimum);
            maximum = Mathf.Max(3, maximum);
            gradeProb += $"특급 : {Utils.ConvertNum(tableData.Gachalv4*100,3)}% ";
        }
        if (tableData.Gachalv5 > 0)
        {
            minimum = Mathf.Min(4, minimum);
            maximum = Mathf.Max(4, maximum);
            gradeProb += $"전설 : {Utils.ConvertNum(tableData.Gachalv5*100,3)}% ";
        }
        if (tableData.Gachalv6 > 0)
        {
            minimum = Mathf.Min(5, minimum);
            maximum = Mathf.Max(5, maximum);
            gradeProb += $"요물 : {Utils.ConvertNum(tableData.Gachalv6*100,3)}% ";
        }
        if (tableData.Gachalv7 > 0)
        {
            minimum = Mathf.Min(6, minimum);
            maximum = Mathf.Max(6, maximum);
            gradeProb += $"신물 : {Utils.ConvertNum(tableData.Gachalv7*100,3)}% ";
        }
        if (tableData.Gachalv8 > 0)
        {
            minimum = Mathf.Min(7, minimum);
            maximum = Mathf.Max(7, maximum);
            gradeProb += $"영물 : {Utils.ConvertNum(tableData.Gachalv8*100,3)}% ";
        }
        
        gradeProbText.SetText(gradeProb);
        
        var equipmentData = TableManager.Instance.DimensionEquip.dataArray;

        if (cellContainer.Count < equipmentData.Length)
        {
            for (int i = 0; i < equipmentData.Length; i++)
            {
                var prefab = Instantiate(sweepCell, sweepCellParent);
                cellContainer.Add(prefab);
                cellContainer[i].Initialize(equipmentData[i]);
            }
        }
        

        for (var i = 0; i < cellContainer.Count; i++)
        {
            if (i < (minimum-1)*5||i>=(maximum+1)*5)
            {
                cellContainer[i].gameObject.SetActive(false);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(true);

            }
        }

    }
    // Start is called before the first frame update
    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

        SetUI(currentId);

        UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        currentId++;

        currentId = Mathf.Min(currentId, GetLength());

        SetUI(currentId);

        UpdateButtonState();
    }
    
    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != GetLength();
    }
    private int GetLength()
    {
        return TableManager.Instance.DimensionDungeon.dataArray.Length - 1;
    }

    public void OnClickSweepButton()
    {
        
        if (ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DCT)}이 부족합니다!");
            return;
        }
        
        var score = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value;

        if (score <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        score = Mathf.Min(score, GetLength());
        var tableData = TableManager.Instance.DimensionDungeon.dataArray[score];
        var sweepValue = tableData.Sweepvalue;
        var sweepType = tableData.Sweeptype;


           
            if (ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value < 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DCT)}이 부족합니다!");
                return;
            }
            

            List<float> probs = new List<float>();

            var startIdx = 8;
            if (tableData.Gachalv1 > 0)
            {
                probs.Add(tableData.Gachalv1);
                startIdx = Mathf.Min(startIdx, 0);
            }
            if (tableData.Gachalv2 > 0)
            {
                probs.Add(tableData.Gachalv2);
                startIdx = Mathf.Min(startIdx, 1);
            }
            if (tableData.Gachalv3 > 0)
            {
                probs.Add(tableData.Gachalv3);
                startIdx = Mathf.Min(startIdx, 2);
            }
            if (tableData.Gachalv4 > 0)
            {
                probs.Add(tableData.Gachalv4);
                startIdx = Mathf.Min(startIdx, 3);
            }
            if (tableData.Gachalv5 > 0)
            {
                probs.Add(tableData.Gachalv5);
                startIdx = Mathf.Min(startIdx, 4);
            }
            if (tableData.Gachalv6 > 0)
            {
                probs.Add(tableData.Gachalv6);
                startIdx = Mathf.Min(startIdx, 5);
            }
            if (tableData.Gachalv7 > 0)
            {
                probs.Add(tableData.Gachalv7);
                startIdx = Mathf.Min(startIdx, 6);
            }
            if (tableData.Gachalv8 > 0)
            {
                probs.Add(tableData.Gachalv8);
                startIdx = Mathf.Min(startIdx, 7);
            }
          
            Dictionary<int, int> rewards =new Dictionary<int, int>();

            for (int i = 0; i < 1 * tableData.Gachacount; i++)
            {
                var grade = Utils.GetRandomIdx(probs)+startIdx;
                var level = (4-Utils.GetRandomIdx(GameBalance.dimensionGachaProbs));

                var equipIdx = grade*5+level;
                
                Utils.AddOrUpdateValue(ref rewards, equipIdx, 1);
            }

            var sortedRewards = rewards.OrderByDescending(x => x.Key);
            
            var equipData = TableManager.Instance.DimensionEquip.dataArray;

            while (tableData.Gachacount > rewardCellContainer1.Count)
            {
                var prefab = Instantiate(rewardCell1, rewardCellParent1);
                rewardCellContainer1.Add(prefab);
            }

            

            for (int i = 0; i < rewardCellContainer1.Count; i++)
            {
                if (i < tableData.Gachacount)
                {
                    rewardCellContainer1[i].gameObject.SetActive(true);
                }
                else
                {
                    rewardCellContainer1[i].gameObject.SetActive(false);
                }
            }

            using var e = sortedRewards.GetEnumerator();

            var idx = 0;
            var max = 0;

            List<TransactionValue> transactions = new List<TransactionValue>();


            Param goodsParam = new Param();
            Param equipParam = new Param();
            

            Dictionary<Item_Type,float> goodsRewards = new Dictionary<Item_Type,float>();
            
            var cubeBuff = 1 + PlayerStats.GetDimensionCubeGainPer();
            var essenceBuff = 1 + PlayerStats.GetDimensionEssenceGainPer();
            
            while (e.MoveNext())
            {
                var equipValue = ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value;

                var data = equipData[e.Current.Key];
                for (int i = 0; i < e.Current.Value; i++)
                {
                    var isChange = false;
                    
                    max = Mathf.Max(max, e.Current.Key);

                    if (max > equipValue)
                    {
                        isChange = true;
                        //교체된 건 분해취소
                        Utils.AddOrUpdateValue(ref goodsRewards, (Item_Type)data.Decompositiontype, (int)(-data.Decompositionvalue*cubeBuff));
                        //교체되기전장비 분해
                        var beforeEquipData = equipData[equipValue];
                        Utils.AddOrUpdateValue(ref goodsRewards, (Item_Type)beforeEquipData.Decompositiontype, (int)(beforeEquipData.Decompositionvalue*cubeBuff));
                        
                        ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value = max;
                        equipParam.Add(EquipmentTable.DimensionEquipment, ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value);

                        equipValue = ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value;;
                    }

                    rewardCellContainer1[idx].Initialize(data, isChange);
                    idx++;
                }


                Utils.AddOrUpdateValue(ref goodsRewards, (Item_Type)data.Decompositiontype, (int)(data.Decompositionvalue * cubeBuff) * e.Current.Value);
            }

            Utils.AddOrUpdateValue(ref goodsRewards, (Item_Type)sweepType, (int)(sweepValue * essenceBuff));

            using var e1 = goodsRewards.GetEnumerator();

            while (goodsRewards.Count > rewardCellContainer0.Count)
            {
                var prefab = Instantiate(rewardCell0, rewardCellParent0);
                rewardCellContainer0.Add(prefab);
            }          
            
            for (int i = 0; i < rewardCellContainer0.Count; i++)
            {
                if (i < goodsRewards.Count)
                {
                    rewardCellContainer0[i].gameObject.SetActive(true);
                }
                else
                {
                    rewardCellContainer0[i].gameObject.SetActive(false);
                }
            }

            var goodsIdx = 0;
            while (e1.MoveNext())
            {
                var key = ServerData.goodsTable.ItemTypeToServerString(e1.Current.Key);
                ServerData.goodsTable.GetTableData(key).Value += e1.Current.Value;
                goodsParam.Add(key, ServerData.goodsTable.GetTableData(key).Value);
                rewardCellContainer0[goodsIdx].Initialize(e1.Current.Key,e1.Current.Value);
                goodsIdx++;
            }

            ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value -= 1;
            goodsParam.Add(GoodsTable.DCT, ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            transactions.Add(TransactionValue.SetUpdate(EquipmentTable.tableName, EquipmentTable.Indate, equipParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                rewardPopup.SetActive(true);
            });
    }
    
}