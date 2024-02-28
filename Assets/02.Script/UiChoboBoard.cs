using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public enum HelpType{
    Goods,
    Contents,
    System,
}
public class UiChoboBoard : MonoBehaviour
{
    [SerializeField]
    private ChoboLeftCell choboLeftCellPrefab;

    [SerializeField]
    private Transform goodsCellParent;
    [SerializeField]
    private Transform contentsCellParent;
    [SerializeField]
    private Transform systemCellParent;

    private List<ChoboLeftCell> choboLeftCellList = new List<ChoboLeftCell>();

    private ReactiveProperty<int> idx = new ReactiveProperty<int>();

    private ReactiveProperty<int> descIdx = new ReactiveProperty<int>();

    private int descIdxMax = 0;
    
    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private TextMeshProUGUI DescriptionText;

    
    void Start()
    {
        Initialize();
        Subscribe();
        
    }

    void Subscribe()
    {
        idx.AsObservable().Subscribe(e =>
        {
            DescriptionText.SetText(TableManager.Instance.choboTable.dataArray[e].Description0);
        }).AddTo(this);
        
        descIdx.AsObservable().Subscribe(e =>
        {
            ButtonInitialize();

            if (e == 0)
            {
                DescriptionText.SetText(TableManager.Instance.choboTable.dataArray[idx.Value].Description0);
                
            }
            else if (e==1)
            {
                DescriptionText.SetText(TableManager.Instance.choboTable.dataArray[idx.Value].Description1);
            }
            else
            {
                DescriptionText.SetText(TableManager.Instance.choboTable.dataArray[idx.Value].Description2);
            }
        }).AddTo(this);


    }

    public void Initialize()
    {

        var stageDatas = TableManager.Instance.choboTable.dataArray;

        for (int i = 0; i < stageDatas.Length; i++)
        {
            if (stageDatas[i].HELPTYPE == HelpType.Goods)
            {
                choboLeftCellList.Add(Instantiate<ChoboLeftCell>(choboLeftCellPrefab, goodsCellParent));
            }
            else if(stageDatas[i].HELPTYPE == HelpType.Contents)
            {
                choboLeftCellList.Add(Instantiate<ChoboLeftCell>(choboLeftCellPrefab, contentsCellParent));
            }
            else if(stageDatas[i].HELPTYPE == HelpType.System)
            {
                choboLeftCellList.Add(Instantiate<ChoboLeftCell>(choboLeftCellPrefab, systemCellParent));
            }
        }
            for (int i = 0; i < choboLeftCellList.Count; i++)
        {
            choboLeftCellList[i].Initialize(stageDatas[i], this);
        }

        SetIndex();
    }
    public void OnClickLeftButton()
    {
        if (descIdx.Value == 0)
        {
            return;
        }
        else
        {
           descIdx.Value--;
        }
    }
    public void OnClickRightButton()
    {
        if (descIdx.Value == 2)
        {
            return;
        }
        else
        {
            descIdx.Value++;
        }
    }
    private void ButtonInitialize()
    {
        leftButton.interactable = descIdx.Value != 0;
        rightButton.interactable = descIdx.Value < descIdxMax;
    }
    public void SetIndex(int  _idx=0)
    {
        idx.Value = _idx;
        descIdx.Value = 0;
        descIdxMax = IdxMax();
        ButtonInitialize();
    }

    private int IdxMax()
    {
        
        if (string.IsNullOrEmpty(TableManager.Instance.choboTable.dataArray[idx.Value].Description1))
        {
            // 없음
            return 0;
        }
        else if (string.IsNullOrEmpty(TableManager.Instance.choboTable.dataArray[idx.Value].Description2))
        {
            
            return 1;
        }
        else 
        {
            return 2;
        }
    }

    public void OnClickUserGuideButton() 
    {
        Application.OpenURL("https://cafe.naver.com/yokiki/7151");
    }
    
    public void OnClickDamageGuideButton() 
    {
        Application.OpenURL("https://cafe.naver.com/yokiki/23117");
    }

    private void OnEnable()
    {
        var mission = PlayerPrefs.GetInt(CommonString.YorinMission_Help,0);
        if (mission <1)
        {
            PlayerPrefs.SetInt(CommonString.YorinMission_Help,1);
        }
    }
}
