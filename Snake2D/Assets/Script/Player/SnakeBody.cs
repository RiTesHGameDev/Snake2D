using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    [Header("Body Properties")]
    [SerializeField] private Snake parentSnake;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle when another snake's head hits this body part
        Snake otherSnake = collision.GetComponent<Snake>();
        if (otherSnake != null && otherSnake != parentSnake)
        {
            if (collision.transform == otherSnake.GetSnakeparts(0)) // Only if it's the head
            {
                // The snake that hit this body part dies
                otherSnake.SnakeDie();
            }
        }
    }
    public Snake GetParentSnake()
    {
        return parentSnake;
    }
    public void SetParentSnake(Snake snake)
    {
        parentSnake = snake;
    }
}