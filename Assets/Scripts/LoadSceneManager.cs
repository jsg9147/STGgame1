using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public void PlaySceneLoad()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void TitleSceneLoad()
    {
        SceneManager.LoadScene(0);
    }

    public void RankSceneLoad()
    {
        SceneManager.LoadScene(2);
    }
}
