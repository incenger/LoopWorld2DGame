using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNavigation : MonoBehaviour
{
    // Start is called before the first frame update
    public Button PauseButton;
    public GameObject PauseMenu;
    public bool isActivePauseMenu = false;
    public void PauseHandler() {
        isActivePauseMenu = !isActivePauseMenu;
        PauseMenu.SetActive(isActivePauseMenu);
    }
    void Start()
    {
        PauseButton.onClick.AddListener(PauseHandler);
        PauseMenu.SetActive(isActivePauseMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
