using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildSpecialReward : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI price;

    [SerializeField] private int _itemType;
    
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;


    
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        var itemType = (Item_Type)_itemType;
        ServerData.costumeServerTable.TableDatas[itemType.ToString()].hasCostume.AsObservable().Subscribe(e =>
        {
            if (e == false)
            {
                price.SetText("교환 가능");
            }
            else
            {
                price.SetText("보유중!");
            }

        }).AddTo(this);
    }

    private void Initialize()
    {
        var itemType = (Item_Type)_itemType;
        var idx = ServerData.costumeServerTable.TableDatas[itemType.ToString()].idx;
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        itemName.SetText(CommonString.GetItemName(itemType));
    }

    public void OnClickExchangeButton()
    {
        var itemType = (Item_Type)_itemType;

        if (ServerData.costumeServerTable.TableDatas[itemType.ToString()].hasCostume.Value)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
            return;
        }

        if (GuildManager.Instance.guildIconIdx.Value < 30)
        {
            PopupManager.Instance.ShowAlarmMessage("특별 문파 아이콘을 보유한 문파원만 교환이 가능합니다.");
            return;
        }

        ServerData.AddLocalValue(itemType, 1);

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }


    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    public IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param costumeParam = new Param();

        var itemType = (Item_Type)_itemType;
        costumeParam.Add(itemType.ToString(), ServerData.costumeServerTable.TableDatas[itemType.ToString()].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
        });
    }

}
