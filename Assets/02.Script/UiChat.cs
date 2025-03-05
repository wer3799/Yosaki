using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using static ChatManager;

public class UiChat : MonoBehaviour
{
    [SerializeField]
    private List<UiMessageText> messagePool;

    [SerializeField]
    private TMP_InputField inputfield;

    private int currentIdx = 0;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
       // ChatManager.Instance.whenChatReceived.Subscribe(e => { SetMessage(e, false); }).AddTo(this);

   //     ChatManager.Instance.LoadNormalChatHistory();
    }

    private void Initialize()
    {
        messagePool.ForEach(e => e.Initialize(string.Empty, true));

        // if (ChatManager.Instance.chatConnected)
        // {
        //     SetMessage(new ChatInfo(CommonString.ChatConnectString), true);
        // }
    }

    private void SetMessage(ChatInfo message, bool isSystem)
    {
        messagePool[currentIdx].Initialize(message.message, isSystem, message.frameId);
        messagePool[currentIdx].transform.SetAsFirstSibling();

        currentIdx++;
        if (currentIdx == messagePool.Count)
        {
            currentIdx = 0;
        }
    }

    public void SendChat(string chat)
    {
        if (chat.Length > 40)
        {
            chat = chat.Substring(0, 40);
        }
       // ChatManager.Instance.SendChat(chat);
        inputfield.text = string.Empty;
    }
}
