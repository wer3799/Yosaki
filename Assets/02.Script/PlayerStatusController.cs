using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerStatusController : SingletonMono<PlayerStatusController>
{
    private bool canHit = true;

    private WaitForSeconds hitDelay = new WaitForSeconds(1f);
    public ReactiveProperty<double> maxHp { get; private set; } = new ReactiveProperty<double>(GameBalance.initHp);
    public ReactiveProperty<double> hp { get; private set; } = new ReactiveProperty<double>();

    public ReactiveProperty<double> maxMp { get; private set; } = new ReactiveProperty<double>(GameBalance.initMp);
    public ReactiveProperty<double> mp { get; private set; } = new ReactiveProperty<double>();

    public ReactiveCommand whenPlayerDead = new ReactiveCommand();

    private float damTextYOffect = 1f;

    private static bool isFirstGame = true;

    private void Start()
    {
        Initialize();
        Subscribe();
        if (GameManager.contentsType.IsDimensionContents())
        {
            
        }
        else
        {
            StartCoroutine(RecoverRoutine());
        }
    }

    public bool IsPlayerDead()
    {
        return hp.Value <= 0f;
    }

    private IEnumerator RecoverRoutine()
    {
        WaitForSeconds recoverDelay = new WaitForSeconds(5.0f);

        while (true)
        {
            yield return recoverDelay;

            if (IsPlayerDead()) continue;

            float hpRecoverPer = PlayerStats.GetHpRecover();
            float mpRecoverPer = PlayerStats.GetMpRecover();

            if (IsHpFull() == false && hpRecoverPer != 0f)
            {
                UpdateHp(maxHp.Value * hpRecoverPer);
            }

            if (IsMpFull() == false && mpRecoverPer != 0f)
            {
                //  UpdateMp(maxMp.Value * mpRecoverPer);
            }
        }
    }

    private void UpdateHpMax()
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            maxHp.Value = PlayerStats.GetMaxHpDimension();
        }
        else
        {
            maxHp.Value = PlayerStats.GetMaxHp();
        }
    }
    private void UpdateMpMax()
    {
        maxMp.Value = PlayerStats.GetMaxMp();
    }

    private void Subscribe()
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            
        }
        else
        {
            ServerData.statusTable.GetTableData(StatusTable.HpLevel_Gold).Subscribe(e => { UpdateHpMax(); })
                .AddTo(this);

            ServerData.statusTable.GetTableData(StatusTable.HpPer_StatPoint).Subscribe(e => { UpdateHpMax(); })
                .AddTo(this);

            ServerData.statusTable.GetTableData(StatusTable.MpLevel_Gold).Subscribe(e => { UpdateMpMax(); })
                .AddTo(this);

            ServerData.statusTable.GetTableData(StatusTable.MpPer_StatPoint).Subscribe(e => { UpdateMpMax(); })
                .AddTo(this);
            //레벨업때
            ServerData.statusTable.GetTableData(StatusTable.Level).Subscribe(e => { SetHpMpFull(); }).AddTo(this);

            //코스튬 인덱스 바뀔때
            ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeSlot].AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            ServerData.equipmentTable.TableDatas[EquipmentTable.CostumePresetId].AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            //노리개 바뀔때
            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            //
            ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);
            //

            ServerData.equipmentTable.TableDatas[EquipmentTable.CostumePresetId].AsObservable().Subscribe(e =>
            {
                StartCoroutine(LateUpdateHp());
            }).AddTo(this);

            //코스튬 새로운 능력치 뽑기 했을때
            ServerData.costumeServerTable.WhenCostumeOptionChanged.AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            //펫 획득 했을때

            var iter = ServerData.petTable.TableDatas.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.hasItem.AsObservable().Subscribe(e =>
                {
                    UpdateHpMax();
                    UpdateMpMax();
                }).AddTo(this);

                iter.Current.Value.level.AsObservable().Subscribe(e =>
                {
                    UpdateHpMax();
                    UpdateMpMax();
                }).AddTo(this);
            }

            //날개 렙업
            ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            ServerData.petEquipmentServerTable.TableDatas["petequip0"].hasAbil.AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            ServerData.petEquipmentServerTable.TableDatas["petequip0"].level.AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);
            //패시브스킬
            var tableData = TableManager.Instance.PassiveSkill.dataArray;

            for (int i = 0; i < tableData.Length; i++)
            {
                if (tableData[i].Abilitytype != (int)StatusType.HpAddPer) continue;

                var serverData = ServerData.passiveServerTable.TableDatas[tableData[i].Stringid];

                serverData.level.AsObservable().Subscribe(e => { UpdateHpMax(); }).AddTo(this);
            }

            //유물
            ServerData.relicServerTable.TableDatas["relic1"].level.AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            ServerData.relicServerTable.TableDatas["relic3"].level.AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);

            ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).AsObservable().Subscribe(e =>
            {
                UpdateHpMax();
                UpdateMpMax();
            }).AddTo(this);
        }


        hp.AsObservable().Subscribe(e =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.Hp).Value = e;
        }).AddTo(this);

        mp.AsObservable().Subscribe(e =>
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.Mp).Value = e;
        }).AddTo(this);

        

    
    }

    private IEnumerator LateUpdateHp()
    {
        yield return null;
        UpdateHpMax();
        UpdateMpMax();
    }

    private void Initialize()
    {
        UpdateHpMax();

        UpdateMpMax();

        if (isFirstGame)
        {
            isFirstGame = false;
            SetHpMpFull();
        }
        else
        {
            if (GameManager.contentsType.IsDimensionContents())
            {
                hp.Value = PlayerStats.GetMaxHpDimension();
            }
            else
            {
                hp.Value = (float)ServerData.userInfoTable.GetTableData(UserInfoTable.Hp).Value;
                mp.Value = (float)ServerData.userInfoTable.GetTableData(UserInfoTable.Mp).Value;
            }
        }
    }

    private void SetHpMpFull()
    {
        hp.Value = maxHp.Value;
        mp.Value = maxMp.Value;
    }

    private bool IsHpFull()
    {
        return hp.Value == maxHp.Value;
    }

    private bool IsMpFull()
    {
        return mp.Value == maxMp.Value;
    }

    public void SetHpToMax()
    {
        hp.Value = maxHp.Value;
    }

    public void UpdateHp(double value, float percentDamage = 0)
    {
        if (GameManager.contentsType.IsDimensionContents())
        {
            //데미지입음
            if (value < 0)
            {
                if (canHit == false || IsPlayerDead()) return;

                value = -1;

                StartCoroutine(HitDelayRoutine());
            }
            //회복함
            else
            {
                if (IsPlayerDead()) return;
            }


#if UNITY_EDITOR
            // Debug.Log($"Player damaged {value}");
#endif
            SpawnDamText(value);
            hp.Value += value;
             
            hp.Value = Mathf.Clamp((float)hp.Value, 0f, (float)maxHp.Value);

            CheckDead();      
        }
        else
        {
            //데미지입음
            if (value < 0)
            {
                if (canHit == false || IsPlayerDead() || UiSusanoBuff.isImmune.Value) return;

                float damDecreaseValue = PlayerStats.GetDamDecreaseValue();

                value -= value * damDecreaseValue;

                StartCoroutine(HitDelayRoutine());
            }
            //회복함
            else
            {
                if (IsPlayerDead()) return;
            }


#if UNITY_EDITOR
            // Debug.Log($"Player damaged {value}");
#endif
            if (percentDamage == 0)
            {
                SpawnDamText(value);
                hp.Value += value;
            }
            else
            {
                value = maxHp.Value * -percentDamage;

                SpawnDamText(value);
                hp.Value += value;
            }
             
            hp.Value = Mathf.Clamp((float)hp.Value, 0f, (float)maxHp.Value);

            CheckDead();      
        }
    }

    public void UpdateMp(float value)
    {
        mp.Value += value;

        mp.Value = Mathf.Clamp((float)mp.Value, 0f, (float)maxMp.Value);
    }

    private void CheckDead()
    {
        if (hp.Value <= 0)
        {
            if (GameManager.contentsType.IsDimensionContents())
            {
                whenPlayerDead.Execute();
                UiAutoBoss.Instance.WhenToggleChanged(false);
            }
            else
            {
                if(GameManager.Instance.IsNormalField ==false &&UiDokebiBuff.isImmune.Value==false)
                {
                    UiDokebiBuff.Instance.ActiveDokebiImmune();
                }

                if (GameManager.Instance.IsNormalField == false && UiSusanoBuff.isImmune.Value == false &&UiDokebiBuff.isImmune.Value==false)
                {
                    UiSusanoBuff.Instance.ActiveSusanoImmune();
                }

                if (UiSusanoBuff.isImmune.Value == false && UiDokebiBuff.isImmune.Value==false)
                {
                
                    whenPlayerDead.Execute();
                    UiAutoBoss.Instance.WhenToggleChanged(false);
                }
            }
          
        }
    }

    private void SpawnDamText(double value)
    {
        Vector2 position = Vector2.up * damTextYOffect + UnityEngine.Random.insideUnitCircle;

        if (value < 0)
        {
            BattleObjectManagerAllTime.Instance.SpawnDamageText(value * -1f, this.transform.position, DamTextType.Red);
        }
        else
        {
            BattleObjectManagerAllTime.Instance.SpawnDamageText(value, this.transform.position, DamTextType.Green);
        }
    }
    private IEnumerator HitDelayRoutine()
    {
        canHit = false;
        yield return hitDelay;
        canHit = true;
    }
}
