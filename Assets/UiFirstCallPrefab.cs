using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFirstCallPrefab : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefab;

    private static bool isFirst = true;
    private List<GameObject> popupObject = new List<GameObject>();
    void Start()
    {
        if (isFirst) 
        {
            isFirst = false;

            for (int i = 0; i < prefab.Count; i++)
            {
                popupObject.Add(Instantiate<GameObject>(prefab[i],InGameCanvas.Instance.transform));
            }
            
        }
    }

}
