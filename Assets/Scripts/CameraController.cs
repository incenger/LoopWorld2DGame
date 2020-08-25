using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<Vector3> CameraPosition = new List<Vector3>();
    private int maxLevel = 1;
    private int curLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        maxLevel = 1;
        CameraPosition.Add(new Vector3(0, 0, -10));
        CameraPosition.Add(new Vector3(6, 0, -10));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
