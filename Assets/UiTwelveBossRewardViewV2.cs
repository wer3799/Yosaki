using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UiTwelveRewardPopup;

public class UiTwelveBossRewardViewV2 : MonoBehaviour
{
    private TwelveBossRewardInfo rewardInfo;


    [SerializeField] private Image bgImage;
    [SerializeField] private Image itemIcon;


    [SerializeField] private TextMeshProUGUI rewardAmount;

    [SerializeField] private GameObject rewardLockMask;

    [SerializeField] private TextMeshProUGUI lockDescription;

    [SerializeField] private TextMeshProUGUI gradeText;

    private BossServerData bossServerData;

    [SerializeField] private GameObject rewardedIcon;

    [SerializeField] private GameObject skillObject;

    [SerializeField] private bool showUnlockTextAlways = true;



    public void Initialize(TwelveBossRewardInfo rewardInfo, BossServerData bossServerData)
    {
        this.rewardInfo = rewardInfo;

        this.bossServerData = bossServerData;

        UpdateUi();


        Subscribe();

    }
    

    private void UpdateUi()
    {
        switch (rewardInfo.rewardColor)
        {
            case RewardColor.None:
                break;
            default:
                bgImage.sprite = CommonUiContainer.Instance.itemGradeFrame[(int)rewardInfo.rewardColor];
                break;
        }

        
        rewardLockMask.SetActive(rewardInfo.currentDamage < rewardInfo.damageCut);

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)rewardInfo.rewardType);

        rewardAmount.SetText($"{Utils.ConvertBigNum(rewardInfo.rewardAmount)}개");

        lockDescription.SetText($"{Utils.ConvertBigNumForRewardCell(rewardInfo.damageCut)}에 해금");


        if (skillObject != null && (rewardInfo.rewardType == (int)Item_Type.VisionSkill0 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill1 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill2 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill3 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill4 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill5 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill6 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill7 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill8 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill9 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill10 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill11 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill12 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill13 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill14 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill15 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill16 ||
                                    ((Item_Type)rewardInfo.rewardType).IsVisionSkill()
            )
           )
        {
            skillObject.SetActive(true);
        }
        else
        {
            if (skillObject != null)
            {
                skillObject.SetActive(false);
            }


            //언락돼도 보이게
            if (showUnlockTextAlways)
            {
                lockDescription.transform.SetParent((this.transform));
            }

            if (gradeText != null)
            {
                gradeText.SetText($"{rewardInfo.idx + 1}단계\n({rewardInfo.idx + 1}점)");

                //문파만
                if (bossServerData.idx == 12)
                {
                    if (rewardInfo.currentDamage >= rewardInfo.damageCut)
                    {
                        if (UiGuildBossView.Instance != null &&
                            UiGuildBossView.Instance.rewardGrade < rewardInfo.idx + 1)
                        {
                            UiGuildBossView.Instance.rewardGrade = rewardInfo.idx + 1;
                        }
                    }

                    var bossTableData = TableManager.Instance.TwelveBossTable.dataArray[20];

                    var bsd = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

                    double currentDamage = 0f;

                    if (string.IsNullOrEmpty(bsd.score.Value) == false)
                    {
                        currentDamage = double.Parse(bsd.score.Value);
                    }

                    if (currentDamage >= rewardInfo.damageCut)
                    {
                        //강철은 구미호와 같은 점수 계산 사용
                        if (UiGangChulView.Instance != null && UiGangChulView.Instance.rewardGrade < rewardInfo.idx + 1)
                        {
                            UiGangChulView.Instance.rewardGrade = rewardInfo.idx + 1;
                        }

                        if (UiGuildBossView.Instance != null &&
                            UiGuildBossView.Instance.rewardGrade_GangChul < rewardInfo.idx + 1)
                        {
                            UiGuildBossView.Instance.rewardGrade_GangChul = rewardInfo.idx + 1;
                        }
                    }
                }
            }
        }
    }



private void Subscribe()
    {
        bossServerData.rewardedId.AsObservable().Subscribe(e => { ResetUi(e); }).AddTo(this);
    }

    private void ResetUi(string _rewardedId)
    {
        var rewards = _rewardedId.Split(BossServerTable.rewardSplit).ToList();

        bool rewarded = rewards.Contains(rewardInfo.idx.ToString());

        if (IsRegainableItem() == false)
        {
            rewardedIcon.SetActive(rewarded);
        }
        else
        {
            Item_Type type = (Item_Type)rewardInfo.rewardType;
            string typeStr = ServerData.goodsTable.ItemTypeToServerString(type);

            bool hasGoods = false;

            if (ServerData.goodsTable.TableDatas.ContainsKey((typeStr)) == true)
            {
                hasGoods = ServerData.goodsTable.TableDatas[typeStr].Value != 0;
                
                rewardedIcon.SetActive(rewarded && hasGoods);
            }
            else
            {
                rewardedIcon.SetActive(rewarded);
            }
        }
    }

    private bool IsRegainableItem()
    {
        Item_Type type = (Item_Type)rewardInfo.rewardType;
        string typeStr = ServerData.goodsTable.ItemTypeToServerString(type);

        //키 잘못된경우
        if (ServerData.goodsTable.TableDatas.ContainsKey((typeStr)) == false)
        {
            //Debug.LogError(($"{type} cast to string is failed"));
            return false;
        }

        return Utils.IsRegainableItem((type));
    }

    public void OnClickGetButton()
    {
        Item_Type type = (Item_Type)rewardInfo.rewardType;

        if (rewardInfo.currentDamage < rewardInfo.damageCut)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 피해량이 부족 합니다.");
            return;
        }


        if (IsRegainableItem() == false)
        {
            var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit).ToList();

            if (rewards.Contains(rewardInfo.idx.ToString()))
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
                return;
            }
        }
        else
        {
            string typeStr = ServerData.goodsTable.ItemTypeToServerString(type);

            bool hasGoods2 = false;

            var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit).ToList();

            if (ServerData.goodsTable.TableDatas.ContainsKey((typeStr)) == true)
            {
                hasGoods2 = ServerData.goodsTable.TableDatas[typeStr].Value != 0;
            }

            if (rewards.Contains(rewardInfo.idx.ToString()))
            {
                if (hasGoods2 == true)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
                    return;
                }
            }
        }



        if (type.IsWeaponItem())
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas[type.ToString()].amount.Value += 1;
            ServerData.weaponTable.TableDatas[type.ToString()].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add(type.ToString(), ServerData.weaponTable.TableDatas[type.ToString()].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(type)} 획득!!",
                    null);
            });
        }
        else if(type.IsCostumeItem()){
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas[type.ToString()];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add(type.ToString(), costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate,
                costumeParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(type)} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type.IsNorigaeItem())
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas[type.ToString()].amount.Value += 1;
            ServerData.magicBookTable.TableDatas[type.ToString()].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add(type.ToString(),
                ServerData.magicBookTable.TableDatas[type.ToString()].ConvertToString());

            transactions.Add(
                TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(type)} 획득!!",
                    null);
            });
        }
        else
        {
            float amount = rewardInfo.rewardAmount;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)amount));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");

                ResetUi(bossServerData.rewardedId.Value);
            });
        }
    }
}