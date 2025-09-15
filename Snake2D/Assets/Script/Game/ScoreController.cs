using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI powerActivatedText;
    private int score;
    private int scoreMultiplier = 1;
    private int baseIncrement = 10; // Base increment amount

    private void Awake()
    {
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();

        if (powerActivatedText == null)
            powerActivatedText = GameObject.Find("PowerText")?.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        RefreshUI();
        ClearPowerText();
    }

    public void IncreaseScore()
    {
        int incrementAmount = baseIncrement * scoreMultiplier;
        score += incrementAmount;
        RefreshUI();
        
    }

    public void SetScoreMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
    }

    public IEnumerator ResetScoreMultiplier(float delay)
    {
        yield return new WaitForSeconds(delay);
        scoreMultiplier = 1;
    }

    private void RefreshUI()
    {
        scoreText.text = "Score :" + score;
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
}
