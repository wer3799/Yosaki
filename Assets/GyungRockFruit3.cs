using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GyungRockFruit3 : MonoBehaviour
{
    [SerializeField] private List<Sprite> fruits;
    [SerializeField] private Image icon_Image;
    [SerializeField] private SpriteRenderer icon_SpriteRenderer;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (GameManager.contentsType == GameManager.ContentsType.GyungRockTower3)
        {
            icon_Image = GetComponent<Image>();
            icon_SpriteRenderer = GetComponent<SpriteRenderer>();

            int currentIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.gyungRockTower3].Value;
            
            if (icon_Image != null)
            {
                icon_Image.sprite = fruits[currentIdx];
            }

            if (icon_SpriteRenderer != null)
            {
                icon_SpriteRenderer.sprite = fruits[currentIdx];
            }
        }
        else if(GameManager.contentsType == GameManager.ContentsType.GyungRockTower6)
        {
            icon_Image = GetComponent<Image>();
            icon_SpriteRenderer = GetComponent<SpriteRenderer>();

            int currentIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.gyungRockTower6].Value;
            
            if (icon_Image != null)
            {
                icon_Image.sprite = fruits[currentIdx];
            }

            if (icon_SpriteRenderer != null)
            {
                icon_SpriteRenderer.sprite = fruits[currentIdx];
            }
        }
        else if(GameManager.contentsType == GameManager.ContentsType.GyungRockTower7)
        {
            icon_Image = GetComponent<Image>();
            icon_SpriteRenderer = GetComponent<SpriteRenderer>();

            int currentIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.gyungRockTower7].Value;
            
            if (icon_Image != null)
            {
                icon_Image.sprite = fruits[currentIdx];
            }

            if (icon_SpriteRenderer != null)
            {
                icon_SpriteRenderer.sprite = fruits[currentIdx];
            }
        }
        else if(GameManager.contentsType == GameManager.ContentsType.GyungRockTower8)
        {
            icon_Image = GetComponent<Image>();
            icon_SpriteRenderer = GetComponent<SpriteRenderer>();

            int currentIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.gyungRockTower8].Value;
            
            if (icon_Image != null)
            {
                icon_Image.sprite = fruits[currentIdx];
            }

            if (icon_SpriteRenderer != null)
            {
                icon_SpriteRenderer.sprite = fruits[currentIdx];
            }
        }
        else if(GameManager.contentsType == GameManager.ContentsType.GyungRockTower9)
        {
            icon_Image = GetComponent<Image>();
            icon_SpriteRenderer = GetComponent<SpriteRenderer>();

            int currentIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.gyungRockTower9].Value;
            
            if (icon_Image != null)
            {
                icon_Image.sprite = fruits[currentIdx];
            }

            if (icon_SpriteRenderer != null)
            {
                icon_SpriteRenderer.sprite = fruits[currentIdx];
            }
        }
        else
        {
            Debug.LogError("컨텐츠 타입이 올바르지 않습니다.");
        }
    }
}
