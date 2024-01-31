using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UniRx;

public class UiSmithWoodBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI clearAmount;

    [SerializeField] private GameObject transObject;
    [SerializeField] private GameObject transAfterObject;
    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].AsObservable().Subscribe(e =>
        {
            int addAmount = (int)(e * GameBalance.smithTreeAddValue);

            description.SetText($"{CommonString.GetItemName(Item_Type.SmithFire)} + {addAmount}개 추가 적용됨");

            clearAmount.SetText($"{Utils.ConvertBigNum(e)}");

            bool isTrans = GameBalance.smithTreeGraduateAfterScore<= e;
            
            transObject.SetActive(!isTrans);
            transAfterObject.SetActive(isTrans);

        }).AddTo(this);

    }
    public void OnClickTransButton()
    {
        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].Value;
        if (score < GameBalance.smithTreeGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {GameBalance.smithTreeGraduateScore} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"손오공 분신을 각성하려면 {Utils.ConvertNum(GameBalance.smithTreeGraduateScore)} 이상 이어야 합니다.\n" +
                $"각성 시 점수가 {Utils.ConvertNum(GameBalance.smithTreeGraduateAfterScore)}점으로 고정됩니다.\n" +
                $"각성하시겠습니까?", () =>
                {

                    ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].Value = GameBalance.smithTreeGraduateAfterScore;

                    List<TransactionValue> transactions = new List<TransactionValue>();
            
                    Param userinfoParam = new Param();
                    userinfoParam.Add(UserInfoTable.smithTreeClear,ServerData.userInfoTable.GetTableData(UserInfoTable.smithTreeClear).Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));
            
                    ServerData.SendTransactionV2(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
                    });
                }, null);
        }
    }
}
