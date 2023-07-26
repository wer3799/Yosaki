using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiEventMiniGamePlayer : MonoBehaviour
{
    [SerializeField]
    private Image thunbnailIcon;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            thunbnailIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail((int)e);
        }).AddTo(this);
    }


    private string bulletTag = "Bullet";
    private string eventBottom = "EventBottom";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(bulletTag))
        {
            EventMiniGame.Instance.PlayerDamaged();
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(eventBottom))
        {
            EventMiniGame.Instance.ResetJumpCount();
        }
    }
}
