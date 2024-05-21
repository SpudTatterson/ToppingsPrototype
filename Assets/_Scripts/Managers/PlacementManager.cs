using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager instance;
    // this code is disgusting 
    Camera cam;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask itemLayers;
    [SerializeField] GameObject itemToPlace;
    [SerializeField] Material unplacedMaterial;
    [SerializeField] Color canPlaceColor = Color.green;
    [SerializeField] Color cantPlaceColor = Color.red;
    [SerializeField] KeyCode destroyShortCut = KeyCode.LeftAlt;

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
        instance = this;
        cam = Camera.main;
        guideLine = GetComponent<GuideLineShower>();
    }
    void Update()
    {
        CheckForDestroyShortCut();

        if (isDestroying)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
                ResetAllVariables();
            LookForObjectsToDestroy();
            return;
        }
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, Mathf.Infinity, groundLayer) && itemToPlace != null)
        {
            Initialize();

            if (Input.GetButtonDown("Fire2")) // if pressed right click cancel everything
            {
                ResetAllVariables();
                return;
            }

            Vector3 placementPoint = HandlePlacementPoint(hit);

            if (Input.GetKeyDown(KeyCode.R))
            {
                tempGO.transform.Rotate(Vector3.up * 90);
            }

            tempGO.transform.position = placementPoint;
            if (isPlacingSecondaryObject && CanPlaceSecondaryObject())
                itemToPlace.transform.position = tempGO.transform.position;
            if (isPlacingSecondaryObject && !CanPlaceSecondaryObject())
                tempGO.transform.position = itemToPlace.transform.position;

            bool canPlace = CheckIfObjectFits();

            if (mr)
                mr.material.color = canPlace ? canPlaceColor : cantPlaceColor; // visually show if player can or cant place object

            if (Input.GetButtonDown("Fire1"))
            {
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
                        isPlacingSecondaryObject = false;
                        ResetAllVariables();
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

    Vector3 HandlePlacementPoint(RaycastHit hit)
    {
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
        return placementPoint;
    }

    void Initialize()
    {
        if (tempGO == null && itemToPlace != null)
        {

            Texture2D placeCursor = UIManager.instance.placeCursor;
            Cursor.SetCursor(placeCursor, Vector2.zero, CursorMode.Auto); //new Vector2(placeCursor.width / 2, placeCursor.height)

            HandleTempGO();

            if (tempGO.TryGetComponent<Placeable>(out Placeable placeable))
                placeable.enabled = false; // disable the placeable component in the temp game object so it wont interact with the world

            HandleGridGuideLines(placeable);
        }
        if (heldPlaceable == null)
            heldPlaceable = itemToPlace.GetComponent<Placeable>();
        if (isPlacingSecondaryObject)
            heldPlaceable = lastPlaced;

    }

    void HandleTempGO()
    {
        tempGO = Instantiate(itemToPlace);// spawn visual aid if it doesn't exist
        mr = tempGO.GetComponentInChildren<MeshRenderer>(); // get mesh render for later

        Collider[] colliders = tempGO.GetComponentsInChildren<Collider>(); // get all colliders
        foreach (Collider c in colliders) // disable collider on tempGameObject so it wont interrupt placement
        {
            c.enabled = false;
        }
        if (unplacedMaterial != null) // set temp gameObject mats to the unplaced mat
        {
            MeshRenderer[] mrs = tempGO.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in mrs)
            {
                mr.material = unplacedMaterial;
            }
        }
    }

    void HandleGridGuideLines(Placeable placeable)
    {
        guideLine.toggle = true;
        if (placeable)
        {
            guideLine.SetShowShape(placeable.radiusShape);
            if (placeable.radiusShape == GridGuideLineShape.Sphere)
                guideLine.SetShowRadius(placeable.GetShowRadius());
            if (placeable.radiusShape == GridGuideLineShape.Box)
                guideLine.SetHalfExtents(placeable.GetHalfExtents());
        }
        else
            guideLine.SetShowRadius(3f);
    }

    void CheckForDestroyShortCut()
    {
        if (Input.GetKeyDown(destroyShortCut))
            TurnOnObjectDestruction();
        if (Input.GetKeyUp(destroyShortCut))
            ResetAllVariables();
    }

    public void ResetAllVariables()
    {
        isDestroying = false;
        itemToPlace = null;
        heldPlaceable = null;
        Destroy(tempGO);
        if (isPlacingSecondaryObject) Destroy(lastPlaced.gameObject);
        isPlacingSecondaryObject = false;
        guideLine.toggle = false;
        Cursor.SetCursor(UIManager.instance.defaultCursor, Vector2.zero, CursorMode.Auto);
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, itemLayers) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject selectedGameObject = hit.collider.gameObject.GetComponentInParent<Placeable>().gameObject;
            if (IsPlaced(selectedGameObject))
            {
                Destroy(selectedGameObject);
                //isDestroying = false;
            }
        }
    }
    public void TurnOnObjectDestruction()
    {
        ResetAllVariables();
        isDestroying = true;
        Cursor.SetCursor(UIManager.instance.deleteCursor, Vector2.zero, CursorMode.Auto);
    }
    GameObject SpawnPrefab(Vector3 spawnPosition, quaternion rotation)
    {
        return Instantiate(itemToPlace, spawnPosition, rotation);
    }
    public void SetNewItemToPlace(GameObject item)
    {
        ResetAllVariables();
        itemToPlace = item;
    }
    void OnDrawGizmos()
    {
        if (tempGO == null) return;
        // MeshRenderer mr = tempGO.GetComponentInChildren<MeshRenderer>();
        // Gizmos.DrawWireCube(mr.bounds.center, mr.bounds.size);
    }
}
