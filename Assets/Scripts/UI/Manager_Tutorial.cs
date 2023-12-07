using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Tutorial : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject chooseButtons;
    public GameObject initialMenu;
    public TMP_Text closeTMP;

    [Header("Dialogue")]
    public TMP_Text dialogueTMP;
    public string sentence;
    public float charDelay;

    [Header("Tutorial Images")]
    public Image pageImg;
    public Sprite[] pages;
    public GameObject prevButton;
    public GameObject nextButton;

    public int pageIndex;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Is First Play") == 1)
        {
            initialMenu.SetActive(true);
            gameObject.SetActive(false);

            return;
        }

        StartCoroutine(TextWriter());
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("Is First Play", 0);
    }

    public void CloseTutorial()
    {
        PlayerPrefs.SetInt("Is First Play", 1);
    }

    public void NextPage()
    {
        pageIndex += 1;
        prevButton.SetActive(true);
        closeTMP.text = "Pular Tutorial";

        if(pageIndex >= pages.Length - 1)
        {
            nextButton.SetActive(false);
            pageImg.sprite = pages[pageIndex];
            closeTMP.text = "Fechar";

            return;
        }

        nextButton.SetActive(true);
        pageImg.sprite = pages[pageIndex];
    }

    public void PrevPage()
    {
        pageIndex -= 1;
        nextButton.SetActive(true);

        if(pageIndex <= 0)
        {
            prevButton.SetActive(false);
            pageImg.sprite = pages[pageIndex];

            return;
        }

        prevButton.SetActive(true);
        pageImg.sprite = pages[pageIndex];
    }

    public IEnumerator TextWriter()
    {
        foreach(char i in sentence)
        {
            dialogueTMP.text += i;

            yield return new WaitForSeconds(charDelay);
        }

        chooseButtons.SetActive(true);
    }
}