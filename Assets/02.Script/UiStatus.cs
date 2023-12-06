using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class UiStatus : SingletonMono<UiStatus>
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image costumeIcon;


    private int loadedMyRank = -1;

    private static bool LevelInit = false;

    void Start()
    {
        if (LevelInit == false) 
        {
            LevelInit = true;
            RankManager.Instance.RequestMyLevelRank();
        }

        Subscribe();
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
            nameText.SetText($"Lv:{Utils.ConvertNum(level)} {PlayerData.Instance.NickName}");
        }
        else
        {
            nameText.SetText($"Lv:{Utils.ConvertNum(level)} {PlayerData.Instance.NickName} ({loadedMyRank}등)");
        }
    }
}
