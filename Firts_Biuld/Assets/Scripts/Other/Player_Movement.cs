using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player_Movement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    Rigidbody2D rb;
    private Animator animator;

    public float speed = 1f;
    public float acceleration = 1f;

    [SerializeField] public Image StaminaBar;
    
    public float CurrenStamina;
    public float MaxinunStamina;

    public float RunCost;
    public float recharge_rate;
    private Coroutine recharge;


    void Start()
    {
        StaminaBar = GameObject.FindWithTag("Stamina").GetComponent<Image>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal * speed, vertical * speed );

        
        
        
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
            
            if(recharge != null ) StopCoroutine(recharge);
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

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while(CurrenStamina < MaxinunStamina)
        {
            CurrenStamina+= recharge_rate /10f;
            if (CurrenStamina > MaxinunStamina) CurrenStamina = MaxinunStamina;
            StaminaBar.fillAmount = CurrenStamina /MaxinunStamina;
            yield return new WaitForSeconds(.1f);
        }
        
    }
}

