using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 技能基类
public abstract class Skill
{
    public string skillName;        // 技能名称
    public float cooldown;          // 冷却时间
    public float manaCost;          // 魔法消耗
    public float castRange;         // 施放范围
    public float damage;            // 伤害值
    public float currentCooldown;   // 当前冷却时间
    public Sprite skillIcon;        // 技能图标
    
    // 技能效果预制体
    public GameObject effectPrefab;
    
    // 构造函数
    public Skill(string name, float cd, float mana, float range, float dmg)
    {
        skillName = name;
        cooldown = cd;
        manaCost = mana;
        castRange = range;
        damage = dmg;
        currentCooldown = 0;
    }
    
    // 更新冷却时间
    public void UpdateCooldown(float deltaTime)
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= deltaTime;
            if (currentCooldown < 0)
                currentCooldown = 0;
        }
    }
    
    // 检查技能是否可用
    public bool IsReady()
    {
        return currentCooldown <= 0;
    }
    
    // 使用技能
    public void Use()
    {
        if (IsReady())
        {
            currentCooldown = cooldown;
            Cast();
        }
    }
    
    // 具体技能效果实现（由子类重写）
    protected abstract void Cast();
}

// 技能管理器
public class SkillManager : MonoBehaviour
{
    // 单例实例
    public static SkillManager Instance { get; private set; }
    
    // 角色属性
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float maxMana = 100f;
    public float currentMana = 100f;
    public float manaRegenRate = 5f; // 每秒回复魔法值
    
    // 技能列表
    public List<Skill> skills = new List<Skill>();
    
    // 当前选中的目标
    public Transform currentTarget;
    
    // 初始化单例
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    // 初始化
    void Start()
    {
        // 可以在这里添加初始技能
    }
    
    // 更新
    void Update()
    {
        // 更新所有技能冷却
        foreach (var skill in skills)
        {
            skill.UpdateCooldown(Time.deltaTime);
            
            // 如果是增益技能，更新其状态
            if (skill is BuffSkill buffSkill)
            {
                buffSkill.UpdateBuff(Time.deltaTime);
            }
        }
        
        // 魔法值回复
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            if (currentMana > maxMana)
                currentMana = maxMana;
        }
    }
    
    // 添加技能
    public void AddSkill(Skill skill)
    {
        skills.Add(skill);
    }
    
    // 使用技能
    public void UseSkill(int index)
    {
        if (index >= 0 && index < skills.Count)
        {
            Skill skill = skills[index];
            if (skill.IsReady() && currentMana >= skill.manaCost)
            {
                currentMana -= skill.manaCost;
                skill.Use();
            }
            else if (!skill.IsReady())
            {
                Debug.Log(skill.skillName + " 技能冷却中: " + skill.currentCooldown.ToString("F1") + "秒");
            }
            else
            {
                Debug.Log("魔法值不足!");
            }
        }
    }
    
    // 受到伤害
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // 角色死亡逻辑
            currentHealth = 0;
            Debug.Log(gameObject.name + " has died!");
        }
    }
    
    // 设置当前目标
    public void SetTarget(Transform target)
    {
        currentTarget = target;
        Debug.Log("目标已设置为: " + target.name);
    }
}