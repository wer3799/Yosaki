using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;
using LitJson;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiStatus : SingletonMono<UiStatus>
{
    [SerializeField]
    private List<TextMeshProUGUI> nameText =new List<TextMeshProUGUI>();

    [SerializeField]
    private Image costumeIcon;


    private int loadedMyRank = -1;

    private static bool LevelInit = false;
    [SerializeField] private List<GameObject> normalObjects =new List<GameObject>();
    [SerializeField] private List<GameObject> dimensionObjects =new List<GameObject>();

    void Start()
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            nameText[1].SetText($"Lv:{Utils.ConvertNum(ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level).Value)} {PlayerData.Instance.NickName}");

        }
        else
        {
            if (LevelInit == false) 
            {
                LevelInit = true;
                RankManager.Instance.RequestMyLevelRank();
            }

            Subscribe();
        }

    }

    private void OnEnable()
    {
        using var e1 = normalObjects.GetEnumerator();
        using var e2 = dimensionObjects.GetEnumerator();

        while (e1.MoveNext())
        {
            e1.Current.gameObject.SetActive(!GameManager.contentsType.IsDimensionContents());
        }
        while (e2.MoveNext())
        {
            e2.Current.gameObject.SetActive(GameManager.contentsType.IsDimensionContents());
        }      
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(WhenLevelChanged).AddTo(this);

        RankManager.Instance.WhenMyLevelRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                loadedMyRank = e.Rank;

                WhenLevelChanged(ServerData.statusTable.GetTableData(StatusTable.Level).Value);

            }
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            costumeIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail((int)e);
        }).AddTo(this);

        PlayerData.Instance.whenNickNameChanged.AsObservable().Subscribe(e =>
        {
            WhenLevelChanged(ServerData.statusTable.GetTableData(StatusTable.Level).Value);
        }).AddTo(this);

    }

    private void WhenLevelChanged(float level)
    {
        if (loadedMyRank == -1)
        {
            nameText[0].SetText($"Lv:{Utils.ConvertNum(level)} {PlayerData.Instance.NickName}");
        }
        else
        {
            nameText[0].SetText($"Lv:{Utils.ConvertNum(level)} {PlayerData.Instance.NickName} ({loadedMyRank}등)");
        }
    }
}
