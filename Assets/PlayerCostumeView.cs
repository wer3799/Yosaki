using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerCostumeView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic; 
    
    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic_SealWeapon;

    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic_Mask;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(WhenCostumeChanged).AddTo(this);
    }

    private void WhenCostumeChanged(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        boneFollowerGraphic.SetBone("Weapon1");
        boneFollowerGraphic_SealWeapon.SetBone("bone21");
        boneFollowerGraphic_Mask.SetBone("bone21");
    }
}
