using UnityEngine;
using System.Collections;

public class Enemy_Manager : MonoBehaviour
{
    public int baseHealth = 50;
    public int baseDamage = 10;

    [SerializeField] private GameObject[] enemies;
    private int current_level;

    void Start()
    {
        StartCoroutine(InitializeAfterDelay());
    }

    private IEnumerator InitializeAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (Level_Manager.Instance == null)
        {
            Debug.LogError("Level_Manager.Instance is null!");
            yield break;
        }

        current_level = Level_Manager.Instance.CurrentLevel;
        InitializeEnemies();
    }

    private void InitializeEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Debug.LogWarning("No enemies found with tag 'Enemy'!");
            return;
        }

        foreach (var enemy in enemies)
        {
            AssignLevel(enemy, current_level);
        }
    }

    private void AssignLevel(GameObject enemy_object, int level)
    {
        Enemy enemy = enemy_object.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogWarning($"GameObject {enemy_object.name} does not have an Enemy component!");
            return;
        }

        enemy.Level = level;

        if (enemy.name == "Slime")
        {
            enemy.Health = baseHealth + level * 10;
            enemy.Damage = baseDamage + level * 2;
            Debug.Log("Level Assigned");
        }
        else if (enemy.name == "Skeleton")
        {
            enemy.Health = baseHealth + level * 15;
            enemy.Damage = baseDamage + level * 3;
            Debug.Log("Level Assigned");
        }
        else if (enemy.name == "Ghost")
        {
            enemy.Health = baseHealth + level * 20;
            enemy.Damage = baseDamage + level * 4;
            Debug.Log("Level Assigned");
        }
        else
        {
            enemy.Health = baseHealth + level * 5;
            enemy.Damage = baseDamage + level * 2;
        }

        Debug.Log($"Enemy {enemy.name} initialized! Level: {enemy.Level}, Health: {enemy.Health}, Damage: {enemy.Damage}");
    }
}
