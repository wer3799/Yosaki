using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFirstCallPrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private static bool isFirst = true;
    private GameObject popupObject;
    void Start()
    {
        if (isFirst) 
        {
            isFirst = false;

            if (popupObject == null)
            {
                popupObject = Instantiate<GameObject>(prefab,InGameCanvas.Instance.transform);
            }
            else
            {
                popupObject.transform.SetAsLastSibling();
                popupObject.SetActive(true);
            }
        }
    }

}
