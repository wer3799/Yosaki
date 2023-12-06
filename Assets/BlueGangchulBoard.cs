using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum BlueGangChulUnlockType
{
    None=-1,
    Dosul,
    Level,
    Stage,
}

public class BlueGangchulBoard : MonoBehaviour
{
    
    
    [SerializeField] private BlueGangChulCell prefab;
    [SerializeField] private Transform transform;


    [SerializeField] private TextMeshProUGUI scoreText;
   

    private bool initialized = false;
    private void Start()
    {
        MakeCell();

        UpdateUi();
        
        initialized = true;
    }
    private void UpdateUi()
    {
        var score = ServerData.bossServerTable.TableDatas["b204"].score.Value;
        if (string.IsNullOrEmpty(score) == false)
        {
            scoreText.SetText($"최고 피해량 : {Utils.ConvertBigNum(double.Parse(score))}");
        }
        else
        {
            scoreText.SetText("기록 없음");
        }
        
    }
    
    private void MakeCell()
    {
        var tableData = TableManager.Instance.BlueGangCheol.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            //tableData[i].
            var cell = Instantiate<BlueGangChulCell>(prefab, transform);
            cell.Initialize(tableData[i]);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "도전 할까요?", () =>
        {
            GameManager.Instance.SetBossId(204);
            GameManager.Instance.LoadContents(GameManager.ContentsType.TwelveDungeon);
        }, () => { });
    }
}
