using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [Header("Interscene Manager Prefab")]
    [SerializeField] IntersceneManager intersceneManagerPrefab = default;
    IntersceneManager intersceneManagerInstance = default;
    public void CheckForIntersceneManager()
    {
        if (IntersceneManager.intersceneManager == null)
        {
            if (intersceneManagerPrefab != null)
            {
                intersceneManagerInstance = Instantiate(intersceneManagerPrefab);
                intersceneManagerInstance.SetUp();
            }
        }
        else
            intersceneManagerInstance = IntersceneManager.intersceneManager;
    }

    [Header("Important References")]
    [SerializeField] PlayerSpaceShipScript player = default;
    public PlayerSpaceShipScript GetPlayer { get { return player; } }

    [SerializeField] PlayerUI playerUI = default;
    public PlayerUI GetPlayerUI { get { return playerUI; } }

    [SerializeField] Camera mainCamera = default;
    public Camera GetMainCamera { get { return mainCamera; } }

    [SerializeField] GameScroller gameScroller = default;
    public GameScroller GetGameScroller { get { return gameScroller; } }

    [SerializeField] PoolingManager poolingManager = default;
    public PoolingManager GetPoolingManager { get { return poolingManager; } }

    [SerializeField] QuestCheckingManager questCheckingManager = default;
    public QuestCheckingManager GetQuestCheckingManager { get { return questCheckingManager; } }


    private void Awake()
    {
        gameManager = this;

        CheckForIntersceneManager();
        intersceneManagerInstance.GetQuestsIntersceneManagement.ResetQuests();
        intersceneManagerInstance.GetQuestsIntersceneManagement.AssignNewQuests();

        if (gameScroller != null)
            gameScroller.SetUp(player);

        poolingManager.CreatePoolsQueuesAndDictionary();

        player.GetRelatedDamageableComponent.OnLifeAmountChanged += playerUI.UpdateLifeBarFillAmount;
        questCheckingManager.OnValuesChanged += playerUI.UpdateProgressionTexts;

        OnStartGame += gameScroller.StartScroller;
        OnStartGame += playerUI.HideStartPanel;

        OnGameOver += gameScroller.StopScroller;
        OnGameOver += playerUI.ShowGameOverPanel;

        playerUI.ShowStartPanel();
        player.GetShootingSystem.OnWeaponChanged += playerUI.UpdateWeaponText;

        currentGameState = GameState.Starting;

        playerUI.OpenQuestsFrames(intersceneManagerInstance.GetQuestsIntersceneManagement.GetCurrentQuests);

        player.GetShootingSystem.SetUpWeaponSets(intersceneManagerInstance.GetQuestsIntersceneManagement.GetAllUnlockedWeapons);
    }

    public void StartGame()
    {
        currentGameState = GameState.Playing;
        OnStartGame?.Invoke();
        playerUI.CloseQuestsFrames();
    }
    System.Action OnStartGame;
    public void AddActionToOnStartGame(System.Action action)
    {
        OnStartGame += action;
    }

    public void GameOver()
    {
        currentGameState = GameState.Over;
        OnGameOver?.Invoke();

        intersceneManagerInstance.GetQuestsIntersceneManagement.CheckForFinishedQuests(questCheckingManager.GetTavelledDistance, questCheckingManager.GetNumberOfKilledEnemies, questCheckingManager.GetNumberOfBarellRoll);
        playerUI.OpenQuestsFrames(intersceneManagerInstance.GetQuestsIntersceneManagement.GetCurrentQuests);
    }
    System.Action OnGameOver;
    public void AddActionToOnGameOver(System.Action action)
    {
        OnGameOver += action;
    }

    GameState currentGameState;
    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.Starting:

                if (Input.GetKeyDown(KeyCode.Return))
                    StartGame();

                break;

            case GameState.Over:

                if (Input.GetKeyDown(KeyCode.Return))
                    RestartScene();

                break;
        }
    }

    bool restartingScene = false;
    public void RestartScene()
    {
        if (restartingScene)
            return;

        intersceneManagerInstance.GetQuestsIntersceneManagement.RemoveDoneQuests();

        restartingScene = true;

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}

public enum GameState { Starting, Playing, Over }