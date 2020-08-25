using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUI : MonoBehaviour
{
    public GameObject NormalLockUIPrefab;
    public GameObject JumpLockUIPrefab;
    public GameObject MoveLockUIPrefab;
    public GameObject AttackLockUIPrefab;
    public GameObject FinishedUIPrefab;
    public GameObject ReadyUIPrefab;
    private GameObject LockUI = null;
    private GameObject FinishedUI = null;
    private GameObject ReadyUI = null;
    public void setLevelLock(bool isAttackLock, bool isMoveLock, bool isJumpLock, Vector3 position)
    {
        GameObject LockUIPrefab;
        if (isAttackLock) LockUIPrefab = AttackLockUIPrefab;
        else if (isMoveLock) LockUIPrefab = MoveLockUIPrefab;
        else if (isJumpLock) LockUIPrefab = JumpLockUIPrefab;
        else LockUIPrefab = NormalLockUIPrefab;
        this.Init(LockUIPrefab, position);
    }

    public void Init(GameObject LockUIPrefab,Vector3 position)
    {
        if (LockUI == null)
            LockUI = Instantiate(LockUIPrefab, position, Quaternion.identity);
        if (FinishedUI == null)
            FinishedUI = Instantiate(FinishedUIPrefab, position, Quaternion.identity);
        if (ReadyUI == null)
            ReadyUI = Instantiate(ReadyUIPrefab, position, Quaternion.identity);
    }
    public void setState(bool isReady, bool isPlaying, bool isFinished)
    {
        if (isReady)
        {
            ReadyUI.SetActive(true);
            LockUI.SetActive(false);
            FinishedUI.SetActive(false);
        }
        else if (isFinished)
        {
            FinishedUI.SetActive(true);
            LockUI.SetActive(false);
            ReadyUI.SetActive(false);
        }
        else if (isPlaying)
        {
            LockUI.SetActive(false);
            ReadyUI.SetActive(false);
            FinishedUI.SetActive(false);
        }
        else
        {
            LockUI.SetActive(true);
            ReadyUI.SetActive(false);
            FinishedUI.SetActive(false);
        }
    }
}