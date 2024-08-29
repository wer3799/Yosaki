using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleContestViewController : PoolItem
{
    [FormerlySerializedAs("skeletonGraphic")] [SerializeField]
    private SkeletonGraphic skeletonAnimation;
   
    [SerializeField]
    private SpriteRenderer weaponImage;
    [SerializeField]
    private Image norigaeImage;
    
    public void Initialize(RankManager.RankInfo rankInfo)
    {
        SetCostumeSpine(rankInfo.costumeIdx);
        SetWeapon(rankInfo.weaponIdx);
        SetMagicBook(rankInfo.magicbookIdx);
    }
    
    private void SetCostumeSpine(int idx)
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.skeletonDataAsset.Clear();
            skeletonAnimation.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
            skeletonAnimation.Initialize(true);
        }
        
    }
    private void SetWeapon(int idx)
    {
        if (weaponImage != null)
        {
            weaponImage.sprite = CommonResourceContainer.GetWeaponSprite(idx);
        }

    }
    private void SetMagicBook(int idx)
    {
        if (norigaeImage != null)
        {
            norigaeImage.sprite = CommonResourceContainer.GetMagicBookSprite(idx);
        }

    }
}
