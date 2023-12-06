using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainTabButtons : MonoBehaviour
{
    [SerializeField]
    public GameObject popupPrefab;

    private GameObject popupObject;

    [SerializeField]
    private Transform popupParents;

    [FormerlySerializedAs("initializeByInspector")] [SerializeField] private bool addButtonEvent = true;
    
    private static Dictionary<string, GameObject> popups = new Dictionary<string, GameObject>();

    public static void ResetPopups()
    {
        popups.Clear();
    }
    
    private void Awake()
    {
        if (addButtonEvent)
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClickButton);
        }
    }
    private string GetKey()
    {
        return $"{popupPrefab.GetHashCode()}{popupPrefab.name}";
    }
    public void OnClickButton()
    {
        if (popupPrefab == null)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "업데이트 예정 입니다.", null);
            return;
        }
        
        if (popups.ContainsKey(GetKey()) == false)
        {
            popupObject = Instantiate(popupPrefab, popupParents == null ? InGameCanvas.Instance.transform : popupParents);

            popups.Add(GetKey(), popupObject);
        }
        else
        {
            popupObject = popups[GetKey()];

            popupObject.transform.SetAsLastSibling();
            popupObject.SetActive(true);
        }
        
        // UiStatus.Instance.transform.SetAsLastSibling();
    }
    public void OnClickOnlyInstantiateButton()
    {
        if (popupPrefab == null)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "업데이트 예정 입니다.", null);
            return;
        }

        if (popups.ContainsKey(GetKey()) == false)
        {
            popupObject = Instantiate(popupPrefab, popupParents == null ? InGameCanvas.Instance.transform : popupParents);

            popups.Add(GetKey(), popupObject);
        }
        else
        {
            popupObject = popups[GetKey()];

            popupObject.transform.SetAsLastSibling();
            popupObject.SetActive(true);
        }         
        //다시끔

        StartCoroutine(ObjectSetActiveFalseRoutine());

        // UiStatus.Instance.transform.SetAsLastSibling();
    }

    private IEnumerator ObjectSetActiveFalseRoutine()
    {
        yield return new WaitForSeconds(0.01f);
        popupObject.SetActive(false);
    }

    public void OnClickYachaButton()
    {
        // if (ServerData.weaponTable.TableDatas["weapon21"].hasItem.Value < 1)
        // {
        //     PopupManager.Instance.ShowAlarmMessage("야차검이 필요합니다.");
        //     return;
        // }
        //
        // if (popupPrefab == null)
        // {
        //     PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "업데이트 예정 입니다.", null);
        //     return;
        // }
        //
        // if (popupObject == null)
        // {
        //
        //     popupObject = Instantiate<GameObject>(popupPrefab,
        //         popupParents == null ? InGameCanvas.Instance.transform : popupParents);
        // }
        // else
        // {
        //     popupObject.transform.SetAsLastSibling();
        //     popupObject.SetActive(true);
        // }
    }

}
