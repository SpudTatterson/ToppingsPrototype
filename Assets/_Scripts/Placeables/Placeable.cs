using UnityEngine;
using NaughtyAttributes;
public enum ShowRadiusShape
{
    Sphere,
    Box
}
public class Placeable : MonoBehaviour
{
    public bool lockToGrid = true;
    public bool lockToCenter = true;
    public bool hasSecondaryPlacement = false;
    [SerializeField] float showRadius = 3f;
    [SerializeField] Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
    [HideInInspector] public bool FullyPlaced = false;
    [ShowIf("hasSecondaryPlacement")] public GameObject secondaryPlacable;
    [ShowIf("hasSecondaryPlacement")] public float maxSecondaryObjectDistance = 5;

    public ShowRadiusShape radiusShape;

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
        if (radiusShape == ShowRadiusShape.Box)
        {
            Gizmos.DrawWireCube(transform.position, halfExtents * 2);
        }
        if(radiusShape == ShowRadiusShape.Sphere)
        {
            Gizmos.DrawWireSphere(transform.position, showRadius);
        }
    }
}
