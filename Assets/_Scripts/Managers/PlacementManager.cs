using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    // this code is disgusting 
    Camera cam;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject itemToPlace;
    [SerializeField] Material unplacedMaterial;

    GameObject tempGO;
    MeshRenderer mr;
    bool isPlacingSecondaryObject = false;
    bool isDestroying = false;
    Placeable heldPlaceable;
    Placeable lastPlaced;
    List<GameObject> placedObjects = new List<GameObject>();
    GuideLineShower guideLine;


    void Awake()
    {
        cam = Camera.main;
        guideLine = GetComponent<GuideLineShower>();
    }
    void Update()
    {
        if (isDestroying)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
                isDestroying = false;
            LookForObjectsToDestroy();
            return;
        }
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, Mathf.Infinity, groundLayer) && itemToPlace != null)
        {
            if (tempGO == null && itemToPlace != null)// initialization 
            {
                guideLine.toggle = true;
                tempGO = Instantiate(itemToPlace);// spawn visual aid if it doesn't exist
                mr = tempGO.GetComponentInChildren<MeshRenderer>(); // get mesh render for later

                Collider[] colliders = tempGO.GetComponentsInChildren<Collider>(); // get all colliders
                foreach (Collider c in colliders) // disable collider on tempGameObject so it wont interrupt placement
                {
                    c.enabled = false;
                }
                if (tempGO.TryGetComponent<Placeable>(out Placeable placeable))
                {
                    placeable.enabled = false;
                }


                if (unplacedMaterial != null)
                {
                    MeshRenderer[] mrs = tempGO.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mr in mrs)
                    {
                        mr.material = unplacedMaterial;
                    }
                }
            }

            if (heldPlaceable == null)
                heldPlaceable = itemToPlace.GetComponent<Placeable>();
            if (isPlacingSecondaryObject)
                heldPlaceable = lastPlaced;

            if (Input.GetButtonDown("Fire2")) // if pressed right click cancel everything
            {
                ClearCurrentVars();
                return;
            }
            Vector3 placementPoint;
            if (heldPlaceable.lockToGrid)
            {
                GridInfo currentGrid = hit.collider.GetComponentInParent<GridInfo>();
                if (heldPlaceable.lockToCenter)
                    placementPoint = currentGrid.GetCenter();
                else
                    placementPoint = currentGrid.FindClosestPoint(hit.point);
            }
            else
                placementPoint = hit.point;

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
            bool canPlace = CheckIfObjectFits();
            unplacedMaterial.color = canPlace ? Color.green: Color.red;
            if (Input.GetButtonDown("Fire1"))
            {
                 // in real game this func should be on the 
                                                     // parent class of all placeable object so it can be customized for each
                if (canPlace)
                {
                    GameObject actualGO = SpawnPrefab(tempGO.transform.position, tempGO.transform.rotation); // spawn actual object
                    Placeable placeable = actualGO.GetComponent<Placeable>();
                    lastPlaced = placeable;
                    placedObjects.Add(actualGO);

                    if (isPlacingSecondaryObject)
                    {
                        itemToPlace.GetComponentInParent<Placeable>().FullyPlaced = true;
                        itemToPlace.transform.position = actualGO.transform.position;
                        Destroy(actualGO);
                        itemToPlace = null;
                        heldPlaceable = null;
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
    void ClearCurrentVars()
    {
        itemToPlace = null;
        heldPlaceable = null;
        Destroy(tempGO);
        if (isPlacingSecondaryObject) Destroy(lastPlaced.gameObject);
        isPlacingSecondaryObject = false;
        guideLine.toggle = false;
    }
    bool CheckIfObjectFits()
    {
        if (mr == null) return true;
        return !Physics.CheckBox(mr.bounds.center, mr.bounds.size / 2, mr.transform.localRotation, ~groundLayer, QueryTriggerInteraction.Ignore);
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
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject selectedGameObject = hit.collider.gameObject.GetComponentInParent<Placeable>().gameObject;
            if (IsPlaced(selectedGameObject))
            {
                Destroy(selectedGameObject);
                isDestroying = false;
            }
        }
    }
    public void TurnOnObjectDestruction()
    {
        ClearCurrentVars();
        isDestroying = true;
    }
    GameObject SpawnPrefab(Vector3 spawnPosition, quaternion rotation)
    {
        return Instantiate(itemToPlace, spawnPosition, rotation);
    }
    public void SetNewItemToPlace(GameObject item)
    {
        ClearCurrentVars();
        itemToPlace = item;
    }
    void OnDrawGizmos()
    {
        if (tempGO == null) return;
        // MeshRenderer mr = tempGO.GetComponentInChildren<MeshRenderer>();
        // Gizmos.DrawWireCube(mr.bounds.center, mr.bounds.size);
    }
}
