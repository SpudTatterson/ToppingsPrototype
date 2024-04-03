using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuideLineShower : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float showRadius = 5f;

    List<GridGuideLine> oldGrids = new List<GridGuideLine>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(oldGrids != null)
        {
            foreach (GridGuideLine grid in oldGrids)
            {
                grid.DoNotShow();
            }
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            List<GridGuideLine> grids = ComponentUtility.GetComponentsInRadius<GridGuideLine>(hit.point, showRadius);
            oldGrids = grids;
            foreach (GridGuideLine grid in grids)
            {
                float distance = Vector3.Distance(hit.point, grid.GetCenter());
                distance = Mathf.InverseLerp(showRadius, 0, distance);
                grid.UpdateTrans(distance);
            }
        }
    }
}
