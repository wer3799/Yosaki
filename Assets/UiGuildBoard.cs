using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UiGuildBoard : MonoBehaviour
{
    public void OnEnable()
    {
        var currentLevel = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        if (currentLevel < GameBalance.GuildEnterMinLevel)
        {
            PopupManager.Instance.ShowAlarmMessage($"문파는 레벨 {GameBalance.GuildEnterMinLevel}부터 가입할 수 있습니다.");
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            GuildManager.Instance.LoadGuildInfo();
        }
    }

    [SerializeField]
    private GameObject guildMakeBoard;
    [SerializeField]
    private GameObject guildInfoBoard;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.hasGuild.AsObservable().Subscribe(hasGuild =>
        {

            guildMakeBoard.SetActive(!hasGuild);

            guildInfoBoard.SetActive(hasGuild);

        }).AddTo(this);
    }
}
