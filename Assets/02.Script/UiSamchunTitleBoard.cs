using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using WebSocketSharp;

public class UiSamchunTitleBoard : SingletonMono<UiSamchunTitleBoard>
{
    [SerializeField] private TextMeshProUGUI titleDesc;
    [SerializeField] private TextMeshProUGUI totalAbilityDesc;
    
    [SerializeField] private UiSamchunTitleTabCell tabCell;
    [SerializeField] private Transform tabParent;

    [SerializeField] private UiSamchunTitleAbilityCell abilityCell;
    [SerializeField] private Transform abilityParent;

    
    private List<UiSamchunTitleAbilityCell> abilityCellContainer = new List<UiSamchunTitleAbilityCell>();


    private Dictionary<int, List<Title_SamcheonData>> dataDic = new Dictionary<int, List<Title_SamcheonData>>();

    private bool isInitialized = false;
    private void Start()
    {
        MakeTabCell();
        MakeDict();


        RefreshUi();
        OnSelectTab(0);
        isInitialized = true;
    }

    private void OnEnable()
    {
        if (isInitialized)
        {
            OnSelectTab(0);
        }
    }

    private void MakeTabCell()
    {
        var contentsCount = GameBalance.samchunTitle.Count; 
        for (int i = 0; i < contentsCount; i++)
        {
            var prefab = Instantiate<UiSamchunTitleTabCell>(tabCell, tabParent);
            prefab.Initialize(i);
        }
    }

    private void MakeDict()
    {
        var tableData = TableManager.Instance.Title_Samcheon.dataArray;

        var contentsCount = GameBalance.samchunTitle.Count;
        for (var i = 0; i < contentsCount; i++)
        {
            var list = tableData.Where(t => t.Tabtype == i).ToList();

            dataDic.Add(i,list);
        }
    }
    
    public void OnSelectTab(int idx)
    {
        titleDesc.SetText($"{GameBalance.samchunTitle[idx]} 업적");
        
        var list = dataDic[idx];

        var cellCount = list.Count;
        
        while (cellCount > abilityCellContainer.Count)
        {
            var prefab = Instantiate<UiSamchunTitleAbilityCell>(abilityCell,abilityParent);
            abilityCellContainer.Add(prefab);
        }
        
        for (var i = 0; i < abilityCellContainer.Count; i++)
        {
            if (i < cellCount)
            {
                abilityCellContainer[i].gameObject.SetActive(true);
                abilityCellContainer[i].Initialize(list[i]);
            }
            else
            {
                abilityCellContainer[i].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshUi()
    {
        SetAbilityText();
    }

    private void SetAbilityText()
    {
        var tableData = TableManager.Instance.Title_Samcheon.dataArray;

        var dic = new Dictionary<StatusType, float>();
        
        for(int i = 0 ; i < tableData.Length;i++)
        {
            if(ServerData.samchunTitleServerTable.TableDatas[tableData[i].Stringid].hasReward.Value<1) continue;
            
            Utils.AddOrUpdateValue(ref dic, (StatusType)tableData[i].Abiltype, tableData[i].Abilvalue);
        }

        if (dic.Count < 1)
        {
            totalAbilityDesc.SetText("능력치 없음");
        }
        else
        {
            using var e = dic.GetEnumerator();
            var desc = string.Empty;
            while (e.MoveNext())
            {
                if (desc.IsNullOrEmpty())
                {
                    if (e.Current.Key.IsPercentStat())
                    {
                        desc += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value*100,4)}";
                    }
                    else
                    {
                        desc += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value)}";
                    }
                }
                else
                {
                    if (e.Current.Key.IsPercentStat())
                    {
                        desc += $"\n{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value*100,4)}";
                    }
                    else
                    {
                        desc += $"\n{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value)}";
                    }                }
            }
            
            totalAbilityDesc.SetText(desc);
        }

    }
}
