using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    public void loadSceneWithName(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
