using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using UnityEngine.Serialization;

public class UiYorinMission : MonoBehaviour
{
    [SerializeField] private UiYorinMissionCell _yorinMissionCell;

    [SerializeField] private List<Transform> cellParentTransform;

    [SerializeField] private List<UiYorinMissionCell> _yorinMissionCellsContainer;
    [SerializeField] private UiRewardResultView _uiRewardResultView;
    private void Start()
    {
        Initialize();
        Subscribe();
        UiTutorialManager.Instance.SetClear(TutorialStep.ClickYorinMission);

    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.YorinMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var prefab = Instantiate<UiYorinMissionCell>(_yorinMissionCell, cellParentTransform[tableData[i].Missionday - 1]);
            prefab.Initialize(tableData[i]);
            _yorinMissionCellsContainer.Add(prefab);
        }
        //정렬
        for (int i = 0; i < _yorinMissionCellsContainer.Count; i++)
        {
            _yorinMissionCellsContainer[i].transform.SetSiblingIndex(tableData[i].Displayorder);
        }
        //획득한기준으로 정렬
        for (int i = 0; i < _yorinMissionCellsContainer.Count; i++)
        {
            _yorinMissionCellsContainer[i].ReArrange();
        }
    }

    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var data = TableManager.Instance.YorinMission.dataArray;

            for (int i = 0; i < data.Length; i++)
            {
                if(data[i].Missionday!=SelectableTabIdx+1)continue;
                ServerData.yorinMissionServerTable.TableDatas[data[i].Stringid].rewardCount.Value = 0;
            }
            
        }
    }
#endif
    
    private int SelectableTabIdx = 0;

    public void SetIdx(int idx)
    {
        SelectableTabIdx = idx;
    }
    public void AllReceiveButton()
    {
        List<Tuple<Item_Type, float>> rewards = new List<Tuple<Item_Type, float>>();
        foreach (var cell in _yorinMissionCellsContainer)
        {
            if (cell.dayIdx-1 != SelectableTabIdx) continue;
            
            rewards.Add(cell.OnClickGetButtonAll());
        }

        List<RewardData> rewardData = new List<RewardData>();

        for (int i = 0; i < rewards.Count; i++)
        {
            if(rewards[i].Item1==Item_Type.None)continue;
            rewardData.Add(new RewardData(rewards[i].Item1,rewards[i].Item2));
        }

        if (rewardData.Count > 0)
        {
            _uiRewardResultView.gameObject.SetActive(true);
            _uiRewardResultView.Initialize(rewardData);
        }
    }
    public void AllReceiveButtonNoRewardPopup()
    {
        foreach (var cell in _yorinMissionCellsContainer)
        {
            if (cell.dayIdx-1 != SelectableTabIdx) continue;
            cell.OnClickGetButton();
        }
    }
    
    
    private void Subscribe()
    {
        ServerData.iapServerTable.TableDatas["starter0"].buyCount.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_1].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).AsObservable().Subscribe(e =>
        {
            if (e >= 5)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_2].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.petTable.TableDatas["pet4"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e >0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_3].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonMaxKillCount).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_4].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount3).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_5].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).AsObservable().Subscribe(e =>
        {
            if (e > 10-2)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_6].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.weaponTable.TableDatas["weapon16"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_7].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.magicBookTable.TableDatas["magicBook12"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_8].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Skill).AsObservable().Subscribe(e =>
        {
            if (e >= 10000)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission1_9].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.smithClear).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission2_1].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);

        ServerData.magicBookTable.TableDatas["magicBook16"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                
                    string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission2_2].Stringid;
                    ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission2_3].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission2_4].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taeguekTower).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission2_5].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        //전방베기4형
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][3].AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission2_7].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.sonScore).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission3_1].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(Item_Type.WeaponUpgradeStone).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission3_2].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.weaponTable.TableDatas["weapon20"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission3_3].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.petEquipmentServerTable.TableDatas["petequip0"].hasAbil.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission3_4].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.bossServerTable.TableDatas["boss20"].score.AsObservable().Subscribe(e =>
        {
            if (string.IsNullOrEmpty(e))
            {
                return;
            }
            else if (double.Parse(e) > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission3_5].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.yoguiSogulLastClear).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission3_6].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.goodsTable.GetTableData(Item_Type.TigerBossStone).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission4_1].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);

        ServerData.stageRelicServerTable.TableDatas["relic0"].level.AsObservable().Subscribe(e =>
        {
            if (e >= 100)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission4_2].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).AsObservable().Subscribe(e =>
        {
            if (e >= 10)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission4_3].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.susanoScore].AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission4_4].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.foxMask).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission4_5].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.goodsTable.GetTableData(Item_Type.SheepStone).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission5_1].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.weaponTable.TableDatas["weapon21"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission5_2].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.bossServerTable.TableDatas["boss15"].score.AsObservable().Subscribe(e =>
        {
            if (string.IsNullOrEmpty(e))
            {
                return;
            }
            else if (double.Parse(e) > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission5_3].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulLevel).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission5_6].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.goodsTable.GetTableData(Item_Type.PigStone).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission6_1].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.userInfoTable.GetTableData(UserInfoTable.hellScore).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission6_2].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.gumGiClear).AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission6_3].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.bossServerTable.TableDatas["b50"].score.AsObservable().Subscribe(e =>
        {
            if (string.IsNullOrEmpty(e))
            {
                return;
            }
            else if (double.Parse(e) > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission6_4].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.weaponTable.TableDatas["weapon22"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission7_1].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        GuildManager.Instance.hasGuild.AsObservable().Subscribe(e =>
        {
            if (e)//hasGuild가 true일때
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission7_2].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).AsObservable().Subscribe(e =>
        {
            if (e >= 30)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission7_4].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.bossServerTable.TableDatas["b55"].score.AsObservable().Subscribe(e =>
        {
            if (string.IsNullOrEmpty(e))
            {
                return;
            }
            else if (double.Parse(e) > 0)
            {
                string key = TableManager.Instance.YorinMissionDatas[(int)YorinMissionKey.YMission7_5].Stringid;
                ServerData.yorinMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
    }

}
