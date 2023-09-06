using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiSkillAwakeBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lockDescription;

    [SerializeField]
    private GameObject lockObject;

    [SerializeField]
    private TextMeshProUGUI remainPoints;

    private void Start()
    {
        Initialize();

        Subscribe();
    }

    private void Initialize()
    {

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.skillInitialized).Value == 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.userInfoTable.TableDatas[UserInfoTable.skillInitialized].Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value = GetMaxSkillAwakePoint();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.skillInitialized, ServerData.userInfoTable.TableDatas[UserInfoTable.skillInitialized].Value);
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            Param statusParam = new Param();
            statusParam.Add(StatusTable.SkillAdPoint, ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value);
            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

            ServerData.SendTransaction(transactions);
        }

        lockDescription.SetText($"요물무기가 필요합니다.");

    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).AsObservable().Subscribe(e =>
        {
            remainPoints.SetText($"강화 포인트 {e}");
        }).AddTo(this);

        ServerData.weaponTable.TableDatas["weapon20"].hasItem.AsObservable().Subscribe(e =>
        {
            lockObject.SetActive(e == 0);
        }).AddTo(this);
    }

    public int GetMaxSkillAwakePoint()
    {
        return ((int)ServerData.statusTable.GetTableData(StatusTable.Level).Value) / 20;
    }

    public void OnClickResetButton()
    {
        var currentMaxPoint = GetMaxSkillAwakePoint();

        currentMaxPoint -= (int)ServerData.statusTable.GetTableData(StatusTable.Skill0_AddValue).Value;
        currentMaxPoint -= (int)ServerData.statusTable.GetTableData(StatusTable.Skill1_AddValue).Value;
        currentMaxPoint -= (int)ServerData.statusTable.GetTableData(StatusTable.Skill2_AddValue).Value;

        ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value = currentMaxPoint;

        Param statusParam = new Param();

        statusParam.Add(StatusTable.SkillAdPoint, ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value);

        List<TransactionValue> transactions = new List<TransactionValue>();

        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage($"갱신 성공!");
        });
    }

    public void OnClickAllResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말 초기화 합니까?", () =>
        {
            ServerData.statusTable.GetTableData(StatusTable.Skill0_AddValue).Value = 0;
            ServerData.statusTable.GetTableData(StatusTable.Skill1_AddValue).Value = 0;
            ServerData.statusTable.GetTableData(StatusTable.Skill2_AddValue).Value = 0;
            ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value = GetMaxSkillAwakePoint();

            Param statusParam = new Param();

            statusParam.Add(StatusTable.Skill0_AddValue, ServerData.statusTable.GetTableData(StatusTable.Skill0_AddValue).Value);
            statusParam.Add(StatusTable.Skill1_AddValue, ServerData.statusTable.GetTableData(StatusTable.Skill1_AddValue).Value);
            statusParam.Add(StatusTable.Skill2_AddValue, ServerData.statusTable.GetTableData(StatusTable.Skill2_AddValue).Value);
            statusParam.Add(StatusTable.SkillAdPoint, ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value);

            List<TransactionValue> transactions = new List<TransactionValue>();

            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"초기화 성공!", null);
            });
        }, () => { });
    }
}
