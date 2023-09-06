using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class UiSkillPointResetButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceText;

    private void Start()
    {
        SetPriceText();
    }

    private void SetPriceText()
    {
        priceText.SetText(Utils.ConvertBigNum(GameBalance.SkillPointResetPrice));
    }

    public void OnClickResetSkillPoint()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value < GameBalance.SkillPointResetPrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Jade)} {Utils.ConvertBigNum(GameBalance.SkillPointResetPrice)}개를 사용해서\n모든 기술을 초기화 하시겠습니까?", () =>
          {
              ResetSkillAll();
          }, null);

    }

    public void OnClickResetPassiveSkillPoint()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value < GameBalance.SkillPointResetPrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Jade)} {Utils.ConvertBigNum(GameBalance.SkillPointResetPrice)}개를 사용해서\n패시브 기술을 초기화 하시겠습니까?\n<color=red>주의 : 환수 패시브도 초기화 됩니다.</color>\n<color=red>주의 : 환수 패시브도 초기화 됩니다.</color>\n<color=red>주의 : 환수 패시브도 초기화 됩니다.</color>", () =>
        {
            ResetPassiveOnly();
        }, null);

    }

    private void ResetPassiveOnly() 
    {
        //패시브 스킬 초기화
        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        int passiveSkillPoint = 0;

        for (int i = 0; i < tableData.Length; i++)
        {
            passiveSkillPoint += ServerData.passiveServerTable.TableDatas[tableData[i].Stringid].level.Value;
            ServerData.passiveServerTable.TableDatas[tableData[i].Stringid].level.Value = 0;
        }

        //리셋한 데이터 적용
        ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value += passiveSkillPoint;

        //비용 차감
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.SkillPointResetPrice;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.SkillPoint, ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value);

        Param passiveSkillParam = new Param();
        var passiveTableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < passiveTableData.Length; i++)
        {
            passiveSkillParam.Add(passiveTableData[i].Stringid, ServerData.passiveServerTable.TableDatas[passiveTableData[i].Stringid].ConvertToString());
        }

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactionList.Add(TransactionValue.SetUpdate(PassiveServerTable.tableName, PassiveServerTable.Indate, passiveSkillParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "패시브 기술 초기화 성공!", null);
        });
    }

    private void ResetSkillAll()
    {
        //패시브 스킬 초기화
        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            ServerData.passiveServerTable.TableDatas[tableData[i].Stringid].level.Value = 0;
        }
        //

        var skillLevelList = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel];

        for (int i = 0; i < skillLevelList.Count; i++)
        {
            //로컬 데이터 초기화
            skillLevelList[i].Value = 0;
        }

        var skillSlotList = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillSlotIdx_0];

        for (int i = 0; i < skillSlotList.Count; i++)
        {
            skillSlotList[i].Value = -1;
        }

        for (int i = 0; i < GameBalance.skillSlotGroupNum; i++)
        {
            ServerData.skillServerTable.UpdateSelectedSkillIdx(ServerData.skillServerTable.TableDatas[SkillServerTable.GetSkillGroupKey(i)].Select(e => e.Value).ToList(), i);
        }

        var currentLevel = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        //리셋한 데이터 적용
        ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value = currentLevel * GameBalance.SkillPointGet;

        //비용 차감
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.SkillPointResetPrice;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param skillParam = new Param();
        skillParam.Add(SkillServerTable.SkillLevel, ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel].Select(e => e.Value).ToList());
        skillParam.Add(SkillServerTable.SkillSlotIdx_0, ServerData.skillServerTable.TableDatas[SkillServerTable.SkillSlotIdx_0].Select(e => e.Value).ToList());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

        Param statusParam = new Param();
        statusParam.Add(StatusTable.SkillPoint, ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value);

        Param passiveSkillParam = new Param();
        var passiveTableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < passiveTableData.Length; i++)
        {
            passiveSkillParam.Add(passiveTableData[i].Stringid, ServerData.passiveServerTable.TableDatas[passiveTableData[i].Stringid].ConvertToString());
        }

        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactionList.Add(TransactionValue.SetUpdate(PassiveServerTable.tableName, PassiveServerTable.Indate, passiveSkillParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "기술포인트 초기화 성공!", null);
        });
    }
}
