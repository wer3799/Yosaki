using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiCommonPassCell : MonoBehaviour
{    
    [SerializeField]
    protected Image itemIcon_free;

    [SerializeField]
    protected TextMeshProUGUI itemName_free;

    [SerializeField]
    protected Image itemIcon_ad;

    [SerializeField]
    protected TextMeshProUGUI itemName_ad;

    [SerializeField]
    protected TextMeshProUGUI itemAmount_free;

    [SerializeField]
    protected TextMeshProUGUI itemAmount_ad;

    [SerializeField]
    protected GameObject lockIcon_Free;

    [SerializeField]
    protected GameObject lockIcon_Ad;

    protected PassInfo passInfo;

    [SerializeField]
    protected GameObject rewardedObject_Free;

    [SerializeField]
    protected GameObject rewardedObject_Ad;

    [SerializeField]
    protected GameObject gaugeImage;

    [SerializeField]
    protected TextMeshProUGUI descriptionText;

    protected CompositeDisposable disposables = new CompositeDisposable();
   
}
