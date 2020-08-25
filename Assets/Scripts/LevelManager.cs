using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LevelManager
{
    [SerializeField] private Vector3 initPlayerPosition;
    [SerializeField] private List<Vector3> initEnemiesPosition = null;
    [SerializeField] private Vector3 initGoalPosition;
    private Vector3 initUIPosition;
    public Vector3 cameraPosition = new Vector3(0, 0, -10);
    [SerializeField] public bool isMovingLock;
    [SerializeField] public bool isJumpingLock;
    [SerializeField] public bool isAttackingLock;

    private GameObject playerPrefab;
    private GameObject enemyPrefab;
    private GameObject goalPrefab;
    private GameObject levelControllerPrefab;
    private GameController gameController;
    private bool isFinished;
    private bool isPlaying;
    private bool isReady;
    public int levelID;

    private List<GameObject> enemiesList = new List<GameObject>();
    public GameObject player { get; private set; }
    private GameObject goal;
    private GameObject levelUI = null;
    public void InitLevel()
    {
        // Create camera position
        cameraPosition = new Vector3(18 * levelID, 0, -10);
        initUIPosition = new Vector3(18 * levelID, 0, 0);
        // Create player object
        player = MonoBehaviour.Instantiate(playerPrefab, initPlayerPosition, Quaternion.identity);
        // Initialize characters
        CharacterController2D characterController = player.GetComponent<CharacterController2D>();
        Debug.Log("Char level " + levelID);
        characterController.setLocked(isMovingLock, isJumpingLock, isAttackingLock);
        characterController.addController(gameController);
        // Create enemies
        foreach (Vector3 enemyPos in initEnemiesPosition)
        {
            var enemy = MonoBehaviour.Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
            Enemy enemyController = enemy.GetComponent<Enemy>();
            enemyController.setPlayer(player);
            enemiesList.Add(enemy);
        }
        // Create goal
        goal = MonoBehaviour.Instantiate(goalPrefab, initGoalPosition, Quaternion.identity);
        // Create UI
        if (levelUI == null) levelUI = MonoBehaviour.Instantiate(levelControllerPrefab, initUIPosition, Quaternion.identity);
        var levelController = levelUI.GetComponent<LevelUI>();
        levelController.setLevelLock(isAttackingLock, isMovingLock, isJumpingLock, initUIPosition);
    }
    public void InitGameObject(GameController gameController, GameObject playerPrefab = null, GameObject enemyPrefab = null, GameObject goalPrefab = null, GameObject levelControllerPrefab = null)
    {
        this.gameController = gameController;
        this.playerPrefab = playerPrefab;
        this.enemyPrefab = enemyPrefab;
        this.goalPrefab = goalPrefab;
        this.levelControllerPrefab = levelControllerPrefab;
    }
    public void ClearLevel(int currentLevel, bool isPlaying)
    {
        // Destory game object
        if (player != null) MonoBehaviour.Destroy(player);
        foreach (GameObject enemy in enemiesList)
            if (enemy != null) MonoBehaviour.Destroy(enemy);
        if (goal != null) MonoBehaviour.Destroy(goal);
        // Reset value
        player = null;
        goal = null;
        enemiesList.Clear();
        this.InitLevel();
        // Update UI
        this.UpdateState(currentLevel, isPlaying);
    }

    public void UpdateState(int currentLevel, bool isPlaying)
    {
        if (currentLevel > levelID) isFinished = true;
        else isFinished = false;
        if (currentLevel == levelID) isReady = true;
        else isReady = false;
        if (currentLevel == levelID) this.isPlaying = isPlaying;
        else this.isPlaying = false;
        // Update UI
        LevelUI levelController = levelUI.GetComponent<LevelUI>();
        levelController.setState(((currentLevel == levelID) && !this.isPlaying), this.isPlaying, isFinished);
        // Update player
        CharacterController2D characterController = player.GetComponent<CharacterController2D>();
        characterController.setFreeze(this.isPlaying);
        // Update enemy
        foreach (GameObject enemy in enemiesList)
        {
            Enemy enemyController = enemy.GetComponent<Enemy>();
            enemyController.setFreeze(this.isPlaying);
        }
    }
}