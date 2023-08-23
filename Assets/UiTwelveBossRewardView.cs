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

public class UiTwelveBossRewardView : MonoBehaviour
{
    private TwelveBossRewardInfo rewardInfo;

    public TwelveBossRewardInfo RewardInfo => rewardInfo;

    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private Button rewardButton;

    [SerializeField] private TextMeshProUGUI rewardButtonDescription;

    [SerializeField] private TextMeshProUGUI rewardAmount;

    [SerializeField] private GameObject rewardLockMask;

    [SerializeField] private TextMeshProUGUI lockDescription;

    [SerializeField] private TextMeshProUGUI gradeText;

    private BossServerData bossServerData;

    [SerializeField] private GameObject rewardedIcon;

    [FormerlySerializedAs("descriptObject")] [SerializeField] private GameObject skillObject;
    [SerializeField] private GameObject costumeObject;
    [SerializeField] private GameObject magicbookObject;

    [SerializeField] private bool showUnlockTextAlways = true;

    private CompositeDisposable disposable = new CompositeDisposable();


    private void OnDestroy()
    {
        disposable.Dispose();
    }

    public void Initialize(TwelveBossRewardInfo rewardInfo, BossServerData bossServerData)
    {
        this.rewardInfo = rewardInfo;

        this.bossServerData = bossServerData;

        UpdateUi();


        Subscribe();

        SubscribeForGuildTowerReward();
    }

