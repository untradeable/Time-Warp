using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Events : MonoBehaviour
{
    public static Manager_Events instance;

    private Manager_Menu        menuManager;
    private Manager_Market      marketManager;
    private Manager_UpgradeMenu upgradeManager;
    private Manager_Level       levelManager;

    private Component_Tile[]    tiles;

    // ====================================================

    private void Awake()
    {
        instance = this;

        levelManager = FindObjectOfType<Manager_Level>();
        marketManager = FindObjectOfType<Manager_Market>();

        menuManager = FindObjectOfType<Manager_Menu>();
        upgradeManager = FindObjectOfType<Manager_UpgradeMenu>();

        tiles = FindObjectsOfType<Component_Tile>();

        TilesEvents();
        LevelEvents();

        MarketEvents();
    }

    private void TilesEvents()
    {
        foreach(Component_Tile tile in tiles)
        {
            tile.SelectTile += marketManager.OnSelectTile;
            tile.BuyTower   += marketManager.OnBuyTower;
        }
    }

    private void LevelEvents()
    {
        levelManager.WaveEvent   += menuManager.OnWaveEvent;
        levelManager.DamageEvent += menuManager.OnDamageEvent;
        levelManager.LoseScreen  += menuManager.LoseScreen;
        levelManager.WinScreen   += menuManager.WinScreen;
    }

    private void MarketEvents()
    {
        marketManager.BuyAlert     += menuManager.CallAlert;
        marketManager.UpgradeAlert += menuManager.CallAlert;
        marketManager.UpgradeMenu  += menuManager.OnCloseToolseMenu;

        marketManager.BlueprintOnAlert += menuManager.OnBlueprintOn;
        marketManager.BlueprintOffAlert += menuManager.OnBlueprintOff;

        upgradeManager.UnlockTower += marketManager.OnUnlockTower;
    }

    // ====================================================
    // Characters Events

    public void TowerEvents(Component_Tower _tower)
    {
        _tower.SelectTower   += marketManager.OnSelectTower;
        _tower.SpawnTower    += levelManager.OnTowerSpawn;
        _tower.OpenToolsMenu += menuManager.OnOpenToolsMenu;

        //_tower.levelManager.AddTowerInGame(_tower);
    }

    public void EnemiesEvents(Component_Enemy _enemy)
    {
        _enemy.MoneyEvent      +=   marketManager.IncreaseMoney;
        _enemy.ExperienceEvent +=   upgradeManager.OnDropExperience;
        //_enemy.SpawnEvent      +=   levelManager.OnEnemySpawn;

        _enemy.DamageEvent +=   levelManager.OnEnemyDamage;
        //_enemy.DieEvent    +=   levelManager.OnEnemyDie;
    }
}