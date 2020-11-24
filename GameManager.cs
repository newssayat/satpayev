using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public static bool isStart = true;
    
    public static string PREFS_LEVEL = "level_prefs";
    public static string PREFS_BRILLIANCE = "PREFS_BRILLIANCE";
    
    public int score = 0;

    public int currentLevel = 1;

    private int highestLevel = 3;

    public static GameManager instance;
    
    private void Awake()
    {
        isStart = true;
        
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //LoadData();
        currentLevel = PlayerPrefs.GetInt(PREFS_LEVEL, currentLevel);
        score = PlayerPrefs.GetInt("score", score);
    }

    private void Start()
    {
                

    }





    private void IncreaseScore()
    {
        score += 10;
        PlayerPrefs.SetInt("score", score);
    }

    public void IncreaseLevel()
    {
        
        // check if there are more levels
        if (currentLevel < highestLevel)
        {
            currentLevel++;
            PlayerPrefs.SetInt(PREFS_LEVEL, currentLevel);
            PlayerPrefs.SetInt(PREFS_BRILLIANCE, PlayerPrefs.GetInt(PREFS_BRILLIANCE, 0) + 21);
        }
        else
        {
            currentLevel = 1;
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(PREFS_LEVEL, currentLevel);
        }
        IncreaseScore();

        SceneManager.LoadScene("LevelTrans");
    }

    
    
    public void ResetGame()
    {
        score = 0;

        // load level
        currentLevel = 1;
        PlayerPrefs.SetInt("level", currentLevel);

        // load level 1
        SceneManager.LoadScene("Level" + currentLevel);
    }

    public void StartGame()
    {
        currentLevel = PlayerPrefs.GetInt("PREFS_LEVEL", currentLevel);
        SceneManager.LoadScene("Level" + currentLevel);
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("MainPage");
    }
}
