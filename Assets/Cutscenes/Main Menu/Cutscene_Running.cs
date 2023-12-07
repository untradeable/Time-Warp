using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Running : MonoBehaviour
{
    public Transform location;
    public Transform target;
    public float speed;

    private void Start()
    {
        location = transform.parent;
    }

    private void Update()
    {
        if(Vector3.Distance(location.position, target.position) < 3f)
        {
            Destroy(location.gameObject);
        }

        location.position =  Vector3.MoveTowards(transform.position, target.position, speed * 2f * Time.deltaTime);
    }
}