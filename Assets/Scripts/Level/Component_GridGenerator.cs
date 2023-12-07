using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_GridGenerator : MonoBehaviour
{
    public int        size;
    
    public float      tileScale;
    public GameObject tile;

    private void Start()
    {
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(x * tileScale, 0f, y * tileScale), Quaternion.identity, transform);
            }
        }
    }
}