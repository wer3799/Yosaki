using System.Collections;
using System.Collections.Generic;
using BackEnd;
using GoogleMobileAds.Api;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiSpecialRequestClearRewardCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gradeText;
    
    [SerializeField] private ItemView rewardView;
    [SerializeField] private Transform rewardViewParent;

    [SerializeField] private Button rewardButton;

    private List<ItemView> cellContainer = new List<ItemView>();
    private SpecialRequestTableData tableData;

    private int rewardIdx = -1;
    public void Initialize(int idx)
    {
        rewardIdx = idx;
        
        tableData = Utils.GetCurrentSeasonSpecialRequestData();

        gradeText.SetText($"{rewardIdx + 1}단계");

        var cellCount = tableData.Rewardtype.Length;

        while (cellCount > cellContainer.Count)
        {
            var prefab = Instantiate(rewardView, rewardViewParent);
            cellContainer.Add(prefab);
        }
        
        for (var i = 0; i < cellContainer.Count; i++)
        {
            if (i < cellCount)
            {
                cellContainer[i].gameObject.SetActive(true);
                cellContainer[i].Initialize((Item_Type)tableData.Rewardtype[i],tableData.Rewardvalue[i]);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(false);
            }
        }
        
        Subscribe();
    }

    private void Subscribe()
    {
        var stringId =  Utils.GetCurrentSeasonSpecialRequestData().Stringid[rewardIdx];
        
                
        ServerData.specialRequestBossServerTable.TableDatas[stringId].score
            .AsObservable()
            .Subscribe(e=>
            {
                rewardButton.interactable = e >= 0;
                if (e >= 0)
                {
                    transform.SetAsFirstSibling();
                }
            })
            .AddTo(this);
        
        ServerData.specialRequestBossServerTable.TableDatas[stringId].isRewarded
            .AsObservable()
            .Subscribe(e=>
            {
                if (e > 0)
                {
                    transform.SetAsLastSibling();
                }
                rewardButton.gameObject.SetActive(e<1);
            })
            .AddTo(this);

    }

    public void SetArrange()
    {
        if (ServerData.specialRequestBossServerTable.TableDatas[Utils.GetCurrentSeasonSpecialRequestData().Stringid[rewardIdx]].score.Value >= 0)
        {
            transform.SetAsFirstSibling();
        }
        if (ServerData.specialRequestBossServerTable.TableDatas[Utils.GetCurrentSeasonSpecialRequestData().Stringid[rewardIdx]].isRewarded.Value > 0)
        {
            transform.SetAsLastSibling();
        }
    }

    private bool IsClear(string stringId)
    {
        //var stringId =  Utils.GetCurrentSeasonSpecialRequestData().Stringid[rewardIdx];
        return ServerData.specialRequestBossServerTable.TableDatas[stringId].score.Value > -1;
    }
    private bool IsRewarded(string stringId)
    {
        //var stringId =  Utils.GetCurrentSeasonSpecialRequestData().Stringid[rewardIdx];
        return ServerData.specialRequestBossServerTable.TableDatas[stringId].isRewarded.Value > 0;
    }
    
    public void OnClickButton()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();
        Param passParam = new Param();
        Param goodsParam = new Param();
        UiRewardResultPopUp.Instance.Clear();

        //stringId 배열
        var data = tableData.Stringid;

        for (int i = 0; i < data.Length; i++)
        {
            if (IsClear(data[i]) == false) continue;
            if (IsRewarded(data[i]) == true) continue;
            ServerData.specialRequestBossServerTable.TableDatas[data[i]].isRewarded.Value = 1;
            passParam.Add(data[i], ServerData.specialRequestBossServerTable.TableDatas[data[i]].ConvertToString());
            for (int j = 0; j < tableData.Rewardtype.Length; j++)
            {
                UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)tableData.Rewardtype[j], tableData.Rewardvalue[j]);
            }
        }

        if (UiRewardResultPopUp.Instance.RewardList.Count < 1)
        {
            return;
        }

        using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();

        while (e.MoveNext())
        {
            var type = ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType);
            ServerData.goodsTable.AddLocalData(type,e.Current.amount);
            goodsParam.Add(type, ServerData.goodsTable.GetTableData(type).Value);
        }
        
        //패스 보상
        transactionList.Add(TransactionValue.SetUpdate(SpecialRequestBossServerTable.tableName, SpecialRequestBossServerTable.Indate, passParam));
        
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        
        ServerData.SendTransactionV2(transactionList,successCallBack: () =>
        {
            UiRewardResultPopUp.Instance.Show().Clear();
        });
    }


}
