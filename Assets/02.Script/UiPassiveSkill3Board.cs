using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine.Serialization;

public class UiPassiveSkill3Board : MonoBehaviour
{
    [SerializeField]
    private UiPassiveSkill3Cell cellPrefab;


    [SerializeField]
    private Transform passiveSkillCellParent;

    [SerializeField]
    private TextMeshProUGUI description;

    private void Start()
    {
        InitView();
    }

    private void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        InitStat();

        description.SetText($"강화 포인트는 {Utils.ConvertBigNum(GameBalance.passive3PointDivideNum)} 레벨당 1개씩 획득 합니다");
    }


    private void InitStat()
    {
        //포인트 없으면 리턴
        if (GetMaxSkillAwakePoint() <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertNum(GameBalance.passive3UnlockLevel)} 레벨을 달성해야 합니다!");
            return;
        }
        //패시브 스킬 초기화
        var tableData = TableManager.Instance.PassiveSkill3.dataArray;

        int passiveSkill3Point = GetMaxSkillAwakePoint();

        for (int i = 0; i < tableData.Length; i++)
        {
            //배운스킬 뺌
            passiveSkill3Point -= ServerData.passive3ServerTable.TableDatas[tableData[i].Stringid].level.Value;
        }

        //max-투자한포인트를 포인트로 환산
        ServerData.statusTable.GetTableData(StatusTable.Skill3Point).Value = passiveSkill3Point;
    }
    private void InitView()
    {
        var passiveSkillList = TableManager.Instance.PassiveSkill3.dataArray.ToList();

        for (int i = 0; i < passiveSkillList.Count; i++)
        {
           
            var cell = Instantiate<UiPassiveSkill3Cell>(cellPrefab, passiveSkillCellParent);

            cell.Refresh(passiveSkillList[i]);
            
        }
    }
    public int GetMaxSkillAwakePoint()
    {
        int level = (int)ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        return Mathf.Max((level) / GameBalance.passive3PointDivideNum, 0);
    }

    public void OnClickResetPassiveSkill()
    {
        //패시브 스킬 초기화
        var tableData = TableManager.Instance.PassiveSkill3.dataArray;

        int passiveSkillPoint = 0;

        for (int i = 0; i < tableData.Length; i++)
        {
            passiveSkillPoint += ServerData.passive3ServerTable.TableDatas[tableData[i].Stringid].level.Value;
            ServerData.passive3ServerTable.TableDatas[tableData[i].Stringid].level.Value = 0;
        }

        //리셋한 데이터 적용
        ServerData.statusTable.GetTableData(StatusTable.Skill3Point).Value += passiveSkillPoint;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        statusParam.Add(StatusTable.Skill3Point, ServerData.statusTable.GetTableData(StatusTable.Skill3Point).Value);

        Param passiveSkillParam = new Param();
        var passiveTableData = TableManager.Instance.PassiveSkill3.dataArray;

        for (int i = 0; i < passiveTableData.Length; i++)
        {
            passiveSkillParam.Add(passiveTableData[i].Stringid, ServerData.passive3ServerTable.TableDatas[passiveTableData[i].Stringid].ConvertToString());
        }

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactionList.Add(TransactionValue.SetUpdate(Passive3ServerTable.tableName, Passive3ServerTable.Indate, passiveSkillParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "포인트 초기화 성공!", null);
        });
    }

}
