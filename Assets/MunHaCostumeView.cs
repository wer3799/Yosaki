using System;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Purchasing;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MunHaCostumeView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField] private GameObject rootObject;
    [FormerlySerializedAs("boneFollowerGraphic")] [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic_weapon;    
    
    [SerializeField]
    private Image magicBookImage;

    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic_magicBook;
    [SerializeField]
    private List<Image> weaponImage;

    [SerializeField]
    private List<Image> weaponImage_long;

    [SerializeField]
    private List<Image> weaponImage_Bu;
    private void Awake()
    {
        SetSpine();
    }

    private void SetSpine()
    {
        var data= TableManager.Instance.StudentTable.dataArray[0];

        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(data.Change_Costume);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
        
        boneFollowerGraphic_weapon.SetBone("bone14");
        boneFollowerGraphic_magicBook.SetBone("acc");

        var tower2Grade = Mathf.Max((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaTower).Value,1);

        var towerIdx = tower2Grade - 1;
        var towerData = TableManager.Instance.StudentTower.dataArray[towerIdx];

        var weaponGrade = towerData.Change_Weapon;
        var magicBookGrade = towerData.Change_Magicbook;
        for (int i = 0; i < weaponImage.Count; i++)
        {
            weaponImage[i].sprite = CommonResourceContainer.GetWeaponSprite(weaponGrade);
            weaponImage_long[i].sprite = CommonResourceContainer.GetWeaponSprite(weaponGrade);
            weaponImage_Bu[i].sprite = CommonResourceContainer.GetWeaponSprite(weaponGrade);

            weaponImage[i].gameObject.SetActive(weaponGrade < 21);
        
            //147->부적
            weaponImage_long[i].gameObject.SetActive((weaponGrade >= 21 && weaponGrade < 37) || (weaponGrade >= 42 && weaponGrade != 147));
            weaponImage_Bu[i].gameObject.SetActive((weaponGrade >= 37 && weaponGrade <= 41) || weaponGrade == 147);
        }
        
        magicBookImage.sprite = CommonResourceContainer.GetMagicBookSprite(magicBookGrade);

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
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(data.Change_Costume);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
        boneFollowerGraphic_weapon.SetBone("bone14");
        boneFollowerGraphic_magicBook.SetBone("acc");

        var tower2Grade = Mathf.Max((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaTower).Value,1);

        var towerIdx = tower2Grade - 1;
        var towerData = TableManager.Instance.StudentTower.dataArray[towerIdx];

        var weaponGrade = towerData.Change_Weapon;
        var magicBookGrade = towerData.Change_Magicbook;
        for (int i = 0; i < weaponImage.Count; i++)
        {
            weaponImage[i].sprite = CommonResourceContainer.GetWeaponSprite(weaponGrade);
            weaponImage_long[i].sprite = CommonResourceContainer.GetWeaponSprite(weaponGrade);
            weaponImage_Bu[i].sprite = CommonResourceContainer.GetWeaponSprite(weaponGrade);

            weaponImage[i].gameObject.SetActive(weaponGrade < 21);
        
            //147->부적
            weaponImage_long[i].gameObject.SetActive((weaponGrade >= 21 && weaponGrade < 37) || (weaponGrade >= 42 && weaponGrade != 147));
            weaponImage_Bu[i].gameObject.SetActive((weaponGrade >= 37 && weaponGrade <= 41) || weaponGrade == 147);
        }
        
        magicBookImage.sprite = CommonResourceContainer.GetMagicBookSprite(magicBookGrade);

        
        
    }
}
