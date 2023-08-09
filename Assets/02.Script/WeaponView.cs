using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Coffee.UIEffects;

public class WeaponView : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image weaponIcon;

    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private TextMeshProUGUI lvText;

    [SerializeField]
    private TextMeshProUGUI gradeNumText;

    [SerializeField]
    private GameObject gradeNumObject;

    private WeaponData weaponData;
    private MagicBookData magicBookData;
    private SkillTableData skillData;
    private NewGachaTableData newGachaData;
    private SealSwordData sealSwordData;

    private bool initialized = false;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private GameObject weaponMagicBookObject;

    [SerializeField]
    private GameObject skillObject;

    [SerializeField]
    private UIShiny uishiny;

    [SerializeField]
    private bool IsGachaCell = false;


    public void Initialize(WeaponData weaponData, MagicBookData magicBookData, SkillTableData skillData = null, NewGachaTableData newGachaData = null, SealSwordData sealSwordData = null)
    {
        weaponMagicBookObject.SetActive(skillData == null);

        skillObject.SetActive(skillData != null);

        this.weaponData = weaponData;
        this.magicBookData = magicBookData;
        this.skillData = skillData;
        this.newGachaData = newGachaData;
        this.sealSwordData = sealSwordData;

        int grade = 0;
        int id = 0;

        if (weaponData != null)
        {
            grade = weaponData.Grade;
            id = weaponData.Id;
            weaponIcon.sprite = CommonResourceContainer.GetWeaponSprite(id);
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_Weapon[grade]);
        }
        else if (magicBookData != null)
        {
            grade = magicBookData.Grade;
            id = magicBookData.Id;
            weaponIcon.sprite = CommonResourceContainer.GetMagicBookSprite(id);
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_Norigae[grade]);
        }
        else if (skillData != null)
        {
            grade = skillData.Skillgrade;
            id = skillData.Id;
            skillIcon.sprite = CommonResourceContainer.GetSkillIconSprite(id);
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_Skill[grade]);
        }
        else if (newGachaData != null)
        {
            grade = newGachaData.Grade;
            id = newGachaData.Id;
            weaponIcon.sprite = CommonResourceContainer.GetNewGachaIconSprite(id);
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_NewGacha[grade]);
        }
        else if (sealSwordData != null)
        {
            grade = sealSwordData.Grade;
            id = sealSwordData.Id;
            weaponIcon.sprite = CommonResourceContainer.GetSealSwordIconSprite(id);
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_SealSword[grade]);
        }

        if (skillData != null || newGachaData != null || sealSwordData != null)
        {
            lvText.gameObject.SetActive(false);
        }
        else
        {
            lvText.gameObject.SetActive(true);
        }

        this.gradeText.color = (CommonUiContainer.Instance.itemGradeColor[grade]);


        int gradeText = 4 - (id % 4);

        if (magicBookData != null)
        {
            if (id <= 19)
            {
                if (magicBookData.Id / 4 != 4)
                {
                    gradeNumText.SetText($"{gradeText}등급");
                }
                else
                {
                    int remain = magicBookData.Id % 4;

                    if (remain == 0)
                    {
                        gradeNumText.SetText($"<color=green>현무</color>");
                    }
                    else if (remain == 1)
                    {
                        gradeNumText.SetText($"<color=white>백호</color>");
                    }
                    else if (remain == 2)
                    {
                        gradeNumText.SetText($"<color=red>주작</color>");
                    }
                    else if (remain == 3)
                    {
                        gradeNumText.SetText($"<color=blue>청룡</color>");
                    }
                }

                if (gradeNumObject != null)
                {
                    gradeNumObject.SetActive(true);
                }
            }
            else
            {
                if (gradeNumObject != null)
                {
                    gradeNumObject.SetActive(false);
                }
            }
        }
        //무기
        else if (weaponData != null)
        {
            if (id <= 19)
            {
                if (gradeNumObject != null)
                {
                    gradeNumObject.SetActive(true);
                }

                gradeNumText.SetText($"{gradeText}등급");
            }
            else
            {
                if (gradeNumObject != null)
                {
                    gradeNumObject.SetActive(false);
                }
            }
        }
        //스킬
        else if (skillData != null)
        {
            if (id <= 14)
            {
                gradeNumText.SetText($"{gradeText}등급");
                if (gradeNumObject != null)
                {
                    gradeNumObject.SetActive(true);
                }
            }
            else
            {
                if (gradeNumObject != null)
                {
                    gradeNumObject.SetActive(false);
                }
            }
        }
        // 신규장비
        else
        {
            gradeNumText.SetText($"{gradeText}등급");
        }

        bg.sprite = CommonUiContainer.Instance.itemGradeFrame[grade];

        if (IsGachaCell == false)
        {
            if (weaponData != null)
            {
                SubscribeWeapon();
            }
            else if (magicBookData != null)
            {
                SubscribeMagicBook();
            }
            else if (skillData != null)
            {
                SubscribeSkill();
            }
            else if (newGachaData != null)
            {
                SubscribeNewGacha();
            }
            else
            {
                SubscribeSealSword();
            }
        }

        //uishiny.width = ((float)grade / 3f) * 0.6f;
        uishiny.brightness = ((float)grade / 3f) * 0.8f;
    }


    private void SubscribeWeapon()
    {
        disposable.Clear();

        ServerData.weaponTable.TableDatas[weaponData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        ServerData.weaponTable.TableDatas[weaponData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    private void SubscribeMagicBook()
    {
        disposable.Clear();

        ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);
    }

    private void SubscribeSkill()
    {
        disposable.Clear();

        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);
    }


    private void SubscribeNewGacha()
    {
        disposable.Clear();

        ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);
    }

    private void SubscribeSealSword()
    {
        disposable.Clear();

        ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);
    }

    private void WhenLevelChanged(int level)
    {
        lvText.SetText($"Lv.{level}");
    }

    private void WhenCountChanged(int amount)
    {
            UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        if (weaponData != null)
        {
            int require = weaponData.Id < 20 ? weaponData.Requireupgrade : 1;
            amountText.SetText($"({ServerData.weaponTable.GetCurrentWeaponCount(weaponData.Stringid)}/{require})");
        }
        else if (magicBookData != null)
        {
            int require = magicBookData.Id < 16 ? magicBookData.Requireupgrade : 1;
            amountText.SetText($"({ServerData.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid)}/{require})");
        }
        else if (skillData != null)
        {
            int skillAwakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillData.Id].Value;
            int requireNum = skillAwakeNum == 0 ? 1 : 10;
            amountText.SetText($"({ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].Value}/{requireNum})");
        }
        else if (newGachaData != null)
        {
            int require = newGachaData.Requireupgrade;
            amountText.SetText($"({ServerData.newGachaServerTable.GetCurrentNewGachaCount(newGachaData.Stringid)}/{require})");
        }
        else if (sealSwordData != null)
        {
            int require = sealSwordData.Requireupgrade;
            amountText.SetText($"({ServerData.sealSwordServerTable.GetCurrentWeaponCount(sealSwordData.Stringid)}/{require})");
        }
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}