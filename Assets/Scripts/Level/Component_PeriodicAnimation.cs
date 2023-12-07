using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_PeriodicAnimation : MonoBehaviour
{
    public Animator animator;

    private IEnumerator DisableAnimation()
    {
        animator.SetBool("isActive", false);

        yield return new WaitForSeconds(Random.Range(12f, 40f));

        animator.SetBool("isActive", true);
    }
}