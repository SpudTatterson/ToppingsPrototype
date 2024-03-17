using UnityEngine;
using NaughtyAttributes;

public class Placeable : MonoBehaviour
{
    [ShowIf("hasSecondaryPlacement")]public GameObject secondaryPlacable;
    public bool hasSecondaryPlacement = false;
    [ShowIf("hasSecondaryPlacement")]public float maxSecondaryObjectDistance = 5;
    public virtual void SecondaryPlacement()
    {

    }
}
