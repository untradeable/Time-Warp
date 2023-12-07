using UnityEngine;

public class Component_Helicopter : MonoBehaviour
{   
    private Animator animator;
    
    private Manager_Level levelManager;

    private int        marketID;
    private GameObject tower;
    private GameObject tile;

    private Transform  towerPosition;
    private Vector3  targetPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = new Vector3(towerPosition.position.x, 20f, towerPosition.position.z + 6f);
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            animator.SetBool("isArrived", true);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 180f * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 20f, transform.position.z);

        var lookDir = targetPosition - transform.position;
        lookDir.y = 0;
        
        transform.rotation = Quaternion.Euler(-lookDir);
    }

    public void SendDelivery()
    {
        GameObject newTower = Instantiate(tower, towerPosition.position, Quaternion.identity, transform.parent);
        
        newTower.GetComponent<Component_Tower>().SetTile(tile);
        newTower.GetComponent<Component_Tower>().SetLevelManager(levelManager);

        Destroy(gameObject);
    }

    public void SetDelivery(GameObject _tower, GameObject _tile, Transform _towerPosition, Manager_Level _levelManager)
    {
        tower = _tower;
        tile = _tile;
        towerPosition = _towerPosition;
        levelManager = _levelManager;
    }
}