using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PassInfo
{
    public int require;

    public ObscuredInt id;

    public ObscuredInt rewardType_Free;
    public ObscuredFloat rewardTypeValue_Free;
    public ObscuredString rewardType_Free_Key;

    public ObscuredInt rewardType_IAP;
    public ObscuredFloat rewardTypeValue_IAP;
    public ObscuredString rewardType_IAP_Key;

    public ObscuredString shopId;

    public ObscuredString key0;
    
    public int passGrade;
}
public class DamagePassInfo
{
    public double require;

    public ObscuredInt id;

    public ObscuredInt rewardType_Free;
    public ObscuredFloat rewardTypeValue_Free;
    public ObscuredString rewardType_Free_Key;

    public ObscuredInt rewardType_IAP;
    public ObscuredFloat rewardTypeValue_IAP;
    public ObscuredString rewardType_IAP_Key;

    public ObscuredString shopId;

    public int passGrade;
}
public class EquipmentPassInfo
{
    public string require;

    public ObscuredInt id;

    public ObscuredInt rewardType_Free;
    public ObscuredFloat rewardTypeValue_Free;
    public ObscuredString rewardType_Free_Key;

    public ObscuredInt rewardType_IAP;
    public ObscuredFloat rewardTypeValue_IAP;
    public ObscuredString rewardType_IAP_Key;

    public ObscuredString shopId;

    public int passGrade;
}

public enum PassType
{
    Level, Daily
}
public enum RewardItemType
{
    GoodsItem,
    PassItem,
}

public class UiPassSystem : MonoBehaviour
{
    //[SerializeField]
    //private UiPassCell uiPassCellPrefab;

    //[SerializeField]
    //private Transform cellParent;

    //private List<UiPassCell> uiPassCellContainer = new List<UiPassCell>();

    //[SerializeField]
    //private Button passBuyButton;

    //[SerializeField]
    //private TextMeshProUGUI passBuyButtonText;

    //private ObscuredString passShopId;

    //private int currentIdx = 0;
    //private void Start()
    //{
    //    Subscribe();
    //}

    //private void Subscribe()
    //{
    //    ServerData.userInfoTable.GetTableData(UserInfoTable.passSelectedIdx).AsObservable().Subscribe(WhenSelectedIdxChanged).AddTo(this);

    //    IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(WhenBuyComplete).AddTo(this);
    //}

    //private void WhenBuyComplete(PurchaseEventArgs args)
    //{
    //    if (args.purchasedProduct.definition.id == passShopId)
    //    {
    //        WhenSelectedIdxChanged(ServerData.userInfoTable.GetTableData(UserInfoTable.passSelectedIdx).Value);
    //        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "구매 성공!", null);
    //    }
    //}

    //private void WhenSelectedIdxChanged(float id)
    //{
    //    currentIdx = (int)id;

    //    int groupId = (int)id;

    //    var tableData = TableManager.Instance.GetLevelPassDataByGroup(groupId);

    //    if (tableData == null || tableData.Count == 0)
    //    {
    //        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"미등록 된그룹 {groupId}", null);
    //        return;
    //    }
    //    else
    //    {
    //        passShopId = tableData[0].Shopid;
    //    }

    //    UpdatePassView(groupId);

    //    SetPassButton();
    //}

    //private void UpdatePassView(int groupId)
    //{
    //    var tableData = TableManager.Instance.GetLevelPassDataByGroup(groupId);

    //    int interval = tableData.Count - uiPassCellContainer.Count;

    //    for (int i = 0; i < interval; i++)
    //    {
    //        var prefab = Instantiate<UiPassCell>(uiPassCellPrefab, cellParent);
    //        uiPassCellContainer.Add(prefab);
    //    }

    //    for (int i = 0; i < uiPassCellContainer.Count; i++)
    //    {
    //        if (i < tableData.Count)
    //        {
    //            var passInfo = new PassInfo();

    //            passInfo.require = tableData[i].Unlocklevel;
    //            passInfo.id = tableData[i].Id;

    //            passInfo.rewardType_Free = tableData[i].Reward1;
    //            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
    //            passInfo.rewardType_Free_Key = PassServerTable.stagePassReward;

    //            passInfo.rewardType_IAP = tableData[i].Reward2;
    //            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
    //          //  passInfo.rewardType_IAP_Key = PassServerTable.levelpassIAPReward;

    //            passInfo.shopId = tableData[i].Shopid;

    //            uiPassCellContainer[i].gameObject.SetActive(true);
    //            uiPassCellContainer[i].Initialize(passInfo);
    //        }
    //        else
    //        {
    //            uiPassCellContainer[i].gameObject.SetActive(false);
    //        }
    //    }

    //    cellParent.transform.localPosition = new Vector3(0f, cellParent.transform.localPosition.y, cellParent.transform.localPosition.z);
    //}

    //private void SetPassButton()
    //{
    //    //상품보유
    //    if (IAPManager.Instance.HasProduct(passShopId))
    //    {
    //        passBuyButton.interactable = false;
    //        passBuyButtonText.SetText("보유중");
    //    }
    //    //미보유
    //    else
    //    {
    //        passBuyButton.interactable = true;
    //        passBuyButtonText.SetText($"{currentIdx+1}단계\n패스 구매");
    //    }
    //}

    //public void OnClickPassBuyButton()
    //{
    //    if (IAPManager.Instance.HasProduct(passShopId))
    //    {
    //        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "이미 보유중입니다.", null);
    //    }

    //    IAPManager.Instance.BuyProduct(passShopId);
    //}

    //public void RefreshPassView(int groupId)
    //{
    //    SoundManager.Instance.PlayButtonSound();
    //    ServerData.userInfoTable.GetTableData(UserInfoTable.passSelectedIdx).Value = groupId;
    //}
}
