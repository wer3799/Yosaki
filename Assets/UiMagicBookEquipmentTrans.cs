using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiMagicBookEquipmentTrans : MonoBehaviour
{
    [SerializeField]
    private UiMagicBookTransView ViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI abilList;
    List<UiMagicBookTransView> cellList = new List<UiMagicBookTransView>();

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        UpdateDescription();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.MagicBookTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if ((tableData[i].MAGICBOOKTYPE != MagicBookType.Normal))
            {
                continue;
            }
            var cell = Instantiate<UiMagicBookTransView>(ViewPrefab, cellParent);
            cell.Initialize(tableData[i]);

            cellList.Add(cell);
            

        }
        
        List<(int displayOrder, UiMagicBookTransView gameObject)> equipment = new List<(int, UiMagicBookTransView)>();
        foreach (var cell in cellList)
        {
            if (cell.GetMagicBookData().MAGICBOOKTYPE == MagicBookType.Normal)
            {
                equipment.Add((cell.GetMagicBookData().Displayorder, cell));
            }
        }
        equipment.Sort((a, b) => a.displayOrder.CompareTo(b.displayOrder));

        for (int i = 0; i < equipment.Count; i++)
        {
            UiMagicBookTransView equipmentObject = equipment[i].gameObject;
            equipmentObject.transform.SetSiblingIndex(i);
        }
        

    }
    
    private void UpdateDescription()
    {
        SetAbilText();

    }

    private void SetAbilText()
    {
        
        var tableData = TableManager.Instance.MagicBookTable.dataArray;
        
        var serverData = ServerData.magicBookTable.TableDatas;
        Dictionary<StatusType, float> rewards = new Dictionary<StatusType, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].MAGICBOOKTYPE != MagicBookType.Normal) continue;

            StatusType abilType = (StatusType)tableData[i].Transeffecttype;

            if (rewards.ContainsKey(abilType) == false)
            {
                var ret = PlayerStats.GetMagicBookTransHasValue(abilType);
                if (ret != 0)
                {
                    rewards.Add(abilType, ret);
                }
            }
        }
        
        var e = rewards.GetEnumerator();

        string description = "";

        while (e.MoveNext())
        {
            if (Utils.IsPercentStat(e.Current.Key))
            {
                description += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value * 100f)} 증가\n";
            }
            else
            {
                description += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value)} 증가\n";
            }
        }

        if (rewards.Count == 0)
        {
            abilDescription.SetText("초월된 노리개가 없습니다.");
        }
        else
        {
            abilDescription.SetText(description);
        }

        string abils = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].MAGICBOOKTYPE != MagicBookType.Normal) continue;
            if(Utils.IsPercentStat((StatusType)tableData[i].Transeffecttype))
            {
                abils += $"{tableData[i].Name} 보유 : {CommonString.GetStatusName((StatusType)tableData[i].Transeffecttype)} {Utils.ConvertBigNum(tableData[i].Transeffectvalue * 100f)}\n";
            }
            else
            {
                abils += $"{tableData[i].Name} 보유 : {CommonString.GetStatusName((StatusType)tableData[i].Transeffecttype)} {Utils.ConvertBigNum(tableData[i].Transeffectvalue)}\n";
            }
        }

        abils += "<color=red>모든 효과는 중첩됩니다!</color>";


        abilList.SetText(abils);
    }

}
