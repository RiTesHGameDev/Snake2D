using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("----- PowerUp Settings -----")]
    public PowerUpType powerUptype;
    public float duration = 5f;   
    public float lifeTime = 8f;   

    private void Start()
    {
        // Destroy if not collected within lifetime
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Snake snake = other.GetComponent<Snake>();
        if (snake != null)
        {
            ApplyEffect(snake);
            
        }
        SpawnController spawnController = FindObjectOfType<SpawnController>();
        if (spawnController != null)
        {
            spawnController.SpawnPowerUp();
        }

        Destroy(gameObject);
    }

    private void ApplyEffect(Snake snake)
    {
        switch (powerUptype)
        {
            case PowerUpType.SHIELD:
                snake.ActivateShield();
                break;

            case PowerUpType.SCOREBOOST:
                snake.ActivateScoreBoost();
                break;

            case PowerUpType.SPEEDUP:
                snake.ActivateSpeedUp();
                break;
        }
    }
}