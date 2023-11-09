using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;

public class UiCaveTwoBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentFloorDescription;
    [SerializeField]
    private TextMeshProUGUI currentFloorAbilDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        string bossKey = "b91";

        var serverData = ServerData.bossServerTable.TableDatas[bossKey];

        serverData.score.AsObservable().Subscribe(e =>
        {
            if (string.IsNullOrEmpty(e))
            {
                currentFloorDescription.SetText($"점수 없음");
                currentFloorAbilDescription.SetText($"효과 없음");
            }
            else
            {

                if (int.TryParse(e, out var score))
                {
                    currentFloorDescription.SetText($"현재 단계 : {score}");
                    currentFloorAbilDescription.SetText($"십만동굴 허리띠 효과 : +{PlayerStats.GetTwoCaveBeltAbilPlusValue() * 100}%");
                }
                else
                {
                    currentFloorDescription.SetText($"점수 없음");
                    currentFloorAbilDescription.SetText($"효과 없음");
                }
            }
        }).AddTo(this);

        
    }

    public void OnClickAllReceiveButton()
    {

        string bossStringId = "b91";
        if(double.TryParse(ServerData.bossServerTable.TableDatas[bossStringId].score.Value,out double score)==false)
        {
            PopupManager.Instance.ShowAlarmMessage("점수를 등록해주세요!");
            return;
        }

        var tableData = TableManager.Instance.TwelveBossTable.dataArray[91];

        var rewardedIdxList = ServerData.bossServerTable.GetShadowCaveRewardedIdxList();

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
                if(rewardedIdxList.Contains(i) ==false)
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
            ServerData.bossServerTable.TableDatas[bossStringId].rewardedId.Value += addStringValue;

            Param bossParam = new Param();
            bossParam.Add(bossStringId, ServerData.bossServerTable.TableDatas[bossStringId].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            Param goodsParam = new Param();
            var e = rewardTypes.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current), ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(e.Current)).Value);
            }

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
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