    private void UpdateUi()
    {
        rewardLockMask.SetActive(rewardInfo.currentDamage < rewardInfo.damageCut);

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)rewardInfo.rewardType);

        itemDescription.SetText($"{CommonString.GetItemName((Item_Type)rewardInfo.rewardType)}");

        rewardAmount.SetText($"{Utils.ConvertBigNum(rewardInfo.rewardAmount)}개");

        lockDescription.SetText($"{Utils.ConvertBigNumForRewardCell(rewardInfo.damageCut)}에 해금");


        if (skillObject != null && (rewardInfo.rewardType == (int)Item_Type.VisionSkill0 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill1 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill2 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill3 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill4 ||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill5||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill6||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill7||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill8||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill9||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill10||
                                    rewardInfo.rewardType == (int)Item_Type.VisionSkill11
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
        }
        if (magicbookObject != null && (rewardInfo.rewardType == (int)Item_Type.RecommendNorigae0)
           )
        {
            magicbookObject.SetActive(true);
        }
        else
        {
            if (magicbookObject != null)
            {
                magicbookObject.SetActive(false);
            }
        }
        if (costumeObject != null && (rewardInfo.rewardType == (int)Item_Type.costume149
            )
           )
        {
            costumeObject.SetActive(true);
        }
        else
        {
            if (costumeObject != null)
            {
                costumeObject.SetActive(false);
            }
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
                    if (UiGuildBossView.Instance != null && UiGuildBossView.Instance.rewardGrade < rewardInfo.idx + 1)
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

    private void SubscribeForGuildTowerReward()
    {
        if (bossServerData.idx == 117)
        {
            bossServerData.score.AsObservable().Subscribe(e =>
            {
                if (string.IsNullOrEmpty(e) == false && double.TryParse(e, out var score))
                {
                    rewardInfo.currentDamage = score;
                }

                UpdateUi();
            }).AddTo(this);
        }
    }

    private void Subscribe()
    {
        disposable.Clear();

        bossServerData.rewardedId.AsObservable().Subscribe(e => { ResetUi(e); }).AddTo(disposable);
    }

    private void ResetUi(string _rewardedId)
    {
        var rewards = _rewardedId.Split(BossServerTable.rewardSplit).ToList();

        bool rewarded = rewards.Contains(rewardInfo.idx.ToString());

        if (IsRegainableItem() == false)
        {
            rewardButtonDescription.SetText(rewarded ? "완료" : "받기");

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

                rewardButtonDescription.SetText(rewarded && hasGoods ? "완료" : "받기");

                rewardedIcon.SetActive(rewarded && hasGoods);
            }
            else
            {
                rewardButtonDescription.SetText(rewarded ? "완료" : "받기");

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

        rewardButton.interactable = false;


        if (type == Item_Type.DogPet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet18"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet18", ServerData.petTable.TableDatas["pet18"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼목구 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunMaPet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet19"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet19", ServerData.petTable.TableDatas["pet19"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천마 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet20"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet20", ServerData.petTable.TableDatas["pet20"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet21"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet21", ServerData.petTable.TableDatas["pet21"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "고양이 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet22"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet22", ServerData.petTable.TableDatas["pet22"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천둥오리 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet23"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet23", ServerData.petTable.TableDatas["pet23"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "근두운 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //////////////사흉
        else if (type == Item_Type.SahyungPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet28"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet28", ServerData.petTable.TableDatas["pet28"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사흉수 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SahyungPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet29"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet29", ServerData.petTable.TableDatas["pet29"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사흉수 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SahyungPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet30"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet30", ServerData.petTable.TableDatas["pet30"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사흉수 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SahyungPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet31"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet31", ServerData.petTable.TableDatas["pet31"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사흉수 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        ///
        else if (type == Item_Type.VisionPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet32"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet32", ServerData.petTable.TableDatas["pet32"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사룡환수 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.VisionPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet33"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet33", ServerData.petTable.TableDatas["pet33"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사룡 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.VisionPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet34"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet34", ServerData.petTable.TableDatas["pet34"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사룡 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.VisionPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet35"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet35", ServerData.petTable.TableDatas["pet35"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사룡 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.FoxPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet36"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet36", ServerData.petTable.TableDatas["pet36"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.FoxPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet37"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet37", ServerData.petTable.TableDatas["pet37"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.FoxPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet38"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet38", ServerData.petTable.TableDatas["pet38"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.FoxPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet39"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet39", ServerData.petTable.TableDatas["pet39"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.TigerPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet40"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet40", ServerData.petTable.TableDatas["pet40"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.TigerPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet41"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet41", ServerData.petTable.TableDatas["pet41"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.TigerPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet42"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet42", ServerData.petTable.TableDatas["pet42"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.TigerPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet43"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet43", ServerData.petTable.TableDatas["pet43"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunGuPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet44"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet44", ServerData.petTable.TableDatas["pet44"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunGuPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet45"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet45", ServerData.petTable.TableDatas["pet45"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunGuPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet46"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet46", ServerData.petTable.TableDatas["pet46"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunGuPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet47"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet47", ServerData.petTable.TableDatas["pet47"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 펫 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p14"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p14", ServerData.suhoAnimalServerTable.TableDatas["p14"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p15"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p15", ServerData.suhoAnimalServerTable.TableDatas["p15"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p16"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p16", ServerData.suhoAnimalServerTable.TableDatas["p16"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p17"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p17", ServerData.suhoAnimalServerTable.TableDatas["p17"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p20"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p20", ServerData.suhoAnimalServerTable.TableDatas["p20"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p21"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p21", ServerData.suhoAnimalServerTable.TableDatas["p21"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p22"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p22", ServerData.suhoAnimalServerTable.TableDatas["p22"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p23"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p23", ServerData.suhoAnimalServerTable.TableDatas["p23"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p27"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p27", ServerData.suhoAnimalServerTable.TableDatas["p27"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet9)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p28"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p28", ServerData.suhoAnimalServerTable.TableDatas["p28"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet10)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();
        
            ServerData.suhoAnimalServerTable.TableDatas["p30"].hasItem.Value = 1;
        
            Param petParam = new Param();
        
            petParam.Add("p30", ServerData.suhoAnimalServerTable.TableDatas["p30"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate, petParam));
        
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SpecialSuhoPet11)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.suhoAnimalServerTable.TableDatas["p31"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("p31", ServerData.suhoAnimalServerTable.TableDatas["p31"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(SuhoAnimalServerTable.tableName, SuhoAnimalServerTable.Indate,
                petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 수호동물 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        // }
        /////////////////
        else if (type == Item_Type.DogNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook27"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook27"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook27", ServerData.magicBookTable.TableDatas["magicBook27"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼목구 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MihoNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook28"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook28"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook28", ServerData.magicBookTable.TableDatas["magicBook28"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunMaNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook29"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook29"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook29", ServerData.magicBookTable.TableDatas["magicBook29"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천마 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume39)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume39"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume39", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼목구 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume53)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume53"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume53", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "해원맥 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.costume56)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume56"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume56", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume57)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume57"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume57", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume58)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume58"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume58", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.costume59)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume59"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume59", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume60)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume60"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume60", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume61)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume61"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume61", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "단풍 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume62)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume62"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume62", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강시 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume63)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume63"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume63", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강시 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume64)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume64"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume64", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume65)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume65"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume65", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.costume66)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume66"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume66", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "혈량 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume67)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume67"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume67", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "뇌량 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume68)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume68"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume68", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "암량 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.costume69)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume69"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume69", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "12 월간 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //도깨비
        else if (type == Item_Type.costume70)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume70"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume70", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "화량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume71)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume71"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume71", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "설량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume72)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume72"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume72", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "크리스마스 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume73)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume73"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume73", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "핑크 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.costume74)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume74"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume74", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "미량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume75)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume75"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume75", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "흑량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume78)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume78"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume78", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "남량 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume79)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume79"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume79", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여량 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume80)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume80"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume80", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "우량 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume81)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume81"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume81", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "지국천왕 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume82)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume82"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume82", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광목천왕 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume84)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume84"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume84", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "증장천왕 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume85)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume85"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume85", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "다문천왕 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume86)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume86"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume86", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "그림자 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume87)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume87"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume87", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아수라 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume88)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume88"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume88", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "인드라 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume89)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume89"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume89", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아드라 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume92)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume92"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume92", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "일지매 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume93)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume93"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume93", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "임꺽정 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume94)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume94"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume94", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "전우치 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume95)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume95"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume95", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "홍길동 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        //
        else if (type == Item_Type.costume96)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume96"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume96", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닌자 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume97)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume97"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume97", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닌자 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.costume98)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume98"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume98", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닌자 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume101)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume101"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume101", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume102)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume102"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume102", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume103)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume103"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume103", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume104)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume104"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume104", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
//
        else if (type == Item_Type.costume105)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume105"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume105", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume106)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume106"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume106", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume107)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume107"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume107", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume108)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume108"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume108", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
//
        else if (type == Item_Type.costume110)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume110"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume110", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume111)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume111"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume111", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume112)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume112"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume112", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume113)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume113"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume113", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume116)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume116"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume116", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume117)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume117"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume117", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume118)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume118"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume118", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume120)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume120"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume120", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume121)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume121"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume121", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume122)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume122"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume122", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume123)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume123"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume123", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume124)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume124"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume124", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume125)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume125"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume125", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume126)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume126"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume126", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume127)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume127"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume127", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume128)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume128"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume128", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume129)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume129"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume129", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume130)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume130"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume130", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume132)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume132"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume132", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume133)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume133"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume133", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume134)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume134"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume134", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume135)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume135"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume135", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume138)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume138"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume138", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume139)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume139"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume139", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume140)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume140"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume140", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume141)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume141"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume141", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume142)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume142"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume142", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume145)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume145"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume145", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume146)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume146"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume146", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume147)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume147"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume147", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume148)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume148"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume148", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume149)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume149"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume149", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"십만 대산 명예 보상 획득 !!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume150)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume150"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume150", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume151)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume151"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume151", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume152)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume152"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume152", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
