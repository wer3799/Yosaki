using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using Spine.Unity;
using UniRx;

public class PlayerViewController : SingletonMono<PlayerViewController>
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private SkeletonGraphic munHaSkeletonGraphic;

    private AnimState animState;

    private string CurrentAnimation;

    private const string Anim_Idle = "idle";
    private const string Anim_Run = "run";
    private const string Anim_Attack = "attack1";
    private const string Anim_Attack2 = "attack2";
    private const string Anim_Attack3 = "attack3";
    private const string Attack = "attack";

    private Coroutine attackAnimEndRoutine;

    private WaitForSeconds attackAnimDelay = new WaitForSeconds(0.2f);

    [SerializeField]
    private GameObject attackWeapon;

    [SerializeField]
    private GameObject idleWeapon;
    [SerializeField]
    private GameObject attackWeapon_student;

    [SerializeField]
    private GameObject idleWeapon_student;


    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.hasGuild.AsObservable().Subscribe(e =>
        {
            //길드 정보 불러온적없으면 리턴
            if (GuildManager.Instance.isLoadGuildInfo == false)
                return;
            
            //길드있음
            if (e == true)
            {
                
            }
            //길드없음
            else
            {
                var costumeServerData = ServerData.costumeServerTable.TableDatas["costume171"];

                if (costumeServerData.hasCostume.Value == true)
                {
                    List<TransactionValue> transactions = new List<TransactionValue>();
                
                    costumeServerData.hasCostume.Value = false;
                
                    Param costumeParam = new Param();
                    costumeParam.Add("costume171", costumeServerData.ConvertToString());

                    transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
                    ServerData.SendTransactionV2(transactions, successCallBack: () =>
                    {
                    
                    });
                }
         
                
                //끼고 있으면
                if (ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value == 171)
                {
                    //기본코스튬으로 변경
                    SetCostumeSpine(0);
                
                    //서버 저장
                    ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value = 0;
                    ServerData.equipmentTable.SyncData(EquipmentTable.CostumeLook);
                }

                if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value > 0)
                {
                    ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value = 0;
                    ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.guildTower2ClearIndex,false);
                }
            }
        }).AddTo(this);
    }
    private void SetCostumeSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }
    private IEnumerator AttackAnimEndRoutine()
    {
        yield return attackAnimDelay;
        CurrentAnimation = string.Empty;
        attackAnimEndRoutine = null;
    }

    public enum AnimState
    {
        idle, run, attack
    }

    private void SetAnimation(string animName)
    {
        if (attackAnimEndRoutine != null || (CurrentAnimation == animName && animName.Contains(Attack) == false)) return;

        bool isAttackAnim = animName.Contains(Attack);

        attackWeapon.SetActive(isAttackAnim);
        idleWeapon.SetActive(!isAttackAnim);
        attackWeapon_student.SetActive(isAttackAnim);
        idleWeapon_student.SetActive(!isAttackAnim);

        if (isAttackAnim)
        {
            skeletonGraphic.timeScale = 4f;
            munHaSkeletonGraphic.timeScale = 4f;
            if (attackAnimEndRoutine != null)
            {
                StopCoroutine(attackAnimEndRoutine);
            }

            attackAnimEndRoutine = StartCoroutine(AttackAnimEndRoutine());
        }
        else
        {
            skeletonGraphic.timeScale = 2f;
            munHaSkeletonGraphic.timeScale = 2f;
        }

        bool loop = animName.Equals(Anim_Idle) || animName.Equals(Anim_Run);


        skeletonGraphic.AnimationState.SetAnimation(0, animName, loop);
        munHaSkeletonGraphic.AnimationState.SetAnimation(0, animName, loop);

        CurrentAnimation = animName;
    }
    int attackIdx = 0;
    public void SetCurrentAnimation(AnimState state)
    {
        //attack idle run
        switch (state)
        {
            case AnimState.attack:
                {
                    if (attackIdx == 0)
                    {
                        SetAnimation(Anim_Attack);
                        attackIdx++;
                    }
                    else if (attackIdx == 1)
                    {
                        SetAnimation(Anim_Attack2);
                        attackIdx++;
                    }
                    else
                    {
                        SetAnimation(Anim_Attack3);
                        attackIdx = 0;
                    }

                }
                break;
            case AnimState.idle:
                {
                    SetAnimation(Anim_Idle);
                }
                break;
            case AnimState.run:
                {
                    SetAnimation(Anim_Run);
                }
                break;
        }
    }
}
