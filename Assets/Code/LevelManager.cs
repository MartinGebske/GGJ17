using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void ClickOnButton(string name)
    {
        SceneManager.LoadScene(name);   
    }

    public void ClickQuitButton()
    {
        Application.Quit();
    }
}
