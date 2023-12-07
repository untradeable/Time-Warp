using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Component_LoadScreenTips : MonoBehaviour
{
    [SerializeField] private TMP_Text tipTMP;
    [SerializeField] private string[] tips;
    
    private int id;

    private void Start()
    {
        id = Random.Range(0, tips.Length -1);

        tipTMP.text = tips[id];
    }
}