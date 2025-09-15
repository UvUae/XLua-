using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testwork : MonoBehaviour
{

    public enum emAntion
    {
        None = 0,
        Sleep,
        Getup,
        Wash,
        Eat,
        Play,
    }

    enum emState
    {
        None = 0,
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
    }

    public emAntion emAntionState = emAntion.Play;
    // Start is called before the first frame update
    void Start()
    {

        //Func1
        Test();
    }

    void Test()
    {

        string str = "layayadamm";

        string[] array = str.Split(new char[] { 'a' });
        for (int i = 0; i < array.Length; i++)
            Debug.Log("i:" + i + "  content : " + array[i]);
    }
    void Func1()
    {
        Debug.Log(emAntionState);
        Debug.Log(emAntionState.ToString());

        //emAntionState = (emAntion)Enum.Parse(typeof(emAntion), "Eat");
        int iPlay = (int)emAntionState;

        int iConvertValue = Convert.ToInt32(emAntionState);
        emAntionState = (emAntion)3;
        Debug.Log("test:" + emAntionState); 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
