using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private bool isPaused = false;

    private void Awake()
    {
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(OnResumeClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);


        menuPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    private void TogglePause()
    {
        SoundController.SoundInstance.PlayButtonClick();
        isPaused = !isPaused;
        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;          // Freeze game
        menuPanel.SetActive(true);    // Show pause menu
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;          // Resume game
        menuPanel.SetActive(false);   // Hide pause menu
        confirmPanel.SetActive(false);
        isPaused = false;
    }

    private void OnResumeClicked()
    {
        SoundController.SoundInstance.PlayButtonClick();
        ResumeGame();
    }

    private void OnRestartClicked()
    {
        SoundController.SoundInstance.PlayButtonClick();
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMainMenuClicked()
    {
        SoundController.SoundInstance.PlayButtonClick();
        // Show confirmation panel
        menuPanel.SetActive(false);
        confirmPanel.SetActive(true);
    }

    private void OnYesClicked()
    {
        SoundController.SoundInstance.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnNoClicked()
    {
        SoundController.SoundInstance.PlayButtonClick();
        confirmPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
