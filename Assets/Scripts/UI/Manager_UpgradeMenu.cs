using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager_UpgradeMenu : MonoBehaviour
{
    public delegate void UnlockTowerEvent(int id, int id2);
    public event UnlockTowerEvent UnlockTower;

    [SerializeField] private int         currentPageID = 0;
    [SerializeField] private int         currentExperience;

    public TMP_Text     experienceTMP;
    public TMP_Text[]   pricesTMP;
    public TMP_Text[]   isUnlockedTMP;

    [Space]

    public Image[]      slotImages;    
    public Image[]      iconImages;
    public GameObject[] unlockButtons;
    
    [Space]

    [SerializeField] private UpgradesData[] upgradesData;

    // ====================================================

    public void Start()
    {
        currentExperience = PlayerPrefs.GetInt("Current Experience");
    }

    public void OnDropExperience(int amount)
    {
        currentExperience += amount;

        PlayerPrefs.SetInt("Current Experience", currentExperience);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void DecreaseExperience(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        currentExperience -= amount;
        PlayerPrefs.SetInt("Current Experience", currentExperience);
    }

    public void UpdateMenu()
    {
        Manager_Market market = GetComponent<Manager_Market>();
        experienceTMP.text = $"XP Atual: {currentExperience}";

        for(int i = 0; i < iconImages.Length; i++)
        {
            bool unlockedCheck = market.GetIsUnlocked(currentPageID, i);

            pricesTMP[i].gameObject.SetActive(!unlockedCheck);
            unlockButtons[i].SetActive(!unlockedCheck);

            slotImages[i].color = Color.white;
            iconImages[i].sprite = upgradesData[currentPageID].sprites[i];
            pricesTMP[i].text       = $"Custo: {upgradesData[currentPageID].prices[i]}";

            isUnlockedTMP[i].text = unlockedCheck ? isUnlockedTMP[i].text = "Desbloqueado!" : isUnlockedTMP[i].text = "Bloqueado!";
        }
    }

    public void UnlockItem(int id)
    {
        if(currentExperience >= upgradesData[currentPageID].prices[id])
        {
            UnlockTower(currentPageID, id);

            DecreaseExperience(upgradesData[currentPageID].prices[id]);
            UpdateMenu();
        }
    }

    public void NextPage()
    {
        currentPageID = currentPageID >= upgradesData.Length - 1 ? currentPageID = 0 : currentPageID += 1;
        UpdateMenu();
    }

    public void PreviousPage()
    {
        currentPageID = currentPageID <= 0 ? currentPageID = upgradesData.Length - 1 : currentPageID -= 1;
        UpdateMenu();
    }
}

[System.Serializable]
public class UpgradesData
{
    public string   name;

    public int[]    prices;
    public Sprite[] sprites;
}