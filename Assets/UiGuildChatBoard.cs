using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ChatManager;
using UniRx;
using BackEnd.Tcp;

public class UiGuildChatBoard : SingletonMono<UiGuildChatBoard>
{
    [SerializeField]
    private TextMeshProUGUI guildDescription;

    [SerializeField]
    private TextMeshProUGUI chatText;

    [SerializeField]
    private TMP_InputField inputfield;

    private List<ChatInfo> inputChats = null;

    [SerializeField]
    private GameObject connectText;


    private int chatMax = 30;

    private bool subscribed = false;

    private IEnumerator GuildChatInitialize()
    {
        connectText.gameObject.SetActive(true);

        inputChats = new List<ChatInfo>();

        chatText.gameObject.SetActive(false);

        Backend.Chat.LeaveChannel(ChannelType.Guild);

        yield return new WaitForSeconds(1.0f);

    //    ChatManager.Instance.ConnectToChattingServer_Guild();

        if (subscribed == false)
        {
            Subscribe();
            subscribed = true;
        }

        yield return new WaitForSeconds(1.0f);

        chatText.gameObject.SetActive(true);

        connectText.gameObject.SetActive(false);

        GetMessage(new ChatInfo("문파 채팅에 입장 했습니다."), true);
    }

    private void Subscribe()
    {
  //      ChatManager.Instance.whenChatReceived_Guild.Subscribe(e => { GetMessage(e, false); }).AddTo(this);
    }

    private void GetMessage(ChatInfo message, bool isSystem)
    {
        inputChats.Add(message);

        if (inputChats.Count >= chatMax)
        {
            inputChats.RemoveAt(0);
        }

        string allChats = string.Empty;


        for (int i = 0; i < inputChats.Count; i++)
        {
            if (i != inputChats.Count - 1)
            {
                allChats += $"{inputChats[i].message}\n";
            }
            else
            {
                allChats += $"{inputChats[i].message}";
            }
        }

        chatText.SetText(allChats);

    }

    public void SendChat(string chat)
    {
        if (chat.Length > 40)
        {
            chat = chat.Substring(0, 40);
        }
    //    ChatManager.Instance.SendChat_Guild(chat);
        inputfield.text = string.Empty;
    }

    public void SendRankScore(string chat)
    {
        if (chat.Length > 40)
        {
            chat = chat.Substring(0, 40);
        }
     //   ChatManager.Instance.SendChat_Guild(chat, false);
        inputfield.text = string.Empty;
    }

    public void SendRankScore_System(string chat)
    {
    //    ChatManager.Instance.SendChat_Guild(chat, false);
        inputfield.text = string.Empty;
    }

#if UNITY_EDITOR
    int count = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var ChatInfo = new ChatInfo(count.ToString());
            count++;

            GetMessage(ChatInfo, false);
        }
    }
#endif


    #region GuildDescription

    private void OnEnable()
    {
        if (GuildManager.Instance.guildInfoData.Value != null)
        {
            string desc = GuildManager.Instance.guildInfoData.Value["guildDesc"]["S"].ToString();
            guildDescription.SetText(desc);
        }

        StartCoroutine(GuildChatInitialize());
    }

    public void OnGuildDescriptionEditEnd(string desc)
    {
        if (string.IsNullOrEmpty(desc))
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "텍스트를 입력해 주세요", null);
            return;
        }

        Param param = new Param();

        param.Add("guildDesc", desc);

        var bro = Backend.Social.Guild.ModifyGuildV3(param);

        if (bro.IsSuccess())
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "소개글 등록 성공!", null);
            guildDescription.SetText(desc);
        }
        else
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"소개글 등록에 실패했습니다.\n({bro.GetStatusCode()})", null);
        }

    }
    #endregion
}
