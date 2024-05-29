using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMissionBoardTextAdjuster : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts;

    [SerializeField] private string eventName;
    [SerializeField] private Item_Type itemType;
    // Start is called before the first frame update
    void Start()
    {
        texts[0].SetText($"{eventName} 미션");
        texts[1].SetText($"{eventName} 출석");
        texts[2].SetText($"{eventName} 상점");
        texts[3].SetText($"{eventName} 일일 미션");
        texts[4].SetText($"{eventName} 시즌 미션");
        texts[5].SetText($"{eventName} 이벤트를 맞아 다양한 아이템을 구입해보세요!");
        texts[6].SetText($"패스 구매시\n{eventName} 미션보상 2배 획득 가능!");
        texts[7].SetText($"구매시\n{CommonString.GetItemName(itemType)} 즉시 획득!");



    }
}
