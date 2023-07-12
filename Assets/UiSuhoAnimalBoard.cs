using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;

public class UiSuhoAnimalBoard : SingletonMono<UiSuhoAnimalBoard>
{
    [SerializeField]
    private UiSuhoPetButton uiSuhoPetButton;

    [SerializeField]
    private Transform buttonParents;

    [SerializeField]
    private TMP_InputField instantClearNum;

    public ReactiveProperty<int> currentSelectedIdx = new ReactiveProperty<int>();
    public void Start()
    {
        Inisitlize();

        SetStage();
    }

    private void Inisitlize()
    {
        var tableDatas = TableManager.Instance.suhoPetTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].SUHOPETTYPE == SuhoPetType.Basic)
            {
                var button = Instantiate<UiSuhoPetButton>(uiSuhoPetButton, buttonParents);
                button.Initialize(tableDatas[i]);
            }
        }
    }


    private void SetStage()
    {
        var lastId = 0;
        if (GameManager.Instance.lastContentsType != GameManager.ContentsType.NormalField)
        {
            lastId = GameManager.Instance.suhoAnimalId;
            currentSelectedIdx.Value = lastId;
            UiSuhoAnimalRewardPopup.Instance.Initialize(lastId);
            return;
        }
        lastId = ServerData.suhoAnimalServerTable.GetLastPetId();
        
        UiSuhoAnimalRewardPopup.Instance.Initialize(lastId);

        if (lastId == -1)
        {
            lastId = 0;
        }
        
        currentSelectedIdx.Value = lastId;
    }

    public void OnClickInstantClearButton()
    {
        int lastPetId = ServerData.suhoAnimalServerTable.GetLastPetId();

        //플레이 X
        if (lastPetId == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("수호동물을 보유하고 있어야 소탕하실 수 있습니다!");
            return;
        }


        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 없습니다.");
            return;
        }

        if (int.TryParse(instantClearNum.text, out var inputNum))
        {
            if (inputNum == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                return;
            }
            else if (remainItemNum < inputNum)
            {
                PopupManager.Instance.ShowAlarmMessage(
                    $"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }

        var instanClearGetNum = TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue * inputNum;

        string desc= "";
        if (PlayerStats.GetSuhoGainValue() > 0f)
        {
            desc +=
                $"{lastPetId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.SuhoPetFeed)} {instanClearGetNum}(+{instanClearGetNum*PlayerStats.GetSuhoGainValue()})개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({lastPetId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.SuhoPetFeed)} {(int)TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue}(+{(int)TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue*PlayerStats.GetSuhoGainValue()})개 획득)</color>";
        }
        else
        {
            desc +=
                $"{lastPetId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.SuhoPetFeed)} {instanClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({lastPetId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.SuhoPetFeed)} {(int)TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue}개 획득)</color>";
        }
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, desc, () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 없습니다.");

                    return;
                }

                if (int.TryParse(instantClearNum.text, out var inputNum))
                {
                    if (inputNum == 0)
                    {
                        PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                        return;
                    }
                    else if (remainItemNum < inputNum)
                    {
                        PopupManager.Instance.ShowAlarmMessage(
                            $"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeed].Value +=
                    instanClearGetNum + (instanClearGetNum * PlayerStats.GetSuhoGainValue());

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.SuhoPetFeed, ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeed].Value);
                goodsParam.Add(GoodsTable.SuhoPetFeedClear,
                    ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.SuhoPetFeed)} {instanClearGetNum+(instanClearGetNum * PlayerStats.GetSuhoGainValue())}개 획득!", null);
                    });
            }, null);
    }

    private void OnDisable()
    {
        PlayerStats.ResetSuperCritical11CalculatedValue();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value += 2;
        }
        
    }
#endif
}