using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRecordUI : MonoBehaviour
{
    public GameObject ShootBar;
    private DisplayBarManager ShootBarController;
    public GameObject JumpBar;
    private DisplayBarManager JumpBarController;
    public GameObject MoveBar;
    private DisplayBarManager MoveBarController;
    public GameObject TimeBar;
    public GameObject ShootLock, JumpLock, MoveLock;
    public GameController gameController;
    void Awake()
    {
        ShootBarController = ShootBar.GetComponent<DisplayBarManager>();
        JumpBarController = JumpBar.GetComponent<DisplayBarManager>();
        MoveBarController = MoveBar.GetComponent<DisplayBarManager>();
    } 

    public void UpdateUITimeline(Timeline ShootTimeline, Timeline JumpTimeline, Timeline MoveTimeline)
    {
        ShootBarController.actions = ShootTimeline._actionList;
        JumpBarController.actions = JumpTimeline._actionList;
        MoveBarController.actions = MoveTimeline._actionList;
        ShootLock.SetActive(ShootTimeline.Locked);
        JumpLock.SetActive(JumpTimeline.Locked);
        MoveLock.SetActive(MoveTimeline.Locked);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isPlaying)
        {
            float currentTime = Time.time;
            var trans = TimeBar.GetComponent<RectTransform>();
            Vector3 barPosition = trans.anchoredPosition;
            barPosition.x = -220 + (ShootBarController.barLength * (currentTime - gameController.StartTime) / gameController.maxTime);
            trans.anchoredPosition = barPosition;
        }
    }
}
