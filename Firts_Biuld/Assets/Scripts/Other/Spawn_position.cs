using System;
using UnityEngine;

public class Spawn_position : MonoBehaviour
{
    public GameObject playerPrefab; // ������ ������
    public Vector3 spawnPosition; // ������� ������
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
        // ������� ������ �� �������� �������
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        
    }
}
