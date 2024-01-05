using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using static UiRewardView;
public class DamageCalculateManager : SingletonMono<DamageCalculateManager>
{
    //[SerializeField]
    //private TextMeshProUGUI calculatorText;

    
    public ReactiveProperty<double> normalDamage = new ReactiveProperty<double>();
    public ReactiveProperty<double> dosulDamage = new ReactiveProperty<double>();
    public ReactiveProperty<double> sealSwordDamage = new ReactiveProperty<double>();
    public ReactiveProperty<double> VisionDamage = new ReactiveProperty<double>();
    public ReactiveProperty<double> MunhaDamage = new ReactiveProperty<double>();

    public bool isCalculate = false;
    private void Start()
    {
        #if UNITY_EDITOR
                InitializeValue();
        #else
                this.gameObject.SetActive(false);
        #endif
    }

    private void InitializeValue()
    {
        normalDamage.Value = 0;
        dosulDamage.Value = 0;
        sealSwordDamage.Value = 0;
        VisionDamage.Value = 0;
        MunhaDamage.Value = 0;
    }

    public void StartCalculate()
    {
        InitializeValue();
        isCalculate = true;
    }
    public void StopCalculate()
    {
        InitializeValue();
        isCalculate = false;
    }

    
    public void AddDamage(SkillCastType type,double data)
    {
        if (isCalculate)
        {
            switch (type)
            {
                case SkillCastType.Player:
                    normalDamage.Value += data;
                    break;
                case SkillCastType.Dosul:
                    dosulDamage.Value += data;
                    break;
                case SkillCastType.SealSword:
                    sealSwordDamage.Value += data;
                    break;
                case SkillCastType.Vision:
                    VisionDamage.Value += data;
                    break;
                case SkillCastType.Student:
                    MunhaDamage.Value += data;
                    break;
                default:
                    normalDamage.Value += data;
                    break;
            }
        }
    }
}
