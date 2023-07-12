using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiSusanoBuff : SingletonMono<UiSusanoBuff>
{
    public static ReactiveProperty<bool> isImmune = new ReactiveProperty<bool>(false);
    public int immuneCount = 0;
    public void ActiveSusanoImmune()
    {
        if (immuneCount > 0) return;
        if (GameManager.contentsType == GameManager.ContentsType.PartyRaid) return;
        if (GameManager.contentsType == GameManager.ContentsType.PartyRaid_Guild) return;
        if (GameManager.contentsType == GameManager.ContentsType.DokebiTower) return;
        if (GameManager.contentsType == GameManager.ContentsType.Ok) return;
        if (GameManager.contentsType == GameManager.ContentsType.Yum) return;
        if (GameManager.contentsType == GameManager.ContentsType.Do) return;
        if (GameManager.contentsType == GameManager.ContentsType.Sumi) return;
        if (GameManager.contentsType == GameManager.ContentsType.Thief) return;
        if (GameManager.contentsType == GameManager.ContentsType.Dark) return;
        if (GameManager.contentsType == GameManager.ContentsType.Sinsun) return;
        if (GameManager.contentsType == GameManager.ContentsType.Sasinsu) return;
        if (GameManager.contentsType == GameManager.ContentsType.Online_Tower) return;
        if (GameManager.contentsType == GameManager.ContentsType.GradeTest) return;
        if (GameManager.contentsType == GameManager.ContentsType.SumisanTower) return;
        if (GameManager.contentsType == GameManager.ContentsType.OldDokebi2) return;
        if (GameManager.contentsType == GameManager.ContentsType.Online_Tower2) return;
        if (GameManager.contentsType == GameManager.ContentsType.RoyalTombTower) return;
        if (GameManager.contentsType == GameManager.ContentsType.DarkTower) return;
        if (GameManager.contentsType == GameManager.ContentsType.FoxTower) return;
        if (GameManager.contentsType == GameManager.ContentsType.TestSword) return;
        if (GameManager.contentsType == GameManager.ContentsType.TestMonkey) return;
        if (GameManager.contentsType == GameManager.ContentsType.TestHell) return;
        if (GameManager.contentsType == GameManager.ContentsType.TestChun) return;
        if (GameManager.contentsType == GameManager.ContentsType.TestDo) return;
        if (GameManager.contentsType == GameManager.ContentsType.TestSumi) return;
        //산신령
        
        if (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon)
        {
            var twelveBossTableData = TableManager.Instance.TwelveBossTable.dataArray[GameManager.Instance.bossId];
            if (twelveBossTableData.NOTIMMUNETYPE == NotImmuneType.Susano)
            {
                return;
            }
        }


        int susanoGrade = PlayerStats.GetSusanoGrade();
        if (susanoGrade == -1) return;

        var tableData = TableManager.Instance.susanoTable.dataArray[susanoGrade];
        if (tableData.Buffsec == 0) return;

        immuneCount = 1;

        PlayerStatusController.Instance.SetHpToMax();

        isImmune.Value = true;

        StartCoroutine(ImmuneRoutine());
    }

    private IEnumerator ImmuneRoutine()
    {
        int susanoGrade = PlayerStats.GetSusanoGrade();

        var tableData = TableManager.Instance.susanoTable.dataArray[susanoGrade];

        float tick = 0f;

        while (tick < tableData.Buffsec)
        {
            tick += Time.deltaTime;

            yield return null;
        }

        isImmune.Value = false;
    }

}
