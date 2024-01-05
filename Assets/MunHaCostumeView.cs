using System;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Purchasing;

public class MunHaCostumeView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField] private GameObject rootObject;

    private void Awake()
    {
        SetSpine();
    }

    private void SetSpine()
    {
        var data= TableManager.Instance.StudentTable.dataArray[0];

        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[data.Change_Costume];
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).AsObservable().Subscribe(e =>
        {
            CostumeChange((int)e);
        }).AddTo(this);
    }

    private void OnEnable()
    {
        CostumeChange((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value);
    }

    private void CostumeChange(int grade)
    {
        if (grade < 0)
        {
            rootObject.SetActive(false);
            return;
        }
        rootObject.SetActive(true);

        var data= TableManager.Instance.StudentTable.dataArray[grade];

        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[data.Change_Costume];
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }
}
