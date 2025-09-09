using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodType foodtype;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Snake snake = collision.GetComponent<Snake>();
            if (snake != null)
            {
                if(foodtype == FoodType.MASS_GAINER)
                {
                    snake.Grow();
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
