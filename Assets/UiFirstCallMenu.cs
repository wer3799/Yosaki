using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFirstCallMenu : MonoBehaviour
{
    [SerializeField]
    private List<MainTabButtons> buttons;

    [SerializeField] private GameObject prefab;

    private static bool isFirst = true;

    void Start()
    {
        if (isFirst) 
        {
            isFirst = false;
            buttons.ForEach(e => e.OnClickButton());
        }
    }

}
