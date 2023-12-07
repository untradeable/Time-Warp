using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Component_Enemy : MonoBehaviour
{
    //public delegate void EnemySpawnEvent(GameObject _enemy);
    public delegate void DropMoneyEvent(int amount);
    public delegate void DropExperienceEvent(int amount);
    //public delegate void EnemyDieEvent(GameObject _enemy);
    public delegate void EnemyDamagevent(int _damage);

    //public event EnemySpawnEvent SpawnEvent;
    public event DropMoneyEvent  MoneyEvent;
    public event DropExperienceEvent ExperienceEvent;
    //public event EnemyDieEvent   DieEvent;
    public event EnemyDamagevent DamageEvent;

    // ====================================================

    [Header("Status")]
    [SerializeField] private int   life;
    [SerializeField] private int   damage;
    [SerializeField] private float speed;

    [Space]

    [SerializeField] private int   dropAmount;
    [SerializeField] private int   experienceAmount;

    private float originalSpeed;
    private float slownessSpeed;

    [Header("Visual Effects")]

    [SerializeField] private Animator animator;
    [SerializeField] private Animator bossAnimator;

    [Space]
    [SerializeField] private Image healthbar;
    [SerializeField] private GameObject debuffEffect;

    private float maxLife;
    private bool isSlowned;
    private bool isParalyzed;

    [Header("Audios")]

    public AudioSource audioSource;
    public AudioClip   spawnAudio;

    public AudioClip[]   damageAudios;
    public AudioClip[]   deathAudios;

    private int         waypointIndex;
    private Transform[] waypoints;

    // ====================================================

    private void Start()
    {
        originalSpeed = speed;
        slownessSpeed = speed - 8f;

        audioSource.clip = spawnAudio;
        audioSource.Play();

        healthbar.fillAmount = 1f;
        maxLife = life;
    }

    private void Update()
    {
        if(life <= 0)
        {
            Die();
            return;
        }

        if(!isParalyzed)
        {
            PathFollow();
        }
    }

    // ====================================================
    // Walking

    private bool CheckPathEnd()
    {
        if(waypointIndex >= waypoints.Length)
        {
            FindObjectOfType<Manager_Level>().OnEnemyDie(gameObject);
            DamageEvent(damage);

            Destroy(gameObject);

            return true;
        }

        return false;
    }

    private void PathFollow()
    {
        if(Vector3.Distance(transform.position, waypoints[waypointIndex].position) <= 0.1f)
        {
           waypointIndex += 1;
        }

        if(CheckPathEnd())
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);
        transform.LookAt(waypoints[waypointIndex]);
    }

    public void SetWaypoints(Transform[] _waypoints)
    {
        waypoints = _waypoints;
    }

    // ====================================================
    // Damage

    private void ProjectileEffect(string effect, float effectTime)
    {
        switch(effect)
        {
            case "slowness":
                StartCoroutine(DebuffParticles(effectTime));
                StartCoroutine(SlownessEffect(effectTime));
            break;

            case "paralyze":
                StartCoroutine(DebuffParticles(effectTime));
                StartCoroutine(ParalyzeEffect(effectTime));
            break;
                
            default:
                StartCoroutine(DefaultEffect());
            break;
        }
    }

    private IEnumerator ParalyzeEffect(float effectTime)
    {
        StartCoroutine(DamageEffect(effectTime, Color.yellow));
        //float _speed = speed;

        if(!isParalyzed)
        {
            animator.enabled = false;

            if(bossAnimator != null)
            {
                bossAnimator.enabled = false;
            }
            
            isParalyzed = true;
        }

        yield return new WaitForSeconds(effectTime);

        animator.enabled = true;

        if(bossAnimator != null)
        {
            bossAnimator.enabled = true;
        }

        isParalyzed = false;
    }

    private IEnumerator SlownessEffect(float effectTime)
    {
        StartCoroutine(DamageEffect(effectTime, Color.blue));

        if(!isSlowned)
        {
            speed = slownessSpeed;
            isSlowned = true;
        }

        yield return new WaitForSeconds(effectTime);

        speed = originalSpeed;
        isSlowned = false;
    }
    
    private IEnumerator DefaultEffect()
    {
        GetComponent<Renderer>().material.color = Color.red;

        yield return new WaitForSeconds(0.3f);

        GetComponent<Renderer>().material.color = Color.white;
    }

    private IEnumerator DamageEffect(float effectTime, Color _color)
    {
        if(!isParalyzed && !isSlowned)
        {
            Renderer renderer = GetComponent<Renderer>();
        
            renderer.material.color = _color;
        }

        yield return new WaitForSeconds(effectTime);

        GetComponent<Renderer>().material.color = Color.white;
    }

    // ====================================================

    private IEnumerator DebuffParticles(float effectTime)
    {
        debuffEffect.SetActive(true);

        yield return new WaitForSeconds(effectTime);

        debuffEffect.SetActive(false);
    }

    public void LostLife(int _damage, string _effect, float effectTime, Component_Tower tower)
    {
        life -= _damage;
        
        tower.SetIsDead(true);

        AudioClip damageAudio = damageAudios[Random.Range(0, damageAudios.Length - 1)];

        audioSource.clip = damageAudio;
        audioSource.Play();

        ProjectileEffect(_effect, effectTime);

        healthbar.fillAmount = life /  maxLife;
    }

    // ====================================================

    public void OnDeathAudio()
    {
        FindObjectOfType<Manager_Level>().OnEnemyDie(gameObject);

        AudioClip deathAudio = deathAudios[Random.Range(0, deathAudios.Length - 1)];

        audioSource.clip = deathAudio;
        audioSource.Play();
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void Die()
    {
        animator.enabled = true;
        animator.SetInteger("Life", 0);

        if(bossAnimator != null)
        {
            bossAnimator.enabled = true;
            bossAnimator.SetInteger("Life", 0);
        }

        MoneyEvent(dropAmount);
        ExperienceEvent(experienceAmount);

        this.enabled = false;
    }

    // ====================================================

    public int GetLife
    {
        get{return life;}
    }

    public int GetDamage
    {
        get{return damage;}
    }

    public int GetDropAmount
    {
        get{return dropAmount;}
    }
}