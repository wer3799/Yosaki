using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class FoxMaskBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private UiMaskView uiMaskView;

    [SerializeField]
    private TextMeshProUGUI currentFloor;
    [SerializeField]
    private TextMeshProUGUI transAfterDesc;

    [SerializeField] private GameObject transBefore;
    [SerializeField] private GameObject transAfter;
    
    private bool initialized = false;
    
        
    [SerializeField]
    private Toggle towerAutoMode;

    public void Start()
    {

        Initialize();

        Subscribe();

    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.foxMask].AsObservable().Subscribe(e =>
        {
            currentFloor.SetText($"{e + 1}단계 입장");
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateGhostTree).AsObservable().Subscribe(e =>
        {
            transBefore.SetActive(e<1);
            transAfter.SetActive(e > 0);
        }).AddTo(this);
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.FoxMask.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiMaskView>(uiMaskView, cellParent);

            cell.Initialize(tableData[i]);
        }

        transAfterDesc.SetText($"각성 효과로 강화됩니다.\n귀신 나무 능력치 {(GameBalance.GhostTreeGraduatePlusValue - 1) * 100}% 증가");
        
        if (PlayerPrefs.HasKey(SettingKey.towerAutoMode) == false)
            PlayerPrefs.SetInt(SettingKey.towerAutoMode, 1);     
        
        towerAutoMode.isOn = PlayerPrefs.GetInt(SettingKey.towerAutoMode) == 1;
        
        initialized = true;
    }

    public void OnClickEnterButton()
    {
        int currentIdx = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.foxMask].Value;

        if (currentIdx >= TableManager.Instance.FoxMask.dataArray.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("도전 완료!");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{currentIdx + 1}단계\n도전 할까요?", () =>
          {
              GameManager.Instance.LoadContents(GameManager.ContentsType.FoxMask);

          }, null);
    }

    public void OnClickUpEquipButton()
    {
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.FoxMaskView, -1);
    }
    
    
    public static string bossKey = "b69";
    public void OnClickTransButton()
    {
        
        if (double.Parse(ServerData.bossServerTable.TableDatas[bossKey].score.Value) < GameBalance.GhostTreeGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"최고 점수 {Utils.ConvertBigNumForRewardCell(GameBalance.GhostTreeGraduateScore)} 이상일때 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"귀신나무를 각성하려면 점수가 {Utils.ConvertBigNumForRewardCell(GameBalance.GhostTreeGraduateScore)}이상 이어야 합니다. \n" +
                $"각성시 나무조각 효과가 {(GameBalance.GhostTreeGraduatePlusValue-1)*100f}% 강화 됩니다.\n" +
                "각성 하시겠습니까??", () =>
                {
                    ServerData.userInfoTable.TableDatas[UserInfoTable.graduateGhostTree].Value = 1;
                    
                    List<TransactionValue> transactions = new List<TransactionValue>();
                    
                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.graduateGhostTree, ServerData.userInfoTable.TableDatas[UserInfoTable.graduateGhostTree].Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName,UserInfoTable.Indate,userInfoParam));
                    
                    ServerData.SendTransaction(transactions,successCallBack: () =>
                    {
                        Initialize();
                    });
                    
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
              
                }, null);
        }
    }
    
    public void AutoModeOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.towerAutoMode.Value = on ? 1 : 0;
    }

}
