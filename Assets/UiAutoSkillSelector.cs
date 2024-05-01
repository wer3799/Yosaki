using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UiAutoSkillSelector : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> toggleObject;

    [SerializeField]
    private List<GameObject> toggleCheckBoxObject;

    //[SerializeField]
    //private GameObject jumpAutoObject;

    //[SerializeField]
    //private GameObject jumpToggleObject;
    [SerializeField] private List<GameObject> _visionSkills; //버튼과 토글

    [SerializeField] private List<GameObject> keyPads =new List<GameObject>();

    private void OnEnable()
    {
        using var e = keyPads.GetEnumerator();

        while (e.MoveNext())
        {
            e.Current.gameObject.SetActive(false);
        }

        if (GameManager.contentsType.IsDimensionContents())
        {
            keyPads[1].SetActive(true);
        }
        else
        {
            keyPads[0].SetActive(true);

        }

        
    }

    void Start()
    {
        SkillCoolTimeManager.LoadSelectedSkill();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.VisionSkill0).AsObservable().Subscribe(e =>
        {
            for (int i = 0; i < _visionSkills.Count; i++)
            {
                _visionSkills[i].SetActive(ServerData.goodsTable.GetVisionSkillIdx()>0);
            }
        }).AddTo(this);

    }

}
