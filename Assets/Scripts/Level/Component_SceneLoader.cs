using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Component_SceneLoader : MonoBehaviour
{
    public string sceneName;

    private void Start()
    {
        SceneManager.LoadScene(sceneName);
    }
}