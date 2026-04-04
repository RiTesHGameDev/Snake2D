using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private ScoreController scoreController;
    [Header("----- PowerUp Settings -----")]
    public PowerUpType powerUptype;
    public float duration = 5f;
    public float lifeTime = 8f;

    private void Start()
    {
        if (scoreController == null)
        {
            scoreController = FindObjectOfType<ScoreController>();
            if (scoreController == null)
            {
                Debug.LogError("ScoreController not found!");
            }
        }
        // Destroy if not collected within lifetime
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SoundController.SoundInstance.PlayPowerPickupSound();
        Snake snake = other.GetComponent<Snake>();
        if (snake != null)
        {
            int playerNumber = snake.IsPlayer1() ? 1 : 2;
            ApplyEffect(snake, playerNumber);

        }
        SpawnController spawnController = FindObjectOfType<SpawnController>();
        if (spawnController != null)
        {
            spawnController.SpawnPowerUp();
        }

        Destroy(gameObject);
    }

    private void ApplyEffect(Snake snake, int playerNumber)
    {
        switch (powerUptype)
        {
            case PowerUpType.SHIELD:
                snake.ActivateShield();
                if (scoreController != null)
                    scoreController.ActivePower("Shield");
                break;

            case PowerUpType.SCOREBOOST:
                snake.ActivateScoreBoost();
                if (scoreController != null)
                {
                    scoreController.SetScoreMultiplier(playerNumber, 2);
                    scoreController.IncreaseScore(playerNumber);
                    scoreController.ActivePower("Score Boost 2x");
                    scoreController.StartCoroutine(scoreController.ResetScoreMultiplier(playerNumber, 5f));
                }
                break;

            case PowerUpType.SPEEDBOOST:
                snake.ActivateSpeedUp();
                if (scoreController != null)
                    scoreController.ActivePower("Speed Boost");
                break;
        }
    }
}