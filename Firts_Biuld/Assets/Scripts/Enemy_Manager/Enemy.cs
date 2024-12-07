using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int health;
    [SerializeField] private int damage;

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int Health
    {
        get => health;
        set => health = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
