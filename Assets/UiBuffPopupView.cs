using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System;

public class UiBuffPopupView : MonoBehaviour
{
    [SerializeField]
    private Image buffIcon;

    [SerializeField]
    private TextMeshProUGUI buffDescription;

    [SerializeField]
    private GameObject useButtonObject;

    [SerializeField]
    private TextMeshProUGUI remainSecText;

    private BuffTableData buffTableData;

    [SerializeField]
    private Image buffGetButton;

    [SerializeField]
    private Sprite getEnable;

    [SerializeField]
    private Sprite getDisable;

    [SerializeField]
    private TextMeshProUGUI yomulDesc;

    [SerializeField]
    private TextMeshProUGUI remainUseDesc;

    [SerializeField]
    private GameObject alwaysActiveObject;

    [SerializeField]
    private GameObject remainSecObject;

    [SerializeField]
    private GameObject adImage;

    [SerializeField]
    private GameObject guildImage;

    [SerializeField]
    private GameObject oneYearImage;

    [SerializeField]
    private GameObject monthImage;
    
    [SerializeField]
    private GameObject coldImage;

    [SerializeField]
    private GameObject winterImage;

    private bool initialized = false;



    public void Initalize(BuffTableData buffTableData)
    {
        this.buffTableData = buffTableData;

        UpdateAbilUi();

        buffIcon.sprite = CommonUiContainer.Instance.buffIconList[buffTableData.Id];

        Subscribe();

        initialized = true;

        if (adImage != null)
        {
            adImage.gameObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.Normal);
        }

