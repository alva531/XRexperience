using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    Animator fade;
    public void PlaneScene() 
    {
        fade.SetTrigger("end");
        StartCoroutine(Plane2());
    }

    public void Scene3D() 
    {
        fade.SetTrigger("end");
        StartCoroutine(Scene3D2());
    }

    public void Scene2D() 
    {
        fade.SetTrigger("end");
        StartCoroutine(Scene2D2());
        
    }

    public void Menu() 
    {
        fade.SetTrigger("end");
        StartCoroutine(Menu2());
    }

    public void Exit()
    {
        fade.SetTrigger("end");
        StartCoroutine(Exit2());
    }

    IEnumerator Plane2()
    {
        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene("PlaneDetectTest");
    }

    IEnumerator Scene3D2()
    {
        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene("3DSceneTest");
    }

    IEnumerator Scene2D2()
    {
        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene("2DSceneTest");
    }

    IEnumerator Menu2()
    {
        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator Exit2()
    {
        yield return new WaitForSeconds(0.25f);

        Application.Quit();
    }

}
