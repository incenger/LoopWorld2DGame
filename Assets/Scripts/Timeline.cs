using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private GameObject character;
    public List<Action> _actionList = new List<Action>();

    private float EPSILON = 1e-3f;

    CharacterController2D m_CharacterController2D;
    ActionManager m_ActionManager;
    public bool Locked {get; set;}

    private void Awake() {
        m_CharacterController2D = character.GetComponent<CharacterController2D>();
        m_ActionManager = new ActionManager(m_CharacterController2D);
    }

    public void AddAction(Action action) {
        if (!Locked)
            _actionList.Add(action);
    }

    public void Clear() {
        _actionList.Clear();
    }

    public int Length() {
        return _actionList.Count;
    }


    public void Replay() {
        if (!Locked)
            return;
        foreach (var action in _actionList) {
            Debug.Log("Action: " + action.StartTime + ", " + action.Duration + ", " + action.CallCount);
            var coroutine = RepeatAction(action);
            StartCoroutine(coroutine);
        }
    }

    public void UpdateDurationLastAction(float duration)
    {
        _actionList[_actionList.Count - 1].Duration = duration;
    }

    public void UpdateDirectionLastAction(float direction)
    {
        MoveAction mv_action = _actionList[_actionList.Count - 1] as MoveAction;
        mv_action.Direction = direction;
    }

    private IEnumerator RepeatAction(Action action) {

        // Debug.Log("Action: " + action.Duration + ", " + action.StartTime);
        yield return new WaitForSeconds(action.StartTime);

        float elapsedTime = 0.0f;
        do
        {
            m_ActionManager.ExecuteAction(action);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        } while(elapsedTime + 0.1f  < action.Duration);
    } 
}
