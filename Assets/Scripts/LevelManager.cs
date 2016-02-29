using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()

    {

        Application.Quit();

    }


    }
        




