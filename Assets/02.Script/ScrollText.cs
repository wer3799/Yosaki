using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollText : MonoBehaviour
{
    public Scrollbar scrollbar;
    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
    }

    private void OnScrollbarValueChanged(float value)
    {
        // 스크롤바의 값에 따라 텍스트의 Y 위치를 조정하여 스크롤 효과를 구현합니다.
        float contentYPos = Mathf.Lerp(0, textMeshPro.preferredHeight - textMeshPro.rectTransform.rect.height, value);
        textMeshPro.rectTransform.anchoredPosition = new Vector2(0, contentYPos);
    }
}
