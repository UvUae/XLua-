using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L6_8_JS : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody component for physics interactions
    public float speed = 2f; // Speed of the js

    private int hp = 100; // Health points of the js

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 offset = transform.forward * Time.fixedDeltaTime * speed;
        rb.MovePosition(transform.position + offset);
    }

    public void Hurt(int damage)
    {
        hp-= damage; // Reduce health by damage amount
        if(hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
       L6_8_Player.intance.JsDead(this); // Notify the player that this js is dead

        Destroy(gameObject); // Destroy the js object
    }
}
