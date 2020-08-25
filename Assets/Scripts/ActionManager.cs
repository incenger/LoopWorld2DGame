using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private CharacterController2D m_CharacterController2D;

    public ActionManager(CharacterController2D characterController2D)
    {
        m_CharacterController2D = characterController2D;
    }

    public void ExecuteAction(Action action)
    {
        Debug.Log("Execute action");
        if (action is JumpAction)
        {
            m_CharacterController2D.Jump();
        } else if (action is ShootAction) {
            m_CharacterController2D.Fire();
        } else if (action is MoveAction) {
            MoveAction moveAction = (MoveAction) action;
            m_CharacterController2D.Move(moveAction.Direction);
        }

    }
}
