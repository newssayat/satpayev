using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    
    private void Awake()
    {
        if (GameManager.isStart)
        {
            int currentLevel = PlayerPrefs.GetInt(GameManager.PREFS_LEVEL, 1);
            if (currentLevel == 1)
            {
                GameObject.FindWithTag("text").GetComponent<Text>().text = "Play";
            }
            else
            {
                GameObject.FindWithTag("text").GetComponent<Text>().text = "Level " + currentLevel;
            }
        }
        else
        {
            GameObject.FindWithTag("text").GetComponent<Text>().text = "Next";
        }

    }

    private void OnMouseDown()
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void onClickPlayBtn()
    {
        GameManager.instance.StartGame();
    }
}
