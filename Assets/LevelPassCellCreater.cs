using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class PassData_Fancy
{
    public PassInfo passInfo { get; private set; }
    public PassData_Fancy(PassInfo passData)
    {
        this.passInfo = passData;
    }
}
public class LevelPassCellCreater : FancyScrollView<PassData_Fancy>
{
    
    private List<List<PassData_Fancy>> passInfosList = new List<List<PassData_Fancy>>();


    [SerializeField] private UiLEvelPassBuyButton4 _uiLEvelPassBuyButton4;

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _buyText;
    [SerializeField] private TextMeshProUGUI productDescription;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private Image _image;
    private float cellSize = 105.9f;


    private bool initialized = false;

    private int scrollId = 0;

    private Dictionary<int, int> passIdx = new Dictionary<int, int>();

    public void Initialize(int passNum)
    {
        var data = TableManager.Instance.InAppPurchase.dataArray[passIdx[passNum]];
        var levelPassData = TableManager.Instance.LevelPass.dataArray;
        _uiLEvelPassBuyButton4.SetNewString(data.Productid);
        //여우패스1은 passNum+1임.
        int passCount = passNum + 1;
        
        int passMin = 0;
        int passMax = 0;
        for (var i = 0; i < levelPassData.Length; i += 100)
        {
            if (levelPassData[i].Shopid != data.Productid)
            {
                continue;
            }

            passMin = levelPassData[i].Idminmax[0];
            passMax = levelPassData[i].Idminmax[1];
            break;
        }

        var startLevel = 0;
        var endLevel = 0;

        startLevel = (levelPassData[passMin].Unlocklevel/10000)*10000;
        endLevel = levelPassData[passMax].Unlocklevel;

        //색변경
        var spriteIdx = passCount % _sprites.Count;
        _image.sprite = _sprites[spriteIdx];
        _title.color = _colors[spriteIdx];
        //
        _title.SetText($"여우패스 {passCount}");


        _buyText.SetText($"패스 구매 \nLV {Utils.ConvertBigNum(startLevel)}~{Utils.ConvertBigNum(endLevel)}");
    }

    private void SetPassIdx()
    {
        var tableData = TableManager.Instance.InAppPurchase.dataArray;
        int dataCount=0;
        foreach (var data in tableData)
        {
            if (data.PASSPRODUCTTYPE == PassProductType.LevelPass)
            {
                passIdx.Add(dataCount, data.Absoluteid);
                dataCount++;
            }
        }

    }

    private void SetPassDescription(int idx)
    {
        var passInfos = passInfosList[idx];
        
        List<RewardItem> rewards = new List<RewardItem>();
        

        for (int i = 0; i < passInfos.Count; i++)
        {
            Utils.AddOrUpdateReward(ref rewards,(Item_Type)(int)passInfos[i].passInfo.rewardType_IAP,passInfos[i].passInfo.rewardTypeValue_IAP);
        }

        if (rewards.Count > 0)
        {
            using var e =rewards.GetEnumerator();
            string desc = "";
            while (e.MoveNext())
            {
                desc+=$"{CommonString.GetItemName(e.Current.ItemType)} : {Utils.ConvertNum(e.Current.ItemValue)}개\n";
            }
            productDescription.SetText(desc);
        }
        else
        {
            productDescription.SetText("수령할 보상이 없습니다!");
        }
    }
    private void AllPassUpdate()
    {
        scroller.Initialize(TypeScroll.LevelPass);
            
        scroller.OnValueChanged(UpdatePosition);
        
        var tableData = TableManager.Instance.LevelPass.dataArray;
        
        List<PassData_Fancy> passInfos = new List<PassData_Fancy>();    
        //레벨패스 6부터 = 1001
        for (int i = 0; i < tableData.Length; i++)
        {
            var passInfo = new PassInfo();
            
            passInfo.require = tableData[i].Unlocklevel;
            passInfo.id = tableData[i].Id;
        
            passInfo.rewardType_Free = tableData[i].Reward1_Free;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = NewLevelPass.freeReward;
        
            passInfo.rewardType_IAP = tableData[i].Reward2_Pass;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = NewLevelPass.premiumReward;

            passInfo.passGrade = tableData[i].Passgrade;
            
            passInfos.Add(new PassData_Fancy(passInfo));
        
            if (tableData[i].Idminmax[1]==i)
            {
                passInfosList.Add(new List<PassData_Fancy>(passInfos));
                passInfos.Clear();
            }
        }
        
        
        this.UpdateContents(passInfosList[0].ToArray());
        scroller.SetTotalCount(passInfosList[0].Count);
    }
    
    //여우패스1 = _idx=0
    public void ChangeContents(int _idx)
    {
        Initialize(_idx);
        this.UpdateContents(passInfosList[_idx].ToArray());
        scroller.SetTotalCount(passInfosList[_idx].Count);
        scroller.JumpTo(0);
        SetPassDescription(_idx);
    }
    
    [SerializeField]
    private Scroller scroller;
    
    
    [SerializeField] GameObject cellPrefab = default;

    protected override GameObject CellPrefab => cellPrefab;
    
    private void Start()
    {
        SetPassIdx();
        AllPassUpdate();
        Initialize(0);
        SetPassDescription(0);
    }
}
