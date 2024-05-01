using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYeoRaeBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;


    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[51]);

    }
}
