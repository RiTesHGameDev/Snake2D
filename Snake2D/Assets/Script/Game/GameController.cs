using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameControllerInstance { get; private set; }
    [SerializeField] private ScoreController scoreController;
    [Header("Player Prefabs")]
    [SerializeField] private GameObject playerPrefab; 
    [SerializeField] private GameObject playerPrefab2; 

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private int playerCount = 1;
    private bool isLoading = false;

    public bool isTwoPlayerMode = false;
    private void Awake()
    {
        // Singleton pattern
        if (gameControllerInstance == null)
        {
            gameControllerInstance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetSinglePlayerMode()
    {
        isTwoPlayerMode = false;
        if (scoreController != null)
            scoreController.SetTwoPlayerMode(false);
    }

    public void SetTwoPlayerMode()
    {
        isTwoPlayerMode = true;
        if (scoreController != null)
            scoreController.SetTwoPlayerMode(true);
    }
    public void SetPlayerCount(int count)
    {
        playerCount = Mathf.Clamp(count, 1, 2);
    }

    public void LoadGameScene()
    {
        Debug.Log("LoadGameScene called");
        if (isLoading) { Debug.Log("Already loading!"); return; }
        StartCoroutine(LoadLevelAsync());
    }

    private IEnumerator LoadLevelAsync()
    {
        isLoading = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
        isLoading = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            scoreController = FindObjectOfType<ScoreController>();
            if (scoreController != null)
            {
                scoreController.SetTwoPlayerMode(isTwoPlayerMode);
            }
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
                spawnPoints = new Transform[spawnObjects.Length];
                for (int i = 0; i < spawnObjects.Length; i++)
                {
                    spawnPoints[i] = spawnObjects[i].transform;
                }
            }
            Debug.Log("Found " + spawnPoints.Length + " spawn points in scene.");
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        if (playerCount <= 0)
        {
            Debug.LogWarning("Player count was not set! Defaulting to 1.");
            playerCount = 1;
        }
        if (spawnPoints == null || spawnPoints.Length < playerCount)
        {
            Debug.LogError("Spawn points not set up properly!");
            return;
        }

        if (playerCount == 1)
        {
            SpawnSinglePlayer();
        }
        else if (playerCount == 2)
        {
            SpawnTwoPlayers();
        }
    }

    private void SpawnSinglePlayer()
    {
        if (playerPrefab != null && spawnPoints.Length > 0)
        {
            Instantiate(playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation)
                .name = "Snake 1";
        }
    }

    private void SpawnTwoPlayers()
    {
        if (playerPrefab != null && spawnPoints.Length >= 2)
        {
            // Player 1
            Instantiate(playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation)
                .name = "Snake 1";

            // Player 2
            GameObject prefabToUse = playerPrefab2 != null ? playerPrefab2 : playerPrefab;
            Instantiate(prefabToUse, spawnPoints[1].position, spawnPoints[1].rotation)
                .name = "Snake 2";
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}