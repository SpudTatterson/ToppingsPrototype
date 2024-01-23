using UnityEngine;

public class Placement : MonoBehaviour
{
    // this code is disgusting 
    Camera cam;
    [SerializeField] LayerMask groundLayer;
    public GameObject itemToPlace;
    GameObject tempGO;

    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(camRay, out hit, Mathf.Infinity, groundLayer) && itemToPlace != null)
        {
            if(Input.GetButtonDown("Fire2"))
            {
                itemToPlace = null;
                Destroy(tempGO);
                return;
            }
            Vector3 hitPoint = hit.point;
            if(tempGO == null && itemToPlace != null)
                tempGO = Instantiate(itemToPlace);// spawn visual aid if it doesn't exist
            tempGO.transform.position = hitPoint;
            if (Input.GetButtonDown("Fire1"))
            {
                Destroy(tempGO); // destroy visual aid
                
                SpawnPrefab(hitPoint); // spawn actual object
                // should probably add them to list or something to keep track of them
                //maybe add last placed and a control + z
                // also add cost system
            }
        }
        else
        {
            Destroy(tempGO);    
        }

    }
    void SpawnPrefab(Vector3 spawnPosition)
    {
        Instantiate(itemToPlace, spawnPosition, Quaternion.identity);
    }
    public void SetNewItemToPlace(GameObject item)
    {
        itemToPlace = item;
    }
}
