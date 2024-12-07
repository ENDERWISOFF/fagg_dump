using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveMent : MonoBehaviour
{
    private float range;
    [SerializeField] GameObject target;
    private bool target_collision = false;
    public float minDistance = 5.0f;
    private float speed = 2.0f;


    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 0f;

        target = GameObject.FindWithTag("Player");
        
    }

    void Update()
    {
        target = GameObject.FindWithTag("Player");
        range = Vector2.Distance(transform.position, target.transform.position);
        
        if(range < minDistance)
        {
            if (!target_collision)
            {
                transform.LookAt(target.transform.position);

                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));


            }
        }
        transform.rotation = Quaternion.identity;
    }

   
}
