using System.Collections.Generic;
using Unity.Mathematics;
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
    bool isDestroying = false;
    Placeable lastPlaced;
    List<GameObject> placedObjects = new List<GameObject>();


    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (isDestroying)
        {
            if(Input.GetKeyDown(KeyCode.Mouse1))
                isDestroying = false;
            LookForObjectsToDestroy();
            return;
        }
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
            if (Input.GetKeyDown(KeyCode.R))
            {
                tempGO.transform.Rotate(Vector3.up * 90);
            }
            tempGO.transform.position = placementPoint;
            if (isPlacingSecondaryObject && CanPlaceSecondaryObject())
            {
                itemToPlace.transform.position = tempGO.transform.position;
            }
            if (isPlacingSecondaryObject && !CanPlaceSecondaryObject())
            {
                tempGO.transform.position = itemToPlace.transform.position;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                bool canPlace = CheckIfObjectFits(); // in real game this func should be on the 
                                                     // parent class of all placeable object so it can be customized for each
                if (canPlace)
                {

                    GameObject actualGO = SpawnPrefab(tempGO.transform.position, tempGO.transform.rotation); // spawn actual object
                    Placeable placeable = actualGO.GetComponent<Placeable>();
                    lastPlaced = placeable;
                    placedObjects.Add(actualGO);

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
        return !Physics.CheckBox(mr.bounds.center, mr.bounds.size / 2, tempGO.transform.rotation, ~groundLayer, QueryTriggerInteraction.Ignore);
    }
    bool CanPlaceSecondaryObject()
    {
        return Vector3.Distance(lastPlaced.transform.position, tempGO.transform.position) < lastPlaced.maxSecondaryObjectDistance;
    }
    bool IsPlaced(GameObject gameObjectToDestroy)
    {
        return placedObjects.Contains(gameObjectToDestroy);

    }
    void LookForObjectsToDestroy()
    {
        Debug.Log("test");
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject selectedGameObject = hit.collider.gameObject;
            if (IsPlaced(selectedGameObject))
            {
                Destroy(selectedGameObject);
                isDestroying = false;
            }
        }
    }
    public void TurnOnObjectDestruction()
    {
        isDestroying = true;
    }
    GameObject SpawnPrefab(Vector3 spawnPosition, quaternion rotation)
    {
        return Instantiate(itemToPlace, spawnPosition, rotation);
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
