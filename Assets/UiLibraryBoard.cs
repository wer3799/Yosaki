using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLibraryBoard : MonoBehaviour
{

    [SerializeField]
    private UiTwelveBossContentsView twelveBossContentsView;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        twelveBossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[72]);
    }

}
