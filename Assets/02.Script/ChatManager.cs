using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ChatManager : SingletonMono<ChatManager>
{
    private string chatGroupName_Guild = "Guild";
    //#if UNITY_ANDROID
    private string chatGroupName = "Normal";
    //#endif
    //#if UNITY_IOS
    //    private string chatGroupName = "Normal_IOS";
    //#endif

    ChannelType channelType = ChannelType.Public;

    public RecentChatLogsEventArgs args { get; private set; }

    private int retryCountMax = 3;
    private int retryCount = 0;

    private int retryCountMax_Guild = 3;
    private int retryCount_Guild = 0;

    WaitForSeconds retryDelay = new WaitForSeconds(3.0f);

    public ReactiveCommand<string> WhenBanned = new ReactiveCommand<string>();

    public class ChatInfo
    {
        public string message;
        public int frameId = 0;

        public ChatInfo(string message)
        {
            this.message = message;
        }
        public ChatInfo(string message, int frameId)
        {
            this.message = message;
            this.frameId = frameId;
        }
    }

    public ReactiveCommand<ChatInfo> whenChatReceived = new ReactiveCommand<ChatInfo>();
    public ReactiveCommand<ChatInfo> whenChatReceived_Guild = new ReactiveCommand<ChatInfo>();

    private Coroutine aliveRoutine;

    private string serverAddress_str = "serverAddress";
    private string serverPort_str = "serverPort";
    private string inDate_str = "inDate";

    public bool chatConnected { get; private set; }
    public bool chatConnected_Guild { get; private set; }

    public void ConnectToChattingServer()
    {
        LinkChatCallBacks();

        var bro = Backend.Chat.GetGroupChannelList(chatGroupName);

        for (int i = 0; i < bro.Rows().Count; i++)
        {
            JsonData channel = bro.Rows()[i];

            int maxUserNum = int.Parse(channel["maxUserCount"].ToString());
            int joinedUserCount = int.Parse(channel["joinedUserCount"].ToString());
            if (joinedUserCount >= maxUserNum)
            {
                continue;
            }

            string address = channel[serverAddress_str].ToString();
            var serverPort = (ushort.Parse(channel[serverPort_str].ToString()));
            string inDate = channel[inDate_str].ToString();
            string groupName = chatGroupName;

            ErrorInfo errorInfo;
            bool join = Backend.Chat.JoinChannel(ChannelType.Public, address, serverPort, groupName, inDate, out errorInfo);

            if (join)
            {
                retryCount = 0;

                //비속어 필터링
                whenChatReceived.Execute(new ChatInfo(CommonString.ChatConnectString));
                Debug.LogError("채팅  JoinChannel success");
                chatConnected = true;
                Backend.Chat.SetFilterUse(true);
                Backend.Chat.OnChat = OnChatReceived;
            }
            else
            {
                Debug.LogError("채팅  JoinChannel failed");
                chatConnected = false;

                if (errorInfo != null)
                {
                    RetryConnect();
                }
            }

            return;
        }
    }


    public void ConnectToChattingServer_Guild()
    {
        LinkChatCallBacks_Guild();

        var bro = Backend.Chat.GetGroupChannelList(chatGroupName_Guild);

        for (int i = 0; i < bro.Rows().Count; i++)
        {
            JsonData channel = bro.Rows()[i];

            int maxUserNum = int.Parse(channel["maxUserCount"].ToString());
            int joinedUserCount = int.Parse(channel["joinedUserCount"].ToString());
            if (joinedUserCount >= maxUserNum)
            {
                continue;
            }

            string address = channel[serverAddress_str].ToString();
            var serverPort = (ushort.Parse(channel[serverPort_str].ToString()));
            string inDate = channel[inDate_str].ToString();
            string groupName = chatGroupName_Guild;

            ErrorInfo errorInfo;
            bool join = Backend.Chat.JoinChannel(ChannelType.Guild, address, serverPort, groupName, inDate, out errorInfo);

            if (join)
            {
                retryCount_Guild = 0;

                //비속어 필터링
                // whenChatReceived_Guild.Execute(new ChatInfo(CommonString.ChatConnectString));
                Debug.LogError("채팅  JoinChannel success");
                chatConnected_Guild = true;
                Backend.Chat.SetFilterUse(true);
                Backend.Chat.OnGuildChat = OnChatReceived_Guild;
            }
            else
            {
                Debug.LogError("채팅  JoinChannel failed");
                chatConnected_Guild = false;

                if (errorInfo != null)
                {
                    RetryConnect_Guild();
                }
            }

            return;
        }
    }

    private void LinkChatCallBacks()
    {
        Backend.Chat.OnJoinChannel = OnJoinChannel;

        Backend.Chat.OnRecentChatLogs = RecentChatEvent;
    }

    private void LinkChatCallBacks_Guild()
    {
        Backend.Chat.OnJoinGuildChannel = OnJoinChannel_Guild;

        Backend.Chat.OnRecentChatLogs = RecentChatEvent;
    }

    private void RecentChatEvent(RecentChatLogsEventArgs args)
    {
        if (args.channelType == ChannelType.Public)
        {
            this.args = args;

        }
        else if (args.channelType == ChannelType.Guild)
        {
            for (int i = args.LogInfos.Count - 1; i >= 0; i--)
            {
                if (args.LogInfos[i].NickName.Equals(PlayerData.Instance.NickName))
                {
                    var split = args.LogInfos[i].Message.Split(CommonString.ChatSplitChar);

                    if (split.Length == 2)
                    {
                        whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]} 나:{split[1]}"));
                    }
                    else if (split.Length == 1)
                    {
                        whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]}"));
                    }
                }
                // 다른 유저의 메시지일 경우
                else
                {
                    var split = args.LogInfos[i].Message.Split(CommonString.ChatSplitChar);

                    if (split.Length == 2)
                    {
                        whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]} {args.LogInfos[i].NickName}:{split[1]}"));
                    }
                    else if (split.Length == 1)
                    {
                        whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]}"));
                    }
                }
            }
        }

    }

    public void LoadNormalChatHistory()
    {
        if (args == null) return;

        for (int i = args.LogInfos.Count - 1; i >= 0; i--)
        {

            if (args.LogInfos[i].NickName.Equals(PlayerData.Instance.NickName))
            {
                var split = args.LogInfos[i].Message.Split(CommonString.ChatSplitChar);

                if (split.Length >= 4)
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} 나:{split[2]}", int.Parse(split[3])));
                }
                else
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} 나:{split[2]}"));
                }
            }
            // 다른 유저의 메시지일 경우
            else
            {
                var split = args.LogInfos[i].Message.Split(CommonString.ChatSplitChar);

                if (split.Length >= 4)
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} {args.LogInfos[i].NickName}:{split[2]}", int.Parse(split[3])));
                }
                else
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} {args.LogInfos[i].NickName}:{split[2]}"));
                }
            }
        }
    }

    private void RetryConnect()
    {
        whenChatReceived.Execute(new ChatInfo("채팅 서버 연결이 끊겼습니다. 다시 연결 합니다."));
        chatConnected = false;
        StartCoroutine(RetryRoutine());
    }

    private IEnumerator RetryRoutine()
    {
        if (retryCount > retryCountMax)
        {
            whenChatReceived.Execute(new ChatInfo("채팅 서버와 연결이 끊겼습니다."));
            chatConnected = false;
            yield break;
        }

        retryCount++;
        yield return retryDelay;
        ConnectToChattingServer();
    }


    private void RetryConnect_Guild()
    {
        whenChatReceived_Guild.Execute(new ChatInfo("채팅 서버 연결이 끊겼습니다. 다시 연결 합니다."));
        chatConnected_Guild = false;
        StartCoroutine(RetryRoutine_Guild());
    }

    private IEnumerator RetryRoutine_Guild()
    {
        if (retryCount_Guild > retryCountMax_Guild)
        {
            whenChatReceived_Guild.Execute(new ChatInfo("채팅 서버와 연결이 끊겼습니다."));
            chatConnected_Guild = false;
            yield break;
        }

        retryCount_Guild++;
        yield return retryDelay;
        ConnectToChattingServer_Guild();
    }



    private void OnJoinChannel(JoinChannelEventArgs args)
    {
        Debug.Log(string.Format("OnJoinChannel {0}", args.ErrInfo));
        //입장에 성공한 경우
        if (args.ErrInfo == ErrorInfo.Success)
        {
            // 내가 접속한 경우 
            if (!args.Session.IsRemote)
            {
                Debug.Log("채널에 접속했습니다");
                chatConnected = true;

            }
            //다른유저가 접속한 경우
            else
            {
#if UNITY_EDITOR
                //Debug.LogError(args.Session.NickName + "님이 접속했습니다");
#endif
            }
        }
        else
        {
            //에러가 발생했을 경우
            Debug.LogError("입장도중 에러가 발생했습니다 : " + args.ErrInfo.Reason);

            chatConnected = false;
        }
    }

    private void OnJoinChannel_Guild(JoinChannelEventArgs args)
    {
        Debug.Log(string.Format("OnJoinChannel {0}", args.ErrInfo));
        //입장에 성공한 경우
        if (args.ErrInfo == ErrorInfo.Success)
        {
            // 내가 접속한 경우 
            if (!args.Session.IsRemote)
            {
                Debug.Log("채널에 접속했습니다");
                chatConnected_Guild = true;

            }
            //다른유저가 접속한 경우
            else
            {
#if UNITY_EDITOR
                //Debug.LogError(args.Session.NickName + "님이 접속했습니다");
#endif
            }
        }
        else
        {
            //에러가 발생했을 경우
            Debug.LogError("입장도중 에러가 발생했습니다 : " + args.ErrInfo.Reason);

            chatConnected_Guild = false;
        }
    }

    private void Update()
    {
        Backend.Chat.Poll();
    }

    bool paused = false;
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            paused = true;
        }
        else if (paused == true && pause == false)
        {
            if (IsChatConnecting() == false)
            {
                RetryConnect();
            }
        }
    }

    private bool IsChatConnecting()
    {
        return Backend.Chat.IsChatConnect(ChannelType.Public);
    }
    private bool IsChatConnecting_Guild()
    {
        return Backend.Chat.IsChatConnect(ChannelType.Guild);
    }



    public void SendChat(string message)
    {
        if (IsChatConnecting() == false)
        {
            RetryConnect();
            return;
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.chatBan).Value == 1f)
        {
            PopupManager.Instance.ShowAlarmMessage("채팅이 차단된 상태입니다.");
            return;
        }

        if (string.IsNullOrEmpty(message))
        {
            PopupManager.Instance.ShowAlarmMessage("메세지가 비었습니다.");
            return;
        }

