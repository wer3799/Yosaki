using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using static UiRewardView;
public class UiPartyTowerBoard : MonoBehaviour
{
    [SerializeField]
    private UiTower4RewardView uiTower4RewardView;

    [SerializeField]
    private TextMeshProUGUI currentStageText;

    [SerializeField]
    private GameObject normalRoot;

    [SerializeField]
    private GameObject allClearRoot;

    [SerializeField]
    private TextMeshProUGUI helpDescription;

    [SerializeField]
    private TextMeshProUGUI adTicketDescription;
    [SerializeField]
    private TextMeshProUGUI abilityText;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.partyTowerRecommend].AsObservable().Subscribe(e =>
        {
            helpDescription.SetText($"남은 도움 요청권 : {e}개");
        }).AddTo(this);
        ServerData.userInfoTable.TableDatas[UserInfoTable.receivedPartyTowerTicket].AsObservable().Subscribe(e =>
        {
            adTicketDescription.SetText(e == 0 ? $"요청권 획득\n(1주 1회)" : $"획득 완료");
        }).AddTo(this);
        ServerData.equipmentTable.TableDatas[EquipmentTable.CaveBelt].AsObservable().Subscribe(e =>
        {
            abilityText.SetText($"{CommonString.GetStatusName(StatusType.EnhanceAttack)} {Utils.ConvertNum(PlayerStats.GetCaveBeltEnhanceAttackAdd()*100,2)}");
        }).AddTo(this);
    }

    void OnEnable()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 300000)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨 30만부터 입장하실 수 있습니다.");
            this.gameObject.SetActive(false);
        }
        SetStageText();
        SetReward();
    }
    public void OnClickAdButton()
    {
        bool received = ServerData.userInfoTable.GetTableData(UserInfoTable.receivedPartyTowerTicket).Value == 1;
        if (received)
        {
            PopupManager.Instance.ShowAlarmMessage("일주일에 한 번 획득 가능합니다.");
            return;
        }

        AdManager.Instance.ShowRewardedReward(RewardAdFinished);
    }

    private void RewardAdFinished()
    {
        //�̹� �޾�����
        if(ServerData.userInfoTable.GetTableData(UserInfoTable.receivedPartyTowerTicket).Value==1)
        {
            return;
        }    
        ServerData.userInfoTable.TableDatas[UserInfoTable.partyTowerRecommend].Value++;
        ServerData.userInfoTable.GetTableData(UserInfoTable.receivedPartyTowerTicket).Value = 1f;

        List<TransactionValue> transactionList = new List<TransactionValue>();
     
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.receivedPartyTowerTicket, ServerData.userInfoTable.GetTableData(UserInfoTable.receivedPartyTowerTicket).Value);
        userInfoParam.Add(UserInfoTable.partyTowerRecommend, ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerRecommend).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList);
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value;

        return currentFloor >= TableManager.Instance.towerTableMulti.dataArray.Length;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value;
            currentStageText.SetText($"{currentFloor + 1}층");
        }
        else
        {
            currentStageText.SetText($"업데이트 예정 입니다");
        }

    }

    private void SetReward()
    {
        bool isAllClear = IsAllClear();

        normalRoot.SetActive(isAllClear == false);
        allClearRoot.SetActive(isAllClear == true);

        if (isAllClear == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value;

            if (currentFloor >= TableManager.Instance.towerTableMulti.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            var towerTableData = TableManager.Instance.towerTableMulti.dataArray[currentFloor];

            uiTower4RewardView.UpdateRewardView(towerTableData.Id);
        }
        else
        {
            
            var towerTableData = TableManager.Instance.towerTableMulti.dataArray.Last();

            uiTower4RewardView.UpdateRewardView(towerTableData.Id);
        }


    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () =>
        {

            GameManager.Instance.LoadContents(GameManager.ContentsType.DokebiTower);

        }, () => { });
    }
}
