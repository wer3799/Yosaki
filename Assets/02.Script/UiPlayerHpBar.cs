using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class UiPlayerHpBar : MonoBehaviour
{
    [SerializeField]
    private Transform barObject;

    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private Animator animator;

    private static string PlayTrigger = "Play";

    [SerializeField] private List<GameObject> maxHp;
    [SerializeField] private List<GameObject> hp;
    
    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        PlayerStatusController.Instance.maxHp.AsObservable().Subscribe(WhenMaxHpChanged).AddTo(this);
        PlayerStatusController.Instance.hp.AsObservable().Subscribe(WhenHpChanged).AddTo(this);
    }

    private void WhenMaxHpChanged(double value)
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            for (var i = 0; i < maxHp.Count; i++)
            {
                maxHp[i].SetActive(!(i >= value));
            }
        }
        else
        {
            WhenHpChanged(value);
        }
        
    }

    private void WhenHpChanged(double value)
    {

        double maxHp = PlayerStatusController.Instance.maxHp.Value;
        double currentHp = PlayerStatusController.Instance.hp.Value;
        if (GameManager.contentsType.IsDimensionContents())
        {
            for (var i = 0; i < hp.Count; i++)
            {
                hp[i].SetActive(!(i >= value));
            }
        }
        else
        {
            animator.SetTrigger(PlayTrigger);

            hpText.SetText($"{Utils.ConvertBigNum(currentHp)}/{Utils.ConvertBigNum(maxHp)}");
            barObject.transform.localScale = new Vector3((float)currentHp / (float)maxHp, barObject.transform.localScale.y, barObject.transform.localScale.z);
        }

    }

}
