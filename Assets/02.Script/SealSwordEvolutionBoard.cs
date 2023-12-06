using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SealSwordEvolutionBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sealSwordEvolutionGradeText;
    [SerializeField] private TextMeshProUGUI abilityDesc;
    [SerializeField] private TextMeshProUGUI gaugeDesc;

    [SerializeField] private SealSwordEvolutionView mainSealSwordEvolutionView;

    [SerializeField] private SealSwordEvolutionView prefab;
    [SerializeField] private Transform cellParent;
    [SerializeField] private RegisteredSealSwordEvolutionView prefab2;
    [SerializeField] private Transform cellParent2;
    
    [SerializeField]
    private Image gaugeImage;
    
    [SerializeField] private Button sealSwordEvolutionButton;
    [SerializeField] private Button sealSwordAutoRegisterButton;
    [SerializeField] private GameObject registeredDesc;
    [SerializeField] private GameObject currentDesc;
    private List<SealSwordEvolutionView> currentContainer = new List<SealSwordEvolutionView>();
    
    private List<RegisteredSealSwordEvolutionView> registeredContainer = new List<RegisteredSealSwordEvolutionView>();

    private ReactiveProperty<float> exp = new ReactiveProperty<float>(0f);

    private int currentIdx = 0;
    [SerializeField]
    private GameObject applyObject;
    
    // Start is called before the first frame update
    private void Start()
    {
        Subscribe();
    }

    private void OnEnable()
    {
        if (ServerData.sealSwordServerTable.TableDatas["SealSword28"].hasItem.Value < 1)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("요도 각성은 영물 요도 보유 시 할 수 있습니다.");
            return;
        }
        Initialize();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).AsObservable().Subscribe(e =>
        {
            UpdateUi((int)e);

        }).AddTo(this);

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionExp).AsObservable()
            .Subscribe(e =>
            {
                UpdateUi((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value);
            }).AddTo(this);

        exp.AsObservable().Subscribe(e =>
        {
            sealSwordEvolutionButton.interactable = IsUpgradable();
            
            UpdateUi((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value);
        }).AddTo(this);
    }

    private void Initialize()
    {
        currentIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value;
        
        var data = TableManager.Instance.sealSwordTable.dataArray[28];
        
        UpdateUi((int)currentIdx);

        mainSealSwordEvolutionView.Initialize(data);

        CreateCells();
        
    }

    private void UpdateUi(int idx)
    {
        
        applyObject.SetActive(currentIdx == (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value);
        
        
        sealSwordEvolutionGradeText.SetText($"요도 각성 {idx+1} 단계");
        
        UpdateAbilityText(idx);
        
        UpdateGaugeText();
        
        registeredDesc.SetActive(registCount<1);
    }
    private void CreateCells()
    {
        CreateSealSwordList();
        
        CreateRegisteredSealSwordList();

        exp.Value = 0;
        registCount = 0;
    }
    private void UpdateAbilityText(int idx)
    {
        var abil0 = PlayerStats.GetSealSwordEvolutionAbilityByIdx(StatusType.SuperCritical26DamPer,idx);
        var abil1 = PlayerStats.GetSealSwordEvolutionAbilityByIdx(StatusType.ReduceSealSwordSkillRequireCount,idx);

        string desc = "";
        if (abil0 > 0)
        {
            desc += $"{CommonString.GetStatusName(StatusType.SuperCritical26DamPer)} +{Utils.ConvertNum(abil0*100,2)}";
        }
        else
        {
            desc += "획득한 능력치가 없습니다.";
        }
        if (abil1 > 0)
        {
            desc += $"\n{CommonString.GetStatusName(StatusType.ReduceSealSwordSkillRequireCount)} +{abil1}";
        }
        
        abilityDesc.SetText(desc);
    }

    private void UpdateGaugeText()
    {
        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value;
        
        var tableData= TableManager.Instance.SealSwordEvolution.dataArray;
        
        var gauge = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionExp).Value;
        //만렙 달성
        if (tableData.Length <= grade + 1)
        {
            gaugeDesc.SetText($"Max");
            return;
        }
        
        var requireExp = tableData[grade + 1].Requireexp;

        gaugeImage.fillAmount = (gauge+exp.Value) / requireExp;

        
        gaugeDesc.SetText($"{gauge+exp.Value} / {requireExp}");
    }
    private void CreateSealSwordList()
    {
        var tableData = TableManager.Instance.sealSwordTable.dataArray;
        var legendIdx = 16;
        int slotNum = 0;
        List<int> idxList = new List<int>();
        //슬롯 개수 정리
        for (int i = legendIdx; i < tableData.Length; i++)
        {
            if (ServerData.sealSwordServerTable.TableDatas[tableData[i].Stringid].amount.Value > 0)
            {
                slotNum++;
                idxList.Add(i);
            }
        }
        //셀 만들기
        while (currentContainer.Count < slotNum)
        {
            SealSwordEvolutionView weaponView = Instantiate<SealSwordEvolutionView>(prefab, cellParent);
            currentContainer.Add(weaponView);
        }
        //만들어진 셀 on/off
        for (int i = 0; i < currentContainer.Count; i++)
        {
            if (i < slotNum)
            {
                currentContainer[i].gameObject.SetActive(true);
                currentContainer[i].Initialize(tableData[idxList[i]],this);
            }
            else
            {
                currentContainer[i].gameObject.SetActive(false);
            }
        }
        
        currentDesc.SetActive(slotNum<1);
        if (buttonCoroutine != null)
        {
            StopCoroutine(buttonCoroutine);
        }
        buttonCoroutine = StartCoroutine(ButtonDelayRoutine(slotNum));
        
    }
    private void CreateRegisteredSealSwordList()
    {

        var tableData = TableManager.Instance.sealSwordTable.dataArray;
        var legendIdx = 16;
        int slotNum = 0;
        List<int> idxList = new List<int>();
        //슬롯 개수 정리
        for (int i = legendIdx; i < tableData.Length; i++)
        {
            if (ServerData.sealSwordServerTable.TableDatas[tableData[i].Stringid].amount.Value > 0)
            {
                slotNum++;
                idxList.Add(i);
            }
        }
        //셀 만들기
        while (registeredContainer.Count < slotNum)
        {
            RegisteredSealSwordEvolutionView weaponView = Instantiate<RegisteredSealSwordEvolutionView>(prefab2, cellParent2);
            registeredContainer.Add(weaponView);
        }
        //만들어진 셀 off
        for (int i = 0; i < registeredContainer.Count; i++)
        {
            registeredContainer[i].gameObject.SetActive(false);
            if (slotNum > i)
            {
                registeredContainer[i].Initialize(tableData[idxList[slotNum-i-1]],this);
            }
            //나한테 더이상 없음
            else
            {
                registeredContainer[i].Initialize(null);
            }
        }
    }
    //등록된 곳에 생성
    public void RegisterSealSword(SealSwordData sealSwordData)
    {
        var e = registeredContainer.GetEnumerator();

        while (e.MoveNext())
        {
            //같은게 있으면 올리고 탈출
            if (e.Current != null && e.Current.GetSealSwordData() == null) continue;
            if (e.Current == null || e.Current.GetSealSwordData().Id != sealSwordData.Id) continue;
            //2차 개수 검증
            if (ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].amount.Value - e.Current.GetRegisterCount() <= 0)
            {
                return;
            }            
            e.Current.AddRegister(1);
            e.Current.gameObject.SetActive(true);
            registCount++;
            break;
        }
        var e2 = currentContainer.GetEnumerator();

        while (e2.MoveNext())
        {
            //같은게 있으면 올리고 탈출
            if (e2.Current != null && e2.Current.GetSealSwordData() == null) continue;
            if (e2.Current == null || e2.Current.GetSealSwordData().Id != sealSwordData.Id) continue;
            e2.Current.AddRegister(1);
            break;

        }

        SetExp();
    }
    
    //등록된 데이터 감소시키기.
    public void UnRegisterSealSword(SealSwordData sealSwordData)
    {
        var e = registeredContainer.GetEnumerator();

        while (e.MoveNext())
        {
            //같은게 있으면 내린다.
            if (e.Current != null && e.Current.GetSealSwordData() == null) continue;
            if (e.Current == null || e.Current.GetSealSwordData().Id != sealSwordData.Id) continue;
            e.Current.AddRegister(-1);
            registCount--;
            if (e.Current.GetRegisterCount() < 1)
            {
                e.Current.gameObject.SetActive(false);
            }   
            break;


        }
        
        var e2 = currentContainer.GetEnumerator();
        while (e2.MoveNext())
        {
            if (e2.Current != null && e2.Current.GetSealSwordData() == null) continue;
            if (e2.Current != null && e2.Current.GetSealSwordData().Id == sealSwordData.Id)
            {
                e2.Current.AddRegister(-1);
                break;
            };
        }

        SetExp();
    }

    private void SetExp()
    {
        float sum = 0f;
        
        var e = registeredContainer.GetEnumerator();
        var tableData = TableManager.Instance.sealSwordTable.dataArray;
        while (e.MoveNext())
        {
            sum += tableData[e.Current.GetSealSwordData().Id].Sealswordexp * e.Current.GetRegisterCount();
        }

        exp.Value = sum;
    }

    public bool IsUpgradable()
    {
        


        var gauge = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionExp).Value;
        
        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value;

        var tableData= TableManager.Instance.SealSwordEvolution.dataArray;
        //만렙 달성
        if (tableData.Length <= grade + 1)
        {
            return false;
        }
        
        var requireExp = tableData[grade + 1].Requireexp;

        return gauge+exp.Value>requireExp;
    }
    
    private int registCount = 0;

    public void AutoRegister()
    {
        if (buttonCoroutine != null)
        {
            StopCoroutine(buttonCoroutine);
        }
        
        
        buttonCoroutine = StartCoroutine(ButtonDelayRoutine(-1));
        
        // 최대 등록 가능 개수
        int maxRegistrationCount = 1000;

        for (int i = 0; i < registeredContainer.Count; i++)
        {
            var item = registeredContainer[i];
        
            if (IsUpgradable())
            {
                PopupManager.Instance.ShowAlarmMessage("요도를 더 이상 등록할 수 없습니다.");
                break;
            }

            var sealSwordData = item.GetSealSwordData();
            var availableAmount = ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].amount.Value - item.GetRegisterCount();

            if (availableAmount <= 0)
            {
                continue;
            }

            while (registCount < maxRegistrationCount)
            {
                RegisterSealSword(sealSwordData);
                if (availableAmount > 0)
                {
                    availableAmount--;
                }
                else
                {
                    break; // 해당 원소의 등록을 완료하고 다음 원소로 이동
                }
            }

            if (registCount >= maxRegistrationCount)
            {
                PopupManager.Instance.ShowAlarmMessage("최대 " + maxRegistrationCount + " 개까지만 등록 가능합니다.");
                break;
            }
        }
        
        //
        // var e = registeredContainer.GetEnumerator();
        //
        //
        // while (e.MoveNext())
        // {
        //     if (IsUpgradable() == true)
        //     {
        //         PopupManager.Instance.ShowAlarmMessage("요도를 더 이상 등록할 수 없습니다.");
        //         break;
        //     }
        //     //등록할 수 없는 상태
        //     if (ServerData.sealSwordServerTable.TableDatas[e.Current.GetSealSwordData().Stringid].amount.Value - e.Current.GetRegisterCount() <= 0)
        //     {
        //         continue;
        //     }
        //
        //     
        //     if (registCount <500)
        //     {
        //         RegisterSealSword(e.Current.GetSealSwordData());
        //         AutoRegister();
        //     }
        //     else
        //     {
        //         PopupManager.Instance.ShowAlarmMessage("최대 500 개까지만 등록 가능합니다.");
        //         break;
        //     }
        //     
        //
        //
        // }

    }

    private Coroutine buttonCoroutine;
    private IEnumerator ButtonDelayRoutine(int slotNum=0)
    {
        sealSwordAutoRegisterButton.interactable = false;
        yield return new WaitForSeconds(0.5f);;
        if (slotNum < 0)
        {
            sealSwordAutoRegisterButton.interactable = true;
        }
        else
        {
            sealSwordAutoRegisterButton.interactable = (slotNum > 0 && IsUpgradable()==false);
        }

    }

    private bool isEvolution = false;
    public void OnClickEvolution()
    {
        if (IsUpgradable() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("요도 각성이 불가능합니다.");
            return;
        }

        //중복방지
        if (isEvolution == true)
        {
            return;
        }
        
        isEvolution = true;
        
        var expSum = 0f;

        //업그레이드 가능

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param sealSwordParam = new Param();


        var e = registeredContainer.GetEnumerator();

        // while (e.MoveNext())
        // {
        //     if (ServerData.sealSwordServerTable.TableDatas[e.Current.GetSealSwordData().Stringid].amount.Value < 0)
        //     {
        //         // 마이너스 방지용 코드
        //         PopupManager.Instance.ShowAlarmMessage("요도 개수가 부족합니다!");
        //         return;
        //     }
        // }
        
        
        while (e.MoveNext())
        {
            if (e.Current == null || e.Current.GetRegisterCount() < 1) continue;
            expSum += TableManager.Instance.sealSwordTable.dataArray[e.Current.GetSealSwordData().Id].Sealswordexp * e.Current.GetRegisterCount();
            
            ServerData.sealSwordServerTable.TableDatas[e.Current.GetSealSwordData().Stringid].amount.Value -= e.Current.GetRegisterCount();
            e.Current.SetRegister(0);
            sealSwordParam.Add(e.Current.GetSealSwordData().Stringid,
                ServerData.sealSwordServerTable.TableDatas[e.Current.GetSealSwordData().Stringid]
                    .ConvertToString());
        }

        if (expSum > 0)
        {
            transactionList.Add(TransactionValue.SetUpdate(SealSwordServerTable.tableName, SealSwordServerTable.Indate,
                sealSwordParam));
        }

        var requireExp = TableManager.Instance.SealSwordEvolution.dataArray[(int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value+1].Requireexp;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionExp).Value += expSum- requireExp;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value ++;

        //재화
        Param userinfo2Param = new Param();
        userinfo2Param.Add(UserInfoTable_2.sealSwordEvolutionExp,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionExp).Value);
        userinfo2Param.Add(UserInfoTable_2.sealSwordEvolutionIdx,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate,
            userinfo2Param));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("레벨업");
            CreateCells();
            SetExp();
            UpdateUi((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value);
            isEvolution = false;
        });
    }

    public void OnClickLeftButton()
    {
        if (currentIdx < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("최초 단계입니다!");
            return;
        }
        currentIdx--;
        
        UpdateUi(currentIdx);

    }
    public void OnClickRightButton()
    {
        if (currentIdx >=TableManager.Instance.SealSwordEvolution.dataArray.Length-1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
            return;
        }
        currentIdx++;
        
        UpdateUi(currentIdx);

    }
}
