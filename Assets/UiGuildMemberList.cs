using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using TMPro;
using UniRx;
using UnityEngine;
using static UiGuildMemberCell;
using System;
using LitJson;
using UnityEngine.Serialization;

public class UiGuildMemberList : SingletonMono<UiGuildMemberList>
{
    [SerializeField]
    private UiGuildMemberCell memberCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiGuildMemberCell> memberCells;

    public int guildMemberCount = 0;

    [SerializeField]
    private TextMeshProUGUI guildNameInputBoard;

    [SerializeField]
    private TextMeshProUGUI memberNumText;

    private bool initialized = false;

    [SerializeField]
    private GameObject guildInfoButton;

    public ReactiveProperty<int> attenUserNum = new ReactiveProperty<int>();

    public GuildMemberInfo myMemberInfo { get; set; }

    public static int myCurrentGuildTowerScore = 0;
    public static int serverRecordedTowerScore = 0;

    [SerializeField]
    private ReactiveProperty<int> currentGuildTowerTotalScore = new ReactiveProperty<int>();

    [SerializeField]
    private TextMeshProUGUI myGuildTotalTowerScore;

    [FormerlySerializedAs("nowWorkPlayerDescription")] [SerializeField]
    private TextMeshProUGUI notWorkPlayerDescription;


