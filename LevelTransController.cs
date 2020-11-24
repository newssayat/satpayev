using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransController : MonoBehaviour
{
    public GameObject slider;
    public Text textSlider;
    
    void Start()
    {
        int currentLevel = ((PlayerPrefs.GetInt(GameManager.PREFS_LEVEL) % 100) == 0) ? 100 : PlayerPrefs.GetInt(GameManager.PREFS_LEVEL) % 100;
        slider.GetComponent<Slider>().value = (currentLevel - 1) / (float) 100;
        textSlider.text = (currentLevel - 1) + " / 100";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
