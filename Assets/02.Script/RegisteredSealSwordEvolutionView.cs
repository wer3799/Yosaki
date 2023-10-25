using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Coffee.UIEffects;
using Photon.Pun.UtilityScripts;
using UnityEngine.Serialization;

public class RegisteredSealSwordEvolutionView : MonoBehaviour
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

    private SealSwordData sealSwordData;

    private bool initialized = false;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private UIShiny uishiny;


    private ReactiveProperty<int> registerCount = new ReactiveProperty<int>(0);

    public SealSwordData GetSealSwordData()
    {
        return sealSwordData;
    }
    
    public void Initialize(SealSwordData sealSwordData = null,SealSwordEvolutionBoard parent=null)
    {
        if (sealSwordData == null)
        {
            return;
        }
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
        
        SubscribeSealSword();
    }

    

    private void SubscribeSealSword()
    {
        if (sealSwordData == null)
        {
            return;
        }
        disposable.Clear();

        registerCount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);
    }


    private void WhenCountChanged(int amount)
    {
        UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        amountText.SetText($"{registerCount.Value}");
    }


    private void OnDestroy()
    {
        disposable.Dispose();
    }

    public void OnClickUnRegister()
    {
        if (sealSwordData == null)
        {
            return;
        }
        
        if (registerCount.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("더 이상 해제할 수 없습니다.");
            return;
        }

        parentsBoard.UnRegisterSealSword(sealSwordData);
        
    }

    public int GetRegisterCount()
    {
        if (sealSwordData == null)
        {
            return 0;
        }
        return registerCount.Value;
    }

    public void AddRegister(int count)
    {
        if (sealSwordData == null)
        {
            return;
        }
        registerCount.Value += count;
    }
    public void SetRegister(int count)
    {
        if (sealSwordData == null)
        {
            return;
        }
        registerCount.Value = count;
    }
}