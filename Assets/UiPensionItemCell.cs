using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using System.Linq;
using TMPro;

public class UiPensionItemCell : MonoBehaviour
{
    private ObscuredString pensionKey;
    private ObscuredInt rewardAmount;
    private ObscuredInt idx;
    private ObscuredInt rewardMaxCount;

    [SerializeField]
    private TextMeshProUGUI rewardAmounText;

    [SerializeField]
    private TextMeshProUGUI dayText;

    [SerializeField]
    private GameObject rewardedObject;

    [SerializeField]
    private GameObject lockObject;

    private bool initialized = false;
    public void Initialize(string key, int idx, int amount, int rewardMaxCount)
    {
        if (initialized == true) return;

        initialized = true;

        this.pensionKey = key;
        this.rewardAmount = amount;
        this.idx = idx;
        this.rewardMaxCount = rewardMaxCount;

        rewardAmounText.SetText(Utils.ConvertBigNum(amount));

        dayText.SetText($"{idx + 1}일차");

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.pensionServerTable.TableDatas[pensionKey].AsObservable().Subscribe(e =>
        {
            bool hasRewrad = ServerData.pensionServerTable.HasReward(pensionKey, idx);
            rewardedObject.SetActive(hasRewrad);
        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[pensionKey].AsObservable().Subscribe(attenCount =>
        {
            if (ServerData.iapServerTable.TableDatas[pensionKey].buyCount.Value == 0)
            {
                lockObject.SetActive(true);
            }
            else
            {
                lockObject.SetActive(attenCount < idx);
            }

        }).AddTo(this);

        ServerData.iapServerTable.TableDatas[pensionKey].buyCount.AsObservable().Subscribe(e =>
        {
            if (ServerData.iapServerTable.TableDatas[pensionKey].buyCount.Value > 0)
            {
                lockObject.SetActive(idx > ServerData.userInfoTable.TableDatas[pensionKey].Value);
            }

        }).AddTo(this);
    }

    public void OnClickRewardButton()
    {
        var iapServerData = ServerData.iapServerTable.TableDatas[pensionKey];

        if (iapServerData.buyCount.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("연금 상품이 필요합니다.");
            return;
        }

        int attendanceCount = (int)ServerData.userInfoTable.TableDatas[pensionKey].Value;

        if (attendanceCount < idx)
        {
            PopupManager.Instance.ShowAlarmMessage("아직 받을수 없습니다.");
            return;
        }

        if (ServerData.pensionServerTable.HasReward(pensionKey, idx))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 받았습니다!");
            return;
        }

        Item_Type itemType = KeyToItemType(pensionKey);

        SoundManager.Instance.PlaySound("Reward");

        //로컬
        ServerData.pensionServerTable.TableDatas[pensionKey].Value += $",{idx.ToString()}";

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param pensionParam = new Param();
        pensionParam.Add(pensionKey, ServerData.pensionServerTable.TableDatas[pensionKey].Value);

        transactions.Add(TransactionValue.SetUpdate(PensionServerTable.tableName, PensionServerTable.Indate, pensionParam));
        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(itemType, rewardAmount));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage("연금 수령!");
              //    LogManager.Instance.SendLogType("Pension", pensionKey, idx.ToString());

              //모든 보상 다받음. 보상리셋,상품초기화,출석 초기화
              if (ServerData.pensionServerTable.RewarededCount(pensionKey) - 1 == rewardMaxCount)
              {
                  iapServerData.buyCount.Value = 0;
                  ServerData.pensionServerTable.TableDatas[pensionKey].Value = string.Empty;

                  ServerData.userInfoTable.TableDatas[pensionKey].Value = 0f;

                  List<TransactionValue> completeTransactions = new List<TransactionValue>();

                  Param iapParam = new Param();
                  iapParam.Add(pensionKey, iapServerData.ConvertToString());

                  Param pensionDefaultParam = new Param();
                  pensionDefaultParam.Add(pensionKey, ServerData.pensionServerTable.TableDatas[pensionKey].Value);

                  Param userInfoParam = new Param();

                  userInfoParam.Add(pensionKey, ServerData.userInfoTable.TableDatas[pensionKey].Value);

                  completeTransactions.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));
                  completeTransactions.Add(TransactionValue.SetUpdate(PensionServerTable.tableName, PensionServerTable.Indate, pensionDefaultParam));
                  completeTransactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


