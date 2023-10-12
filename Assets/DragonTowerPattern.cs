using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DragonTowerPattern : MonoBehaviour
{
    [SerializeField] private AlarmHitObject _alarmHitObject;

    [SerializeField] private float percentDamage = 0.5f;


    private void Start()
    {
        SetDamage();
    }

    private void SetDamage()
    {
        _alarmHitObject.SetDamage(1,percentDamage);
    }
    
    public void AttackStart()
    {
        _alarmHitObject.AttackStart();
    }
}
