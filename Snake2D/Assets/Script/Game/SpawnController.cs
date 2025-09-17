using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController SpawnControllerInstance;
    [Header("----- Snake -----")]
    [SerializeField] private GameObject snake1;
    [SerializeField] private GameObject snake2;

    [Header("----- Food Prefabs -----")]
    [SerializeField] private GameObject massGainerPrefab;
    [SerializeField] private GameObject massBurnerPrefab;

    [Header("----- Power-Up Prefabs -----")]
    [SerializeField] private GameObject[] powerUpPrefabs;

    [Header("----- Spawn Area -----")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("----- Spawn Timings -----")]
    [SerializeField] private float minFoodSpawnTime;
    [SerializeField] private float maxFoodSpawnTime;
    [SerializeField] private float minPowerUpSpawnTime ;
    [SerializeField] private float maxPowerUpSpawnTime ;

    private PowerUpType powerUpType;
    private FoodType foodType;

    private GameObject currentFood;
    private GameObject currentPowerUp;

    public float lifeTime;

    private void Awake()
    {
        if(SpawnControllerInstance == null)
        {
            SpawnControllerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(SpawnFoodRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public void SpawnPlayerOne()
    {
        snake1.gameObject.SetActive(true);
    }

    public void SpawnPlayerTwo()
    {
        snake2.gameObject.SetActive(true);
    }
    private IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            if (currentPowerUp == null) SpawnPowerUp();
            yield return new WaitForSeconds(Random.Range(minFoodSpawnTime, maxFoodSpawnTime));
            
        }
    }
    private IEnumerator SpawnFoodRoutine()
    {
        while (true)
        {
            if (currentFood == null) SpawnFood();
            yield return new WaitForSeconds(Random.Range(minPowerUpSpawnTime, maxPowerUpSpawnTime));
        }
    }

    public void SpawnPowerUp()
    {
        GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        Vector2 pos = GetRandomPosition();
        currentPowerUp = Instantiate(prefab, pos, Quaternion.identity);

        // Auto-destroy after lifetime
        Destroy(currentPowerUp, lifeTime);
    }

    public void PowerUpCollected()
    {
        if (currentPowerUp != null) Destroy(currentPowerUp);
        currentPowerUp = null;
        SpawnPowerUp();
    }

    public void SpawnFood()
    {
        if (currentFood != null) Destroy(currentFood);

        // Decide which food to spawn(70 % mass gainer, 30 % mass burner)
        bool spawnMassGainer = Random.value > 0.3;
        GameObject foodPrefab = spawnMassGainer ? massGainerPrefab : massBurnerPrefab;

        Vector2 spawnPosition = GetRandomPosition();
        currentFood = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
    public void FoodEaten()
    {
        if (currentFood != null) Destroy(currentFood);
        currentFood = null;
        SpawnFood();
    }


    private Vector2Int GetRandomPosition()
    {
        int x = Random.Range(-width / 2, width / 2);
        int y = Random.Range(-height / 2, height / 2);
        return new Vector2Int(x, y);
    }
}
