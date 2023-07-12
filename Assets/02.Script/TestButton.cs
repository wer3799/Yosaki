using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
        
#else
        this.gameObject.SetActive(false);
        #endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.F8))
        {
            rootObject.SetActive(true);
        }
#endif
    }
}
