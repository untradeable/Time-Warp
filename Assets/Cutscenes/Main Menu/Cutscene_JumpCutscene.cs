using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene_JumpCutscene : MonoBehaviour
{
    public string sceneName;

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}