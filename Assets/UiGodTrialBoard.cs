using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiGodTrialBoard : MonoBehaviour
{
    [SerializeField]
    private SeletableTab selectableTab;

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        if (selectableTab != null)
        {
            if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestSword)
            {
                selectableTab.OnSelect(0);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestMonkey)
            {
                selectableTab.OnSelect(1);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestHell)
            {
                selectableTab.OnSelect(2);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestChun)
            {
                selectableTab.OnSelect(3);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestDo)
            {
                selectableTab.OnSelect(4);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestSumi)
            {
                selectableTab.OnSelect(5);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestThief)
            {
                selectableTab.OnSelect(6);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestDark)
            {
                selectableTab.OnSelect(7);
            }
            else if (GameManager.Instance.lastContentsType == GameManager.ContentsType.TestSin)
            {
                selectableTab.OnSelect(8);
            }
        }

    }

}
