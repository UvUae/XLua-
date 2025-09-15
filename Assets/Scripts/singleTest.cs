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
    
    public Action actions;//������

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
        Debug.Log( Name + " ��ʼ׷���� " );
    }
}

public class Mouse : Animal
{
    public Mouse(string name, int age,Cat cat) : base(name, age)
    {
        //������
        cat.actions += this.Run;
    }
    public override void Run()
    {
        Debug.Log(Name + " ��ʼ���� " );
    }
}

public class singleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cat cat = new Cat("С��è", 2);

        Animal mouse = new Mouse("С����a", 1, cat);
        Animal mouse2 = new Mouse("С����b", 1, cat);
        Animal mouse3 = new Mouse("С����c", 1, cat);

        cat.Coming();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
