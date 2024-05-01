using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYorinGrowthPass : MonoBehaviour
{
    [SerializeField] private SeletableTab seletableTab;
    [SerializeField] private List<GameObject> passList = new List<GameObject>();

    private int stageLimit = GameBalance.yorinPassLockStage;
    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < stageLimit)
        {
            //아무 금지사항 없음.
        }
        //stageLimit
        else
        {
            var list = ServerData.iapServerTable.GetBuyYorinPassProductIdList();
            if (list.Count>0)
            {
                for (int i = 0; i < passList.Count; i++)
                {
                    passList[i].SetActive(false);
                } 

                using var e = list.GetEnumerator();
                bool isAllReceive = true;
                var tableData = TableManager.Instance.YorinPass.dataArray;
                while (e.MoveNext())
                {
                    for (int i = 0; i < tableData.Length; i++)
                    {
                        if (tableData[i].Productid != e.Current) continue;
                        if (int.Parse(ServerData.yorinPassServerTable.TableDatas[e.Current].Value) >=
                            tableData[i].Adrewardcount)
                        {
                            continue;
                        }
                        else
                        {
                            isAllReceive = false;
                            var idx = (int)tableData[i].YORINPASSTYPE;
                            switch (tableData[i].YORINPASSTYPE)
                            {
                                case YorinPassType.SealSword:
                                    passList[idx].SetActive(true);
                                    break;
                                case YorinPassType.NewGacha:
                                    passList[idx].SetActive(true);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }


                        }
                    }
                }


                if (isAllReceive == true)
                {
                    if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value >= stageLimit)
                    {
                        this.gameObject.SetActive(false);
                        seletableTab.OnSelect(0);
                        PopupManager.Instance.ShowAlarmMessage(
                            $"스테이지 {Utils.ConvertStage(stageLimit)} 미만인 요린이만 구입할 수 있는 패스입니다!");
                    }
                }
                else
                {
                    //Dont Receive
                }
            }
            //구매상품 없음.
            else
            {
                this.gameObject.SetActive(false);
                seletableTab.OnSelect(0);
                PopupManager.Instance.ShowAlarmMessage(
                    $"스테이지 {Utils.ConvertStage(stageLimit)} 미만인 요린이만 구입할 수 있는 패스입니다!");
            }
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
