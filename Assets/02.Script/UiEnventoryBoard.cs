using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun.Demo.Cockpit.Forms;
using UnityEngine;

public class UiEnventoryBoard : SingletonMono<UiEnventoryBoard>
{
    [SerializeField]
    private UiInventoryWeaponView uiInventoryWeaponViewPrefab;
    [SerializeField]
    private UiInventoryWeaponView uiInventoryRingViewPrefab;

    [SerializeField]
    private Transform viewParentWeapon;
    [SerializeField]
    private Transform viewParentMagicBook;
    [SerializeField]
    private Transform viewParentNewGacha;


    [SerializeField]
    private UiWeaponDetailView uiWeaponDetailView;

    private List<UiInventoryWeaponView> weaponViewContainer = new List<UiInventoryWeaponView>();
    private List<UiInventoryWeaponView> magicBookViewContainer = new List<UiInventoryWeaponView>();
    private List<UiInventoryWeaponView> newGachaViewContainer = new List<UiInventoryWeaponView>();

    [SerializeField]
    private Transform equipViewParent;

    [SerializeField]
    private Transform equipViewParent_Recommend;

    [SerializeField]
    private Transform equipViewParent_Sasinsu;

    public void AllUpgradeWeapon(int myIdx)
    {
        for (int i = 0; i <= myIdx; i++)
        {
            weaponViewContainer[i].OnClickUpgradeButton();
        }
    }

    public void AllUpgradeMagicBook(int myIdx)
    {
        for (int i = 0; i <= myIdx; i++)
        {
            magicBookViewContainer[i].OnClickUpgradeButton();
        }
    }
    public void AllUpgradeNewGacha(int myIdx)
    {
        for (int i = 0; i <= myIdx; i++)
        {
            newGachaViewContainer[i].OnClickUpgradeButton();
        }
    }

    public void Start()
    {
        //MakeWeaponBoard();
        //MakeMagicBookBoard();
        //MakeNewGachaBoard();
        MakePetBoard();
    }

    
    private void MakeWeaponBoard()
    {
        using var e = TableManager.Instance.WeaponData.GetEnumerator();

        while (e.MoveNext())
        {

            if (e.Current.Value.WEAPONTYPE == WeaponType.View)
            {
                //유료 외형무기 등급
                if (e.Current.Value.Grade == 18)
                {
                    UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, equipViewParent);

                    view.Initialize(e.Current.Value, null, null,null,OnClickWeaponView);

                    weaponViewContainer.Add(view);
                }
                //추천용 무기
                else if (e.Current.Value.Grade == 20)
                {
                    UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, equipViewParent_Recommend);

                    view.Initialize(e.Current.Value, null, null,null, OnClickWeaponView);

                    weaponViewContainer.Add(view);

                }
            }
            else if(e.Current.Value.WEAPONTYPE==WeaponType.HasEffectOnly)
            {
                //사신수 무기
                if (e.Current.Value.Grade == 23)
                {
                    UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, equipViewParent_Sasinsu);

                    view.Initialize(e.Current.Value, null, null, null,OnClickWeaponView);

                    weaponViewContainer.Add(view);

                }
                //사흉수 무기
                else if (e.Current.Value.Grade == 25)
                {
                    UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, equipViewParent_Sasinsu);

                    view.Initialize(e.Current.Value, null, null,null,OnClickWeaponView);