                  ServerData.SendTransactionV2(completeTransactions, successCallBack: () =>
                  {
                      //  LogManager.Instance.SendLogType("Pension", pensionKey, "Reset");
                      PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상 전부 수령 완료!\n연금 초기화", null);
                  });

              }
          });


    }

    // 쇼메시지 없는 전부받기용
    public Tuple<Item_Type,string> OnClickRewardButton_All()
    {
        var iapServerData = ServerData.iapServerTable.TableDatas[pensionKey];

        if (iapServerData.buyCount.Value == 0)
        {
            return null;
        }

        int attendanceCount = (int)ServerData.userInfoTable.TableDatas[pensionKey].Value;

        if (attendanceCount < idx)
        {
            return null;
        }

        if (ServerData.pensionServerTable.HasReward(pensionKey, idx))
        {
            return null;
        }

        Item_Type itemType = KeyToItemType(pensionKey);

        //로컬//
        ServerData.pensionServerTable.TableDatas[pensionKey].Value += $",{idx.ToString()}";
        ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(itemType)).Value+=rewardAmount;
        //////
        
        //모든 보상 다받음. 보상리셋,상품초기화,출석 초기화
      if (ServerData.pensionServerTable.RewarededCount(pensionKey) - 1 == rewardMaxCount)
      {
          iapServerData.buyCount.Value = 0;
          ServerData.pensionServerTable.TableDatas[pensionKey].Value = string.Empty;

          ServerData.userInfoTable.TableDatas[pensionKey].Value = 0f;

          List<TransactionValue> completeTransactions = new List<TransactionValue>();

          Param iapParam = new Param();
          iapParam.Add(pensionKey, iapServerData.ConvertToString());

          Param pensionDefaultParam = new Param();
          pensionDefaultParam.Add(pensionKey, ServerData.pensionServerTable.TableDatas[pensionKey].Value);

          Param userInfoParam = new Param();

          userInfoParam.Add(pensionKey, ServerData.userInfoTable.TableDatas[pensionKey].Value);

          completeTransactions.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));
          completeTransactions.Add(TransactionValue.SetUpdate(PensionServerTable.tableName, PensionServerTable.Indate, pensionDefaultParam));
          completeTransactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


          ServerData.SendTransactionV2(completeTransactions, successCallBack: () =>
          {
              //  LogManager.Instance.SendLogType("Pension", pensionKey, "Reset");
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상 전부 수령 완료!\n연금 초기화", null);
          });

      }

      return new Tuple<Item_Type,string>(itemType,pensionKey);
    }
    
    
    private Item_Type KeyToItemType(string key)
    {
        if (key == "oakpension")
        {
            return Item_Type.Jade;
        }
        else if (key == "marblepension")
        {
            return Item_Type.Marble;
        }
        else if (key == "relicpension")
        {
            return Item_Type.RelicTicket;
        }
        else if (key == "peachpension")
        {
            return Item_Type.PeachReal;
        }
        else if (key == "weaponpension")
        {
            return Item_Type.SP;
        }
        else if (key == "hellpension")
        {
            return Item_Type.Hel;
        }
        else if (key == "chunpension")
        {
            return Item_Type.Cw;
        }
        else if (key == "dokebipension")
        {
            return Item_Type.DokebiFire;
        }
        else if (key == "sumipension")
        {
            return Item_Type.SumiFire;
        }   
        else if (key == "sealWeaponpension")
        {
            return Item_Type.SealWeaponClear;
        }
        else if (key == "ringpension")
        {
            return Item_Type.NewGachaEnergy;
        }
        else if (key == "suhopetfeedclearpension")
        {
            return Item_Type.SuhoPetFeedClear;
        }
        else if (key == "foxfirepension")
        {
            return Item_Type.FoxRelicClearTicket;
        }
        else if (key == "sealswordpension")
        {
            return Item_Type.SealWeaponClear;
        }
        else if (key == "dosulpension")
        {
            return Item_Type.DosulClear;
        }
        else if (key == "guimoonpension")
        {
            return Item_Type.GuimoonRelicClearTicket;
        }
        else if (key == "meditationpension")
        {
            return Item_Type.MeditationClearTicket;
        }
        else if (key == "taeguekpension")
        {
            return Item_Type.TaeguekElixir;
        }
        else if (key == "blacksoulpension")
        {
            return Item_Type.BlackFoxClear;
        }
        else if (key == "studentspotpension")
        {
            return Item_Type.HYC;
        }
        else if (key == "transjeweltpension")
        {
            return Item_Type.TJCT;
        }
        else if (key == "masterpension")
        {
            return Item_Type.ClearTicket;
        }
        else
        {
            return Item_Type.SmithFire;
        }
    }

}
