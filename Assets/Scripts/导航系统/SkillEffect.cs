using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public float lifeTime = 2f; // 特效生命周期
    public bool followTarget = false; // 是否跟随目标
    public Transform target; // 跟随的目标
    public bool scaleOverTime = false; // 是否随时间缩放
    public float startScale = 0.1f; // 起始缩放
    public float endScale = 1.0f; // 结束缩放
    public bool rotateEffect = false; // 是否旋转特效
    public Vector3 rotationSpeed = new Vector3(0, 90, 0); // 旋转速度
    
    private float startTime;
    
    void Start()
    {
        startTime = Time.time;
        
        // 如果需要缩放，设置初始大小
        if (scaleOverTime)
        {
            transform.localScale = Vector3.one * startScale;
        }
        
        // 设置特效自动销毁
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
        // 如果需要跟随目标
        if (followTarget && target != null)
        {
            transform.position = target.position;
        }
        
        // 如果需要缩放
        if (scaleOverTime)
        {
            float t = (Time.time - startTime) / lifeTime;
            float currentScale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = Vector3.one * currentScale;
        }
        
        // 如果需要旋转
        if (rotateEffect)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}