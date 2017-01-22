using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Config")]
    public RectTransform Logo;
    public RectTransform HotDogsImg;
    public RectTransform BtnPlay;
    public RectTransform BtnInstr;
    public RectTransform BtnLeaderboard;
    public RectTransform BtnQuit;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        if (Logo != null)
        {
            LeanTween.moveY(Logo, -316f, 0.7f)
                .setEase(LeanTweenType.easeOutElastic);

            AudioManager.Instance.PlayHotDog();
        }

        if (HotDogsImg != null)
        {
            LeanTween.move(HotDogsImg, new Vector3(-670f, 377f), 1f)
                .setEase(LeanTweenType.easeOutCubic);
        }

        yield return new WaitForSeconds(0.2f);

        if (BtnPlay != null)
        {
            LeanTween.move(BtnPlay, new Vector3(217f, 452f), 0.5f)
                 .setEase(LeanTweenType.easeOutSine);

            AudioManager.Instance.PlayHotDog();
        }

        yield return new WaitForSeconds(0.2f);

        if (BtnInstr != null)
        {
            LeanTween.move(BtnInstr, new Vector3(206f, 153f), 0.5f)
                 .setEase(LeanTweenType.easeOutSine);

            AudioManager.Instance.PlayHotDog();
        }

        yield return new WaitForSeconds(0.2f);

        if (BtnLeaderboard != null)
        {
            LeanTween.move(BtnLeaderboard, new Vector3(488f, 149f), 0.5f)
                 .setEase(LeanTweenType.easeOutSine);

            AudioManager.Instance.PlayHotDog();
        }

        yield return new WaitForSeconds(0.2f);

        if (BtnQuit != null)
        {
            LeanTween.move(BtnQuit, new Vector3(734f, 139f), 0.5f)
                 .setEase(LeanTweenType.easeOutSine);

            AudioManager.Instance.PlayHotDog();
        }
    }


    public void ClickOnButton(string name)
    {
        SceneManager.LoadScene(name);   
    }

    public void ClickQuitButton()
    {
        Application.Quit();
    }
}
