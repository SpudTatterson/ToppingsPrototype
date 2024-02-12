using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    // this code is disgusting 
    Camera cam;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject itemToPlace;

    GameObject tempGO;
    MeshRenderer mr;
    bool isPlacingSecondaryObject = false;
    Placeable lastPlaced;


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
            if (Input.GetButtonDown("Fire2")) // if pressed right click cancel everything
            {
                itemToPlace = null;
                Destroy(tempGO);
                if (isPlacingSecondaryObject) Destroy(lastPlaced.gameObject);
                return;
            }

            Vector3 placementPoint = hit.point;
            if (tempGO == null && itemToPlace != null)// initialization 
            {
                tempGO = Instantiate(itemToPlace);// spawn visual aid if it doesn't exist
                mr = tempGO.GetComponentInChildren<MeshRenderer>(); // get mesh render for later
                tempGO.GetComponentInChildren<Collider>().enabled = false; // disable collider on tempGameObject so it wont interrupt placement
            }

            tempGO.transform.position = placementPoint;
            if (isPlacingSecondaryObject && CanPlaceSecondaryObject())
            {
                itemToPlace.transform.position = tempGO.transform.position;
            }
            if(isPlacingSecondaryObject  && !CanPlaceSecondaryObject())
            {
                tempGO.transform.position = itemToPlace.transform.position;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                bool canPlace = CheckIfObjectFits(); // in real game this func should be on the 
                                                     // parent class of all placeable object so it can be customized for each
                if (canPlace)
                {

                    GameObject actualGO = SpawnPrefab(tempGO.transform.position); // spawn actual object
                    Placeable placeable = actualGO.GetComponent<Placeable>();
                    lastPlaced = placeable;

                    if (isPlacingSecondaryObject)
                    {
                        itemToPlace.transform.position = actualGO.transform.position;
                        Destroy(actualGO);
                        itemToPlace = null;
                        isPlacingSecondaryObject = false;
                    }
                    if (placeable != null && placeable.hasSecondaryPlacement)
                    {
                        placeable.SecondaryPlacement();
                        isPlacingSecondaryObject = true;
                    }
                    Destroy(tempGO); // destroy visual aid
                }
                else
                    Debug.Log("Not enough space");

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
        if (mr == null) return true;
        return !Physics.CheckBox(mr.bounds.center, mr.bounds.size / 2, tempGO.transform.rotation, ~groundLayer);
    }
    bool CanPlaceSecondaryObject()
    {
        return Vector3.Distance(lastPlaced.transform.position, tempGO.transform.position) < lastPlaced.maxSecondaryObjectDistance;
    }

    GameObject SpawnPrefab(Vector3 spawnPosition)
    {
        return Instantiate(itemToPlace, spawnPosition, Quaternion.identity);
    }
    public void SetNewItemToPlace(GameObject item)
    {
        itemToPlace = item;
    }
    void OnDrawGizmos()
    {
        if (tempGO == null) return;
        // MeshRenderer mr = tempGO.GetComponentInChildren<MeshRenderer>();
        // Gizmos.DrawWireCube(mr.bounds.center, mr.bounds.size);
    }
}
