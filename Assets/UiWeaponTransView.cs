using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiWeaponTransView : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI weaponName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private WeaponData weaponData;

    private WeaponServerData weaponServerData;

    [SerializeField]
    private GameObject notHasObject;

    [SerializeField]
    private Image weaponIcon;


    public WeaponData GetWeaponData()
    {
        return weaponData;
    }
    public void Initialize(WeaponData weaponData)
    {

        this.weaponData = weaponData;

        this.weaponServerData = ServerData.weaponTable.TableDatas[weaponData.Stringid];

        weaponName.SetText($"{weaponData.Name}");
        
        weaponIcon.sprite = CommonResourceContainer.GetWeaponSprite(weaponData.Id);        


        Subscribe();
    }

    private void Subscribe()
    {
        weaponServerData.trans.AsObservable().Subscribe(e =>
        {
            notHasObject.SetActive(e == 0);

            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);

    }

}