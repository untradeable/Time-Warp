using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Component_Tower_Helicopter : MonoBehaviour
{
    private bool isUp = false;
    private float speed = 8f;   
    private Vector3 originalPos;
    
    private void Start()
    {
        GetComponent<AudioSource>().Play();

        transform.position = new Vector3(transform.position.x, 5f, transform.position.z);

        originalPos = transform.position;
    }

    private void FixedUpdate()
    {
        Fly();
    }

    private void Fly()
    {
        if(isUp)
        {
            GoDown();
        }
        else
        {
            Vector3 upPos = new Vector3(originalPos.x, 15f, originalPos.z);
            transform.position = Vector3.MoveTowards(transform.position, upPos, speed * Time.deltaTime);

            if(transform.position.y >= 15)
            {
                isUp = true;
            }
        }
    }

    private void GoDown()
    {
        Vector3 downPos = new Vector3(originalPos.x, 5f, originalPos.z);
        transform.position = Vector3.MoveTowards(transform.position, downPos, speed * Time.deltaTime);

        if(transform.position.y <= 5)
        {
            isUp = false;
        }
    }
}