        if (guildImage != null)
        {
            guildImage.gameObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.Guild);
        }

        if (monthImage != null)
        {
            monthImage.gameObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.Month);
        }


        if (oneYearImage != null)
        {
            oneYearImage.gameObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.OneYear);
        }


        if (yomulDesc != null)
        {
            yomulDesc.gameObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.Yomul);
        }

        if (coldImage != null)
        {
            coldImage.gameObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.Cold);
        }

        if (winterImage != null)
        {
            winterImage.gameObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.Winter);
        }

        if (buffTableData.BUFFTYPEENUM == BuffTypeEnum.Yomul)
        {
            var yomulTableData = TableManager.Instance.YomulAbilTable.dataArray[buffTableData.Yomulid];
            yomulDesc.SetText($"{yomulTableData.Abilname} LV:{buffTableData.Unlockyomullevel} 필요");
        }
    }

    private void UpdateAbilUi()
    {
        if (alwaysActiveObject != null)
        {
            alwaysActiveObject.SetActive(buffTableData.BUFFTYPEENUM == BuffTypeEnum.Yomul);
        }

        if (buffTableData.BUFFTYPEENUM != BuffTypeEnum.Yomul)
        {
            TimeSpan ts = TimeSpan.FromSeconds(buffTableData.Buffseconds);

            StatusType type = (StatusType)buffTableData.Bufftype;

            if (type.IsPercentStat())
            {
                buffDescription.SetText($"{CommonString.GetStatusName(type)}+{buffTableData.Buffvalue * 100f}%({ts.TotalMinutes}분)");
            }
            else
            {
                buffDescription.SetText($"{CommonString.GetStatusName(type)}+{Utils.ConvertBigNum(buffTableData.Buffvalue)}({ts.TotalMinutes}분)");
            }
        }
        //요물버프
        else
        {
            if (alwaysActiveObject != null && remainSecObject != null)
            {
                alwaysActiveObject.SetActive(ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 1);

                remainSecObject.SetActive(ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 0);
            }

            if (ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 0)
            {
                TimeSpan ts = TimeSpan.FromSeconds(buffTableData.Buffseconds);

                StatusType type = (StatusType)buffTableData.Bufftype;

                if (type.IsPercentStat())
                {
                    buffDescription.SetText($"{CommonString.GetStatusName(type)}+{buffTableData.Buffvalue * 100f}%({ts.TotalMinutes}분)");
                }
                else
                {
                    buffDescription.SetText($"{CommonString.GetStatusName(type)}+{Utils.ConvertBigNum(buffTableData.Buffvalue)}({ts.TotalMinutes}분)");
                }
            }
            //각성
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(buffTableData.Buffseconds);

                StatusType type = (StatusType)buffTableData.Bufftype;

                if (type.IsPercentStat())
                {
                    buffDescription.SetText($"{CommonString.GetStatusName(type)}+{buffTableData.Buffawakevalue * 100f}%");
                }
                else
                {
                    buffDescription.SetText($"{CommonString.GetStatusName(type)}+{Utils.ConvertBigNum(buffTableData.Buffawakevalue)}");
                }
            }
        }
    }

    private void OnEnable()
    {
        if (initialized == false) return;

        BuffEndCheck();

        WhenRemainSecChanged(ServerData.buffServerTable.TableDatas[buffTableData.Stringid].remainSec.Value);
    }

    private void WhenRemainSecChanged(float remainSec)
    {
        if (this.gameObject.activeInHierarchy == false) return;

        if (remainSec <= 0f)
        {
            remainSecText.SetText("0초");
        }
        else
        {
            TimeSpan ts = TimeSpan.FromSeconds(remainSec);

            if (ts.Hours != 0)
            {
                remainSecText.SetText($"{ts.Hours}시간 {ts.Minutes}분 {ts.Seconds}초");
            }
            else
            {
                remainSecText.SetText($"{ts.Minutes}분 {ts.Seconds}초");
            }
        }
    }

    private void Subscribe()
    {
        if (initialized) return;

        ServerData.buffServerTable.TableDatas[buffTableData.Stringid].remainSec.AsObservable().Subscribe(e =>
        {
            WhenRemainSecChanged(e);
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(buffTableData.Stringid).AsObservable().Subscribe(e =>
        {
            buffGetButton.sprite = e < buffTableData.Usecount ? getEnable : getDisable;

            if (remainUseDesc != null)
            {
                remainUseDesc.SetText($"{buffTableData.Usecount - e}/{buffTableData.Usecount}회");
            }
        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].AsObservable().Subscribe(e =>
        {
            UpdateAbilUi();
        }).AddTo(this);
    }

    public void OnClickGetBuffButton()
    {
        if (buffTableData.BUFFTYPEENUM == BuffTypeEnum.Yomul && ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("항상 활성화 되어 있습니다.");
            return;
        }

        if (ServerData.userInfoTable.GetTableData(buffTableData.Stringid).Value >= buffTableData.Usecount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 획득할 수 없습니다.");
            return;
        }

        if (buffTableData.BUFFTYPEENUM == BuffTypeEnum.Normal)
        {
            //모두버튼일때
            if (PopupManager.Instance.ignoreAlarmMessage) 
            {
                //광고제거 있어야지만 작동
                if (AdManager.Instance.HasRemoveAdProduct()) 
                {
                    AdManager.Instance.ShowRewardedReward(() =>
                    {
                        BuffGetRoutine();
                    });
                }
            }
            else 
            {
                AdManager.Instance.ShowRewardedReward(() =>
                {
                    BuffGetRoutine();
                });
            }
        }
        else if (buffTableData.BUFFTYPEENUM == BuffTypeEnum.Yomul)
        {
            var yomulTableData = TableManager.Instance.YomulAbilTable.dataArray[buffTableData.Yomulid];
            var yomulServerData = ServerData.yomulServerTable.TableDatas[yomulTableData.Stringid];

            if (yomulServerData.hasAbil.Value == 0 || yomulServerData.level.Value < buffTableData.Unlockyomullevel)
            {
                PopupManager.Instance.ShowAlarmMessage("요물 능력치 레벨이 부족합니다.");
                return;
            }
            else
            {
                BuffGetRoutine();
            }
        }
        else if (buffTableData.BUFFTYPEENUM == BuffTypeEnum.Guild)
        {
            int getLevel = GuildManager.Instance.GetBuffGetExp(buffTableData.Id);

            //길드 체크
            if (GuildManager.Instance.guildInfoData.Value == null)
            {
                PopupManager.Instance.ShowAlarmMessage($"문파에 가입되어 있어야 합니다.(명성 {getLevel}이상)");
                return;
            }

            if (GuildManager.Instance.HasGuildBuff(buffTableData.Id))
            {
                BuffGetRoutine();
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage($"문파의 명성이 부족합니다.(명성 {getLevel}이상)");
            }
        }
        else if (buffTableData.BUFFTYPEENUM == BuffTypeEnum.OneYear
            || buffTableData.BUFFTYPEENUM == BuffTypeEnum.Chuseok
            || buffTableData.BUFFTYPEENUM == BuffTypeEnum.Month
            || buffTableData.BUFFTYPEENUM == BuffTypeEnum.Cold
            || buffTableData.BUFFTYPEENUM == BuffTypeEnum.Winter
            
            )
        {
            BuffGetRoutine();
        }
    }

    private void BuffGetRoutine()
    {
        if (ServerData.userInfoTable.GetTableData(buffTableData.Stringid).Value >= buffTableData.Usecount)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "중복요청", null);
            return;
        }

        //추석 유료버프
        if (buffTableData.Stringid.Equals("ch2"))
        {
            if (ServerData.iapServerTable.TableDatas[UiChildPassBuyButton.PassKey].buyCount.Value == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("한가위 패스권이 필요 합니다.");
                return;
            }

        }

        
        var serverTime = ServerData.userInfoTable.currentServerTime;
        
        //월간패스 유료버프
        if (buffTableData.Stringid.Equals("ma11"))
        {
            // #if UNITY_EDITOR
            // string description = $"{serverTime.Month + 1}월 월간 패스권이 필요합니다.";
            // #else
            //#endif
                string description = $"{serverTime.Month}월 월간 패스권이 필요 합니다.";
            if (ServerData.userInfoTable.IsMonthlyPass2()) 
            {
                if (ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton2.monthPassKey].buyCount.Value == 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(description);
                    return;
                }
            }
            else 
            {
                if (ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton.monthPassKey].buyCount.Value == 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(description);
                    return;
                }
            }
        }

        //패스 유료버프
        if (buffTableData.Stringid.Equals("season1"))
        {
            if (ServerData.iapServerTable.TableDatas[UiSeasonPassBuyButton.seasonPassKey].buyCount.Value == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("여름 훈련 패스권이 필요 합니다.");
                return;
            }

        }
        //혹한기 패스 유료버프
        if (buffTableData.Stringid.Equals("season3"))
        {
            if (ServerData.iapServerTable.TableDatas[UiColdSeasonPassBuyButton.seasonPassKey].buyCount.Value == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("봄 훈련 패스권이 필요 합니다.");
                return;
            }
        }
        //2023~24 겨울훈련 유료버프
        if (buffTableData.Stringid.Equals("winter1"))
        {
            if (ServerData.iapServerTable.TableDatas[UiTrainingPassBuyButton.productKey].buyCount.Value == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("훈련 패스권이 필요 합니다.");
                return;
            }
        }
        ServerData.userInfoTable.GetTableData(buffTableData.Stringid).Value++;
        ServerData.buffServerTable.TableDatas[buffTableData.Stringid].remainSec.Value += buffTableData.Buffseconds;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();

        userInfoParam.Add(buffTableData.Stringid, ServerData.userInfoTable.GetTableData(buffTableData.Stringid).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        Param buffParam = new Param();

        buffParam.Add(buffTableData.Stringid, ServerData.buffServerTable.TableDatas[buffTableData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(BuffServerTable.tableName, BuffServerTable.Indate, buffParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              //LogManager.Instance.SendLog("버프 획득", $"{buffTableData.Stringid}");
          });
    }

    private void BuffEndCheck()
    {
        var severTime = ServerData.userInfoTable.currentServerTime;


        //봄훈련 무료버프
        if (buffTableData.Stringid.Equals("season2"))
        {
            if (severTime.Month >= 4)
            {
                this.gameObject.SetActive(false);
                return;
            }
        }
        //봄훈련 유료버프
        else if (buffTableData.Stringid.Equals("season3"))
        {
            if (severTime.Month >= 4)
            {
                this.gameObject.SetActive(false);
                return;
            }            
        }
    }
}
