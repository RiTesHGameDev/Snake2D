using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    [Header("----- MOVEMENT -----")]
    [SerializeField] private float moveSpeed; // Time between grid moves
    private float originalMoveSpeed;
    private float moveTimer;
    private Vector2Int gridPosition;

    [Header("----- SNAKE PARTS -----")]
    [SerializeField] private GameObject snakeHeadPrefab;
    [SerializeField] private GameObject snakeBodyPrefab;
    [SerializeField] private Transform bodyPart;
    [SerializeField] private Transform tailPart;
    [SerializeField] private int addBodyCount;
    [SerializeField] private int removeBodyCount;

    [Header("----- PowerUp Settings -----")]
    [SerializeField] private GameObject shieldEffectPrefab;
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private float speedBoostDuration = 5F;
    [SerializeField] private float scoreBoostDuration = 5f;

    [SerializeField] private PlayerController playerController;
    private List<Transform> snakeParts;
    private List<GameObject> activeShields = new List<GameObject>();
    private bool growThisStep = false;
    private bool shrinkThisStep = false;

    private int screenWidth;
    private int screenHeight;

    // Power-up states
    private bool hasShield = false;
    private bool hasSpeedBoost = false;
    private bool hasScoreBoost = false;
    private Coroutine speedBoostCoroutine;
   

    private void Awake()
    {
        //gridPosition = new Vector2Int(-20, 0);
        gridPosition = Vector2Int.RoundToInt(transform.position);
        moveTimer = moveSpeed;
        originalMoveSpeed = moveSpeed;

        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("playerController not found");
        }

        //initialize snake with 3 parts
        snakeParts = new List<Transform>();

        //Body parts attached by default
        snakeParts.Clear();
        snakeParts.Add(transform);

        if (bodyPart != null)
        {
            snakeParts.Add(bodyPart);
        }
        if (tailPart != null)
        {
            tailPart.rotation = Quaternion.identity;
            snakeParts.Add(tailPart);
        }
        screenHeight = Mathf.RoundToInt(Camera.main.orthographicSize * 2f);
        screenWidth = Mathf.RoundToInt(Camera.main.aspect * screenHeight);
    }
    private void Update()
    {
        HandleGridMovement();
    }
    private void HandleGridMovement()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            moveTimer += moveSpeed;
            SnakeMove();
        }

    }
    private void SnakeMove()
    {
        //Get direction from input
        Vector2Int moveDir = playerController.GetMoveDirection();
        if (moveDir == Vector2Int.zero) return;

        //update grid position
        Vector3 nextHeadWorld = new Vector3(gridPosition.x + moveDir.x, gridPosition.y + moveDir.y, 0);
        gridPosition += moveDir;

        //store old position
        Vector3 prevPos = transform.position;
        Vector3 tailPrevPos = snakeParts[snakeParts.Count - 1].position;

        //Apply to transfer
        transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);

        //Movement and Rotation Head
        //transform.position = nextHeadWorld;
        snakeParts[0].position = nextHeadWorld;
        HeadRotation(moveDir);
        HandleSccreenWrapping();

        //move body Parts
        for (int i = 1; i < snakeParts.Count; i++)
        {
            Vector3 tempPos = snakeParts[i].position;
            snakeParts[i].position = prevPos;
            prevPos = tempPos;
        }

        // grow
        if (growThisStep)
        {
            AddBodyPartAt(tailPrevPos);
            growThisStep = false;
        }
        //shrink
        if (shrinkThisStep)
        {
            ShrinkBodyPartAt(tailPrevPos);
            shrinkThisStep = false;
        }

        TailRotation();
    }
    private void HeadRotation(Vector2Int moveDir)
    {
        if (moveDir == Vector2Int.up)
            transform.rotation = Quaternion.Euler(0, 0, 90);   // Face up
        else if (moveDir == Vector2Int.down)
            transform.rotation = Quaternion.Euler(0, 0, -90);  // Face down
        else if (moveDir == Vector2Int.left)
            transform.rotation = Quaternion.Euler(0, 0, 180);  // Face left
        else if (moveDir == Vector2Int.right)
            transform.rotation = Quaternion.Euler(0, 0, 0);    // Face right
    }
    private void TailRotation()
    {
        if (snakeParts.Count < 2) return;

        Transform tail = snakeParts[snakeParts.Count - 1];
        Transform beforeTail = snakeParts[snakeParts.Count - 2];

        Vector2 moveDir = (tail.position - beforeTail.position).normalized;

        if (moveDir == Vector2.up)
            tail.rotation = Quaternion.Euler(0, 0, -90);   // Face up
        else if (moveDir == Vector2.down)
            tail.rotation = Quaternion.Euler(0, 0, 90);  // Face down
        else if (moveDir == Vector2.left)
            tail.rotation = Quaternion.Euler(0, 0, 0);  // Face left
        else if (moveDir == Vector2.right)
            tail.rotation = Quaternion.Euler(0, 0, 180);    // Face right
    }
    public void AddBodyPartAt(Vector3 pos)
    {
        if (snakeBodyPrefab == null)
        {
            Debug.LogError("Body Prefab not assigned on Snake.");
            return;
        }
        for (int i = 0; i < addBodyCount; i++)
        {
            GameObject newBody = Instantiate(snakeBodyPrefab, pos, Quaternion.identity);
            snakeParts.Insert(snakeParts.Count - 1, newBody.transform);

            // Glow effect
            SpriteRenderer sr = newBody.GetComponent<SpriteRenderer>();
            if (sr != null)
                StartCoroutine(GlowEffect(sr, Color.white, 0.5f));
        }

        UpdateShieldVisuals(); // Update shields after adding body parts
    }

    public void ShrinkBodyPartAt(Vector3 pos)
    {
        for (int i = 0; i < removeBodyCount; i++)
        {
            if (snakeParts.Count > 3)
            {
                Transform lastBodyPart = snakeParts[snakeParts.Count - 3];
                SpriteRenderer sr = lastBodyPart.GetComponent<SpriteRenderer>();

                if (sr != null)
                    StartCoroutine(ShrinkFlashAndDestroy(lastBodyPart.gameObject, sr, Color.red, 0.5f));
                else
                {
                    // fallback if no sprite renderer
                    snakeParts.RemoveAt(snakeParts.Count - 1);
                    Destroy(lastBodyPart.gameObject);
                }
            }
        }

        UpdateShieldVisuals(); // Update shields after removing body parts
    }
    private void UpdateShieldVisuals()
    {
        if (hasShield)
        {
            // If shield is active, recreate visuals to match current snake parts
            RemoveShieldVisuals();
            CreateShieldVisuals();
        }
    }
    private void CreateShieldVisuals()
    {
        RemoveShieldVisuals();

    foreach (Transform part in snakeParts)
    {
        if (part != null && part.gameObject.scene.IsValid()) //  ensures it's in scene
        {
            GameObject shield = Instantiate(shieldEffectPrefab, part.position, Quaternion.identity);
            shield.transform.SetParent(part, true); //  set parent after instantiation
            activeShields.Add(shield);
        }
    }
    }

    private void RemoveShieldVisuals()
    {
        // Remove all shield visuals
        foreach (GameObject shield in activeShields)
        {
            if (shield != null)
            {
                Destroy(shield);
            }
        }
        activeShields.Clear();
    }
    private IEnumerator GlowEffect(SpriteRenderer sr, Color glowColor, float duration)
    {
        Color original = sr.color;
        sr.color = glowColor;

        yield return new WaitForSeconds(duration);

        sr.color = original;
    }

    private IEnumerator ShrinkFlashAndDestroy(GameObject bodyPart, SpriteRenderer sr, Color flashColor, float duration)
    {
        Color original = sr.color;
        sr.color = flashColor;

        yield return new WaitForSeconds(duration);

        sr.color = original;

        // remove from list & destroy
        snakeParts.Remove(bodyPart.transform);
        Destroy(bodyPart);
    }
    public void Grow()
    {
        growThisStep = true;
    }
    public void Shrink()
    {
        shrinkThisStep = true;
    }
    private void HandleSccreenWrapping()
    {
        Vector3 pos = transform.position;

        // Wrap on X axis
        if (pos.x > screenWidth / 2)
        {
            pos.x = -screenWidth / 2;
            gridPosition.x = Mathf.RoundToInt(pos.x);

        }
        else if (pos.x < -screenWidth / 2)
        {
            pos.x = screenWidth / 2;
            gridPosition.x = Mathf.RoundToInt(pos.x);

        }

        // Wrap on Y axis
        if (pos.y > screenHeight / 2)
        {
            pos.y = -screenHeight / 2;
            gridPosition.y = Mathf.RoundToInt(pos.y);

        }
        else if (pos.y < -screenHeight / 2)
        {
            pos.y = screenHeight / 2;
            gridPosition.y = Mathf.RoundToInt(pos.y);

        }

        if (transform.position != pos)
        {
            transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
            return;

        // Check if the head hit one of its own body parts
        if (snakeParts.Contains(collision.transform) && collision.transform != snakeParts[0])
        {
            SnakeDie();
        }

        Snake otherSnake = collision.GetComponent<Snake>();
        if (otherSnake != null)
        {
            SnakeDie();
            if (otherSnake.snakeParts.Contains(collision.transform) && collision.transform != otherSnake.snakeParts[0])
            {
                SnakeDie();
                return;
            }
            return;
        }

    }

    private void SnakeDie()
    {
        if (hasShield == false)
        {
            Debug.Log("Snake died!");
            RemoveShieldVisuals(); // Clean up any shields
            enabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("Shielded !");
        }

    }

    public void ActivateShield()
    {
        StartCoroutine(ShieldDuration());
        CreateShieldVisuals();
    }

    public void ActivateSpeedUp()
    {
        // If there's already an active speed boost, stop it first
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        speedBoostCoroutine = StartCoroutine(SpeedBoostDuration());
    }

    public void ActivateScoreBoost()
    {
        StartCoroutine(ScoreBoostDuration());
    }

    private IEnumerator ShieldDuration()
    {
        hasShield = true;
        CreateShieldVisuals(); // Ensure visuals are created

        yield return new WaitForSeconds(shieldDuration);

        hasShield = false;
        RemoveShieldVisuals(); // Remove visuals when shield expire
    }

    private IEnumerator SpeedBoostDuration()
    {
        hasSpeedBoost = true;

        // Apply speed boost (half the time between moves = faster movement)
        moveSpeed = originalMoveSpeed * 0.5f;

        // Wait for the duration
        yield return new WaitForSeconds(speedBoostDuration);

        // Restore original speed
        moveSpeed = originalMoveSpeed;
        hasSpeedBoost = false;
        speedBoostCoroutine = null;
    }

    private IEnumerator ScoreBoostDuration()
    {
        hasScoreBoost = true;
        yield return new WaitForSeconds(scoreBoostDuration);
        hasScoreBoost = false;
    }
}
