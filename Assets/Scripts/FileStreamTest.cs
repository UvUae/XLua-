using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileStreamTest : MonoBehaviour
{
    Hashtable ht1 = new Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        ht1.Add("key1", "value1");
        ht1.Add("key2", "value2");

        ht1.Add("key3", "value3");

        ICollection collection = ht1.Keys;
        foreach (var key in collection)
        {
            Debug.Log("Key: " + key + ", Value: " + ht1[key]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
