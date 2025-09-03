using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private PlayerOneController playerOneController;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private float moveSpeed = 1f;
    private List<GameObject> segments = new List<GameObject>();
    private Vector2 lastPostion;
    private float timer = 0f;
    private void Start()
    {
        playerOneController = FindAnyObjectByType<PlayerOneController>();
        if (playerOneController == null)
        {
            Debug.LogError("PlayerOneCOntroller not found");
            return;
        }

        segments.Add(this.gameObject);

        for (int i = 0; i < 2; i++)
        {
            AddSegment();
        }
    }

    private void AddSegment()
    {
        GameObject newSegment = Instantiate(segmentPrefab,lastPostion,Quaternion.identity);
        segments.Add(newSegment);
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (playerOneController == null) return;
        transform.position = new Vector3(transform.position.x + playerOneController.direction.x,transform.position.y + playerOneController.direction.y,0f);
    }
}

