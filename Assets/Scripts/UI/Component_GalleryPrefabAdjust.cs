using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_GalleryPrefabAdjust : MonoBehaviour
{
    public Vector3 position;

    private void Start()
    {
        transform.position = position;
    }
}