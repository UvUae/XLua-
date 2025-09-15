using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyClassType
{
    public string a;
    public MyClassType(string value)
    {
        this.a = value;
    }
}


public class TestClass<T>where T: MyClassType
{

    public T[] Value { get; set; }
    public TestClass(int size)
    {
       Value = new T[size];
    }
    public void SetValue(int index, T value)
    {
        if (index >= 0 && index < Value.Length)
        {
            Value[index] = value;
        }
        else
        {
            Debug.LogError("Index out of bounds");
        }
    }

    public string GetValue(int index)
    {
        if (index >= 0 && index < Value.Length)
        {
            return Value[index].a;
        }
        else
        {
            Debug.LogError("Index out of bounds");
            return default;
        }
    }

    public void Show<X>(X A)
    {
        Debug.Log("A=" + A);
    }

}

public class fanT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestClass<MyClassType> testClass = new TestClass<MyClassType>(5);
        testClass.SetValue(0, new MyClassType("处女作"));
        testClass.SetValue(1, new MyClassType("二作"));
        testClass.SetValue(2, new MyClassType("三作"));

        testClass.Show<string>("泛型方法测试");

        //MyClassType a = testClass.GetValue(0);
        //MyClassType b = testClass.GetValue(1);
        //MyClassType c = testClass.GetValue(2);
        //testClass.SetValue(0, "处女作");
        //testClass.SetValue(1, "第二个作品");
        //testClass.SetValue(2, "第三个作品");
        string a = testClass.GetValue(0);
        string b = testClass.GetValue(1);
        string c = testClass.GetValue(2);

        Debug.Log("获取的值: " + a + ", " + b + ", " + c);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
