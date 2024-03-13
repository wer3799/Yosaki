using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSubHpBar : SingletonMono<UiSubHpBar>
{
    [SerializeField]
    private Image greenRenderer;
    [SerializeField]
    private Image greyRenderer;

    private float fixedLerpSpeed = 5f;

    private TextMeshProUGUI remainDescription;

    [Header("Ui")]
    [SerializeField]
    private TextMeshProUGUI damageIndicator;
    [SerializeField]
    private Animator damagedAnim;
    private string DamageAnimName = "Play";

    private void OnEnable()
    {
        ResetGauge();

        StartCoroutine(GreyRoutine());
    }

    private void Awake()
    {
        
        base.Awake();
        
        GetTextMeshProObject();
        
    }

    private void GetTextMeshProObject()
    {
        var findObject = this.transform.Find("Top");
        
        if (findObject != null)
        {
            remainDescription = findObject.GetComponent<TextMeshProUGUI>();
            
            if (remainDescription!=null)
            {
                remainDescription.fontSizeMax = 42;
                remainDescription.fontSizeMin = 1;
            }
        }
    }

    private void whenDamageAmountChanged(ObscuredDouble hp)
    {
        if (damageIndicator != null)
        {
            damageIndicator.SetText(Utils.ConvertBigNum(hp));
        }
        if (damagedAnim != null)
        {
            damagedAnim.SetTrigger(DamageAnimName);
        }
    }
    private void ResetGauge()
    {
        greenRenderer.fillAmount = 1f;
        greyRenderer.fillAmount = 1f;
    }

    private IEnumerator GreyRoutine()
    {
        while (true)
        {
            float lerpValue = Mathf.Lerp(greyRenderer.fillAmount, greenRenderer.fillAmount, Time.deltaTime * fixedLerpSpeed);

            greyRenderer.fillAmount = lerpValue;

            yield return null;
        }
    }

    [SerializeField] private bool isDamageText = false;
    public void UpdateGauge(double currentHp, double maxHp)
    {
        if (maxHp == 0f) return;

        greenRenderer.fillAmount = (float)(currentHp / maxHp);

        if (remainDescription != null)
        {
            if (currentHp > 0)
            {
                remainDescription.SetText($"{Utils.ConvertBigNum(currentHp)}");
            }
            else
            {
                remainDescription.SetText("클리어!");
            }
        }

        if (isDamageText)
        {
            whenDamageAmountChanged(maxHp - currentHp);
        }
    }
}
