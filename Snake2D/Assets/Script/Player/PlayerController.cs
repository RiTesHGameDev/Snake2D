using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    private Vector2Int moveDirection;
    [SerializeField] private KeyCode moveUp;
    [SerializeField] private KeyCode moveDown;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode moveLeft;

    // Player identification
    public bool isPlayer1 = true;
    private void Awake()
    {
        if (gameController == null)
        {

            gameController = GetComponent<GameController>();
        }

        moveDirection = isPlayer1 ? new Vector2Int(1, 0) : new Vector2Int(-1, 0);
    }
    private void Update()
    {
        HandlePlayerInput();
    }

    public Vector2Int GetMoveDirection()
    {
        return moveDirection;
    }
    public void HandlePlayerInput()
    {
        if (Input.GetKeyDown(moveUp) && moveDirection.y != -1)
        {
            moveDirection = new Vector2Int(0, 1);
        }
        if (Input.GetKeyDown(moveDown) && moveDirection.y != 1)
        {
            moveDirection = new Vector2Int(0, -1);
        }
        if (Input.GetKeyDown(moveLeft) && moveDirection.x != 1)
        {
            moveDirection = new Vector2Int(-1, 0);
        }
        if (Input.GetKeyDown(moveRight) && moveDirection.x != -1)
        {
            moveDirection = new Vector2Int(1, 0);
        }
    }
}
