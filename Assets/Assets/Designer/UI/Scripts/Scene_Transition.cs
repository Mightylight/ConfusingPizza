using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene_Transition : MonoBehaviour
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     }

    public void ResetGame()
    {
        SceneManager.LoadScene("Main Menu Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
