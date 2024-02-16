using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene_Transition : MonoBehaviour
{
    public void LoadScene()
     {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     }
}
