using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class UiCostumeLookIndicator : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private UiDescriptionBoard uidescriptionBoard;

    [SerializeField]
    private TextMeshProUGUI costumeName;

    [SerializeField]
    private TextMeshProUGUI costumeDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            CostumeData costumeData = TableManager.Instance.CostumeData[e];
            uidescriptionBoard.SetDescription(costumeData.Description);
            SetCostumeSpine(e);
            costumeName.SetText(costumeData.Name);
            //costumeDescription.SetText(costumeData.Description);
            costumeDescription.SetText(string.Empty);
        }).AddTo(this);
    }

    private void SetCostumeSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }


}
