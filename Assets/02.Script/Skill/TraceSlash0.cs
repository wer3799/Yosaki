using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceSlash0 : SkillBase
{
    private float moveDistance;

    protected float boxSize = 4;

    protected bool attractEnemy = false;

    public TraceSlash0()
    {
    }

    public override void UseSkill()
    {
        //Transform neariestEnemy = AutoManager.Instance.GetNeariestEnemy();

        //if (neariestEnemy == null)
        //{
        //    if (AutoManager.Instance.IsAutoMode == false)
        //    {
        //        PopupManager.Instance.ShowAlarmMessage("근처에 적이 없습니다.");
        //    }
        //    return;
        //}

        base.UseSkill();

        //이동제한있을경우
        playerSkillCaster.SetMoveRestriction(skillInfo.Movedelay);

        //파티클
        CoroutineExecuter.Instance.StartCoroutine(SpawnLineEffect());

        //캐릭터 이동
        //playerTr.position = neariestEnemy.position;

        //데미지
        double damage = GetSkillDamage(skillInfo);

        var hitEnemies = playerSkillCaster.GetEnemiesInCircle(playerTr.position, skillInfo.Targetrange);

        //데미지적용
        for (int i = 0; i < hitEnemies.Length && i < skillInfo.Targetcount; i++)
        {
            PlayerSkillCaster.Instance.StartCoroutine(playerSkillCaster.ApplyDamage(hitEnemies[i], skillInfo, damage, i == 0));
        }
    }
    public override void UseDimensionSkill()
    {
        base.UseDimensionSkill();

        //이동제한있을경우
        playerSkillCaster.SetMoveRestriction(0f);

        //파티클
        CoroutineExecuter.Instance.StartCoroutine(SpawnLineEffect());


        //데미지
        double damage = GetDimensionSkillDamage();

        var hitEnemies = playerSkillCaster.GetEnemiesInCircle(playerTr.position, GameBalance.dimensionAttackRange);

        //데미지적용
        for (int i = 0; i < hitEnemies.Length && i < 500; i++)
        {
            PlayerSkillCaster.Instance.StartCoroutine(playerSkillCaster.ApplyDimensionDamage(hitEnemies[i],  damage, i == 0));
        }
    }

    private IEnumerator SpawnLineEffect()
    {
        var effect = EffectManager.SpawnEffectAllTime("LightningFloorYellowTrail", PlayerMoveController.Instance.transform.position);

        yield return null;

        if (effect != null)
        {
            effect.transform.position = PlayerMoveController.Instance.transform.position;
        }
    }
}
