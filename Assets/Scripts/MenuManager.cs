using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{
    public void PlaneScene() 
    {
        SceneManager.LoadScene("PlaneDetectTest");
    }

    public void Scene3D() 
    {
        SceneManager.LoadScene("3DSceneTest");
    }

    public void Scene2D() 
    {
        SceneManager.LoadScene("2DSceneTest");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
