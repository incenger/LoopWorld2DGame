using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineManager : MonoBehaviour
{


    public Timeline m_MoveTimeline;
    public Timeline m_JumpTimeline;
    public Timeline m_ShootTimeline;
    // Start is called before the first frame update


    private void Awake()
    {


        m_MoveTimeline.Locked = false;
        m_JumpTimeline.Locked = false;
        m_ShootTimeline.Locked = false;
    }






    public void RecordAction(Action action)
    {
        if (action is JumpAction)
        {
            m_JumpTimeline.AddAction(action as JumpAction);
        }
        else if (action is ShootAction)
        {
            m_ShootTimeline.AddAction(action as ShootAction);
        }
        else if (action is MoveAction)
        {
            m_MoveTimeline.AddAction(action as MoveAction);
        }
    }

    public void Replay()
    {
        m_MoveTimeline.Replay();
        m_JumpTimeline.Replay();
        m_ShootTimeline.Replay();
    }
    // Update is called once per frame

}