    public UiGuildMemberCell GetMemberCell(string nickName)
    {
        nickName = nickName.Replace(CommonString.IOS_nick, "");

        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null &&
                memberCells[i].guildMemberInfo.nickName.Replace(CommonString.IOS_nick, "").Equals(nickName))
            {
                return memberCells[i];
            }
        }

        return null;
    }

    public void RemovePlayer(string nickName)
    {
        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName == nickName)
            {
                memberCells[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    public GuildGrade GetMyGuildGrade()
    {
        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName.Replace(CommonString.IOS_nick, "").Equals(PlayerData.Instance.NickName.Replace(CommonString.IOS_nick, "")))
            {
                return memberCells[i].guildMemberInfo.guildGrade;
            }
        }

        return GuildGrade.Member;
    }

    public GuildGrade GetGuildGrade(string nickName)
    {
        nickName = nickName.Replace(CommonString.IOS_nick, "");

        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null && memberCells[i].guildMemberInfo.nickName.Replace(CommonString.IOS_nick, "").Equals(nickName))
            {
                return memberCells[i].guildMemberInfo.guildGrade;
            }
        }

        return GuildGrade.Member;
    }

    void Start()
    {
        Initialize();

        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildLevelExp.AsObservable().Subscribe(e => { RefreshGuildMemberCountText(); }).AddTo(this);

        currentGuildTowerTotalScore.AsObservable().Subscribe(e =>
        {
            myGuildTotalTowerScore.SetText($"문파 총점:{e}점");

            var rewardServerData = ServerData.bossServerTable.TableDatas[TableManager.Instance.TwelveBossTable.dataArray[117].Stringid];

            rewardServerData.score.Value = $"{e}";
        }).AddTo(this);
    }

    private void OnEnable()
    {
        if (initialized)
        {
            RefreshMemberList();
        }
    }

    private void Initialize()
    {
        myCurrentGuildTowerScore = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.currentFloorGuildTower].Value;

        memberCells = new List<UiGuildMemberCell>();

        for (int i = 0; i < 35; i++)
        {
            var cell = Instantiate<UiGuildMemberCell>(memberCellPrefab, cellParent);

            cell.gameObject.SetActive(false);

            memberCells.Add(cell);
        }

        RefreshMemberList();


        initialized = true;
    }

    private void RelocateGuildMember()
    {
        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null)
            {
                if (memberCells[i].guildMemberInfo.guildGrade == GuildGrade.ViceMaster)
                {
                    memberCells[i].gameObject.transform.SetAsFirstSibling();
                }
            }
        }

        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null)
            {
                if (memberCells[i].guildMemberInfo.guildGrade == GuildGrade.Master)
                {
                    memberCells[i].gameObject.transform.SetAsFirstSibling();
                }
            }
        }
    }

    private void RefreshGuildMemberCountText()
    {
        memberNumText.SetText($"문파 인원 : {guildMemberCount}/{GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value)}");
    }

    public void RefreshMemberList()
    {
        memberNumText.SetText(string.Empty);

        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].guildMemberInfo != null)
                memberCells[i].guildMemberInfo.guildTowerFloor = 0;
        }

        GetGuildGoods6AmountByIndateV3();

        if (goodsDictionary.Count == 0)
        {
            memberCells.ForEach(e => e.gameObject.SetActive(false));
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "조회 실패\n잠시후 다시 시도해 주세요", null);
            return;
        }


        UiGuildLoadingMask.Instance.Show(true);

        SendQueue.Enqueue(Backend.Social.Guild.GetGuildMemberListV3, GuildManager.Instance.myGuildIndate, GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value) + 5, (bro) =>
        {
            UiGuildLoadingMask.Instance.Show(false);

            if (bro.IsSuccess())
            {
                var returnValue = bro.GetReturnValuetoJSON();

                var rows = returnValue["rows"];

                guildMemberCount = rows.Count;
                guildMemberCount = Mathf.Min(guildMemberCount, GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value));

                memberNumText.SetText($"문파 인원 : {guildMemberCount}/{GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value)}");

                bool findMyData = false;
                string myNickName = PlayerData.Instance.NickName.Replace(CommonString.IOS_nick, "");

                attenUserNum.Value = 0;

                int attenNum = 0;

                for (int i = 0; i < memberCells.Count; i++)
                {
                    if (i < guildMemberCount)
                    {
                        memberCells[i].gameObject.SetActive(true);

                        var data = rows[i];

                        string nickName = data["nickname"]["S"].ToString();
                        string position = data["position"]["S"].ToString();
                        string lastLogin = data["lastLogin"]["S"].ToString();
                        string gamerIndate = data["gamerInDate"]["S"].ToString();
                        int donateGoods = int.Parse(data["totalGoods3Amount"]["N"].ToString());
                        int donateDogFeedAmounts = int.Parse(data["totalGoods5Amount"]["N"].ToString());
                        bool todayDonated = int.Parse(data["totalGoods9Amount"]["N"].ToString()) >= 1;
                        bool todayDonatedPetExp = int.Parse(data["totalGoods8Amount"]["N"].ToString()) >= 1;
                        int guildTowerFloor = GetGoods6Amount(nickName);


                        if (todayDonated)
                        {
                            attenNum++;
                        }

                        bool isMyData = nickName.Replace(CommonString.IOS_nick, "").Equals(myNickName);

                        if (isMyData)
                        {
                            if (guildTowerFloor != int.MaxValue)
                            {
                                serverRecordedTowerScore = guildTowerFloor;
                            }
                        }

                        if (isMyData && myCurrentGuildTowerScore != guildTowerFloor && guildTowerFloor != int.MaxValue)
                        {
                            UpdateGuildTowerScore(guildTowerFloor);
                            guildTowerFloor = myCurrentGuildTowerScore;
                        }

                        if (guildTowerFloor == int.MaxValue)
                        {
                            guildTowerFloor = 0;
                        }

                        var memberData = new GuildMemberInfo(nickName, position, lastLogin, gamerIndate, donateGoods, todayDonated, todayDonatedPetExp, guildTowerFloor, donateDogFeedAmounts);

                        memberCells[i].Initialize(memberData);
                        memberCells[i].transform.SetAsFirstSibling();

                        if (findMyData == false)
                        {
                            findMyData = nickName.Replace(CommonString.IOS_nick, "").Equals(myNickName);
                        }

                        if (nickName.Replace(CommonString.IOS_nick, "").Equals(myNickName))
                        {
                            this.myMemberInfo = memberData;
                        }
                    }
                    else
                    {
                        memberCells[i].gameObject.SetActive(false);
                    }
                }

                attenUserNum.Value = attenNum;

#if UNITY_EDITOR
                attenUserNum.Value = 29;
#endif

                if (findMyData == false)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        var data = rows[i];

                        string nickName = data["nickname"]["S"].ToString();

                        string position = data["position"]["S"].ToString();
                        string lastLogin = data["lastLogin"]["S"].ToString();
                        string gamerIndate = data["gamerInDate"]["S"].ToString();
                        int donateGoods = int.Parse(data["totalGoods3Amount"]["N"].ToString());
                        int donateDogFeedAmount = int.Parse(data["totalGoods5Amount"]["N"].ToString());
                        bool todayDonated = int.Parse(data["totalGoods9Amount"]["N"].ToString()) >= 1;
                        bool todayDonatedPetExp = int.Parse(data["totalGoods8Amount"]["N"].ToString()) >= 1;
                        int guildTowerFloor = GetGoods6Amount(nickName);

                        //내꺼찾음
                        if (nickName.Replace(CommonString.IOS_nick, "").Equals(myNickName))
                        {
                            //점수추가
                            if (myCurrentGuildTowerScore != guildTowerFloor && guildTowerFloor != int.MaxValue)
                            {
                                UpdateGuildTowerScore(guildTowerFloor);
                                guildTowerFloor = myCurrentGuildTowerScore;
                            }

                            if (guildTowerFloor == int.MaxValue)
                            {
                                guildTowerFloor = 0;
                            }
                            else
                            {
                                serverRecordedTowerScore = guildTowerFloor;
                            }


                            var memberData = new GuildMemberInfo(nickName, position, lastLogin, gamerIndate, donateGoods, todayDonated, todayDonatedPetExp, guildTowerFloor, donateDogFeedAmount);
                            memberCells[0].Initialize(memberData);
                            memberCells[0].transform.SetAsFirstSibling();

                            this.myMemberInfo = memberData;

                            break;
                        }
                    }
                }

                for (int i = 0; i < memberCells.Count; i++)
                {
                    if (i < guildMemberCount)
                    {
                        memberCells[i].RefreshKickButton();
                    }
                }

                PopupManager.Instance.ShowAlarmMessage("갱신 완료");
                RelocateGuildMember();
            }
            else
            {
                memberCells.ForEach(e => e.gameObject.SetActive(false));
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "조회 실패\n잠시후 다시 시도해 주세요", null);
            }

            guildInfoButton.SetActive(GetMyGuildGrade() == GuildGrade.Master);


            currentGuildTowerTotalScore.Value = 0;

            int updatedScore = 0;

            for (int i = 0; i < memberCells.Count; i++)
            {
                if (memberCells[i].guildMemberInfo != null)
                    updatedScore += memberCells[i].guildMemberInfo.guildTowerFloor;
            }

            currentGuildTowerTotalScore.Value = updatedScore;

            UpdateNotWorkUserText();
        });
        // var bro = Backend.Social.Guild.GetGuildMemberListV3();
    }

    private List<string> gumihoList = new List<string>();
    private List<string> petExpList = new List<string>();

    private void UpdateNotWorkUserText()
    {
        string description = string.Empty;

        gumihoList.Clear();
        petExpList.Clear();

        for (int i = 0; i < memberCells.Count; i++)
        {
            if (memberCells[i].gameObject.activeInHierarchy == true)
            {
                if (memberCells[i].guildMemberInfo.todayDonated == false)
                {
                    gumihoList.Add(memberCells[i].guildMemberInfo.nickName);
                }

                if (memberCells[i].guildMemberInfo.todayDonatedPetExp == false)
                {
                    petExpList.Add(memberCells[i].guildMemberInfo.nickName);
                }
            }
        }

        description += "<color=red>구미호 미등록</color>\n\n";

        for (int i = 0; i < gumihoList.Count; i++)
        {
            description += $"{Utils.GetOriginNickName(gumihoList[i])},";
        }

        description += "\n\n<color=blue>강아지 밥 안준사람</color>\n\n";

        for (int i = 0; i < petExpList.Count; i++)
        {
            description += $"{Utils.GetOriginNickName(petExpList[i])},";
        }

        notWorkPlayerDescription.SetText(description);
    }

    public void UpdateGuildTowerScore(int serverScore)
    {
        int currentScore = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.currentFloorGuildTower].Value;

        if (currentScore == serverScore) return;

        int addValue = currentScore - serverScore;

        if (addValue <= 0) return;

        //점수추가
        var bro = Backend.Social.Guild.ContributeGoodsV3(goodsType.goods6, addValue);

        if (bro.IsSuccess())
        {
        }
    }


    /// <summary>
    /// //////////////////////////////////////////
    /// </summary>
    public class GoodsItem
    {
        public int totalGoodsAmount;
        public List<GoodsUserItem> userList = new List<GoodsUserItem>();

        public override string ToString()
        {
            string userString = string.Empty;
            for (int i = 0; i < userList.Count; i++)
            {
                userString += userList[i].ToString() + "\n";
            }

            return $"[totalGoodsAmount : {totalGoodsAmount}]\n" +
                   $"{userString}\n";
        }
    }

    public class GoodsUserItem
    {
        public int usingTotalAmount;
        public int totalAmount;
        public string inDate;
        public string nickname;
        public string updatedAt;

        public override string ToString()
        {
            return $"\tnickname : {nickname}\n" +
                   $"\tinDate : {inDate}\n" +
                   $"\ttotalAmount : {totalAmount}\n" +
                   $"\tusingTotalAmount : {usingTotalAmount}\n" +
                   $"\tupdatedAt : {updatedAt}\n";
        }
    }

    private int GetGoods6Amount(string nickName)
    {
        if (goodsDictionary.ContainsKey("goods6") == false) return int.MaxValue;

        var myData = goodsDictionary["goods6"].userList.Find(e => e.nickname.Equals(nickName));

        if (myData == null) return 0;

        if (myData.totalAmount > TableManager.Instance.guildTowerTable.dataArray.Length + 20)
        {
            myData.totalAmount = (int)(myData.totalAmount / 2);
        }

        return myData.totalAmount;
    }

    private Dictionary<string, GoodsItem> goodsDictionary = new Dictionary<string, GoodsItem>();

    public void GetGuildGoods6AmountByIndateV3()
    {
        goodsDictionary.Clear();

        var bro = Backend.Social.Guild.GetGuildGoodsByIndateV3(GuildManager.Instance.myGuildIndate);

        if (bro.IsSuccess() == false)
            return;

        var goodsJson = bro.GetFlattenJSON()["goods"];
        foreach (var column in goodsJson.Keys)
        {
            if (column.Contains("totalGoods"))
            {
                GoodsItem goodsItem = new GoodsItem();

                goodsItem.totalGoodsAmount = Int32.Parse(goodsJson[column].ToString());

                string goodsNum = column.Replace("totalGoods", "");
                goodsNum = goodsNum.Replace("Amount", "");

                string goodsName = "goods" + goodsNum + "UserList";

                JsonData userListJson = goodsJson[goodsName];
                for (int i = 0; i < userListJson.Count; i++)
                {
                    GoodsUserItem user = new GoodsUserItem();

                    user.inDate = userListJson[i]["inDate"].ToString();
                    user.nickname = userListJson[i]["nickname"].ToString();
                    if (userListJson[i].ContainsKey("usingTotalAmount"))
                    {
                        user.usingTotalAmount = Int32.Parse(userListJson[i]["usingTotalAmount"].ToString());
                    }

                    user.totalAmount = Int32.Parse(userListJson[i]["totalAmount"].ToString());
                    user.updatedAt = userListJson[i]["updatedAt"].ToString();

                    goodsItem.userList.Add(user);
                }

                goodsDictionary.Add("goods" + goodsNum, goodsItem);
            }
        }
    }
}