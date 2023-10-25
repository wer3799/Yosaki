using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Coffee.UIEffects;
using Photon.Pun.UtilityScripts;
using UnityEngine.Serialization;

public class SealSwordEvolutionView : MonoBehaviour
{
    private SealSwordEvolutionBoard parentsBoard;
    
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image weaponIcon;


    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private TextMeshProUGUI gradeNumText;
    [SerializeField]
    private TextMeshProUGUI expText;

    private SealSwordData sealSwordData;

    private bool initialized = false;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private UIShiny uishiny;

    [FormerlySerializedAs("IsGachaCell")] [SerializeField]
    private bool IsMainCell = false;

    private ReactiveProperty<int> registerCount = new ReactiveProperty<int>(0);
    public SealSwordData GetSealSwordData()
    {
        return sealSwordData;
    }
    public void Initialize(SealSwordData sealSwordData = null,SealSwordEvolutionBoard parent=null)
    {
        this.sealSwordData = sealSwordData;

        this.parentsBoard = parent;
        
        int grade = 0;
        int id = 0;

        grade = sealSwordData.Grade;
        id = sealSwordData.Id;
        weaponIcon.sprite = CommonResourceContainer.GetSealSwordIconSprite(id);
        this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_SealSword[grade]);
        
        this.gradeText.color = (CommonUiContainer.Instance.itemGradeColor[grade]);


        
        int gradeText = 4 - (id % 4);
        
        gradeNumText.SetText($"{gradeText}등급");

        bg.sprite = CommonUiContainer.Instance.itemGradeFrame[grade];

        uishiny.brightness = ((float)grade / 3f) * 0.8f;
        
        registerCount.Value = 0;
        
        if (IsMainCell == false)
        {
            expText.SetText($"경험치 : {sealSwordData.Sealswordexp}");
            SubscribeSealSword();
        }

    }

    

    private void SubscribeSealSword()
    {
        disposable.Clear();

        ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        registerCount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);
    }


    private void WhenCountChanged(int amount)
    {
        UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        amountText.SetText($"{ServerData.sealSwordServerTable.GetCurrentWeaponCount(sealSwordData.Stringid)-registerCount.Value}");
    }


    private void OnDestroy()
    {
        disposable.Dispose();
    }

    public void OnClickRegister()
    {
        if (IsMainCell == true)
        {
            return;
        }

        if (parentsBoard.IsUpgradable() == true)
        {
            PopupManager.Instance.ShowAlarmMessage("요도를 더 이상 등록할 수 없습니다.");
            return;
        }
        //등록할 수 없는 상태
        if (ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].amount.Value - registerCount.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("요도를 더 이상 등록할 수 없습니다.");
            return;
        }
        parentsBoard.RegisterSealSword(sealSwordData);
    }

    public int GetRegisterCount()
    {
        return registerCount.Value;
    }
    
    public void AddRegister(int count)
    {
        registerCount.Value += count;
    }
}