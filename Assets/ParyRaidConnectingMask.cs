using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParyRaidConnectingMask : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI connectingText;

    [SerializeField] private string text;
    private void OnEnable()
    {
        StartCoroutine(TextingRoutine());
    }

    private IEnumerator TextingRoutine()
    {
        var ws = new WaitForSeconds(0.5f);

        while (true)
        {
            connectingText.SetText($"{text}.");
            yield return ws;
            connectingText.SetText($"{text}.");
            yield return ws;
            connectingText.SetText($"{text}..");
            yield return ws;
            connectingText.SetText($"{text}...");
            yield return ws;

        }
    }
}
