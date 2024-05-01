using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityOnlyObject : MonoBehaviour
{
    private void OnEnable()
    {
        #if UNITY_EDITOR
        #else
        this.gameObject.SetActive(false);
        #endif
    }
}
