using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDarkContentsBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView uiBossContentsViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField] private Difficulty difficulty = Difficulty.Normal;
    
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.TwelveBossTable.dataArray;

        switch (difficulty)
        {
            case Difficulty.Hard:
                
                //길드보스 -1
                for (int i = 281; i < 289; i++)
                {
                    var cell = Instantiate<UiTwelveBossContentsView>(uiBossContentsViewPrefab, cellParent);

                    cell.Initialize(tableDatas[i]);
                }
                break;
            case Difficulty.Normal:
                //길드보스 -1
                for (int i = 124; i < 132; i++)
                {
                    var cell = Instantiate<UiTwelveBossContentsView>(uiBossContentsViewPrefab, cellParent);

                    cell.Initialize(tableDatas[i]);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}
