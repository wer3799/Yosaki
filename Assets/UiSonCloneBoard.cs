using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UniRx;
using static GameManager;
using UnityEngine.UI;

public class UiSonCloneBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI clearAmount;

    [SerializeField]
    private Button enterButton;

    [SerializeField] private GameObject transObject;
    [SerializeField] private GameObject transAfterObject;
    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.sonCloneClear].AsObservable().Subscribe(e =>
        {
            int addAmount = (int)(e * GameBalance.sonCloneAddValue);

            description.SetText($"{CommonString.GetItemName(Item_Type.PeachReal)} + {addAmount}개 추가 적용됨");

            clearAmount.SetText($"{Utils.ConvertBigNum(e)}");

            bool isTrans = GameBalance.sonCloneGraduateAfterScore <= e;
            
            transObject.SetActive(!isTrans);
            transAfterObject.SetActive(isTrans);

        }).AddTo(this);

    }

    public void OnClickEnterCloneButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.SonClone);
            enterButton.interactable = false;
        }, () => { });
    }
    
     public void OnClickTransButton()
    {
        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.sonCloneClear].Value;
        if (score < GameBalance.sonCloneGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {GameBalance.sonCloneGraduateScore} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"손오공 분신을 각성하려면 {Utils.ConvertNum(GameBalance.sonCloneGraduateScore)} 이상 이어야 합니다.\n" +
                $"각성 시 점수가 {Utils.ConvertNum(GameBalance.sonCloneGraduateAfterScore)}점으로 고정됩니다.\n" +
                $"각성하시겠습니까?", () =>
                {

                    ServerData.userInfoTable.TableDatas[UserInfoTable.sonCloneClear].Value = GameBalance.sonCloneGraduateAfterScore;

                    List<TransactionValue> transactions = new List<TransactionValue>();
            
                    Param userinfoParam = new Param();
                    userinfoParam.Add(UserInfoTable.sonCloneClear,ServerData.userInfoTable.GetTableData(UserInfoTable.sonCloneClear).Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));
            
                    ServerData.SendTransactionV2(transactions, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
                    });
                }, null);
        }
    }
}
