using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class VisionSkillButton : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private TextMeshProUGUI count;
    [SerializeField]
    private TextMeshProUGUI visionSkillCount;

    [SerializeField]
    private Image gauge;


    private int count_Real;
    private int count_Max = -1;
    private int count_Showing;

    [SerializeField] private GameObject mask;
    private SkillTableData _skillTableData;

    void Start()
    {
        Subscribe();

        StartCoroutine(SkillCountAnimRoutine());
    }

    private bool CheckNormalField()
    {
        return GameManager.contentsType == GameManager.ContentsType.NormalField;
    }


    private bool CheckCanSpawnEnemy()
    {
        if (!CheckNormalField()) return false;
        return MapInfo.Instance != null && MapInfo.Instance.canSpawnEnemy.Value;
    }
    
    private WaitForSeconds delay = new WaitForSeconds(0.01f);

    private IEnumerator SkillCountAnimRoutine()
    {
        while (true)
        {
            if (CheckCanSpawnEnemy())
            {
                //초기화
                if (count_Showing > 0)
                {
                    count_Showing = 0;
                }
                count.SetText("사용불가");
            }
            else
            {
                if (PlayerSkillCaster.Instance.visionSkillUseCount.Value>0)
                {
                    count.SetText($"{count_Max - count_Showing}");
                
                    //0일때
                    if (count_Max - count_Showing < 1)
                    {
                        count.SetText($"사용 가능");
                    }
                }
            
            
                if (count_Real + count_Showing <= count_Max)
                {
                    gauge.fillAmount = (count_Max - (float)count_Showing) / (float)count_Max;
                    count_Showing += PlayerSkillCaster.Instance.addChargeCount.Value;
                }
            }
            yield return delay;
        }
    }

    void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill6).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill0).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill1).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill2).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill3).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill4).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill5).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill7).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill8).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill9).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill10).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill11).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill12).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill13).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill14).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill15).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill16).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill17).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill18).AsObservable().Subscribe(e => { SetSkillImage(); }).AddTo(this);
        PlayerSkillCaster.Instance.visionSkillUseCount.AsObservable().Subscribe(e =>
        {
            //사용시 카운트 초기화
            if (count_Showing > 0)
            {
                count_Showing = 0;
            }
            if (e==0)
            {
                count.SetText("사용함");
            }

            visionSkillCount.SetText($"{e}");
            mask.SetActive(e==0);
        }).AddTo(this);
        PlayerSkillCaster.Instance.visionChargeCount.AsObservable().Subscribe(e =>
        {

            if (e > 0 && PlayerSkillCaster.Instance.visionSkillUseCount.Value>0)
            {
                count_Real = e;
                if (count_Max == -1)
                {
                    count_Max = e;
                }
                if (CheckCanSpawnEnemy())
                {
                    count.SetText("사용불가");
                    return;
                }
            }
            else
            {
                count.SetText("사용함");
            }
            mask.SetActive(e > 0);

        }).AddTo(this);

    }

    private void SetSkillImage()
    {
        int visionSkillIdx = ServerData.goodsTable.GetVisionSkillIdx();
        if (visionSkillIdx < 1)
        {
            //스킬이없는것
            return;
        }

        
        _skillTableData = TableManager.Instance.SkillTable.dataArray[visionSkillIdx];
        

        _image.sprite = CommonResourceContainer.GetSkillIconSprite(_skillTableData.Id);
    }

    public void OnClickSkillButton()
    {
        //차지횟수가 요구조건보다 낮은경우
        if (PlayerSkillCaster.Instance.visionChargeCount.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("아직 사용하실 수 없습니다.");
            return;
        }

        //이미 사용한 경우
        if (PlayerSkillCaster.Instance.visionSkillUseCount.Value==0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 사용하였습니다.");
            return;
        }

        //사용
        PlayerSkillCaster.Instance.UseSkill(_skillTableData.Id);

        PlayerSkillCaster.Instance.UseVisionSkill();
    }
}