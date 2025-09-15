using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    // 单例实例
    public static SkillUIManager Instance { get; private set; }
    
    // 技能图标
    public Image[] skillIcons;
    
    // 冷却遮罩
    public Image[] cooldownOverlays;
    
    // 冷却文本
    public Text[] cooldownTexts;
    
    // 血条
    public Image healthBar;
    public Text healthText;
    
    // 蓝条
    public Image manaBar;
    public Text manaText;
    
    // 目标信息面板
    public GameObject targetInfoPanel;
    public Text targetNameText;
    public Image targetHealthBar;
    
    // 技能管理器引用
    private SkillManager skillManager;
    
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
    
    void Start()
    {
        // 获取技能管理器
        skillManager = FindObjectOfType<MOBACharacterController>().GetComponent<SkillManager>();
        
        // 初始化UI
        InitializeUI();
    }
    
    void Update()
    {
        // 更新技能UI
        UpdateSkillUI();
        
        // 更新角色状态UI
        UpdateStatusUI();
        
        // 更新目标信息
        UpdateTargetInfo();
    }
    
    // 初始化UI
    private void InitializeUI()
    {
        // 确保所有UI组件都已设置
        if (skillIcons == null || cooldownOverlays == null || cooldownTexts == null)
        {
            Debug.LogWarning("技能UI组件未设置!");
        }
        
        // 初始隐藏目标信息面板
        if (targetInfoPanel != null)
        {
            targetInfoPanel.SetActive(false);
        }
    }
    
    // 更新技能UI
    private void UpdateSkillUI()
    {
        if (skillManager == null || skillManager.skills == null) return;
        
        for (int i = 0; i < skillManager.skills.Count && i < skillIcons.Length; i++)
        {
            Skill skill = skillManager.skills[i];
            
            // 更新技能图标
            if (skill.skillIcon != null && skillIcons[i] != null)
            {
                skillIcons[i].sprite = skill.skillIcon;
            }
            
            // 更新冷却遮罩
            if (cooldownOverlays[i] != null)
            {
                if (skill.currentCooldown > 0)
                {
                    cooldownOverlays[i].fillAmount = skill.currentCooldown / skill.cooldown;
                    cooldownOverlays[i].gameObject.SetActive(true);
                }
                else
                {
                    cooldownOverlays[i].gameObject.SetActive(false);
                }
            }
            
            // 更新冷却文本
            if (cooldownTexts[i] != null)
            {
                if (skill.currentCooldown > 0)
                {
                    cooldownTexts[i].text = skill.currentCooldown.ToString("F1");
                    cooldownTexts[i].gameObject.SetActive(true);
                }
                else
                {
                    cooldownTexts[i].gameObject.SetActive(false);
                }
            }
        }
    }
    
    // 更新角色状态UI
    private void UpdateStatusUI()
    {
        if (skillManager == null) return;
        
        // 更新血条
        if (healthBar != null)
        {
            healthBar.fillAmount = skillManager.currentHealth / skillManager.maxHealth;
        }
        
        // 更新血量文本
        if (healthText != null)
        {
            healthText.text = Mathf.Round(skillManager.currentHealth) + " / " + skillManager.maxHealth;
        }
        
        // 更新蓝条
        if (manaBar != null)
        {
            manaBar.fillAmount = skillManager.currentMana / skillManager.maxMana;
        }
        
        // 更新蓝量文本
        if (manaText != null)
        {
            manaText.text = Mathf.Round(skillManager.currentMana) + " / " + skillManager.maxMana;
        }
    }
    
    // 更新目标信息
    private void UpdateTargetInfo()
    {
        if (skillManager == null) return;
        
        if (skillManager.currentTarget != null)
        {
            // 显示目标信息面板
            if (targetInfoPanel != null)
            {
                targetInfoPanel.SetActive(true);
            }
            
            // 更新目标名称
            if (targetNameText != null)
            {
                targetNameText.text = skillManager.currentTarget.name;
            }
            
            // 更新目标血条
            if (targetHealthBar != null)
            {
                SkillManager targetSkillManager = skillManager.currentTarget.GetComponent<SkillManager>();
                if (targetSkillManager != null)
                {
                    targetHealthBar.fillAmount = targetSkillManager.currentHealth / targetSkillManager.maxHealth;
                }
            }
        }
        else
        {
            // 隐藏目标信息面板
            if (targetInfoPanel != null)
            {
                targetInfoPanel.SetActive(false);
            }
        }
    }
}