using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiMunhaTowerBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;


    [SerializeField]
    private TextMeshProUGUI gradeText_Current;

    [SerializeField]
    private TextMeshProUGUI gradeText_pref;
    
    [SerializeField]
    private TextMeshProUGUI gradeText_next;
    
    [SerializeField]
    private TextMeshProUGUI unlockDesc;

    [SerializeField]
    private GameObject equipFrame;

    private int currentIdx;
    [SerializeField]
    private TextMeshProUGUI currentFloor;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;
    
    [SerializeField]
    private Toggle towerAutoMode;

    [SerializeField] private Image weaponImage;
    [SerializeField] private Image magicBookImage;
    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic_magicBook;
    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic_weapon;
    
    private bool initialized = false;

    public void Start()
    {
        Subscribe();
        
        currentIdx = Mathf.Max((int)PlayerStats.GetMunhaGrade(),0);

        Initialize(currentIdx);
        
        towerAutoMode.isOn = PlayerPrefs.GetInt(SettingKey.towerAutoMode) == 1;

        initialized = true;
    }


    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaTower].AsObservable().Subscribe(e => { currentFloor.SetText($"{e + 1}단계 입장"); }).AddTo(this);

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).AsObservable().Subscribe(e =>
        {
            var grade = Mathf.Max((int)e, 0);
            var data= TableManager.Instance.StudentTable.dataArray[grade];

            skeletonGraphic.Clear();
            skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[data.Change_Costume];
            skeletonGraphic.Initialize(true);
            skeletonGraphic.SetMaterialDirty();
        }).AddTo(this);

    }
    
    public void OnClickEnterButton()
    {
        int currentIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaTower].Value;

        if (currentIdx >= TableManager.Instance.StudentTower.dataArray.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("도전 완료!");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{currentIdx + 1}단계\n도전 할까요?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.MunhaTower2);

        }, null);
    }
    public void AutoModeOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.towerAutoMode.Value = on ? 1 : 0;
    }
    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.StudentTower.dataArray[idx];

        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Hp)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetMunhaGrade());

        gradeText_Current.SetText($"{tableData.Id+1}단계");

        var addDescription = "";

        abilDescription.SetText($"{CommonString.GetStatusName((StatusType)tableData.Abil_Type)}{tableData.Abil_Value  * 100f}");
        
        //
        int prefIdx = idx - 1;
        if (prefIdx >= 0)
        {
            var prefTableData = TableManager.Instance.StudentTower.dataArray[prefIdx];
            
            gradeText_pref.SetText($"{prefTableData.Id+1}단계");
        }
        else
        {
            gradeText_pref.SetText("없음");
        }
        
        int nextIdx = idx + 1;
        
        if (nextIdx < TableManager.Instance.StudentTower.dataArray.Length)
        {
            var nextTableData = TableManager.Instance.StudentTower.dataArray[nextIdx];
            
            gradeText_next.SetText($"{nextTableData.Id+1}단계");
        }
        else
        {
            gradeText_next.SetText("최고단계");
        }
        
        boneFollowerGraphic_weapon.SetBone("bone14");
        boneFollowerGraphic_magicBook.SetBone("acc");

        var weaponGrade = tableData.Change_Weapon;
        var magicBookGrade = tableData.Change_Magicbook;
        
        weaponImage.sprite = CommonResourceContainer.GetWeaponSprite(weaponGrade);
        magicBookImage.sprite = CommonResourceContainer.GetMagicBookSprite(magicBookGrade);

    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.StudentTower.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.StudentTower.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.StudentTower.dataArray.Length - 1);

        Initialize(currentIdx);
    }
}