using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeolAttenButton : MonoBehaviour
{
    private static bool active = true;

    void OnEnable()
    {
        if (active == true)
        {
            CheckEnable();
        }

        this.gameObject.SetActive(active);
    }

    private void CheckEnable()
    {
        var tableData = TableManager.Instance.SeolPass.dataArray;

        string freeKey = SeolPassServerTable.MonthlypassFreeReward;
        string addKey = SeolPassServerTable.MonthlypassAdReward;

        var freeSplits = GetSplitData(freeKey);
        var adSlpits = GetSplitData(addKey);

        bool allReceived = true;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (HasReward(freeSplits, tableData[i].Id) == false)
            {
                allReceived = false;
                break;
            }
            if (HasReward(adSlpits, tableData[i].Id) == false)
            {
                allReceived = false;
                break;
            }
        }

        active = (allReceived == false);
    }

    public bool HasReward(List<string> splitData, int data)
    {
        return splitData.Contains(data.ToString());
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.seolPassServerTable.TableDatas[key].Value.Split(',').ToList();
    }

}
