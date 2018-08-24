using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public void GoToScene(int sceneNum)
    {
        if (sceneNum == 69)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(sceneNum);
        }
    }
}
