using UnityEngine;
using NaughtyAttributes;

public class GridInfo : MonoBehaviour
{
    Bounds bounds;
    Renderer mr;
    Vector3 centerPoint;
    Vector3 rightSide, leftSide, top, bottom;
    Vector3[] points;

    [Tooltip("Control how close to the edge the point is (0.0 = center, 1.0 = edge)")]
    [SerializeField, Range(0, 1)] float lerpFactor = 0.75f;

    void Start()
    {
        mr = GetComponentInChildren<Renderer>();

        // Apply the GameObject's scale to the bounds extents
        bounds = mr.bounds;
        bounds.extents = Vector3.Scale(bounds.extents, transform.localScale);
        centerPoint = bounds.center + new Vector3(0, bounds.extents.y, 0);
    }

    public Vector3 GetRightSide()
    {
        if (rightSide == Vector3.zero)
            rightSide = Vector3.Lerp(centerPoint, centerPoint + new Vector3(0f, 0f, bounds.extents.z), lerpFactor);
        return rightSide;
    }

    public Vector3 GetLeftSide()
    {
        if (leftSide == Vector3.zero)
            leftSide = Vector3.Lerp(centerPoint, centerPoint - new Vector3(0f, 0f, bounds.extents.z), lerpFactor);
        return leftSide;
    }

    public Vector3 GetTop()
    {
        if (top == Vector3.zero)
            top = Vector3.Lerp(centerPoint, centerPoint + new Vector3(bounds.extents.x, 0f, 0f), lerpFactor);
        return top;
    }

    public Vector3 GetBottom()
    {
        if (bottom == Vector3.zero)
            bottom = Vector3.Lerp(centerPoint, centerPoint - new Vector3(bounds.extents.x, 0f, 0f), lerpFactor);
        return bottom;
    }

    public Vector3 GetCenter()
    {
        return centerPoint;
    }
    public Vector3 FindClosestPoint(Vector3 hitPosition)
    {
        if(points == null)
            points = new Vector3[] { GetCenter(), GetRightSide(), GetLeftSide(), GetTop(), GetBottom() };
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 closestPoint = Vector3.zero;

        foreach (Vector3 point in points)
        {
            float distanceSqr = (hitPosition - point).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestPoint = point;
            }
        }

        return closestPoint;
    }
    void InitAllVectors() //for gizmos
    {
        GetRightSide();
        GetLeftSide();
        GetTop();
        GetBottom();
    }
    void OnDrawGizmosSelected()
    {
        InitAllVectors();
        // Visualize the center point and the adjustable points for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerPoint, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rightSide, 0.1f);
        Gizmos.DrawSphere(leftSide, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(top, 0.1f);
        Gizmos.DrawSphere(bottom, 0.1f);
    }
}
