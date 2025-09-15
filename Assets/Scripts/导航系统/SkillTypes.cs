using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 单体目标技能
public class TargetSkill : Skill
{
    public Transform target; // 目标对象
    
    public TargetSkill(string name, float cd, float mana, float range, float dmg) 
        : base(name, cd, mana, range, dmg)
    {
    }
    
    protected override void Cast()
    {
        if (target != null)
        {
            // 检查目标是否在范围内
            float distance = Vector3.Distance(SkillManager.Instance.transform.position, target.position);
            if (distance <= castRange)
            {
                Debug.Log("使用技能: " + skillName + " 对目标: " + target.name);
                
                // 尝试获取目标的技能管理器组件并造成伤害
                SkillManager targetSkillManager = target.GetComponent<SkillManager>();
                if (targetSkillManager != null)
                {
                    targetSkillManager.TakeDamage(damage);
                }
                
                // 生成技能效果
                if (effectPrefab != null)
                {
                    GameObject effect = GameObject.Instantiate(effectPrefab, target.position, Quaternion.identity);
                    GameObject.Destroy(effect, 2f); // 2秒后销毁特效
                }
            }
            else
            {
                Debug.Log("目标超出施法范围!");
            }
        }
        else
        {
            // 如果没有明确设置目标，尝试使用SkillManager中的当前目标
            if (SkillManager.Instance.currentTarget != null)
            {
                target = SkillManager.Instance.currentTarget;
                Cast(); // 递归调用，此时已有目标
            }
            else
            {
                Debug.Log("没有选择目标!");
            }
        }
    }
}

// 范围伤害技能
public class AOESkill : Skill
{
    public float radius; // 影响半径
    
    public AOESkill(string name, float cd, float mana, float range, float dmg, float aoeRadius) 
        : base(name, cd, mana, range, dmg)
    {
        radius = aoeRadius;
    }
    
    protected override void Cast()
    {
        // 获取施法者位置
        Vector3 casterPosition = SkillManager.Instance.transform.position;
        
        // 在施法者前方生成技能效果
        Vector3 skillPosition = casterPosition + SkillManager.Instance.transform.forward * castRange;
        
        Debug.Log("使用范围技能: " + skillName + " 在位置: " + skillPosition);
        
        // 生成技能效果
        if (effectPrefab != null)
        {
            GameObject effect = GameObject.Instantiate(effectPrefab, skillPosition, Quaternion.identity);
            GameObject.Destroy(effect, 2f); // 2秒后销毁特效
        }
        
        // 查找范围内的所有目标
        Collider[] hitColliders = Physics.OverlapSphere(skillPosition, radius);
        foreach (var hitCollider in hitColliders)
        {
            // 排除自己
            if (hitCollider.transform != SkillManager.Instance.transform)
            {
                SkillManager targetSkillManager = hitCollider.GetComponent<SkillManager>();
                if (targetSkillManager != null)
                {
                    targetSkillManager.TakeDamage(damage);
                    Debug.Log("对 " + hitCollider.name + " 造成伤害: " + damage);
                }
            }
        }
    }
}

// 自我增益技能
public class BuffSkill : Skill
{
    public float duration; // 持续时间
    public float speedBoost; // 速度提升
    
    private NavMeshAgent agent; // 导航代理
    private float originalSpeed; // 原始速度
    private bool isActive = false; // 是否激活
    private float activeTime = 0; // 激活时间
    
    public BuffSkill(string name, float cd, float mana, float range, float dmg, float buffDuration, float boost) 
        : base(name, cd, mana, range, dmg)
    {
        duration = buffDuration;
        speedBoost = boost;
    }
    
    protected override void Cast()
    {
        // 获取导航代理组件
        if (agent == null)
        {
            agent = SkillManager.Instance.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                originalSpeed = agent.speed;
            }
        }
        
        if (agent != null)
        {
            Debug.Log("使用增益技能: " + skillName);
            
            // 应用速度提升
            agent.speed = originalSpeed + speedBoost;
            
            // 生成技能效果
            if (effectPrefab != null)
            {
                GameObject effect = GameObject.Instantiate(effectPrefab, SkillManager.Instance.transform.position, Quaternion.identity);
                effect.transform.SetParent(SkillManager.Instance.transform);
                GameObject.Destroy(effect, duration); // 持续时间后销毁特效
            }
            
            // 设置激活状态
            isActive = true;
            activeTime = 0;
            
            // 启动协程来恢复原始速度
            SkillManager.Instance.StartCoroutine(EndBuff());
        }
    }
    
    private IEnumerator EndBuff()
    {
        yield return new WaitForSeconds(duration);
        
        if (agent != null)
        {
            agent.speed = originalSpeed;
            Debug.Log(skillName + " 效果结束");
        }
        
        isActive = false;
    }
    
    // 更新方法，用于在Update中调用
    public void UpdateBuff(float deltaTime)
    {
        if (isActive)
        {
            activeTime += deltaTime;
            if (activeTime >= duration)
            {
                isActive = false;
                if (agent != null)
                {
                    agent.speed = originalSpeed;
                    Debug.Log(skillName + " 效果结束");
                }
            }
        }
    }
}

// 位移技能
public class DashSkill : Skill
{
    public float dashDistance; // 冲刺距离
    public float dashSpeed; // 冲刺速度
    
    private NavMeshAgent agent; // 导航代理
    private bool isDashing = false; // 是否正在冲刺
    
    public DashSkill(string name, float cd, float mana, float range, float dmg, float distance, float speed) 
        : base(name, cd, mana, range, dmg)
    {
        dashDistance = distance;
        dashSpeed = speed;
    }
    
    protected override void Cast()
    {
        // 获取导航代理组件
        if (agent == null)
        {
            agent = SkillManager.Instance.GetComponent<NavMeshAgent>();
        }
        
        if (agent != null && !isDashing)
        {
            Debug.Log("使用位移技能: " + skillName);
            
            // 计算目标位置
            Vector3 dashDirection = SkillManager.Instance.transform.forward;
            Vector3 targetPosition = SkillManager.Instance.transform.position + dashDirection * dashDistance;
            
            // 检查目标位置是否可达
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, dashDistance, NavMesh.AllAreas))
            {
                // 保存原始速度
                float originalSpeed = agent.speed;
                
                // 设置冲刺速度
                agent.speed = dashSpeed;
                
                // 设置目标位置
                agent.SetDestination(hit.position);
                
                // 生成技能效果
                if (effectPrefab != null)
                {
                    GameObject effect = GameObject.Instantiate(effectPrefab, SkillManager.Instance.transform.position, Quaternion.identity);
                    effect.transform.SetParent(SkillManager.Instance.transform);
                    GameObject.Destroy(effect, 1f); // 1秒后销毁特效
                }
                
                // 设置冲刺状态
                isDashing = true;
                
                // 启动协程来恢复原始速度
                SkillManager.Instance.StartCoroutine(EndDash(originalSpeed));
            }
            else
            {
                Debug.Log("无法到达目标位置!");
            }
        }
    }
    
    private IEnumerator EndDash(float originalSpeed)
    {
        // 等待到达目标位置或超时
        float timeout = dashDistance / dashSpeed + 0.5f; // 添加一些额外时间
        float elapsed = 0;
        
        while (elapsed < timeout && agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // 恢复原始速度
        agent.speed = originalSpeed;
        isDashing = false;
        Debug.Log(skillName + " 冲刺结束");
    }
}