using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiPlayerDescription : MonoBehaviour
{
    [SerializeField]
    private GameObject titleObject;

    [SerializeField]
    private GameObject guildNameObject;

    [SerializeField]
    private SpriteRenderer guilIcon;

    [SerializeField]
    private TextMeshPro guildName;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.hasGuild.AsObservable().Subscribe(e =>
        {

            titleObject.SetActive(e == false);

            guildNameObject.SetActive(e == true);

            if (e == true)
            {
                if (GuildManager.Instance.guildInfoData.Value != null)
                {
                    guilIcon.sprite = CommonUiContainer.Instance.guildIcon[GuildManager.Instance.guildIconIdx.Value];
                    guildName.SetText(GuildManager.Instance.myGuildName);
                }
                else
                {
                    titleObject.SetActive(true);
                    guildNameObject.SetActive(false);
                }
            }

        }).AddTo(this);

        GuildManager.Instance.guildIconIdx.AsObservable().Subscribe(e =>
        {
            guilIcon.sprite = CommonUiContainer.Instance.guildIcon[e];
        }).AddTo(this);
    }
}
