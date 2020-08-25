using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public float StartTime {get; set;}
    public float Duration {get; set;}
    public int CallCount {get; set;}
    public Action(float startTime, float duration, int callCount = 1)
    {
        StartTime = startTime;
        Duration = duration;
        CallCount = callCount;
    }
}
