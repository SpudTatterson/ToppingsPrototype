using UnityEngine;
using NaughtyAttributes;
public class Placeable : MonoBehaviour
{
    [Header("Placement Settings")]
    public bool lockToGrid = true;
    public bool lockToCenter = true;
    public bool hasSecondaryPlacement = false;
    [HideInInspector] public bool FullyPlaced = false;
    [ShowIf("hasSecondaryPlacement")] public GameObject secondaryPlacable;
    [ShowIf("hasSecondaryPlacement")] public float maxSecondaryObjectDistance = 5;

    [Header("Grid Guide Line Settings")]
    public GridGuideLineShape radiusShape;

    [SerializeField, HideIf("radiusShape", GridGuideLineShape.Box)] float showRadius = 3f;
    [SerializeField, HideIf("radiusShape", GridGuideLineShape.Sphere)] Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);

    public virtual void SecondaryPlacement()
    {

    }
    public float GetShowRadius()
    {
        return showRadius;
    }
    public Vector3 GetHalfExtents()
    {
        return halfExtents;
    }

    void OnDrawGizmosSelected()
    {
        if (radiusShape == GridGuideLineShape.Box)
        {
            Gizmos.DrawWireCube(transform.position, halfExtents * 2);
        }
        if(radiusShape == GridGuideLineShape.Sphere)
        {
            Gizmos.DrawWireSphere(transform.position, showRadius);
        }
    }
}
