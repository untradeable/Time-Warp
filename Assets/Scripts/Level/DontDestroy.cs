using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Start()
    {
        AudioListener[] instances = FindObjectsOfType<AudioListener>();

        for(int i = 0; i < instances.Length; i++)
        {
            if(i != 0)
            {
                Destroy(instances[i].gameObject);
            }
        }

        DontDestroyOnLoad(this);
    }
}