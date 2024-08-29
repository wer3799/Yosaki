using System;
using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiGangChulPassCostumeCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI price;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    private string itemKey = "costume123";

    private void Start()
    {
        Initialize();
    }

    private void Subscribe()
    {
        

            ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.AsObservable().Subscribe(e =>
            {
                if (e == false)
                {
                    price.SetText("획득");
                }
                else
                {
                    price.SetText("보유중!");
                }
            }).AddTo(this);
    }
    
    public void Initialize()
    {
    
        var idx = ServerData.costumeServerTable.TableDatas[itemKey].idx;
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        itemName.SetText(CommonString.GetItemName(Item_Type.costume123));
        
        Subscribe();
    }

    private bool HasPassItem()
    {
        return ServerData.iapServerTable.TableDatas[UiGangChulPassBuyButton.seasonPassKey].buyCount.Value > 0;
    }
    public void OnClickExchangeButton()
    {
    
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }
    
        if (ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.Value)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
            return;
        }

        if (HasPassItem() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"패스권이 필요합니다!");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");
    
        ServerData.AddLocalValue(Item_Type.costume123, 1);
    
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }
    
        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    
    }
    
    
    private Coroutine syncRoutine;
    
    private WaitForSeconds syncDelay = new WaitForSeconds(0.1f);
    
    public IEnumerator SyncRoutine()
    {
        yield return syncDelay;
    
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param costumeParam = new Param();

        string costumeKey = ((Item_Type)Item_Type.costume123).ToString();

        costumeParam.Add(costumeKey, ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
    
    
        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
        });
    
    }
}
