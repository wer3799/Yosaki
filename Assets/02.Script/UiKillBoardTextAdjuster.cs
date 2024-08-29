using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiKillBoardTextAdjuster : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts;

    [SerializeField] private string eventName;
    [SerializeField] private Item_Type itemType;
    // Start is called before the first frame update
    void Start()
    {
        var itemName = CommonString.GetItemName(itemType);
        texts[0].SetText($"{eventName} 이벤트");
        texts[1].SetText($"{eventName} 패스");
        texts[2].SetText($"{itemName} 2배 획득!");
        texts[3].SetText($"이벤트 기간 동안 요괴들을 처치해서 {CommonString.GetJongsung(itemName,JongsungType.Type_EulRul)} 획득해보세요!!" +
                         $"\n<size=18><color=red>(요괴 1마리 처치시 1개 획득,100개 단위로 갱신)</color></size>");
        texts[4].SetText($"패스 구매시\n" +
                         $"\n{itemName} 획득량 2배\n" +
                         $"\n{eventName} 패스 추가보상 획득 가능!");
        texts[5].SetText($"구매시 {itemName} 즉시 획득");




    }
}
