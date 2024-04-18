using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class HaetalSkeleton : MonoBehaviour
{
    
    [SerializeField]
    private SkeletonAnimation skeletonAnimation;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            skeletonAnimation.skeletonDataAsset = CommonUiContainer.Instance.costumeList[e];
            skeletonAnimation.Initialize(true);
        }).AddTo(this);
    }
}
