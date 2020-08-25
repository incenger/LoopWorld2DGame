using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject goalPrefab;
    private ActionRecordUI ActionUIController;
    private EndRoundController EndRoundUIContorller;
    [SerializeField] private GameObject levelControllerPrefab;
    [SerializeField] private GameObject EndRoundUI;
    [SerializeField] private GameObject Camera;
    [SerializeField] private Vector3 WideCameraPosition = new Vector3();
    [SerializeField] private float WideCameraSize = 20f;
    [SerializeField] private List<LevelManager> LevelList = new List<LevelManager>();
    [SerializeField] private GameObject ActionUI;

    public float maxTime = 20f;

    public float StartTime;

    public string NextLevelSceneName;
    int currentLevel = 0;

    int choosingLevel = 0;

    public bool isPlaying = false;
    bool isWinning = false;

    bool isWideCamera = false;

    void Awake()
    {
        ActionUIController = ActionUI.GetComponent<ActionRecordUI>();
        ActionUIController.gameController = this;
        EndRoundUIContorller = EndRoundUI.GetComponent<EndRoundController>();
        EndRoundUIContorller.SetListener(this);
        int levelID = 0;
        foreach (LevelManager level in LevelList)
        {
            level.InitGameObject(this, playerPrefab, enemyPrefab, goalPrefab, levelControllerPrefab);
            level.levelID = levelID;
            level.InitLevel();
            level.UpdateState(currentLevel, isPlaying);
            levelID += 1;
        }
        EndRoundUI.SetActive(false);
        ActionUI.SetActive(true);
    }

    public void Review()
    {

    }
    public void MoveNextScene()
    {
        SoundManager.PlaySound("Click");
        Debug.Log("Move next scene");
        SceneManager.LoadScene(NextLevelSceneName);
    }

    void LoopAction()
    {
        if (currentLevel == 0) return;
        bool nxtMovingLock = LevelList[currentLevel].isMovingLock;
        bool nxtJumpingLock = LevelList[currentLevel].isJumpingLock;
        bool nxtAttackLock = LevelList[currentLevel].isAttackingLock;

        GameObject nx_player = LevelList[currentLevel].player;
        var nx_playerController = nx_player.GetComponent<CharacterController2D>();
        GameObject player = LevelList[currentLevel - 1].player;
        var playerController = player.GetComponent<CharacterController2D>();
        if (nxtAttackLock)
            nx_playerController.m_TimelineManager.m_ShootTimeline._actionList = playerController.m_TimelineManager.m_ShootTimeline._actionList;
        else if (nxtJumpingLock)
            nx_playerController.m_TimelineManager.m_JumpTimeline._actionList = playerController.m_TimelineManager.m_JumpTimeline._actionList;
        else if (nxtMovingLock)
            nx_playerController.m_TimelineManager.m_MoveTimeline._actionList = playerController.m_TimelineManager.m_MoveTimeline._actionList;
    }

    void EndRound()
    {
        toggleWideCamera();
        isWinning = true;
        isPlaying = false;
        ActionUI.SetActive(false);
        EndRoundUI.SetActive(true);
    }

    public void FinishLevel()
    {
        currentLevel += 1;
        choosingLevel += 1;
        isPlaying = false;
        SoundManager.PlaySound("Finish");
        foreach (LevelManager level in LevelList)
        {
            level.UpdateState(currentLevel, isPlaying);
        }
        if (currentLevel == LevelList.Count)
        {
            this.EndRound();
            return;
        }
    }

    void StartGame()
    {
        this.LoopAction();
        isPlaying = true;
        StartTime = Time.time;
        LevelList[currentLevel].UpdateState(currentLevel, isPlaying);
        var playerController = LevelList[currentLevel].player.GetComponent<CharacterController2D>();
        playerController.Replay();
    }

    public void Replay()
    {
        Debug.Log("New game");
        StartTime = Time.time;
        currentLevel = choosingLevel;
        isPlaying = true;
        foreach (LevelManager level in LevelList)
        {
            if (level.levelID < choosingLevel) continue;
            level.ClearLevel(currentLevel, isPlaying);
        }
        StartGame();
    }

    public void GameOver()
    {
        isPlaying = false;
        LevelList[currentLevel].ClearLevel(currentLevel, isPlaying);
    }

    void UpdateActionBarUI()
    {
        GameObject player = LevelList[currentLevel].player;
        var playerController = player.GetComponent<CharacterController2D>();
        ActionUIController.UpdateUITimeline(playerController.m_TimelineManager.m_ShootTimeline,
                                            playerController.m_TimelineManager.m_JumpTimeline,
                                            playerController.m_TimelineManager.m_MoveTimeline);
    }

    void toggleWideCamera()
    {
        isWideCamera = !isWideCamera;
        if (isWideCamera)
        {
            Camera.transform.position = WideCameraPosition;
            var cameraProperty = Camera.GetComponent<Camera>();
            cameraProperty.orthographicSize = WideCameraSize;
        }
        else
        {
            Camera.transform.position = LevelList[choosingLevel].cameraPosition;
            var cameraProperty = Camera.GetComponent<Camera>();
            cameraProperty.orthographicSize = 14f;
        }
    }

    void Update()
    {
        if (isWinning)
        {
            return;
        }
        this.UpdateActionBarUI();
        if (isPlaying)
        {
            var playingTime = Time.time - StartTime;
            if (playingTime > maxTime)
            {
                GameOver();
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameOver();
                return;
            }
            return;
        }
        else if (isWinning)
        {
            return;
        }
        else if (isWideCamera)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                toggleWideCamera();
            }
            return;
        }
        else
        {
            Camera.transform.position = LevelList[choosingLevel].cameraPosition;
            var cameraProperty = Camera.GetComponent<Camera>();
            cameraProperty.orthographicSize = 14f;

        }
        if (Input.GetKeyDown(KeyCode.Return) && choosingLevel == currentLevel)
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.R) && choosingLevel < currentLevel)
        {
            this.Replay();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && choosingLevel > 0)
        {
            choosingLevel -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && choosingLevel < LevelList.Count - 1)
        {
            choosingLevel += 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleWideCamera();
        }
    }
}