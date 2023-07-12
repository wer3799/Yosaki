using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPetHomeView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI petName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private PetTableData petData;

    private PetServerData petServerData;

    [SerializeField]
    private GameObject notHasObject;

    [SerializeField]
    private Image rewardIcon;
    [SerializeField]
    private Image reward1Icon;

    [SerializeField]
    private TextMeshProUGUI rewardValue;
    [SerializeField]
    private TextMeshProUGUI reward1Value;

    [SerializeField]
    private Button rewardButton;
    [SerializeField]
    private Button reward1Button;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;
    [SerializeField]
    private TextMeshProUGUI reward1Description;


    public void Initialize(PetTableData petData)
    {
        SetPetSpine(petData.Id);

        this.petData = petData;

        this.petServerData = ServerData.petTable.TableDatas[petData.Stringid];

        petName.SetText($"{petData.Name}");

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)petData.Getrewardtype);
        reward1Icon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)petData.Getrewardtype1);

        rewardValue.SetText(Utils.ConvertBigNum(petData.Getrewardvalue));
        reward1Value.SetText(Utils.ConvertBigNum(petData.Getrewardvalue1));

        Subscribe();
    }

    private void Subscribe()
    {
        petServerData.hasItem.AsObservable().Subscribe(e =>
        {
            notHasObject.SetActive(e == 0);

            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);

        ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward].AsObservable().Subscribe(e =>
        {

            bool hasReward = ServerData.etcServerTable.HasPetHomeReward(petData.Id);

            rewardButton.interactable = !hasReward;

            rewardDescription.SetText(!hasReward ? "보상수령" : "수령완료");

        }).AddTo(this);
        ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward1].AsObservable().Subscribe(e =>
        {

            bool hasReward = ServerData.etcServerTable.HasPetHomeReward1(petData.Id);

            reward1Button.interactable = !hasReward;

            reward1Description.SetText(!hasReward ? "보상수령" : "수령완료");

        }).AddTo(this);

    }

    private void SetPetSpine(int idx)
    {

        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];

        if (idx != 15)
        {
            skeletonGraphic.startingAnimation = "walk";
        }
        else
        {
            skeletonGraphic.startingAnimation = "idel";
        }

        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        if (idx >= 15&&idx<=23)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }

        else if (idx >= 24 && idx <= 27)
        {
            skeletonGraphic.transform.localScale = new Vector3(1f, 1f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -145f, 1f);
        }
        else if (idx >= 28 && idx <= 31)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }
        else if (idx >= 32 && idx <= 35)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -38.75f, 1f);
        }
        else if (idx >= 36 && idx <= 39)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -65.75f, 1f);
        }
        else if (idx >= 48 && idx <= 51)
        {
            skeletonGraphic.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(1.6f, -177.1f, 1f);
        }
        else
        {
            skeletonGraphic.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -65.75f, 1f);
        }
    }

    public void OnClickGetRewardButton()
    {
        if (ServerData.etcServerTable.HasPetHomeReward(petData.Id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Item_Type rewardType = (Item_Type)petData.Getrewardtype;

        float rewardValue = petData.Getrewardvalue;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));

        ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward].Value += $"{BossServerTable.rewardSplit}{petData.Id}";

        Param etcParam = new Param();
        etcParam.Add(EtcServerTable.PetHomeReward, ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
    public void OnClickGetAdRewardButton()
    {
        if (ServerData.etcServerTable.HasPetHomeReward1(petData.Id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (ServerData.iapServerTable.TableDatas[UiEquipmentCollectionPassBuyButton.collectionPassKey].buyCount.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("도감 패스권이 필요합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Item_Type rewardType = (Item_Type)petData.Getrewardtype;

        float rewardValue = petData.Getrewardvalue;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));

        ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward1].Value += $"{BossServerTable.rewardSplit}{petData.Id}";

        Param etcParam = new Param();
        etcParam.Add(EtcServerTable.PetHomeReward1, ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward1].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
}
