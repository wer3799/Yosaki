using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiFallEventUsedCollection : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI usedCountText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.usedCollectionCount).AsObservable().Subscribe(e =>
        {
            usedCountText.SetText($"교환한 {CommonString.GetItemName(Item_Type.Event_Kill1_Item)} 수 : {Utils.ConvertBigNum(e)}");
        }).AddTo(this);
    }
}
