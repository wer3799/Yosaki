using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiMagicBookTransView : MonoBehaviour
{

    [FormerlySerializedAs("weaponName")] [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private MagicBookData tableData;

    private MagicBookServerData serverData;

    [SerializeField]
    private GameObject notHasObject;

    [FormerlySerializedAs("weaponIcon")] [SerializeField]
    private Image itemIcon;


    public MagicBookData GetMagicBookData()
    {
        return tableData;
    }
    public void Initialize(MagicBookData magicBookData)
    {

        this.tableData = magicBookData;

        this.serverData = ServerData.magicBookTable.TableDatas[magicBookData.Stringid];

        itemName.SetText($"{magicBookData.Name}");
        
        itemIcon.sprite = CommonResourceContainer.GetMagicBookSprite(magicBookData.Id);        


        Subscribe();
    }

    private void Subscribe()
    {
        serverData.trans.AsObservable().Subscribe(e =>
        {
            notHasObject.SetActive(e == 0);

            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);

    }

}