using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI MAIN MENU")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button onePlayer;
    [SerializeField] private Button twoPlayer;
    [SerializeField] private GameObject playerOptionPanel;

    private GameController gameController;
    private void Awake()
    {
        gameController = GameController.gameControllerInstance;
        if (gameController == null)
        {
            GameObject controllerObj = new GameObject("GameController");
            gameController = controllerObj.AddComponent<GameController>();
        }
        // If buttons aren't assigned in inspector, try to find them
        if (playButton == null)
            playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        if (quitButton == null)
            quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        if (backButton == null)
            backButton = GameObject.Find("BackButton").GetComponent<Button>();
        if (onePlayer == null)
            onePlayer = GameObject.Find("OnePlayer").GetComponent<Button>();
        if (twoPlayer == null)
            twoPlayer = GameObject.Find("TwoPlayer").GetComponent<Button>();
        if(playerOptionPanel == null)
            playerOptionPanel = GameObject.Find("PlayerOptionPanel");

        // Add event listeners
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
        onePlayer.onClick.AddListener(OnOnePlayerClicked);
        twoPlayer.onClick.AddListener(OnTwoPlayerClicked);

        if (playerOptionPanel != null)
            playerOptionPanel.SetActive(false);
    }

    private void OnTwoPlayerClicked()
    {
        gameController?.SetPlayerCount(2);
        gameController?.SetTwoPlayerMode();
        gameController?.LoadGameScene();
    }

    private void OnOnePlayerClicked()
    {
        gameController?.SetPlayerCount(1);
        gameController?.SetSinglePlayerMode();
        gameController?.LoadGameScene();
    }

    private void OnBackButtonClicked()
    {
        HidePlayerOptions();
    }

    private void OnPlayButtonClicked()
    {
        ShowPlayerOptions();
    }

    private void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked!");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    private void ShowPlayerOptions()
    {
        if (playerOptionPanel != null)
            playerOptionPanel.SetActive(true);
    }
    private void HidePlayerOptions()
    {
        if (playerOptionPanel != null)
            playerOptionPanel.SetActive(false);
    }
}