//
        //
        else if (type == Item_Type.costume51)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume51"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume51", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "월직 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume42)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume42"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume42", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천마호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        ///////////////////
        else if (type == Item_Type.costume36)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume36"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume36", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "달토끼 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume47)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume47"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume47", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여래 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume48)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume48"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume48", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강림 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RabitPet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet17"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet17", ServerData.petTable.TableDatas["pet17"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "달토끼 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RabitNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook26"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook26"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook26", ServerData.magicBookTable.TableDatas["magicBook26"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "달토끼 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.YeaRaeNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook32"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook32"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook32", ServerData.magicBookTable.TableDatas["magicBook32"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여래 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.GangrimNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook33"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook33"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook33", ServerData.magicBookTable.TableDatas["magicBook33"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강림 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.ChunNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook35"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook35"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook35", ServerData.magicBookTable.TableDatas["magicBook35"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook36"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook36"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook36", ServerData.magicBookTable.TableDatas["magicBook36"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook37"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook37"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook37", ServerData.magicBookTable.TableDatas["magicBook37"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.ChunNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook38"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook38"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook38", ServerData.magicBookTable.TableDatas["magicBook38"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunNorigae4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook39"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook39"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook39", ServerData.magicBookTable.TableDatas["magicBook39"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //////
        else if (type == Item_Type.ChunNorigae5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook40"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook40"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook40", ServerData.magicBookTable.TableDatas["magicBook40"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunNorigae6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook41"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook41"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook41", ServerData.magicBookTable.TableDatas["magicBook41"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        /////
        else if (type == Item_Type.DokebiNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook42"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook42"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook42", ServerData.magicBookTable.TableDatas["magicBook42"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "혈량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook43"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook43"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook43", ServerData.magicBookTable.TableDatas["magicBook43"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "뇌량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.DokebiNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook44"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook44"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook44", ServerData.magicBookTable.TableDatas["magicBook44"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "암량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook46"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook46"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook46", ServerData.magicBookTable.TableDatas["magicBook46"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "화량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook47"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook47"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook47", ServerData.magicBookTable.TableDatas["magicBook47"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "설량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook48"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook48"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook48", ServerData.magicBookTable.TableDatas["magicBook48"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "미량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook49"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook49"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook49", ServerData.magicBookTable.TableDatas["magicBook49"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "흑량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook51"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook51"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook51", ServerData.magicBookTable.TableDatas["magicBook51"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "남량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook52"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook52"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook52", ServerData.magicBookTable.TableDatas["magicBook52"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae9)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook53"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook53"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook53", ServerData.magicBookTable.TableDatas["magicBook53"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "우량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        //
        else if (type == Item_Type.SumisanNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook54"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook54"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook54", ServerData.magicBookTable.TableDatas["magicBook54"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "지국천왕 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.SumisanNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook55"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook55"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook55", ServerData.magicBookTable.TableDatas["magicBook55"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광목천왕 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook57"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook57"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook57", ServerData.magicBookTable.TableDatas["magicBook57"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "증장천왕 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook58"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook58"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook58", ServerData.magicBookTable.TableDatas["magicBook58"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "다문천왕 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanNorigae4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook59"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook59"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook59", ServerData.magicBookTable.TableDatas["magicBook59"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아수라 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanNorigae5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook60"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook60"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook60", ServerData.magicBookTable.TableDatas["magicBook60"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "인드라 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanNorigae6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook61"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook61"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook61", ServerData.magicBookTable.TableDatas["magicBook61"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아드라 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
//////////////////
        else if (type == Item_Type.ThiefNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook63"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook63"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook63", ServerData.magicBookTable.TableDatas["magicBook63"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "일지매 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ThiefNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook64"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook64"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook64", ServerData.magicBookTable.TableDatas["magicBook64"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "임꺽정 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ThiefNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook65"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook65"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook65", ServerData.magicBookTable.TableDatas["magicBook65"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "전우치 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ThiefNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook66"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook66"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook66", ServerData.magicBookTable.TableDatas["magicBook66"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "홍길동 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
//
        else if (type == Item_Type.NinjaNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook67"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook67"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook67", ServerData.magicBookTable.TableDatas["magicBook67"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닌자 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.NinjaNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook68"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook68"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook68", ServerData.magicBookTable.TableDatas["magicBook68"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닌자 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.NinjaNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook69"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook69"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook69", ServerData.magicBookTable.TableDatas["magicBook69"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "닌자 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
//
        else if (type == Item_Type.KingNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook71"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook71"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook71", ServerData.magicBookTable.TableDatas["magicBook71"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.KingNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook72"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook72"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook72", ServerData.magicBookTable.TableDatas["magicBook72"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.KingNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook73"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook73"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook73", ServerData.magicBookTable.TableDatas["magicBook73"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.KingNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook74"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook74"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook74", ServerData.magicBookTable.TableDatas["magicBook74"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        ////////////
        else if (type == Item_Type.DarkNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook76"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook76"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook76", ServerData.magicBookTable.TableDatas["magicBook76"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.DarkNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook77"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook77"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook77", ServerData.magicBookTable.TableDatas["magicBook77"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.DarkNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook78"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook78"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook78", ServerData.magicBookTable.TableDatas["magicBook78"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.DarkNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook79"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook79"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook79", ServerData.magicBookTable.TableDatas["magicBook79"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MasterNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook75"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook75"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook75", ServerData.magicBookTable.TableDatas["magicBook75"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MasterNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook81"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook81"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook81", ServerData.magicBookTable.TableDatas["magicBook81"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MasterNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook82"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook82"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook82", ServerData.magicBookTable.TableDatas["magicBook82"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MasterNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook83"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook83"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook83", ServerData.magicBookTable.TableDatas["magicBook83"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.SinsunNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook84"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook84"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook84", ServerData.magicBookTable.TableDatas["magicBook84"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.SinsunNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook85"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook85"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook85", ServerData.magicBookTable.TableDatas["magicBook85"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.SinsunNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook86"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook86"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook86", ServerData.magicBookTable.TableDatas["magicBook86"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.SinsunNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook88"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook88"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook88", ServerData.magicBookTable.TableDatas["magicBook88"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.SinsunNorigae4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook89"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook89"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook89", ServerData.magicBookTable.TableDatas["magicBook89"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SinsunNorigae5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook90"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook90"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook90", ServerData.magicBookTable.TableDatas["magicBook90"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SinsunNorigae6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook91"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook91"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook91", ServerData.magicBookTable.TableDatas["magicBook91"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SinsunNorigae7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook92"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook92"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook92", ServerData.magicBookTable.TableDatas["magicBook92"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SinsunNorigae8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook93"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook93"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook93", ServerData.magicBookTable.TableDatas["magicBook93"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook104"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook104"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook104", ServerData.magicBookTable.TableDatas["magicBook104"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"십만 대산 명예 보상 획득 !!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.HyunSangNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook95"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook95"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook95", ServerData.magicBookTable.TableDatas["magicBook95"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.HyunSangNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook96"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook96"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook96", ServerData.magicBookTable.TableDatas["magicBook96"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook97"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook97"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook97", ServerData.magicBookTable.TableDatas["magicBook97"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook98"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook98"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook98", ServerData.magicBookTable.TableDatas["magicBook98"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangNorigae4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook99"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook99"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook99", ServerData.magicBookTable.TableDatas["magicBook99"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangNorigae5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook100"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook100"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook100", ServerData.magicBookTable.TableDatas["magicBook100"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangNorigae6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook102"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook102"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook102", ServerData.magicBookTable.TableDatas["magicBook102"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangNorigae7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook103"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook103"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook103", ServerData.magicBookTable.TableDatas["magicBook103"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangNorigae8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook105"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook105"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook105", ServerData.magicBookTable.TableDatas["magicBook105"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 노리개 획득!!", null);
            });
        }

        //////
        //
        else if (type == Item_Type.DokebiHorn0)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[0];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn1)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[1];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn2)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[2];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn3)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[3];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn4)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[4];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn5)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[5];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn6)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[6];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }

        //

        else if (type == Item_Type.DokebiHorn7)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[7];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn8)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[8];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn9)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[9];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value +=
                $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }

        //
        else if (type == Item_Type.NataWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon27"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon27"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon27", ServerData.weaponTable.TableDatas["weapon27"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "나타의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.YeaRaeWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon33"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon33"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon33", ServerData.weaponTable.TableDatas["weapon33"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여래의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.GangrimWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon34"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon34"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon34", ServerData.weaponTable.TableDatas["weapon34"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강림검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.HaeWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon36"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon36"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon36", ServerData.weaponTable.TableDatas["weapon36"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사인검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.OrochiWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon28"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon28"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon28", ServerData.weaponTable.TableDatas["weapon28"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오로치의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MihoWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon30"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon30"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon30", ServerData.weaponTable.TableDatas["weapon30"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon43"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon43"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon43", ServerData.weaponTable.TableDatas["weapon43"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "태양검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon44"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon44"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon44", ServerData.weaponTable.TableDatas["weapon44"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "달의검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon50"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon50"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon50", ServerData.weaponTable.TableDatas["weapon50"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "번개검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon51"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon51"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon51", ServerData.weaponTable.TableDatas["weapon51"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "구름검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //--------------------------------------
        else if (type == Item_Type.DokebiWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon57"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon57"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon57", ServerData.weaponTable.TableDatas["weapon57"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "혈량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon58"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon58"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon58", ServerData.weaponTable.TableDatas["weapon58"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "뇌량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon59"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon59"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon59", ServerData.weaponTable.TableDatas["weapon59"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "암량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon63"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon63"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon63", ServerData.weaponTable.TableDatas["weapon63"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "화량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon64"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon64"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon64", ServerData.weaponTable.TableDatas["weapon64"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "설량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon65"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon65"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon65", ServerData.weaponTable.TableDatas["weapon65"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "미량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon66"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon66"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon66", ServerData.weaponTable.TableDatas["weapon66"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "흑량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        ///
        else if (type == Item_Type.DokebiWeapon7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon77"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon77"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon77", ServerData.weaponTable.TableDatas["weapon77"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "남량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon78"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon78"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon78", ServerData.weaponTable.TableDatas["weapon78"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon9)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon79"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon79"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon79", ServerData.weaponTable.TableDatas["weapon79"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "우량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        /////
        else if (type == Item_Type.SumisanWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon80"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon80"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon80", ServerData.weaponTable.TableDatas["weapon80"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "지국천왕 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon84"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon84"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon84", ServerData.weaponTable.TableDatas["weapon84"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광목천왕 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon85"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon85"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon85", ServerData.weaponTable.TableDatas["weapon85"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "증장천왕 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon86"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon86"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon86", ServerData.weaponTable.TableDatas["weapon86"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "다문천왕 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanWeapon4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon87"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon87"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon87", ServerData.weaponTable.TableDatas["weapon87"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아수라 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanWeapon5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon88"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon88"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon88", ServerData.weaponTable.TableDatas["weapon88"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "인드라 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanWeapon6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon89"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon89"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon89", ServerData.weaponTable.TableDatas["weapon89"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아드라 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ThiefWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon95"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon95"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon95", ServerData.weaponTable.TableDatas["weapon95"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "일지매 검 획득!!", null);
            });
        }
        else if (type == Item_Type.ThiefWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon96"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon96"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon96", ServerData.weaponTable.TableDatas["weapon96"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "임꺽정 검 획득!!", null);
            });
        }
        else if (type == Item_Type.ThiefWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon97"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon97"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon97", ServerData.weaponTable.TableDatas["weapon97"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "전우치 검 획득!!", null);
            });
        }
        else if (type == Item_Type.ThiefWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon98"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon98"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon98", ServerData.weaponTable.TableDatas["weapon98"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "홍길동 검 획득!!", null);
            });
        }
        ////////////
        else if (type == Item_Type.NinjaWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon99"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon99"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon99", ServerData.weaponTable.TableDatas["weapon99"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"닌자검 획득!!", null);
            });
        }
        else if (type == Item_Type.NinjaWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon100"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon100"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon100", ServerData.weaponTable.TableDatas["weapon100"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"닌자검 획득!!", null);
            });
        }
        else if (type == Item_Type.NinjaWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon101"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon101"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon101", ServerData.weaponTable.TableDatas["weapon101"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"닌자검 획득!!", null);
            });
        }
        //
        else if (type == Item_Type.KingWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon104"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon104"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon104", ServerData.weaponTable.TableDatas["weapon104"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.KingWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon105"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon105"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon105", ServerData.weaponTable.TableDatas["weapon105"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.KingWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon106"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon106"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon106", ServerData.weaponTable.TableDatas["weapon106"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.KingWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon107"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon107"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon107", ServerData.weaponTable.TableDatas["weapon107"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        ///
        else if (type == Item_Type.DarkWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon109"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon109"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon109", ServerData.weaponTable.TableDatas["weapon109"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.DarkWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon110"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon110"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon110", ServerData.weaponTable.TableDatas["weapon110"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.DarkWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon111"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon111"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon111", ServerData.weaponTable.TableDatas["weapon111"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.DarkWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon112"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon112"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon112", ServerData.weaponTable.TableDatas["weapon112"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.MasterWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon108"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon108"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon108", ServerData.weaponTable.TableDatas["weapon108"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.MasterWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon113"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon113"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon113", ServerData.weaponTable.TableDatas["weapon113"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.MasterWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon114"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon114"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon114", ServerData.weaponTable.TableDatas["weapon114"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.MasterWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon115"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon115"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon115", ServerData.weaponTable.TableDatas["weapon115"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon116"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon116"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon116", ServerData.weaponTable.TableDatas["weapon116"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon117"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon117"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon117", ServerData.weaponTable.TableDatas["weapon117"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon118"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon118"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon118", ServerData.weaponTable.TableDatas["weapon118"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon119"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon119"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon119", ServerData.weaponTable.TableDatas["weapon119"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon120"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon120"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon120", ServerData.weaponTable.TableDatas["weapon120"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon121"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon121"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon121", ServerData.weaponTable.TableDatas["weapon121"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon122"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon122"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon122", ServerData.weaponTable.TableDatas["weapon122"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon123"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon123"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon123", ServerData.weaponTable.TableDatas["weapon123"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.SinsunWeapon8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon124"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon124"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon124", ServerData.weaponTable.TableDatas["weapon124"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon125"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon125"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon125", ServerData.weaponTable.TableDatas["weapon125"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon126"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon126"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon126", ServerData.weaponTable.TableDatas["weapon126"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon127"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon127"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon127", ServerData.weaponTable.TableDatas["weapon127"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon128"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon128"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon128", ServerData.weaponTable.TableDatas["weapon128"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon129"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon129"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon129", ServerData.weaponTable.TableDatas["weapon129"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon130"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon130"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon130", ServerData.weaponTable.TableDatas["weapon130"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon132"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon132"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon132", ServerData.weaponTable.TableDatas["weapon132"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon133"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon133"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon133", ServerData.weaponTable.TableDatas["weapon133"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        else if (type == Item_Type.HyunSangWeapon8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon134"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon134"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon134", ServerData.weaponTable.TableDatas["weapon134"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{localTableData.Name} 무기 획득!!", null);
            });
        }
        ////////////
        else if (type == Item_Type.SahyungWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon91"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon91"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon91", ServerData.weaponTable.TableDatas["weapon91"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도철 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SahyungWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon92"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon92"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon92", ServerData.weaponTable.TableDatas["weapon92"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도올 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SahyungWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon93"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon93"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon93", ServerData.weaponTable.TableDatas["weapon93"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "혼돈 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SahyungWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon94"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon94"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon94", ServerData.weaponTable.TableDatas["weapon94"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "궁기 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //--------------------------------------
        else if (type == Item_Type.RecommendWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon45"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon45"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon45", ServerData.weaponTable.TableDatas["weapon45"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon46"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon46"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon46", ServerData.weaponTable.TableDatas["weapon46"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon47"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon47"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon47", ServerData.weaponTable.TableDatas["weapon47"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon48"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon48"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon48", ServerData.weaponTable.TableDatas["weapon48"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon49"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon49"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon49", ServerData.weaponTable.TableDatas["weapon49"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon52"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon52"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon52", ServerData.weaponTable.TableDatas["weapon52"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon53"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon53"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon53", ServerData.weaponTable.TableDatas["weapon53"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon54"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon54"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon54", ServerData.weaponTable.TableDatas["weapon54"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon55"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon55"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon55", ServerData.weaponTable.TableDatas["weapon55"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon9)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon56"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon56"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon56", ServerData.weaponTable.TableDatas["weapon56"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon10)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon60"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon60"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon60", ServerData.weaponTable.TableDatas["weapon60"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon11)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon61"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon61"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon61", ServerData.weaponTable.TableDatas["weapon61"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon12)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon62"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon62"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon62", ServerData.weaponTable.TableDatas["weapon62"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //=======================================================================================
        else if (type == Item_Type.RecommendWeapon13)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon71"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon71"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon71", ServerData.weaponTable.TableDatas["weapon71"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon14)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon72"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon72"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon72", ServerData.weaponTable.TableDatas["weapon72"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon15)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon73"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon73"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon73", ServerData.weaponTable.TableDatas["weapon73"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon16)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon74"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon74"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon74", ServerData.weaponTable.TableDatas["weapon74"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon17)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon75"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon75"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon75", ServerData.weaponTable.TableDatas["weapon75"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon18)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon76"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon76"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon76", ServerData.weaponTable.TableDatas["weapon76"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.RecommendWeapon19)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon82"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon82"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon82", ServerData.weaponTable.TableDatas["weapon82"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon20)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon83"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon83"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon83", ServerData.weaponTable.TableDatas["weapon83"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon21)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon102"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon102"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon102", ServerData.weaponTable.TableDatas["weapon102"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon22)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon103"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon103"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon103", ServerData.weaponTable.TableDatas["weapon103"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //=======================================================================================

        //----------------------------------------
        else if (type == Item_Type.Kirin_Pet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet16"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet16", ServerData.petTable.TableDatas["pet16"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "기린 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.KirinNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook25"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook25"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook25", ServerData.magicBookTable.TableDatas["magicBook25"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "기린 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.IndraWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon26"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon26"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon26", ServerData.weaponTable.TableDatas["weapon26"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "인드라의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.Sam_Pet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet15"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet15", ServerData.petTable.TableDatas["pet15"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아기 삼족오 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.Sam_Norigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook24"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook24"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook24", ServerData.magicBookTable.TableDatas["magicBook24"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼족오 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.Hae_Pet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet14"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet14", ServerData.petTable.TableDatas["pet14"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아기 해태 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.Hae_Norigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook22"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook22"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook22", ServerData.magicBookTable.TableDatas["magicBook22"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "해태 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
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
                if (rewardButton != null)
                {
                    rewardButton.interactable = true;
                }

                ResetUi(bossServerData.rewardedId.Value);
            });
        }
    }

    public bool GetRewardByScript()
    {
        if (rewardInfo.currentDamage < rewardInfo.damageCut)
        {
            return false;
        }

        var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit).ToList();

        if (rewards.Contains(rewardInfo.idx.ToString()))
        {
            return false;
        }

        Item_Type type = (Item_Type)rewardInfo.rewardType;

        float amount = rewardInfo.rewardAmount;

        bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";
        ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value += amount;

        return true;
    }
}