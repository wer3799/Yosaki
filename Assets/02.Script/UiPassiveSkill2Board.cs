using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BackEnd;
using TMPro;

public class UiPassiveSkill2Board : MonoBehaviour
{
    [SerializeField]
    private UiPassiveSkill2Cell passiveSkill2CellPrefab;


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

        description.SetText($"강화 포인트는 {Utils.ConvertBigNum(GameBalance.passive2PointDivideNum)} 레벨당 1개씩 획득 합니다");
    }


    private void InitStat()
    {
        //포인트 없으면 리턴
        if (GetMaxSkillAwakePoint() <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertNum(GameBalance.passive2UnlockLevel)} 레벨을 달성해야 합니다!");
            return;
        }
        //패시브 스킬 초기화
        var tableData = TableManager.Instance.PassiveSkill2.dataArray;

        //패시브2 포인트 최대치
        int passiveSkill2Point = GetMaxSkillAwakePoint();

        for (int i = 0; i < tableData.Length; i++)
        {
            //배운스킬 뺌
            passiveSkill2Point -= ServerData.passive2ServerTable.TableDatas[tableData[i].Stringid].level.Value;
        }

        //max-투자한포인트를 포인트로 환산
        ServerData.statusTable.GetTableData(StatusTable.Skill2Point).Value = passiveSkill2Point;
    }
    private void InitView()
    {
        var passiveSkillList = TableManager.Instance.PassiveSkill2.dataArray.ToList();

        for (int i = 0; i < passiveSkillList.Count; i++)
        {
            if (passiveSkillList[i].Issinpassive == false)
            {
                var cell = Instantiate<UiPassiveSkill2Cell>(passiveSkill2CellPrefab, passiveSkillCellParent);

                cell.Refresh(passiveSkillList[i]);
            }
        }
    }
    public int GetMaxSkillAwakePoint()
    {
        int level = (int)ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        return Mathf.Max((level) / GameBalance.passive2PointDivideNum, 0);
    }

    public void OnClickResetPassiveSkill2()
    {
        //패시브 스킬 초기화
        var tableData = TableManager.Instance.PassiveSkill2.dataArray;

        int passiveSkillPoint = 0;

        for (int i = 0; i < tableData.Length; i++)
        {
            passiveSkillPoint += ServerData.passive2ServerTable.TableDatas[tableData[i].Stringid].level.Value;
            ServerData.passive2ServerTable.TableDatas[tableData[i].Stringid].level.Value = 0;
        }

        //리셋한 데이터 적용
        ServerData.statusTable.GetTableData(StatusTable.Skill2Point).Value += passiveSkillPoint;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        statusParam.Add(StatusTable.Skill2Point, ServerData.statusTable.GetTableData(StatusTable.Skill2Point).Value);

        Param passiveSkillParam = new Param();
        var passiveTableData = TableManager.Instance.PassiveSkill2.dataArray;

        for (int i = 0; i < passiveTableData.Length; i++)
        {
            passiveSkillParam.Add(passiveTableData[i].Stringid, ServerData.passive2ServerTable.TableDatas[passiveTableData[i].Stringid].ConvertToString());
        }

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactionList.Add(TransactionValue.SetUpdate(Passive2ServerTable.tableName, Passive2ServerTable.Indate, passiveSkillParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "포인트 초기화 성공!", null);
        });
    }

}
