using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class L9_1Demo : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    public Transform target;
    
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //agent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.nearClipPlane)));

        //agent.SetDestination(target.position);

        HandleMovement();
        UpdateAnimation();
        HandleSpaceKey();
    }
    
    // 处理右键移动
    protected virtual void HandleMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hitInfo, 1000))
            {
                agent.SetDestination(hitInfo.point);
                animator.SetBool("Run", true);
            }
        }
    }
    
    // 更新动画状态
    protected virtual void UpdateAnimation()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }
    
    // 处理空格键停止/继续
    protected virtual void HandleSpaceKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.isStopped = !agent.isStopped;
        }
    }
}
