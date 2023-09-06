using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildMakeBoard : MonoBehaviour
{
    [SerializeField]
    private Button createButton;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        priceText.SetText($"{Utils.ConvertBigNum(GameBalance.GuildMakePrice)}");
    }

    private bool CanMakeNickName()
    {

        bool isRightRangeChar = Regex.IsMatch(inputField.text, "^[가-힣]*$");
        bool hasBadWorld = Utils.HasBadWord(inputField.text);
        return isRightRangeChar && hasBadWorld == false;

    }


    public void OnClickCreateButton()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            PopupManager.Instance.ShowAlarmMessage($"문파 이름을 입력 해주세요!");
            return;
        }

#if !UNITY_EDITOR
        float currentLevel = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        if (currentLevel < GameBalance.GuildCreateMinLevel)
        {
            PopupManager.Instance.ShowAlarmMessage($"문파 생성은 {GameBalance.GuildCreateMinLevel}부터 가능 합니다.");
            return;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value < GameBalance.GuildMakePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"옥이 부족 합니다\n{Utils.ConvertBigNum(GameBalance.GuildMakePrice)}개 필요");
            return;
        }
#endif

        if (CanMakeNickName() == false)
        {
            PopupManager.Instance.ShowConfirmPopup("알림", "한글만 입력 가능 합니다! (8자 이내,금지 단어 포함X)", null);
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"문파를 생성 합니까?\n옥 {Utils.ConvertBigNum(GameBalance.GuildMakePrice)}개 필요", () =>
        {
            string guildName = inputField.text;

            // 메타정보가 있는 경우 
            Param param = new Param();
            param.Add("guildIcon", 0); //길드아이콘
            param.Add("guildDesc", "아직 소개가 없습니다."); //길드소개글
#if UNITY_ANDROID
            param.Add("isAnd", 1);
#endif
#if UNITY_IOS
            param.Add("isAnd", 0);
#endif
            param.Add("param4", 0);
            param.Add("param5", 0);
            param.Add("param6", 0);

            createButton.interactable = false;

            var bro = Backend.Social.Guild.CreateGuildV3(guildName, 10, param);

            createButton.interactable = true;
            // 이후 처리
            if (bro.IsSuccess())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "문파 생성 성공!", null);

                GuildManager.Instance.LoadGuildInfo();

                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.GuildMakePrice;
                ServerData.goodsTable.UpData(GoodsTable.Jade, false);
            }
            else
            {
                switch (bro.GetStatusCode())
                {
                    case "409":
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "이미 존재하는 문파명 입니다.", null);
                        }
                        break;
                    case "412":
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "이미 문파에 가입되어 있습니다.", null);
                        }
                        break;
                    default:
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"생성 오류\n{bro.GetStatusCode()}", null);
                        }
                        break;
                }
            }

        }, null);


    }
}
