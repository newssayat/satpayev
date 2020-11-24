using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public Image[] images;
    public Text brilliance;
    public GameObject Settings;
    public SettingsAnimController AnimController;
    
    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        brilliance.text = PlayerPrefs.GetInt(GameManager.PREFS_BRILLIANCE,0).ToString();
        score.text = PlayerPrefs.GetInt("score").ToString();

        AllBackGroundsInvisible();
        if (PlayerPrefs.GetInt("level", 1) < 100)
        {
            images[0].GetComponent<Transform>().localScale = new Vector3(1,1,1);
        } else if (PlayerPrefs.GetInt("level", 1) < 200)
        {
            images[1].GetComponent<Transform>().localScale = new Vector3(1,1,1);
        }
    }

    private void AllBackGroundsInvisible()
    {
        foreach (Image image in images)
        {
            image.GetComponent<Transform>().localScale = new Vector3(0,0,0);
        }
    }

    public void OnClickSettings()
    {
        if (Settings.transform.localScale.x < 1)
        {
            Settings.transform.localScale += new Vector3(1,1,0);
            AnimController.ClickSettingsBtn();
        }
    }

    public void OnClickClose()
    {
        if (Settings.transform.localScale.x > 0)
        {
            AnimController.ClickSettingsClose();
            StartCoroutine(layerRemove());
        }
    }

    private IEnumerator layerRemove()
    {
        yield return new WaitForSeconds(0.4f);
        Settings.transform.localScale -= new Vector3(1,1,0);
    }
}
