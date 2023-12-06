using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;

public class WeaponEquipView : MonoBehaviour
{
    [SerializeField]
    private Image weaponImage;

    [SerializeField]
    private Image weaponImage_long;

    [SerializeField]
    private Image weaponImage_Bu;

    [SerializeField]
    private GameObject newEffect;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].AsObservable().Subscribe(WhenEquipIdxChanged).AddTo(this);
    }

    private void WhenEquipIdxChanged(int idx)
    {
        weaponImage.sprite = CommonResourceContainer.GetWeaponSprite(idx);
        weaponImage_long.sprite = CommonResourceContainer.GetWeaponSprite(idx);
        weaponImage_Bu.sprite = CommonResourceContainer.GetWeaponSprite(idx);

        weaponImage.gameObject.SetActive(idx < 21);
        
        //147->부적
        weaponImage_long.gameObject.SetActive((idx >= 21 && idx < 37) || (idx >= 42 && idx != 147));
        weaponImage_Bu.gameObject.SetActive((idx >= 37 && idx <= 41) || idx == 147);
        //weaponImage_long.gameObject.SetActive(idx >= 42);

        newEffect.gameObject.SetActive(idx == 22);
        //var weaponGrade = TableManager.Instance.WeaponData[idx].Grade;

        //var emission = equipEffect.emission;
        //emission.rateOverTime = weaponGrade * 5;
    }

    private void OnDisable()
    {
    }
}