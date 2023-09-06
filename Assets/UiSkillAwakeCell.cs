using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiSkillAwakeCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelDescription;

    [SerializeField]
    private string key;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(key).AsObservable().Subscribe(e =>
        {
            levelDescription.SetText($"+{e}");
        }).AddTo(this);
    }

    public void OnClickUpgradeButton0()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("강화 포인트가 부족합니다.");
            return;
        }

        ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value--;
        ServerData.statusTable.GetTableData(key).Value++;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    public void OnClickUpgradeButton1()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("강화 포인트가 부족합니다.");
            return;
        }

        var remainPoint = ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value;

        var addNum = Mathf.Min(remainPoint, 100);

        ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value -= (int)addNum;

        ServerData.statusTable.GetTableData(key).Value += (int)addNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    public void OnClickUpgradeButtonAll()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("강화 포인트가 부족합니다.");
            return;
        }

        var remainPoint = ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value;

        var addNum = remainPoint;

        ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value -= (int)addNum;

        ServerData.statusTable.GetTableData(key).Value += (int)addNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private Coroutine syncRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);
    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param statusParam = new Param();
        statusParam.Add(key, ServerData.statusTable.GetTableData(key).Value);
        statusParam.Add(StatusTable.SkillAdPoint, ServerData.statusTable.GetTableData(StatusTable.SkillAdPoint).Value);

        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
#if UNITY_EDITOR
              Debug.LogError("SkillAwake sync complete");
#endif
          });
    }
}
