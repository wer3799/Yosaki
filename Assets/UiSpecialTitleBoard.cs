using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiSpecialTitleBoard : MonoBehaviour
{
     [SerializeField]
    private UiSpecialTitleCell prefab;

    [SerializeField]
    private Transform cellParent;


    [SerializeField]
    private TextMeshProUGUI abilityDescriptionText;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.Title_Special.dataArray;

        List<UiSpecialTitleCell> cells = new List<UiSpecialTitleCell>();
        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiSpecialTitleCell>(prefab, cellParent);
            cell.Initialize(tableDatas[i]);
            cells.Add(cell);
        }

        using var e = cells.GetEnumerator();

        while (e.MoveNext())
        {
            e.Current.gameObject.transform.SetSiblingIndex(e.Current.GetTransformIdx());
        }
    }

    private void Subscribe()
    {
        var tableData = TableManager.Instance.Title_Special.dataArray;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            ServerData.specialTitleServerTable.TableDatas[tableData[i].Stringid].hasItem.AsObservable()
                .DistinctUntilChanged().Subscribe(
                    e =>
                    {
                        UpdateUi();                
                    }).AddTo(this);
        }
    }

    private void UpdateUi()
    {
        var tableData = TableManager.Instance.Title_Special.dataArray;

        Dictionary<StatusType, float> dictionary = new  Dictionary<StatusType, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if(ServerData.specialTitleServerTable.TableDatas[tableData[i].Stringid].hasItem.Value<1) continue;
            
            Utils.AddOrUpdateValue(ref dictionary,(StatusType)tableData[i].Abiltype,tableData[i].Abilvalue);
        }

        if (dictionary.Count < 1)
        {
            abilityDescriptionText.SetText("획득한 능력치가 없습니다.");
        }
        else
        {
            var str = "";
            
            var e =  dictionary.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.Key.IsPercentStat())
                {
                    str += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value*100, 5)}\n";
                }
                else
                {
                    str += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value, 4)}\n";
                }
            }
            abilityDescriptionText.SetText(str);

        }
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }
}
