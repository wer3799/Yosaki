using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHellContentsBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView uiBossContentsViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField] private Difficulty _difficulty = Difficulty.Normal;
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.TwelveBossTable.dataArray;
        
        switch (_difficulty)
        {
            case Difficulty.Normal:
                //길드보스 -1
                for (int i = 40; i < 50; i++)
                {
                    var cell = Instantiate<UiTwelveBossContentsView>(uiBossContentsViewPrefab, cellParent);

                    cell.Initialize(tableDatas[i]);
                }
                break;
            case Difficulty.Hard:
                for (int i = 221; i < 231; i++)
                {
                    var cell = Instantiate<UiTwelveBossContentsView>(uiBossContentsViewPrefab, cellParent);

                    cell.Initialize(tableDatas[i]);
                }
                break;
        }
        

    }
}
