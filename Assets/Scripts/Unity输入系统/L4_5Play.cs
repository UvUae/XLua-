using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L4_5Play : MonoBehaviour
{   
    public float speed = 6; // Speed of the player
    public  float jumpForce = 10; // Jump force of the player
    private new Rigidbody2D rigidbody2D;
    private bool isGrounded = false; // Check if the player is on the ground
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnAnimatorMove();
        Jump();
    }

    private void OnAnimatorMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, 0).normalized;    
        transform.Translate(direction * Time.deltaTime * speed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 可以在这里添加跳跃着陆的逻辑
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 可以在这里添加离开地面的逻辑
            isGrounded = false;
        }
    }
}
