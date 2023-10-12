using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BackEnd;

public class UiSkillBoard : SingletonMono<UiSkillBoard>
{
    [SerializeField]
    private UiSkillCell skillCellPrefab;

    [SerializeField]
    private UiSkillCell skillCellPrefab_Sin;

    private List<UiSkillCell> skillCells = new List<UiSkillCell>();

    [SerializeField]
    private Transform skillCellParent;

    [SerializeField]
    private Transform skillCellParent_Sin;

    [SerializeField]
    private Transform skillCellParent_Nata;   
    
    [SerializeField]
    private Transform skillCellParent_Sun;
    
    [SerializeField]
    private Transform skillCellParent_Chun;

    [SerializeField]
    private Transform skillCellParent_Dokebi;

    [SerializeField]
    private Transform skillCellParent_Four;
    [SerializeField]
    private Transform skillCellParent_Thief;
    [SerializeField]
    private Transform skillCellParent_Dark;
    [SerializeField]
    private Transform skillCellParent_Sinsun;

    [SerializeField]
    private UiPassiveSkillCell passiveSkillCellPrefab;

    [SerializeField]
    private UiPassiveSkillCell passiveSkillCellPrefab_Sin;

    [SerializeField]
    private Transform passiveSkillCellParent;

    [SerializeField]
    private Transform passiveSkillCellParent_Sin;

    [SerializeField]
    private UiSkillSlotSettingBoard uiSkillSlotSettingBoard;

    [SerializeField]
    private UiSkillDescriptionPopup uiSkillDescriptionPopup;

    private void Start()
    {
        InitView();
    }

    private void UpdateSkillDescriptionPopup(SkillTableData data)
    {
        uiSkillDescriptionPopup.gameObject.SetActive(true);
        uiSkillDescriptionPopup.Initialize(data);
    }

    private void InitView()
    {
        var skillList = TableManager.Instance.SkillTable.dataArray.ToList();



        //skillList.Sort((a, b) =>
        //{
        //    if (a.Displayorder < b.Displayorder)
        //        return -1;

        //    return 1;
        //});

        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].SKILLCASTTYPE != SkillCastType.Player)
            {
                //Debug.LogError("Hasn't  Player Skill");
                continue;
            }
            if (skillList[i].Id == 18 || skillList[i].Id == 19)
            {
                continue;
            }

            //신선검술
            if (skillList[i].Skilltype == 13)
            {

                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Sinsun);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);
                 
            }    

            //심연검술
            else if (skillList[i].Skilltype == 12)
            {

                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Dark);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);
                 
            }    

            //섬광검술
            else if (skillList[i].Skilltype == 11)
            {

                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Thief);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);
                 
            }    
            //금강검술
            else if (skillList[i].Skilltype == 10)
            {

                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Four);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);
                 
            }    
            //도깨비술
            else if (skillList[i].Skilltype == 8)
            {

                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Dokebi);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);
                 
            }    
            //천계술
            else if (skillList[i].Skilltype == 7)
            {

                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Chun);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);

            }    
            //선술
            else if (skillList[i].Skilltype == 6)
            {
                //나타

                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Sun);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);

            }
            else if (skillList[i].Skilltype == 5)
            {
                //나타
                if (skillList[i].Id == 20 || skillList[i].Id == 21 || skillList[i].Id == 22)
                {
                    var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Nata);

                    cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                    skillCells.Add(cell);
                }
            }
            else if (skillList[i].Skilltype == 4 && skillList[i].Id != 18 && skillList[i].Id != 19)
            {
                var cell = Instantiate<UiSkillCell>(skillCellPrefab_Sin, skillCellParent_Sin);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);
            }
            else
            {
                var cell = Instantiate<UiSkillCell>(skillCellPrefab, skillCellParent);

                cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

                skillCells.Add(cell);
            }


        }

        var passiveSkillList = TableManager.Instance.PassiveSkill.dataArray.ToList();

        for (int i = 0; i < passiveSkillList.Count; i++)
        {
            if (passiveSkillList[i].Issinpassive == false)
            {
                var cell = Instantiate<UiPassiveSkillCell>(passiveSkillCellPrefab, passiveSkillCellParent);

                cell.Refresh(passiveSkillList[i]);
            }
            else
            {
                var cell = Instantiate<UiPassiveSkillCell>(passiveSkillCellPrefab_Sin, passiveSkillCellParent_Sin);

                cell.Refresh(passiveSkillList[i]);
            }
        }
    }

    public void WhenSkillRegistered()
    {
        for (int i = 0; i < skillCells.Count; i++)
        {
            skillCells[i].CheckUnlock(0);
        }
    }

    private void OnCliCkSlotSettingButton(int idx)
    {
        uiSkillSlotSettingBoard.gameObject.SetActive(true);
        uiSkillSlotSettingBoard.SetSkillIdx(idx);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ServerData.statusTable.GetTableData(StatusTable.Skill0_AddValue).Value += 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ServerData.statusTable.GetTableData(StatusTable.Skill1_AddValue).Value += 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ServerData.statusTable.GetTableData(StatusTable.Skill2_AddValue).Value += 1;
        }
    }
#endif
}
