using UnityEngine;
using NaughtyAttributes;

public class Placeable : MonoBehaviour
{
    public bool lockToGrid = true;
    public bool lockToCenter = true;
    public bool hasSecondaryPlacement = false;
    [ShowIf("hasSecondaryPlacement")] public GameObject secondaryPlacable;
    [ShowIf("hasSecondaryPlacement")] public float maxSecondaryObjectDistance = 5;
    public virtual void SecondaryPlacement()
    {

    }
}
