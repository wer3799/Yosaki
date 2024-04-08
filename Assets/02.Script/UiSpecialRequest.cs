using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSpecialRequest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private UiSpecialRequestBossCell cell;
    [SerializeField] private Transform parent;

    [SerializeField] private Image bgImage;
    [SerializeField] private Sprite oddSeason;  
    [SerializeField] private Sprite evenSeason;  
    
    private List<UiSpecialRequestBossCell> cellContainer =new List<UiSpecialRequestBossCell>();

    private SpecialRequestTableData currentSeasonData;
    // Start is called before the first frame update
    void Start()
    {
        currentSeasonData = Utils.GetCurrentSeasonSpecialRequestData();

        MakeCells();

        SetPeriod();

        SetBgImage();
        
    }

    private void SetBgImage()
    {
        var isOdd = currentSeasonData.Id % 2 == 1;
        bgImage.sprite = isOdd ? oddSeason : evenSeason;
    }
    
    private void SetPeriod()
    {
        
        var endData = currentSeasonData.Enddate.Split('-');
        DateTime endDate = new DateTime(int.Parse(endData[0]), int.Parse(endData[1]), int.Parse(endData[2]));
        endDate = endDate.AddDays(1);//5월5일을 넣으면 5월6일00시에끝나야함.
        var date = endDate - ServerData.userInfoTable.currentServerTime;
            
        dateText.SetText($"{date.Days}일 {date.Hours}시간");
        
    }
    private void MakeCells()
    {
        var cellCount = currentSeasonData.Specialrequestbossid.Length;
        
        while (cellCount > cellContainer.Count)
        {
            var prefab = Instantiate(cell, parent);
            cellContainer.Add(prefab);
        }

        var bossData = TableManager.Instance.SpecialRequestBossTable.dataArray;
        
        for (var i = 0; i < cellContainer.Count; i++)
        {
            if (i < cellCount)
            {
                cellContainer[i].gameObject.SetActive(true);
                cellContainer[i].Initialize(bossData[currentSeasonData.Specialrequestbossid[i]], idx: i);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(false);
            }
        }

    }
}
