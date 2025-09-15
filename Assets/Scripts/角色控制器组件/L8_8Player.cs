using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L8_8Player : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;  

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Demo2();
    }

    // ���ڽ�ɫ��������ϵ�ƶ�����������ת
    private void Demo1()
    {
       
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        dir = transform.TransformDirection(dir);

        characterController.SimpleMove(dir * 5);
    }

    // ������������ϵ�ƶ���������ת
    private void Demo2()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
        Vector3 dir = new Vector3(0, 0, vertical).normalized;
        dir = transform.TransformDirection(dir);
        characterController.SimpleMove(dir * 5);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y+ horizontal, 0);
    }

}
