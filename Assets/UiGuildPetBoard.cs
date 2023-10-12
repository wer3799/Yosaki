using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildPetBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI petExpText;

    [SerializeField]
    private TextMeshProUGUI sendAmountText;

    [SerializeField]
    private TextMeshProUGUI petDescription;

    private ObscuredInt maxSendNum = 10;
    private ObscuredFloat eachMarbleNum = 200000f;
    private ObscuredFloat eachGrowthStoneNum = 400000000f;

    [SerializeField]
    private Button recordButton;

    private void Start()
    {
        Subscribe();
        SetDescriptionText("1");
    }

    private void OnEnable()
    {
        GuildManager.Instance.LoadGuildLevelGoods();
    }

    public void SetDescriptionText(string goodsNum)
    {
        sendAmountText.SetText($"먹이 1회당 \n레벨 1상승\n<color=#ff00ffff>{CommonString.GetItemName(Item_Type.GrowthStone)} {Utils.ConvertBigNum(eachGrowthStoneNum)}개</color> 획득");
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildPetExp.AsObservable().Subscribe(e =>
        {
            petExpText.SetText($"LV : {e}");

            petDescription.SetText($"{CommonString.GetStatusName(StatusType.ExpGainPer)} {PlayerStats.GetGuildPetEffect(StatusType.ExpGainPer) * 100f}\n{CommonString.GetStatusName(StatusType.GoldGainPer)} {PlayerStats.GetGuildPetEffect(StatusType.GoldGainPer) * 100f}\n{CommonString.GetStatusName(StatusType.AttackAddPer)} {PlayerStats.GetGuildPetEffect(StatusType.AttackAddPer) * 100f}\n");
        }).AddTo(this);
    }

    public void SendPetExp(int exchangeGoodsNum)
    {
        bool canRecord = ServerData.userInfoTable.CanRecordGuildScore();

#if UNITY_EDITOR
        canRecord = true;
#endif

        if (canRecord == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오후11시~ 다음날 오전5시 까지는\n먹이를 줄 수 없습니다!", null);
            return;
        }

        if (ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 이미 먹이를 줬습니다.");
            return;
        }
        

        if (ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value < exchangeGoodsNum * eachMarbleNum)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Marble)} {Utils.ConvertBigNum(exchangeGoodsNum * eachMarbleNum)}개를 먹이로 줄까요?\n레벨 {exchangeGoodsNum}상승", () =>
          {
              if (ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value == 1)
              {
                  PopupManager.Instance.ShowAlarmMessage("오늘은 이미 먹이를 줬습니다.");
                  return;
              }
              
              recordButton.interactable = false;

              //인원체크
              var guildInfoBro = Backend.Social.Guild.GetMyGuildGoodsV3();

              if (guildInfoBro.IsSuccess())
              {
                  var returnValue = guildInfoBro.GetReturnValuetoJSON();

                  int addAmount = int.Parse(returnValue["goods"]["totalGoods8Amount"]["N"].ToString());

                  bool maxContributed = addAmount >= GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value);

                  if (maxContributed)
                  {
                      PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{GuildManager.Instance.myGuildName} 문파는 \n오늘 더이상 먹이를 줄 수 없습니다!\n<color=red>최대 {GuildManager.Instance.GetGuildMemberMaxNum(GuildManager.Instance.guildLevelExp.Value)}번 추가 가능</color>\n<color=red>(매일 오전 5시 초기화)</color>", null);
                      recordButton.interactable = true;
                      return;
                  }
              }
              else
              {
                  recordButton.interactable = true;
                  PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오류가 발생했습니다. 잠시후 다시 시도해 주세요.", null);
                  return;
              }
              //

              ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value = 1;

              List<TransactionValue> transactions = new List<TransactionValue>();

              Param userInfoParam = new Param();

              userInfoParam.Add(UserInfoTable.sendPetExp, ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value);

              Param goodsParam = new Param();

              ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= exchangeGoodsNum * eachMarbleNum;
              ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += exchangeGoodsNum * eachGrowthStoneNum;

              goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
              goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);

              transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
              transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

              var trBro = Backend.GameData.TransactionWrite(transactions);

              if (trBro.IsSuccess())
              {
                  var bro2 = Backend.URank.Guild.ContributeGuildGoods(RankManager.Rank_Guild_Reset_Uuid_Feed, goodsType.goods8, 1);

                  if (bro2.IsSuccess())
                  {
                      var broForGuildLevel = Backend.Social.Guild.ContributeGoodsV3(goodsType.goods5, (int)exchangeGoodsNum);

                      if (broForGuildLevel.IsSuccess())
                      {
                          YorinMissionManager.UpdateYorinMissionClear(YorinMissionKey.YMission7_3, 1);
                          recordButton.interactable = true;
                          GuildManager.Instance.guildPetExp.Value += (int)exchangeGoodsNum;
                          PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"레벨 {exchangeGoodsNum}증가,\n{CommonString.GetItemName(Item_Type.GrowthStone)} {Utils.ConvertBigNum(exchangeGoodsNum * eachGrowthStoneNum)}개 획득!", null);
                          GuildManager.Instance.LoadGuildLevelGoods();

                          EventMissionManager.UpdateEventMissionClear(EventMissionKey.MISSION2, 1);
                          
                          EventMissionManager.UpdateEventMissionClear(EventMissionKey.AMISSION2, 1);

                          EventMissionManager.UpdateEventMissionClear(EventMissionKey.TMISSION2, 1);

                          var memberCell = UiGuildMemberList.Instance.GetMemberCell(PlayerData.Instance.NickName);

                          if (memberCell != null)
                          {
                              memberCell.UpdateDonatedObject_PetExp(true);
                          }

                          if (UiGuildMemberList.Instance.myMemberInfo != null)
                          {
                              UiGuildMemberList.Instance.myMemberInfo.donateDogFeedAmount += 10;
                          }

                      }
                      else
                      {
                          recordButton.interactable = true;
                          ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value = 0;
                          ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += exchangeGoodsNum * eachMarbleNum;
                          ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= exchangeGoodsNum * eachGrowthStoneNum;
                          PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "먹이 주기 실패 잠시후 다시 시도해 주세요", null);
                      }
                  }
                  else
                  {
                      recordButton.interactable = true;
                      ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value = 0;
                      ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += exchangeGoodsNum * eachMarbleNum;
                      ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= exchangeGoodsNum * eachGrowthStoneNum;
                      PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "먹이 주기 실패 잠시후 다시 시도해 주세요", null);
                  }
              }
              else
              {
                  recordButton.interactable = true;
                  ServerData.userInfoTable.TableDatas[UserInfoTable.sendPetExp].Value = 0;
                  ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += exchangeGoodsNum * eachMarbleNum;
                  ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= exchangeGoodsNum * eachGrowthStoneNum;
                  PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "먹이 주기 실패 잠시후 다시 시도해 주세요", null);
              }



          }, () => { });
    }

}
