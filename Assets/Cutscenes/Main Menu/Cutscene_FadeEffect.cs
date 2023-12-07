using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscene_FadeEffect : MonoBehaviour
{
    public Image img;

    public string sceneName;
    public bool isFinalFade;

    public bool fadeIn;
    public bool fadeOut;

    private void Update()
    {
        MakeFade();
    }

    public void MakeFade()
    {
        if(fadeIn)
        {
            StartCoroutine(FadeInImage());
        }

        if(fadeOut)
        {
            StartCoroutine(FadeOutImage());
        }
    }

    private IEnumerator FadeInImage()
    {
        for(float i = 0; i <= 1; i += Time.deltaTime)
        {
            img.color = new Color(0.2f, 0.2f, 0.2f, i);
            yield return null;
        }

        if(isFinalFade)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            this.enabled = false;
        }
    }

    private IEnumerator FadeOutImage()
    {
        for(float i = 0; i <= 1; i -= Time.deltaTime)
        {
            img.color = new Color(0.2f, 0.2f, 0.2f, i);
            yield return null;
        }

        if(isFinalFade)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            this.enabled = false;
        }
    }
}