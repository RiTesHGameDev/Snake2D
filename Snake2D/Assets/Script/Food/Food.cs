using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private ScoreController scoreController;
    [SerializeField] private FoodType foodtype;

    private void Start()
    {
        if (scoreController == null)
        {
            scoreController = FindObjectOfType<ScoreController>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Snake snake = collision.GetComponent<Snake>();
            if (snake != null)
            {
                int playerNumber = snake.IsPlayer1() ? 1 : 2;

                if (foodtype == FoodType.MASS_GAINER)
                {
                    snake.Grow();
                    if (scoreController != null)
                    {
                        scoreController.IncreaseScore(playerNumber);
                    }
                    Debug.Log("Snake and food collide");
                }
                if(foodtype == FoodType.MASS_BURNER)
                {
                    snake.Shrink();
                    Debug.Log("oops !Snake lenth decreased !");
                }
            }

            // Notify SpawnController
            if (SpawnController.SpawnControllerInstance != null)
            {
                SpawnController.SpawnControllerInstance.FoodEaten();
            }

            // Destroy this food object
            Destroy(gameObject);
        }
    }
}
