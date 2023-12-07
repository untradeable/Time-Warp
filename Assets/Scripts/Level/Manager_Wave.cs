using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Wave : MonoBehaviour
{
    [SerializeField] private Manager_Level levelManager;

    private float spawnRate;
    private bool  canStartSpawn = false;
    
    [Space]
    public Transform   spawnLocation;
    public Transform   enemiesRoot;
    public Transform[] path;

    [Header("Configuração da Wave")]
    public WaveData   currentWave;
    [SerializeField] private WaveData[] waves;

    // ====================================================

    private void Update()
    {
        if(canStartSpawn && !IsEnd())
        {
            WaveSpawner();
        }
    }

    private void FixedUpdate()
    {
        if(currentWave.isSpecialWave)
        {
            SpecialEffect();
        }
        else
        {
            RemoveSpecialEffect();
        }
    }

    public void WaveSpawner()
    {
        if(spawnRate > 0f)
        {
            spawnRate -= Time.deltaTime;

            return;
        }

        for(int i = 0; i < currentWave.quantity.Length; i++)
        {
            if(currentWave.quantity[i] > 0f && spawnRate <= 0f)
            {
                GameObject enemy = Instantiate(currentWave.prefabs[i], spawnLocation.position, Quaternion.identity, enemiesRoot);

                Component_Enemy enemyComponent = enemy.GetComponent<Component_Enemy>();
                Manager_Events.instance.EnemiesEvents(enemyComponent);
                enemyComponent.SetWaypoints(path);

                currentWave.quantity[i] -= 1;
                spawnRate = currentWave.delay;

                levelManager.OnEnemySpawn(enemy);
            }
        }
    }

    public void SpecialEffect()
    {
        foreach(Component_Tower tower in levelManager.towersInGame)
        {
            if(!tower.haveBadEffect)
            {
                tower.ActiveBadEffect(currentWave.effectID, currentWave.effectForce);
            }
        }
    }

    public void RemoveSpecialEffect()
    {
        foreach(Component_Tower tower in levelManager.towersInGame)
        {
            tower.RemoveBadEffect();
        }
    }

    public bool IsEnd()
    {
        if(currentWave.quantity[currentWave.quantity.Length - 1] <= 0f)
        { 
            canStartSpawn = false;

            return true;
        }

        return false;
    }

    // ====================================================

    public void SetCanSpawn(bool _self)
    {
        canStartSpawn = _self;
        
        Manager_Menu m = FindObjectOfType<Manager_Menu>();

        if(!currentWave.isSpecialWave)
        {
            m.ChangeSpecialIcon(3);
            m.DisableSpecialWaveEffect();

            return;
        }

        m.ChangeSpecialIcon(currentWave.effectID);
    }

    public int GetWavesLength()
    {
        return waves.Length - 1;
    }

    public void SetCurrentWave(int _waveID)
    {
        if(_waveID > waves.Length - 1)
        {
            this.enabled = false;

            return;
        }
        
        currentWave = waves[_waveID];
    }
}

[System.Serializable]
public class WaveData
{
    public bool isSpecialWave;
    public bool isStart;
    
    public int effectID;
    
    [Range(10f,50f)]
    public float effectForce;

    [Space]
    public float        delay;
    public int[]        quantity;
    public GameObject[] prefabs;
}