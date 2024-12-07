using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public int Health = 5000;
        public float CurrentHealth;
        public int damageAmount = 100;
        public float defenceAttribute = 0f;

        private Image HealthBar;
        private Image StaminaBar;

        private float horizontal;
        private float vertical;
        private Rigidbody2D rb;
        private Animator animator;

        public float speed = 1f;
        public float acceleration = 1f;

        public float CurrenStamina;
        public float MaxinunStamina;

        public float RunCost;
        public float recharge_rate;
        private Coroutine recharge;

        private static Player instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject); // Уничтожаем новый экземпляр, если он уже есть
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            HealthBar = GameObject.FindWithTag("Health").GetComponent<Image>();
            StaminaBar = GameObject.FindWithTag("Stamina").GetComponent<Image>();
            CurrentHealth = Health;
            CurrenStamina = MaxinunStamina;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

        }

        void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontal * speed, vertical * speed);




            if (movement.magnitude > 1)
            {
                movement.Normalize();
            }

            if (Input.GetKey(KeyCode.LeftShift) && movement.magnitude > 0)
            {
                if (CurrenStamina > 0)
                {
                    rb.linearVelocity = movement * (speed + acceleration);
                    CurrenStamina -= RunCost * Time.deltaTime;
                }
                else
                {
                    rb.linearVelocity = movement * speed;
                }

                if (CurrenStamina < 0) CurrenStamina = 0;
                StaminaBar.fillAmount = CurrenStamina / MaxinunStamina;

                if (recharge != null) StopCoroutine(recharge);
                recharge = StartCoroutine(RechargeStamina());


            }
            else
            {
                rb.linearVelocity = movement * speed;
            }





            if (horizontal > 0)
            {
                GetComponent<Animator>().Play("Right");
            }
            else if (horizontal < 0)
            {
                GetComponent<Animator>().Play("Left");
            }
            else if (vertical > 0)
            {
                GetComponent<Animator>().Play("Up");
            }
            else if (vertical < 0)
            {
                GetComponent<Animator>().Play("Down");
            }
            else
            {
                // Если никаких клавиш не нажато, можно установить состояние "Стоп"
                animator.Play("Idle"); // Или любое другое состояние бездвижения
            }
        }
        //private IEnumerator WaitToRun()
        //{
        //    canRun = false;
        //    yield return new WaitForSeconds(3f);
        //    canRun = true;
        //}
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    FindObjectOfType<BattleManager>().StartBattle(this, enemy);
                }
            }
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage - defenceAttribute;

            if (CurrentHealth < 0) CurrentHealth = 0;
            HealthBar.fillAmount = (float)CurrentHealth / Health;

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        private IEnumerator RechargeStamina()
        {
            yield return new WaitForSeconds(1f);

            while (CurrenStamina < MaxinunStamina)
            {
                CurrenStamina += recharge_rate / 10f;
                if (CurrenStamina > MaxinunStamina) CurrenStamina = MaxinunStamina;
                StaminaBar.fillAmount = CurrenStamina / MaxinunStamina;
                yield return new WaitForSeconds(.1f);
            }

        }
    }

}