using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Component_Tower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void OpenToolsMenuEvent(Component_Tower _tower);
    public delegate void SelectTowerEvent(Component_Tower _tower);
    public delegate void SpawnTowerEvent(Component_Tower _tower);
    
    public event SelectTowerEvent SelectTower;
    public event SpawnTowerEvent    SpawnTower;
    public event OpenToolsMenuEvent OpenToolsMenu;

    public enum TargetType{Lentos, Mediana, Velozes};
    public TargetType targetTypeSelected;

    private GameObject      currentTile;
    private Component_Enemy enemyLocked;
    private Manager_Level   levelManager;

    // ====================================================

    [Header("ID")]
    [SerializeField] private int marketID;
    [SerializeField] private int towerID;

    public int priorityID;

    [Header("Voxel Adjust's")]

    public float adjustX;
    public float adjustY;
    public float adjustZ;
    public float rotationAdjust;
    
    [Header("Badly effect")]
    public bool  haveBadEffect;

    private int   originalDamage;
    private float originalFireRate;
    private float originalRangeSize;

    [Header("Tower Info")]

    [SerializeField] private int        damage;
    [SerializeField] private float      rangeSize;
    [SerializeField] private GameObject rangeObject;
    [SerializeField] private Transform  rangeTransform;

    private Outline outlineComponent;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip shotAudio;

    [Header("Shot Settings")]
    [SerializeField] private bool useShotAnimation;
    [SerializeField] private float fireRate;
    
    private Vector3 oldPosition;

    private float cooldown;
    private bool  isDead = false;

    [SerializeField] private GameObject  projectile;
    [SerializeField] private Transform[] firePoints;
    
    // ====================================================

    private void EnableBoxCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    private void Awake()
    {
        Manager_Events.instance.TowerEvents(this);
    }

    private void Start()
    {
        oldPosition = transform.position;
        GetComponent<BoxCollider>().enabled = false;

        outlineComponent = GetComponent<Outline>();
        rangeObject = transform.GetChild(1).gameObject;
        rangeTransform = transform.GetChild(1);
        rangeTransform.localScale = new Vector3(rangeSize, 0.1f, rangeSize);
        
        originalDamage = damage;
        originalFireRate = fireRate;
        originalRangeSize = rangeSize;

        transform.position = new Vector3(transform.position.x + adjustX, adjustY, transform.position.z + adjustZ);

        Invoke("EnableBoxCollider", 0.2f);
        SpawnTower(this);
    }

    private void Update()
    {
        if(enemyLocked != null)
        {
            CheckTargetPriority();
            CheckTargetOutRange();
            return;
        }

        UpdateTarget();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowRange();

        SelectTower(this);
        OpenToolsMenu(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outlineComponent.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outlineComponent.enabled = false;
    }

    public void ShowRange()
    {
        if(rangeObject != null)
        {
            this.rangeObject.SetActive(true);
        }
    }

    public void HideRange()
    {
        if(rangeObject != null)
        {
            this.rangeObject.SetActive(false);
        }
    }

    // ====================================================
    // Debuff Method's

    public void ActiveBadEffect(int effectID, float effectForce)
    {
        float percentage;

        switch(effectID)
        {
            case 0:
                percentage = (fireRate * effectForce) / 100;
                fireRate = fireRate + percentage;
            break;
            
            case 1:
                percentage = (damage * effectForce) / 100;
                damage = damage - (int) percentage;
            break;

            case 2:
                percentage = (rangeSize * effectForce) / 100;
                rangeSize = rangeSize - percentage;
            break;
        }

        haveBadEffect = true;
    }

    public void RemoveBadEffect()
    {
        damage = originalDamage;
        fireRate = originalFireRate;
        rangeSize = originalRangeSize;

        haveBadEffect = false;
    }

    // ====================================================
    // Shoot Method's

    private void ShotSound()
    {
        if(shotAudio != null)
        {
            audioSource.clip = shotAudio;
            audioSource.Play();
        }
    }
    
    private void CheckTargetOutRange()
    {
        if(isDead)
        {   
            enemyLocked = null;
            isDead = false;

            return;
        }
        
        if(Vector3.Distance(transform.position, enemyLocked.gameObject.transform.position) > rangeSize - 6.5f || enemyLocked.enabled == false)
        {
            enemyLocked = null;
            return;
        }

        LookToEnemy();
        Shoot();
    }

    private void CheckTargetPriority()
    {
        string priorityTag = $"Enemy/{targetTypeSelected}";

        foreach(GameObject enemy in levelManager.enemiesAlive)
        {
            if(enemy.CompareTag(priorityTag) && Vector3.Distance(transform.position, enemy.transform.position) <= rangeSize - 6.5f)
            {
                enemyLocked = enemy.GetComponent<Component_Enemy>();
            }
        }
    }

    private void UpdateTarget()
    {
        CheckTargetPriority();

        if(enemyLocked != null)
        {
            return;
        }

        foreach(GameObject enemy in levelManager.enemiesAlive)
        {   
            if(Vector3.Distance(transform.position, enemy.transform.position) <= rangeSize - 6.5f)
            {
                enemyLocked = enemy.GetComponent<Component_Enemy>();
            }
        }
    }

    private void Shoot()
    {
        if(cooldown > 0f)
        {
            cooldown -= Time.deltaTime;

            return;
        }

        ShotSound();

        foreach(Transform point in firePoints)
        {
            GameObject prjt = Instantiate(projectile, point.position, Quaternion.identity, transform);
            Component_Projectile bullet = prjt.GetComponent<Component_Projectile>();

            bullet.SetTarget(enemyLocked.transform);
            bullet.SetDamage(damage);

            cooldown = fireRate;
        }
    }

    private void LookToEnemy()
    {
        Vector3 dir = enemyLocked.gameObject.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        transform.LookAt(enemyLocked.gameObject.transform.position, transform.right);

        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y - rotationAdjust, 0f);
    }

    public void SetTargetTypeOnUpgrade(int id)
    {
        switch(id)
        {
            case 1:
                targetTypeSelected = TargetType.Mediana;
            break;

            case 2:
                targetTypeSelected = TargetType.Velozes;
            break;

            default:
                targetTypeSelected = TargetType.Lentos;
            break;
        }
    }

    public void SetTargetType()
    {
        priorityID += 1;

        if(priorityID == 3)
        {
            priorityID = 0;
        }

        switch(priorityID)
        {
            case 1:
                targetTypeSelected = TargetType.Mediana;
            break;

            case 2:
                targetTypeSelected = TargetType.Velozes;
            break;

            default:
                targetTypeSelected = TargetType.Lentos;
            break;
        }
    }

    // ====================================================
    // Set ID'S

    public void SetLevelManager(Manager_Level _levelManager)
    {
        levelManager = _levelManager;
    }

    public void SetIsDead(bool self)
    {
        isDead = self;
    }

    public void SetMarketID(int _id)
    {
        marketID = _id;
    }

    public void SetTile(GameObject _tile)
    {
        currentTile = _tile;
    }

    public int GetMarketID()
    {
        return marketID;
    }

    public int GetTowerID()
    {
        return towerID;
    }

    public GameObject GetCurrentTile()
    {
        return currentTile;
    }

    public string GetTowerStat(int id)
    {
        List<string> status = new List<string>();

        status.Add($"Dano: {damage}");

        if(rangeSize < 50)
        {
            status.Add("Curto");
        }
        else if(rangeSize <= 70)
        {
            status.Add("Médio");
        }
        else
        {
            status.Add("Longo");
        }

        status.Add($"Cooldown: {fireRate} segundos");

        return status[id];
    }

    /// ====================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeSize - 6.5f);
    }
}