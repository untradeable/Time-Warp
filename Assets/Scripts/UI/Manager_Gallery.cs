using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Gallery : MonoBehaviour
{
    [Header("See Tower/Enemies")]

    private GameObject    currentPrefab;

    public TMP_Text      nickname;
    public TMP_Text      towerName;
    public TMP_Text      description;

    [Space]
    
    public GameObject    enemyInfoWindow;
    public TMP_Text[]    prefabStat;

    [Space]

    public TowerInfo[]   informations;
    
    public TowerInfo[]   towerInfo;
    public TowerInfo[]   enemyInfo;

    [Space]
    
    public Image[]       gridImages;
    public Sprite[]      towerSprites;
    public Sprite[]      enemiesSprites;

    [Space]

    public GameObject[]  towerPrefabs;
    public GameObject[]  enemiesPrefabs;

    private int          currentID;
    private GameObject[] currentPrefabs;

    public void Start()
    {
        TowerGrid();
    }

    public void NextButton()
    {
        DestroyObject();

        if(currentID + 1 > currentPrefabs.Length - 1)
        {
            SpawnObject(0);

            return;
        }

        currentID += 1;
        SpawnObject(currentID);
    }

    public void PreviousButton()
    {
        DestroyObject();

        if(currentID - 1 <= -1)
        {
            SpawnObject(currentPrefabs.Length - 1);

            return;
        }

        currentID -= 1;
        SpawnObject(currentID);
    }

    public void SpawnObject(int id)
    {
        TowerInfo currentInfo = informations[id];

        currentPrefab    = Instantiate(currentPrefabs[id], Vector3.zero, Quaternion.identity);
        currentID = id;

        nickname.text    = currentInfo.nickname;
        towerName.text   = currentInfo.towerName;
        description.text = currentInfo.description;
        
        enemyInfoWindow.SetActive(false);

        if(currentInfo.enemyPrefab != null)
        {
            enemyInfoWindow.SetActive(true);
            
            Component_Enemy e = currentInfo.enemyPrefab.GetComponent<Component_Enemy>();

            prefabStat[0].text = $"Vida: {e.GetLife}";
            prefabStat[1].text = $"Dano: {e.GetDamage}";
            prefabStat[2].text = $"Moedas dropadas: {e.GetDropAmount}";
        }
    }

    public void DestroyObject()
    {
        Destroy(currentPrefab);
    }

    public void TowerGrid()
    {
        currentPrefabs = towerPrefabs;
        informations =  towerInfo;

        foreach(Image img in gridImages)
        {
            img.gameObject.SetActive(false);
            img.sprite = null;
        }

        for(int i = 0; i < towerSprites.Length; i++)
        {
            gridImages[i].gameObject.SetActive(true);
            gridImages[i].sprite = towerSprites[i];
        }
    }

    public void EnemiesGrid()
    {
        currentPrefabs = enemiesPrefabs;
        informations = enemyInfo;

        foreach(Image img in gridImages)
        {
            img.gameObject.SetActive(false);
            img.sprite = null;
        }

        for(int i = 0; i < enemiesSprites.Length; i++)
        {
            gridImages[i].gameObject.SetActive(true);
            gridImages[i].sprite = enemiesSprites[i];
        }
    }
}

[System.Serializable]
public class TowerInfo
{
    public string nickname;
    public string towerName;
    public string description;

    [Space]
    public GameObject enemyPrefab;
}