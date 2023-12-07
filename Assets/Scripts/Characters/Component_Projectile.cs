using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Projectile : MonoBehaviour
{
    private enum EffectType{normal, slowness, paralyze, burn};

    [SerializeField] private float      effectTime;
    [SerializeField] private EffectType prjEffect;

    private Transform target;
    private int damage;
    private float speed = 120f;

    // ====================================================

    private void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        SeekTarget();
    }

    private void SeekTarget()
    {
        Vector3 dir = target.position - transform.position;
        float distance = speed * Time.deltaTime;

        if(dir.magnitude <= distance)
        {
            DealDamage();

            return;
        }
        
        transform.Translate(dir.normalized * distance, Space.World);
        transform.LookAt(target.position);
    }

    public void DealDamage()
    {
        try
        {
            Component_Enemy enemy = target.GetComponent<Component_Enemy>();
            enemy.LostLife(damage, prjEffect.ToString(), effectTime, transform.parent.GetComponent<Component_Tower>());
        }    
        catch(System.NullReferenceException){}
        finally
        {
            Destroy(gameObject);
        }    
    }

    // ====================================================
    // Set projectile attributes
    public void SetDamage(int _damage)
    {
        damage = _damage;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}