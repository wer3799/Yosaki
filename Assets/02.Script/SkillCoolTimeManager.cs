using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
public static class SkillCoolTimeManager
{
    public static Dictionary<int, ReactiveProperty<float>> remainCool { get; private set; } = new Dictionary<int, ReactiveProperty<float>>();
    public static Dictionary<int, Coroutine> remainCoolRoutine { get; private set; } = new Dictionary<int, Coroutine>();

    private const float updateTick = 0.01f;

    private const float minimumCoolValue = 0.05f;

    private static WaitForSeconds delay = new WaitForSeconds(updateTick);

    public static List<ReactiveProperty<int>> registeredSkillIdx = new List<ReactiveProperty<int>>() { new ReactiveProperty<int>(1), new ReactiveProperty<int>(1), new ReactiveProperty<int>(1), new ReactiveProperty<int>(1), new ReactiveProperty<int>(1) };
    public static ReactiveProperty<int> jumpAutoValue = new ReactiveProperty<int>();
    public static ReactiveProperty<int> moveAutoValue = new ReactiveProperty<int>();

    private static string registerSkillNameKey = "AutoSkill";
    private static string jumpAutoNameKey = "AutoJump";
    private static string moveAutoNameKey = "AutoMove";
    public static void LoadSelectedSkill()
    {
        for (int i = 0; i < registeredSkillIdx.Count; i++)
        {
            //맨처음은 자동등록
            if (PlayerPrefs.HasKey(registerSkillNameKey + i.ToString()) == false)
            {
                registeredSkillIdx[i].Value = 1;
            }
            else
            {
                registeredSkillIdx[i].Value = PlayerPrefs.GetInt(registerSkillNameKey + i.ToString());
            }
        }

        jumpAutoValue.Value = PlayerPrefs.GetInt(jumpAutoNameKey, 1);
        moveAutoValue.Value = PlayerPrefs.GetInt(moveAutoNameKey, 1);
    }

    public static void SetUseSkill(int idx)
    {
        registeredSkillIdx[idx].Value = 1;
        PlayerPrefs.SetInt(registerSkillNameKey + idx.ToString(), 1);
    }

    public static void RemoveUseSkill(int idx)
    {
        registeredSkillIdx[idx].Value = 0;
        PlayerPrefs.SetInt(registerSkillNameKey + idx.ToString(), 0);
    }

    public static void SetJumpAuto(bool on)
    {
        return;
        jumpAutoValue.Value = on ? 1 : 0;
        PlayerPrefs.SetInt(jumpAutoNameKey, on ? 1 : 0);
    }

    public static void SetMoveAuto(bool on)
    {
        moveAutoValue.Value = on ? 1 : 0;
        PlayerPrefs.SetInt(moveAutoNameKey, on ? 1 : 0);
    }

    public static bool HasSkillCooltime(int idx)
    {
        if (remainCool.ContainsKey(idx) == false) return false;

        return remainCool[idx].Value > 0f;
    }

    public static void SetActiveSkillCool(int idx, float coolTime)
    {
        float skillCoolTimeDecValue = PlayerStats.GetSkillCoolTimeDecreaseValue();

        var skillTableData = TableManager.Instance.SkillData[idx];

        float reduceFixedCoolTime = 0f;
        //도술은 쿨감적용 X
        if (skillTableData.SKILLCASTTYPE == SkillCastType.Dosul)
        {
            skillCoolTimeDecValue = 0;
            reduceFixedCoolTime = Mathf.Min(PlayerStats.GetReduceDosulSkillCoolTime(),4);
        }
        
        float calculatedCoolTime = coolTime - (coolTime * skillCoolTimeDecValue) - reduceFixedCoolTime;
        
        calculatedCoolTime = Mathf.Max(minimumCoolValue, calculatedCoolTime);

        if (remainCool.ContainsKey(idx) == false)
        {
            remainCool.Add(idx, new ReactiveProperty<float>());
        }

        if (remainCoolRoutine.ContainsKey(idx) == false)
        {
            remainCoolRoutine.Add(idx, null);
        }

        if (remainCoolRoutine[idx] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(remainCoolRoutine[idx]);
        }

        remainCool[idx].Value = calculatedCoolTime;

        remainCoolRoutine[idx] = CoroutineExecuter.Instance.StartCoroutine(CooltimeRoutine(remainCool[idx]));
    }

    private static IEnumerator CooltimeRoutine(ReactiveProperty<float> data)
    {
        while (data.Value >= 0f)
        {
            yield return null;
            data.Value -= Time.deltaTime;
        }

        //data.Value = 0f;
    }

    public static float GetSkillCoolTimeMax(SkillTableData skillInfo)
    {
        return GetSkillCoolTimeMax(skillInfo.Id);
    }

    public static float GetSkillCoolTimeMax(int idx)
    {
        return TableManager.Instance.SkillData[idx].Cooltime;
    }
}