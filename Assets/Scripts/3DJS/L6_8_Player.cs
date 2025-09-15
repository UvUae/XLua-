using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L6_8_Player : MonoBehaviour
{
    private int attackValue = 100; // Example damage value

    public static L6_8_Player intance;
    public List<L6_8_JS> jsGroup;
    private int score=0;
    private bool isGameOver = false;

    private void Awake()
    {
        if (intance == null)
        {
            intance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

     private void shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitInfo, 100f))
        {
            Debug.DrawLine(ray.origin,hitInfo.point,Color.red);
            // Add logic to handle the hit object, e.g., apply damage or effects
            if (hitInfo.collider.gameObject.tag == "JS") 
            {
                L6_8_JS js = hitInfo.collider.gameObject.GetComponent<L6_8_JS>();
                js.Hurt(attackValue);
            }
        }

    }

      public void JsDead(L6_8_JS js)
    {
        score += 1;
        L6_8_UIManager.Instance.UpdateScore(score); // Update the score in the UI
        jsGroup.Remove(js); // Remove the js from the group
        if(jsGroup.Count ==0)
        {
            Win();
        }
    }

    private void Win()
    {
        //print("You win! All js are dead.");
        L6_8_UIManager.Instance.ShowGameOver(true); // Show win UI
    }

    private void Over()
    {
        isGameOver = true; // Prevent further game over calls
        L6_8_UIManager.Instance.ShowGameOver(false); 
        print("Äã±»½©Ê¬Ò§ÁË");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "JS" && isGameOver==false)
        {
            Over(); // Call game over logic
        }
    }
}
