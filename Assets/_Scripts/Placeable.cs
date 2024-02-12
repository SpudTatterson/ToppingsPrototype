using UnityEngine;

public class Placeable : MonoBehaviour
{
    public GameObject secondaryPlacable;
    public bool hasSecondaryPlacement = false;
    public float maxSecondaryObjectDistance = 5;
    public virtual void SecondaryPlacement()
    {

    }
}
