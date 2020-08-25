using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndRoundController : MonoBehaviour
{
    public Button NextButton;

    public void SetListener(GameController gameController)
    {
        Debug.Log("Set Listener");
        NextButton.onClick.AddListener(gameController.MoveNextScene);
    }

}
