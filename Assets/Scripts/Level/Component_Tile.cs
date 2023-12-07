using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Component_Tile : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public delegate void        SelectTileEvent(bool _activeSelf, Color32 _color, Transform _position);
    public delegate void        BuyTowerEvent(GameObject _tile);

    public event SelectTileEvent SelectTile;
    public event BuyTowerEvent   BuyTower;

    // ====================================================

    private Color32 FreeSpaceChecking()
    {
        if(CompareTag("Tile/Empty"))
        {
            return new Color32(0, 140, 255, 80);
        }

        return new Color32(255, 0, 0, 80);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectTile(true, FreeSpaceChecking(), transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerId == -1 && CompareTag("Tile/Empty"))
        {
            BuyTower(gameObject);
            SelectTile(true, FreeSpaceChecking(), transform);   
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SelectTile(false, FreeSpaceChecking(), transform);
    }
}