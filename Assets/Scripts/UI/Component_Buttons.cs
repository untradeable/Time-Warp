using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Component_Buttons : MonoBehaviour
{   
    [SerializeField] private Data_Save dataSave;

    [Header("Configuração do highligth nos textos")][Space]
    public float         originalFontSize  = 55f;
    public float         highlightFontSize = 65f;

    public Color         defaultColor;
    public Color         highlightColor;

    public GameObject    loadScreenObj;
    public GameObject[]  menuButtons;
    public string[]      scenes;

    [Space]

    [Header("Efeito na troca de menus")][Space]
    public RectTransform transitionIMG;
    public bool          useTransition;   
    
    [Space]
    
    [Header("Exibição das fases")][Space]
    public TMP_Text      faseNameTMP;
    public TMP_Text      faseDescTMP;

    public string[]      faseNames;
    public string[]      faseDescriptions;

    public GameObject[]  fasePreviewObjects;

    // ==============================================
    // On Click

    public void ActiveObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void DesactiveObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void OpenMenu(GameObject menu)
    {
        DisableHighligth();

        if(useTransition)
        {
            StartCoroutine(Transition(menu));
            return;
        }

        menu.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator Transition(GameObject menu)
    {
        float size = 0;

        while(size < 1934)
        {
            size += 300f;

            transitionIMG.anchoredPosition = new Vector2(transitionIMG.anchoredPosition.x + 150f, transitionIMG.anchoredPosition.y);
            transitionIMG.sizeDelta = new Vector2(size, 1081f);

            yield return null;
        }

        gameObject.SetActive(false);
        menu.SetActive(true);

        transitionIMG.anchoredPosition = new Vector2(0f, transitionIMG.anchoredPosition.y);
        transitionIMG.sizeDelta = new Vector2(0f, 1081f);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();     
    }

    public void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void FaseLoader(int id)
    {
        if(!dataSave.CheckIsUnlocked(id))
        {
            return;
        }

        StartCoroutine(LoadScreen(scenes[id]));
    }

    public IEnumerator LoadScreen(string sceneName)
    {
        loadScreenObj.SetActive(true);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadSceneAsync(sceneName);
    }
     
    public void ExitGame()
    {
        Application.Quit();
    }

    // ==============================================
    // On Enter/On Exit
    
    public void DisplayFaseInfo(int id)
    {
        faseNameTMP.text = faseNames[id];
        fasePreviewObjects[id].SetActive(!fasePreviewObjects[id].activeSelf);

        if(!dataSave.CheckIsUnlocked(id))
        {
            faseDescTMP.text = "Nivel bloqueado!";

            return;
        }

        faseDescTMP.text = faseDescriptions[id];
    }

    public void ResetFaseInfo()
    {
        faseNameTMP.text = "Escolha o nivel";
        faseDescTMP.text = "";
    }

    public void HighlightText(TMP_Text textTMP)
    {
        DisableHighligth();
        
        textTMP.fontSize = highlightFontSize;
        textTMP.color = highlightColor;
    }

    public void DisableHighligth()
    {
        foreach(GameObject btn in menuButtons)
        {
            TMP_Text textTMP = btn.transform.GetChild(0).GetComponent<TMP_Text>();

            textTMP.fontSize = originalFontSize;
            textTMP.color = defaultColor;
        }
    }
}