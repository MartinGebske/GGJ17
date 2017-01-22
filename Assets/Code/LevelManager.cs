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
    public RectTransform BtnQuit;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        LeanTween.moveY(Logo, -316f, 0.7f)
            .setEase(LeanTweenType.easeOutElastic);

        yield return new WaitForSeconds(0.3f);

        LeanTween.move(HotDogsImg, new Vector3(-670f, 377f), 1f)
            .setEase(LeanTweenType.easeOutCubic);

        yield return new WaitForSeconds(0.2f);

        LeanTween.move(BtnPlay, new Vector3(217f, 452f), 0.7f)
             .setEase(LeanTweenType.easeOutElastic);

        yield return new WaitForSeconds(0.2f);

        LeanTween.move(BtnInstr, new Vector3(206f, 153f), 0.7f)
             .setEase(LeanTweenType.easeOutElastic);

        yield return new WaitForSeconds(0.2f);

        LeanTween.move(BtnQuit, new Vector3(488f, 149f), 0.7f)
             .setEase(LeanTweenType.easeOutElastic);
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
