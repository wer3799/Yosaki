using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiHotTimeEventBoardTextAdjuster : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts;

    [SerializeField] private string eventName;
    [SerializeField] private Item_Type itemType;
    // Start is called before the first frame update
    void Start()
    {
        texts[0].SetText($"{eventName} 이벤트");
        texts[1].SetText($"{eventName} 버프는 이벤트 기간 동안 상시 적용됩니다 !\n오프라인 상태에서도 핫타임 버프는 적용됩니다 !");
        texts[2].SetText($"{eventName} 버프 1");
        texts[3].SetText($"{eventName} 버프 2(패스권 필요)");
        texts[4].SetText($"접속시간 10분마다 {CommonString.GetJongsung(CommonString.GetItemName(itemType),JongsungType.Type_EulRul)} 획득할 수 있습니다!\n" +
                         $"{CommonString.GetJongsung(CommonString.GetItemName(itemType),JongsungType.Type_EulRul)} 통해서 다른 재화로 교환하실 수 있습니다!");
        texts[5].SetText($"패스 구매시\n{CommonString.GetItemName(itemType)} 2배 획득 가능!)");
        texts[6].SetText($"구매시 {CommonString.GetItemName(itemType)} 즉시 획득 :");



    }
}
