using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuideLineShower : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float showRadius = 5f;
    Vector3 halfExtents;

    List<GridGuideLine> oldGrids = new List<GridGuideLine>();

    public bool toggle = false;
    GridGuideLineShape shape;

    // Update is called once per frame
    void Update()
    {
        if (oldGrids != null || oldGrids.Count != 0)
        {
            foreach (GridGuideLine grid in oldGrids)
            {
                grid.DoNotShow();
            }
        }
        if (!toggle)
        {
            oldGrids.Clear();
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            List<GridGuideLine> grids = new List<GridGuideLine>();
            Vector3 point = hit.transform.GetComponentInParent<GridInfo>().GetCenter();

            if (shape == GridGuideLineShape.Sphere)
                grids = ComponentUtility.GetComponentsInRadius<GridGuideLine>(point, showRadius);
            else if (shape == GridGuideLineShape.Box)
            {
                grids = ComponentUtility.GetComponentsInBox<GridGuideLine>(point, halfExtents);
            }
            oldGrids = grids;
            foreach (GridGuideLine grid in grids)
            {
                float distance = Vector3.Distance(point, grid.GetCenter());
                float maxDistance = (shape ==  GridGuideLineShape.Box) ? halfExtents.magnitude : showRadius; 
                distance = Mathf.InverseLerp(maxDistance , 0, distance);
                grid.UpdateTrans(distance);
            }
        }
    }

    public void SetShowRadius(float radius)
    {
        showRadius = radius;
        halfExtents = Vector3.zero;
    }
    public void SetShowShape(GridGuideLineShape shape)
    {
        this.shape = shape;
    }
    public void SetHalfExtents(Vector3 halfExtents)
    {
        showRadius = 0;
        this.halfExtents = halfExtents;
    }

}
