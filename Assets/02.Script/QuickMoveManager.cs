using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class QuickMoveManager : SingletonMono<QuickMoveManager>
{
    [SerializeField]
    public List<GameObject> popupPrefab;

    private GameObject popupObject;

    [SerializeField]
    private Transform popupParents;

    private static Dictionary<string, GameObject> popups = new Dictionary<string, GameObject>();

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        GameManager.Instance.whenSceneChanged.AsObservable().Subscribe(e =>
        {
            popups.Clear();
        }).AddTo(this);
    }
    private string GetKey(int idx)
    {
        return $"{popupPrefab[idx].GetHashCode()}{popupPrefab[idx].name}";
    }
    public void OnClickButton(int idx)
    {
        if (popupPrefab == null)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "업데이트 예정 입니다.", null);
            return;
        }
        
        if (popups.ContainsKey(GetKey(idx)) == false)
        {
            popupObject = Instantiate(popupPrefab[idx], popupParents == null ? InGameCanvas.Instance.transform : popupParents);

            popups.Add(GetKey(idx), popupObject);
        }
        else
        {
            popupObject = popups[GetKey(idx)];

            popupObject.transform.SetAsLastSibling();
            popupObject.SetActive(true);
        }
    }

}
