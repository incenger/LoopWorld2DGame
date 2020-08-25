using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public Button newGameButton;
    public Button exitGameButton;
    public Button increaseResolution;
    public Button decreaseResolution;
    public Text resolutionText;
    Resolution[] resolutions;
    // Start is called before the first frame update
    public string newGameSceneName;
    int currentResolutionIndex;
    public void NewGame() {
        SoundManager.PlaySound("Click");
        SceneManager.LoadScene(newGameSceneName);
    }
    public void ExitGame() {
        Debug.Log("Exit Game!!!");
        Application.Quit();
    }

    void IncRes()
    {
        if (currentResolutionIndex == resolutions.Length - 1)
            return;
        currentResolutionIndex += 1;
        updateResolution();
    }

    void DecRes()
    {
        if (currentResolutionIndex <= resolutions.Length - 3)
            return;
        currentResolutionIndex -= 1;
        updateResolution();
    }

    void updateResolution()
    {
        int width = resolutions[currentResolutionIndex].width;
        int height = resolutions[currentResolutionIndex].height;
        Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
        resolutionText.text = width + " x " + height;
    }

    void Awake() {
        newGameButton.onClick.AddListener(NewGame);
        exitGameButton.onClick.AddListener(ExitGame);
        increaseResolution.onClick.AddListener(IncRes);
        decreaseResolution.onClick.AddListener(DecRes);
        resolutions = Screen.resolutions;
        currentResolutionIndex = resolutions.Length - 1;
        updateResolution();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
