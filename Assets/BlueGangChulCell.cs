using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BlueGangChulCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI lockMaskText;
    [SerializeField]
    private GameObject lockMask;
    [SerializeField] 
    private Image buttonImage;

    private BlueGangCheolData _tableData;

    [SerializeField] private Sprite Image_Lock;
    [SerializeField] private Sprite Image_CanReward;
    
    private void Subscribe()
    {
        if (_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Level)
        {
            ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
            {
                buttonImage.sprite = e >= _tableData.Unlockamount ? Image_CanReward : Image_Lock;
            }).AddTo(this);    
        }
        else if(_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Stage)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).AsObservable().Subscribe(e =>
            {
                buttonImage.sprite = e >= _tableData.Unlockamount ? Image_CanReward : Image_Lock;
            }).AddTo(this);
        }
        else if(_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Dosul)
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulLevel).AsObservable().Subscribe(e =>
            {
                buttonImage.sprite = e >= _tableData.Unlockamount ? Image_CanReward : Image_Lock;
            }).AddTo(this);
        }

        ServerData.etcServerTable.TableDatas[EtcServerTable.blueGangChulUnlock].AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(ServerData.etcServerTable.IsBlueGangChulUnlocked(_tableData.Id)==false);
        }).AddTo(this);
    }

    public void Initialize(BlueGangCheolData tableData)
    {
        _tableData = tableData;
        
        titleText.SetText($"{_tableData.Description}");
        if (_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Level)
        {
            lockMaskText.SetText($"{Utils.ConvertNum(_tableData.Unlockamount)} 레벨 필요");
        }
        else if(_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Stage)
        {
            lockMaskText.SetText($"{Utils.ConvertStage(_tableData.Unlockamount)} 스테이지 필요");
        }
        else if(_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Dosul)
        {
            lockMaskText.SetText($"{_tableData.Unlockamount} 도술 레벨 필요");
        }

        Subscribe();
    }

    public void OnClickUnlockButton()
    {

        var currentAmount=0f;
        
        
        if(_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Level)
        {
            currentAmount = ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        }
        else if(_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Stage)
        {
            currentAmount = (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;
        }
        else if(_tableData.BLUEGANGCHULUNLOCKTYPE == BlueGangChulUnlockType.Dosul)
        {
            currentAmount = (float)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulLevel).Value;
        }

        var require = _tableData.Unlockamount;

        if (require > currentAmount)
        {
            PopupManager.Instance.ShowAlarmMessage($"요구 조건 미달성!");
            return;
        }

        //업그레이드 가능
        ServerData.etcServerTable.TableDatas[EtcServerTable.blueGangChulUnlock].Value += $"{BossServerTable.rewardSplit}{_tableData.Id}";

        ServerData.etcServerTable.UpdateData(EtcServerTable.blueGangChulUnlock);

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{_tableData.Description} 컨텐츠 해금!",null);


    }
    

}
