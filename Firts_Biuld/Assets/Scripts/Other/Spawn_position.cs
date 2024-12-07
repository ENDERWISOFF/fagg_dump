using System;
using UnityEngine;

public class Spawn_position : MonoBehaviour
{
    public GameObject playerPrefab; // Префаб игрока
    public Vector3 spawnPosition; // Позиция спавна
    public Spawn_position(GameObject playerPrefab, Vector3 spawnPosition)
    {
        this.playerPrefab = playerPrefab;
        this.spawnPosition = spawnPosition;
    }

    void Start()
    {   
        SpawnPlayer(spawnPosition);
    }

    void SpawnPlayer(Vector3 spawnPosition)
    {
        // Создаем игрока на заданной позиции
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        
    }
}
