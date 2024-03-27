using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBridgeController : MonoBehaviour
{
    private void OnEnable()
    {
        this.gameObject.SetActive(CheckAllReceive());
    }

    private bool CheckAllReceive()
    {
        var max = TableManager.Instance.dolPass.dataArray.Length;

        var splitData_Free = GetSplitData(SeolPassServerTable.MonthlypassFreeReward_dol);
        var splitData_Ad = GetSplitData(SeolPassServerTable.MonthlypassAdReward_dol);

        //Debug.LogError($"{splitData_Free.Count}{splitData_Ad.Count}{max}" );
        
        return (splitData_Free.Count >= max && splitData_Ad.Count >= max)==false;

    } 
    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.seolPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }
}
