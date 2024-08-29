using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Spine.Unity;

public class UiCostumeCell : MonoBehaviour
{
    private CostumeData costumeData;

    private bool subscribed = false;

    //실제로 장착중
    [SerializeField]
    private GameObject selectedObject;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI costumeName;

    [SerializeField]
    private GameObject hasObject;

    public void Initialize(CostumeData costumeData)
    {
        this.costumeData = costumeData;

        SetCostumeSpine(costumeData.Id);

        costumeName.SetText(costumeData.Name);

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }

    private void SetCostumeSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(idx =>
        {
            selectedObject.SetActive(idx == costumeData.Id);
        }).AddTo(this);

        ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.AsObservable().Subscribe(e =>
        {
            hasObject.SetActive(e);
        }).AddTo(this);
    }

    public void OnClickCostume()
    {
        if (ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.Value == true)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value = costumeData.Id;

            //서버 저장
            ServerData.equipmentTable.SyncData(EquipmentTable.CostumeLook);
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"{costumeData.Description}");
        }

        //  InitBoard();
    }
    private void InitBoard()
    {
        // UiCostume.Instance.WhenSelectIdxChanged(costumeData.Id);
        UiCostumeAbilityBoard.Instance.Initialize(costumeData);
    }
}
