using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;

public class UiFullMoonCostumeBuyButton : MonoBehaviour
{
    string costumeKey = "costume252";
    [SerializeField] private CharacterView characterView;
    private void Start()
    {
        Subscribe();
        characterView.Initialize(costumeKey);
    }

    private void Subscribe()
    {
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.AsObservable().Subscribe(e =>
        {
            this.gameObject.SetActive(!e);
        }).AddTo(this);
    }
    
    public void OnClickGetCostumeButton()
    {
        if (ServerData.iapServerTable.TableDatas[UiEvent2PassBuyButton.productKey].buyCount.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("패스권이 필요합니다!");
            return;
        }
        
 
        
        if(ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value==true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중입니다!");
            return;
        };
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value = true;
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param costumeParam = new Param();

        costumeParam.Add(costumeKey.ToString(), ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
        });
    }
}
