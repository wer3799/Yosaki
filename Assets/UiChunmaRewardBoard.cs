using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;
using BackEnd;

public class UiChunmaRewardBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Normal;

    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Recommend;
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView_Recommend2;

    [SerializeField]
    private TextMeshProUGUI RecommendCount;
    [SerializeField]
    private TextMeshProUGUI RecommendCount2;

    private void Start()
    {
        Initialize();
        Subscribe();


    }

    private void Subscribe()
    {
        ServerData.bossServerTable.TableDatas["b68"].score.AsObservable().Subscribe(e=>
        {
            if (RecommendCount != null)
            {
                if (string.IsNullOrEmpty(ServerData.bossServerTable.TableDatas["b68"].score.Value))
                {
                    RecommendCount.SetText($"받은 추천 : 0");
                    ServerData.bossServerTable.TableDatas["b176"].score.Value = e;
                }
                else
                {
                    RecommendCount.SetText($"받은 추천 : {ServerData.bossServerTable.TableDatas["b68"].score.Value}");
                    ServerData.bossServerTable.TableDatas["b176"].score.Value = e;

                }
            }
        }).AddTo(this);
        ServerData.bossServerTable.TableDatas["b176"].score.AsObservable().Subscribe(e=>
        {
            if (RecommendCount2 != null)
            {
                if (string.IsNullOrEmpty(ServerData.bossServerTable.TableDatas["b176"].score.Value))
                {
                    RecommendCount2.SetText($"받은 추천 : 0");
                }
                else
                {
                    RecommendCount2.SetText($"받은 추천 : {ServerData.bossServerTable.TableDatas["b176"].score.Value}");
                }
            }
        }).AddTo(this);
    }

    private void Initialize()
    {
        bossContentsView_Normal.Initialize(TableManager.Instance.TwelveBossTable.dataArray[55]);

        bossContentsView_Recommend.Initialize(TableManager.Instance.TwelveBossTable.dataArray[68]);

        bossContentsView_Recommend2.Initialize(TableManager.Instance.TwelveBossTable.dataArray[176]);
    }
    
    public void OnClickAllReceiveButton()
    {
        if(double.TryParse(ServerData.bossServerTable.TableDatas["b55"].score.Value,out double score)==false)
        {
            PopupManager.Instance.ShowAlarmMessage("점수를 등록해주세요!");
            return;
        }

        var tableData = TableManager.Instance.TwelveBossTable.dataArray[55];

        var chunmaRewardedIdxList = ServerData.bossServerTable.GetChunmaRewardedIdxList();

        int rewardCount = 0;

        string addStringValue = string.Empty;

        List<Item_Type> rewardTypes = new List<Item_Type>();

        for (int i = 0; i < tableData.Rewardcut.Length; i++)
        {
            if(score< tableData.Rewardcut[i])
            {
                break;
            }
            else
            {
                if(chunmaRewardedIdxList.Contains(i) ==false)
                {
                    
                    float amount = tableData.Rewardvalue[i];

                    addStringValue += $"{BossServerTable.rewardSplit}{i}";

                    ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Rewardtype[i])).Value += (int)amount;

                    if (!rewardTypes.Contains((Item_Type)tableData.Rewardtype[i]))
                    {
                        rewardTypes.Add((Item_Type)tableData.Rewardtype[i]);
                    }
                    rewardCount++;
                }
            }
        }

        if (rewardCount != 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();
            ServerData.bossServerTable.TableDatas["b55"].rewardedId.Value += addStringValue;

            Param bossParam = new Param();
            bossParam.Add("b55", ServerData.bossServerTable.TableDatas["b55"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            Param goodsParam = new Param();
            var e = rewardTypes.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current), ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(e.Current)).Value);
            }

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("받을수 있는 보상이 없습니다.");
        }
    }

}
