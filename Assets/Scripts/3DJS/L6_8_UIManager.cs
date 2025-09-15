using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L6_8_UIManager : MonoBehaviour
{
    public static L6_8_UIManager Instance;

    public Text ScoreText;
    public GameObject GameResult;
    public Text ResultText; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore(int score)
    {
        ScoreText.text = "Score: " + score.ToString();
    }

    public void ShowGameOver(bool isWin)
    {
        GameResult.SetActive(true);
        ResultText.text = isWin ? "You Win!" : "Game Over!";
    }
}