#if UNITY_EDITOR
        Debug.LogError("채팅 SendMessage");
#endif

        ReplaceBracket(ref message);
        
        AddRankInfo(ref message);

        AddCostumeInfo(ref message);

        AddFrameInfo(ref message);

        Backend.Chat.ChatToChannel(ChannelType.Public, message);
    }

    public void SendChat_Guild(string message, bool addRankInfo = true)
    {
        if (IsChatConnecting_Guild() == false)
        {
            RetryConnect_Guild();
            return;
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.chatBan).Value == 1f)
        {
            PopupManager.Instance.ShowAlarmMessage("채팅이 차단된 상태입니다.");
            return;
        }

        if (string.IsNullOrEmpty(message))
        {
            PopupManager.Instance.ShowAlarmMessage("메세지가 비었습니다.");
            return;
        }

#if UNITY_EDITOR
        Debug.LogError("채팅 SendMessage");
#endif

        if (addRankInfo)
        {
            AddRankInfo(ref message);
        }

        //AddCostumeInfo(ref message);

        //AddFrameInfo(ref message);

        Backend.Chat.ChatToChannel(ChannelType.Guild, message);
    }


    private void ReplaceBracket(ref string message)
    {
        message = message.Replace("<", "＜");
        message = message.Replace(">", "＞");
    }
    private void AddFrameInfo(ref string message)
    {
        message = $"{message}{CommonString.ChatSplitChar}{(int)ServerData.userInfoTable.TableDatas[UserInfoTable.chatFrame].Value}";
    }

    private void AddCostumeInfo(ref string message)
    {
        message = $"{(int)ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value}{CommonString.ChatSplitChar}{message}";
    }

    private void AddRankInfo(ref string message)
    {
        RankType rankType = RankType.None;

        var e = RankManager.Instance.MyRankInfo.GetEnumerator();

        int bestRank = int.MaxValue;

        while (e.MoveNext())
        {
            if (e.Current.Value != null)
            {
                if (e.Current.Value.Rank < bestRank)
                {
                    bestRank = e.Current.Value.Rank;
                    rankType = e.Current.Key;
                }
            }
        }

        if (rankType != RankType.None && bestRank != int.MaxValue && bestRank < 10000)
        {
            string colorPrefix = null;
            if (bestRank < 100)
            {
                colorPrefix = "<color=#ff00ffff>";
            }
            else if (bestRank < 1000)
            {
                colorPrefix = "<color=#ffff00ff>";
            }
            else
            {
                colorPrefix = "<color=#ffffffff>";
            }

            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value < 0)
            {
                message = $"{colorPrefix}({GetRankName(rankType)}{bestRank}위)</color>{CommonString.ChatSplitChar}{message}";
            }
            else
            {
                if (SettingData.showByeolhotitle.Value > 0)
                {
                    var tableData = TableManager.Instance.Byeolho.dataArray[
                        (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value];
                    var str = tableData.Title_Name;
                    var color =CommonUiContainer.Instance.itemGradeColor[tableData.Grade];
                    str = Utils.ColorToHexString(color, str);
                    
                    message = $"{str}{CommonString.ChatSplitChar}{message}";
                }
                else
                {
                    message = $"{colorPrefix}({GetRankName(rankType)}{bestRank}위)</color>{CommonString.ChatSplitChar}{message}";
                }
            }

        }
        else
        {
            message = $"(초보자){CommonString.ChatSplitChar}{message}";
        }
    }

    public string GetRankName(RankType rankType)
    {
        switch (rankType)
        {
            case RankType.Level:
                return CommonString.RankPrefix_Level;
                break;
            case RankType.Stage:
                return CommonString.RankPrefix_Stage;
                break;
            case RankType.Boss:
                return CommonString.RankPrefix_Boss;
                break;
            case RankType.Real_Boss:
                return CommonString.RankPrefix_Real_Boss;
                break;
            case RankType.GangChul:
                return CommonString.RankPrefix_GangChul;
                break;
            case RankType.Relic:
                return CommonString.RankPrefix_Relic;
                break;
            case RankType.MiniGame:
                return CommonString.RankPrefix_MiniGame;
                break;
            case RankType.ChunmaTop:
                return CommonString.RankPrefix_ChunMaTop;
                break;
        }

        return "미등록";
    }

    public void OnChatReceived(ChatEventArgs args)
    {
        if (args.ErrInfo == ErrorInfo.Success)
        {
            // 자신의 메시지일 경우
            if (!args.From.IsRemote)
            {
                var split = args.Message.Split(CommonString.ChatSplitChar);

                if (split.Length >= 4)
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} 나:{split[2]}", int.Parse(split[3])));
                }
                else
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} 나:{split[2]}"));
                }
            }
            // 다른 유저의 메시지일 경우
            else
            {
                var split = args.Message.Split(CommonString.ChatSplitChar);

                if (split.Length >= 4)
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} {args.From.NickName}:{split[2]}", int.Parse(split[3])));
                }
                else
                {
                    whenChatReceived.Execute(new ChatInfo($"{split[0]}{CommonString.ChatSplitChar}{split[1]} {args.From.NickName}:{split[2]}"));
                }
            }
        }
        else if (args.ErrInfo.Category == ErrorCode.BannedChat)
        {
            // 도배방지 메세지 
            if (args.ErrInfo.Detail == ErrorCode.BannedChat)
            {
                PopupManager.Instance.ShowAlarmMessage("메시지를 너무 많이 입력하였습니다.");
            }
        }
    }

    public void OnChatReceived_Guild(ChatEventArgs args)
    {
        if (args.ErrInfo == ErrorInfo.Success)
        {
            // 자신의 메시지일 경우
            if (!args.From.IsRemote)
            {
                var split = args.Message.Split(CommonString.ChatSplitChar);

                if (split.Length == 2)
                {
                    whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]} 나:{split[1]}"));
                }
                else if (split.Length == 1)
                {
                    whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]}"));
                }
            }
            // 다른 유저의 메시지일 경우
            else
            {
                var split = args.Message.Split(CommonString.ChatSplitChar);

                if (split.Length == 2)
                {
                    whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]} {args.From.NickName}:{split[1]}"));
                }
                else if (split.Length == 1)
                {
                    whenChatReceived_Guild.Execute(new ChatInfo($"{split[0]}"));
                }

            }
        }
        else if (args.ErrInfo.Category == ErrorCode.BannedChat)
        {
            // 도배방지 메세지 
            if (args.ErrInfo.Detail == ErrorCode.BannedChat)
            {
                PopupManager.Instance.ShowAlarmMessage("메시지를 너무 많이 입력하였습니다.");
            }
        }
    }
}
