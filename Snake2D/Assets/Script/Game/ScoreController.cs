using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreText2;
    [SerializeField] private TextMeshProUGUI powerActivatedText;
    private int scorePlayer1;
    private int scorePlayer2;
    private int scoreMultiplierPlayer1 = 1;
    private int scoreMultiplierPlayer2 = 1;
    private int baseIncrement = 10; // Base increment amount

    private void Awake()
    {
        if (scoreText == null)
            scoreText = GameObject.Find("p1_Score")?.GetComponent<TextMeshProUGUI>();

        if (scoreText2 == null)
            scoreText2 = GameObject.Find("p2_Score")?.GetComponent<TextMeshProUGUI>();

        if (powerActivatedText == null)
            powerActivatedText = GameObject.Find("ActivePower")?.GetComponent<TextMeshProUGUI>();

        SetTwoPlayerMode(false);
    }

    private void Start()
    {
        RefreshUI();
        ClearPowerText();
    }
    public void SetTwoPlayerMode(bool isTwoPlayer)
    {
        if (scoreText2 != null)
        {
            scoreText2.gameObject.SetActive(isTwoPlayer);
        }
    }

    public void IncreaseScore(int playerNumber)
    {
        if (playerNumber == 1)
        {
            int incrementAmount = baseIncrement * scoreMultiplierPlayer1;
            scorePlayer1 += incrementAmount;
        }
        else
        {
            int incrementAmount = baseIncrement * scoreMultiplierPlayer2;
            scorePlayer2 += incrementAmount;
        }
        RefreshUI();
    }

    public void SetScoreMultiplier(int playerNumber, int multiplier)
    {
        if (playerNumber == 1)
            scoreMultiplierPlayer1 = multiplier;
        else if (playerNumber == 2)
            scoreMultiplierPlayer2 = multiplier;
    }

    public IEnumerator ResetScoreMultiplier(int playerNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerNumber == 1)
            scoreMultiplierPlayer1 = 1;
        else if (playerNumber == 2)
            scoreMultiplierPlayer2 = 1;
    }

    private void RefreshUI()
    {
        scoreText.text = "Score: " + scorePlayer1;

        // Only update player 2 score if it's active
        if (scoreText2 != null && scoreText2.gameObject.activeSelf)
        {
            scoreText2.text = "Score: " + scorePlayer2;
        }
    }

    public void ActivePower(string power)
    {
        if (powerActivatedText != null)
        {
            powerActivatedText.text = "Power : " + "[ " + power + " ]";
            StartCoroutine(ClearPowerTextAfterDelay(5f));
        }
    }

    public void ClearPowerText()
    {
        if (powerActivatedText != null)
            powerActivatedText.text = "";
    }

    private IEnumerator ClearPowerTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearPowerText();
    }

    public int GetScore(int playerNumber)
    {
        return playerNumber == 1 ? scorePlayer1 : scorePlayer2;
    }
}
