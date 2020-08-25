using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action
{
    public float Direction {get; set;}
    
    public MoveAction(float startTime, float duration, float direction, int callCount) : base (startTime, duration)
    {
        Direction = direction;
        CallCount = callCount;
    }
}
