using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLeMuGiBoard : SingletonMono<UiLeMuGiBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_LeeMuGi;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_GoldDragon;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Haetae;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Sam;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Kirin;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Rabit;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Dog; 
    
    [SerializeField]
    private UiPetView uiPetViewPrefeab_Horse;   
    
    [SerializeField]
    private UiPetView uiPetViewPrefeab_ChunDog; 
    
    [SerializeField]
    private UiPetView uiPetViewPrefeab_ChunCat;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Chungdung;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Cloud;
    [SerializeField]
    private List<UiPetView> uiPetViewPrefeab_Sahyung;
    [SerializeField]
    private List<UiPetView> uiPetViewPrefeab_Vision;
    [SerializeField]
    private List<UiPetView> uiPetViewPrefeab_Fox;
    [SerializeField]
    private List<UiPetView> uiPetViewPrefeab_Tiger;
    [SerializeField]
    private List<UiPetView> uiPetViewPrefeab_ChunGu;
    [SerializeField]
    private List<UiPetView> uiPetViewPrefab_Event;

    [SerializeField]
    private Transform petViewParent;
    [SerializeField]
    private Transform petViewEventParent;
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        using var e = TableManager.Instance.PetDatas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.PETTYPE != PetType.Special) continue;

            switch (e.Current.Value.Id)
            {
                case 12 :
                    var petView12 = Instantiate<UiPetView>(uiPetViewPrefeab_LeeMuGi, petViewParent);

                    petView12.gameObject.SetActive(true);

                    petView12.transform.localPosition = Vector3.zero;

                    petView12.Initialize(e.Current.Value);
                    break;
                case 13:
                    var petView13 = Instantiate<UiPetView>(uiPetViewPrefeab_GoldDragon, petViewParent);

                    petView13.gameObject.SetActive(true);

                    petView13.transform.localPosition = Vector3.zero;

                    petView13.Initialize(e.Current.Value);
                    break;
                case 14:
                    var petView14 = Instantiate<UiPetView>(uiPetViewPrefeab_Haetae, petViewParent);

                    petView14.gameObject.SetActive(true);

                    petView14.transform.localPosition = Vector3.zero;

                    petView14.Initialize(e.Current.Value);
                    break;
                case 15:
                    var petView15 = Instantiate<UiPetView>(uiPetViewPrefeab_Sam, petViewParent);

                    petView15.gameObject.SetActive(true);

                    petView15.transform.localPosition = Vector3.zero;

                    petView15.Initialize(e.Current.Value);
                    break;
                case 16:
                    var petView16 = Instantiate<UiPetView>(uiPetViewPrefeab_Kirin, petViewParent);

                    petView16.gameObject.SetActive(true);

                    petView16.transform.localPosition = Vector3.zero;

                    petView16.Initialize(e.Current.Value);
                    break;
                case 17:
                    var petView17 = Instantiate<UiPetView>(uiPetViewPrefeab_Rabit, petViewParent);

                    petView17.gameObject.SetActive(true);

                    petView17.transform.localPosition = Vector3.zero;

                    petView17.Initialize(e.Current.Value);
                    break;
                case 18 : 
                    var petView18 = Instantiate<UiPetView>(uiPetViewPrefeab_Dog, petViewParent);

                    petView18.gameObject.SetActive(true);

                    petView18.transform.localPosition = Vector3.zero;

                    petView18.Initialize(e.Current.Value);
                    break;
                case 19:
                    var petView19 = Instantiate<UiPetView>(uiPetViewPrefeab_Horse, petViewParent);

                    petView19.gameObject.SetActive(true);

                    petView19.transform.localPosition = Vector3.zero;

                    petView19.Initialize(e.Current.Value);
                    break;
                case 20 :
                    
                    var petView20 = Instantiate<UiPetView>(uiPetViewPrefeab_ChunDog, petViewParent);

                    petView20.gameObject.SetActive(true);

                    petView20.transform.localPosition = Vector3.zero;

                    petView20.Initialize(e.Current.Value);
                    break;
                case 21 :
                    var petView21 = Instantiate<UiPetView>(uiPetViewPrefeab_ChunCat, petViewParent);

                    petView21.gameObject.SetActive(true);

                    petView21.transform.localPosition = Vector3.zero;

                    petView21.Initialize(e.Current.Value);
                    break;
                case 22:
                    var petView22 = Instantiate<UiPetView>(uiPetViewPrefeab_Chungdung, petViewParent);

                    petView22.gameObject.SetActive(true);

                    petView22.transform.localPosition = Vector3.zero;

                    petView22.Initialize(e.Current.Value);
                    break;
                case 23:
                    var petView23 = Instantiate<UiPetView>(uiPetViewPrefeab_Cloud, petViewParent);

                    petView23.gameObject.SetActive(true);

                    petView23.transform.localPosition = Vector3.zero;

                    petView23.Initialize(e.Current.Value);
                    break;
                case 28:
                    var petView28 = Instantiate<UiPetView>(uiPetViewPrefeab_Sahyung[0], petViewParent);

                    petView28.gameObject.SetActive(true);

                    petView28.transform.localPosition = Vector3.zero;

                    petView28.Initialize(e.Current.Value);
                    break;
                case 29:
                    var petView29 = Instantiate<UiPetView>(uiPetViewPrefeab_Sahyung[1], petViewParent);

                    petView29.gameObject.SetActive(true);

                    petView29.transform.localPosition = Vector3.zero;

                    petView29.Initialize(e.Current.Value);
                    break;
                case 30:
                    var petView30 = Instantiate<UiPetView>(uiPetViewPrefeab_Sahyung[2], petViewParent);

                    petView30.gameObject.SetActive(true);

                    petView30.transform.localPosition = Vector3.zero;

                    petView30.Initialize(e.Current.Value);
                    break;
                case 31:
                    var petView31 = Instantiate<UiPetView>(uiPetViewPrefeab_Sahyung[3], petViewParent);

                    petView31.gameObject.SetActive(true);

                    petView31.transform.localPosition = Vector3.zero;

                    petView31.Initialize(e.Current.Value);
                    break;
                case 32:
                    var petView32 = Instantiate<UiPetView>(uiPetViewPrefeab_Vision[0], petViewParent);

                    petView32.gameObject.SetActive(true);

                    petView32.transform.localPosition = Vector3.zero;

                    petView32.Initialize(e.Current.Value);
                    break;
                case 33:
                    var petView33 = Instantiate<UiPetView>(uiPetViewPrefeab_Vision[1], petViewParent);

                    petView33.gameObject.SetActive(true);

                    petView33.transform.localPosition = Vector3.zero;

                    petView33.Initialize(e.Current.Value);
                    break;
                case 34:
                    var petView34 = Instantiate<UiPetView>(uiPetViewPrefeab_Vision[2], petViewParent);

                    petView34.gameObject.SetActive(true);

                    petView34.transform.localPosition = Vector3.zero;

                    petView34.Initialize(e.Current.Value);
                    break;
                case 35:
                    var petView35 = Instantiate<UiPetView>(uiPetViewPrefeab_Vision[3], petViewParent);

                    petView35.gameObject.SetActive(true);

                    petView35.transform.localPosition = Vector3.zero;

                    petView35.Initialize(e.Current.Value);
                    break;
                case 36:
                    var petView36 = Instantiate<UiPetView>(uiPetViewPrefeab_Fox[0], petViewParent);

                    petView36.gameObject.SetActive(true);

                    petView36.transform.localPosition = Vector3.zero;

                    petView36.Initialize(e.Current.Value);
                    break;
                case 37:
                    var petView37 = Instantiate<UiPetView>(uiPetViewPrefeab_Fox[1], petViewParent);

                    petView37.gameObject.SetActive(true);

                    petView37.transform.localPosition = Vector3.zero;

                    petView37.Initialize(e.Current.Value);
                    break;
                case 38:
                    var petView38 = Instantiate<UiPetView>(uiPetViewPrefeab_Fox[2], petViewParent);

                    petView38.gameObject.SetActive(true);

                    petView38.transform.localPosition = Vector3.zero;

                    petView38.Initialize(e.Current.Value);
                    break;
                case 39:
                    var petView39 = Instantiate<UiPetView>(uiPetViewPrefeab_Fox[3], petViewParent);

                    petView39.gameObject.SetActive(true);

                    petView39.transform.localPosition = Vector3.zero;

                    petView39.Initialize(e.Current.Value);
                    break;
                case 40:
                    var petView40 = Instantiate<UiPetView>(uiPetViewPrefeab_Tiger[0], petViewParent);

                    petView40.gameObject.SetActive(true);

                    petView40.transform.localPosition = Vector3.zero;

                    petView40.Initialize(e.Current.Value);
                    break;
                case 41:
                    var petView41 = Instantiate<UiPetView>(uiPetViewPrefeab_Tiger[1], petViewParent);

                    petView41.gameObject.SetActive(true);

                    petView41.transform.localPosition = Vector3.zero;

                    petView41.Initialize(e.Current.Value);
                    break;
                case 42:
                    var petView42 = Instantiate<UiPetView>(uiPetViewPrefeab_Tiger[2], petViewParent);

                    petView42.gameObject.SetActive(true);

                    petView42.transform.localPosition = Vector3.zero;

                    petView42.Initialize(e.Current.Value);
                    break;
                case 43:
                    var petView43 = Instantiate<UiPetView>(uiPetViewPrefeab_Tiger[3], petViewParent);

                    petView43.gameObject.SetActive(true);

                    petView43.transform.localPosition = Vector3.zero;

                    petView43.Initialize(e.Current.Value);
                    break;
                case 44:
                    var petView44 = Instantiate<UiPetView>(uiPetViewPrefeab_ChunGu[0], petViewParent);

                    petView44.gameObject.SetActive(true);

                    petView44.transform.localPosition = Vector3.zero;

                    petView44.Initialize(e.Current.Value);
                    break;
                case 45:
                    var petView45 = Instantiate<UiPetView>(uiPetViewPrefeab_ChunGu[1], petViewParent);

                    petView45.gameObject.SetActive(true);

                    petView45.transform.localPosition = Vector3.zero;

                    petView45.Initialize(e.Current.Value);
                    break;
                case 46:
                    var petView46 = Instantiate<UiPetView>(uiPetViewPrefeab_ChunGu[2], petViewParent);

                    petView46.gameObject.SetActive(true);

                    petView46.transform.localPosition = Vector3.zero;

                    petView46.Initialize(e.Current.Value);
                    break;
                case 47:
                    var petView47 = Instantiate<UiPetView>(uiPetViewPrefeab_ChunGu[3], petViewParent);

                    petView47.gameObject.SetActive(true);

                    petView47.transform.localPosition = Vector3.zero;

                    petView47.Initialize(e.Current.Value);
                    break;
                case 52:
                    var petView52 = Instantiate<UiPetView>(uiPetViewPrefab_Event[0], petViewEventParent);

                    petView52.gameObject.SetActive(true);

                    petView52.transform.localPosition = Vector3.zero;

                    petView52.Initialize(e.Current.Value);
                    break;
                case 53:
                    var petView53 = Instantiate<UiPetView>(uiPetViewPrefab_Event[1], petViewEventParent);

                    petView53.gameObject.SetActive(true);

                    petView53.transform.localPosition = Vector3.zero;

                    petView53.Initialize(e.Current.Value);
                    break;
                case 54:
                    var petView54 = Instantiate<UiPetView>(uiPetViewPrefab_Event[2], petViewEventParent);

                    petView54.gameObject.SetActive(true);

                    petView54.transform.localPosition = Vector3.zero;

                    petView54.Initialize(e.Current.Value);
                    break;
            }
        }
    }
}
