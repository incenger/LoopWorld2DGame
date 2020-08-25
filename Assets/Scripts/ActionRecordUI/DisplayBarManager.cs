using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayBarManager : MonoBehaviour
{
    private List<GameObject> actionsRecord = new List<GameObject>();
    public GameObject GameManagement;
    public string barName;
    [SerializeField] private Color actionColor;
    [SerializeField] private GameObject barPrefab;
    private float totalTime = 20f;
    public float barLength;

    public List<Action> actions;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectObject = gameObject.GetComponent<RectTransform>() as RectTransform;
        barLength = rectObject.rect.width;
    }

    void ConfigActionRecord(GameObject actionRecObj, float startPos, float durationLength)
    {
        actionRecObj.layer = 9;
        RectTransform trans = actionRecObj.GetComponent<RectTransform>();
        trans.anchoredPosition = new Vector3(startPos, 0, 0);
        trans.sizeDelta = new Vector2(durationLength, 40);
        trans.localScale = Vector3.one;
        Image image = actionRecObj.GetComponent<Image>();
        image.color = actionColor;
    }
    void Draw(List<Action> actions)
    {
        foreach (GameObject rec in actionsRecord)
        {
            Destroy(rec);
        }
        actionsRecord.Clear();
        if (barName == "Shoot" || barName == "Jump")
        {
            float durationLength = 0.01f * barLength;
            foreach (Action action in actions)
            {
                float startPos       = (action.StartTime / totalTime) * barLength;
                GameObject actionRecObj = Instantiate(barPrefab, gameObject.transform);
                ConfigActionRecord(actionRecObj, startPos, durationLength);
                actionsRecord.Add(actionRecObj);
            }
        }
        else 
        {
            foreach (MoveAction action in actions)
            {
                float durationLength = (action.Duration / totalTime) * barLength;
                float startPos       = (action.StartTime / totalTime) * barLength;
                if (action.Direction > 0)
                {
                    actionColor = new Color(0.22f, 0.66f, 0.5f, 1);
                }
                else 
                {
                    actionColor = new Color(0.99f, 0.8f, 0.6f, 1);
                }
                GameObject actionRecObj = Instantiate(barPrefab, gameObject.transform);
                ConfigActionRecord(actionRecObj, startPos, durationLength);
                actionsRecord.Add(actionRecObj);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (actions != null)
            this.Draw(actions);
    }
}
