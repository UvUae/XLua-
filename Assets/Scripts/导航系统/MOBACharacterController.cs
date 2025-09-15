using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MOBACharacterController : L9_1Demo
{
    // 技能管理器
    private SkillManager skillManager;
    
    // 技能预制体
    public GameObject skillEffect1;
    public GameObject skillEffect2;
    public GameObject skillEffect3;
    public GameObject skillEffect4;
    
    // UI相关
    public UnityEngine.UI.Image[] skillIcons; // 技能图标
    public UnityEngine.UI.Image[] cooldownOverlays; // 冷却遮罩
    public UnityEngine.UI.Text[] cooldownTexts; // 冷却文本
    public UnityEngine.UI.Image healthBar; // 血条
    public UnityEngine.UI.Image manaBar; // 蓝条
    
    // 覆盖父类的Start方法
    new void Start()
    {
        // 调用父类的Start方法
        base.Start();
        
        // 获取技能管理器组件
        skillManager = GetComponent<SkillManager>();
        if (skillManager == null)
        {
            skillManager = gameObject.AddComponent<SkillManager>();
        }
        
        // 初始化技能
        InitializeSkills();
    }
    
    // 覆盖父类的Update方法
    new void Update()
    {
        // 调用父类的Update方法
        base.Update();
        
        // 处理技能输入
        HandleSkillInput();
        
        // 处理目标选择
        HandleTargetSelection();
        
        // 更新UI
        UpdateUI();
    }
    
    // 初始化技能
    private void InitializeSkills()
    {
        // 技能1: 单体伤害技能 (Q)
        TargetSkill skill1 = new TargetSkill("火球术", 5f, 20f, 10f, 25f);
        skill1.effectPrefab = skillEffect1;
        skillManager.AddSkill(skill1);
        
        // 技能2: 范围伤害技能 (W)
        AOESkill skill2 = new AOESkill("地震术", 8f, 30f, 8f, 20f, 5f);
        skill2.effectPrefab = skillEffect2;
        skillManager.AddSkill(skill2);
        
        // 技能3: 增益技能 (E)
        BuffSkill skill3 = new BuffSkill("疾跑", 15f, 15f, 0f, 0f, 5f, 3f);
        skill3.effectPrefab = skillEffect3;
        skillManager.AddSkill(skill3);
        
        // 技能4: 位移技能 (R)
        DashSkill skill4 = new DashSkill("闪现", 30f, 40f, 0f, 0f, 10f, 20f);
        skill4.effectPrefab = skillEffect4;
        skillManager.AddSkill(skill4);
        
        // 保存技能列表的引用
        skills = skillManager.skills;
    }
    
    // 技能列表的引用，方便访问
    private List<Skill> skills = new List<Skill>();
    
    // 处理技能输入
    private void HandleSkillInput()
    {
        // Q技能
        if (Input.GetKeyDown(KeyCode.Q) && skills.Count > 0)
        {
            // 如果是目标技能，需要设置目标
            if (skills[0] is TargetSkill targetSkill)
            {
                targetSkill.target = skillManager.currentTarget;
            }
            skillManager.UseSkill(0);
        }
        
        // W技能
        if (Input.GetKeyDown(KeyCode.W) && skills.Count > 1)
        {
            skillManager.UseSkill(1);
        }
        
        // E技能
        if (Input.GetKeyDown(KeyCode.E) && skills.Count > 2)
        {
            skillManager.UseSkill(2);
        }
        
        // R技能
        if (Input.GetKeyDown(KeyCode.R) && skills.Count > 3)
        {
            skillManager.UseSkill(3);
        }
    }
    
    // 处理目标选择
    private void HandleTargetSelection()
    {
        // 左键点击选择目标
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            if (Physics.Raycast(ray, out hitInfo, 1000))
            {
                // 检查是否点击了一个可选择的目标
                SkillManager targetSkillManager = hitInfo.collider.GetComponent<SkillManager>();
                if (targetSkillManager != null && targetSkillManager != skillManager)
                {
                    // 设置为当前目标
                    skillManager.SetTarget(hitInfo.collider.transform);
                    
                    // 可视化目标选择（例如高亮显示）
                    // 这里可以添加目标选择的视觉效果
                }
            }
        }
    }
    
    // 覆盖父类的移动处理方法
    protected override void HandleMovement()
    {
        // 调用父类的移动处理
        base.HandleMovement();
        
        // 右键点击时取消当前目标选择
        if (Input.GetMouseButtonDown(1))
        {
            skillManager.currentTarget = null;
            Debug.Log("目标已取消");
        }
    }
    
    // 更新UI
    private void UpdateUI()
    {
        // 如果UI组件已设置
        if (skillIcons != null && cooldownOverlays != null && cooldownTexts != null)
        {
            for (int i = 0; i < skills.Count && i < skillIcons.Length; i++)
            {
                Skill skill = skills[i];
                
                // 更新技能图标
                if (skill.skillIcon != null)
                {
                    skillIcons[i].sprite = skill.skillIcon;
                }
                
                // 更新冷却遮罩
                if (cooldownOverlays[i] != null)
                {
                    cooldownOverlays[i].fillAmount = skill.currentCooldown / skill.cooldown;
                }
                
                // 更新冷却文本
                if (cooldownTexts[i] != null && skill.currentCooldown > 0)
                {
                    cooldownTexts[i].text = skill.currentCooldown.ToString("F1");
                    cooldownTexts[i].gameObject.SetActive(true);
                }
                else if (cooldownTexts[i] != null)
                {
                    cooldownTexts[i].gameObject.SetActive(false);
                }
            }
        }
        
        // 更新血条和蓝条
        if (healthBar != null)
        {
            healthBar.fillAmount = skillManager.currentHealth / skillManager.maxHealth;
        }
        
        if (manaBar != null)
        {
            manaBar.fillAmount = skillManager.currentMana / skillManager.maxMana;
        }
    }
}