using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreSubmit : MonoBehaviour {

    [Header("Config")]
    public InputField InputName;
    public Text TextMoney;

    private int m_Score;

    public void Init(int score)
    {
        m_Score = score;
        TextMoney.text = ((float)score / 100f).ToString("c2");
    }

    public void OnSubmitClicked()
    {
        StartCoroutine(PostScores(InputName.text, m_Score));
    }

    private string secretKey = "12312312";
    string addScoreUrl = "http://138.68.69.107/ggj17/addscore.php?";
    string highscoreUrl = "http://138.68.69.107/ggj17/display.php";
    IEnumerator PostScores(string name, int score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = StaticUtil.Md5Sum(name + score + secretKey);

        string post_url = addScoreUrl + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        Debug.LogWarning(post_url);
        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
        else
        {
            PlayerPrefs.SetString("last_name", name);
            PlayerPrefs.SetInt("last_score", score);
            PlayerPrefs.Save();
        }

        gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("highscore", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
