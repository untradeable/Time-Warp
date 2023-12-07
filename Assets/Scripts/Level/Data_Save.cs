using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Save : MonoBehaviour
{
    [SerializeField]
    private bool[] unlockeds = new bool[4];

    private void Awake()
    {
        UnlockLevel(0);
        UpdateLevels();
    }
    
    private void UpdateLevels()
    {
        for(int i = 0; i < unlockeds.Length; i++)
        {
            if(PlayerPrefs.GetInt($"Fase {i} unlocked") == 1)
            {
                unlockeds[i] = true;
            }
        }
    }

    public bool CheckIsUnlocked(int id)
    {
        return unlockeds[id];
    }

    public void UnlockLevel(int id)
    {
        PlayerPrefs.SetInt($"Fase {id} unlocked", 1);
    }   

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}