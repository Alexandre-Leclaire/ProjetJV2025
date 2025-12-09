using System;
using UnityEngine;

public class EnemieSpawner : MonoBehaviour  
{
    [SerializeField] private Enemie prefab;
    [SerializeField] private float maxSpawnTimer = 120f;
    [SerializeField] private float minSpawnTimer = 60f;

    private float timer;

    public bool CanSpawn { get; set; } = true;

    private float GetRandomTime()
    {
        return UnityEngine.Random.Range(minSpawnTimer, maxSpawnTimer);
    }
    
    private void Start()
    {
        timer = GetRandomTime();
    }

    private void Update()
    {
        if (!CanSpawn || gameObject.transform.childCount > 0) return;
        
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = GetRandomTime();
            Instantiate(prefab, transform);
        }
    }
}