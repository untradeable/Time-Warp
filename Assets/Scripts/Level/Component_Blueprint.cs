using UnityEngine;

public class Component_Blueprint : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;

    private Manager_Market marketManager;
    private Manager_Menu menuManager;

    [SerializeField] private bool adjustY;

    private void Start()
    {
        menuManager = FindObjectOfType<Manager_Menu>();
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Input.GetMouseButtonDown(1))
        {
            marketManager.ResetID();
            marketManager.EndBuying();

            menuManager.OnBlueprintOff();
            Destroy(gameObject);
        }

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        { 
            transform.position = hit.point;

            if(adjustY)
            {
                transform.position = new Vector3(transform.position.x, 4.4f, transform.position.z);
            }
        }
    }

    public void SetMarket(Manager_Market _manager)
    {
        marketManager = _manager;
    }
}
