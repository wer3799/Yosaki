using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiTowerResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI amountText;
    [SerializeField]
    private GameObject amountObject;
    public void Initialize(double damagedAmount,float GoodsAmount)
    {
        scoreText.SetText(Utils.ConvertBigNum(damagedAmount));
        amountText.SetText($"{Utils.ConvertNum(GoodsAmount)}개 획득!");

        amountObject.SetActive(GoodsAmount > 0);
    
    }
}
