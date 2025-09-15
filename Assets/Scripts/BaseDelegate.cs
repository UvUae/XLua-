using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDelegate : MonoBehaviour
{
    public delegate void DelegateMethod();
    public delegate int DelegateMethod2(int a,int b);
    public delegate void DelegateMethod3<T>(T a);

    // Start is called before the first frame update
    void Start()
    {
        DelegateMethod show = ShowMessage;
        show();
        show.Invoke(); // Another way to invoke the delegate
        DelegateMethod2 show2 = Show;
        int a=Show(3, 7);
        Debug.Log(a);
        DelegateMethod3<string> show3 = ShowStr;
        ShowStr("5ÔÂ");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowMessage()
    {
        Debug.Log("Hello from BaseDelegate!");
    }

    int Show(int a, int b) {

        return a + b;
    }

    public void ShowStr(string a)
    {
        Debug.Log("ShowStr:"+ a);
    }
}
