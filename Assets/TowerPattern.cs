using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerPattern : MonoBehaviour
{
    [SerializeField] private List<AlarmHitObject> _alarmHitObject;

    [SerializeField] private float percentDamage = 0.5f;


    private void Start()
    {
        SetDamage();
    }

    private void SetDamage()
    {
        using var e = _alarmHitObject.GetEnumerator();

        while (e.MoveNext())
        {
            e.Current.SetDamage(1,percentDamage);
        }
    }
    
    public void AttackStart()
    {
        using var e = _alarmHitObject.GetEnumerator();

        while (e.MoveNext())
        {
            e.Current.AttackStart();
        };
    }
}
