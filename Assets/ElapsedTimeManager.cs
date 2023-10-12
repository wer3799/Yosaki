using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ElapsedTimeManager : SingletonMono<ElapsedTimeManager>
{
    private DateTime startTime;
    private TimeSpan elapsedTime;

    public void Reset()
    {
        startTime = DateTime.Now;
    }

    public double GetElapsedTimeReal()
    {
        TimeSpan elapsedTime = DateTime.Now - startTime;

        return elapsedTime.TotalSeconds;
    }
}
