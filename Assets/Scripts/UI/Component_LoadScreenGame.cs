using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Component_LoadScreenGame : MonoBehaviour, IPointerClickHandler
{
    private Image img;
    private RectTransform rectTransform;

    private void Start()
    {
        img = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(rectTransform.sizeDelta.x >= 400f)
        {
            rectTransform.sizeDelta = new Vector2(100f, 100f);

            return;
        }

        rectTransform.sizeDelta += new Vector2(30f, 30f);
    }
}