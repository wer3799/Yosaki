using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField] private int costumeId = 0;

    [SerializeField] private TextMeshProUGUI costumeNameText;

    [SerializeField] private bool isInitByInspector = true;
    private void Awake()
    {
        skeletonGraphic.gameObject.SetActive(false);
    }

    void Start()
    {
        if (isInitByInspector)
        {
            Initialize(costumeId);
        }
    }

    public void Initialize(int costumeIdx)
    {
        costumeId = costumeIdx;
        
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(costumeId);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
        skeletonGraphic.gameObject.SetActive(true);
        
        string costumeKey = "costume"+$"{costumeId}";
        Item_Type itemType = (Item_Type)Enum.Parse(typeof(Item_Type), costumeKey);
        costumeNameText.SetText(CommonString.GetItemName(itemType));
    }
    public void Initialize(string costumeKey)
    {
        costumeId = int.Parse(costumeKey.Replace("costume", ""));
        
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(costumeId);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
        skeletonGraphic.gameObject.SetActive(true);
        
        Item_Type itemType = (Item_Type)Enum.Parse(typeof(Item_Type), costumeKey);
        costumeNameText.SetText(CommonString.GetItemName(itemType));
    }
}
