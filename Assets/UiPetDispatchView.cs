using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPetDispatchView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI petName;

    private PetTableData petData;

    private PetServerData petServerData;

    [SerializeField]
    private GameObject notHasObject;
    [SerializeField]
    private TextMeshProUGUI scoreDesc;

    [SerializeField] private Image bgImage;

    public void Initialize(PetTableData petData)
    {
        SetPetSpine(petData.Id);

        this.petData = petData;

        this.petServerData = ServerData.petTable.TableDatas[petData.Stringid];

        petName.SetText($"{petData.Name}");

        scoreDesc.SetText($"{petData.Dispatchscore}점");



        bgImage.sprite = CommonUiContainer.Instance.itemGradeFrame[petData.Dispatchscore];
        
        Subscribe();
    }

    private void Subscribe()
    {
        petServerData.hasItem.AsObservable().Subscribe(e =>
        {
            //notHasObject.SetActive(e == 0);

        }).AddTo(this);
    }

    private void SetPetSpine(int idx)
    {

        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];

        if (idx != 15)
        {
            skeletonGraphic.startingAnimation = "walk";
        }
        else
        {
            skeletonGraphic.startingAnimation = "idel";
        }

        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        if (idx ==0)
        {
            skeletonGraphic.transform.localScale = new Vector3(2f, 2f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -265f, 1f);
        }
        else if (idx >= 1 && idx <= 3)
        {
            skeletonGraphic.transform.localScale = new Vector3(2f, 2f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -260f, 1f);
        }
        else if (idx ==4)
        {
            skeletonGraphic.transform.localScale = new Vector3(2f, 2f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -265f, 1f);
        }
        else if (idx >= 5 && idx <= 7)
        {
            skeletonGraphic.transform.localScale = new Vector3(2f, 2f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -270f, 1f);
        }
        else if (idx >= 8 && idx <= 14)
        {
            skeletonGraphic.transform.localScale = new Vector3(2f, 2f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -293f, 1f);
        }

        else if (idx ==15)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -170f, 1f);
        }

        else if (idx ==16)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }
        
        else if (idx == 17)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }
        else if (idx == 18)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -139.5f, 1f);
        }

        else if (idx ==19)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }
        else if (idx ==20)
        {
            var scale = 0.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }
        else if (idx ==21)
        {
            var scale = 0.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -135f, 1f);
        }
        else if (idx ==22)
        {
            var scale = 0.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(33.8f, -161.6f, 1f);
        }
        else if (idx ==23)
        {
            var scale = 0.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(9f, -121.94f, 1f);
        }
        else if (idx ==24)
        {
            var scale = 1.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(39.6f, -239f, 1f);
        }
        else if (idx ==25)
        {
            var scale = 1.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(31.7f, -288.4f, 1f);
        }
        else if (idx ==26)
        {
            var scale = 1.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(39.6f, -309.6f, 1f);
        }
        else if (idx ==27)
        {
            var scale = 1.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(39.6f, -218.6f, 1f);
        }
        
        else if (idx >= 28 && idx <= 31)
        {
            var scale = 0.8f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -106.8f, 1f);
        }

        else if (idx >= 32 && idx <= 35)
        {
            var scale = 0.6f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -74.9f, 1f);
        }
        else if (idx >= 36 && idx <= 39)
        {
            var scale = 0.7f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -82f, 1f);
        }
        else if (idx >= 40 && idx <= 43)
        {
            var scale = 0.7f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(28.5f, -111f, 1f);
        }
        else if (idx >= 44 && idx <= 47)
        {
            var scale = 1f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -78f, 1f);
        }

        else if (idx ==48)
        {
            var scale = 1.2f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(1.6f, -203f, 1f);
        }
        else if (idx ==49)
        {
            var scale = 1.2f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -221.51f, 1f);
        }
        else if (idx ==50)
        {
            var scale = 1.2f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(37f, -215f, 1f);
        }
        else if (idx ==51)
        {
            var scale = 1.2f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(41f, -165f, 1f);
        }
        else if (idx ==52)
        {
            var scale = 1.5f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -133f, 1f);
        }
        else if (idx >= 53 && idx <= 54)
        {
            var scale = 2f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(2f, -228f, 1f);
        }
        else if (idx >= 55 && idx <= 56)
        {
            var scale = 2f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -147.6f, 1f);
        }
        else if (idx >= 57 && idx <= 58)
        {
            var scale = 2f;
            skeletonGraphic.transform.localScale = new Vector3(scale, scale, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -214.19f, 1f);
        }
    }

}
