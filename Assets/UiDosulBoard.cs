using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UiDosulBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;


    [SerializeField]
    private TextMeshProUGUI currentDosulLevel;

    [SerializeField]
    private TextMeshProUGUI currentDosulAddValue;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice;

    [SerializeField]
    private Image levelUpButton;

    [SerializeField]
    private Sprite normalSprite;

    [SerializeField]
    private Sprite maxLevelSprite;

    [SerializeField] private GameObject dosulPopup;

    private void Start()
    {
        Initialize();

        Subscribe();
    }

    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 297)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertStage(299)} 스테이지 달성시 개방!");
            dosulPopup.SetActive(false);
            return;
        }
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].AsObservable().Subscribe(e =>
        {
            bool isMaxLevel = IsMaxLevel();

            if (e == -1)
            {
                currentDosulLevel.SetText($"해방되지 않음");
                currentDosulAddValue.SetText($"도술 추가 피해량\n{0}% 증가");
            }
            else
            {
                currentDosulLevel.SetText($"LV : {(int)e + 1}");

                currentDosulAddValue.SetText($"도술 추가 피해량\n{Utils.ConvertBigNum(TableManager.Instance.dosulTable.dataArray[(int)e].Abil_Value * 100f)}% 증가");

                if (isMaxLevel)
                {
                    levelUpPrice.SetText("최고단계");
                }
                else
                {
                    levelUpPrice.SetText(Utils.ConvertNum(GetCurrentDosulLevelUpPrice()));
                }
            }


            levelUpButton.sprite = isMaxLevel ? maxLevelSprite : normalSprite;
        }).AddTo(this);
    }

    private float GetCurrentDosulLevelUpPrice()
    {
        //-1부터 시작
        int currentDosulLevel = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].Value;

        return TableManager.Instance.dosulTable.dataArray[currentDosulLevel + 1].Conditoin_Value;
    }

    private bool IsMaxLevel()
    {
        int currentDosulLevel = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].Value;

        return currentDosulLevel >= TableManager.Instance.dosulTable.dataArray.Length - 1;
    }

    public void OnClickLevelUpButton()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage($"최고레벨 입니다!");
            return;
        }

        float levelUpPrice = GetCurrentDosulLevelUpPrice();

        if (ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value < levelUpPrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DosulGoods)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value -= levelUpPrice;

        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].Value++;

        int currentDosulLevel = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].Value;

        PopupManager.Instance.ShowAlarmMessage($"도술 레벨이 올랐습니다!");

        if (TableManager.Instance.dosulTable.dataArray[currentDosulLevel].Unlock_Skill_Id != 0)
        {
            PopupManager.Instance.ShowConfirmPopup("알림", "신규 도술이 해금됐습니다!", null);
        }

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private Coroutine syncRoutine;

    private WaitForSeconds delay = new WaitForSeconds(0.5f);

    private IEnumerator SyncRoutine()
    {
        yield return delay;
        //
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.DosulGoods, ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable_2.dosulLevel, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].Value);
        //

        List<TransactionValue> transactions = new List<TransactionValue>();
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () => { LogManager.Instance.SendLogType("Dosul", "LevelUp", $"{ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].Value},{ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value}"); });
    }


    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulScore].Value * GameBalance.BossScoreConvertToOrigin)}");


    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.DosulBoss); }, () => { });
    }
}