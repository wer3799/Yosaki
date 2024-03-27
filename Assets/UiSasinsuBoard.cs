using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiSasinsuBoard : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI sasinsuDescription;

    [SerializeField]
    private WeaponView weaponView;
    [SerializeField]
    private UiSasinsuRewardBoard sasinsuRewardBoard;

    [SerializeField]
    private UiSasinsuDescription star0;
    [SerializeField]
    private UiSasinsuDescription star1;
    [SerializeField]
    private UiSasinsuDescription star2;
    [SerializeField]
    private UiSasinsuDescription star3;
    [SerializeField]
    private UiSasinsuDescription star4;
    [SerializeField]
    private UiSasinsuDescription star5;
    [SerializeField]
    private UiSasinsuDescription star6;

    [SerializeField]
    private TextMeshProUGUI totalAbilAddValue;

    [SerializeField]
    private int sasinIndex = 0;

    [SerializeField]
    private GameObject sasinsuObject;

    [SerializeField]
    private GameObject goldDragonObject;
    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        UpdateToalAbilAddValue();
    }

    private void UpdateToalAbilAddValue()
    {
        totalAbilAddValue.SetText($"총 :{Utils.ConvertNum(PlayerStats.GetSasinsuStarAddValue() * 100f)}% 적용됨");
    }
    //

    private void Initialize()
    {
        star0.Initialize(0, sasinIndex);
        star1.Initialize(1, sasinIndex);
        star2.Initialize(2, sasinIndex);
        star3.Initialize(3, sasinIndex);
        star4.Initialize(4, sasinIndex);
        star5.Initialize(5, sasinIndex);
        star6.Initialize(6, sasinIndex);

        SasinScoreCheck();
        ShowDescription();
    }

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.SetBossId(sasinIndex);
            GameManager.Instance.LoadContents(GameManager.ContentsType.Sasinsu);
        }, () => { });
    }
    public void OnClickSasinsuButton(int select)

    {
        if (select == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("업데이트 예정 입니다!");
            return;
        }

        sasinsuObject.SetActive(select < 4);
        goldDragonObject.SetActive(select >= 4);

        if (select < 4) 
        {
            sasinIndex = select;
            sasinsuRewardBoard.SetSasinIdx(sasinIndex);
            Initialize();
        }
    }
    public void OnClickRewardButton()
    {
        sasinsuRewardBoard.SetSasinIdx(sasinIndex);
    }

    private void SasinScoreCheck()
    {
        int check = -1;
        for (int i = TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score.Length - 1; i >= 0; i--)
        {
            if (ServerData.sasinsuServerTable.TableDatas[$"b{sasinIndex}"].score.Value >= TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[i])
            {
                check = i;
                break;
            }
        }
        switch (check)
        {
            case 0:
                star0.OnClickButton();
                break;
            case 1:
                star1.OnClickButton();
                break;

            case 2:
                star2.OnClickButton();
                break;

            case 3:
                star3.OnClickButton();
                break;

            case 4:
                star4.OnClickButton();
                break;

            case 5:
                star5.OnClickButton();
                break;
            case 6:
                star6.OnClickButton();
                break;
            default:
                star0.OnClickButton();
                break;
        }
    }

    private void ShowDescription()
    {
        switch (sasinIndex)
        {
            case 0:
                scoreText.SetText($"최고 점수 : {Utils.ConvertNum(ServerData.sasinsuServerTable.TableDatas["b0"].score.Value)}");
                sasinsuDescription.SetText($"보스를 공략해 점수를 등록하세요!\n단계별 효과가 누적되어 적용 됩니다!");
                weaponView.Initialize(TableManager.Instance.WeaponData[67], null);
                break;
            case 1:
                scoreText.SetText($"최고 점수 : {Utils.ConvertNum(ServerData.sasinsuServerTable.TableDatas["b1"].score.Value)}");
                sasinsuDescription.SetText($"보스를 공략해 점수를 등록하세요!\n단계별 효과가 누적되어 적용 됩니다!");
                weaponView.Initialize(TableManager.Instance.WeaponData[68], null);
                break;
            case 2:
                scoreText.SetText($"최고 점수 : {Utils.ConvertNum(ServerData.sasinsuServerTable.TableDatas["b2"].score.Value)}");
                sasinsuDescription.SetText($"보스를 공략해 점수를 등록하세요!\n단계별 효과가 누적되어 적용 됩니다!");
                weaponView.Initialize(TableManager.Instance.WeaponData[69], null);
                break;
            case 3:
                scoreText.SetText($"최고 점수 : {Utils.ConvertNum(ServerData.sasinsuServerTable.TableDatas["b3"].score.Value)}");
                sasinsuDescription.SetText($"보스를 공략해 점수를 등록하세요!\n단계별 효과가 누적되어 적용 됩니다!");
                weaponView.Initialize(TableManager.Instance.WeaponData[70], null);
                break;
        }
        //
    }
}
