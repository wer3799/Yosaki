using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiPlayerViewController : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer sonObject;


    [SerializeField]
    private GameObject sonObject_EFX;

    [SerializeField]
    private GameObject dogObject;

    [SerializeField]
    private GameObject marbleCircleObject;

    [SerializeField]
    private GameObject asuraObject;

    [SerializeField]
    private GameObject akGuiObject;

    [SerializeField]
    private GameObject tailObject;

    //
    [SerializeField]
    private GameObject hyonMu;
    [SerializeField]
    private GameObject baekHo;
    [SerializeField]
    private GameObject pet;
    [SerializeField]
    private GameObject orb;
    [SerializeField]
    private GameObject indra;
    [SerializeField]
    private GameObject dragon;

    [SerializeField]
    private GameObject fourObject_EFX;

    [SerializeField]
    private GameObject showFoxCup;
    [SerializeField]
    private GameObject showWolfRing;

    [SerializeField]
    private GameObject showDragonBracelet;

    [SerializeField]
    private GameObject showRingEffect; 
    
    [SerializeField]
    private GameObject suhoAnimal;
    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        SettingData.sonView.AsObservable().Subscribe(e =>
        {
            sonObject.enabled = e == 1;
            sonObject_EFX.SetActive(e == 1);
        }).AddTo(this);

        SettingData.dogView.AsObservable().Subscribe(e =>
        {
            dogObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.marbleCircleView.AsObservable().Subscribe(e =>
        {
            marbleCircleObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.asuarView.AsObservable().Subscribe(e =>
        {
            asuraObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.akGuiView.AsObservable().Subscribe(e =>
        {
            akGuiObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.tailView.AsObservable().Subscribe(e =>
        {
            tailObject.SetActive(e == 1);

        }).AddTo(this);
        //
        SettingData.hyonMu.AsObservable().Subscribe(e =>
        {
            hyonMu.SetActive(e == 1);

        }).AddTo(this);

        SettingData.baekHo.AsObservable().Subscribe(e =>
        {
            baekHo.SetActive(e == 1);

        }).AddTo(this);

        SettingData.pet.AsObservable().Subscribe(e =>
        {
            pet.SetActive(e == 1);

        }).AddTo(this);

        SettingData.orb.AsObservable().Subscribe(e =>
        {
            orb.SetActive(e == 1);

        }).AddTo(this);

        SettingData.indra.AsObservable().Subscribe(e =>
        {
            indra.SetActive(e == 1);

        }).AddTo(this);

        SettingData.dragon.AsObservable().Subscribe(e =>
        {
            dragon.SetActive(e == 1);

        }).AddTo(this);
        SettingData.fourView.AsObservable().Subscribe(e =>
        {
            fourObject_EFX.SetActive(e == 1);
        }).AddTo(this);

        SettingData.showFoxCup.AsObservable().Subscribe(e =>
        {
            showFoxCup.gameObject.SetActive(e == 1);
        }).AddTo(this);
        SettingData.showWolfRing.AsObservable().Subscribe(e =>
        {
            showWolfRing.gameObject.SetActive(e == 1);
        }).AddTo(this);
        SettingData.showDragonBracelet.AsObservable().Subscribe(e =>
        {
            showDragonBracelet.gameObject.SetActive(e == 1);
        }).AddTo(this);

        SettingData.showRingEffect.AsObservable().Subscribe(e =>
        {
            showRingEffect.gameObject.SetActive(e == 1);
        }).AddTo(this);   
        
        SettingData.norigaeSize.AsObservable().Subscribe(e =>
        {
            suhoAnimal.gameObject.SetActive(e == 1);
        }).AddTo(this);
    }

}
