using UnityEngine;
using NaughtyAttributes;

public class GridInfo : MonoBehaviour
{
    Bounds bounds;
    Renderer mr;
    Vector3 centerPoint;
    Vector3 rightSide, leftSide, top, bottom;
    Vector3 topLeft, topRight, bottomLeft, bottomRight;
    Vector3[] points;

    [Tooltip("Control how close to the edge the point is (0.0 = center, 1.0 = edge)")]
    [SerializeField, Range(0, 1), EnableIf("includeSides")] float lerpFactor = 0.75f;

    [Tooltip("Use the corners in the grid(effectively turning it to a 9x9)")]
    [SerializeField, EnableIf("includeSides")] bool includeCorners = false;
    [Tooltip("Use the sides in the grid")]
    [SerializeField] bool includeSides = false;

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
    public Vector3 GetTopLeft()
    {
        if (topLeft == Vector3.zero)
            topLeft = centerPoint + new Vector3(-bounds.extents.x, 0f, bounds.extents.z) * lerpFactor;
        return topLeft;
    }

    public Vector3 GetTopRight()
    {
        if (topRight == Vector3.zero)
            topRight = centerPoint + new Vector3(bounds.extents.x, 0f, bounds.extents.z) * lerpFactor;
        return topRight;
    }

    public Vector3 GetBottomLeft()
    {
        if (bottomLeft == Vector3.zero)
            bottomLeft = centerPoint + new Vector3(-bounds.extents.x, 0f, -bounds.extents.z) * lerpFactor;
        return bottomLeft;
    }

    public Vector3 GetBottomRight()
    {
        if (bottomRight == Vector3.zero)
            bottomRight = centerPoint + new Vector3(bounds.extents.x, 0f, -bounds.extents.z) * lerpFactor;
        return bottomRight;
    }
    void UpdatePointsArray()
    {
        // This should be called after all individual points have been initialized.
        if (includeCorners && includeSides)
        {
            points = new Vector3[]
            {
                GetCenter(), GetRightSide(), GetLeftSide(), GetTop(), GetBottom(),
                GetTopLeft(), GetTopRight(), GetBottomLeft(), GetBottomRight()
            };
        }
        if (includeSides)
        {
            points = new Vector3[]
            {
                GetCenter(), GetRightSide(), GetLeftSide(), GetTop(), GetBottom()
            };
        }
        if(!includeCorners && !includeSides)
        {
            points = new Vector3[]
            {
                GetCenter()
            };
        }
    }
    public Vector3 FindClosestPoint(Vector3 hitPosition)
    {
        if (points == null)
            UpdatePointsArray();
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
    void InitAllVectors() // For gizmos and initialization
    {
        if (includeSides)
        {
            GetRightSide();
            GetLeftSide();
            GetTop();
            GetBottom();
        }
        if (includeCorners)
        {
            GetTopLeft();
            GetTopRight();
            GetBottomLeft();
            GetBottomRight();
        }
    }
    void OnDrawGizmosSelected()
    {
        InitAllVectors();
        UpdatePointsArray();
        // Visualize the center point and the adjustable points for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerPoint, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rightSide, 0.1f);
        Gizmos.DrawSphere(leftSide, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(top, 0.1f);
        Gizmos.DrawSphere(bottom, 0.1f);

        if (includeCorners)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(topLeft, 0.1f);
            Gizmos.DrawSphere(topRight, 0.1f);
            Gizmos.DrawSphere(bottomLeft, 0.1f);
            Gizmos.DrawSphere(bottomRight, 0.1f);
        }
    }
}