                    weaponViewContainer.Add(view);

                }
            }
            else if (e.Current.Value.WEAPONTYPE == WeaponType.Basic)
            {
                UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, viewParentWeapon);

                view.Initialize(e.Current.Value, null, null, null,OnClickWeaponView);

                weaponViewContainer.Add(view);
            }
            else if (e.Current.Value.WEAPONTYPE == WeaponType.Normal)
            {
         
                    UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, viewParentWeapon);

                    view.Initialize(e.Current.Value, null, null, null,OnClickWeaponView);

                    weaponViewContainer.Add(view);
            }
        }
        var tableData = TableManager.Instance.WeaponTable.dataArray;
        List<(int displayOrder, UiInventoryWeaponView gameObject)> weapons = new List<(int, UiInventoryWeaponView)>();
        foreach (var weapon in tableData)
        {
            if (weapon.WEAPONTYPE == WeaponType.Basic || weapon.WEAPONTYPE == WeaponType.Normal)
            {
                UiInventoryWeaponView gameObject = weaponViewContainer[weapon.Id];
                weapons.Add((weapon.Displayorder, gameObject));
            }
        }
        weapons.Sort((a, b) => a.displayOrder.CompareTo(b.displayOrder));

        for (int i = 0; i < weapons.Count; i++)
        {
            UiInventoryWeaponView weaponObject = weapons[i].gameObject;
            weaponObject.transform.SetSiblingIndex(i);

            WeaponData weaponData = tableData.FirstOrDefault(data => data.Id == weaponObject.GetWeaponData().Id);
            if (weaponData != null && ServerData.weaponTable.TableDatas[weaponData.Stringid].hasItem.Value > 0)
            {
                weaponObject.transform.SetAsFirstSibling();
            }
        }
    }

    private void MakeMagicBookBoard()
    {
        using var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.MAGICBOOKTYPE == MagicBookType.View)
            {
                UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, equipViewParent);

                view.Initialize(null, e.Current.Value,null, null,OnClickWeaponView);

                magicBookViewContainer.Add(view);
            }
            else
            {

                UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, viewParentMagicBook);

                view.Initialize(null, e.Current.Value, null,null, OnClickWeaponView);

                magicBookViewContainer.Add(view);
            }

        }
        
        var tableData = TableManager.Instance.MagicBookTable.dataArray;
        List<(int displayOrder, UiInventoryWeaponView gameObject)> magicBooks = new List<(int, UiInventoryWeaponView)>();
        foreach (var magicBook in tableData)
        {
            if (magicBook.MAGICBOOKTYPE == MagicBookType.Basic || magicBook.MAGICBOOKTYPE == MagicBookType.Normal)
            {
                UiInventoryWeaponView gameObject = magicBookViewContainer[magicBook.Id];
                magicBooks.Add((magicBook.Displayorder, gameObject));
            }
        }
        magicBooks.Sort((a, b) => a.displayOrder.CompareTo(b.displayOrder));

        for (int i = 0; i < magicBooks.Count; i++)
        {
            UiInventoryWeaponView magicBookObject = magicBooks[i].gameObject;
            magicBookObject.transform.SetSiblingIndex(i);

            MagicBookData magicBookData = tableData.FirstOrDefault(data => data.Id == magicBookObject.GetMagicBookData().Id);
            if (magicBookData != null && ServerData.magicBookTable.TableDatas[magicBookData.Stringid].hasItem.Value > 0)
            {
                magicBookObject.transform.SetAsFirstSibling();
            }
        }
    }
    private void MakeNewGachaBoard()
    {
        using var e = TableManager.Instance.NewGachaData.GetEnumerator();

        while (e.MoveNext())
        {

                UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryRingViewPrefab, viewParentNewGacha);

                view.Initialize(null, null, e.Current.Value, null,OnClickWeaponView);

                newGachaViewContainer.Add(view);
         

        }
    }

    private void OnClickWeaponView(WeaponData weaponData, MagicBookData magicBookData)
    {
        uiWeaponDetailView.gameObject.SetActive(true);
        uiWeaponDetailView.Initialize(weaponData, magicBookData);
    }

    [SerializeField]
    private UiPetView uiPetViewPrefeab;
    [SerializeField]
    private Transform petViewParent;

    private List<UiPetView> petViewContainer = new List<UiPetView>();

    private void MakePetBoard()
    {
        using var e = TableManager.Instance.PetDatas.GetEnumerator();

        while (e.MoveNext())
        {
            // //이무기는 생성X
            // if (e.Current.Value.Id >= 28) break;
            // if (e.Current.Value.Id >= 12 && e.Current.Value.Id <= 23) continue;
            if(e.Current.Value.PETTYPE!=PetType.Basic) continue;
            
            var petView = Instantiate<UiPetView>(uiPetViewPrefeab, petViewParent);

            petView.Initialize(e.Current.Value);

            petViewContainer.Add(petView);
        }
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }

#if UNITY_EDITOR
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U)) 
        //{
        //    var e = TableManager.Instance.WeaponData.GetEnumerator();
        //    while (e.MoveNext()) 
        //    {
        //        DatabaseManager.weaponTable.TableDatas[e.Current.Value.Stringid].hasItem.Value = 1;
        //        DatabaseManager.weaponTable.TableDatas[e.Current.Value.Stringid].amount.Value = 999;
        //        DatabaseManager.weaponTable.TableDatas[e.Current.Value.Stringid].level.Value = e.Current.Value.Maxlevel;
        //        DatabaseManager.weaponTable.SyncToServerAll();
        //    }

        //    var e2 = TableManager.Instance.MagicBoocDatas.GetEnumerator();
        //    while (e2.MoveNext())
        //    {
        //        DatabaseManager.magicBookTable.TableDatas[e2.Current.Value.Stringid].hasItem.Value = 1;
        //        DatabaseManager.magicBookTable.TableDatas[e2.Current.Value.Stringid].amount.Value = 999;
        //        DatabaseManager.magicBookTable.TableDatas[e2.Current.Value.Stringid].level.Value = e2.Current.Value.Maxlevel;
        //        DatabaseManager.magicBookTable.SyncToServerAll();
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.Space)) 
        //{

        //}
    }

#endif
}
