using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_Market : MonoBehaviour
{
    public delegate IEnumerator BuyAlertEvent(string text, float timer);
    public delegate IEnumerator UpgradeAlertEvent(string text, float timer);

    public delegate void BlueprintOnEvent();
    public delegate void BlueprintOffEvent();
    public delegate void UpgradeMenuEvent();

    public event UpgradeAlertEvent BuyAlert;
    public event UpgradeAlertEvent UpgradeAlert;

    public event BlueprintOnEvent BlueprintOnAlert;
    public event BlueprintOffEvent BlueprintOffAlert;
    public event UpgradeMenuEvent  UpgradeMenu;

    private int marketID  = -1;
    private int upgradeID = -1;
    private bool isBuying = false;

    // ====================================================

    [Header("Tower Market Bar")]
    [SerializeField] private int          currentMoney;
    [SerializeField] private GameObject   currentBlueprint;

    [Space]

    [SerializeField] private GameObject   toolsMenu;
    [SerializeField] private GameObject[] informationTabs;
    
    [Space]
    
    [Tooltip("Preços, prefabs e upgrades")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buyAudio;
    [SerializeField] private AudioClip upgradeAudio;

    [SerializeField] private TowerData[]     towersData;
    [SerializeField] private GameObject[]    blueprints;

    [Space]

    [SerializeField] private TMP_Text     moneyTMP;
    [SerializeField] private TMP_Text     towerPriorityTMP;
    [SerializeField] private TMP_Text     towerPriorityDescTMP;
    [SerializeField] private TMP_Text[]   towerPricesTMP;
    [SerializeField] private GameObject[] lockedPanels;

    [Header("Market Features")]
    public GameObject      selectionTile;
    
    public GameObject marketHelicopter;
    public GameObject smokeEffect;
    public Transform  helicopterSpawnPos;

    public Transform  towerRoot;
    private Component_Tower selectedTower;

    private bool[]  canUseHotkey = new bool[3];
    private float[] cooldown = new float[3];

    private List<UpgradeData> upgradeList = new List<UpgradeData>();

    // ====================================================

    private void Awake()
    {
        LoadUnlockedTowers();
        SetupMarketBar();
    }

    private void Start()
    {
        for(int index = 0; index < canUseHotkey.Length; index++)
        {
            canUseHotkey[index] = true;
        }
    }

    private void Update()
    {
        moneyTMP.text = $"{currentMoney}";

        if(selectedTower != null)
        {
            if(Input.GetKeyDown(KeyCode.F3) && canUseHotkey[0])
            {
                OnUpgrade();
                StartCoroutine(HotkeyCooldown(0));
            }

            if(Input.GetKeyDown(KeyCode.F4) && canUseHotkey[1])
            {
                SellTower();
                CloseToolsMenuHotKey();

                StartCoroutine(HotkeyCooldown(1));
            }

            if(Input.GetKeyDown(KeyCode.F5))
            {
                SetTowerPriority();
                StartCoroutine(HotkeyCooldown(2));
            }

            if(Input.GetKeyDown(KeyCode.Escape) && toolsMenu.activeSelf == true)
            {
                CloseToolsMenuHotKey();
            }
        }
    }

    private IEnumerator HotkeyCooldown(int id)
    {
        canUseHotkey[id] = false;

        yield return new WaitForSeconds(1.6f);

        canUseHotkey[id] = true;
    }

    private void SetupMarketBar()
    {
        for(int i = 0; i < towerPricesTMP.Length; i++)
        {
            towerPricesTMP[i].text = $"{towersData[i].prices[0]}";
            
            if(towersData[i].isUnlocked[0])
            {
                lockedPanels[i].SetActive(false);
            }
        }
    }

    private void LoadUnlockedTowers()
    {
        for(int i = 0; i < towersData.Length; i++)
        {
            for(int j = 0; j < towersData[i].prefabs.Length; j++)
            {
                if(PlayerPrefs.GetInt($"ID: {i} Fase: {j}") == 1)
                {
                    towersData[i].isUnlocked[j] = true;
                }
            }
        }
    }

    // ====================================================
    // ID's and Buttons

    public TowerData GetTowerData(int id)
    {
        return towersData[id];
    }

    public void SetMarketID(int _id)
    {
        marketID = _id;
    }

    public void HideRange()
    {
        selectedTower.HideRange();
    }

    public void BuyButton(int _id)
    {
        Destroy(currentBlueprint);

        marketID = _id;
        isBuying = true;

        currentBlueprint = Instantiate(blueprints[marketID], Vector3.zero, Quaternion.identity);
        currentBlueprint.GetComponent<Component_Blueprint>().SetMarket(this);

        BlueprintOnAlert();
    }   
    public void ResetAll()
    {
        Destroy(currentBlueprint);

        marketID = -1;
        isBuying = false;
    }

    public void ResetID()
    {
        marketID = -1;
    }

    public void EndBuying()
    {
        isBuying = false;
    }

    public void IncreaseMoney(int _price)
    {
        currentMoney += _price;
    }

    public void DecreaseMoney(int _price)
    {
        currentMoney -= _price;
    }

    public void CloseToolsMenuHotKey()
    {
        ResetAll();
        HideRange();

        selectedTower = null;

        GetComponent<Manager_Menu>().OnCloseToolseMenu();

        foreach(GameObject tab in informationTabs)
        {
            tab.SetActive(false);
        }
    }

    // ====================================================
    // Set Tower Priority

    public void SetTowerPriority()
    {
        if(selectedTower.priorityID == 0)
        {
            selectedTower.SetTargetType();

            towerPriorityTMP.text = selectedTower.targetTypeSelected.ToString();
            towerPriorityDescTMP.text = "Focado em inimigos de velocidade mediana";
            
            return;
        }

        if(selectedTower.priorityID == 1)
        {
            selectedTower.SetTargetType();

            towerPriorityTMP.text = selectedTower.targetTypeSelected.ToString();
            towerPriorityDescTMP.text = "Focado em inimigos de velocidade alta";

            return;
        }

        if(selectedTower.priorityID == 2)
        {
            selectedTower.SetTargetType();

            towerPriorityTMP.text = selectedTower.targetTypeSelected.ToString();
            towerPriorityDescTMP.text = "Focado em inimigos de velocidade baixa";
        }
    }

    private void PriorityDesc()
    {
        if(selectedTower.priorityID == 0)
        {
            towerPriorityDescTMP.text = "Focado em inimigos de velocidade baixa";
            return;
        }

        if(selectedTower.priorityID == 1)
        {
            towerPriorityDescTMP.text = "Focado em inimigos de velocidade mediana";
            return;
        }

        if(selectedTower.priorityID == 2)
        {
            towerPriorityDescTMP.text = "Focado em inimigos de velocidade alta";
        }
    }

    // ====================================================
    // Sell Tower

    public void SellTower()
    {
        GameObject _tile = selectedTower.GetComponent<Component_Tower>().GetCurrentTile();
        _tile.tag = "Tile/Empty";

        int decrease = (SellPrice() * 20) / 100;

        GetComponent<Manager_Level>().OnRemoveTower(selectedTower);

        IncreaseMoney(SellPrice() - decrease);
        Destroy(selectedTower.gameObject);
    }

    public int SellPrice()
    {
        return towersData[selectedTower.GetMarketID()].prices[selectedTower.GetTowerID()];
    }

    // ====================================================
    // Buy Tower

    private bool CanBuy(int _price)
    {
        if(currentMoney >= _price)
        {
            return true;
        }

        StartCoroutine(BuyAlert("Dinheiro insuficiente!", 1f));
        return false;
    }

    private bool IsBuying()
    {
        if(marketID <= -1 || !isBuying)
        {
            return false;
        }

        return true;
    }

    public void OnSelectTile(bool _activeSelf, Color32 _color, Transform _tilePostion)
    {
        selectionTile.SetActive(_activeSelf);
        selectionTile.GetComponent<Renderer>().material.color = _color;
        selectionTile.transform.position = _tilePostion.position;
    }

    public void OnSelectTower(Component_Tower _tower)
    {
        EndBuying();

        if(selectedTower != null && selectedTower != _tower)
        {
            selectedTower.HideRange();
        }

        selectedTower = _tower;
        
        PriorityDesc();
        towerPriorityTMP.text = selectedTower.targetTypeSelected.ToString();

        marketID  = selectedTower.GetMarketID();
        upgradeID = selectedTower.GetTowerID();
    }
    
    public void OnBuyTower(GameObject _tile)
    {
        if(!IsBuying())
        {
            return;
        }

        if(!CanBuy(towersData[marketID].prices[0]))
        {
            return;
        }

        audioSource.clip = buyAudio;
        audioSource.Play();

        GameObject helicopter = Instantiate(marketHelicopter, helicopterSpawnPos.position, Quaternion.identity, towerRoot);
        Component_Helicopter helicopterScript = helicopter.GetComponent<Component_Helicopter>();

        helicopterScript.SetDelivery(towersData[marketID].prefabs[0], _tile, _tile.transform, GetComponent<Manager_Level>());
        _tile.tag = "Tile/Busy";

        DecreaseMoney(towersData[marketID].prices[0]);
        EndBuying();
        ResetID();

        Destroy(currentBlueprint);

        BlueprintOffAlert();
    }

    // ====================================================
    // Upgrade Tower

    private bool IsUnlocked()
    {
        if(towersData[selectedTower.GetMarketID()].isUnlocked[selectedTower.GetTowerID() + 1])
        {
            return true;
        }

        return false;
    }

    public int UpgradePrice()
    {
        return towersData[selectedTower.GetMarketID()].prices[selectedTower.GetTowerID() + 1];
    }

    private GameObject UpgradedTower()
    {
        return towersData[selectedTower.GetMarketID()].prefabs[selectedTower.GetTowerID() + 1];
    }

    public void OnUnlockTower(int id, int id2)
    {
        towersData[id].isUnlocked[id2] = true;
        PlayerPrefs.SetInt($"ID: {id} Fase: {id2}", 1);

        if(id2 == 0)
        {
            lockedPanels[id].SetActive(false);
        }
    }

    public bool GetIsUnlocked(int id, int id2)
    {
        return towersData[id].isUnlocked[id2];
    }

    public void OnUpgrade()
    {
        if(selectedTower.GetTowerID() >= towersData[selectedTower.GetMarketID()].prefabs.Length - 1)
        {
            return;
        }

        if(!IsUnlocked())
        {
            StartCoroutine(UpgradeAlert("Desbloqueie essa torre primeiro!", 1f));
            return;
        }

        if(!CanBuy(UpgradePrice()))
        {
            return;
        }

        audioSource.clip = upgradeAudio;
        audioSource.Play();

        UpgradeMenu();
        HideRange();

        StartCoroutine(SpawnUpgrade());
    }
    
    private IEnumerator SpawnUpgrade()
    {
        UpgradeData upgrade = new UpgradeData();

        upgrade.SetTower = selectedTower.gameObject;
        upgrade.SetUpgrade = UpgradedTower();

        upgradeList.Add(upgrade);

        int id = upgradeList.Count - 1;
        int price = UpgradePrice();

        upgrade.SetTower.GetComponent<BoxCollider>().enabled = false;
        GetComponent<Manager_Level>().OnRemoveTower(upgrade.SetTower.GetComponent<Component_Tower>());

        GameObject smoke = Instantiate(smokeEffect, selectedTower.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));

        yield return new WaitForSeconds(1.5f);
        
        ResetAll();

        GameObject oldTower = upgradeList[id].SetTower;
        GameObject upgradedTower = upgradeList[id].SetUpgrade;
        
        Vector3 location = oldTower.transform.position;
        GameObject up = Instantiate(upgradedTower, location, Quaternion.identity, towerRoot);

        up.GetComponent<Component_Tower>().SetTile(oldTower.GetComponent<Component_Tower>().GetCurrentTile());
        up.GetComponent<Component_Tower>().SetLevelManager(GetComponent<Manager_Level>());
        up.GetComponent<Component_Tower>().SetTargetTypeOnUpgrade(oldTower.GetComponent<Component_Tower>().priorityID);

        DecreaseMoney(price);
        
        Destroy(smoke);
        Destroy(oldTower);
        
        upgradeList.Remove(upgrade);
    }
}

[System.Serializable]
public class TowerData
{
    public string       towerName;

    public int[]        prices;
    public bool[]       isUnlocked;
    public GameObject[] prefabs;
}

public class UpgradeData
{
    private GameObject tower;
    private GameObject upgrade;

    public GameObject SetTower
    {
        get{return tower;}
        set{tower = value;}
    }

    public GameObject SetUpgrade
    {
        get{return upgrade;}
        set{upgrade = value;}
    }
}