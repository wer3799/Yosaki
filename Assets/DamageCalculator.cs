using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using static UiRewardView;
public class DamageCalculator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI calculatorText;
    private bool isCalculate = false;
    private void Start()
    {
        #if UNITY_EDITOR
                Subscribe();
                DamageCalculateManager.Instance.StartCalculate();
        #else
                this.gameObject.SetActive(false);
        #endif
    }

    private void Subscribe()
    {
        DamageCalculateManager.Instance.normalDamage.AsObservable().Subscribe(e =>
        {
            UpdateText();
        }).AddTo(this);
    }

    public void OnClickStartButton()
    {
        DamageCalculateManager.Instance.StartCalculate();
    }

    private void UpdateText()
    {
        var normal = DamageCalculateManager.Instance.normalDamage.Value;   
        var dosul = DamageCalculateManager.Instance.dosulDamage.Value;   
        var sealSword = DamageCalculateManager.Instance.sealSwordDamage.Value;   
        var vision = DamageCalculateManager.Instance.VisionDamage.Value;   
        var munha = DamageCalculateManager.Instance.MunhaDamage.Value;   
        var sum = normal+dosul+sealSword+vision+munha;   
        calculatorText.SetText($"딜 미터기\n" +
                               $"기본 : {Utils.ConvertBigNum(normal)}({Utils.ConvertNum((normal/sum*100),2)}%)\n" +
                               $"도술 : {Utils.ConvertBigNum(dosul)}({Utils.ConvertNum((dosul/sum*100),2)}%)\n" +
                               $"요도 : {Utils.ConvertBigNum(sealSword)}({Utils.ConvertNum((sealSword/sum*100),2)}%)\n" +
                               $"필살 : {Utils.ConvertBigNum(vision)}({Utils.ConvertNum((vision/sum*100),2)}%)\n" +
                               $"문하 : {Utils.ConvertBigNum(munha)}({Utils.ConvertNum((munha/sum*100),2)}%)\n" +
                               $"합계 : {Utils.ConvertBigNum(sum)}");
    }
    
}
