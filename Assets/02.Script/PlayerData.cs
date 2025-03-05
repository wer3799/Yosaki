using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerData : SingletonMono<PlayerData>
{
    public string NickName { get; private set; }
    public string Indate { get; private set; }

    public ReactiveCommand<string> whenNickNameChanged = new ReactiveCommand<string>();

    [SerializeField]
    private UiNickNameInputBoard uiNickNameInputBoard;

#if UNITY_IOS
    public bool HasIOSFlag { get; private set; } = false;
#endif

    public void NickNameChanged(string nickName)
    {
        NickName = nickName;
        whenNickNameChanged.Execute(nickName);
    }

    public void LoadUserNickName()
    {
        Debug.Log("IOS_5");
        Backend.BMember.GetUserInfo(WhenUserInfoLoadComplete);
    }

    private void WhenUserInfoLoadComplete(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            var returnData = bro.GetReturnValuetoJSON();
            JsonData row = returnData["row"];

            if (row.Keys.Contains("inDate"))
            {
                Indate = row["inDate"].ToString();
            }

            if (row.Keys.Contains("nickname"))
            {
                if (row["nickname"] != null)
                {
#if UNITY_ANDROID
                    NickName = row["nickname"].ToString();
#endif
#if UNITY_IOS
                    NickName = row["nickname"].ToString().Replace(CommonString.IOS_nick, "");
#endif
                    WhenUserInfoLoadComplete();
                }
                else
                {
                    uiNickNameInputBoard.gameObject.SetActive(true);
                }
            }
            else
            {
                uiNickNameInputBoard.gameObject.SetActive(true);
            }
        }
        else
        {
            
        }
    }

    private void WhenUserInfoLoadComplete()
    {
        Debug.Log("IOS_9");
        PreSceneStartButton.Instance.SetInteractive();
      //  ChatManager.Instance.ConnectToChattingServer();
        Subscribe();

        SaveManager.Instance.StartAutoSave();
        GameManager.Instance.Initialize();
        PushManager.Instance.Initialize();
        Debug.Log("IOS_10");

    }

    private void Subscribe()
    {
      //  RankManager.Instance.Subscribe();
    }
}
