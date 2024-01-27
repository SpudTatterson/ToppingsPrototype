using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    // this code is disgusting 
    Camera cam;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject itemToPlace;
    
    GameObject tempGO;
    MeshRenderer mr;


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
            if (Input.GetButtonDown("Fire2"))
            {
                itemToPlace = null;
                Destroy(tempGO);
                return;
            }
            Vector3 hitPoint = hit.point;
            if (tempGO == null && itemToPlace != null)
            {
                tempGO = Instantiate(itemToPlace);// spawn visual aid if it doesn't exist
                mr = tempGO.GetComponentInChildren<MeshRenderer>(); // get mesh render for later
                tempGO.GetComponentInChildren<Collider>().enabled = false; // disable collider on tempGameObject so it wont interrupt placement
            }

            tempGO.transform.position = hitPoint;
            if (Input.GetButtonDown("Fire1"))
            {
                Destroy(tempGO); // destroy visual aid

                bool canPlace = CheckIfObjectFits(); // in real game this func should be on the 
                                                     // parent class of all placeable object so it can be customized for each
                if (canPlace)
                    SpawnPrefab(hitPoint); // spawn actual object
                // should probably add them to list or something to keep track of them
                // maybe add last placed and a control + z
                // also add cost system
            }
        }
        else
        {
            Destroy(tempGO);    // destroy temp if player isn't pointing at ground
        }

    }

    bool CheckIfObjectFits()
    {

        return !Physics.CheckBox(mr.bounds.center, mr.bounds.size / 2, tempGO.transform.rotation, ~groundLayer);
    }

    void SpawnPrefab(Vector3 spawnPosition)
    {
        Instantiate(itemToPlace, spawnPosition, Quaternion.identity);
    }
    public void SetNewItemToPlace(GameObject item)
    {
        itemToPlace = item;
    }
    void OnDrawGizmos()
    {
        if (tempGO == null) return;
        MeshRenderer mr = tempGO.GetComponentInChildren<MeshRenderer>();
        Gizmos.DrawWireCube(mr.bounds.center, mr.bounds.size);
    }
}
