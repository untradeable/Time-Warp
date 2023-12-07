using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_Level : MonoBehaviour
{
    public delegate IEnumerator NewWaveEvent(float timer);
    public delegate IEnumerator  TakeDamageEvent();

    public delegate void        LoseScreenEvent();
    public delegate void        WinScreenEvent();

    public event NewWaveEvent       WaveEvent;
    public event TakeDamageEvent    DamageEvent;
    public event LoseScreenEvent    LoseScreen;
    public event WinScreenEvent     WinScreen;

    [SerializeField] private Data_Save dataSave;

    [SerializeField] private int faseID;
    [SerializeField] private bool isSpecialFase;

    [Header("Player")]
    [SerializeField] private int currentLife;

    public TMP_Text currentLifeTMP;

    [Header("Wave")]
    [SerializeField] private int waveRound;
    [SerializeField] private int waveMax;

    [Space]
    public Animator wavePortal;
    public GameObject guideTMP;

    public TMP_Text enemieCountTMP;
    public TMP_Text waveTMP;

    [Space]

    public int enemiesInWave;

    public List<Component_Tower> towersInGame = new List<Component_Tower>();
    public List<GameObject> enemiesAlive = new List<GameObject>();
    public List<Manager_Wave> waves = new List<Manager_Wave>();

    // ====================================================

    private void Awake()
    {
        foreach(Manager_Wave wave in waves)
        {
            if(wave.GetWavesLength() > waveMax)
            {
                waveMax = wave.GetWavesLength();
            }
        }
    }
    
    private void Start()
    {
        StartCoroutine(NewWave());
        StartCoroutine(Guide());

        currentLifeTMP.text = currentLife.ToString();
    }
    
    private IEnumerator Guide()
    {
        TMP_Text tmp1 = guideTMP.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text tmp2 = guideTMP.transform.GetChild(1).GetComponent<TMP_Text>();

        guideTMP.SetActive(true);

        yield return new WaitForSeconds(15f);

        float i = 1;

        while(i > 0)
        {
            i -= 0.1f;
            tmp1.color = new Color(1, 1, 1, i);
            tmp2.color = new Color(1, 1, 1, i);

            yield return null;
        }
    }

    // ====================================================
    // Towers
    
    public void OnTowerSpawn(Component_Tower t)
    {
        towersInGame.Add(t);
    }

    public void OnRemoveTower(Component_Tower t)
    {
        towersInGame.Remove(t);
    }

    // ====================================================
    // Enemies
    
    public void OnEnemySpawn(GameObject _enemy)
    {
        enemiesAlive.Add(_enemy);
    }

    public void OnEnemyDie(GameObject _enemy)
    {
        enemiesAlive.Remove(_enemy);
        enemiesInWave -= 1;

        enemieCountTMP.text = $"Inimigos restante: {enemiesInWave}";

        WaveManager();
    }

    public void OnEnemyDamage(int damage)
    {
        currentLife -= damage;
        currentLifeTMP.text = currentLife.ToString();
        
        StartCoroutine(DamageEvent());
        WaveManager();
    }

    // ====================================================
    // Wave

    private void WaveManager()
    {
        if(currentLife <= 0)
        {
            StartCoroutine(CallLoseSecreen());
        }

        if(enemiesInWave <= 0 && currentLife > 0f)
        {
            waveRound += 1;

            if(waveRound > waveMax)
            {
                StartCoroutine(CallWinScreen());
            }
            else
            {
                StartCoroutine(NewWave());
            }
        }
    }

    private IEnumerator NewWave()
    {
        if(wavePortal != null)
        {
            wavePortal.SetBool("isOpen", false);    
        }

        foreach(Manager_Wave wave in waves)
        {
            wave.SetCurrentWave(waveRound);

            for(int i = 0; i < wave.currentWave.quantity.Length; i++)
            {
                enemiesInWave += wave.currentWave.quantity[i];
            }
        }

        StartCoroutine(WaveEvent(10f));

        enemieCountTMP.text = $"Inimigos restante: ---";

        yield return new WaitForSeconds(10f / 1f);

        enemieCountTMP.text = $"Inimigos restante: {enemiesInWave}";

        OnStartWave();
    }

    private void OnStartWave()
    {
        if(wavePortal != null)
        {
            wavePortal.SetBool("isOpen", true);
        }

        waveTMP.text = $"{waveRound + 1}/{waveMax + 1}";

        foreach(Manager_Wave wave in waves)
        {
            wave.SetCanSpawn(true);
        }
    }

    private IEnumerator CallLoseSecreen()
    {
        yield return new WaitForSeconds(1f);
        
        Time.timeScale = 0f;
        LoseScreen();
    }

    private IEnumerator CallWinScreen()
    {
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0f;
        
        WinScreen();
        dataSave.UnlockLevel(faseID);
    }

    private bool CheckGameEnd()
    {
        if(waveRound > waveMax)
        {
            return true;
        }

        return false;
    }

    public int GetFaseID
    {
        get{return faseID;}
    }
}