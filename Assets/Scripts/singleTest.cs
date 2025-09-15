using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal
{
    public string Name;
    public int age;
    public Animal(string name, int age)
    {
        this.Name = name;
        this.age = age;
    }
    public virtual void Run()
    {
       
    }
}

public class Cat : Animal
{
    
    public Action actions;//发布者

    public Cat(string name, int age) : base(name, age)
    {
    }

    public void Coming()
    {
        Debug.Log(Name+"is coming!");
        if (actions != null)
        {
            actions.Invoke();
        }
        this.Run();
    }
    
    public override void Run()
    {
        Debug.Log( Name + " 开始追老鼠 " );
    }
}

public class Mouse : Animal
{
    public Mouse(string name, int age,Cat cat) : base(name, age)
    {
        //订阅者
        cat.actions += this.Run;
    }
    public override void Run()
    {
        Debug.Log(Name + " 开始逃跑 " );
    }
}

public class singleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cat cat = new Cat("小花猫", 2);

        Animal mouse = new Mouse("小老鼠a", 1, cat);
        Animal mouse2 = new Mouse("小老鼠b", 1, cat);
        Animal mouse3 = new Mouse("小老鼠c", 1, cat);

        cat.Coming();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
