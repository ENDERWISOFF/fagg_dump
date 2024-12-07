using UnityEngine;
using UnityEngine.UI;

public class Player_Vitality : MonoBehaviour
{
    public int maxHealth = 100;
    private int CurrentHealth;
    public int damageAmount = 10;

    [SerializeField] Image HealthBar;


    void Start()
    {

        HealthBar = GameObject.FindWithTag("Health").GetComponent<Image>();
        CurrentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collide");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageAmount);
        }
    }

    void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth < 0) CurrentHealth = 0;
        HealthBar.fillAmount = (float)CurrentHealth / maxHealth;

        // ≈сли здоровье упало до нул€, персонаж "умирает"
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
